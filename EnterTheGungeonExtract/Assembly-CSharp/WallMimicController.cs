using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010D4 RID: 4308
public class WallMimicController : CustomEngageDoer, IPlaceConfigurable
{
	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x06005EDB RID: 24283 RVA: 0x002467F0 File Offset: 0x002449F0
	protected bool CanAwaken
	{
		get
		{
			return this.m_isHidden && !PassiveItem.IsFlagSetAtAll(typeof(MimicRingItem));
		}
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x00246814 File Offset: 0x00244A14
	public void Awake()
	{
		ObjectVisibilityManager visibilityManager = base.visibilityManager;
		visibilityManager.OnToggleRenderers = (Action)Delegate.Combine(visibilityManager.OnToggleRenderers, new Action(this.OnToggleRenderers));
		base.aiActor.IsGone = true;
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x0024684C File Offset: 0x00244A4C
	public void Start()
	{
		if (!this.m_configured)
		{
			this.ConfigureOnPlacement(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Ceil)));
		}
		base.transform.position = this.m_startingPos;
		base.specRigidbody.Reinitialize();
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = DungeonData.GetAngleFromDirection(this.m_facingDirection);
		this.m_fakeWall = SecretRoomBuilder.GenerateWallMesh(this.m_facingDirection, this.pos1, "Mimic Wall", null, true);
		if (base.aiActor.ParentRoom != null)
		{
			this.m_fakeWall.transform.parent = base.aiActor.ParentRoom.hierarchyParent;
		}
		this.m_fakeWall.transform.position = this.pos1.ToVector3().WithZ((float)(this.pos1.y - 2)) + Vector3.down;
		if (this.m_facingDirection == DungeonData.Direction.SOUTH)
		{
			StaticReferenceManager.AllShadowSystemDepthHavers.Add(this.m_fakeWall.transform);
		}
		else if (this.m_facingDirection == DungeonData.Direction.WEST)
		{
			this.m_fakeWall.transform.position = this.m_fakeWall.transform.position + new Vector3(-0.1875f, 0f);
		}
		this.m_fakeCeiling = SecretRoomBuilder.GenerateRoomCeilingMesh(this.GetCeilingTileSet(this.pos1, this.pos2, this.m_facingDirection), "Mimic Ceiling", null, true);
		if (base.aiActor.ParentRoom != null)
		{
			this.m_fakeCeiling.transform.parent = base.aiActor.ParentRoom.hierarchyParent;
		}
		this.m_fakeCeiling.transform.position = this.pos1.ToVector3().WithZ((float)(this.pos1.y - 4));
		if (this.m_facingDirection == DungeonData.Direction.NORTH)
		{
			this.m_fakeCeiling.transform.position += new Vector3(-1f, 0f);
		}
		else if (this.m_facingDirection == DungeonData.Direction.SOUTH)
		{
			this.m_fakeCeiling.transform.position += new Vector3(-1f, 2f);
		}
		else if (this.m_facingDirection == DungeonData.Direction.EAST)
		{
			this.m_fakeCeiling.transform.position += new Vector3(-1f, 0f);
		}
		this.m_fakeCeiling.transform.position = this.m_fakeCeiling.transform.position.WithZ(this.m_fakeCeiling.transform.position.y - 5f);
		for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
		{
			base.specRigidbody.PixelColliders[i].Enabled = false;
		}
		if (this.m_facingDirection == DungeonData.Direction.NORTH)
		{
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.LowObstacle, 38, 38, 32, 8, true));
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.HighObstacle, 38, 54, 32, 8, true));
		}
		else if (this.m_facingDirection == DungeonData.Direction.SOUTH)
		{
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.LowObstacle, 38, 38, 32, 16, true));
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.HighObstacle, 38, 54, 32, 16, true));
		}
		else if (this.m_facingDirection == DungeonData.Direction.WEST || this.m_facingDirection == DungeonData.Direction.EAST)
		{
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.LowObstacle, 46, 38, 16, 32, true));
			base.specRigidbody.PixelColliders.Add(PixelCollider.CreateRectangle(CollisionLayer.HighObstacle, 46, 38, 16, 32, true));
		}
		base.specRigidbody.ForceRegenerate(null, null);
		base.aiActor.HasDonePlayerEnterCheck = true;
		this.m_collisionKnockbackStrength = base.aiActor.CollisionKnockbackStrength;
		base.aiActor.CollisionKnockbackStrength = 0f;
		base.aiActor.CollisionDamage = 0f;
		this.m_goopDoer = base.GetComponent<GoopDoer>();
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x00246CC0 File Offset: 0x00244EC0
	public void Update()
	{
		if (this.CanAwaken)
		{
			Vector2 vector = base.specRigidbody.PixelColliders[2].UnitBottomLeft;
			Vector2 vector2 = vector;
			if (this.m_facingDirection == DungeonData.Direction.SOUTH)
			{
				vector += new Vector2(0f, -1.5f);
				vector2 += new Vector2(2f, 0f);
			}
			else if (this.m_facingDirection == DungeonData.Direction.NORTH)
			{
				vector += new Vector2(0f, 1f);
				vector2 += new Vector2(2f, 3f);
			}
			else if (this.m_facingDirection == DungeonData.Direction.WEST)
			{
				vector += new Vector2(-1.5f, 0f);
				vector2 += new Vector2(0f, 2f);
			}
			else if (this.m_facingDirection == DungeonData.Direction.EAST)
			{
				vector += new Vector2(1f, 0f);
				vector2 += new Vector2(2.5f, 2f);
			}
			bool flag = false;
			foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			{
				if (playerController.CanDetectHiddenEnemies)
				{
					flag = true;
					if (!this.m_playerTrueSight)
					{
						this.m_playerTrueSight = true;
						base.aiActor.ToggleRenderers(true);
					}
				}
				if (playerController && playerController.healthHaver.IsAlive && !playerController.IsGhost)
				{
					Vector2 unitCenter = playerController.specRigidbody.GetUnitCenter(ColliderType.Ground);
					if (unitCenter.IsWithin(vector, vector2))
					{
						if (this.m_goopDoer)
						{
							Vector2 vector3 = base.specRigidbody.PixelColliders[2].UnitCenter;
							if (this.m_facingDirection == DungeonData.Direction.NORTH)
							{
								vector3 += Vector2.up;
							}
							DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.m_goopDoer.goopDefinition);
							goopManagerForGoopType.TimedAddGoopArc(vector3, 3f, 90f, DungeonData.GetIntVector2FromDirection(this.m_facingDirection).ToVector2(), 0.2f, null);
						}
						base.StartCoroutine(this.BecomeMimic());
					}
				}
			}
			if (!flag && this.m_playerTrueSight)
			{
				this.m_playerTrueSight = false;
				base.aiActor.ToggleRenderers(false);
			}
		}
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x00246F30 File Offset: 0x00245130
	protected override void OnDestroy()
	{
		if (base.visibilityManager)
		{
			ObjectVisibilityManager visibilityManager = base.visibilityManager;
			visibilityManager.OnToggleRenderers = (Action)Delegate.Remove(visibilityManager.OnToggleRenderers, new Action(this.OnToggleRenderers));
		}
		base.OnDestroy();
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x00246F70 File Offset: 0x00245170
	public override void StartIntro()
	{
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x00246F80 File Offset: 0x00245180
	private IEnumerator DoIntro()
	{
		base.aiActor.enabled = false;
		base.behaviorSpeculator.enabled = false;
		base.aiActor.ToggleRenderers(false);
		base.aiActor.IsGone = true;
		base.healthHaver.IsVulnerable = false;
		base.knockbackDoer.SetImmobile(true, "WallMimicController");
		this.m_hands = base.GetComponentsInChildren<GunHandController>();
		for (int i = 0; i < this.m_hands.Length; i++)
		{
			this.m_hands[i].gameObject.SetActive(false);
		}
		yield return null;
		base.aiActor.ToggleRenderers(false);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "WallMimicController");
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnBeamCollision = (SpeculativeRigidbody.OnBeamCollisionDelegate)Delegate.Combine(specRigidbody2.OnBeamCollision, new SpeculativeRigidbody.OnBeamCollisionDelegate(this.HandleBeamCollision));
		for (int j = 0; j < this.m_hands.Length; j++)
		{
			this.m_hands[j].gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00246F9C File Offset: 0x0024519C
	public override bool IsFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x00246FA4 File Offset: 0x002451A4
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.CanAwaken && rigidbodyCollision.OtherRigidbody.projectile)
		{
			base.StartCoroutine(this.BecomeMimic());
		}
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x00246FD4 File Offset: 0x002451D4
	private void HandleBeamCollision(BeamController beamController)
	{
		if (this.CanAwaken)
		{
			base.StartCoroutine(this.BecomeMimic());
		}
	}

	// Token: 0x06005EE5 RID: 24293 RVA: 0x00246FF0 File Offset: 0x002451F0
	private void OnToggleRenderers()
	{
		if (this.m_isHidden && base.aiActor)
		{
			if (base.aiActor.sprite)
			{
				base.aiActor.sprite.renderer.enabled = false;
			}
			if (base.aiActor.ShadowObject)
			{
				base.aiActor.ShadowObject.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x00247070 File Offset: 0x00245270
	public void ConfigureOnPlacement(RoomHandler room)
	{
		Vector2 vector = base.transform.position.XY() + new Vector2((float)base.specRigidbody.GroundPixelCollider.ManualOffsetX / 16f, (float)base.specRigidbody.GroundPixelCollider.ManualOffsetY / 16f);
		Vector2 vector2 = vector.ToIntVector2(VectorConversions.Round).ToVector2();
		base.transform.position += vector2 - vector;
		this.pos1 = vector2.ToIntVector2(VectorConversions.Floor);
		this.pos2 = this.pos1 + IntVector2.Right;
		this.m_facingDirection = this.GetFacingDirection(this.pos1, this.pos2);
		if (this.m_facingDirection == DungeonData.Direction.WEST)
		{
			this.pos1 = this.pos2;
			this.m_startingPos = base.transform.position + new Vector3(1f, 0f);
		}
		else if (this.m_facingDirection == DungeonData.Direction.EAST)
		{
			this.pos2 = this.pos1;
			this.m_startingPos = base.transform.position;
		}
		else
		{
			this.m_startingPos = base.transform.position + new Vector3(0.5f, 0f);
		}
		CellData cellData = GameManager.Instance.Dungeon.data[this.pos1];
		CellData cellData2 = GameManager.Instance.Dungeon.data[this.pos2];
		cellData.isSecretRoomCell = true;
		cellData2.isSecretRoomCell = true;
		cellData.forceDisallowGoop = true;
		cellData2.forceDisallowGoop = true;
		cellData.cellVisualData.preventFloorStamping = true;
		cellData2.cellVisualData.preventFloorStamping = true;
		cellData.isWallMimicHideout = true;
		cellData2.isWallMimicHideout = true;
		if (this.m_facingDirection == DungeonData.Direction.WEST || this.m_facingDirection == DungeonData.Direction.EAST)
		{
			GameManager.Instance.Dungeon.data[this.pos1 + IntVector2.Up].isSecretRoomCell = true;
		}
		this.m_configured = true;
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x00247290 File Offset: 0x00245490
	private IEnumerator BecomeMimic()
	{
		if (this.m_hands == null)
		{
			base.StartCoroutine(this.DoIntro());
		}
		this.m_isHidden = false;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnBeamCollision = (SpeculativeRigidbody.OnBeamCollisionDelegate)Delegate.Remove(specRigidbody2.OnBeamCollision, new SpeculativeRigidbody.OnBeamCollisionDelegate(this.HandleBeamCollision));
		AIAnimator tongueAnimator = base.aiAnimator.ChildAnimator;
		tongueAnimator.renderer.enabled = true;
		tongueAnimator.spriteAnimator.enabled = true;
		AIAnimator spitAnimator = tongueAnimator.ChildAnimator;
		spitAnimator.renderer.enabled = true;
		spitAnimator.spriteAnimator.enabled = true;
		tongueAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
		float delay = tongueAnimator.CurrentClipLength;
		float timer = 0f;
		bool hasPlayedVFX = false;
		while (timer < delay)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			if (!hasPlayedVFX && delay - timer < 0.1f)
			{
				hasPlayedVFX = true;
				if (this.WallDisappearVFX)
				{
					Vector2 zero = Vector2.zero;
					Vector2 zero2 = Vector2.zero;
					DungeonData.Direction facingDirection = this.m_facingDirection;
					if (facingDirection != DungeonData.Direction.SOUTH)
					{
						if (facingDirection != DungeonData.Direction.EAST)
						{
							if (facingDirection == DungeonData.Direction.WEST)
							{
								zero = new Vector2(0f, -1f);
								zero2 = new Vector2(0f, 1f);
							}
						}
						else
						{
							zero = new Vector2(0f, -1f);
							zero2 = new Vector2(0f, 1f);
						}
					}
					else
					{
						zero = new Vector2(0f, -1f);
						zero2 = new Vector2(0f, 1f);
					}
					Vector2 vector = Vector2.Min(this.pos1.ToVector2(), this.pos2.ToVector2()) + zero;
					Vector2 vector2 = Vector2.Max(this.pos1.ToVector2(), this.pos2.ToVector2()) + new Vector2(1f, 1f) + zero2;
					for (int i = 0; i < 5; i++)
					{
						Vector2 vector3 = BraveUtility.RandomVector2(vector, vector2, new Vector2(0.25f, 0.25f)) + new Vector2(0f, 1f);
						GameObject gameObject = SpawnManager.SpawnVFX(this.WallDisappearVFX, vector3, Quaternion.identity);
						tk2dBaseSprite tk2dBaseSprite = ((!gameObject) ? null : gameObject.GetComponent<tk2dBaseSprite>());
						if (tk2dBaseSprite)
						{
							tk2dBaseSprite.HeightOffGround = 8f;
							tk2dBaseSprite.UpdateZDepth();
						}
					}
				}
			}
		}
		PickupObject.ItemQuality targetQuality = ((UnityEngine.Random.value >= 0.2f) ? ((!BraveUtility.RandomBool()) ? PickupObject.ItemQuality.C : PickupObject.ItemQuality.D) : PickupObject.ItemQuality.B);
		GenericLootTable lootTable = ((!BraveUtility.RandomBool()) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
		PickupObject item = LootEngine.GetItemOfTypeAndQuality<PickupObject>(targetQuality, lootTable, false);
		if (item)
		{
			base.aiActor.AdditionalSafeItemDrops.Add(item);
		}
		base.aiActor.enabled = true;
		base.behaviorSpeculator.enabled = true;
		if (base.aiActor.ParentRoom != null && base.aiActor.ParentRoom.IsSealed)
		{
			base.aiActor.IgnoreForRoomClear = false;
		}
		int count = base.specRigidbody.PixelColliders.Count;
		for (int j = 0; j < count - 2; j++)
		{
			base.specRigidbody.PixelColliders[j].Enabled = true;
		}
		base.specRigidbody.PixelColliders.RemoveAt(count - 1);
		base.specRigidbody.PixelColliders.RemoveAt(count - 2);
		StaticReferenceManager.AllShadowSystemDepthHavers.Remove(this.m_fakeWall.transform);
		UnityEngine.Object.Destroy(this.m_fakeWall);
		UnityEngine.Object.Destroy(this.m_fakeCeiling);
		for (int k = 0; k < this.m_hands.Length; k++)
		{
			this.m_hands[k].gameObject.SetActive(true);
		}
		base.aiActor.ToggleRenderers(true);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "WallMimicController");
		}
		base.aiActor.IsGone = false;
		base.healthHaver.IsVulnerable = true;
		base.aiActor.State = AIActor.ActorState.Normal;
		for (int l = 0; l < this.m_hands.Length; l++)
		{
			this.m_hands[l].gameObject.SetActive(false);
		}
		this.m_isFinished = true;
		delay = 0.58f;
		timer = 0f;
		Vector3 targetPos = this.m_startingPos + DungeonData.GetIntVector2FromDirection(this.m_facingDirection).ToVector3();
		while (timer < delay)
		{
			base.aiAnimator.LockFacingDirection = true;
			base.aiAnimator.FacingDirection = DungeonData.GetAngleFromDirection(this.m_facingDirection);
			yield return null;
			timer += BraveTime.DeltaTime;
			base.transform.position = Vector3.Lerp(this.m_startingPos, targetPos, Mathf.InverseLerp(0.42f, 0.58f, timer));
			base.specRigidbody.Reinitialize();
		}
		base.aiAnimator.LockFacingDirection = false;
		base.knockbackDoer.SetImmobile(false, "WallMimicController");
		base.aiActor.CollisionDamage = 0.5f;
		base.aiActor.CollisionKnockbackStrength = this.m_collisionKnockbackStrength;
		yield break;
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x002472AC File Offset: 0x002454AC
	private DungeonData.Direction GetFacingDirection(IntVector2 pos1, IntVector2 pos2)
	{
		DungeonData data = GameManager.Instance.Dungeon.data;
		if (data.isWall(pos1 + IntVector2.Down) && data.isWall(pos1 + IntVector2.Up))
		{
			return DungeonData.Direction.EAST;
		}
		if (data.isWall(pos2 + IntVector2.Down) && data.isWall(pos2 + IntVector2.Up))
		{
			return DungeonData.Direction.WEST;
		}
		if (data.isWall(pos1 + IntVector2.Down) && data.isWall(pos2 + IntVector2.Down))
		{
			return DungeonData.Direction.NORTH;
		}
		if (data.isWall(pos1 + IntVector2.Up) && data.isWall(pos2 + IntVector2.Up))
		{
			return DungeonData.Direction.SOUTH;
		}
		Debug.LogError("Not able to determine the direction of a wall mimic!");
		return DungeonData.Direction.SOUTH;
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x0024738C File Offset: 0x0024558C
	private HashSet<IntVector2> GetCeilingTileSet(IntVector2 pos1, IntVector2 pos2, DungeonData.Direction facingDirection)
	{
		IntVector2 intVector;
		IntVector2 intVector2;
		if (facingDirection == DungeonData.Direction.NORTH)
		{
			intVector = pos1 + new IntVector2(-1, 0);
			intVector2 = pos2 + new IntVector2(1, 1);
		}
		else if (facingDirection == DungeonData.Direction.SOUTH)
		{
			intVector = pos1 + new IntVector2(-1, 2);
			intVector2 = pos2 + new IntVector2(1, 3);
		}
		else if (facingDirection == DungeonData.Direction.EAST)
		{
			intVector = pos1 + new IntVector2(-1, 0);
			intVector2 = pos2 + new IntVector2(0, 3);
		}
		else
		{
			if (facingDirection != DungeonData.Direction.WEST)
			{
				return null;
			}
			intVector = pos1 + new IntVector2(0, 0);
			intVector2 = pos2 + new IntVector2(1, 3);
		}
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				IntVector2 intVector3 = new IntVector2(i, j);
				hashSet.Add(intVector3);
			}
		}
		return hashSet;
	}

	// Token: 0x0400590B RID: 22795
	public GameObject WallDisappearVFX;

	// Token: 0x0400590C RID: 22796
	protected bool m_playerTrueSight;

	// Token: 0x0400590D RID: 22797
	private Vector3 m_startingPos;

	// Token: 0x0400590E RID: 22798
	private IntVector2 pos1;

	// Token: 0x0400590F RID: 22799
	private IntVector2 pos2;

	// Token: 0x04005910 RID: 22800
	private DungeonData.Direction m_facingDirection;

	// Token: 0x04005911 RID: 22801
	private GameObject m_fakeWall;

	// Token: 0x04005912 RID: 22802
	private GameObject m_fakeCeiling;

	// Token: 0x04005913 RID: 22803
	private GunHandController[] m_hands;

	// Token: 0x04005914 RID: 22804
	private GoopDoer m_goopDoer;

	// Token: 0x04005915 RID: 22805
	private bool m_isHidden = true;

	// Token: 0x04005916 RID: 22806
	private bool m_isFinished;

	// Token: 0x04005917 RID: 22807
	private float m_collisionKnockbackStrength;

	// Token: 0x04005918 RID: 22808
	private bool m_configured;
}
