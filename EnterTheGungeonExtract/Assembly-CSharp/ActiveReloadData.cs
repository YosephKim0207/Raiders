using System;

// Token: 0x02001324 RID: 4900
[Serializable]
public class ActiveReloadData
{
	// Token: 0x04006CC3 RID: 27843
	public float damageMultiply = 1f;

	// Token: 0x04006CC4 RID: 27844
	public float knockbackMultiply = 1f;

	// Token: 0x04006CC5 RID: 27845
	public bool usesOverrideAngleVariance;

	// Token: 0x04006CC6 RID: 27846
	public float overrideAngleVariance;

	// Token: 0x04006CC7 RID: 27847
	public float reloadSpeedMultiplier = 1f;

	// Token: 0x04006CC8 RID: 27848
	public bool ActiveReloadStacks;

	// Token: 0x04006CC9 RID: 27849
	public bool ActiveReloadIncrementsTier;

	// Token: 0x04006CCA RID: 27850
	public int MaxTier = 5;
}
