using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018F3 RID: 6387
[Serializable]
public class AkGameObjListenerList : AkAudioListener.BaseListenerList
{
	// Token: 0x06009D78 RID: 40312 RVA: 0x003EFEFC File Offset: 0x003EE0FC
	public void SetUseDefaultListeners(bool useDefault)
	{
		if (this.useDefaultListeners != useDefault)
		{
			this.useDefaultListeners = useDefault;
			if (useDefault)
			{
				AkSoundEngine.ResetListenersToDefault(this.akGameObj.gameObject);
				for (int i = 0; i < base.ListenerList.Count; i++)
				{
					AkSoundEngine.AddListener(this.akGameObj.gameObject, base.ListenerList[i].gameObject);
				}
			}
			else
			{
				ulong[] listenerIds = base.GetListenerIds();
				AkSoundEngine.SetListeners(this.akGameObj.gameObject, listenerIds, (uint)((listenerIds != null) ? listenerIds.Length : 0));
			}
		}
	}

	// Token: 0x06009D79 RID: 40313 RVA: 0x003EFFA0 File Offset: 0x003EE1A0
	public void Init(AkGameObj akGameObj)
	{
		this.akGameObj = akGameObj;
		if (!this.useDefaultListeners)
		{
			AkSoundEngine.SetListeners(akGameObj.gameObject, null, 0U);
		}
		for (int i = 0; i < this.initialListenerList.Count; i++)
		{
			this.initialListenerList[i].StartListeningToEmitter(akGameObj);
		}
	}

	// Token: 0x06009D7A RID: 40314 RVA: 0x003EFFFC File Offset: 0x003EE1FC
	public override bool Add(AkAudioListener listener)
	{
		bool flag = base.Add(listener);
		if (flag && AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.AddListener(this.akGameObj.gameObject, listener.gameObject);
		}
		return flag;
	}

	// Token: 0x06009D7B RID: 40315 RVA: 0x003F003C File Offset: 0x003EE23C
	public override bool Remove(AkAudioListener listener)
	{
		bool flag = base.Remove(listener);
		if (flag && AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.RemoveListener(this.akGameObj.gameObject, listener.gameObject);
		}
		return flag;
	}

	// Token: 0x04009EF2 RID: 40690
	[NonSerialized]
	private AkGameObj akGameObj;

	// Token: 0x04009EF3 RID: 40691
	[SerializeField]
	public List<AkAudioListener> initialListenerList = new List<AkAudioListener>();

	// Token: 0x04009EF4 RID: 40692
	[SerializeField]
	public bool useDefaultListeners = true;
}
