using System;
using UnityEngine;

// Token: 0x020018F7 RID: 6391
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(AkTerminator))]
[AddComponentMenu("Wwise/AkInitializer")]
public class AkInitializer : MonoBehaviour
{
	// Token: 0x06009D80 RID: 40320 RVA: 0x003F0144 File Offset: 0x003EE344
	public static string GetBasePath()
	{
		return AkSoundEngineController.Instance.basePath;
	}

	// Token: 0x06009D81 RID: 40321 RVA: 0x003F0150 File Offset: 0x003EE350
	public static string GetCurrentLanguage()
	{
		return AkSoundEngineController.Instance.language;
	}

	// Token: 0x06009D82 RID: 40322 RVA: 0x003F015C File Offset: 0x003EE35C
	private void Awake()
	{
		if (AkInitializer.ms_Instance)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		AkInitializer.ms_Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06009D83 RID: 40323 RVA: 0x003F0180 File Offset: 0x003EE380
	private void OnEnable()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.Init(this);
		}
	}

	// Token: 0x06009D84 RID: 40324 RVA: 0x003F01A0 File Offset: 0x003EE3A0
	private void OnDisable()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnDisable();
		}
	}

	// Token: 0x06009D85 RID: 40325 RVA: 0x003F01BC File Offset: 0x003EE3BC
	private void OnDestroy()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkInitializer.ms_Instance = null;
		}
	}

	// Token: 0x06009D86 RID: 40326 RVA: 0x003F01D4 File Offset: 0x003EE3D4
	private void OnApplicationPause(bool pauseStatus)
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnApplicationPause(pauseStatus);
		}
	}

	// Token: 0x06009D87 RID: 40327 RVA: 0x003F01F4 File Offset: 0x003EE3F4
	private void OnApplicationFocus(bool focus)
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnApplicationFocus(focus);
		}
	}

	// Token: 0x06009D88 RID: 40328 RVA: 0x003F0214 File Offset: 0x003EE414
	private void OnApplicationQuit()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.Terminate();
		}
	}

	// Token: 0x06009D89 RID: 40329 RVA: 0x003F0230 File Offset: 0x003EE430
	private void LateUpdate()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.LateUpdate();
		}
	}

	// Token: 0x04009EFB RID: 40699
	public string basePath = AkSoundEngineController.s_DefaultBasePath;

	// Token: 0x04009EFC RID: 40700
	public string language = AkSoundEngineController.s_Language;

	// Token: 0x04009EFD RID: 40701
	public int defaultPoolSize = AkSoundEngineController.s_DefaultPoolSize;

	// Token: 0x04009EFE RID: 40702
	public int lowerPoolSize = AkSoundEngineController.s_LowerPoolSize;

	// Token: 0x04009EFF RID: 40703
	public int streamingPoolSize = AkSoundEngineController.s_StreamingPoolSize;

	// Token: 0x04009F00 RID: 40704
	public int preparePoolSize = AkSoundEngineController.s_PreparePoolSize;

	// Token: 0x04009F01 RID: 40705
	public float memoryCutoffThreshold = AkSoundEngineController.s_MemoryCutoffThreshold;

	// Token: 0x04009F02 RID: 40706
	public int monitorPoolSize = AkSoundEngineController.s_MonitorPoolSize;

	// Token: 0x04009F03 RID: 40707
	public int monitorQueuePoolSize = AkSoundEngineController.s_MonitorQueuePoolSize;

	// Token: 0x04009F04 RID: 40708
	public int callbackManagerBufferSize = AkSoundEngineController.s_CallbackManagerBufferSize;

	// Token: 0x04009F05 RID: 40709
	public int spatialAudioPoolSize = AkSoundEngineController.s_SpatialAudioPoolSize;

	// Token: 0x04009F06 RID: 40710
	[Range(0f, 8f)]
	public uint maxSoundPropagationDepth = 8U;

	// Token: 0x04009F07 RID: 40711
	[Tooltip("Default Diffraction Flags combine all the diffraction flags")]
	public AkDiffractionFlags diffractionFlags = AkDiffractionFlags.DefaultDiffractionFlags;

	// Token: 0x04009F08 RID: 40712
	public bool engineLogging = AkSoundEngineController.s_EngineLogging;

	// Token: 0x04009F09 RID: 40713
	private static AkInitializer ms_Instance;
}
