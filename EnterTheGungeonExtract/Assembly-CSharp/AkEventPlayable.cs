using System;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020018ED RID: 6381
[Serializable]
public class AkEventPlayable : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170016FC RID: 5884
	// (get) Token: 0x06009D37 RID: 40247 RVA: 0x003EEA20 File Offset: 0x003ECC20
	// (set) Token: 0x06009D38 RID: 40248 RVA: 0x003EEA28 File Offset: 0x003ECC28
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

	// Token: 0x170016FD RID: 5885
	// (get) Token: 0x06009D39 RID: 40249 RVA: 0x003EEA34 File Offset: 0x003ECC34
	public override double duration
	{
		get
		{
			if (this.akEvent == null)
			{
				return base.duration;
			}
			return (double)this.eventDurationMax;
		}
	}

	// Token: 0x170016FE RID: 5886
	// (get) Token: 0x06009D3A RID: 40250 RVA: 0x003EEA50 File Offset: 0x003ECC50
	public ClipCaps clipCaps
	{
		get
		{
			if (!this.retriggerEvent)
			{
				return ClipCaps.All;
			}
			return ClipCaps.None;
		}
	}

	// Token: 0x06009D3B RID: 40251 RVA: 0x003EEA60 File Offset: 0x003ECC60
	public void setEaseInDuration(float d)
	{
		this.easeInDuration = d;
	}

	// Token: 0x06009D3C RID: 40252 RVA: 0x003EEA6C File Offset: 0x003ECC6C
	public void setEaseOutDuration(float d)
	{
		this.easeOutDuration = d;
	}

	// Token: 0x06009D3D RID: 40253 RVA: 0x003EEA78 File Offset: 0x003ECC78
	public void setBlendInDuration(float d)
	{
		this.blendInDuration = d;
	}

	// Token: 0x06009D3E RID: 40254 RVA: 0x003EEA84 File Offset: 0x003ECC84
	public void setBlendOutDuration(float d)
	{
		this.blendOutDuration = d;
	}

	// Token: 0x06009D3F RID: 40255 RVA: 0x003EEA90 File Offset: 0x003ECC90
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<AkEventPlayableBehavior> scriptPlayable = ScriptPlayable<AkEventPlayableBehavior>.Create(graph, 0);
		AkEventPlayableBehavior behaviour = scriptPlayable.GetBehaviour();
		this.initializeBehaviour(graph, behaviour, owner);
		behaviour.akEventMinDuration = this.eventDurationMin;
		behaviour.akEventMaxDuration = this.eventDurationMax;
		return scriptPlayable;
	}

	// Token: 0x06009D40 RID: 40256 RVA: 0x003EEAD4 File Offset: 0x003ECCD4
	public void initializeBehaviour(PlayableGraph graph, AkEventPlayableBehavior b, GameObject owner)
	{
		b.akEvent = this.akEvent;
		b.eventTracker = this.eventTracker;
		b.easeInDuration = this.easeInDuration;
		b.easeOutDuration = this.easeOutDuration;
		b.blendInDuration = this.blendInDuration;
		b.blendOutDuration = this.blendOutDuration;
		b.eventShouldRetrigger = this.retriggerEvent;
		b.overrideTrackEmittorObject = this.overrideTrackEmitterObject;
		if (this.overrideTrackEmitterObject)
		{
			b.eventObject = this.emitterObjectRef.Resolve(graph.GetResolver());
		}
		else
		{
			b.eventObject = owner;
		}
	}

	// Token: 0x04009EBC RID: 40636
	private readonly WwiseEventTracker eventTracker = new WwiseEventTracker();

	// Token: 0x04009EBD RID: 40637
	public AK.Wwise.Event akEvent;

	// Token: 0x04009EBE RID: 40638
	private float blendInDuration;

	// Token: 0x04009EBF RID: 40639
	private float blendOutDuration;

	// Token: 0x04009EC0 RID: 40640
	private float easeInDuration;

	// Token: 0x04009EC1 RID: 40641
	private float easeOutDuration;

	// Token: 0x04009EC2 RID: 40642
	public ExposedReference<GameObject> emitterObjectRef;

	// Token: 0x04009EC3 RID: 40643
	[SerializeField]
	private float eventDurationMax = -1f;

	// Token: 0x04009EC4 RID: 40644
	[SerializeField]
	private float eventDurationMin = -1f;

	// Token: 0x04009EC5 RID: 40645
	public bool overrideTrackEmitterObject;

	// Token: 0x04009EC6 RID: 40646
	private TimelineClip owningClip;

	// Token: 0x04009EC7 RID: 40647
	public bool retriggerEvent;
}
