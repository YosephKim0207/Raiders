using System;
using UnityEngine;

// Token: 0x02001913 RID: 6419
public class AkTriggerEnter : AkTriggerBase
{
	// Token: 0x06009E15 RID: 40469 RVA: 0x003F2380 File Offset: 0x003F0580
	private void OnTriggerEnter(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	// Token: 0x04009F63 RID: 40803
	public GameObject triggerObject;
}
