using System;

// Token: 0x0200186A RID: 6250
public class AkDeviceSettings : IDisposable
{
	// Token: 0x060093D4 RID: 37844 RVA: 0x003E3C5C File Offset: 0x003E1E5C
	internal AkDeviceSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093D5 RID: 37845 RVA: 0x003E3C74 File Offset: 0x003E1E74
	public AkDeviceSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkDeviceSettings(), true)
	{
	}

	// Token: 0x060093D6 RID: 37846 RVA: 0x003E3C84 File Offset: 0x003E1E84
	internal static IntPtr getCPtr(AkDeviceSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093D7 RID: 37847 RVA: 0x003E3C9C File Offset: 0x003E1E9C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093D8 RID: 37848 RVA: 0x003E3CAC File Offset: 0x003E1EAC
	~AkDeviceSettings()
	{
		this.Dispose();
	}

	// Token: 0x060093D9 RID: 37849 RVA: 0x003E3CDC File Offset: 0x003E1EDC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkDeviceSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001604 RID: 5636
	// (get) Token: 0x060093DB RID: 37851 RVA: 0x003E3D60 File Offset: 0x003E1F60
	// (set) Token: 0x060093DA RID: 37850 RVA: 0x003E3D50 File Offset: 0x003E1F50
	public IntPtr pIOMemory
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_pIOMemory_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_pIOMemory_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001605 RID: 5637
	// (get) Token: 0x060093DD RID: 37853 RVA: 0x003E3D80 File Offset: 0x003E1F80
	// (set) Token: 0x060093DC RID: 37852 RVA: 0x003E3D70 File Offset: 0x003E1F70
	public uint uIOMemorySize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uIOMemorySize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uIOMemorySize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001606 RID: 5638
	// (get) Token: 0x060093DF RID: 37855 RVA: 0x003E3DA0 File Offset: 0x003E1FA0
	// (set) Token: 0x060093DE RID: 37854 RVA: 0x003E3D90 File Offset: 0x003E1F90
	public uint uIOMemoryAlignment
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uIOMemoryAlignment_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uIOMemoryAlignment_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001607 RID: 5639
	// (get) Token: 0x060093E1 RID: 37857 RVA: 0x003E3DC0 File Offset: 0x003E1FC0
	// (set) Token: 0x060093E0 RID: 37856 RVA: 0x003E3DB0 File Offset: 0x003E1FB0
	public int ePoolAttributes
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_ePoolAttributes_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_ePoolAttributes_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001608 RID: 5640
	// (get) Token: 0x060093E3 RID: 37859 RVA: 0x003E3DE0 File Offset: 0x003E1FE0
	// (set) Token: 0x060093E2 RID: 37858 RVA: 0x003E3DD0 File Offset: 0x003E1FD0
	public uint uGranularity
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uGranularity_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uGranularity_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001609 RID: 5641
	// (get) Token: 0x060093E5 RID: 37861 RVA: 0x003E3E00 File Offset: 0x003E2000
	// (set) Token: 0x060093E4 RID: 37860 RVA: 0x003E3DF0 File Offset: 0x003E1FF0
	public uint uSchedulerTypeFlags
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uSchedulerTypeFlags_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uSchedulerTypeFlags_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700160A RID: 5642
	// (get) Token: 0x060093E7 RID: 37863 RVA: 0x003E3E24 File Offset: 0x003E2024
	// (set) Token: 0x060093E6 RID: 37862 RVA: 0x003E3E10 File Offset: 0x003E2010
	public AkThreadProperties threadProperties
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_threadProperties_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_threadProperties_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	// Token: 0x1700160B RID: 5643
	// (get) Token: 0x060093E9 RID: 37865 RVA: 0x003E3E6C File Offset: 0x003E206C
	// (set) Token: 0x060093E8 RID: 37864 RVA: 0x003E3E5C File Offset: 0x003E205C
	public float fTargetAutoStmBufferLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_fTargetAutoStmBufferLength_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_fTargetAutoStmBufferLength_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700160C RID: 5644
	// (get) Token: 0x060093EB RID: 37867 RVA: 0x003E3E8C File Offset: 0x003E208C
	// (set) Token: 0x060093EA RID: 37866 RVA: 0x003E3E7C File Offset: 0x003E207C
	public uint uMaxConcurrentIO
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uMaxConcurrentIO_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uMaxConcurrentIO_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700160D RID: 5645
	// (get) Token: 0x060093ED RID: 37869 RVA: 0x003E3EAC File Offset: 0x003E20AC
	// (set) Token: 0x060093EC RID: 37868 RVA: 0x003E3E9C File Offset: 0x003E209C
	public bool bUseStreamCache
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_bUseStreamCache_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_bUseStreamCache_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700160E RID: 5646
	// (get) Token: 0x060093EF RID: 37871 RVA: 0x003E3ECC File Offset: 0x003E20CC
	// (set) Token: 0x060093EE RID: 37870 RVA: 0x003E3EBC File Offset: 0x003E20BC
	public uint uMaxCachePinnedBytes
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uMaxCachePinnedBytes_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDeviceSettings_uMaxCachePinnedBytes_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B27 RID: 39719
	private IntPtr swigCPtr;

	// Token: 0x04009B28 RID: 39720
	protected bool swigCMemOwn;
}
