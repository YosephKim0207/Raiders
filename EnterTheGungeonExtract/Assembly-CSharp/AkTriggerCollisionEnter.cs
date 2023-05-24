using System;
using UnityEngine;

// Token: 0x0200190F RID: 6415
public class AkTriggerCollisionEnter : AkTriggerBase
{
	// Token: 0x06009E0C RID: 40460 RVA: 0x003F2238 File Offset: 0x003F0438
	private void OnCollisionEnter(Collision in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	// Token: 0x06009E0D RID: 40461 RVA: 0x003F2288 File Offset: 0x003F0488
	private void OnTriggerEnter(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	// Token: 0x04009F61 RID: 40801
	public GameObject triggerObject;
}
