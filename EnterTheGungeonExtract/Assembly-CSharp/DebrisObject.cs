using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001636 RID: 5686
public class DebrisObject : EphemeralObject
{
	// Token: 0x060084B5 RID: 33973 RVA: 0x0036A6CC File Offset: 0x003688CC
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.AllDebris.Clear();
		DebrisObject.m_STATIC_PitfallPoints = null;
		DebrisObject.SRB_Pits.Clear();
		DebrisObject.SRB_Walls.Clear();
		DebrisObject.m_STATIC_PitfallPoints = null;
	}

	// Token: 0x170013F0 RID: 5104
	// (get) Token: 0x060084B6 RID: 33974 RVA: 0x0036A6F8 File Offset: 0x003688F8
	public bool Static
	{
		get
		{
			return this.isStatic;
		}
	}

	// Token: 0x170013F1 RID: 5105
	// (get) Token: 0x060084B7 RID: 33975 RVA: 0x0036A700 File Offset: 0x00368900
	// (set) Token: 0x060084B8 RID: 33976 RVA: 0x0036A708 File Offset: 0x00368908
	public float GravityOverride { get; set; }

	// Token: 0x170013F2 RID: 5106
	// (get) Token: 0x060084B9 RID: 33977 RVA: 0x0036A714 File Offset: 0x00368914
	public bool HasBeenTriggered
	{
		get
		{
			return this.m_hasBeenTriggered;
		}
	}

	// Token: 0x170013F3 RID: 5107
	// (get) Token: 0x060084BA RID: 33978 RVA: 0x0036A71C File Offset: 0x0036891C
	public bool IsPickupObject
	{
		get
		{
			return this.m_isPickupObject;
		}
	}

	// Token: 0x170013F4 RID: 5108
	// (get) Token: 0x060084BB RID: 33979 RVA: 0x0036A724 File Offset: 0x00368924
	// (set) Token: 0x060084BC RID: 33980 RVA: 0x0036A72C File Offset: 0x0036892C
	public bool IsAccurateDebris
	{
		get
		{
			return this.accurateDebris;
		}
		set
		{
			this.accurateDebris = value;
		}
	}

	// Token: 0x060084BD RID: 33981 RVA: 0x0036A738 File Offset: 0x00368938
	public void ForceUpdatePitfall()
	{
		this.m_forceCheckGrounded = true;
	}

	// Token: 0x170013F5 RID: 5109
	// (get) Token: 0x060084BE RID: 33982 RVA: 0x0036A744 File Offset: 0x00368944
	// (set) Token: 0x060084BF RID: 33983 RVA: 0x0036A74C File Offset: 0x0036894C
	public bool DontSetLayer { get; set; }

	// Token: 0x060084C0 RID: 33984 RVA: 0x0036A758 File Offset: 0x00368958
	protected override void Awake()
	{
		base.Awake();
		if (DebrisObject.m_STATIC_PitfallPoints == null)
		{
			DebrisObject.m_STATIC_PitfallPoints = new DebrisObject.PitFallPoint[5];
			for (int i = 0; i < 5; i++)
			{
				DebrisObject.m_STATIC_PitfallPoints[i] = new DebrisObject.PitFallPoint(null, Vector3.zero);
			}
		}
		this.m_dungeonRef = GameManager.Instance.Dungeon;
		StaticReferenceManager.AllDebris.Add(this);
	}

	// Token: 0x060084C1 RID: 33985 RVA: 0x0036A7C0 File Offset: 0x003689C0
	public override void Start()
	{
		base.Start();
		if (DebrisObject.fgNonsenseLayerID == -1)
		{
			DebrisObject.fgNonsenseLayerID = LayerMask.NameToLayer("FG_Nonsense");
		}
		base.sprite.gameObject.SetLayerRecursively(DebrisObject.fgNonsenseLayerID);
		this.m_spriteBounds = base.sprite.GetBounds();
		if (!this.m_isPickupObject)
		{
			this.m_isPickupObject = base.GetComponent<PickupObject>() != null;
		}
		if (this.m_isPickupObject || this.m_spriteBounds.size.x > this.ACCURATE_DEBRIS_THRESHOLD || this.m_spriteBounds.size.y > this.ACCURATE_DEBRIS_THRESHOLD)
		{
			this.accurateDebris = true;
		}
		if (base.sprite == null)
		{
			base.sprite = base.GetComponentInChildren<tk2dSprite>();
		}
		if (base.sprite != null)
		{
			DepthLookupManager.AssignRendererToSortingLayer(base.sprite.renderer, DepthLookupManager.GungeonSortingLayer.PLAYFIELD);
		}
		if (base.specRigidbody != null && base.GetComponent<MinorBreakable>() == null)
		{
			this.InitializeForCollisions();
		}
		if (!this.shouldUseSRBMotion && !this.DontSetLayer)
		{
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Nonsense"));
		}
	}

	// Token: 0x060084C2 RID: 33986 RVA: 0x0036A910 File Offset: 0x00368B10
	public override void OnDespawned()
	{
		this.m_hasBeenTriggered = false;
		base.OnDespawned();
	}

	// Token: 0x060084C3 RID: 33987 RVA: 0x0036A920 File Offset: 0x00368B20
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllDebris.Remove(this);
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
		base.OnDestroy();
	}

	// Token: 0x060084C4 RID: 33988 RVA: 0x0036A978 File Offset: 0x00368B78
	public void FlagAsPickup()
	{
		this.m_isPickupObject = true;
	}

	// Token: 0x060084C5 RID: 33989 RVA: 0x0036A984 File Offset: 0x00368B84
	public void AssignFinalWorldDepth(float depth)
	{
		this.m_finalWorldDepth = depth;
		this.m_forceUseFinalDepth = true;
	}

	// Token: 0x060084C6 RID: 33990 RVA: 0x0036A994 File Offset: 0x00368B94
	public void InitializeForCollisions()
	{
		if (this.m_collisionsInitialized)
		{
			return;
		}
		this.m_collisionsInitialized = true;
		if (base.specRigidbody != null && base.GetComponent<MinorBreakable>() == null)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
	}

	// Token: 0x060084C7 RID: 33991 RVA: 0x0036AA00 File Offset: 0x00368C00
	public void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (!this.m_hasBeenTriggered)
		{
			this.shouldUseSRBMotion = true;
			Vector2 normalized = otherRigidbody.Velocity.normalized;
			float num = otherRigidbody.Velocity.magnitude;
			num = Mathf.Min(num, 5f);
			Vector2 vector = normalized * num;
			float num2 = Mathf.Lerp(-30f, 30f, UnityEngine.Random.value);
			Vector3 vector2 = Quaternion.Euler(0f, 0f, num2) * (vector.normalized * UnityEngine.Random.Range(num * 0.75f, num * 1.25f)).ToVector3ZUp(1f);
			this.Trigger(vector2, 0.5f, 1f);
			if (!this.collisionStopsBullets)
			{
				PhysicsEngine.SkipCollision = true;
			}
		}
		else
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
	}

	// Token: 0x060084C8 RID: 33992 RVA: 0x0036AAF0 File Offset: 0x00368CF0
	public void OnAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		if (this.followupBehavior == DebrisObject.DebrisFollowupAction.FollowupAnimation)
		{
			base.spriteAnimator.Play(this.followupIdentifier);
		}
		base.spriteAnimator.AnimationCompleted = null;
	}

	// Token: 0x060084C9 RID: 33993 RVA: 0x0036AB1C File Offset: 0x00368D1C
	public void ForceReinitializePosition()
	{
		Vector2 vector = this.m_transform.position.XY();
		this.m_startPosition = new Vector3(vector.x, vector.y - this.m_startingHeightOffGround, this.m_startingHeightOffGround);
		this.m_currentPosition = this.m_startPosition;
	}

	// Token: 0x060084CA RID: 33994 RVA: 0x0036AB6C File Offset: 0x00368D6C
	public void Trigger(Vector3 startingForce, float startingHeight, float angularVelocityModifier = 1f)
	{
		if (this.m_hasBeenTriggered)
		{
			return;
		}
		if (base.specRigidbody != null && base.specRigidbody.enabled)
		{
			this.shouldUseSRBMotion = true;
			if (base.specRigidbody.PrimaryPixelCollider.CollisionLayer == CollisionLayer.BulletBlocker || base.specRigidbody.PrimaryPixelCollider.CollisionLayer == CollisionLayer.BulletBreakable)
			{
				base.specRigidbody.CollideWithOthers = false;
			}
		}
		else if (base.specRigidbody == null)
		{
			this.shouldUseSRBMotion = false;
		}
		if (this.groupManager != null)
		{
			this.groupManager.DeregisterDebris(this);
		}
		this.m_transform = base.transform;
		this.m_renderer = base.renderer;
		if (base.sprite == null)
		{
			base.sprite = base.GetComponentInChildren<tk2dSprite>();
		}
		this.m_initialWorldDepth = base.sprite.HeightOffGround;
		this.m_startingHeightOffGround = startingHeight;
		Vector2 vector = this.m_transform.position.XY();
		this.m_startPosition = new Vector3(vector.x, vector.y - startingHeight, startingHeight);
		this.m_currentPosition = this.m_startPosition;
		this.m_velocity = startingForce / this.inertialMass;
		if (this.usesLifespan)
		{
			this.m_currentLifespan = UnityEngine.Random.Range(this.lifespanMin, this.lifespanMax);
		}
		this.angularVelocity = (this.canRotate ? (this.angularVelocity + UnityEngine.Random.Range(-this.angularVelocityVariance, this.angularVelocityVariance)) : 0f);
		this.angularVelocity *= angularVelocityModifier;
		this.m_hasBeenTriggered = true;
		this.isStatic = false;
		if (this.followupBehavior == DebrisObject.DebrisFollowupAction.FollowupAnimation && !string.IsNullOrEmpty(this.followupIdentifier))
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			base.spriteAnimator.Play();
		}
		else if (this.playAnimationOnTrigger)
		{
			if (this.usesDirectionalFallAnimations)
			{
				base.spriteAnimator.Play(this.directionalAnimationData.GetAnimationForVector(startingForce.XY()));
			}
			else
			{
				base.spriteAnimator.Play();
			}
		}
		if (this.OnTriggered != null)
		{
			this.OnTriggered();
		}
	}

	// Token: 0x060084CB RID: 33995 RVA: 0x0036ADD8 File Offset: 0x00368FD8
	public void ClearVelocity()
	{
		this.m_velocity = Vector3.zero;
		this.m_frameVelocity = Vector3.zero;
	}

	// Token: 0x060084CC RID: 33996 RVA: 0x0036ADF0 File Offset: 0x00368FF0
	public void ApplyFrameVelocity(Vector2 vel)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.m_hasBeenTriggered || this.m_currentPosition.z > 0f)
		{
			return;
		}
		this.doesDecay = true;
		this.isStatic = false;
		this.m_frameVelocity += new Vector3(vel.x, vel.y, 0f) / this.inertialMass;
	}

	// Token: 0x060084CD RID: 33997 RVA: 0x0036AE6C File Offset: 0x0036906C
	public void ApplyVelocity(Vector2 vel)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.m_hasBeenTriggered || this.m_currentPosition.z > 0f)
		{
			return;
		}
		this.doesDecay = true;
		this.isStatic = false;
		this.angularVelocity = 0f;
		if (this.canRotate)
		{
			this.angularVelocity = (float)UnityEngine.Random.Range(30, 90);
		}
		this.m_velocity += new Vector3(vel.x, vel.y, 0f) / this.inertialMass;
	}

	// Token: 0x060084CE RID: 33998 RVA: 0x0036AF10 File Offset: 0x00369110
	protected CellData GetCellFromPosition(Vector3 p)
	{
		if (this.m_dungeonRef == null)
		{
			this.m_dungeonRef = GameManager.Instance.Dungeon;
		}
		IntVector2 intVector = p.IntXY(VectorConversions.Floor);
		if (!this.m_dungeonRef.data.CheckInBounds(intVector))
		{
			return null;
		}
		return this.m_dungeonRef.data[intVector];
	}

	// Token: 0x060084CF RID: 33999 RVA: 0x0036AF70 File Offset: 0x00369170
	protected bool CheckPositionFacewall(Vector3 position)
	{
		CellData cellFromPosition = this.GetCellFromPosition(position);
		if (cellFromPosition != null && cellFromPosition.IsAnyFaceWall())
		{
			return true;
		}
		for (int i = 0; i < DebrisObject.SRB_Walls.Count; i++)
		{
			if (DebrisObject.SRB_Walls[i].ContainsPoint(position, 2147483647, false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060084D0 RID: 34000 RVA: 0x0036AFD8 File Offset: 0x003691D8
	protected Tuple<CellData, Vector3> GetCellPositionTupleFromPosition(Vector3 p)
	{
		return Tuple.Create<CellData, Vector3>(this.GetCellFromPosition(p), p);
	}

	// Token: 0x060084D1 RID: 34001 RVA: 0x0036AFE8 File Offset: 0x003691E8
	protected bool CheckCurrentCellsFacewall(Vector3 currentPosition)
	{
		Quaternion rotation = this.m_transform.rotation;
		currentPosition += rotation * this.m_spriteBounds.min;
		if (this.CheckPositionFacewall(currentPosition + rotation * (0.5f * this.m_spriteBounds.size)))
		{
			return true;
		}
		if (this.accurateDebris)
		{
			if (this.CheckPositionFacewall(currentPosition))
			{
				return true;
			}
			if (this.CheckPositionFacewall(currentPosition + rotation * new Vector3(this.m_spriteBounds.size.x, 0f, 0f)))
			{
				return true;
			}
			if (this.CheckPositionFacewall(currentPosition + rotation * this.m_spriteBounds.size))
			{
				return true;
			}
			if (this.CheckPositionFacewall(currentPosition + rotation * new Vector3(0f, this.m_spriteBounds.size.y, 0f)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060084D2 RID: 34002 RVA: 0x0036B0FC File Offset: 0x003692FC
	private void RecalculateStaticTargetCells_ProcessPosition(Vector3 position, int index, DebrisObject.PitFallPoint[] targetArray)
	{
		DebrisObject.PitFallPoint pitFallPoint = targetArray[index];
		CellData cellFromPosition = this.GetCellFromPosition(position);
		pitFallPoint.cellData = cellFromPosition;
		pitFallPoint.position = position;
		pitFallPoint.inPit = false;
	}

	// Token: 0x060084D3 RID: 34003 RVA: 0x0036B12C File Offset: 0x0036932C
	protected void RecalculateStaticTargetCells(Vector3 newPosition, Quaternion newRotation)
	{
		if (DebrisObject.m_STATIC_PitfallPoints == null)
		{
			return;
		}
		newPosition.z = 0f;
		newPosition += newRotation * this.m_spriteBounds.min;
		this.RecalculateStaticTargetCells_ProcessPosition(newPosition + newRotation * (0.5f * this.m_spriteBounds.size), 0, DebrisObject.m_STATIC_PitfallPoints);
		if (this.accurateDebris)
		{
			this.RecalculateStaticTargetCells_ProcessPosition(newPosition, 1, DebrisObject.m_STATIC_PitfallPoints);
			this.RecalculateStaticTargetCells_ProcessPosition(newPosition + newRotation * new Vector3(this.m_spriteBounds.size.x, 0f, 0f), 2, DebrisObject.m_STATIC_PitfallPoints);
			this.RecalculateStaticTargetCells_ProcessPosition(newPosition + newRotation * this.m_spriteBounds.size, 3, DebrisObject.m_STATIC_PitfallPoints);
			this.RecalculateStaticTargetCells_ProcessPosition(newPosition + newRotation * new Vector3(0f, this.m_spriteBounds.size.y, 0f), 4, DebrisObject.m_STATIC_PitfallPoints);
		}
	}

	// Token: 0x060084D4 RID: 34004 RVA: 0x0036B248 File Offset: 0x00369448
	protected void HandleRotation(float adjustedDeltaTime)
	{
		if (this.canRotate)
		{
			int num = ((this.m_velocity.x <= 0f) ? 1 : (-1));
			this.m_transform.RotateAround(base.sprite.WorldCenter, Vector3.forward, this.angularVelocity * adjustedDeltaTime * (float)num);
			if (this.IsPickupObject)
			{
				base.sprite.ForceRotationRebuild();
			}
		}
	}

	// Token: 0x060084D5 RID: 34005 RVA: 0x0036B2C0 File Offset: 0x003694C0
	protected virtual void UpdateVelocity(float adjustedDeltaTime)
	{
		if (this.m_currentPosition.z > 0f)
		{
			this.m_velocity += new Vector3(0f, 0f, -1f) * ((this.GravityOverride == 0f) ? 10f : this.GravityOverride) * adjustedDeltaTime;
		}
	}

	// Token: 0x060084D6 RID: 34006 RVA: 0x0036B334 File Offset: 0x00369534
	protected void HandleWallOrPitDeflection(IntVector2 currentGridCell, CellData nextCell, float adjustedDeltaTime)
	{
		if (base.name.Contains("Bomb"))
		{
			Debug.Log("deflecto detecto");
		}
		if (nextCell.IsAnyFaceWall() && !this.m_recentlyBouncedOffTopwall)
		{
			if (nextCell.position.x != currentGridCell.x)
			{
				this.m_velocity.x = -this.m_velocity.x * (1f - this.decayOnBounce);
			}
			this.m_velocity.y = -Mathf.Abs(this.m_velocity.y) * (1f - this.decayOnBounce);
			this.m_frameVelocity = Vector3.zero;
		}
		else
		{
			if (nextCell.position.x != currentGridCell.x)
			{
				this.m_velocity.x = -this.m_velocity.x * (1f - this.decayOnBounce);
				this.m_frameVelocity = Vector3.zero;
			}
			if (nextCell.position.y != currentGridCell.y)
			{
				this.m_velocity.y = -this.m_velocity.y * (1f - this.decayOnBounce);
				this.m_frameVelocity = Vector3.zero;
			}
		}
	}

	// Token: 0x170013F6 RID: 5110
	// (get) Token: 0x060084D7 RID: 34007 RVA: 0x0036B478 File Offset: 0x00369678
	public Vector3 UnadjustedDebrisPosition
	{
		get
		{
			return this.m_currentPosition;
		}
	}

	// Token: 0x060084D8 RID: 34008 RVA: 0x0036B480 File Offset: 0x00369680
	public void IncrementZHeight(float amount)
	{
		if (this.HasBeenTriggered)
		{
			this.isStatic = false;
			this.m_currentPosition.z = this.m_currentPosition.z + amount;
		}
	}

	// Token: 0x060084D9 RID: 34009 RVA: 0x0036B4A8 File Offset: 0x003696A8
	protected void ConvertYToZHeight(float amount)
	{
		this.m_currentPosition.y = this.m_currentPosition.y - amount;
		this.m_currentPosition.z = this.m_currentPosition.z + amount;
	}

	// Token: 0x060084DA RID: 34010 RVA: 0x0036B4D0 File Offset: 0x003696D0
	protected bool CheckPitfallPointsForPit(ref DebrisObject.PitFallPoint[] p, ref SpeculativeRigidbody newPlatform)
	{
		if (!this.m_transform || DebrisObject.m_STATIC_PitfallPoints == null)
		{
			return false;
		}
		this.RecalculateStaticTargetCells(this.m_transform.position, this.m_transform.rotation);
		p = DebrisObject.m_STATIC_PitfallPoints;
		int num = ((!this.accurateDebris) ? 1 : 5);
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			CellData cellData = p[i].cellData;
			if (cellData != null)
			{
				if (cellData.type == CellType.PIT && !this.PreventFallingInPits)
				{
					SpeculativeRigidbody speculativeRigidbody = null;
					if (!GameManager.Instance.Dungeon.IsPixelOnPlatform(p[i].position, out speculativeRigidbody))
					{
						num2++;
						p[i].inPit = true;
						if (cellData.fallingPrevented)
						{
							num3++;
						}
						goto IL_14B;
					}
					newPlatform = speculativeRigidbody;
				}
				if (DebrisObject.SRB_Pits != null && !this.PreventFallingInPits)
				{
					for (int j = 0; j < DebrisObject.SRB_Pits.Count; j++)
					{
						if (DebrisObject.SRB_Pits[j].ContainsPoint(p[i].position, 2147483647, true))
						{
							p[i].inPit = true;
							num2++;
							break;
						}
					}
				}
			}
			IL_14B:;
		}
		if (num2 <= Mathf.FloorToInt((float)num / 2f))
		{
			return false;
		}
		if (num2 - num3 > Mathf.FloorToInt((float)num / 2f))
		{
			return true;
		}
		this.m_forceCheckGrounded = true;
		return false;
	}

	// Token: 0x060084DB RID: 34011 RVA: 0x0036B668 File Offset: 0x00369868
	protected void ForceCheckForPitfall()
	{
		this.m_forceCheckGrounded = false;
		DebrisObject.PitFallPoint[] array = null;
		SpeculativeRigidbody speculativeRigidbody = null;
		if (!this.PreventFallingInPits && this.CheckPitfallPointsForPit(ref array, ref speculativeRigidbody))
		{
			this.FallIntoPit(array);
		}
	}

	// Token: 0x060084DC RID: 34012 RVA: 0x0036B6A4 File Offset: 0x003698A4
	protected void EnsurePickupsAreNicelyDistant(float realDeltaTime)
	{
		if (this.IsPickupObject && this.onGround)
		{
			PickupObject component = base.GetComponent<PickupObject>();
			if (component != null && component is CurrencyPickup)
			{
				return;
			}
			for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
			{
				DebrisObject debrisObject = StaticReferenceManager.AllDebris[i];
				if (debrisObject && debrisObject.IsPickupObject && debrisObject != this && debrisObject.onGround)
				{
					this.MovePickupAwayFromObject(debrisObject, 1.5f, 0.5f);
				}
			}
		}
	}

	// Token: 0x060084DD RID: 34013 RVA: 0x0036B74C File Offset: 0x0036994C
	private void MovePickupAwayFromObject(BraveBehaviour otherObject, float minDist, float power)
	{
		Vector2 vector = ((!(otherObject.sprite != null)) ? (otherObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : otherObject.sprite.WorldCenter);
		Vector2 vector2 = ((!(base.sprite != null)) ? (base.transform.position.XY() + new Vector2(0.5f, 0.5f)) : base.sprite.WorldCenter);
		Vector2 vector3 = vector - vector2;
		float magnitude = vector3.magnitude;
		if (magnitude < minDist)
		{
			if (otherObject is DebrisObject)
			{
				((DebrisObject)otherObject).ApplyFrameVelocity(power * vector3.normalized);
			}
			this.ApplyFrameVelocity(power * vector3.normalized * -1f);
		}
	}

	// Token: 0x060084DE RID: 34014 RVA: 0x0036B83C File Offset: 0x00369A3C
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (!base.enabled && !this.ForceUpdateIfDisabled)
		{
			return;
		}
		if (!this.m_hasBeenTriggered)
		{
			return;
		}
		if (this.isPitFalling)
		{
			return;
		}
		if (this.IsPickupObject && base.sprite && !this.isFalling)
		{
			base.sprite.HeightOffGround = Mathf.Max(base.sprite.HeightOffGround, -1f);
		}
		if (this.motionMultiplier <= 0f)
		{
			this.m_currentPosition.z = 0f;
			this.m_velocity = Vector3.zero;
			this.m_frameVelocity = Vector3.zero;
			this.isStatic = true;
			this.OnBecameGrounded();
		}
		SpeculativeRigidbody platform = this.m_platform;
		this.m_platform = null;
		if (this.usesLifespan)
		{
			this.m_currentLifespan -= realDeltaTime;
			if (this.m_currentLifespan <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		if (this.m_forceCheckGrounded && this.onGround)
		{
			this.ForceCheckForPitfall();
		}
		if (this.onGround)
		{
			this.EnsurePickupsAreNicelyDistant(realDeltaTime);
		}
		if (this.isStatic)
		{
			this.m_platform = platform;
			return;
		}
		this.m_forceCheckGrounded = false;
		IntVector2 intVector = new IntVector2(Mathf.FloorToInt(this.m_transform.position.x), Mathf.FloorToInt(this.m_transform.position.y));
		if (this.IsCorpse && base.sprite)
		{
			intVector = base.sprite.WorldCenter.ToIntVector2(VectorConversions.Floor);
		}
		if (GameManager.Instance.Dungeon != null && !GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		CellData cellData = ((!(GameManager.Instance.Dungeon != null)) ? null : GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y]);
		if (cellData == null)
		{
			if (this.accurateDebris)
			{
				Debug.LogError("Destroying large debris for being outside valid cell ranges! " + base.name);
			}
			this.MaybeRespawnIfImportant();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		float num = realDeltaTime * this.motionMultiplier;
		num = Mathf.Clamp(num, 0f, 0.1f);
		bool flag = this.CheckCurrentCellsFacewall(this.m_transform.position);
		if (this.m_currentPosition.z > 0f || this.m_velocity.z > 0f || this.doesDecay || this.isFalling)
		{
			if (this.m_currentPosition.z <= 0f && !this.isFalling)
			{
				this.m_velocity.z = 0f;
				this.m_currentPosition.z = 0f;
			}
			this.HandleRotation(num);
			Vector3 vector = this.m_currentPosition + this.m_velocity * num + this.m_frameVelocity * num;
			if (GameManager.Instance.Dungeon != null)
			{
				this.RecalculateStaticTargetCells(vector, this.m_transform.rotation);
				DebrisObject.PitFallPoint[] static_PitfallPoints = DebrisObject.m_STATIC_PitfallPoints;
				int num2 = 0;
				int num3 = 0;
				int num4 = ((!this.accurateDebris) ? 1 : 5);
				for (int i = 0; i < num4; i++)
				{
					if (static_PitfallPoints != null && static_PitfallPoints.Length > i && static_PitfallPoints[i] != null)
					{
						CellData cellData2 = static_PitfallPoints[i].cellData;
						if (cellData2 != null)
						{
							bool flag2 = ((this.m_currentPosition.z <= 0f) ? (cellData2.type == CellType.WALL) : (cellData2.type == CellType.WALL && !cellData2.IsLowerFaceWall()));
							bool flag3 = this.m_isPickupObject && GameManager.Instance.Dungeon.data.isTopWall(cellData2.position.x, cellData2.position.y);
							if (this.m_isPickupObject)
							{
								bool flag4 = cellData2.parentArea != null && cellData2.parentArea.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.WEIRD_SHOP;
								flag2 = flag2 || (!flag4 && cellData2.type == CellType.PIT);
							}
							if (flag3)
							{
								this.m_recentlyBouncedOffTopwall = true;
								this.m_velocity.y = Mathf.Abs(this.m_velocity.z) + 1f;
								flag2 = true;
							}
							if (flag2)
							{
								CellData cellFromPosition = this.GetCellFromPosition(this.m_currentPosition + this.m_transform.rotation * (0.5f * this.m_spriteBounds.size));
								IntVector2 intVector2 = ((cellFromPosition == null) ? intVector : cellFromPosition.position);
								this.HandleWallOrPitDeflection(intVector2, cellData2, num);
								break;
							}
							bool flag5 = cellData2.type == CellType.PIT;
							if (!flag5)
							{
								for (int j = 0; j < DebrisObject.SRB_Pits.Count; j++)
								{
									if (DebrisObject.SRB_Pits[j].ContainsPoint(static_PitfallPoints[i].position, 2147483647, true))
									{
										flag5 = true;
										break;
									}
								}
							}
							if (this.m_currentPosition.z <= 0f && flag5 && !this.PreventFallingInPits)
							{
								SpeculativeRigidbody speculativeRigidbody;
								if (GameManager.Instance.Dungeon.IsPixelOnPlatform(static_PitfallPoints[i].position, out speculativeRigidbody))
								{
									this.m_platform = speculativeRigidbody;
								}
								else
								{
									num2++;
									static_PitfallPoints[i].inPit = true;
									if (cellData2.fallingPrevented)
									{
										num3++;
									}
								}
							}
						}
					}
				}
				if (num2 > Mathf.FloorToInt((float)num4 / 2f) && !this.PreventFallingInPits)
				{
					if (!this.PreventFallingInPits && num2 - num3 > Mathf.FloorToInt((float)num4 / 2f))
					{
						this.FallIntoPit(static_PitfallPoints);
					}
					else
					{
						this.m_forceCheckGrounded = true;
					}
				}
				bool flag6 = this.m_isPickupObject && GameManager.Instance.Dungeon.data.isTopWall(cellData.position.x, cellData.position.y);
				if (flag6)
				{
					this.m_recentlyBouncedOffTopwall = true;
					this.m_velocity.y = Mathf.Abs(this.m_velocity.z) + 1f;
				}
			}
			vector = this.m_currentPosition + this.m_velocity * num + this.m_frameVelocity * num;
			this.m_frameVelocity = Vector3.zero;
			if (this.shouldUseSRBMotion)
			{
				this.m_transform.position = new Vector3(vector.x, vector.y + vector.z, this.m_transform.position.z);
				if (this.IsPickupObject)
				{
					this.m_transform.position = this.m_transform.position.Quantize(0.0625f);
				}
				if (base.sprite && this.shadowSprite)
				{
					this.m_transform.position = this.m_transform.position.Quantize(0.0625f);
					this.shadowSprite.PlaceAtPositionByAnchor(base.sprite.WorldBottomCenter.WithY(this.m_transform.position.y + 0.0625f), tk2dBaseSprite.Anchor.MiddleCenter);
					this.shadowSprite.transform.position = (this.shadowSprite.transform.position + new Vector3(0f, -vector.z, 0f)).Quantize(0.0625f);
				}
				base.specRigidbody.Reinitialize();
			}
			else
			{
				this.m_transform.position = new Vector3(vector.x, vector.y + vector.z, this.m_transform.position.z);
				if (base.sprite && this.shadowSprite)
				{
					this.m_transform.position = this.m_transform.position.Quantize(0.0625f);
					this.shadowSprite.PlaceAtPositionByAnchor(base.sprite.WorldBottomCenter.WithY(this.m_transform.position.y + 0.0625f), tk2dBaseSprite.Anchor.MiddleCenter);
					this.shadowSprite.transform.position = (this.shadowSprite.transform.position + new Vector3(0f, -vector.z, 0f)).Quantize(0.0625f);
				}
			}
			this.UpdateVelocity(num);
			this.m_currentPosition = vector;
			if (!this.onGround && !this.isFalling)
			{
				base.sprite.HeightOffGround = this.m_currentPosition.z + this.additionalHeightBoost;
			}
			if (this.doesDecay)
			{
				this.m_velocity *= 0.97f;
				if (this.m_velocity.magnitude < 0.5f)
				{
					this.doesDecay = false;
					this.m_velocity = Vector3.zero;
				}
			}
		}
		else
		{
			SpeculativeRigidbody speculativeRigidbody2 = null;
			if (this.OnTouchedGround != null)
			{
				this.OnTouchedGround(this);
			}
			DebrisObject.PitFallPoint[] array = null;
			if (GameManager.Instance.Dungeon == null)
			{
				UnityEngine.Object.Destroy(base.gameObject, 1f);
				this.isStatic = true;
			}
			else if (flag && !this.m_recentlyBouncedOffTopwall && !this.m_wasFacewallFixed)
			{
				while (this.m_currentPosition.z < 0f)
				{
					this.ConvertYToZHeight(0.5f);
				}
				this.isStatic = false;
				this.m_wasFacewallFixed = true;
			}
			else if (this.m_isPickupObject && cellData.IsTopWall())
			{
				this.m_recentlyBouncedOffTopwall = true;
				this.ConvertYToZHeight(0.5f);
				this.m_velocity.y = Mathf.Max(1f, Mathf.Abs(this.m_velocity.y));
			}
			else if (cellData.type == CellType.WALL && !this.m_isPickupObject && !this.IsAccurateDebris)
			{
				this.MaybeRespawnIfImportant();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (!this.PreventFallingInPits && this.CheckPitfallPointsForPit(ref array, ref speculativeRigidbody2))
			{
				this.FallIntoPit(array);
			}
			else if (!this.m_isPickupObject && !this.PreventAbsorption && cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Water && cellData.cellVisualData.absorbsDebris)
			{
				if (base.sprite)
				{
					GameManager.Instance.Dungeon.DoSplashDustupAtPosition(base.sprite.WorldCenter);
				}
				else
				{
					GameManager.Instance.Dungeon.DoSplashDustupAtPosition(base.transform.position.XY());
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (this.bounceCount > 0 && base.GetComponent<MinorBreakable>() == null)
			{
				if (!string.IsNullOrEmpty(this.audioEventName))
				{
					AkSoundEngine.PostEvent(this.audioEventName, base.gameObject);
				}
				this.m_velocity = this.m_velocity.WithZ(Mathf.Min(5f, this.m_velocity.z * -1f)) * (1f - this.decayOnBounce);
				if (this.killTranslationOnBounce)
				{
					this.m_velocity = Vector3.zero.WithZ(this.m_velocity.z);
				}
				if (this.canRotate && this.additionalBounceEnglish > 0f)
				{
					this.angularVelocity += Mathf.Sign(this.angularVelocity) * this.additionalBounceEnglish;
				}
				if (this.optionalBounceVFX != null)
				{
					GameObject gameObject = SpawnManager.SpawnVFX(this.optionalBounceVFX, false);
					tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
					component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
				if (this.DoesGoopOnRest)
				{
					DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.AssignedGoop).AddGoopCircle(base.sprite.WorldCenter, this.GoopRadius, -1, false, -1);
				}
				this.m_currentPosition = this.m_currentPosition.WithZ(0.05f);
				if (this.OnBounced != null)
				{
					this.OnBounced(this);
				}
				this.bounceCount--;
			}
			else if (this.m_isPickupObject && cellData.isOccupied && this.IsVitalPickup())
			{
				this.m_velocity = new Vector3(0f, -3f, 1f);
				this.ConvertYToZHeight(2f);
			}
			else
			{
				if (speculativeRigidbody2 != null)
				{
					this.m_platform = speculativeRigidbody2;
				}
				this.OnBecameGrounded();
			}
		}
		if (platform != null && this.m_platform == null)
		{
			base.transform.parent = ((!SpawnManager.HasInstance) ? null : SpawnManager.Instance.VFX);
		}
		if (base.sprite)
		{
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060084DF RID: 34015 RVA: 0x0036C660 File Offset: 0x0036A860
	protected void OnBecameGrounded()
	{
		this.isStatic = true;
		if (this.detachedParticleSystems != null && this.detachedParticleSystems.Count > 0)
		{
			for (int i = 0; i < this.detachedParticleSystems.Count; i++)
			{
				if (this.detachedParticleSystems[i])
				{
					BraveUtility.EnableEmission(this.detachedParticleSystems[i], false);
				}
			}
		}
		if (base.GetComponent<BlackHoleDoer>() == null)
		{
			GunParticleSystemController gunParticleSystemController = null;
			if (this.IsPickupObject)
			{
				gunParticleSystemController = base.GetComponentInChildren<GunParticleSystemController>();
			}
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (!gunParticleSystemController || !(gunParticleSystemController.TargetSystem == componentsInChildren[j]))
					{
						componentsInChildren[j].Stop();
						UnityEngine.Object.Destroy(componentsInChildren[j]);
					}
				}
			}
		}
		if (!this.onGround && !string.IsNullOrEmpty(this.audioEventName))
		{
			AkSoundEngine.PostEvent(this.audioEventName, base.gameObject);
		}
		this.onGround = true;
		if (this.shouldUseSRBMotion)
		{
			base.specRigidbody.Velocity = Vector2.zero;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		}
		base.sprite.attachParent = null;
		if (!this.m_isPickupObject)
		{
			base.sprite.IsPerpendicular = false;
			base.sprite.HeightOffGround = this.m_finalWorldDepth;
			base.sprite.SortingOrder = 0;
		}
		else if (this.m_forceUseFinalDepth)
		{
			base.sprite.HeightOffGround = this.m_finalWorldDepth;
		}
		base.sprite.UpdateZDepth();
		if (this.changesCollisionLayer && base.specRigidbody != null)
		{
			base.specRigidbody.PrimaryPixelCollider.CollisionLayer = this.groundedCollisionLayer;
			base.specRigidbody.ForceRegenerate(null, null);
		}
		if (this.breaksOnFall && UnityEngine.Random.value < this.breakOnFallChance)
		{
			MinorBreakable component = base.GetComponent<MinorBreakable>();
			if (component != null)
			{
				component.heightOffGround = 0.05f;
				component.Break(this.m_velocity.XY().normalized * 1.5f);
			}
		}
		if (this.DoesGoopOnRest)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.AssignedGoop).AddGoopCircle(base.sprite.WorldCenter, this.GoopRadius, -1, false, -1);
		}
		if (this.removeSRBOnGrounded)
		{
			this.shouldUseSRBMotion = false;
			if (base.specRigidbody != null)
			{
				base.specRigidbody.enabled = false;
				UnityEngine.Object.Destroy(base.specRigidbody);
			}
		}
		if (this.m_platform != null)
		{
			base.transform.parent = this.m_platform.transform;
		}
		if (this.OnGrounded != null)
		{
			this.OnGrounded(this);
		}
		switch (this.followupBehavior)
		{
		case DebrisObject.DebrisFollowupAction.GroundedAnimation:
			if (!string.IsNullOrEmpty(this.followupIdentifier))
			{
				base.spriteAnimator.Play(this.followupIdentifier);
			}
			break;
		case DebrisObject.DebrisFollowupAction.GroundedSprite:
			if (!string.IsNullOrEmpty(this.followupIdentifier))
			{
				base.sprite.SetSprite(this.followupIdentifier);
			}
			break;
		case DebrisObject.DebrisFollowupAction.StopAnimationOnGrounded:
			base.spriteAnimator.Stop();
			break;
		}
		if (this.m_isPickupObject)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			GameObject gameObject = GameObject.FindGameObjectWithTag("SellCellController");
			SellCellController sellCellController = null;
			if (gameObject != null)
			{
				sellCellController = gameObject.GetComponent<SellCellController>();
			}
			PickupObject componentInChildren = base.GetComponentInChildren<PickupObject>();
			if (sellCellController != null)
			{
				RoomHandler absoluteRoomFromPosition2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(sellCellController.transform.position.IntXY(VectorConversions.Round));
				if (absoluteRoomFromPosition == absoluteRoomFromPosition2)
				{
					sellCellController.AttemptSellItem(componentInChildren);
				}
			}
		}
		else if (this.Priority > EphemeralObject.EphemeralPriority.Middling && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES)
		{
			base.sprite.HeightOffGround = base.sprite.HeightOffGround - UnityEngine.Random.Range(0f, 20f);
		}
	}

	// Token: 0x060084E0 RID: 34016 RVA: 0x0036CAF8 File Offset: 0x0036ACF8
	public void FadeToOverrideColor(Color targetColor, float duration, float startAlpha = 0f)
	{
		base.StartCoroutine(this.HandleOverrideColorFade(targetColor, duration, startAlpha));
	}

	// Token: 0x060084E1 RID: 34017 RVA: 0x0036CB0C File Offset: 0x0036AD0C
	private IEnumerator HandleOverrideColorFade(Color targetColor, float duration, float startAlpha = 0f)
	{
		if (!this.m_renderer)
		{
			yield break;
		}
		Color startColor = new Color(targetColor.r, targetColor.g, targetColor.b, startAlpha);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
			Color current = Color.Lerp(startColor, targetColor, t);
			this.m_renderer.material.SetColor("_OverrideColor", current);
			yield return null;
		}
		this.m_renderer.material.SetColor("_OverrideColor", targetColor);
		yield break;
	}

	// Token: 0x060084E2 RID: 34018 RVA: 0x0036CB3C File Offset: 0x0036AD3C
	protected void FallIntoPit(DebrisObject.PitFallPoint[] nextCells = null)
	{
		if (this.isFalling)
		{
			return;
		}
		this.isFalling = true;
		if (this.animatePitFall)
		{
			this.StartAnimatedPitFall(nextCells, this.m_velocity);
		}
		else
		{
			if (this.m_renderer)
			{
				DepthLookupManager.AssignRendererToSortingLayer(this.m_renderer, DepthLookupManager.GungeonSortingLayer.BACKGROUND);
				this.m_renderer.sortingOrder = 0;
				this.m_renderer.material.renderQueue = 2450;
			}
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
			this.additionalHeightBoost = GameManager.PIT_DEPTH;
			if (base.sprite)
			{
				base.sprite.HeightOffGround = GameManager.PIT_DEPTH;
				base.sprite.usesOverrideMaterial = true;
				base.sprite.IsPerpendicular = false;
				base.sprite.UpdateZDepth();
			}
			if (this.m_renderer)
			{
				this.m_renderer.material.shader = ShaderCache.Acquire("Brave/DebrisPitfallShader");
			}
			float num = 0.25f;
			if (GameManager.Instance.IsFoyer)
			{
				num = 2f;
			}
			else if (GameManager.Instance.Dungeon.IsEndTimes)
			{
				num = 5f;
			}
			if (base.sprite && GameManager.Instance.Dungeon.tileIndices.PitAtPositionIsWater(base.sprite.WorldCenter))
			{
				base.StartCoroutine(this.HandleSplashDeath());
			}
			else
			{
				this.FadeToOverrideColor(Color.black, num, 0.5f);
				if (base.GetComponent<NPCCellKeyItem>())
				{
					base.GetComponent<NPCCellKeyItem>().IsBeingDestroyed = true;
				}
				this.MaybeRespawnIfImportant();
				UnityEngine.Object.Destroy(base.gameObject, num + 0.1f);
			}
		}
	}

	// Token: 0x060084E3 RID: 34019 RVA: 0x0036CD10 File Offset: 0x0036AF10
	private IEnumerator HandleSplashDeath()
	{
		float fadeTime = 0.2f;
		this.FadeToOverrideColor(Color.black, fadeTime, 0.5f);
		this.MaybeRespawnIfImportant();
		yield return new WaitForSeconds(fadeTime);
		if (base.sprite && (this.IsCorpse || UnityEngine.Random.value < 0.25f))
		{
			GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(base.sprite.WorldCenter);
		}
		if (this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x060084E4 RID: 34020 RVA: 0x0036CD2C File Offset: 0x0036AF2C
	private void StartAnimatedPitFall(DebrisObject.PitFallPoint[] nextCells, Vector2 velocity)
	{
		List<IntVector2> list = new List<IntVector2>();
		if (nextCells != null && this.accurateDebris)
		{
			if (nextCells[0].inPit && nextCells[1].inPit && nextCells[4].inPit)
			{
				list.Add(IntVector2.Left);
			}
			else if (nextCells[0].inPit && nextCells[2].inPit && nextCells[3].inPit)
			{
				list.Add(IntVector2.Right);
			}
			else if (nextCells[0].inPit && nextCells[3].inPit && nextCells[4].inPit)
			{
				list.Add(IntVector2.Up);
			}
			else if (nextCells[0].inPit && nextCells[1].inPit && nextCells[2].inPit)
			{
				list.Add(IntVector2.Down);
			}
		}
		if (list.Count == 0)
		{
			list.Add(BraveUtility.GetIntMajorAxis(velocity));
		}
		IntVector2 intVector;
		if (list.Contains(BraveUtility.GetIntMajorAxis(velocity)))
		{
			intVector = BraveUtility.GetIntMajorAxis(velocity);
		}
		else
		{
			intVector = list[0];
		}
		base.StartCoroutine(this.StartFallAnimation(intVector, velocity));
	}

	// Token: 0x060084E5 RID: 34021 RVA: 0x0036CE74 File Offset: 0x0036B074
	private IEnumerator StartFallAnimation(IntVector2 dir, Vector2 debrisVelocity)
	{
		this.isPitFalling = false;
		if (base.sprite)
		{
			base.sprite.HeightOffGround = GameManager.PIT_DEPTH;
			base.sprite.UpdateZDepth();
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
		float duration = 0.5f;
		float initialRotation = base.transform.eulerAngles.z;
		float rotation = ((dir.x == 0) ? 0f : (-Mathf.Sign((float)dir.x) * 135f));
		Vector3 fallVelocity = dir.ToVector3() * 1.25f / duration;
		Vector3 acceleration = new Vector3(0f, -10f);
		Vector3 velocity = debrisVelocity;
		if (Mathf.Sign(fallVelocity.x) != Mathf.Sign(debrisVelocity.x) || Mathf.Abs(fallVelocity.x) > Mathf.Abs(debrisVelocity.x))
		{
			velocity.x = fallVelocity.x;
		}
		if (Mathf.Sign(fallVelocity.y) != Mathf.Sign(debrisVelocity.y) || Mathf.Abs(fallVelocity.y) > Mathf.Abs(debrisVelocity.y))
		{
			velocity.y = fallVelocity.y;
		}
		velocity.z = 0f;
		Vector3 cachedVector = base.sprite.transform.position;
		base.transform.position = base.sprite.WorldCenter;
		base.sprite.transform.position = cachedVector;
		float timer = 0f;
		while (timer < duration)
		{
			base.transform.position += velocity * BraveTime.DeltaTime;
			base.transform.eulerAngles = base.transform.eulerAngles.WithZ(initialRotation + Mathf.Lerp(0f, rotation, timer / duration));
			base.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, timer / duration);
			base.sprite.UpdateZDepth();
			yield return null;
			base.sprite.UpdateZDepth();
			timer += BraveTime.DeltaTime;
			velocity += acceleration * BraveTime.DeltaTime;
		}
		if (this.pitFallSplash)
		{
			GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(base.transform.position);
		}
		this.MaybeRespawnIfImportant();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060084E6 RID: 34022 RVA: 0x0036CEA0 File Offset: 0x0036B0A0
	private bool IsVitalPickup()
	{
		if (this && this.IsPickupObject)
		{
			PickupObject componentInChildren = base.GetComponentInChildren<PickupObject>();
			if (componentInChildren)
			{
				if (componentInChildren is CurrencyPickup && (componentInChildren as CurrencyPickup).IsMetaCurrency)
				{
					return true;
				}
				if (componentInChildren is NPCCellKeyItem)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060084E7 RID: 34023 RVA: 0x0036CF00 File Offset: 0x0036B100
	public void ForceDestroyAndMaybeRespawn()
	{
		this.MaybeRespawnIfImportant();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060084E8 RID: 34024 RVA: 0x0036CF14 File Offset: 0x0036B114
	private void MaybeRespawnIfImportant()
	{
		if (this && this.IsPickupObject)
		{
			PickupObject componentInChildren = base.GetComponentInChildren<PickupObject>();
			if (componentInChildren && componentInChildren.RespawnsIfPitfall)
			{
				bool flag = false;
				if (componentInChildren is CurrencyPickup)
				{
					(componentInChildren as CurrencyPickup).ForceSetPickedUp();
					List<RewardPedestal> componentsAbsoluteInRoom = base.transform.position.GetAbsoluteRoom().GetComponentsAbsoluteInRoom<RewardPedestal>();
					if ((componentInChildren as CurrencyPickup).IsMetaCurrency && componentsAbsoluteInRoom != null && componentsAbsoluteInRoom.Count > 0)
					{
						flag = true;
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(componentInChildren.PickupObjectId).gameObject, (componentsAbsoluteInRoom[0].specRigidbody.UnitCenter + UnityEngine.Random.insideUnitCircle.normalized * 3f).ToVector3ZisY(0f), Vector2.zero, 0f, true, true, false);
					}
				}
				if (!flag)
				{
					PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
					if (bestActivePlayer)
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(componentInChildren.PickupObjectId).gameObject, bestActivePlayer.CenterPosition.ToVector3ZUp(0f), Vector2.zero, 0f, true, !(componentInChildren is CurrencyPickup), false);
					}
					else
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(componentInChildren.PickupObjectId).gameObject, base.transform.position.GetAbsoluteRoom().GetCenteredVisibleClearSpot(2, 2).ToVector3(), Vector2.zero, 0f, true, !(componentInChildren is CurrencyPickup), false);
					}
				}
			}
		}
	}

	// Token: 0x04008865 RID: 34917
	public static List<SpeculativeRigidbody> SRB_Walls = new List<SpeculativeRigidbody>();

	// Token: 0x04008866 RID: 34918
	public static List<SpeculativeRigidbody> SRB_Pits = new List<SpeculativeRigidbody>();

	// Token: 0x04008867 RID: 34919
	private float ACCURATE_DEBRIS_THRESHOLD = 0.25f;

	// Token: 0x04008868 RID: 34920
	public string audioEventName;

	// Token: 0x04008869 RID: 34921
	[NonSerialized]
	public bool IsCorpse;

	// Token: 0x0400886A RID: 34922
	public bool playAnimationOnTrigger;

	// Token: 0x0400886B RID: 34923
	public bool usesDirectionalFallAnimations;

	// Token: 0x0400886C RID: 34924
	[ShowInInspectorIf("usesDirectionalFallAnimations", false)]
	public DebrisDirectionalAnimationInfo directionalAnimationData;

	// Token: 0x0400886D RID: 34925
	public bool breaksOnFall = true;

	// Token: 0x0400886E RID: 34926
	[ShowInInspectorIf("breaksOnFall", false)]
	public float breakOnFallChance = 1f;

	// Token: 0x0400886F RID: 34927
	public bool changesCollisionLayer;

	// Token: 0x04008870 RID: 34928
	public CollisionLayer groundedCollisionLayer = CollisionLayer.LowObstacle;

	// Token: 0x04008871 RID: 34929
	public DebrisObject.DebrisFollowupAction followupBehavior;

	// Token: 0x04008872 RID: 34930
	public string followupIdentifier;

	// Token: 0x04008873 RID: 34931
	public bool collisionStopsBullets;

	// Token: 0x04008874 RID: 34932
	public bool animatePitFall;

	// Token: 0x04008875 RID: 34933
	public bool pitFallSplash;

	// Token: 0x04008876 RID: 34934
	public float inertialMass = 1f;

	// Token: 0x04008877 RID: 34935
	public float motionMultiplier = 1f;

	// Token: 0x04008878 RID: 34936
	public bool canRotate = true;

	// Token: 0x04008879 RID: 34937
	public float angularVelocity = 360f;

	// Token: 0x0400887A RID: 34938
	public float angularVelocityVariance;

	// Token: 0x0400887B RID: 34939
	public int bounceCount = 1;

	// Token: 0x0400887C RID: 34940
	public float additionalBounceEnglish;

	// Token: 0x0400887D RID: 34941
	public float decayOnBounce = 0.5f;

	// Token: 0x0400887E RID: 34942
	public GameObject optionalBounceVFX;

	// Token: 0x0400887F RID: 34943
	public tk2dSprite shadowSprite;

	// Token: 0x04008880 RID: 34944
	[HideInInspector]
	public bool killTranslationOnBounce;

	// Token: 0x04008881 RID: 34945
	public Action<DebrisObject> OnTouchedGround;

	// Token: 0x04008882 RID: 34946
	public Action<DebrisObject> OnBounced;

	// Token: 0x04008883 RID: 34947
	public Action<DebrisObject> OnGrounded;

	// Token: 0x04008884 RID: 34948
	public bool usesLifespan;

	// Token: 0x04008885 RID: 34949
	public float lifespanMin = 1f;

	// Token: 0x04008886 RID: 34950
	public float lifespanMax = 1f;

	// Token: 0x04008887 RID: 34951
	public bool shouldUseSRBMotion;

	// Token: 0x04008888 RID: 34952
	public bool removeSRBOnGrounded;

	// Token: 0x04008889 RID: 34953
	[NonSerialized]
	public bool PreventFallingInPits;

	// Token: 0x0400888A RID: 34954
	public DebrisObject.DebrisPlacementOptions placementOptions;

	// Token: 0x0400888B RID: 34955
	public bool DoesGoopOnRest;

	// Token: 0x0400888C RID: 34956
	[ShowInInspectorIf("DoesGoopOnRest", false)]
	public GoopDefinition AssignedGoop;

	// Token: 0x0400888D RID: 34957
	[ShowInInspectorIf("DoesGoopOnRest", false)]
	public float GoopRadius = 1f;

	// Token: 0x0400888E RID: 34958
	[HideInInspector]
	public MinorBreakableGroupManager groupManager;

	// Token: 0x0400888F RID: 34959
	[HideInInspector]
	public float additionalHeightBoost;

	// Token: 0x04008890 RID: 34960
	[HideInInspector]
	public List<ParticleSystem> detachedParticleSystems;

	// Token: 0x04008892 RID: 34962
	public Action OnTriggered;

	// Token: 0x04008893 RID: 34963
	protected Bounds m_spriteBounds;

	// Token: 0x04008894 RID: 34964
	protected float m_currentLifespan;

	// Token: 0x04008895 RID: 34965
	protected float m_initialWorldDepth;

	// Token: 0x04008896 RID: 34966
	[SerializeField]
	protected float m_finalWorldDepth = -1.5f;

	// Token: 0x04008897 RID: 34967
	protected float m_startingHeightOffGround;

	// Token: 0x04008898 RID: 34968
	protected bool m_hasBeenTriggered;

	// Token: 0x04008899 RID: 34969
	protected bool isStatic = true;

	// Token: 0x0400889A RID: 34970
	protected bool doesDecay;

	// Token: 0x0400889B RID: 34971
	protected Vector3 m_startPosition;

	// Token: 0x0400889C RID: 34972
	protected Vector3 m_velocity;

	// Token: 0x0400889D RID: 34973
	protected Vector3 m_frameVelocity;

	// Token: 0x0400889E RID: 34974
	protected Vector3 m_currentPosition;

	// Token: 0x0400889F RID: 34975
	protected static DebrisObject.PitFallPoint[] m_STATIC_PitfallPoints;

	// Token: 0x040088A0 RID: 34976
	protected Transform m_transform;

	// Token: 0x040088A1 RID: 34977
	protected Renderer m_renderer;

	// Token: 0x040088A2 RID: 34978
	protected bool onGround;

	// Token: 0x040088A3 RID: 34979
	protected bool isFalling;

	// Token: 0x040088A4 RID: 34980
	protected bool isPitFalling;

	// Token: 0x040088A5 RID: 34981
	[NonSerialized]
	public bool PreventAbsorption;

	// Token: 0x040088A6 RID: 34982
	protected bool m_isPickupObject;

	// Token: 0x040088A7 RID: 34983
	protected bool m_forceUseFinalDepth;

	// Token: 0x040088A8 RID: 34984
	protected bool accurateDebris;

	// Token: 0x040088A9 RID: 34985
	protected bool m_recentlyBouncedOffTopwall;

	// Token: 0x040088AA RID: 34986
	protected bool m_wasFacewallFixed;

	// Token: 0x040088AB RID: 34987
	protected bool m_collisionsInitialized;

	// Token: 0x040088AC RID: 34988
	protected bool m_forceCheckGrounded;

	// Token: 0x040088AD RID: 34989
	protected bool m_isOnScreen = true;

	// Token: 0x040088AE RID: 34990
	protected Dungeon m_dungeonRef;

	// Token: 0x040088AF RID: 34991
	public bool ForceUpdateIfDisabled;

	// Token: 0x040088B1 RID: 34993
	private static int fgNonsenseLayerID = -1;

	// Token: 0x040088B2 RID: 34994
	private SpeculativeRigidbody m_platform;

	// Token: 0x02001637 RID: 5687
	[Serializable]
	public struct DebrisPlacementOptions
	{
		// Token: 0x040088B3 RID: 34995
		public bool canBeRotated;

		// Token: 0x040088B4 RID: 34996
		public bool canBeFlippedHorizontally;

		// Token: 0x040088B5 RID: 34997
		public bool canBeFlippedVertically;
	}

	// Token: 0x02001638 RID: 5688
	public enum DebrisFollowupAction
	{
		// Token: 0x040088B7 RID: 34999
		None,
		// Token: 0x040088B8 RID: 35000
		FollowupAnimation,
		// Token: 0x040088B9 RID: 35001
		GroundedAnimation,
		// Token: 0x040088BA RID: 35002
		GroundedSprite,
		// Token: 0x040088BB RID: 35003
		StopAnimationOnGrounded
	}

	// Token: 0x02001639 RID: 5689
	protected class PitFallPoint
	{
		// Token: 0x060084EA RID: 34026 RVA: 0x0036D0CC File Offset: 0x0036B2CC
		public PitFallPoint(CellData cellData, Vector3 position)
		{
			this.cellData = cellData;
			this.position = position;
		}

		// Token: 0x040088BC RID: 35004
		public CellData cellData;

		// Token: 0x040088BD RID: 35005
		public Vector3 position;

		// Token: 0x040088BE RID: 35006
		public bool inPit;
	}
}
