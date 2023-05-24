using System;

// Token: 0x020018AF RID: 6319
public class AkPlatformInitSettings : IDisposable
{
	// Token: 0x060096B9 RID: 38585 RVA: 0x003E8E9C File Offset: 0x003E709C
	internal AkPlatformInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096BA RID: 38586 RVA: 0x003E8EB4 File Offset: 0x003E70B4
	public AkPlatformInitSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPlatformInitSettings(), true)
	{
	}

	// Token: 0x060096BB RID: 38587 RVA: 0x003E8EC4 File Offset: 0x003E70C4
	internal static IntPtr getCPtr(AkPlatformInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060096BC RID: 38588 RVA: 0x003E8EDC File Offset: 0x003E70DC
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096BD RID: 38589 RVA: 0x003E8EEC File Offset: 0x003E70EC
	~AkPlatformInitSettings()
	{
		this.Dispose();
	}

	// Token: 0x060096BE RID: 38590 RVA: 0x003E8F1C File Offset: 0x003E711C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPlatformInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016D8 RID: 5848
	// (get) Token: 0x060096C0 RID: 38592 RVA: 0x003E8FA4 File Offset: 0x003E71A4
	// (set) Token: 0x060096BF RID: 38591 RVA: 0x003E8F90 File Offset: 0x003E7190
	public AkThreadProperties threadLEngine
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadLEngine_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadLEngine_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	// Token: 0x170016D9 RID: 5849
	// (get) Token: 0x060096C2 RID: 38594 RVA: 0x003E8FF0 File Offset: 0x003E71F0
	// (set) Token: 0x060096C1 RID: 38593 RVA: 0x003E8FDC File Offset: 0x003E71DC
	public AkThreadProperties threadBankManager
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadBankManager_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadBankManager_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	// Token: 0x170016DA RID: 5850
	// (get) Token: 0x060096C4 RID: 38596 RVA: 0x003E903C File Offset: 0x003E723C
	// (set) Token: 0x060096C3 RID: 38595 RVA: 0x003E9028 File Offset: 0x003E7228
	public AkThreadProperties threadMonitor
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadMonitor_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadMonitor_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	// Token: 0x170016DB RID: 5851
	// (get) Token: 0x060096C6 RID: 38598 RVA: 0x003E9084 File Offset: 0x003E7284
	// (set) Token: 0x060096C5 RID: 38597 RVA: 0x003E9074 File Offset: 0x003E7274
	public uint uLEngineDefaultPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016DC RID: 5852
	// (get) Token: 0x060096C8 RID: 38600 RVA: 0x003E90A4 File Offset: 0x003E72A4
	// (set) Token: 0x060096C7 RID: 38599 RVA: 0x003E9094 File Offset: 0x003E7294
	public float fLEngineDefaultPoolRatioThreshold
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016DD RID: 5853
	// (get) Token: 0x060096CA RID: 38602 RVA: 0x003E90C4 File Offset: 0x003E72C4
	// (set) Token: 0x060096C9 RID: 38601 RVA: 0x003E90B4 File Offset: 0x003E72B4
	public ushort uNumRefillsInVoice
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uNumRefillsInVoice_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uNumRefillsInVoice_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016DE RID: 5854
	// (get) Token: 0x060096CC RID: 38604 RVA: 0x003E90E4 File Offset: 0x003E72E4
	// (set) Token: 0x060096CB RID: 38603 RVA: 0x003E90D4 File Offset: 0x003E72D4
	public uint uSampleRate
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uSampleRate_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uSampleRate_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016DF RID: 5855
	// (get) Token: 0x060096CE RID: 38606 RVA: 0x003E9104 File Offset: 0x003E7304
	// (set) Token: 0x060096CD RID: 38605 RVA: 0x003E90F4 File Offset: 0x003E72F4
	public AkAudioAPI eAudioAPI
	{
		get
		{
			return (AkAudioAPI)AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_eAudioAPI_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_eAudioAPI_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x170016E0 RID: 5856
	// (get) Token: 0x060096D0 RID: 38608 RVA: 0x003E9124 File Offset: 0x003E7324
	// (set) Token: 0x060096CF RID: 38607 RVA: 0x003E9114 File Offset: 0x003E7314
	public bool bGlobalFocus
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_bGlobalFocus_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_bGlobalFocus_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CE7 RID: 40167
	private IntPtr swigCPtr;

	// Token: 0x04009CE8 RID: 40168
	protected bool swigCMemOwn;
}
