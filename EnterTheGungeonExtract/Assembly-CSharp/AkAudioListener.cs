using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018E0 RID: 6368
[AddComponentMenu("Wwise/AkAudioListener")]
[DisallowMultipleComponent]
[RequireComponent(typeof(AkGameObj))]
public class AkAudioListener : MonoBehaviour
{
	// Token: 0x170016FA RID: 5882
	// (get) Token: 0x06009D04 RID: 40196 RVA: 0x003EE0B8 File Offset: 0x003EC2B8
	public static AkAudioListener.DefaultListenerList DefaultListeners
	{
		get
		{
			return AkAudioListener.defaultListeners;
		}
	}

	// Token: 0x06009D05 RID: 40197 RVA: 0x003EE0C0 File Offset: 0x003EC2C0
	public void StartListeningToEmitter(AkGameObj emitter)
	{
		this.EmittersToStartListeningTo.Add(emitter);
		this.EmittersToStopListeningTo.Remove(emitter);
	}

	// Token: 0x06009D06 RID: 40198 RVA: 0x003EE0DC File Offset: 0x003EC2DC
	public void StopListeningToEmitter(AkGameObj emitter)
	{
		this.EmittersToStartListeningTo.Remove(emitter);
		this.EmittersToStopListeningTo.Add(emitter);
	}

	// Token: 0x06009D07 RID: 40199 RVA: 0x003EE0F8 File Offset: 0x003EC2F8
	public void SetIsDefaultListener(bool isDefault)
	{
		if (this.isDefaultListener != isDefault)
		{
			this.isDefaultListener = isDefault;
			if (isDefault)
			{
				AkAudioListener.DefaultListeners.Add(this);
			}
			else
			{
				AkAudioListener.DefaultListeners.Remove(this);
			}
		}
	}

	// Token: 0x06009D08 RID: 40200 RVA: 0x003EE130 File Offset: 0x003EC330
	private void Awake()
	{
		AkGameObj orAddComponent = base.gameObject.GetOrAddComponent<AkGameObj>();
		if (orAddComponent)
		{
			orAddComponent.Register();
		}
		this.akGameObjectID = AkSoundEngine.GetAkGameObjectID(base.gameObject);
	}

	// Token: 0x06009D09 RID: 40201 RVA: 0x003EE16C File Offset: 0x003EC36C
	private void OnEnable()
	{
		if (this.isDefaultListener)
		{
			AkAudioListener.DefaultListeners.Add(this);
		}
	}

	// Token: 0x06009D0A RID: 40202 RVA: 0x003EE188 File Offset: 0x003EC388
	private void OnDisable()
	{
		if (this.isDefaultListener)
		{
			AkAudioListener.DefaultListeners.Remove(this);
		}
	}

	// Token: 0x06009D0B RID: 40203 RVA: 0x003EE1A4 File Offset: 0x003EC3A4
	private void Update()
	{
		for (int i = 0; i < this.EmittersToStartListeningTo.Count; i++)
		{
			this.EmittersToStartListeningTo[i].AddListener(this);
		}
		this.EmittersToStartListeningTo.Clear();
		for (int j = 0; j < this.EmittersToStopListeningTo.Count; j++)
		{
			this.EmittersToStopListeningTo[j].RemoveListener(this);
		}
		this.EmittersToStopListeningTo.Clear();
	}

	// Token: 0x06009D0C RID: 40204 RVA: 0x003EE224 File Offset: 0x003EC424
	public ulong GetAkGameObjectID()
	{
		return this.akGameObjectID;
	}

	// Token: 0x06009D0D RID: 40205 RVA: 0x003EE22C File Offset: 0x003EC42C
	public void Migrate14()
	{
		bool flag = this.listenerId == 0;
		Debug.Log("WwiseUnity: AkAudioListener.Migrate14 for " + base.gameObject.name);
		this.isDefaultListener = flag;
	}

	// Token: 0x04009E8E RID: 40590
	private static readonly AkAudioListener.DefaultListenerList defaultListeners = new AkAudioListener.DefaultListenerList();

	// Token: 0x04009E8F RID: 40591
	private ulong akGameObjectID = ulong.MaxValue;

	// Token: 0x04009E90 RID: 40592
	private List<AkGameObj> EmittersToStartListeningTo = new List<AkGameObj>();

	// Token: 0x04009E91 RID: 40593
	private List<AkGameObj> EmittersToStopListeningTo = new List<AkGameObj>();

	// Token: 0x04009E92 RID: 40594
	public bool isDefaultListener = true;

	// Token: 0x04009E93 RID: 40595
	[SerializeField]
	public int listenerId;

	// Token: 0x020018E1 RID: 6369
	public class BaseListenerList
	{
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06009D10 RID: 40208 RVA: 0x003EE290 File Offset: 0x003EC490
		public List<AkAudioListener> ListenerList
		{
			get
			{
				return this.listenerList;
			}
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x003EE298 File Offset: 0x003EC498
		public virtual bool Add(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			ulong akGameObjectID = listener.GetAkGameObjectID();
			if (this.listenerIdList.Contains(akGameObjectID))
			{
				return false;
			}
			this.listenerIdList.Add(akGameObjectID);
			this.listenerList.Add(listener);
			return true;
		}

		// Token: 0x06009D12 RID: 40210 RVA: 0x003EE2E8 File Offset: 0x003EC4E8
		public virtual bool Remove(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			ulong akGameObjectID = listener.GetAkGameObjectID();
			if (!this.listenerIdList.Contains(akGameObjectID))
			{
				return false;
			}
			this.listenerIdList.Remove(akGameObjectID);
			this.listenerList.Remove(listener);
			return true;
		}

		// Token: 0x06009D13 RID: 40211 RVA: 0x003EE338 File Offset: 0x003EC538
		public ulong[] GetListenerIds()
		{
			return this.listenerIdList.ToArray();
		}

		// Token: 0x04009E94 RID: 40596
		private readonly List<ulong> listenerIdList = new List<ulong>();

		// Token: 0x04009E95 RID: 40597
		private readonly List<AkAudioListener> listenerList = new List<AkAudioListener>();
	}

	// Token: 0x020018E2 RID: 6370
	public class DefaultListenerList : AkAudioListener.BaseListenerList
	{
		// Token: 0x06009D15 RID: 40213 RVA: 0x003EE350 File Offset: 0x003EC550
		public override bool Add(AkAudioListener listener)
		{
			bool flag = base.Add(listener);
			if (flag && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.AddDefaultListener(listener.gameObject);
			}
			return flag;
		}

		// Token: 0x06009D16 RID: 40214 RVA: 0x003EE384 File Offset: 0x003EC584
		public override bool Remove(AkAudioListener listener)
		{
			bool flag = base.Remove(listener);
			if (flag && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.RemoveDefaultListener(listener.gameObject);
			}
			return flag;
		}
	}
}
