using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001661 RID: 5729
[Serializable]
public class ProjectileModule
{
	// Token: 0x060085A3 RID: 34211 RVA: 0x00371E24 File Offset: 0x00370024
	public int GetModifiedNumberOfFinalProjectiles(GameActor owner)
	{
		if (owner && owner is PlayerController && this.numberOfFinalProjectiles > 0 && (owner as PlayerController).OnlyFinalProjectiles.Value)
		{
			return this.GetModNumberOfShotsInClip(owner);
		}
		return this.numberOfFinalProjectiles;
	}

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x060085A4 RID: 34212 RVA: 0x00371E78 File Offset: 0x00370078
	// (set) Token: 0x060085A5 RID: 34213 RVA: 0x00371E80 File Offset: 0x00370080
	public int CloneSourceIndex
	{
		get
		{
			return this.m_cloneSourceIndex;
		}
		set
		{
			this.m_cloneSourceIndex = value;
		}
	}

	// Token: 0x060085A6 RID: 34214 RVA: 0x00371E8C File Offset: 0x0037008C
	public int GetModNumberOfShotsInClip(GameActor owner)
	{
		if (this.numberOfShotsInClip == 1)
		{
			return this.numberOfShotsInClip;
		}
		if (!(owner != null) || !(owner is PlayerController))
		{
			return this.numberOfShotsInClip;
		}
		PlayerController playerController = owner as PlayerController;
		float statValue = playerController.stats.GetStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier);
		float statValue2 = playerController.stats.GetStatValue(PlayerStats.StatType.TarnisherClipCapacityMultiplier);
		int num = Mathf.FloorToInt((float)this.numberOfShotsInClip * statValue * statValue2);
		if (num < 0)
		{
			return num;
		}
		return Mathf.Max(num, 1);
	}

	// Token: 0x060085A7 RID: 34215 RVA: 0x00371F10 File Offset: 0x00370110
	public static ProjectileModule CreateClone(ProjectileModule source, bool inheritGuid = true, int sourceIndex = -1)
	{
		ProjectileModule projectileModule = new ProjectileModule();
		projectileModule.shootStyle = source.shootStyle;
		projectileModule.ammoType = source.ammoType;
		projectileModule.customAmmoType = source.customAmmoType;
		projectileModule.sequenceStyle = source.sequenceStyle;
		projectileModule.maxChargeTime = source.maxChargeTime;
		projectileModule.triggerCooldownForAnyChargeAmount = source.triggerCooldownForAnyChargeAmount;
		projectileModule.angleFromAim = source.angleFromAim;
		projectileModule.alternateAngle = source.alternateAngle;
		projectileModule.angleVariance = source.angleVariance;
		projectileModule.mirror = source.mirror;
		projectileModule.inverted = source.inverted;
		projectileModule.positionOffset = source.positionOffset;
		projectileModule.ammoCost = source.ammoCost;
		projectileModule.cooldownTime = source.cooldownTime;
		projectileModule.numberOfShotsInClip = source.numberOfShotsInClip;
		projectileModule.usesOptionalFinalProjectile = source.usesOptionalFinalProjectile;
		projectileModule.finalAmmoType = source.finalAmmoType;
		projectileModule.finalCustomAmmoType = source.finalCustomAmmoType;
		projectileModule.numberOfFinalProjectiles = source.numberOfFinalProjectiles;
		projectileModule.isFinalVolley = source.isFinalVolley;
		projectileModule.burstCooldownTime = source.burstCooldownTime;
		projectileModule.burstShotCount = source.burstShotCount;
		projectileModule.ignoredForReloadPurposes = source.ignoredForReloadPurposes;
		projectileModule.preventFiringDuringCharge = source.preventFiringDuringCharge;
		projectileModule.isExternalAddedModule = source.isExternalAddedModule;
		projectileModule.IsDuctTapeModule = source.IsDuctTapeModule;
		projectileModule.projectiles = new List<Projectile>();
		for (int i = 0; i < source.projectiles.Count; i++)
		{
			projectileModule.projectiles.Add(source.projectiles[i]);
		}
		projectileModule.chargeProjectiles = source.chargeProjectiles;
		projectileModule.finalProjectile = source.finalProjectile;
		projectileModule.finalVolley = source.finalVolley;
		projectileModule.orderedGroupCounts = source.orderedGroupCounts;
		if (sourceIndex >= 0)
		{
			projectileModule.CloneSourceIndex = sourceIndex;
		}
		if (inheritGuid && source.runtimeGuid != null)
		{
			projectileModule.runtimeGuid = source.runtimeGuid;
		}
		else
		{
			projectileModule.runtimeGuid = Guid.NewGuid().ToString();
		}
		return projectileModule;
	}

	// Token: 0x060085A8 RID: 34216 RVA: 0x00372114 File Offset: 0x00370314
	public void ClearOrderedProjectileData()
	{
		this.currentOrderedGroupNumber = 0;
		this.currentOrderedProjNumber = 0;
	}

	// Token: 0x060085A9 RID: 34217 RVA: 0x00372124 File Offset: 0x00370324
	public void ResetRuntimeData()
	{
		this.currentOrderedProjNumber = 0;
		this.currentOrderedGroupNumber = 0;
		if (string.IsNullOrEmpty(this.runtimeGuid))
		{
			this.runtimeGuid = Guid.NewGuid().ToString();
		}
	}

	// Token: 0x060085AA RID: 34218 RVA: 0x00372168 File Offset: 0x00370368
	public bool IsFinalShot(ModuleShootData runtimeData, GameActor owner)
	{
		return !runtimeData.needsReload && (this.isFinalVolley || (this.usesOptionalFinalProjectile && this.GetModNumberOfShotsInClip(owner) - this.GetModifiedNumberOfFinalProjectiles(owner) <= runtimeData.numberShotsFired));
	}

	// Token: 0x060085AB RID: 34219 RVA: 0x003721B8 File Offset: 0x003703B8
	public bool HasFinalVolleyOverride()
	{
		return this.usesOptionalFinalProjectile && this.finalVolley != null;
	}

	// Token: 0x060085AC RID: 34220 RVA: 0x003721D4 File Offset: 0x003703D4
	public Projectile GetCurrentProjectile(ModuleShootData runtimeData, GameActor owner)
	{
		if (this.usesOptionalFinalProjectile && this.GetModNumberOfShotsInClip(owner) - this.GetModifiedNumberOfFinalProjectiles(owner) <= runtimeData.numberShotsFired)
		{
			return this.finalProjectile;
		}
		if (this.sequenceStyle == ProjectileModule.ProjectileSequenceStyle.Ordered)
		{
			return this.projectiles[this.currentOrderedProjNumber];
		}
		if (this.sequenceStyle == ProjectileModule.ProjectileSequenceStyle.OrderedGroups)
		{
			int num = 0;
			for (int i = 0; i < this.currentOrderedGroupNumber; i++)
			{
				num += this.orderedGroupCounts[i];
			}
			int num2 = UnityEngine.Random.Range(num, num + this.orderedGroupCounts[this.currentOrderedGroupNumber]);
			this.currentOrderedGroupNumber = (this.currentOrderedGroupNumber + 1) % this.orderedGroupCounts.Count;
			return this.projectiles[num2];
		}
		return this.projectiles[UnityEngine.Random.Range(0, this.projectiles.Count)];
	}

	// Token: 0x060085AD RID: 34221 RVA: 0x003722BC File Offset: 0x003704BC
	public Projectile GetCurrentProjectile()
	{
		if (this.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			for (int i = 0; i < this.chargeProjectiles.Count; i++)
			{
				if (this.chargeProjectiles[i].Projectile)
				{
					Projectile projectile = this.chargeProjectiles[i].Projectile;
					projectile.pierceMinorBreakables = true;
					return projectile;
				}
			}
			return null;
		}
		if (this.sequenceStyle == ProjectileModule.ProjectileSequenceStyle.Ordered)
		{
			return this.projectiles[this.currentOrderedProjNumber];
		}
		if (this.sequenceStyle == ProjectileModule.ProjectileSequenceStyle.OrderedGroups)
		{
			int num = 0;
			for (int j = 0; j < this.currentOrderedGroupNumber; j++)
			{
				num += this.orderedGroupCounts[j];
			}
			int num2 = UnityEngine.Random.Range(num, this.orderedGroupCounts[this.currentOrderedGroupNumber]);
			this.currentOrderedGroupNumber = (this.currentOrderedGroupNumber + 1) % this.orderedGroupCounts.Count;
			return this.projectiles[num2];
		}
		return this.projectiles[UnityEngine.Random.Range(0, this.projectiles.Count)];
	}

	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x060085AE RID: 34222 RVA: 0x003723D8 File Offset: 0x003705D8
	public Vector3 InversePositionOffset
	{
		get
		{
			return new Vector3(this.positionOffset.x, -1f * this.positionOffset.y, this.positionOffset.z);
		}
	}

	// Token: 0x060085AF RID: 34223 RVA: 0x00372408 File Offset: 0x00370608
	public float GetEstimatedShotsPerSecond(float reloadTime)
	{
		if (this.cooldownTime <= 0f)
		{
			return 0f;
		}
		float num = this.cooldownTime;
		if (this.shootStyle == ProjectileModule.ShootStyle.Burst && this.burstShotCount > 1 && this.burstCooldownTime > 0f)
		{
			num = ((float)(this.burstShotCount - 1) * this.burstCooldownTime + this.cooldownTime) / (float)this.burstShotCount;
		}
		if (this.numberOfShotsInClip > 0)
		{
			num += reloadTime / (float)this.numberOfShotsInClip;
		}
		return 1f / num;
	}

	// Token: 0x060085B0 RID: 34224 RVA: 0x0037249C File Offset: 0x0037069C
	public void IncrementShootCount()
	{
		this.currentOrderedProjNumber = (this.currentOrderedProjNumber + 1) % this.projectiles.Count;
	}

	// Token: 0x060085B1 RID: 34225 RVA: 0x003724B8 File Offset: 0x003706B8
	public float GetAngleVariance(float varianceMultiplier = 1f)
	{
		float num = BraveMathCollege.GetLowDiscrepancyRandom(ProjectileModule.m_angleVarianceIterator) * (2f * this.angleVariance) - this.angleVariance;
		ProjectileModule.m_angleVarianceIterator++;
		return num * varianceMultiplier;
	}

	// Token: 0x060085B2 RID: 34226 RVA: 0x003724F4 File Offset: 0x003706F4
	public float GetAngleVariance(float customVariance, float varianceMultiplier)
	{
		float num = BraveMathCollege.GetLowDiscrepancyRandom(ProjectileModule.m_angleVarianceIterator) * (2f * customVariance) - customVariance;
		ProjectileModule.m_angleVarianceIterator++;
		return num * varianceMultiplier;
	}

	// Token: 0x060085B3 RID: 34227 RVA: 0x00372528 File Offset: 0x00370728
	public float GetAngleForShot(float alternateAngleSign = 1f, float varianceMultiplier = 1f, float? overrideAngleVariance = null)
	{
		float num = alternateAngleSign * this.angleFromAim;
		float num2 = ((overrideAngleVariance == null) ? this.GetAngleVariance(varianceMultiplier) : overrideAngleVariance.Value);
		return num + num2;
	}

	// Token: 0x060085B4 RID: 34228 RVA: 0x00372564 File Offset: 0x00370764
	public int ContainsFinalProjectile(Projectile testProj)
	{
		if (this.usesOptionalFinalProjectile)
		{
			if (this.finalVolley != null)
			{
				for (int i = 0; i < this.finalVolley.projectiles.Count; i++)
				{
					if (this.finalVolley.projectiles[i].projectiles.Contains(testProj))
					{
						return this.numberOfFinalProjectiles;
					}
				}
			}
			else if (this.finalProjectile == testProj)
			{
				return this.numberOfFinalProjectiles;
			}
		}
		return 0;
	}

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x060085B5 RID: 34229 RVA: 0x003725F4 File Offset: 0x003707F4
	public float LongestChargeTime
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < this.chargeProjectiles.Count; i++)
			{
				ProjectileModule.ChargeProjectile chargeProjectile = this.chargeProjectiles[i];
				num = Mathf.Max(num, chargeProjectile.ChargeTime);
			}
			return num;
		}
	}

	// Token: 0x060085B6 RID: 34230 RVA: 0x00372640 File Offset: 0x00370840
	public ProjectileModule.ChargeProjectile GetChargeProjectile(float chargeTime)
	{
		ProjectileModule.ChargeProjectile chargeProjectile = null;
		for (int i = 0; i < this.chargeProjectiles.Count; i++)
		{
			ProjectileModule.ChargeProjectile chargeProjectile2 = this.chargeProjectiles[i];
			if (chargeProjectile2.ChargeTime <= chargeTime && (chargeProjectile == null || chargeTime - chargeProjectile2.ChargeTime < chargeTime - chargeProjectile.ChargeTime))
			{
				chargeProjectile = chargeProjectile2;
			}
		}
		return chargeProjectile;
	}

	// Token: 0x040089E3 RID: 35299
	public ProjectileModule.ShootStyle shootStyle;

	// Token: 0x040089E4 RID: 35300
	public GameUIAmmoType.AmmoType ammoType;

	// Token: 0x040089E5 RID: 35301
	public string customAmmoType;

	// Token: 0x040089E6 RID: 35302
	public List<Projectile> projectiles = new List<Projectile>();

	// Token: 0x040089E7 RID: 35303
	public ProjectileModule.ProjectileSequenceStyle sequenceStyle;

	// Token: 0x040089E8 RID: 35304
	public List<int> orderedGroupCounts;

	// Token: 0x040089E9 RID: 35305
	public List<ProjectileModule.ChargeProjectile> chargeProjectiles = new List<ProjectileModule.ChargeProjectile>();

	// Token: 0x040089EA RID: 35306
	public float maxChargeTime;

	// Token: 0x040089EB RID: 35307
	public bool triggerCooldownForAnyChargeAmount;

	// Token: 0x040089EC RID: 35308
	public bool isFinalVolley;

	// Token: 0x040089ED RID: 35309
	public bool usesOptionalFinalProjectile;

	// Token: 0x040089EE RID: 35310
	public Projectile finalProjectile;

	// Token: 0x040089EF RID: 35311
	public ProjectileVolleyData finalVolley;

	// Token: 0x040089F0 RID: 35312
	public int numberOfFinalProjectiles = 1;

	// Token: 0x040089F1 RID: 35313
	public GameUIAmmoType.AmmoType finalAmmoType;

	// Token: 0x040089F2 RID: 35314
	public string finalCustomAmmoType;

	// Token: 0x040089F3 RID: 35315
	public float angleFromAim;

	// Token: 0x040089F4 RID: 35316
	public bool alternateAngle;

	// Token: 0x040089F5 RID: 35317
	public float angleVariance;

	// Token: 0x040089F6 RID: 35318
	public Vector3 positionOffset;

	// Token: 0x040089F7 RID: 35319
	public bool mirror;

	// Token: 0x040089F8 RID: 35320
	public bool inverted;

	// Token: 0x040089F9 RID: 35321
	public int ammoCost = 1;

	// Token: 0x040089FA RID: 35322
	public int burstShotCount = 3;

	// Token: 0x040089FB RID: 35323
	public float burstCooldownTime = 0.2f;

	// Token: 0x040089FC RID: 35324
	public float cooldownTime = 1f;

	// Token: 0x040089FD RID: 35325
	public int numberOfShotsInClip = -1;

	// Token: 0x040089FE RID: 35326
	public bool ignoredForReloadPurposes;

	// Token: 0x040089FF RID: 35327
	public bool preventFiringDuringCharge;

	// Token: 0x04008A00 RID: 35328
	[NonSerialized]
	public bool isExternalAddedModule;

	// Token: 0x04008A01 RID: 35329
	private int m_cloneSourceIndex = -1;

	// Token: 0x04008A02 RID: 35330
	[NonSerialized]
	public string runtimeGuid;

	// Token: 0x04008A03 RID: 35331
	[NonSerialized]
	public bool IsDuctTapeModule;

	// Token: 0x04008A04 RID: 35332
	private int currentOrderedProjNumber;

	// Token: 0x04008A05 RID: 35333
	private int currentOrderedGroupNumber;

	// Token: 0x04008A06 RID: 35334
	private static int m_angleVarianceIterator;

	// Token: 0x02001662 RID: 5730
	public enum ShootStyle
	{
		// Token: 0x04008A08 RID: 35336
		SemiAutomatic,
		// Token: 0x04008A09 RID: 35337
		Automatic,
		// Token: 0x04008A0A RID: 35338
		Beam,
		// Token: 0x04008A0B RID: 35339
		Charged,
		// Token: 0x04008A0C RID: 35340
		Burst
	}

	// Token: 0x02001663 RID: 5731
	public enum ProjectileSequenceStyle
	{
		// Token: 0x04008A0E RID: 35342
		Random,
		// Token: 0x04008A0F RID: 35343
		Ordered,
		// Token: 0x04008A10 RID: 35344
		OrderedGroups
	}

	// Token: 0x02001664 RID: 5732
	[Serializable]
	public class ChargeProjectile
	{
		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x060085B9 RID: 34233 RVA: 0x003726B0 File Offset: 0x003708B0
		public bool UsesOverrideShootAnimation
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.shootAnim) == ProjectileModule.ChargeProjectileProperties.shootAnim;
			}
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x060085BA RID: 34234 RVA: 0x003726C0 File Offset: 0x003708C0
		public bool UsesOverrideMuzzleFlashVfxPool
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.muzzleFlash) == ProjectileModule.ChargeProjectileProperties.muzzleFlash;
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x060085BB RID: 34235 RVA: 0x003726D0 File Offset: 0x003708D0
		public bool DepleteAmmo
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.depleteAmmo) == ProjectileModule.ChargeProjectileProperties.depleteAmmo;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x060085BC RID: 34236 RVA: 0x003726E0 File Offset: 0x003708E0
		public bool UsesAmmo
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.ammo) == ProjectileModule.ChargeProjectileProperties.ammo;
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x060085BD RID: 34237 RVA: 0x003726F0 File Offset: 0x003708F0
		public bool UsesVfx
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.vfx) == ProjectileModule.ChargeProjectileProperties.vfx;
			}
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x060085BE RID: 34238 RVA: 0x00372700 File Offset: 0x00370900
		public bool UsesLightIntensity
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.lightIntensity) == ProjectileModule.ChargeProjectileProperties.lightIntensity;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x060085BF RID: 34239 RVA: 0x00372710 File Offset: 0x00370910
		public bool UsesScreenShake
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.screenShake) == ProjectileModule.ChargeProjectileProperties.screenShake;
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x060085C0 RID: 34240 RVA: 0x00372720 File Offset: 0x00370920
		public bool ReflectsIncomingBullets
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.reflectBullets) == ProjectileModule.ChargeProjectileProperties.reflectBullets;
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x060085C1 RID: 34241 RVA: 0x00372738 File Offset: 0x00370938
		public bool DelayedVFXDestruction
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.delayedVFXClear) == ProjectileModule.ChargeProjectileProperties.delayedVFXClear;
			}
		}

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x060085C2 RID: 34242 RVA: 0x00372750 File Offset: 0x00370950
		public bool ShouldDoChargePoof
		{
			get
			{
				return this.Projectile && this.ChargeTime > 0f && (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.disableChargePoof) != ProjectileModule.ChargeProjectileProperties.disableChargePoof;
			}
		}

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x060085C3 RID: 34243 RVA: 0x0037278C File Offset: 0x0037098C
		public bool UsesAdditionalWwiseEvent
		{
			get
			{
				return (this.UsedProperties & ProjectileModule.ChargeProjectileProperties.additionalWwiseEvent) == ProjectileModule.ChargeProjectileProperties.additionalWwiseEvent;
			}
		}

		// Token: 0x04008A11 RID: 35345
		public float ChargeTime;

		// Token: 0x04008A12 RID: 35346
		public Projectile Projectile;

		// Token: 0x04008A13 RID: 35347
		public ProjectileModule.ChargeProjectileProperties UsedProperties;

		// Token: 0x04008A14 RID: 35348
		public int AmmoCost;

		// Token: 0x04008A15 RID: 35349
		public VFXPool VfxPool;

		// Token: 0x04008A16 RID: 35350
		public float LightIntensity;

		// Token: 0x04008A17 RID: 35351
		public ScreenShakeSettings ScreenShake;

		// Token: 0x04008A18 RID: 35352
		public string OverrideShootAnimation;

		// Token: 0x04008A19 RID: 35353
		public VFXPool OverrideMuzzleFlashVfxPool;

		// Token: 0x04008A1A RID: 35354
		public bool MegaReflection;

		// Token: 0x04008A1B RID: 35355
		public string AdditionalWwiseEvent;

		// Token: 0x04008A1C RID: 35356
		[NonSerialized]
		public ProjectileModule.ChargeProjectile previousChargeProjectile;
	}

	// Token: 0x02001665 RID: 5733
	[Flags]
	public enum ChargeProjectileProperties
	{
		// Token: 0x04008A1E RID: 35358
		ammo = 1,
		// Token: 0x04008A1F RID: 35359
		vfx = 2,
		// Token: 0x04008A20 RID: 35360
		lightIntensity = 4,
		// Token: 0x04008A21 RID: 35361
		screenShake = 8,
		// Token: 0x04008A22 RID: 35362
		shootAnim = 16,
		// Token: 0x04008A23 RID: 35363
		muzzleFlash = 32,
		// Token: 0x04008A24 RID: 35364
		depleteAmmo = 64,
		// Token: 0x04008A25 RID: 35365
		delayedVFXClear = 128,
		// Token: 0x04008A26 RID: 35366
		disableChargePoof = 256,
		// Token: 0x04008A27 RID: 35367
		reflectBullets = 512,
		// Token: 0x04008A28 RID: 35368
		additionalWwiseEvent = 1024
	}
}
