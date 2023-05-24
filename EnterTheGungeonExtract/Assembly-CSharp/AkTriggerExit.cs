using System;
using UnityEngine;

// Token: 0x02001914 RID: 6420
public class AkTriggerExit : AkTriggerBase
{
	// Token: 0x06009E17 RID: 40471 RVA: 0x003F23D8 File Offset: 0x003F05D8
	private void OnTriggerExit(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	// Token: 0x04009F64 RID: 40804
	public GameObject triggerObject;
}
