using System;
using UnityEngine;

// Token: 0x02001910 RID: 6416
public class AkTriggerCollisionExit : AkTriggerBase
{
	// Token: 0x06009E0F RID: 40463 RVA: 0x003F22E0 File Offset: 0x003F04E0
	private void OnCollisionExit(Collision in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	// Token: 0x04009F62 RID: 40802
	public GameObject triggerObject;
}
