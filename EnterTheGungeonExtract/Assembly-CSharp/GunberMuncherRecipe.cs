using System;
using System.Collections.Generic;

// Token: 0x02001182 RID: 4482
[Serializable]
public class GunberMuncherRecipe
{
	// Token: 0x04005F0E RID: 24334
	public string Annotation;

	// Token: 0x04005F0F RID: 24335
	[PickupIdentifier]
	public List<int> gunIDs_A;

	// Token: 0x04005F10 RID: 24336
	[PickupIdentifier]
	public List<int> gunIDs_B;

	// Token: 0x04005F11 RID: 24337
	[PickupIdentifier]
	public int resultID;

	// Token: 0x04005F12 RID: 24338
	[LongEnum]
	public GungeonFlags flagToSet;
}
