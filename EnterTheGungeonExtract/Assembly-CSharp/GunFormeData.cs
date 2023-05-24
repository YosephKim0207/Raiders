using System;

// Token: 0x020016EE RID: 5870
[Serializable]
public class GunFormeData
{
	// Token: 0x06008879 RID: 34937 RVA: 0x00389244 File Offset: 0x00387444
	public bool IsValid(PlayerController p)
	{
		return !this.RequiresSynergy || p.HasActiveBonusSynergy(this.RequiredSynergy, false);
	}

	// Token: 0x04008DDD RID: 36317
	public bool RequiresSynergy = true;

	// Token: 0x04008DDE RID: 36318
	[LongNumericEnum]
	[ShowInInspectorIf("RequiresSynergy", false)]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008DDF RID: 36319
	[PickupIdentifier]
	public int FormeID;
}
