using System;

// Token: 0x02001912 RID: 6418
public class AkTriggerEnable : AkTriggerBase
{
	// Token: 0x06009E13 RID: 40467 RVA: 0x003F235C File Offset: 0x003F055C
	private void OnEnable()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
