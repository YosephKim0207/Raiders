using System;

// Token: 0x02001915 RID: 6421
public class AkTriggerMouseDown : AkTriggerBase
{
	// Token: 0x06009E19 RID: 40473 RVA: 0x003F2430 File Offset: 0x003F0630
	private void OnMouseDown()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
