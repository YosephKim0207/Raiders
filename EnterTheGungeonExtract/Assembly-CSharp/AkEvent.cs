using System;
using UnityEngine;

// Token: 0x020018EA RID: 6378
[RequireComponent(typeof(AkGameObj))]
[AddComponentMenu("Wwise/AkEvent")]
public class AkEvent : AkUnityEventHandler
{
	// Token: 0x06009D30 RID: 40240 RVA: 0x003EE7A0 File Offset: 0x003EC9A0
	private void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
	{
		for (int i = 0; i < this.m_callbackData.callbackFunc.Count; i++)
		{
			if ((in_type & (AkCallbackType)this.m_callbackData.callbackFlags[i]) != (AkCallbackType)0 && this.m_callbackData.callbackGameObj[i] != null)
			{
				AkEventCallbackMsg akEventCallbackMsg = new AkEventCallbackMsg();
				akEventCallbackMsg.type = in_type;
				akEventCallbackMsg.sender = base.gameObject;
				akEventCallbackMsg.info = in_info;
				this.m_callbackData.callbackGameObj[i].SendMessage(this.m_callbackData.callbackFunc[i], akEventCallbackMsg);
			}
		}
	}

	// Token: 0x06009D31 RID: 40241 RVA: 0x003EE84C File Offset: 0x003ECA4C
	public override void HandleEvent(GameObject in_gameObject)
	{
		GameObject gameObject = ((!this.useOtherObject || !(in_gameObject != null)) ? base.gameObject : in_gameObject);
		this.soundEmitterObject = gameObject;
		if (this.enableActionOnEvent)
		{
			AkSoundEngine.ExecuteActionOnEvent((uint)this.eventID, this.actionOnEventType, gameObject, (int)this.transitionDuration * 1000, this.curveInterpolation);
			return;
		}
		if (this.m_callbackData != null)
		{
			this.playingId = AkSoundEngine.PostEvent((uint)this.eventID, gameObject, (uint)this.m_callbackData.uFlags, new AkCallbackManager.EventCallback(this.Callback), null, 0U, null, 0U);
		}
		else
		{
			this.playingId = AkSoundEngine.PostEvent((uint)this.eventID, gameObject);
		}
		if (this.playingId == 0U && AkSoundEngine.IsInitialized())
		{
			Debug.LogError("Could not post event ID \"" + (uint)this.eventID + "\". Did you make sure to load the appropriate SoundBank?");
		}
	}

	// Token: 0x06009D32 RID: 40242 RVA: 0x003EE940 File Offset: 0x003ECB40
	public void Stop(int _transitionDuration, AkCurveInterpolation _curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
	{
		AkSoundEngine.ExecuteActionOnEvent((uint)this.eventID, AkActionOnEventType.AkActionOnEventType_Stop, this.soundEmitterObject, _transitionDuration, _curveInterpolation);
	}

	// Token: 0x04009EAA RID: 40618
	public AkActionOnEventType actionOnEventType;

	// Token: 0x04009EAB RID: 40619
	public AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear;

	// Token: 0x04009EAC RID: 40620
	public bool enableActionOnEvent;

	// Token: 0x04009EAD RID: 40621
	public int eventID;

	// Token: 0x04009EAE RID: 40622
	public AkEventCallbackData m_callbackData;

	// Token: 0x04009EAF RID: 40623
	public uint playingId;

	// Token: 0x04009EB0 RID: 40624
	public GameObject soundEmitterObject;

	// Token: 0x04009EB1 RID: 40625
	public float transitionDuration;
}
