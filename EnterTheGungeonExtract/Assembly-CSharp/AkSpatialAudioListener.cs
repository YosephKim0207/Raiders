using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001907 RID: 6407
[RequireComponent(typeof(AkAudioListener))]
[AddComponentMenu("Wwise/AkSpatialAudioListener")]
[DisallowMultipleComponent]
public class AkSpatialAudioListener : AkSpatialAudioBase
{
	// Token: 0x1700170C RID: 5900
	// (get) Token: 0x06009DEF RID: 40431 RVA: 0x003F1CD4 File Offset: 0x003EFED4
	public static AkAudioListener TheSpatialAudioListener
	{
		get
		{
			return (!(AkSpatialAudioListener.s_SpatialAudioListener != null)) ? null : AkSpatialAudioListener.s_SpatialAudioListener.AkAudioListener;
		}
	}

	// Token: 0x1700170D RID: 5901
	// (get) Token: 0x06009DF0 RID: 40432 RVA: 0x003F1CF8 File Offset: 0x003EFEF8
	public static AkSpatialAudioListener.SpatialAudioListenerList SpatialAudioListeners
	{
		get
		{
			return AkSpatialAudioListener.spatialAudioListeners;
		}
	}

	// Token: 0x06009DF1 RID: 40433 RVA: 0x003F1D00 File Offset: 0x003EFF00
	private void Awake()
	{
		this.AkAudioListener = base.GetComponent<AkAudioListener>();
	}

	// Token: 0x06009DF2 RID: 40434 RVA: 0x003F1D10 File Offset: 0x003EFF10
	private void OnEnable()
	{
		AkSpatialAudioListener.spatialAudioListeners.Add(this);
	}

	// Token: 0x06009DF3 RID: 40435 RVA: 0x003F1D20 File Offset: 0x003EFF20
	private void OnDisable()
	{
		AkSpatialAudioListener.spatialAudioListeners.Remove(this);
	}

	// Token: 0x04009F56 RID: 40790
	private static AkSpatialAudioListener s_SpatialAudioListener;

	// Token: 0x04009F57 RID: 40791
	private static readonly AkSpatialAudioListener.SpatialAudioListenerList spatialAudioListeners = new AkSpatialAudioListener.SpatialAudioListenerList();

	// Token: 0x04009F58 RID: 40792
	private AkAudioListener AkAudioListener;

	// Token: 0x02001908 RID: 6408
	public class SpatialAudioListenerList
	{
		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06009DF6 RID: 40438 RVA: 0x003F1D50 File Offset: 0x003EFF50
		public List<AkSpatialAudioListener> ListenerList
		{
			get
			{
				return this.listenerList;
			}
		}

		// Token: 0x06009DF7 RID: 40439 RVA: 0x003F1D58 File Offset: 0x003EFF58
		public bool Add(AkSpatialAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			if (this.listenerList.Contains(listener))
			{
				return false;
			}
			this.listenerList.Add(listener);
			this.Refresh();
			return true;
		}

		// Token: 0x06009DF8 RID: 40440 RVA: 0x003F1D90 File Offset: 0x003EFF90
		public bool Remove(AkSpatialAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			if (!this.listenerList.Contains(listener))
			{
				return false;
			}
			this.listenerList.Remove(listener);
			this.Refresh();
			return true;
		}

		// Token: 0x06009DF9 RID: 40441 RVA: 0x003F1DC8 File Offset: 0x003EFFC8
		private void Refresh()
		{
			if (this.ListenerList.Count == 1)
			{
				if (AkSpatialAudioListener.s_SpatialAudioListener != null)
				{
					AkSoundEngine.UnregisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject);
				}
				AkSpatialAudioListener.s_SpatialAudioListener = this.ListenerList[0];
				if (AkSoundEngine.RegisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject) == AKRESULT.AK_Success)
				{
					AkSpatialAudioListener.s_SpatialAudioListener.SetGameObjectInRoom();
				}
			}
			else if (this.ListenerList.Count == 0 && AkSpatialAudioListener.s_SpatialAudioListener != null)
			{
				AkSoundEngine.UnregisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject);
				AkSpatialAudioListener.s_SpatialAudioListener = null;
			}
		}

		// Token: 0x04009F59 RID: 40793
		private readonly List<AkSpatialAudioListener> listenerList = new List<AkSpatialAudioListener>();
	}
}
