using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001900 RID: 6400
[RequireComponent(typeof(AkRoomPortal))]
[AddComponentMenu("Wwise/AkRoomPortalObstruction")]
public class AkRoomPortalObstruction : AkObstructionOcclusion
{
	// Token: 0x06009DC3 RID: 40387 RVA: 0x003F1368 File Offset: 0x003EF568
	private void Awake()
	{
		base.InitIntervalsAndFadeRates();
		this.m_portal = base.GetComponent<AkRoomPortal>();
	}

	// Token: 0x06009DC4 RID: 40388 RVA: 0x003F137C File Offset: 0x003EF57C
	protected override void UpdateObstructionOcclusionValuesForListeners()
	{
		base.UpdateObstructionOcclusionValues(AkSpatialAudioListener.TheSpatialAudioListener);
	}

	// Token: 0x06009DC5 RID: 40389 RVA: 0x003F138C File Offset: 0x003EF58C
	protected override void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair)
	{
		AkSoundEngine.SetPortalObstruction(this.m_portal.GetID(), ObsOccPair.Value.currentValue);
	}

	// Token: 0x04009F33 RID: 40755
	private AkRoomPortal m_portal;
}
