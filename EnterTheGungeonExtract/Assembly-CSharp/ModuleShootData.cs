using System;

// Token: 0x02001322 RID: 4898
public class ModuleShootData
{
	// Token: 0x04006CB1 RID: 27825
	public bool onCooldown;

	// Token: 0x04006CB2 RID: 27826
	public bool needsReload;

	// Token: 0x04006CB3 RID: 27827
	public int numberShotsFired;

	// Token: 0x04006CB4 RID: 27828
	public int numberShotsFiredThisBurst;

	// Token: 0x04006CB5 RID: 27829
	public int numberShotsActiveReload;

	// Token: 0x04006CB6 RID: 27830
	public float chargeTime;

	// Token: 0x04006CB7 RID: 27831
	public bool chargeFired;

	// Token: 0x04006CB8 RID: 27832
	public ProjectileModule.ChargeProjectile lastChargeProjectile;

	// Token: 0x04006CB9 RID: 27833
	public float activeReloadDamageModifier = 1f;

	// Token: 0x04006CBA RID: 27834
	public float alternateAngleSign = 1f;

	// Token: 0x04006CBB RID: 27835
	public BeamController beam;

	// Token: 0x04006CBC RID: 27836
	public int beamKnockbackID;

	// Token: 0x04006CBD RID: 27837
	public float angleForShot;
}
