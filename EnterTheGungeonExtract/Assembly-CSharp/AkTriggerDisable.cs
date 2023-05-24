using System;

// Token: 0x02001911 RID: 6417
public class AkTriggerDisable : AkTriggerBase
{
	// Token: 0x06009E11 RID: 40465 RVA: 0x003F2338 File Offset: 0x003F0538
	private void OnDisable()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
