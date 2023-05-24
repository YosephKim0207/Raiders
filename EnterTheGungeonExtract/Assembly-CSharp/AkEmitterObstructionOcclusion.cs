using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018E4 RID: 6372
[RequireComponent(typeof(AkGameObj))]
[AddComponentMenu("Wwise/AkEmitterObstructionOcclusion")]
public class AkEmitterObstructionOcclusion : AkObstructionOcclusion
{
	// Token: 0x06009D1E RID: 40222 RVA: 0x003EE4DC File Offset: 0x003EC6DC
	private void Awake()
	{
		base.InitIntervalsAndFadeRates();
		this.m_gameObj = base.GetComponent<AkGameObj>();
	}

	// Token: 0x06009D1F RID: 40223 RVA: 0x003EE4F0 File Offset: 0x003EC6F0
	protected override void UpdateObstructionOcclusionValuesForListeners()
	{
		if (AkRoom.IsSpatialAudioEnabled)
		{
			base.UpdateObstructionOcclusionValues(AkSpatialAudioListener.TheSpatialAudioListener);
		}
		else
		{
			if (this.m_gameObj.IsUsingDefaultListeners)
			{
				base.UpdateObstructionOcclusionValues(AkAudioListener.DefaultListeners.ListenerList);
			}
			base.UpdateObstructionOcclusionValues(this.m_gameObj.ListenerList);
		}
	}

	// Token: 0x06009D20 RID: 40224 RVA: 0x003EE548 File Offset: 0x003EC748
	protected override void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair)
	{
		if (AkRoom.IsSpatialAudioEnabled)
		{
			AkSoundEngine.SetEmitterObstruction(base.gameObject, ObsOccPair.Value.currentValue);
		}
		else
		{
			AkSoundEngine.SetObjectObstructionAndOcclusion(base.gameObject, ObsOccPair.Key.gameObject, 0f, ObsOccPair.Value.currentValue);
		}
	}

	// Token: 0x04009E9B RID: 40603
	private AkGameObj m_gameObj;
}
