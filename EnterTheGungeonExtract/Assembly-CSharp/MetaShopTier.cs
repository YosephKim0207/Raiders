using System;

// Token: 0x02001551 RID: 5457
[Serializable]
public class MetaShopTier
{
	// Token: 0x04007FF7 RID: 32759
	public int overrideTierCost = -1;

	// Token: 0x04007FF8 RID: 32760
	[PickupIdentifier]
	public int itemId1 = -1;

	// Token: 0x04007FF9 RID: 32761
	public int overrideItem1Cost = -1;

	// Token: 0x04007FFA RID: 32762
	[PickupIdentifier]
	public int itemId2 = -1;

	// Token: 0x04007FFB RID: 32763
	public int overrideItem2Cost = -1;

	// Token: 0x04007FFC RID: 32764
	[PickupIdentifier]
	public int itemId3 = -1;

	// Token: 0x04007FFD RID: 32765
	public int overrideItem3Cost = -1;
}
