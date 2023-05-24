using System;

// Token: 0x02001918 RID: 6424
public class AkTriggerMouseUp : AkTriggerBase
{
	// Token: 0x06009E1F RID: 40479 RVA: 0x003F249C File Offset: 0x003F069C
	private void OnMouseUp()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
