using System;
using System.Collections.Generic;

// Token: 0x020015D6 RID: 5590
[Serializable]
public class RandomStartingEquipmentSettings
{
	// Token: 0x0400833F RID: 33599
	public float D_CHANCE = 0.5f;

	// Token: 0x04008340 RID: 33600
	public float C_CHANCE = 0.4f;

	// Token: 0x04008341 RID: 33601
	public float B_CHANCE = 0.3f;

	// Token: 0x04008342 RID: 33602
	public float A_CHANCE = 0.05f;

	// Token: 0x04008343 RID: 33603
	public float S_CHANCE = 0.05f;

	// Token: 0x04008344 RID: 33604
	[PickupIdentifier]
	public List<int> ExcludedPickups;
}
