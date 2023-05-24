using System;

// Token: 0x02001460 RID: 5216
[Serializable]
public struct PlayerOrbitalSynergyData
{
	// Token: 0x0400785B RID: 30811
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x0400785C RID: 30812
	public bool EngagesFiring;

	// Token: 0x0400785D RID: 30813
	public float ShootCooldownMultiplier;

	// Token: 0x0400785E RID: 30814
	public int AdditionalShots;

	// Token: 0x0400785F RID: 30815
	public Projectile OverrideProjectile;

	// Token: 0x04007860 RID: 30816
	public bool HasOverrideAnimations;

	// Token: 0x04007861 RID: 30817
	public string OverrideIdleAnimation;
}
