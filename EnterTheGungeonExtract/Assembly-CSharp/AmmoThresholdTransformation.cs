using System;

// Token: 0x02001693 RID: 5779
[Serializable]
public struct AmmoThresholdTransformation
{
	// Token: 0x060086C1 RID: 34497 RVA: 0x0037DA64 File Offset: 0x0037BC64
	public float GetAmmoPercentage()
	{
		int num = -1;
		if (this.HasSynergyChange && PlayerController.AnyoneHasActiveBonusSynergy(this.RequiredSynergy, out num))
		{
			return this.SynergyAmmoPercentage;
		}
		return this.AmmoPercentage;
	}

	// Token: 0x04008BE3 RID: 35811
	public float AmmoPercentage;

	// Token: 0x04008BE4 RID: 35812
	[PickupIdentifier]
	public int TargetGunID;

	// Token: 0x04008BE5 RID: 35813
	public bool HasSynergyChange;

	// Token: 0x04008BE6 RID: 35814
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008BE7 RID: 35815
	public float SynergyAmmoPercentage;
}
