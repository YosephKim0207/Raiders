using System;
using AK.Wwise;
using UnityEngine;

// Token: 0x02001906 RID: 6406
[AddComponentMenu("Wwise/AkSpatialAudioEmitter")]
[RequireComponent(typeof(AkGameObj))]
public class AkSpatialAudioEmitter : AkSpatialAudioBase
{
	// Token: 0x06009DEC RID: 40428 RVA: 0x003F1C40 File Offset: 0x003EFE40
	private void OnEnable()
	{
		AkEmitterSettings akEmitterSettings = new AkEmitterSettings();
		akEmitterSettings.reflectAuxBusID = (uint)this.reflectAuxBus.ID;
		akEmitterSettings.reflectionMaxPathLength = this.reflectionMaxPathLength;
		akEmitterSettings.reflectionsAuxBusGain = this.reflectionsAuxBusGain;
		akEmitterSettings.reflectionsOrder = this.reflectionsOrder;
		akEmitterSettings.reflectorFilterMask = uint.MaxValue;
		akEmitterSettings.roomReverbAuxBusGain = this.roomReverbAuxBusGain;
		akEmitterSettings.useImageSources = 0;
		if (AkSoundEngine.RegisterEmitter(base.gameObject, akEmitterSettings) == AKRESULT.AK_Success)
		{
			base.SetGameObjectInRoom();
		}
	}

	// Token: 0x06009DED RID: 40429 RVA: 0x003F1CBC File Offset: 0x003EFEBC
	private void OnDisable()
	{
		AkSoundEngine.UnregisterEmitter(base.gameObject);
	}

	// Token: 0x04009F51 RID: 40785
	[Header("Early Reflections")]
	public AuxBus reflectAuxBus;

	// Token: 0x04009F52 RID: 40786
	public float reflectionMaxPathLength = 1000f;

	// Token: 0x04009F53 RID: 40787
	[Range(0f, 1f)]
	public float reflectionsAuxBusGain = 1f;

	// Token: 0x04009F54 RID: 40788
	[Range(1f, 4f)]
	public uint reflectionsOrder = 1U;

	// Token: 0x04009F55 RID: 40789
	[Range(0f, 1f)]
	[Header("Rooms")]
	public float roomReverbAuxBusGain = 1f;
}
