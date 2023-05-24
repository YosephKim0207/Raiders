using System;

// Token: 0x02001917 RID: 6423
public class AkTriggerMouseExit : AkTriggerBase
{
	// Token: 0x06009E1D RID: 40477 RVA: 0x003F2478 File Offset: 0x003F0678
	private void OnMouseExit()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
