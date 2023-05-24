using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x02001904 RID: 6404
public class AkSoundEngineController
{
	// Token: 0x1700170B RID: 5899
	// (get) Token: 0x06009DDA RID: 40410 RVA: 0x003F1688 File Offset: 0x003EF888
	public static AkSoundEngineController Instance
	{
		get
		{
			if (AkSoundEngineController.ms_Instance == null)
			{
				AkSoundEngineController.ms_Instance = new AkSoundEngineController();
			}
			return AkSoundEngineController.ms_Instance;
		}
	}

	// Token: 0x06009DDB RID: 40411 RVA: 0x003F16A4 File Offset: 0x003EF8A4
	~AkSoundEngineController()
	{
		if (AkSoundEngineController.ms_Instance == this)
		{
			AkSoundEngineController.ms_Instance = null;
		}
	}

	// Token: 0x06009DDC RID: 40412 RVA: 0x003F16E0 File Offset: 0x003EF8E0
	public static string GetDecodedBankFolder()
	{
		return "DecodedBanks";
	}

	// Token: 0x06009DDD RID: 40413 RVA: 0x003F16E8 File Offset: 0x003EF8E8
	public static string GetDecodedBankFullPath()
	{
		return Path.Combine(AkBasePathGetter.GetPlatformBasePath(), AkSoundEngineController.GetDecodedBankFolder());
	}

	// Token: 0x06009DDE RID: 40414 RVA: 0x003F16FC File Offset: 0x003EF8FC
	public void LateUpdate()
	{
		AkCallbackManager.PostCallbacks();
		AkBankManager.DoUnloadBanks();
		AkSoundEngine.RenderAudio();
	}

	// Token: 0x06009DDF RID: 40415 RVA: 0x003F1710 File Offset: 0x003EF910
	public void Init(AkInitializer akInitializer)
	{
		bool flag = AkSoundEngine.IsInitialized();
		this.engineLogging = akInitializer.engineLogging;
		AkLogger.Instance.Init();
		if (flag)
		{
			return;
		}
		Debug.Log("WwiseUnity: Initialize sound engine ...");
		this.basePath = akInitializer.basePath;
		this.language = akInitializer.language;
		AkMemSettings akMemSettings = new AkMemSettings();
		akMemSettings.uMaxNumPools = 20U;
		AkDeviceSettings akDeviceSettings = new AkDeviceSettings();
		AkSoundEngine.GetDefaultDeviceSettings(akDeviceSettings);
		AkStreamMgrSettings akStreamMgrSettings = new AkStreamMgrSettings();
		akStreamMgrSettings.uMemorySize = (uint)(akInitializer.streamingPoolSize * 1024);
		AkInitSettings akInitSettings = new AkInitSettings();
		AkSoundEngine.GetDefaultInitSettings(akInitSettings);
		akInitSettings.uDefaultPoolSize = (uint)(akInitializer.defaultPoolSize * 1024);
		akInitSettings.uMonitorPoolSize = (uint)(akInitializer.monitorPoolSize * 1024);
		akInitSettings.uMonitorQueuePoolSize = (uint)(akInitializer.monitorQueuePoolSize * 1024);
		akInitSettings.szPluginDLLPath = Path.Combine(Application.dataPath, "Plugins" + Path.DirectorySeparatorChar);
		AkPlatformInitSettings akPlatformInitSettings = new AkPlatformInitSettings();
		AkSoundEngine.GetDefaultPlatformInitSettings(akPlatformInitSettings);
		akPlatformInitSettings.uLEngineDefaultPoolSize = (uint)(akInitializer.lowerPoolSize * 1024);
		akPlatformInitSettings.fLEngineDefaultPoolRatioThreshold = akInitializer.memoryCutoffThreshold;
		AkMusicSettings akMusicSettings = new AkMusicSettings();
		AkSoundEngine.GetDefaultMusicSettings(akMusicSettings);
		AkSpatialAudioInitSettings akSpatialAudioInitSettings = new AkSpatialAudioInitSettings();
		akSpatialAudioInitSettings.uPoolSize = (uint)(akInitializer.spatialAudioPoolSize * 1024);
		akSpatialAudioInitSettings.uMaxSoundPropagationDepth = akInitializer.maxSoundPropagationDepth;
		akSpatialAudioInitSettings.uDiffractionFlags = (uint)akInitializer.diffractionFlags;
		AkSoundEngine.SetGameName(Application.productName);
		AKRESULT akresult = AkSoundEngine.Init(akMemSettings, akStreamMgrSettings, akDeviceSettings, akInitSettings, akPlatformInitSettings, akMusicSettings, akSpatialAudioInitSettings, (uint)(akInitializer.preparePoolSize * 1024));
		if (akresult != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed to initialize the sound engine. Abort. :" + akresult.ToString());
			AkSoundEngine.Term();
			return;
		}
		string soundbankBasePath = AkBasePathGetter.GetSoundbankBasePath();
		if (string.IsNullOrEmpty(soundbankBasePath))
		{
			Debug.LogError("WwiseUnity: Couldn't find soundbanks base path. Terminate sound engine.");
			AkSoundEngine.Term();
			return;
		}
		akresult = AkSoundEngine.SetBasePath(soundbankBasePath);
		if (akresult != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed to set soundbanks base path. Terminate sound engine.");
			AkSoundEngine.Term();
			return;
		}
		string decodedBankFullPath = AkSoundEngineController.GetDecodedBankFullPath();
		AkSoundEngine.SetDecodedBankPath(decodedBankFullPath);
		AkSoundEngine.SetCurrentLanguage(this.language);
		AkSoundEngine.AddBasePath(Application.persistentDataPath + Path.DirectorySeparatorChar);
		AkSoundEngine.AddBasePath(decodedBankFullPath);
		akresult = AkCallbackManager.Init(akInitializer.callbackManagerBufferSize * 1024);
		if (akresult != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed to initialize Callback Manager. Terminate sound engine.");
			AkSoundEngine.Term();
			return;
		}
		AkBankManager.Reset();
		Debug.Log("WwiseUnity: Sound engine initialized.");
		uint num;
		akresult = AkSoundEngine.LoadBank("Init.bnk", -1, out num);
		if (akresult != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed load Init.bnk with result: " + akresult);
		}
	}

