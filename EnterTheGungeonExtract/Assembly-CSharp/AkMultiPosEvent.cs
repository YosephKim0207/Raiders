using System;
using System.Collections.Generic;

// Token: 0x020018DE RID: 6366
public class AkMultiPosEvent
{
	// Token: 0x06009CF8 RID: 40184 RVA: 0x003EDC54 File Offset: 0x003EBE54
	public void FinishedPlaying(object in_cookie, AkCallbackType in_type, object in_info)
	{
		this.eventIsPlaying = false;
	}

	// Token: 0x04009E87 RID: 40583
	public bool eventIsPlaying;

	// Token: 0x04009E88 RID: 40584
	public List<AkAmbient> list = new List<AkAmbient>();
}
