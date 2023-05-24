using System;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02001901 RID: 6401
[Serializable]
public class AkRTPCPlayable : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17001704 RID: 5892
	// (get) Token: 0x06009DC7 RID: 40391 RVA: 0x003F13C0 File Offset: 0x003EF5C0
	// (set) Token: 0x06009DC8 RID: 40392 RVA: 0x003F13C8 File Offset: 0x003EF5C8
	public RTPC Parameter
	{
		get
		{
			return this.RTPC;
		}
		set
		{
			this.RTPC = value;
		}
	}

	// Token: 0x17001705 RID: 5893
	// (get) Token: 0x06009DC9 RID: 40393 RVA: 0x003F13D4 File Offset: 0x003EF5D4
	// (set) Token: 0x06009DCA RID: 40394 RVA: 0x003F13DC File Offset: 0x003EF5DC
	public TimelineClip OwningClip
	{
		get
		{
			return this.owningClip;
		}
		set
		{
			this.owningClip = value;
		}
	}

	// Token: 0x17001706 RID: 5894
	// (get) Token: 0x06009DCB RID: 40395 RVA: 0x003F13E8 File Offset: 0x003EF5E8
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x06009DCC RID: 40396 RVA: 0x003F13EC File Offset: 0x003EF5EC
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<AkRTPCPlayableBehaviour> scriptPlayable = ScriptPlayable<AkRTPCPlayableBehaviour>.Create(graph, this.template, 0);
		AkRTPCPlayableBehaviour behaviour = scriptPlayable.GetBehaviour();
		this.InitializeBehavior(graph, ref behaviour, go);
		return scriptPlayable;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003F1420 File Offset: 0x003EF620
	public void InitializeBehavior(PlayableGraph graph, ref AkRTPCPlayableBehaviour b, GameObject owner)
	{
		b.overrideTrackObject = this.overrideTrackObject;
		b.setRTPCGlobally = this.setRTPCGlobally;
		if (this.overrideTrackObject)
		{
			b.rtpcObject = this.RTPCObject.Resolve(graph.GetResolver());
		}
		else
		{
			b.rtpcObject = owner;
		}
		b.parameter = this.RTPC;
	}

	// Token: 0x04009F34 RID: 40756
	public bool overrideTrackObject;

	// Token: 0x04009F35 RID: 40757
	private TimelineClip owningClip;

	// Token: 0x04009F36 RID: 40758
	private RTPC RTPC;

	// Token: 0x04009F37 RID: 40759
	public ExposedReference<GameObject> RTPCObject;

	// Token: 0x04009F38 RID: 40760
	public bool setRTPCGlobally;

	// Token: 0x04009F39 RID: 40761
	public AkRTPCPlayableBehaviour template = new AkRTPCPlayableBehaviour();
}