	// Token: 0x06009DE0 RID: 40416 RVA: 0x003F19A8 File Offset: 0x003EFBA8
	public void OnDisable()
	{
	}

	// Token: 0x06009DE1 RID: 40417 RVA: 0x003F19AC File Offset: 0x003EFBAC
	public void Terminate()
	{
		if (!AkSoundEngine.IsInitialized())
		{
			return;
		}
		AkSoundEngine.StopAll();
		AkSoundEngine.ClearBanks();
		AkSoundEngine.RenderAudio();
		int num = 5;
		do
		{
			int num2 = 0;
			do
			{
				num2 = AkCallbackManager.PostCallbacks();
				using (EventWaitHandle eventWaitHandle = new ManualResetEvent(false))
				{
					eventWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1.0));
				}
			}
			while (num2 > 0);
			using (EventWaitHandle eventWaitHandle2 = new ManualResetEvent(false))
			{
				eventWaitHandle2.WaitOne(TimeSpan.FromMilliseconds(10.0));
			}
			num--;
		}
		while (num > 0);
		AkSoundEngine.Term();
		AkCallbackManager.PostCallbacks();
		AkCallbackManager.Term();
		AkBankManager.Reset();
	}

	// Token: 0x06009DE2 RID: 40418 RVA: 0x003F1A80 File Offset: 0x003EFC80
	public void OnApplicationPause(bool pauseStatus)
	{
		AkSoundEngineController.ActivateAudio(!pauseStatus);
	}

	// Token: 0x06009DE3 RID: 40419 RVA: 0x003F1A8C File Offset: 0x003EFC8C
	public void OnApplicationFocus(bool focus)
	{
		AkSoundEngineController.ActivateAudio(focus);
	}

	// Token: 0x06009DE4 RID: 40420 RVA: 0x003F1A94 File Offset: 0x003EFC94
	private static void ActivateAudio(bool activate)
	{
		if (AkSoundEngine.IsInitialized())
		{
			if (activate)
			{
				AkSoundEngine.WakeupFromSuspend();
			}
			else
			{
				AkSoundEngine.Suspend();
			}
			AkSoundEngine.RenderAudio();
		}
	}

	// Token: 0x04009F40 RID: 40768
	public static readonly string s_DefaultBasePath = Path.Combine("Audio", "GeneratedSoundBanks");

	// Token: 0x04009F41 RID: 40769
	public static string s_Language = "English(US)";

	// Token: 0x04009F42 RID: 40770
	public static int s_DefaultPoolSize = 16384;

	// Token: 0x04009F43 RID: 40771
	public static int s_LowerPoolSize = 16384;

	// Token: 0x04009F44 RID: 40772
	public static int s_StreamingPoolSize = 2048;

	// Token: 0x04009F45 RID: 40773
	public static int s_PreparePoolSize = 0;

	// Token: 0x04009F46 RID: 40774
	public static float s_MemoryCutoffThreshold = 0.95f;

	// Token: 0x04009F47 RID: 40775
	public static int s_MonitorPoolSize = 128;

	// Token: 0x04009F48 RID: 40776
	public static int s_MonitorQueuePoolSize = 64;

	// Token: 0x04009F49 RID: 40777
	public static int s_CallbackManagerBufferSize = 4;

	// Token: 0x04009F4A RID: 40778
	public static bool s_EngineLogging = true;

	// Token: 0x04009F4B RID: 40779
	public static int s_SpatialAudioPoolSize = 8194;

	// Token: 0x04009F4C RID: 40780
	public string basePath = AkSoundEngineController.s_DefaultBasePath;

	// Token: 0x04009F4D RID: 40781
	public string language = AkSoundEngineController.s_Language;

	// Token: 0x04009F4E RID: 40782
	public bool engineLogging = AkSoundEngineController.s_EngineLogging;

	// Token: 0x04009F4F RID: 40783
	private static AkSoundEngineController ms_Instance;
}
