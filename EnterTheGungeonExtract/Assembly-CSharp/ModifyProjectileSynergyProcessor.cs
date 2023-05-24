using System;
using UnityEngine;

// Token: 0x02001700 RID: 5888
public class ModifyProjectileSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088DD RID: 35037 RVA: 0x0038BE1C File Offset: 0x0038A01C
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		if (this.Dejams)
		{
			Projectile projectile = this.m_projectile;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.DejamEnemy));
		}
		if (this.Blanks)
		{
			this.m_projectile.OnDestruction += this.DoBlank;
		}
	}

	// Token: 0x060088DE RID: 35038 RVA: 0x0038BE8C File Offset: 0x0038A08C
	private void DoBlank(Projectile obj)
	{
		if (this.m_projectile && this.m_projectile.Owner is PlayerController)
		{
			PlayerController playerController = this.m_projectile.Owner as PlayerController;
			PlayerController playerController2 = playerController;
			Vector2? vector = new Vector2?(this.m_projectile.specRigidbody.UnitCenter);
			playerController2.ForceBlank(25f, 0.5f, false, true, vector, true, -1f);
		}
	}

	// Token: 0x060088DF RID: 35039 RVA: 0x0038BF00 File Offset: 0x0038A100
	private void DejamEnemy(Projectile source, SpeculativeRigidbody target, bool kill)
	{
		if (target && target.aiActor && target.aiActor.IsBlackPhantom)
		{
			target.aiActor.UnbecomeBlackPhantom();
		}
	}

	// Token: 0x060088E0 RID: 35040 RVA: 0x0038BF38 File Offset: 0x0038A138
	private void Start()
	{
		PlayerController playerController = this.m_projectile.Owner as PlayerController;
		if (playerController && playerController.HasActiveBonusSynergy(this.SynergyToCheck, false))
		{
			if (this.TintsBullets)
			{
				this.m_projectile.AdjustPlayerProjectileTint(this.BulletTint, 0, 0f);
			}
			if (this.IncreaseSpawnedProjectileCount)
			{
				SpawnProjModifier component = this.m_projectile.GetComponent<SpawnProjModifier>();
				component.numToSpawnInFlight = (int)((float)component.numToSpawnInFlight * this.SpawnedProjectileCountMultiplier);
				component.numberToSpawnOnCollison = (int)((float)component.numberToSpawnOnCollison * this.SpawnedProjectileCountMultiplier);
			}
			if (this.IncreasesSpawnProjectileRate)
			{
				SpawnProjModifier component2 = this.m_projectile.GetComponent<SpawnProjModifier>();
				component2.inFlightSpawnCooldown *= this.SpawnProjectileRateMultiplier;
			}
			if (this.AddsSpawnedProjectileInFlight)
			{
				SpawnProjModifier spawnProjModifier = this.m_projectile.GetComponent<SpawnProjModifier>();
				if (!spawnProjModifier)
				{
					spawnProjModifier = this.m_projectile.gameObject.AddComponent<SpawnProjModifier>();
					spawnProjModifier.spawnProjectilesInFlight = true;
					spawnProjModifier.projectileToSpawnInFlight = this.AddFlightSpawnedProjectile;
					spawnProjModifier.numToSpawnInFlight = 1;
					spawnProjModifier.inFlightSpawnCooldown = this.InFlightSpawnCooldown;
					spawnProjModifier.inFlightAimAtEnemies = true;
					spawnProjModifier.spawnAudioEvent = this.InFlightAudioEvent;
				}
			}
			if (this.AddsSpawnedProjectileOnDeath)
			{
				SpawnProjModifier spawnProjModifier2 = this.m_projectile.GetComponent<SpawnProjModifier>();
				if (!spawnProjModifier2)
				{
					spawnProjModifier2 = this.m_projectile.gameObject.AddComponent<SpawnProjModifier>();
					spawnProjModifier2.spawnProjectilesOnCollision = !this.OnlySpawnDeathProjectilesInAir;
					spawnProjModifier2.spawnProjecitlesOnDieInAir = true;
					spawnProjModifier2.projectileToSpawnOnCollision = this.AddDeathSpawnedProjectile;
					if (this.OnlySpawnDeathProjectilesInAir)
					{
						spawnProjModifier2.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
					}
					if (this.NumDeathSpawnProjectiles == 1)
					{
						spawnProjModifier2.alignToSurfaceNormal = true;
					}
					spawnProjModifier2.numberToSpawnOnCollison = this.NumDeathSpawnProjectiles;
				}
			}
			if (this.AddsFire && (!this.m_projectile.AppliesFire || this.OverridesPreviousEffects))
			{
				this.m_projectile.AppliesFire = true;
				this.m_projectile.fireEffect = this.FireEffect;
			}
			if (this.AddsPoison && (!this.m_projectile.AppliesPoison || this.OverridesPreviousEffects))
			{
				this.m_projectile.AppliesPoison = true;
				this.m_projectile.healthEffect = this.PoisonEffect;
			}
			if (this.AddsFreeze && (!this.m_projectile.AppliesFreeze || this.OverridesPreviousEffects))
			{
				this.m_projectile.AppliesFreeze = true;
				this.m_projectile.freezeEffect = this.FreezeEffect;
			}
			if (this.AddsSlow && (!this.m_projectile.AppliesSpeedModifier || this.OverridesPreviousEffects))
			{
				this.m_projectile.AppliesSpeedModifier = true;
				this.m_projectile.speedEffect = this.SpeedEffect;
			}
			if (this.AddsExplosion)
			{
				ExplosiveModifier explosiveModifier = this.m_projectile.GetComponent<ExplosiveModifier>();
				if (!explosiveModifier)
				{
					explosiveModifier = this.m_projectile.gameObject.AddComponent<ExplosiveModifier>();
					explosiveModifier.explosionData = this.Explosion;
				}
			}
			if (this.AddsHoming)
			{
				if (this.HomingIsLockOn)
				{
					LockOnHomingModifier lockOnHomingModifier = this.m_projectile.GetComponent<LockOnHomingModifier>();
					if (!lockOnHomingModifier)
					{
						lockOnHomingModifier = this.m_projectile.gameObject.AddComponent<LockOnHomingModifier>();
						lockOnHomingModifier.HomingRadius = 0f;
						lockOnHomingModifier.AngularVelocity = 0f;
					}
					lockOnHomingModifier.HomingRadius += this.HomingRadius;
					lockOnHomingModifier.AngularVelocity += this.HomingAngularVelocity;
					lockOnHomingModifier.LockOnVFX = this.LockOnVFX;
				}
				else
				{
					HomingModifier homingModifier = this.m_projectile.GetComponent<HomingModifier>();
					if (!homingModifier)
					{
						homingModifier = this.m_projectile.gameObject.AddComponent<HomingModifier>();
						homingModifier.HomingRadius = 0f;
						homingModifier.AngularVelocity = 0f;
					}
					homingModifier.HomingRadius += this.HomingRadius;
					homingModifier.AngularVelocity += this.HomingAngularVelocity;
				}
			}
			if (this.AddsBounces > 0)
			{
				BounceProjModifier orAddComponent = this.m_projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
				orAddComponent.numberOfBounces += this.AddsBounces;
			}
			if (this.AddsPierces > 0)
			{
				PierceProjModifier orAddComponent2 = this.m_projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
				orAddComponent2.penetration += this.AddsPierces;
			}
			if (this.CopiesDevolverModifier)
			{
				DevolverModifier devolverModifier = this.m_projectile.gameObject.AddComponent<DevolverModifier>();
				devolverModifier.chanceToDevolve = this.DevolverSourceModifier.chanceToDevolve;
				devolverModifier.DevolverHierarchy = this.DevolverSourceModifier.DevolverHierarchy;
				devolverModifier.EnemyGuidsToIgnore = this.DevolverSourceModifier.EnemyGuidsToIgnore;
			}
			if (this.AddsChainLightning)
			{
				ChainLightningModifier chainLightningModifier = this.m_projectile.GetComponent<ChainLightningModifier>();
				if (!chainLightningModifier)
				{
					chainLightningModifier = this.m_projectile.gameObject.AddComponent<ChainLightningModifier>();
					chainLightningModifier.LinkVFXPrefab = this.ChainLinkVFX;
					chainLightningModifier.maximumLinkDistance = 7f;
					chainLightningModifier.damagePerHit = 5f;
					chainLightningModifier.damageCooldown = 1f;
				}
			}
			if (this.BossDamageMultiplier != 1f)
			{
				this.m_projectile.BossDamageMultiplier *= this.BossDamageMultiplier;
			}
			if (this.DamageMultiplier != 1f)
			{
				this.m_projectile.baseData.damage *= this.DamageMultiplier;
			}
			if (this.RangeMultiplier != 1f)
			{
				this.m_projectile.baseData.range *= this.RangeMultiplier;
			}
			if (this.ScaleMultiplier != 1f)
			{
				this.m_projectile.RuntimeUpdateScale(this.ScaleMultiplier);
			}
			if (this.SpeedMultiplier != 1f)
			{
				this.m_projectile.baseData.speed *= this.SpeedMultiplier;
				this.m_projectile.UpdateSpeed();
			}
			if (this.AddsAccelCurve)
			{
				this.m_projectile.baseData.AccelerationCurve = this.AccelCurve;
				this.m_projectile.baseData.UsesCustomAccelerationCurve = true;
				this.m_projectile.baseData.CustomAccelerationCurveDuration = this.AccelCurveTime;
			}
			if (this.AddsTransmogrifyChance && !this.m_projectile.CanTransmogrify)
			{
				this.m_projectile.CanTransmogrify = true;
				this.m_projectile.ChanceToTransmogrify = this.TransmogrifyChance;
				this.m_projectile.TransmogrifyTargetGuids = this.TransmogrifyTargetGuids;
			}
			if (this.AddsStun && !this.m_projectile.AppliesStun)
			{
				this.m_projectile.AppliesStun = true;
				this.m_projectile.StunApplyChance = this.StunChance;
				this.m_projectile.AppliedStunDuration = this.StunDuration;
			}
		}
	}

	// Token: 0x04008E6B RID: 36459
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008E6C RID: 36460
	public bool TintsBullets;

	// Token: 0x04008E6D RID: 36461
	public Color BulletTint;

	// Token: 0x04008E6E RID: 36462
	[Header("Spawn Proj Modifiers")]
	public bool IncreaseSpawnedProjectileCount;

	// Token: 0x04008E6F RID: 36463
	[ShowInInspectorIf("IncreaseSpawnedProjectileCount", false)]
	public float SpawnedProjectileCountMultiplier = 2f;

	// Token: 0x04008E70 RID: 36464
	public bool IncreasesSpawnProjectileRate;

	// Token: 0x04008E71 RID: 36465
	[ShowInInspectorIf("IncreasesSpawnProjectileRate", false)]
	public float SpawnProjectileRateMultiplier = 1f;

	// Token: 0x04008E72 RID: 36466
	public bool AddsSpawnedProjectileInFlight;

	// Token: 0x04008E73 RID: 36467
	[ShowInInspectorIf("AddsSpawnedProjectileInFlight", false)]
	public Projectile AddFlightSpawnedProjectile;

	// Token: 0x04008E74 RID: 36468
	[ShowInInspectorIf("AddsSpawnedProjectileInFlight", false)]
	public float InFlightSpawnCooldown = 1f;

	// Token: 0x04008E75 RID: 36469
	[ShowInInspectorIf("AddsSpawnedProjectileInFlight", false)]
	public string InFlightAudioEvent;

	// Token: 0x04008E76 RID: 36470
	public bool AddsSpawnedProjectileOnDeath;

	// Token: 0x04008E77 RID: 36471
	[ShowInInspectorIf("AddsSpawnedProjectileOnDeath", false)]
	public Projectile AddDeathSpawnedProjectile;

	// Token: 0x04008E78 RID: 36472
	[ShowInInspectorIf("AddsSpawnedProjectileOnDeath", false)]
	public int NumDeathSpawnProjectiles;

	// Token: 0x04008E79 RID: 36473
	[ShowInInspectorIf("AddsSpawnedProjectileOnDeath", false)]
	public bool OnlySpawnDeathProjectilesInAir;

	// Token: 0x04008E7A RID: 36474
	[Header("Other Settings")]
	public int AddsBounces;

	// Token: 0x04008E7B RID: 36475
	public int AddsPierces;

	// Token: 0x04008E7C RID: 36476
	public bool AddsHoming;

	// Token: 0x04008E7D RID: 36477
	[ShowInInspectorIf("AddsHoming", false)]
	public float HomingRadius = 5f;

	// Token: 0x04008E7E RID: 36478
	[ShowInInspectorIf("AddsHoming", false)]
	public float HomingAngularVelocity = 360f;

	// Token: 0x04008E7F RID: 36479
	[ShowInInspectorIf("AddsHoming", false)]
	public bool HomingIsLockOn;

	// Token: 0x04008E80 RID: 36480
	[ShowInInspectorIf("HomingIsLockOn", false)]
	public GameObject LockOnVFX;

	// Token: 0x04008E81 RID: 36481
	public bool OverridesPreviousEffects;

	// Token: 0x04008E82 RID: 36482
	public bool AddsFire;

	// Token: 0x04008E83 RID: 36483
	public GameActorFireEffect FireEffect;

	// Token: 0x04008E84 RID: 36484
	public bool AddsPoison;

	// Token: 0x04008E85 RID: 36485
	public GameActorHealthEffect PoisonEffect;

	// Token: 0x04008E86 RID: 36486
	public bool AddsFreeze;

	// Token: 0x04008E87 RID: 36487
	public GameActorFreezeEffect FreezeEffect;

	// Token: 0x04008E88 RID: 36488
	public bool AddsSlow;

	// Token: 0x04008E89 RID: 36489
	public GameActorSpeedEffect SpeedEffect;

	// Token: 0x04008E8A RID: 36490
	public bool CopiesDevolverModifier;

	// Token: 0x04008E8B RID: 36491
	[ShowInInspectorIf("CopiesDevolverModifier", false)]
	public DevolverModifier DevolverSourceModifier;

	// Token: 0x04008E8C RID: 36492
	public bool AddsExplosion;

	// Token: 0x04008E8D RID: 36493
	public ExplosionData Explosion;

	// Token: 0x04008E8E RID: 36494
	public float BossDamageMultiplier = 1f;

	// Token: 0x04008E8F RID: 36495
	public float DamageMultiplier = 1f;

	// Token: 0x04008E90 RID: 36496
	public float RangeMultiplier = 1f;

	// Token: 0x04008E91 RID: 36497
	public float ScaleMultiplier = 1f;

	// Token: 0x04008E92 RID: 36498
	public float SpeedMultiplier = 1f;

	// Token: 0x04008E93 RID: 36499
	public bool AddsAccelCurve;

	// Token: 0x04008E94 RID: 36500
	public AnimationCurve AccelCurve;

	// Token: 0x04008E95 RID: 36501
	public float AccelCurveTime = 1f;

	// Token: 0x04008E96 RID: 36502
	public bool AddsChainLightning;

	// Token: 0x04008E97 RID: 36503
	[ShowInInspectorIf("AddsChainLightning", false)]
	public GameObject ChainLinkVFX;

	// Token: 0x04008E98 RID: 36504
	public bool AddsTransmogrifyChance;

	// Token: 0x04008E99 RID: 36505
	public float TransmogrifyChance;

	// Token: 0x04008E9A RID: 36506
	[EnemyIdentifier]
	public string[] TransmogrifyTargetGuids;

	// Token: 0x04008E9B RID: 36507
	public bool AddsStun;

	// Token: 0x04008E9C RID: 36508
	public float StunChance;

	// Token: 0x04008E9D RID: 36509
	public float StunDuration = 2f;

	// Token: 0x04008E9E RID: 36510
	public bool Dejams;

	// Token: 0x04008E9F RID: 36511
	public bool Blanks;

	// Token: 0x04008EA0 RID: 36512
	private Projectile m_projectile;
}
