using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018DF RID: 6367
[AddComponentMenu("Wwise/AkAmbient")]
public class AkAmbient : AkEvent
{
	// Token: 0x170016F9 RID: 5881
	// (get) Token: 0x06009CFA RID: 40186 RVA: 0x003EDC7C File Offset: 0x003EBE7C
	// (set) Token: 0x06009CFB RID: 40187 RVA: 0x003EDC84 File Offset: 0x003EBE84
	public AkAmbient ParentAkAmbience { get; set; }

	// Token: 0x06009CFC RID: 40188 RVA: 0x003EDC90 File Offset: 0x003EBE90
	private void OnEnable()
	{
		if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Simple_Mode)
		{
			AkGameObj[] components = base.gameObject.GetComponents<AkGameObj>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = true;
			}
		}
		else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
		{
			AkGameObj[] components2 = base.gameObject.GetComponents<AkGameObj>();
			for (int j = 0; j < components2.Length; j++)
			{
				components2[j].enabled = false;
			}
			AkPositionArray akPositionArray = this.BuildAkPositionArray();
			AkSoundEngine.SetMultiplePositions(base.gameObject, akPositionArray, (ushort)akPositionArray.Count, this.MultiPositionType);
		}
		else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
		{
			AkGameObj[] components3 = base.gameObject.GetComponents<AkGameObj>();
			for (int k = 0; k < components3.Length; k++)
			{
				components3[k].enabled = false;
			}
			AkMultiPosEvent akMultiPosEvent;
			if (AkAmbient.multiPosEventTree.TryGetValue(this.eventID, out akMultiPosEvent))
			{
				if (!akMultiPosEvent.list.Contains(this))
				{
					akMultiPosEvent.list.Add(this);
				}
			}
			else
			{
				akMultiPosEvent = new AkMultiPosEvent();
				akMultiPosEvent.list.Add(this);
				AkAmbient.multiPosEventTree.Add(this.eventID, akMultiPosEvent);
			}
			AkPositionArray akPositionArray2 = this.BuildMultiDirectionArray(akMultiPosEvent);
			AkSoundEngine.SetMultiplePositions(akMultiPosEvent.list[0].gameObject, akPositionArray2, (ushort)akPositionArray2.Count, this.MultiPositionType);
		}
	}

	// Token: 0x06009CFD RID: 40189 RVA: 0x003EDE04 File Offset: 0x003EC004
	private void OnDisable()
	{
		if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
		{
			AkMultiPosEvent akMultiPosEvent = AkAmbient.multiPosEventTree[this.eventID];
			if (akMultiPosEvent.list.Count == 1)
			{
				AkAmbient.multiPosEventTree.Remove(this.eventID);
			}
			else
			{
				akMultiPosEvent.list.Remove(this);
				AkPositionArray akPositionArray = this.BuildMultiDirectionArray(akMultiPosEvent);
				AkSoundEngine.SetMultiplePositions(akMultiPosEvent.list[0].gameObject, akPositionArray, (ushort)akPositionArray.Count, this.MultiPositionType);
			}
		}
	}

	// Token: 0x06009CFE RID: 40190 RVA: 0x003EDE90 File Offset: 0x003EC090
	public override void HandleEvent(GameObject in_gameObject)
	{
		if (this.multiPositionTypeLabel != MultiPositionTypeLabel.MultiPosition_Mode)
		{
			base.HandleEvent(in_gameObject);
		}
		else
		{
			AkMultiPosEvent akMultiPosEvent = AkAmbient.multiPosEventTree[this.eventID];
			if (akMultiPosEvent.eventIsPlaying)
			{
				return;
			}
			akMultiPosEvent.eventIsPlaying = true;
			this.soundEmitterObject = akMultiPosEvent.list[0].gameObject;
			if (this.enableActionOnEvent)
			{
				AkSoundEngine.ExecuteActionOnEvent((uint)this.eventID, this.actionOnEventType, akMultiPosEvent.list[0].gameObject, (int)this.transitionDuration * 1000, this.curveInterpolation);
			}
			else
			{
				this.playingId = AkSoundEngine.PostEvent((uint)this.eventID, akMultiPosEvent.list[0].gameObject, 1U, new AkCallbackManager.EventCallback(akMultiPosEvent.FinishedPlaying), null, 0U, null, 0U);
			}
		}
	}

	// Token: 0x06009CFF RID: 40191 RVA: 0x003EDF6C File Offset: 0x003EC16C
	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawIcon(base.transform.position, "WwiseAudioSpeaker.png", false);
	}

	// Token: 0x06009D00 RID: 40192 RVA: 0x003EDF84 File Offset: 0x003EC184
	public AkPositionArray BuildMultiDirectionArray(AkMultiPosEvent eventPosList)
	{
		AkPositionArray akPositionArray = new AkPositionArray((uint)eventPosList.list.Count);
		for (int i = 0; i < eventPosList.list.Count; i++)
		{
			akPositionArray.Add(eventPosList.list[i].transform.position, eventPosList.list[i].transform.forward, eventPosList.list[i].transform.up);
		}
		return akPositionArray;
	}

	// Token: 0x06009D01 RID: 40193 RVA: 0x003EE008 File Offset: 0x003EC208
	private AkPositionArray BuildAkPositionArray()
	{
		AkPositionArray akPositionArray = new AkPositionArray((uint)this.multiPositionArray.Count);
		for (int i = 0; i < this.multiPositionArray.Count; i++)
		{
			akPositionArray.Add(base.transform.position + this.multiPositionArray[i], base.transform.forward, base.transform.up);
		}
		return akPositionArray;
	}

	// Token: 0x04009E89 RID: 40585
	public static Dictionary<int, AkMultiPosEvent> multiPosEventTree = new Dictionary<int, AkMultiPosEvent>();

	// Token: 0x04009E8A RID: 40586
	public List<Vector3> multiPositionArray = new List<Vector3>();

	// Token: 0x04009E8B RID: 40587
	public AkMultiPositionType MultiPositionType = AkMultiPositionType.MultiPositionType_MultiSources;

	// Token: 0x04009E8C RID: 40588
	public MultiPositionTypeLabel multiPositionTypeLabel;
}
