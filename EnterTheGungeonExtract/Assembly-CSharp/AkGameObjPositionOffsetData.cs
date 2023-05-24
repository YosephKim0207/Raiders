using System;
using UnityEngine;

// Token: 0x020018F5 RID: 6389
[Serializable]
public class AkGameObjPositionOffsetData
{
	// Token: 0x06009D7D RID: 40317 RVA: 0x003F0084 File Offset: 0x003EE284
	public AkGameObjPositionOffsetData(bool IReallyWantToBeConstructed = false)
	{
		this.KeepMe = IReallyWantToBeConstructed;
	}

	// Token: 0x04009EF8 RID: 40696
	public bool KeepMe;

	// Token: 0x04009EF9 RID: 40697
	public Vector3 positionOffset;
}
