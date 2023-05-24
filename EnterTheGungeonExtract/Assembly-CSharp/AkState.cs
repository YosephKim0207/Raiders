using System;
using UnityEngine;

// Token: 0x02001909 RID: 6409
[AddComponentMenu("Wwise/AkState")]
public class AkState : AkUnityEventHandler
{
	// Token: 0x06009DFB RID: 40443 RVA: 0x003F1E7C File Offset: 0x003F007C
	public override void HandleEvent(GameObject in_gameObject)
	{
		AkSoundEngine.SetState((uint)this.groupID, (uint)this.valueID);
	}

	// Token: 0x04009F5A RID: 40794
	public int groupID;

	// Token: 0x04009F5B RID: 40795
	public int valueID;
}
