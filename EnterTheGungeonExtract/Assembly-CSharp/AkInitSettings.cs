using System;

// Token: 0x02001876 RID: 6262
public class AkInitSettings : IDisposable
{
	// Token: 0x0600944D RID: 37965 RVA: 0x003E495C File Offset: 0x003E2B5C
	internal AkInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600944E RID: 37966 RVA: 0x003E4974 File Offset: 0x003E2B74
	public AkInitSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkInitSettings(), true)
	{
	}

	// Token: 0x0600944F RID: 37967 RVA: 0x003E4984 File Offset: 0x003E2B84
	internal static IntPtr getCPtr(AkInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009450 RID: 37968 RVA: 0x003E499C File Offset: 0x003E2B9C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009451 RID: 37969 RVA: 0x003E49AC File Offset: 0x003E2BAC
	~AkInitSettings()
	{
		this.Dispose();
	}

	// Token: 0x06009452 RID: 37970 RVA: 0x003E49DC File Offset: 0x003E2BDC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700162A RID: 5674
	// (get) Token: 0x06009454 RID: 37972 RVA: 0x003E4A54 File Offset: 0x003E2C54
	// (set) Token: 0x06009453 RID: 37971 RVA: 0x003E4A50 File Offset: 0x003E2C50
	public int pfnAssertHook
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	// Token: 0x1700162B RID: 5675
	// (get) Token: 0x06009456 RID: 37974 RVA: 0x003E4A68 File Offset: 0x003E2C68
	// (set) Token: 0x06009455 RID: 37973 RVA: 0x003E4A58 File Offset: 0x003E2C58
	public uint uMaxNumPaths
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700162C RID: 5676
	// (get) Token: 0x06009458 RID: 37976 RVA: 0x003E4A88 File Offset: 0x003E2C88
	// (set) Token: 0x06009457 RID: 37975 RVA: 0x003E4A78 File Offset: 0x003E2C78
	public uint uDefaultPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700162D RID: 5677
	// (get) Token: 0x0600945A RID: 37978 RVA: 0x003E4AA8 File Offset: 0x003E2CA8
	// (set) Token: 0x06009459 RID: 37977 RVA: 0x003E4A98 File Offset: 0x003E2C98
	public float fDefaultPoolRatioThreshold
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700162E RID: 5678
	// (get) Token: 0x0600945C RID: 37980 RVA: 0x003E4AC8 File Offset: 0x003E2CC8
	// (set) Token: 0x0600945B RID: 37979 RVA: 0x003E4AB8 File Offset: 0x003E2CB8
	public uint uCommandQueueSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700162F RID: 5679
	// (get) Token: 0x0600945E RID: 37982 RVA: 0x003E4AE8 File Offset: 0x003E2CE8
	// (set) Token: 0x0600945D RID: 37981 RVA: 0x003E4AD8 File Offset: 0x003E2CD8
	public int uPrepareEventMemoryPoolID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001630 RID: 5680
	// (get) Token: 0x06009460 RID: 37984 RVA: 0x003E4B08 File Offset: 0x003E2D08
	// (set) Token: 0x0600945F RID: 37983 RVA: 0x003E4AF8 File Offset: 0x003E2CF8
	public bool bEnableGameSyncPreparation
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001631 RID: 5681
	// (get) Token: 0x06009462 RID: 37986 RVA: 0x003E4B28 File Offset: 0x003E2D28
	// (set) Token: 0x06009461 RID: 37985 RVA: 0x003E4B18 File Offset: 0x003E2D18
	public uint uContinuousPlaybackLookAhead
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001632 RID: 5682
	// (get) Token: 0x06009464 RID: 37988 RVA: 0x003E4B48 File Offset: 0x003E2D48
	// (set) Token: 0x06009463 RID: 37987 RVA: 0x003E4B38 File Offset: 0x003E2D38
	public uint uNumSamplesPerFrame
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001633 RID: 5683
	// (get) Token: 0x06009466 RID: 37990 RVA: 0x003E4B68 File Offset: 0x003E2D68
	// (set) Token: 0x06009465 RID: 37989 RVA: 0x003E4B58 File Offset: 0x003E2D58
	public uint uMonitorPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001634 RID: 5684
	// (get) Token: 0x06009468 RID: 37992 RVA: 0x003E4B88 File Offset: 0x003E2D88
	// (set) Token: 0x06009467 RID: 37991 RVA: 0x003E4B78 File Offset: 0x003E2D78
	public uint uMonitorQueuePoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001635 RID: 5685
	// (get) Token: 0x0600946A RID: 37994 RVA: 0x003E4BAC File Offset: 0x003E2DAC
	// (set) Token: 0x06009469 RID: 37993 RVA: 0x003E4B98 File Offset: 0x003E2D98
	public AkOutputSettings settingsMainOutput
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkOutputSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_set(this.swigCPtr, AkOutputSettings.getCPtr(value));
		}
	}

	// Token: 0x17001636 RID: 5686
	// (get) Token: 0x0600946C RID: 37996 RVA: 0x003E4BF4 File Offset: 0x003E2DF4
	// (set) Token: 0x0600946B RID: 37995 RVA: 0x003E4BE4 File Offset: 0x003E2DE4
	public uint uMaxHardwareTimeoutMs
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001637 RID: 5687
	// (get) Token: 0x0600946E RID: 37998 RVA: 0x003E4C14 File Offset: 0x003E2E14
	// (set) Token: 0x0600946D RID: 37997 RVA: 0x003E4C04 File Offset: 0x003E2E04
	public bool bUseSoundBankMgrThread
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001638 RID: 5688
	// (get) Token: 0x06009470 RID: 38000 RVA: 0x003E4C34 File Offset: 0x003E2E34
	// (set) Token: 0x0600946F RID: 37999 RVA: 0x003E4C24 File Offset: 0x003E2E24
	public bool bUseLEngineThread
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseLEngineThread_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseLEngineThread_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001639 RID: 5689
	// (get) Token: 0x06009472 RID: 38002 RVA: 0x003E4C54 File Offset: 0x003E2E54
	// (set) Token: 0x06009471 RID: 38001 RVA: 0x003E4C44 File Offset: 0x003E2E44
	public string szPluginDLLPath
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_AkInitSettings_szPluginDLLPath_get(this.swigCPtr));
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_szPluginDLLPath_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B4A RID: 39754
	private IntPtr swigCPtr;

	// Token: 0x04009B4B RID: 39755
	protected bool swigCMemOwn;
}
