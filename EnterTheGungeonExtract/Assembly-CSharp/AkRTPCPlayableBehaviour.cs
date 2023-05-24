using System;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02001902 RID: 6402
[Serializable]
public class AkRTPCPlayableBehaviour : PlayableBehaviour
{
	// Token: 0x17001707 RID: 5895
	// (set) Token: 0x06009DCF RID: 40399 RVA: 0x003F1490 File Offset: 0x003EF690
	public bool setRTPCGlobally
	{
		set
		{
			this.m_SetRTPCGlobally = value;
		}
	}

	// Token: 0x17001708 RID: 5896
	// (set) Token: 0x06009DD0 RID: 40400 RVA: 0x003F149C File Offset: 0x003EF69C
	public bool overrideTrackObject
	{
		set
		{
			this.m_OverrideTrackObject = value;
		}
	}

	// Token: 0x17001709 RID: 5897
	// (get) Token: 0x06009DD2 RID: 40402 RVA: 0x003F14B4 File Offset: 0x003EF6B4
	// (set) Token: 0x06009DD1 RID: 40401 RVA: 0x003F14A8 File Offset: 0x003EF6A8
	public GameObject rtpcObject
	{
		get
		{
			return this.m_RTPCObject;
		}
		set
		{
			this.m_RTPCObject = value;
		}
	}

	// Token: 0x1700170A RID: 5898
	// (set) Token: 0x06009DD3 RID: 40403 RVA: 0x003F14BC File Offset: 0x003EF6BC
	public RTPC parameter
	{
		set
		{
			this.m_Parameter = value;
		}
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x003F14C8 File Offset: 0x003EF6C8
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!this.m_OverrideTrackObject)
		{
			GameObject gameObject = playerData as GameObject;
			if (gameObject != null)
			{
				this.m_RTPCObject = gameObject;
			}
		}
		if (this.m_Parameter != null)
		{
			if (this.m_SetRTPCGlobally || this.m_RTPCObject == null)
			{
				this.m_Parameter.SetGlobalValue(this.RTPCValue);
			}
			else
			{
				this.m_Parameter.SetValue(this.m_RTPCObject, this.RTPCValue);
			}
		}
	}

	// Token: 0x04009F3A RID: 40762
	private bool m_OverrideTrackObject;

	// Token: 0x04009F3B RID: 40763
	private RTPC m_Parameter;

	// Token: 0x04009F3C RID: 40764
	private GameObject m_RTPCObject;

	// Token: 0x04009F3D RID: 40765
	private bool m_SetRTPCGlobally;

	// Token: 0x04009F3E RID: 40766
	public float RTPCValue;
}
