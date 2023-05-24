using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020018F0 RID: 6384
[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(AkEventPlayable))]
public class AkEventTrack : TrackAsset
{
	// Token: 0x06009D57 RID: 40279 RVA: 0x003EF3B4 File Offset: 0x003ED5B4
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ScriptPlayable<AkEventPlayableBehavior> scriptPlayable = ScriptPlayable<AkEventPlayableBehavior>.Create(graph, 0);
		scriptPlayable.SetInputCount(inputCount);
		this.setFadeTimes();
		this.setOwnerClips();
		return scriptPlayable;
	}

	// Token: 0x06009D58 RID: 40280 RVA: 0x003EF3E4 File Offset: 0x003ED5E4
	public void setFadeTimes()
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			AkEventPlayable akEventPlayable = (AkEventPlayable)timelineClip.asset;
			akEventPlayable.setBlendInDuration((float)this.getBlendInTime(akEventPlayable));
			akEventPlayable.setBlendOutDuration((float)this.getBlendOutTime(akEventPlayable));
			akEventPlayable.setEaseInDuration((float)this.getEaseInTime(akEventPlayable));
			akEventPlayable.setEaseOutDuration((float)this.getEaseOutTime(akEventPlayable));
		}
	}

	// Token: 0x06009D59 RID: 40281 RVA: 0x003EF47C File Offset: 0x003ED67C
	public void setOwnerClips()
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			AkEventPlayable akEventPlayable = (AkEventPlayable)timelineClip.asset;
			akEventPlayable.OwningClip = timelineClip;
		}
	}

	// Token: 0x06009D5A RID: 40282 RVA: 0x003EF4E4 File Offset: 0x003ED6E4
	public double getBlendInTime(AkEventPlayable playableClip)
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			if (playableClip == (AkEventPlayable)timelineClip.asset)
			{
				return timelineClip.blendInDuration;
			}
		}
		return 0.0;
	}

	// Token: 0x06009D5B RID: 40283 RVA: 0x003EF568 File Offset: 0x003ED768
	public double getBlendOutTime(AkEventPlayable playableClip)
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			if (playableClip == (AkEventPlayable)timelineClip.asset)
			{
				return timelineClip.blendOutDuration;
			}
		}
		return 0.0;
	}

	// Token: 0x06009D5C RID: 40284 RVA: 0x003EF5EC File Offset: 0x003ED7EC
	public double getEaseInTime(AkEventPlayable playableClip)
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			if (playableClip == (AkEventPlayable)timelineClip.asset)
			{
				return timelineClip.easeInDuration;
			}
		}
		return 0.0;
	}

	// Token: 0x06009D5D RID: 40285 RVA: 0x003EF670 File Offset: 0x003ED870
	public double getEaseOutTime(AkEventPlayable playableClip)
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			if (playableClip == (AkEventPlayable)timelineClip.asset)
			{
				return timelineClip.easeOutDuration;
			}
		}
		return 0.0;
	}
}
