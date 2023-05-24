using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200150E RID: 5390
public class KickableObject : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x06007AF3 RID: 31475 RVA: 0x003144D8 File Offset: 0x003126D8
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnPlayerCollision));
	}

	// Token: 0x06007AF4 RID: 31476 RVA: 0x00314504 File Offset: 0x00312704
	public void Update()
	{
		if (this.m_shouldDisplayOutline)
		{
			int num;
			DungeonData.Direction inverseDirection = DungeonData.GetInverseDirection(DungeonData.GetDirectionFromIntVector2(this.GetFlipDirection(this.m_lastInteractingPlayer.specRigidbody, out num)));
			if (inverseDirection != this.m_lastOutlineDirection || base.sprite.spriteId != this.m_lastSpriteId)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
				SpriteOutlineManager.AddSingleOutlineToSprite<tk2dSprite>(base.sprite, DungeonData.GetIntVector2FromDirection(inverseDirection), Color.white, 0.25f, 0f);
			}
			this.m_lastOutlineDirection = inverseDirection;
			this.m_lastSpriteId = base.sprite.spriteId;
		}
		if (this.leavesGoopTrail && base.specRigidbody.Velocity.magnitude > 0.1f)
		{
			this.m_goopElapsed += BraveTime.DeltaTime;
			if (this.m_goopElapsed > this.goopFrequency)
			{
				this.m_goopElapsed -= BraveTime.DeltaTime;
				if (this.m_goopManager == null)
				{
					this.m_goopManager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopType);
				}
				this.m_goopManager.AddGoopCircle(base.sprite.WorldCenter, this.goopRadius + 0.1f, -1, false, -1);
			}
			if (this.AllowTopWallTraversal && GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(base.sprite.WorldCenter.ToIntVector2(VectorConversions.Floor)) && GameManager.Instance.Dungeon.data[base.sprite.WorldCenter.ToIntVector2(VectorConversions.Floor)].IsFireplaceCell)
			{
				MinorBreakable component = base.GetComponent<MinorBreakable>();
				if (component && !component.IsBroken)
				{
					component.Break(Vector2.zero);
					GameStatsManager.Instance.SetFlag(GungeonFlags.FLAG_ROLLED_BARREL_INTO_FIREPLACE, true);
				}
			}
		}
	}

	// Token: 0x06007AF5 RID: 31477 RVA: 0x003146E0 File Offset: 0x003128E0
	public void ForceDeregister()
	{
		if (this.m_room != null)
		{
			this.m_room.DeregisterInteractable(this);
		}
		RoomHandler.unassignedInteractableObjects.Remove(this);
	}

	// Token: 0x06007AF6 RID: 31478 RVA: 0x00314708 File Offset: 0x00312908
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06007AF7 RID: 31479 RVA: 0x00314710 File Offset: 0x00312910
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		Vector2 vector = interactor.CenterPosition - base.specRigidbody.UnitCenter;
		switch (BraveMathCollege.VectorToQuadrant(vector))
		{
		case 0:
			return "tablekick_down";
		case 1:
			shouldBeFlipped = true;
			return "tablekick_right";
		case 2:
			return "tablekick_up";
		case 3:
			return "tablekick_right";
		default:
			Debug.Log("fail");
			return "tablekick_up";
		}
	}

	// Token: 0x06007AF8 RID: 31480 RVA: 0x00314788 File Offset: 0x00312988
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		this.m_lastInteractingPlayer = interactor;
		this.m_shouldDisplayOutline = true;
	}

	// Token: 0x06007AF9 RID: 31481 RVA: 0x003147A4 File Offset: 0x003129A4
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		this.ClearOutlines();
		this.m_shouldDisplayOutline = false;
	}

	// Token: 0x06007AFA RID: 31482 RVA: 0x003147C0 File Offset: 0x003129C0
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.sprite.transform.position, bounds.max + base.sprite.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06007AFB RID: 31483 RVA: 0x003148AC File Offset: 0x00312AAC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06007AFC RID: 31484 RVA: 0x003148B4 File Offset: 0x00312AB4
	public void Interact(PlayerController player)
	{
		GameManager.Instance.Dungeon.GetRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round)).DeregisterInteractable(this);
		RoomHandler.unassignedInteractableObjects.Remove(this);
		this.Kick(player.specRigidbody);
		AkSoundEngine.PostEvent("Play_OBJ_table_flip_01", player.gameObject);
		this.ClearOutlines();
		this.m_shouldDisplayOutline = false;
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerRolledBarrel");
		}
	}

	// Token: 0x06007AFD RID: 31485 RVA: 0x00314938 File Offset: 0x00312B38
	private void NoPits(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		Func<IntVector2, bool> func = delegate(IntVector2 pixel)
		{
			Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
			if (!GameManager.Instance.Dungeon.CellSupportsFalling(vector))
			{
				return false;
			}
			List<SpeculativeRigidbody> platformsAt = GameManager.Instance.Dungeon.GetPlatformsAt(vector);
			if (platformsAt != null)
			{
				for (int i = 0; i < platformsAt.Count; i++)
				{
					if (platformsAt[i].PrimaryPixelCollider.ContainsPixel(pixel))
					{
						return false;
					}
				}
			}
			return true;
		};
		PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
		if (primaryPixelCollider != null)
		{
			IntVector2 intVector = pixelOffset - prevPixelOffset;
			if (intVector == IntVector2.Down && func(primaryPixelCollider.LowerLeft + pixelOffset) && func(primaryPixelCollider.LowerRight + pixelOffset) && (!func(primaryPixelCollider.UpperRight + prevPixelOffset) || !func(primaryPixelCollider.UpperLeft + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Right && func(primaryPixelCollider.LowerRight + pixelOffset) && func(primaryPixelCollider.UpperRight + pixelOffset) && (!func(primaryPixelCollider.UpperLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerLeft + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Up && func(primaryPixelCollider.UpperRight + pixelOffset) && func(primaryPixelCollider.UpperLeft + pixelOffset) && (!func(primaryPixelCollider.LowerLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerRight + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Left && func(primaryPixelCollider.UpperLeft + pixelOffset) && func(primaryPixelCollider.LowerLeft + pixelOffset) && (!func(primaryPixelCollider.LowerRight + prevPixelOffset) || !func(primaryPixelCollider.UpperRight + prevPixelOffset)))
			{
				validLocation = false;
			}
		}
		if (!validLocation)
		{
			this.StopRolling(true);
		}
	}

	// Token: 0x06007AFE RID: 31486 RVA: 0x00314B60 File Offset: 0x00312D60
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x06007AFF RID: 31487 RVA: 0x00314B6C File Offset: 0x00312D6C
	private void OnPlayerCollision(CollisionData rigidbodyCollision)
	{
		PlayerController component = rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>();
		if (this.RollingDestroysSafely && component != null && component.IsDodgeRolling)
		{
			MinorBreakable component2 = base.GetComponent<MinorBreakable>();
			component2.destroyOnBreak = true;
			component2.makeParallelOnBreak = false;
			component2.breakAnimName = this.RollingBreakAnim;
			component2.explodesOnBreak = false;
			component2.Break(-rigidbodyCollision.Normal);
		}
	}

	// Token: 0x06007B00 RID: 31488 RVA: 0x00314BE0 File Offset: 0x00312DE0
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		MinorBreakable component = otherRigidbody.GetComponent<MinorBreakable>();
		if (component && !component.onlyVulnerableToGunfire && !component.IsBig)
		{
			component.Break(base.specRigidbody.Velocity);
			PhysicsEngine.SkipCollision = true;
		}
		if (otherRigidbody && otherRigidbody.aiActor && !otherRigidbody.aiActor.IsNormalEnemy)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06007B01 RID: 31489 RVA: 0x00314C60 File Offset: 0x00312E60
	private void OnCollision(CollisionData collision)
	{
		if (collision.collisionType == CollisionData.CollisionType.Rigidbody && collision.OtherRigidbody.projectile != null)
		{
			return;
		}
		if (((BraveMathCollege.ActualSign(base.specRigidbody.Velocity.x) != 0f && Mathf.Sign(collision.Normal.x) != Mathf.Sign(base.specRigidbody.Velocity.x)) || (BraveMathCollege.ActualSign(base.specRigidbody.Velocity.y) != 0f && Mathf.Sign(collision.Normal.y) != Mathf.Sign(base.specRigidbody.Velocity.y))) && ((BraveMathCollege.ActualSign(base.specRigidbody.Velocity.x) != 0f && Mathf.Sign(collision.Contact.x - base.specRigidbody.UnitCenter.x) == Mathf.Sign(base.specRigidbody.Velocity.x)) || (BraveMathCollege.ActualSign(base.specRigidbody.Velocity.y) != 0f && Mathf.Sign(collision.Contact.y - base.specRigidbody.UnitCenter.y) == Mathf.Sign(base.specRigidbody.Velocity.y))))
		{
			this.StopRolling(collision.collisionType == CollisionData.CollisionType.TileMap);
		}
	}

	// Token: 0x06007B02 RID: 31490 RVA: 0x00314DEC File Offset: 0x00312FEC
	private bool IsRollAnimation()
	{
		for (int i = 0; i < this.rollAnimations.Length; i++)
		{
			if (base.spriteAnimator.CurrentClip.name == this.rollAnimations[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007B03 RID: 31491 RVA: 0x00314E38 File Offset: 0x00313038
	private void StopRolling(bool bounceBack)
	{
		if (bounceBack && !this.m_isBouncingBack)
		{
			base.StartCoroutine(this.HandleBounceback());
		}
		else
		{
			base.spriteAnimator.Stop();
			if (this.IsRollAnimation())
			{
				tk2dSpriteAnimationClip currentClip = base.spriteAnimator.CurrentClip;
				base.spriteAnimator.Stop();
				base.spriteAnimator.Sprite.SetSprite(currentClip.frames[currentClip.frames.Length - 1].spriteId);
			}
			base.specRigidbody.Velocity = Vector2.zero;
			MinorBreakable component = base.GetComponent<MinorBreakable>();
			if (component != null)
			{
				component.onlyVulnerableToGunfire = false;
			}
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.NoPits));
			RoomHandler.unassignedInteractableObjects.Add(this);
			this.m_isBouncingBack = false;
		}
	}

	// Token: 0x06007B04 RID: 31492 RVA: 0x00314F48 File Offset: 0x00313148
	private IEnumerator HandleBounceback()
	{
		if (this.m_lastDirectionKicked != null)
		{
			this.m_isBouncingBack = true;
			Vector2 dirToMove = this.m_lastDirectionKicked.Value.ToVector2().normalized * -1f;
			int quadrant = BraveMathCollege.VectorToQuadrant(dirToMove);
			base.specRigidbody.Velocity = this.rollSpeed * dirToMove;
			IntVector2? lastDirectionKicked = this.m_lastDirectionKicked;
			this.m_lastDirectionKicked = ((lastDirectionKicked == null) ? null : new IntVector2?(lastDirectionKicked.GetValueOrDefault() * -1));
			tk2dSpriteAnimationClip rollClip = base.spriteAnimator.GetClipByName(this.rollAnimations[quadrant]);
			if (rollClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
			{
				base.spriteAnimator.PlayFromFrame(rollClip, rollClip.loopStart);
			}
			else
			{
				base.spriteAnimator.Play(rollClip);
			}
			float ela = 0f;
			float dura = 1.5f / base.specRigidbody.Velocity.magnitude;
			while (ela < dura && this.m_isBouncingBack)
			{
				ela += BraveTime.DeltaTime;
				base.specRigidbody.Velocity = this.rollSpeed * dirToMove;
				yield return null;
			}
			if (this.m_isBouncingBack)
			{
				this.StopRolling(false);
			}
		}
		yield break;
	}

	// Token: 0x06007B05 RID: 31493 RVA: 0x00314F64 File Offset: 0x00313164
	private void ClearOutlines()
	{
		this.m_lastOutlineDirection = (DungeonData.Direction)(-1);
		this.m_lastSpriteId = -1;
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x06007B06 RID: 31494 RVA: 0x00314F80 File Offset: 0x00313180
	private IEnumerator HandleBreakTimer()
	{
		this.m_timerIsActive = true;
		if (this.timerVFX != null)
		{
			this.timerVFX.SetActive(true);
		}
		yield return new WaitForSeconds(this.breakTimerLength);
		base.minorBreakable.Break();
		yield break;
	}

	// Token: 0x06007B07 RID: 31495 RVA: 0x00314F9C File Offset: 0x0031319C
	private void RemoveFromRoomHierarchy()
	{
		Transform hierarchyParent = base.transform.position.GetAbsoluteRoom().hierarchyParent;
		Transform transform = base.transform;
		while (transform.parent != null)
		{
			if (transform.parent == hierarchyParent)
			{
				transform.parent = null;
				break;
			}
			transform = transform.parent;
		}
	}

	// Token: 0x06007B08 RID: 31496 RVA: 0x00315000 File Offset: 0x00313200
	private void Kick(SpeculativeRigidbody kickerRigidbody)
	{
		if (base.specRigidbody && !base.specRigidbody.enabled)
		{
			return;
		}
		this.RemoveFromRoomHierarchy();
		List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody.PrimaryPixelCollider, null, false);
		for (int i = 0; i < overlappingRigidbodies.Count; i++)
		{
			if (overlappingRigidbodies[i] && overlappingRigidbodies[i].minorBreakable && !overlappingRigidbodies[i].minorBreakable.IsBroken && !overlappingRigidbodies[i].minorBreakable.onlyVulnerableToGunfire && !overlappingRigidbodies[i].minorBreakable.OnlyBrokenByCode)
			{
				overlappingRigidbodies[i].minorBreakable.Break();
			}
		}
		int num = ~CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox);
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, new int?(num), false);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.NoPits));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody2.OnCollision, new Action<CollisionData>(this.OnCollision));
		SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
		specRigidbody3.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody3.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		int num2;
		IntVector2 flipDirection = this.GetFlipDirection(kickerRigidbody, out num2);
		if (this.AllowTopWallTraversal)
		{
			base.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyBlocker));
		}
		base.specRigidbody.Velocity = this.rollSpeed * flipDirection.ToVector2();
		tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.rollAnimations[num2]);
		bool flag = false;
		if (this.m_lastDirectionKicked != null)
		{
			if (this.m_lastDirectionKicked.Value.y == 0 && flipDirection.y == 0)
			{
				flag = true;
			}
			if (this.m_lastDirectionKicked.Value.x == 0 && flipDirection.x == 0)
			{
				flag = true;
			}
		}
		if (clipByName != null && clipByName.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection && flag)
		{
			base.spriteAnimator.PlayFromFrame(clipByName, clipByName.loopStart);
		}
		else
		{
			base.spriteAnimator.Play(clipByName);
		}
		if (this.triggersBreakTimer && !this.m_timerIsActive)
		{
			base.StartCoroutine(this.HandleBreakTimer());
		}
		MinorBreakable component = base.GetComponent<MinorBreakable>();
		if (component != null)
		{
			component.breakAnimName = this.impactAnimations[num2];
			component.onlyVulnerableToGunfire = true;
		}
		IntVector2 intVector = base.transform.PositionVector2().ToIntVector2(VectorConversions.Round);
		GameManager.Instance.Dungeon.data[intVector].isOccupied = false;
		this.m_lastDirectionKicked = new IntVector2?(flipDirection);
	}

	// Token: 0x06007B09 RID: 31497 RVA: 0x00315308 File Offset: 0x00313508
	public IntVector2 GetFlipDirection(SpeculativeRigidbody kickerRigidbody, out int quadrant)
	{
		Vector2 vector = base.specRigidbody.UnitCenter - kickerRigidbody.UnitCenter;
		quadrant = BraveMathCollege.VectorToQuadrant(vector);
		return IntVector2.Cardinals[quadrant];
	}

	// Token: 0x04007D6B RID: 32107
	public float rollSpeed = 3f;

	// Token: 0x04007D6C RID: 32108
	[CheckAnimation(null)]
	public string[] rollAnimations;

	// Token: 0x04007D6D RID: 32109
	[CheckAnimation(null)]
	public string[] impactAnimations;

	// Token: 0x04007D6E RID: 32110
	public bool leavesGoopTrail;

	// Token: 0x04007D6F RID: 32111
	[ShowInInspectorIf("leavesGoopTrail", false)]
	public GoopDefinition goopType;

	// Token: 0x04007D70 RID: 32112
	[ShowInInspectorIf("leavesGoopTrail", false)]
	public float goopFrequency = 0.05f;

	// Token: 0x04007D71 RID: 32113
	[ShowInInspectorIf("leavesGoopTrail", false)]
	public float goopRadius = 1f;

	// Token: 0x04007D72 RID: 32114
	public bool triggersBreakTimer;

	// Token: 0x04007D73 RID: 32115
	[ShowInInspectorIf("triggersBreakTimer", false)]
	public float breakTimerLength = 3f;

	// Token: 0x04007D74 RID: 32116
	[ShowInInspectorIf("triggersBreakTimer", false)]
	public GameObject timerVFX;

	// Token: 0x04007D75 RID: 32117
	public bool RollingDestroysSafely = true;

	// Token: 0x04007D76 RID: 32118
	public string RollingBreakAnim = "red_barrel_break";

	// Token: 0x04007D77 RID: 32119
	private float m_goopElapsed;

	// Token: 0x04007D78 RID: 32120
	private DeadlyDeadlyGoopManager m_goopManager;

	// Token: 0x04007D79 RID: 32121
	private RoomHandler m_room;

	// Token: 0x04007D7A RID: 32122
	private bool m_isBouncingBack;

	// Token: 0x04007D7B RID: 32123
	private bool m_timerIsActive;

	// Token: 0x04007D7C RID: 32124
	[NonSerialized]
	public bool AllowTopWallTraversal;

	// Token: 0x04007D7D RID: 32125
	public IntVector2? m_lastDirectionKicked;

	// Token: 0x04007D7E RID: 32126
	private bool m_shouldDisplayOutline;

	// Token: 0x04007D7F RID: 32127
	private PlayerController m_lastInteractingPlayer;

	// Token: 0x04007D80 RID: 32128
	private DungeonData.Direction m_lastOutlineDirection = (DungeonData.Direction)(-1);

	// Token: 0x04007D81 RID: 32129
	private int m_lastSpriteId;
}
