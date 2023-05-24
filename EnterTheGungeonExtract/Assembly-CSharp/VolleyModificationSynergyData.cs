using System;

// Token: 0x02001714 RID: 5908
[Serializable]
public class VolleyModificationSynergyData
{
	// Token: 0x04008F2B RID: 36651
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008F2C RID: 36652
	public bool AddsChargeProjectile;

	// Token: 0x04008F2D RID: 36653
	[ShowInInspectorIf("AddsChargeProjectile", false)]
	public ProjectileModule.ChargeProjectile ChargeProjectileToAdd;

	// Token: 0x04008F2E RID: 36654
	public bool AddsModules;

	// Token: 0x04008F2F RID: 36655
	public ProjectileModule[] ModulesToAdd;

	// Token: 0x04008F30 RID: 36656
	public bool AddsDuplicatesOfBaseModule;

	// Token: 0x04008F31 RID: 36657
	[ShowInInspectorIf("AddsDuplicatesOfBaseModule", false)]
	public int DuplicatesOfBaseModule;

	// Token: 0x04008F32 RID: 36658
	[ShowInInspectorIf("AddsDuplicatesOfBaseModule", false)]
	public float BaseModuleDuplicateAngle = 10f;

	// Token: 0x04008F33 RID: 36659
	public bool ReplacesSourceProjectile;

	// Token: 0x04008F34 RID: 36660
	[ShowInInspectorIf("ReplacesSourceProjectile", false)]
	public float ReplacementChance = 1f;

	// Token: 0x04008F35 RID: 36661
	[ShowInInspectorIf("ReplacesSourceProjectile", false)]
	public bool OnlyReplacesAdditionalProjectiles;

	// Token: 0x04008F36 RID: 36662
	[ShowInInspectorIf("ReplacesSourceProjectile", false)]
	public Projectile ReplacementProjectile;

	// Token: 0x04008F37 RID: 36663
	[ShowInInspectorIf("ReplacesSourceProjectile", false)]
	public bool UsesMultipleReplacementProjectiles;

	// Token: 0x04008F38 RID: 36664
	[ShowInInspectorIf("UsesMultipleReplacementProjectiles", false)]
	public bool MultipleReplacementsSequential;

	// Token: 0x04008F39 RID: 36665
	public Projectile[] MultipleReplacementProjectiles;

	// Token: 0x04008F3A RID: 36666
	[ShowInInspectorIf("ReplacesSourceProjectile", false)]
	public bool ReplacementSkipsChargedShots;

	// Token: 0x04008F3B RID: 36667
	public bool SetsNumberFinalProjectiles;

	// Token: 0x04008F3C RID: 36668
	[ShowInInspectorIf("SetsNumberFinalProjectiles", false)]
	public int NumberFinalProjectiles = 1;

	// Token: 0x04008F3D RID: 36669
	[ShowInInspectorIf("SetsNumberFinalProjectiles", false)]
	public bool AddsNewFinalProjectile;

	// Token: 0x04008F3E RID: 36670
	[ShowInInspectorIf("AddsNewFinalProjectile", false)]
	public Projectile NewFinalProjectile;

	// Token: 0x04008F3F RID: 36671
	[ShowInInspectorIf("AddsNewFinalProjectile", false)]
	public string NewFinalProjectileAmmoType;

	// Token: 0x04008F40 RID: 36672
	public bool SetsBurstCount;

	// Token: 0x04008F41 RID: 36673
	[ShowInInspectorIf("SetsBurstCount", false)]
	public bool MakesDefaultModuleBurst;

	// Token: 0x04008F42 RID: 36674
	[ShowInInspectorIf("SetsBurstCount", false)]
	public float BurstMultiplier = 1f;

	// Token: 0x04008F43 RID: 36675
	[ShowInInspectorIf("SetsBurstCount", false)]
	public int BurstShift;

	// Token: 0x04008F44 RID: 36676
	public bool AddsPossibleProjectileToPrimaryModule;

	// Token: 0x04008F45 RID: 36677
	[ShowInInspectorIf("AddsPossibleProjectileToPrimaryModule", false)]
	public Projectile AdditionalModuleProjectile;

	// Token: 0x04008F46 RID: 36678
	[NonSerialized]
	public int multipleSequentialReplacementIndex;
}
