using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011A2 RID: 4514
public class MajorBreakable : PersistentVFXManagerBehaviour
{
	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x06006476 RID: 25718 RVA: 0x0026EFFC File Offset: 0x0026D1FC
	// (set) Token: 0x06006477 RID: 25719 RVA: 0x0026F004 File Offset: 0x0026D204
	public bool ReportZeroDamage { get; set; }

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x06006478 RID: 25720 RVA: 0x0026F010 File Offset: 0x0026D210
	public bool IsDestroyed
	{
		get
		{
			return this.m_isBroken;
		}
	}

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x06006479 RID: 25721 RVA: 0x0026F018 File Offset: 0x0026D218
	public int NumHits
	{
		get
		{
			return this.m_numHits;
		}
	}

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x0600647A RID: 25722 RVA: 0x0026F020 File Offset: 0x0026D220
	// (set) Token: 0x0600647B RID: 25723 RVA: 0x0026F028 File Offset: 0x0026D228
	public float MinHitPointsFromNonExplosions { get; set; }

	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x0600647C RID: 25724 RVA: 0x0026F034 File Offset: 0x0026D234
	// (set) Token: 0x0600647D RID: 25725 RVA: 0x0026F03C File Offset: 0x0026D23C
	public float MaxHitPoints { get; set; }

	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x0600647E RID: 25726 RVA: 0x0026F048 File Offset: 0x0026D248
	public Vector2 CenterPoint
	{
		get
		{
			if (base.specRigidbody)
			{
				return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			if (base.sprite)
			{
				return base.sprite.WorldCenter;
			}
			return base.transform.position.XY();
		}
	}

	// Token: 0x0600647F RID: 25727 RVA: 0x0026F0A0 File Offset: 0x0026D2A0
	public void Awake()
	{
		StaticReferenceManager.AllMajorBreakables.Add(this);
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x0026F0B0 File Offset: 0x0026D2B0
	public void Start()
	{
		if (this.HandlePathBlocking)
		{
			this.m_occupiedCells = new OccupiedCells(base.specRigidbody, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)));
		}
		if (this.MaxHitPoints <= 0f)
		{
			this.MaxHitPoints = this.HitPoints;
		}
		if (this.ScaleWithEnemyHealth)
		{
			float baseLevelHealthModifier = AIActor.BaseLevelHealthModifier;
			this.HitPoints *= baseLevelHealthModifier;
			this.MaxHitPoints *= baseLevelHealthModifier;
		}
		if (this.GameActorMotionBreaks || this.PlayerRollingBreaks)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x0026F184 File Offset: 0x0026D384
	public void Update()
	{
		this.m_damageVfxTimer += BraveTime.DeltaTime;
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x0026F198 File Offset: 0x0026D398
	public float GetCurrentHealthPercentage()
	{
		return this.HitPoints / this.MaxHitPoints;
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x0026F1A8 File Offset: 0x0026D3A8
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllMajorBreakables.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x0026F1BC File Offset: 0x0026D3BC
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_isBroken)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.gameActor)
		{
			if (this.GameActorMotionBreaks)
			{
				this.Break(otherRigidbody.Velocity);
				PhysicsEngine.SkipCollision = true;
			}
			else if (this.PlayerRollingBreaks && otherRigidbody.gameActor is PlayerController && (otherRigidbody.gameActor as PlayerController).IsDodgeRolling)
			{
				this.Break(otherRigidbody.Velocity);
				PhysicsEngine.SkipCollision = true;
			}
		}
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x0026F25C File Offset: 0x0026D45C
	public void ApplyDamage(float damage, Vector2 sourceDirection, bool isSourceEnemy, bool isExplosion = false, bool ForceDamageOverride = false)
	{
		if (this.IsDestroyed)
		{
			return;
		}
		if (this.TemporarilyInvulnerable)
		{
			return;
		}
		if (!ForceDamageOverride && (this.OnlyExplosions || (this.IsSecretDoor && this.HitPoints <= 1f)) && !isExplosion)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		if (this.EnemyDamageOverride > 0 && isSourceEnemy)
		{
			damage = (float)this.EnemyDamageOverride;
		}
		float num = Mathf.Max(0f, damage - this.DamageReduction);
		if (this.IsSecretDoor && !ForceDamageOverride && this.HitPoints - num < 1f)
		{
			num = Mathf.Min(this.HitPoints - 1f, num);
		}
		if (this.MinHitPointsFromNonExplosions > 0f && !isExplosion)
		{
			num = Mathf.Min(this.HitPoints - this.MinHitPointsFromNonExplosions, num);
		}
		if (ForceDamageOverride)
		{
			num = damage;
		}
		if (num <= 0f)
		{
			if (this.ReportZeroDamage && this.OnDamaged != null)
			{
				this.OnDamaged(num);
			}
			return;
		}
		this.HitPoints -= num;
		this.m_numHits++;
		if (this.OnDamaged != null)
		{
			this.OnDamaged(num);
		}
		if (this.m_damageVfxTimer > this.damageVfxMinTimeBetween)
		{
			if (this.damageVfx != null)
			{
				VFXPool vfxpool = this.damageVfx;
				Vector3 vector = this.CenterPoint;
				Vector2? vector2 = new Vector2?(-sourceDirection);
				vfxpool.SpawnAtPosition(vector, 0f, null, null, vector2, null, false, null, null, false);
			}
			this.m_damageVfxTimer = 0f;
		}
		if (this.HitPoints <= 0f && this.m_numHits >= this.MinHits)
		{
			if (this.usesTemporaryZeroHitPointsState && !this.m_inZeroHPState)
			{
				this.m_inZeroHPState = true;
				string text = (string.IsNullOrEmpty(this.overrideSpriteNameToUseAtZeroHP) ? this.spriteNameToUseAtZeroHP : this.overrideSpriteNameToUseAtZeroHP);
				if (!string.IsNullOrEmpty(text))
				{
					base.sprite.SetSprite(text);
				}
			}
			else
			{
				this.Break(sourceDirection);
			}
		}
		else
		{
			if (this.handlesOwnPrebreakFrames)
			{
				for (int i = this.prebreakFrames.Length - 1; i >= 0; i--)
				{
					if (this.GetCurrentHealthPercentage() <= this.prebreakFrames[i].healthPercentage / 100f)
					{
						base.sprite.SetSprite(this.prebreakFrames[i].sprite);
						return;
					}
				}
			}
			if (this.playsAnimationOnNotBroken)
			{
				base.spriteAnimator.Play(this.notBreakAnimation);
			}
		}
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x0026F53C File Offset: 0x0026D73C
	public void Break(Vector2 sourceDirection)
	{
		if (this.m_isBroken)
		{
			return;
		}
		this.m_isBroken = true;
		base.TriggerPersistentVFXClear();
		if (this.OnBreak != null)
		{
			this.OnBreak();
		}
		if (this.spawnShards)
		{
			switch (this.shardBreakStyle)
			{
			case MinorBreakable.BreakStyle.CONE:
				this.SpawnShards(sourceDirection, -45f, 45f, 0.5f, sourceDirection.magnitude * this.minShardPercentSpeed, sourceDirection.magnitude * this.maxShardPercentSpeed);
				break;
			case MinorBreakable.BreakStyle.BURST:
				this.SpawnShards(sourceDirection, -180f, 180f, 0.5f, sourceDirection.magnitude * this.minShardPercentSpeed, sourceDirection.magnitude * this.maxShardPercentSpeed);
				break;
			case MinorBreakable.BreakStyle.JET:
				this.SpawnShards(sourceDirection, -15f, 15f, 0.5f, sourceDirection.magnitude * this.minShardPercentSpeed, sourceDirection.magnitude * this.maxShardPercentSpeed);
				break;
			case MinorBreakable.BreakStyle.WALL_DOWNWARD_BURST:
				this.SpawnShards(Vector2.down, -45f, 45f, 0.5f, sourceDirection.magnitude * this.minShardPercentSpeed, sourceDirection.magnitude * this.maxShardPercentSpeed);
				break;
			}
		}
		if (this.childrenToDestroy != null)
		{
			for (int i = 0; i < this.childrenToDestroy.Count; i++)
			{
				UnityEngine.Object.Destroy(this.childrenToDestroy[i]);
			}
		}
		if (this.breakVfx != null && !this.delayDamageVfx)
		{
			if (this.breakVfxParent)
			{
				this.breakVfx.SpawnAtLocalPosition(Vector3.zero, 0f, this.breakVfxParent.transform, null, null, false, null, false);
			}
			else
			{
				this.breakVfx.SpawnAtPosition(this.CenterPoint, 0f, null, null, null, null, false, null, null, false);
			}
		}
		if (this.HandlePathBlocking)
		{
			this.m_occupiedCells.Clear();
		}
		if (this.SpawnItemOnBreak)
		{
			PickupObject byId = PickupObjectDatabase.GetById(this.ItemIdToSpawnOnBreak);
			LootEngine.SpawnItem(byId.gameObject, base.sprite.WorldCenter, Vector2.zero, 1f, true, true, false);
		}
		if (this.destroyedOnBreak)
		{
			if (this.handlesOwnBreakAnimation)
			{
				if (this.breakVfx != null && this.breakVfx.type != VFXPoolType.None)
				{
					base.spriteAnimator.PlayAndDestroyObject(this.breakAnimation, delegate
					{
						if (this.breakVfxParent)
						{
							this.breakVfx.SpawnAtLocalPosition(Vector3.zero, 0f, this.breakVfxParent.transform, null, null, false, null, false);
						}
						else
						{
							this.breakVfx.SpawnAtPosition(this.CenterPoint, 0f, null, null, null, null, false, null, null, false);
						}
					});
				}
				else
				{
					base.spriteAnimator.PlayAndDestroyObject(this.breakAnimation, null);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (this.handlesOwnBreakAnimation)
		{
			base.spriteAnimator.Play(this.breakAnimation);
			base.specRigidbody.enabled = false;
		}
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x0026F858 File Offset: 0x0026DA58
	public void SpawnShards(Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude)
	{
		if (GameManager.Options.DebrisQuantity == GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			return;
		}
		Vector3 vector = base.sprite.GetBounds().extents + base.transform.position;
		if (this.shardClusters != null && this.shardClusters.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, 10);
			Bounds bounds = base.sprite.GetBounds();
			for (int i = 0; i < this.shardClusters.Length; i++)
			{
				float lowDiscrepancyRandom = BraveMathCollege.GetLowDiscrepancyRandom(num);
				num++;
				float num2 = Mathf.Lerp(minAngle, maxAngle, lowDiscrepancyRandom);
				ShardCluster shardCluster = this.shardClusters[i];
				int num3 = UnityEngine.Random.Range(shardCluster.minFromCluster, shardCluster.maxFromCluster + 1);
				int num4 = UnityEngine.Random.Range(0, shardCluster.clusterObjects.Length);
				for (int j = 0; j < num3; j++)
				{
					Vector3 vector2 = vector;
					if (this.distributeShards)
					{
						vector2 = base.sprite.transform.position + new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
					}
					Vector3 vector3 = Quaternion.Euler(0f, 0f, num2) * (direction.normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).ToVector3ZUp(verticalSpeed);
					if (this.shardBreakStyle == MinorBreakable.BreakStyle.BURST)
					{
						vector3 = ((vector2 - vector).normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).WithZ(verticalSpeed);
					}
					int num5 = (num4 + j) % shardCluster.clusterObjects.Length;
					GameObject gameObject = SpawnManager.SpawnDebris(shardCluster.clusterObjects[num5].gameObject, vector2, Quaternion.identity);
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (base.sprite.attachParent != null && component != null)
					{
						component.attachParent = base.sprite.attachParent;
						component.HeightOffGround = base.sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					component2.Trigger(vector3, 1f, 1f);
				}
			}
		}
	}

	// Token: 0x04005FF6 RID: 24566
	public float HitPoints = 100f;

	// Token: 0x04005FF7 RID: 24567
	public float DamageReduction;

	// Token: 0x04005FF8 RID: 24568
	public int MinHits;

	// Token: 0x04005FF9 RID: 24569
	public int EnemyDamageOverride = -1;

	// Token: 0x04005FFA RID: 24570
	public bool ImmuneToBeastMode;

	// Token: 0x04005FFB RID: 24571
	public bool ScaleWithEnemyHealth;

	// Token: 0x04005FFC RID: 24572
	public bool OnlyExplosions;

	// Token: 0x04005FFD RID: 24573
	public bool IgnoreExplosions;

	// Token: 0x04005FFE RID: 24574
	[NonSerialized]
	public bool IsSecretDoor;

	// Token: 0x04005FFF RID: 24575
	public bool GameActorMotionBreaks;

	// Token: 0x04006000 RID: 24576
	public bool PlayerRollingBreaks;

	// Token: 0x04006001 RID: 24577
	public bool spawnShards = true;

	// Token: 0x04006002 RID: 24578
	[ShowInInspectorIf("spawnShards", true)]
	public bool distributeShards;

	// Token: 0x04006003 RID: 24579
	public ShardCluster[] shardClusters;

	// Token: 0x04006004 RID: 24580
	[ShowInInspectorIf("spawnShards", true)]
	public float minShardPercentSpeed = 0.05f;

	// Token: 0x04006005 RID: 24581
	[ShowInInspectorIf("spawnShards", true)]
	public float maxShardPercentSpeed = 0.3f;

	// Token: 0x04006006 RID: 24582
	[ShowInInspectorIf("spawnShards", true)]
	public MinorBreakable.BreakStyle shardBreakStyle;

	// Token: 0x04006007 RID: 24583
	public bool usesTemporaryZeroHitPointsState;

	// Token: 0x04006008 RID: 24584
	[ShowInInspectorIf("usesTemporaryZeroHitPointsState", true)]
	public string spriteNameToUseAtZeroHP;

	// Token: 0x04006009 RID: 24585
	[NonSerialized]
	public string overrideSpriteNameToUseAtZeroHP;

	// Token: 0x0400600A RID: 24586
	public bool destroyedOnBreak;

	// Token: 0x0400600B RID: 24587
	public List<GameObject> childrenToDestroy;

	// Token: 0x0400600C RID: 24588
	public bool playsAnimationOnNotBroken;

	// Token: 0x0400600D RID: 24589
	[ShowInInspectorIf("playsAnimationOnNotBroken", true)]
	public string notBreakAnimation;

	// Token: 0x0400600E RID: 24590
	public bool handlesOwnBreakAnimation;

	// Token: 0x0400600F RID: 24591
	[ShowInInspectorIf("handlesOwnBreakAnimation", true)]
	public string breakAnimation;

	// Token: 0x04006010 RID: 24592
	public bool handlesOwnPrebreakFrames;

	// Token: 0x04006011 RID: 24593
	public BreakFrame[] prebreakFrames;

	// Token: 0x04006012 RID: 24594
	public VFXPool damageVfx;

	// Token: 0x04006013 RID: 24595
	[ShowInInspectorIf("damageVfx", true)]
	public float damageVfxMinTimeBetween = 0.2f;

	// Token: 0x04006014 RID: 24596
	public VFXPool breakVfx;

	// Token: 0x04006015 RID: 24597
	[ShowInInspectorIf("breakVfx", true)]
	public GameObject breakVfxParent;

	// Token: 0x04006016 RID: 24598
	[ShowInInspectorIf("breakVfx", true)]
	public bool delayDamageVfx;

	// Token: 0x04006017 RID: 24599
	public bool SpawnItemOnBreak;

	// Token: 0x04006018 RID: 24600
	[ShowInInspectorIf("SpawnItemOnBreak", false)]
	[PickupIdentifier]
	public int ItemIdToSpawnOnBreak = -1;

	// Token: 0x04006019 RID: 24601
	public bool HandlePathBlocking;

	// Token: 0x0400601A RID: 24602
	private OccupiedCells m_occupiedCells;

	// Token: 0x0400601B RID: 24603
	public Action OnBreak;

	// Token: 0x0400601C RID: 24604
	public Action<float> OnDamaged;

	// Token: 0x0400601E RID: 24606
	[NonSerialized]
	public bool InvulnerableToEnemyBullets;

	// Token: 0x0400601F RID: 24607
	[NonSerialized]
	public bool TemporarilyInvulnerable;

	// Token: 0x04006022 RID: 24610
	private bool m_inZeroHPState;

	// Token: 0x04006023 RID: 24611
	private bool m_isBroken;

	// Token: 0x04006024 RID: 24612
	private int m_numHits;

	// Token: 0x04006025 RID: 24613
	private float m_damageVfxTimer;
}
