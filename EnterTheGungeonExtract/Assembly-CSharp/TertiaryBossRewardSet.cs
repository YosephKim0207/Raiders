using System;
using System.Collections.Generic;

// Token: 0x02001697 RID: 5783
[Serializable]
public class TertiaryBossRewardSet
{
	// Token: 0x04008BFB RID: 35835
	public string annotation = "reward";

	// Token: 0x04008BFC RID: 35836
	public float weight = 1f;

	// Token: 0x04008BFD RID: 35837
	[PickupIdentifier]
	public List<int> dropIds;
}
