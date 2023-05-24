using System;
using UnityEngine;

// Token: 0x0200190B RID: 6411
[AddComponentMenu("Wwise/AkSwitch")]
public class AkSwitch : AkUnityEventHandler
{
	// Token: 0x06009E03 RID: 40451 RVA: 0x003F211C File Offset: 0x003F031C
	public override void HandleEvent(GameObject in_gameObject)
	{
		AkSoundEngine.SetSwitch((uint)this.groupID, (uint)this.valueID, (!this.useOtherObject || !(in_gameObject != null)) ? base.gameObject : in_gameObject);
	}

	// Token: 0x04009F5E RID: 40798
	public int groupID;

	// Token: 0x04009F5F RID: 40799
	public int valueID;
}
