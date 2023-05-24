using System;

// Token: 0x02001916 RID: 6422
public class AkTriggerMouseEnter : AkTriggerBase
{
	// Token: 0x06009E1B RID: 40475 RVA: 0x003F2454 File Offset: 0x003F0654
	private void OnMouseEnter()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
