using System;
using System.Collections.Generic;

// Token: 0x02001567 RID: 5479
public class MidGameGunData
{
	// Token: 0x06007D84 RID: 32132 RVA: 0x0032D030 File Offset: 0x0032B230
	public MidGameGunData(Gun g)
	{
		this.PickupID = g.PickupObjectId;
		this.CurrentAmmo = g.CurrentAmmo;
		this.SerializedData = new List<object>();
		g.MidGameSerialize(this.SerializedData);
		this.DuctTapedGunIDs = new List<int>();
		if (g.DuctTapeMergedGunIDs != null)
		{
			this.DuctTapedGunIDs.AddRange(g.DuctTapeMergedGunIDs);
		}
	}

	// Token: 0x040080A2 RID: 32930
	public int PickupID = -1;

	// Token: 0x040080A3 RID: 32931
	public int CurrentAmmo = -1;

	// Token: 0x040080A4 RID: 32932
	public List<object> SerializedData;

	// Token: 0x040080A5 RID: 32933
	public List<int> DuctTapedGunIDs;
}
