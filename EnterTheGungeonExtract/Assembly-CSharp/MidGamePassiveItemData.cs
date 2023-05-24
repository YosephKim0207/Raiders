using System;
using System.Collections.Generic;

// Token: 0x02001569 RID: 5481
public class MidGamePassiveItemData
{
	// Token: 0x06007D86 RID: 32134 RVA: 0x0032D0BC File Offset: 0x0032B2BC
	public MidGamePassiveItemData(PassiveItem p)
	{
		this.PickupID = p.PickupObjectId;
		this.SerializedData = new List<object>();
		p.MidGameSerialize(this.SerializedData);
	}

	// Token: 0x040080AA RID: 32938
	public int PickupID = -1;

	// Token: 0x040080AB RID: 32939
	public List<object> SerializedData;
}
