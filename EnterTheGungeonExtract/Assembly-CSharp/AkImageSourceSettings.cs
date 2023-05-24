using System;

// Token: 0x02001875 RID: 6261
public class AkImageSourceSettings : IDisposable
{
	// Token: 0x06009442 RID: 37954 RVA: 0x003E47E4 File Offset: 0x003E29E4
	internal AkImageSourceSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009443 RID: 37955 RVA: 0x003E47FC File Offset: 0x003E29FC
	public AkImageSourceSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceSettings__SWIG_0(), true)
	{
	}

	// Token: 0x06009444 RID: 37956 RVA: 0x003E480C File Offset: 0x003E2A0C
	public AkImageSourceSettings(AkVector in_sourcePosition, float in_fDistanceScalingFactor, float in_fLevel)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceSettings__SWIG_1(AkVector.getCPtr(in_sourcePosition), in_fDistanceScalingFactor, in_fLevel), true)
	{
	}

	// Token: 0x06009445 RID: 37957 RVA: 0x003E4824 File Offset: 0x003E2A24
	internal static IntPtr getCPtr(AkImageSourceSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009446 RID: 37958 RVA: 0x003E483C File Offset: 0x003E2A3C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009447 RID: 37959 RVA: 0x003E484C File Offset: 0x003E2A4C
	~AkImageSourceSettings()
	{
		this.Dispose();
	}

	// Token: 0x06009448 RID: 37960 RVA: 0x003E487C File Offset: 0x003E2A7C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkImageSourceSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06009449 RID: 37961 RVA: 0x003E48F0 File Offset: 0x003E2AF0
	public void SetOneTexture(uint in_texture)
	{
		AkSoundEnginePINVOKE.CSharp_AkImageSourceSettings_SetOneTexture(this.swigCPtr, in_texture);
	}

	// Token: 0x0600944A RID: 37962 RVA: 0x003E4900 File Offset: 0x003E2B00
	public void SetName(string in_pName)
	{
		AkSoundEnginePINVOKE.CSharp_AkImageSourceSettings_SetName(this.swigCPtr, in_pName);
	}

	// Token: 0x17001629 RID: 5673
	// (get) Token: 0x0600944C RID: 37964 RVA: 0x003E4924 File Offset: 0x003E2B24
	// (set) Token: 0x0600944B RID: 37963 RVA: 0x003E4910 File Offset: 0x003E2B10
	public AkImageSourceParams params_
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkImageSourceSettings_params__get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkImageSourceParams(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceSettings_params__set(this.swigCPtr, AkImageSourceParams.getCPtr(value));
		}
	}

	// Token: 0x04009B48 RID: 39752
	private IntPtr swigCPtr;

	// Token: 0x04009B49 RID: 39753
	protected bool swigCMemOwn;
}
