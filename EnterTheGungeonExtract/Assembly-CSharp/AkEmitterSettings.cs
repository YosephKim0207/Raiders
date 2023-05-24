using System;

// Token: 0x0200186F RID: 6255
public class AkEmitterSettings : IDisposable
{
	// Token: 0x06009404 RID: 37892 RVA: 0x003E4164 File Offset: 0x003E2364
	internal AkEmitterSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009405 RID: 37893 RVA: 0x003E417C File Offset: 0x003E237C
	public AkEmitterSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkEmitterSettings(), true)
	{
	}

	// Token: 0x06009406 RID: 37894 RVA: 0x003E418C File Offset: 0x003E238C
	internal static IntPtr getCPtr(AkEmitterSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009407 RID: 37895 RVA: 0x003E41A4 File Offset: 0x003E23A4
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009408 RID: 37896 RVA: 0x003E41B4 File Offset: 0x003E23B4
	~AkEmitterSettings()
	{
		this.Dispose();
	}

	// Token: 0x06009409 RID: 37897 RVA: 0x003E41E4 File Offset: 0x003E23E4
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkEmitterSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001617 RID: 5655
	// (get) Token: 0x0600940B RID: 37899 RVA: 0x003E4268 File Offset: 0x003E2468
	// (set) Token: 0x0600940A RID: 37898 RVA: 0x003E4258 File Offset: 0x003E2458
	public uint reflectAuxBusID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectAuxBusID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectAuxBusID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001618 RID: 5656
	// (get) Token: 0x0600940D RID: 37901 RVA: 0x003E4288 File Offset: 0x003E2488
	// (set) Token: 0x0600940C RID: 37900 RVA: 0x003E4278 File Offset: 0x003E2478
	public float reflectionMaxPathLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionMaxPathLength_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionMaxPathLength_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001619 RID: 5657
	// (get) Token: 0x0600940F RID: 37903 RVA: 0x003E42A8 File Offset: 0x003E24A8
	// (set) Token: 0x0600940E RID: 37902 RVA: 0x003E4298 File Offset: 0x003E2498
	public float reflectionsAuxBusGain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsAuxBusGain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsAuxBusGain_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700161A RID: 5658
	// (get) Token: 0x06009411 RID: 37905 RVA: 0x003E42C8 File Offset: 0x003E24C8
	// (set) Token: 0x06009410 RID: 37904 RVA: 0x003E42B8 File Offset: 0x003E24B8
	public uint reflectionsOrder
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsOrder_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsOrder_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700161B RID: 5659
	// (get) Token: 0x06009413 RID: 37907 RVA: 0x003E42E8 File Offset: 0x003E24E8
	// (set) Token: 0x06009412 RID: 37906 RVA: 0x003E42D8 File Offset: 0x003E24D8
	public uint reflectorFilterMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectorFilterMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectorFilterMask_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700161C RID: 5660
	// (get) Token: 0x06009415 RID: 37909 RVA: 0x003E4308 File Offset: 0x003E2508
	// (set) Token: 0x06009414 RID: 37908 RVA: 0x003E42F8 File Offset: 0x003E24F8
	public float roomReverbAuxBusGain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_roomReverbAuxBusGain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_roomReverbAuxBusGain_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700161D RID: 5661
	// (get) Token: 0x06009417 RID: 37911 RVA: 0x003E4328 File Offset: 0x003E2528
	// (set) Token: 0x06009416 RID: 37910 RVA: 0x003E4318 File Offset: 0x003E2518
	public byte useImageSources
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_useImageSources_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_useImageSources_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B33 RID: 39731
	private IntPtr swigCPtr;

	// Token: 0x04009B34 RID: 39732
	protected bool swigCMemOwn;
}
