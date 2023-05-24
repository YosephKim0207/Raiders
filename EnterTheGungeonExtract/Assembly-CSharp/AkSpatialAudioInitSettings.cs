using System;

// Token: 0x020018A7 RID: 6311
public class AkSpatialAudioInitSettings : IDisposable
{
	// Token: 0x06009668 RID: 38504 RVA: 0x003E8514 File Offset: 0x003E6714
	internal AkSpatialAudioInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009669 RID: 38505 RVA: 0x003E852C File Offset: 0x003E672C
	public AkSpatialAudioInitSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSpatialAudioInitSettings(), true)
	{
	}

	// Token: 0x0600966A RID: 38506 RVA: 0x003E853C File Offset: 0x003E673C
	internal static IntPtr getCPtr(AkSpatialAudioInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600966B RID: 38507 RVA: 0x003E8554 File Offset: 0x003E6754
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600966C RID: 38508 RVA: 0x003E8564 File Offset: 0x003E6764
	~AkSpatialAudioInitSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600966D RID: 38509 RVA: 0x003E8594 File Offset: 0x003E6794
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSpatialAudioInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016C8 RID: 5832
	// (get) Token: 0x0600966F RID: 38511 RVA: 0x003E8618 File Offset: 0x003E6818
	// (set) Token: 0x0600966E RID: 38510 RVA: 0x003E8608 File Offset: 0x003E6808
	public int uPoolID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016C9 RID: 5833
	// (get) Token: 0x06009671 RID: 38513 RVA: 0x003E8638 File Offset: 0x003E6838
	// (set) Token: 0x06009670 RID: 38512 RVA: 0x003E8628 File Offset: 0x003E6828
	public uint uPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016CA RID: 5834
	// (get) Token: 0x06009673 RID: 38515 RVA: 0x003E8658 File Offset: 0x003E6858
	// (set) Token: 0x06009672 RID: 38514 RVA: 0x003E8648 File Offset: 0x003E6848
	public uint uMaxSoundPropagationDepth
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016CB RID: 5835
	// (get) Token: 0x06009675 RID: 38517 RVA: 0x003E8678 File Offset: 0x003E6878
	// (set) Token: 0x06009674 RID: 38516 RVA: 0x003E8668 File Offset: 0x003E6868
	public uint uDiffractionFlags
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016CC RID: 5836
	// (get) Token: 0x06009677 RID: 38519 RVA: 0x003E8698 File Offset: 0x003E6898
	// (set) Token: 0x06009676 RID: 38518 RVA: 0x003E8688 File Offset: 0x003E6888
	public float fDiffractionShadowAttenFactor
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016CD RID: 5837
	// (get) Token: 0x06009679 RID: 38521 RVA: 0x003E86B8 File Offset: 0x003E68B8
	// (set) Token: 0x06009678 RID: 38520 RVA: 0x003E86A8 File Offset: 0x003E68A8
	public float fDiffractionShadowDegrees
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CD0 RID: 40144
	private IntPtr swigCPtr;

	// Token: 0x04009CD1 RID: 40145
	protected bool swigCMemOwn;
}
