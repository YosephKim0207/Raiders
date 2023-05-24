using System;

// Token: 0x020018EC RID: 6380
public class WwiseEventTracker
{
	// Token: 0x06009D35 RID: 40245 RVA: 0x003EE9A4 File Offset: 0x003ECBA4
	public void CallbackHandler(object in_cookie, AkCallbackType in_type, object in_info)
	{
		if (in_type == AkCallbackType.AK_EndOfEvent)
		{
			this.eventIsPlaying = false;
			this.fadeoutTriggered = false;
		}
		else if (in_type == AkCallbackType.AK_Duration)
		{
			float fEstimatedDuration = ((AkDurationCallbackInfo)in_info).fEstimatedDuration;
			this.currentDuration = fEstimatedDuration * this.currentDurationProportion / 1000f;
		}
	}

	// Token: 0x04009EB6 RID: 40630
	public float currentDuration = -1f;

	// Token: 0x04009EB7 RID: 40631
	public float currentDurationProportion = 1f;

	// Token: 0x04009EB8 RID: 40632
	public bool eventIsPlaying;

	// Token: 0x04009EB9 RID: 40633
	public bool fadeoutTriggered;

	// Token: 0x04009EBA RID: 40634
	public uint playingID;

	// Token: 0x04009EBB RID: 40635
	public float previousEventStartTime;
}
