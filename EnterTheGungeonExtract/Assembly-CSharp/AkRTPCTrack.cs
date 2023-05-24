using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02001903 RID: 6403
[TrackColor(0.32f, 0.13f, 0.13f)]
[TrackClipType(typeof(AkRTPCPlayable))]
[TrackBindingType(typeof(GameObject))]
public class AkRTPCTrack : TrackAsset
{
	// Token: 0x06009DD6 RID: 40406 RVA: 0x003F1558 File Offset: 0x003EF758
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ScriptPlayable<AkRTPCPlayableBehaviour> scriptPlayable = ScriptPlayable<AkRTPCPlayableBehaviour>.Create(graph, inputCount);
		this.setPlayableProperties();
		return scriptPlayable;
	}

	// Token: 0x06009DD7 RID: 40407 RVA: 0x003F157C File Offset: 0x003EF77C
	public void setPlayableProperties()
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			AkRTPCPlayable akRTPCPlayable = (AkRTPCPlayable)timelineClip.asset;
			akRTPCPlayable.Parameter = this.Parameter;
			akRTPCPlayable.OwningClip = timelineClip;
		}
	}

	// Token: 0x06009DD8 RID: 40408 RVA: 0x003F15F0 File Offset: 0x003EF7F0
	public void OnValidate()
	{
		IEnumerable<TimelineClip> clips = base.GetClips();
		foreach (TimelineClip timelineClip in clips)
		{
			AkRTPCPlayable akRTPCPlayable = (AkRTPCPlayable)timelineClip.asset;
			akRTPCPlayable.Parameter = this.Parameter;
		}
	}

	// Token: 0x04009F3F RID: 40767
	public RTPC Parameter;
}
