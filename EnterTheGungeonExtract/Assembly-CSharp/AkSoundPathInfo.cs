using System;

// Token: 0x020018A2 RID: 6306
public class AkSoundPathInfo : IDisposable
{
	// Token: 0x06009639 RID: 38457 RVA: 0x003E7EDC File Offset: 0x003E60DC
	internal AkSoundPathInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600963A RID: 38458 RVA: 0x003E7EF4 File Offset: 0x003E60F4
	public AkSoundPathInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSoundPathInfo(), true)
	{
	}

	// Token: 0x0600963B RID: 38459 RVA: 0x003E7F04 File Offset: 0x003E6104
	internal static IntPtr getCPtr(AkSoundPathInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600963C RID: 38460 RVA: 0x003E7F1C File Offset: 0x003E611C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600963D RID: 38461 RVA: 0x003E7F2C File Offset: 0x003E612C
	~AkSoundPathInfo()
	{
		this.Dispose();
	}

	// Token: 0x0600963E RID: 38462 RVA: 0x003E7F5C File Offset: 0x003E615C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSoundPathInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016BE RID: 5822
	// (get) Token: 0x06009640 RID: 38464 RVA: 0x003E7FE4 File Offset: 0x003E61E4
	// (set) Token: 0x0600963F RID: 38463 RVA: 0x003E7FD0 File Offset: 0x003E61D0
	public AkVector imageSource
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_imageSource_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_imageSource_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016BF RID: 5823
	// (get) Token: 0x06009642 RID: 38466 RVA: 0x003E802C File Offset: 0x003E622C
	// (set) Token: 0x06009641 RID: 38465 RVA: 0x003E801C File Offset: 0x003E621C
	public uint numReflections
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_numReflections_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_numReflections_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016C0 RID: 5824
	// (get) Token: 0x06009644 RID: 38468 RVA: 0x003E8050 File Offset: 0x003E6250
	// (set) Token: 0x06009643 RID: 38467 RVA: 0x003E803C File Offset: 0x003E623C
	public AkVector occlusionPoint
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_occlusionPoint_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_occlusionPoint_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016C1 RID: 5825
	// (get) Token: 0x06009646 RID: 38470 RVA: 0x003E8098 File Offset: 0x003E6298
	// (set) Token: 0x06009645 RID: 38469 RVA: 0x003E8088 File Offset: 0x003E6288
	public bool isOccluded
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_isOccluded_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPathInfo_isOccluded_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CC5 RID: 40133
	private IntPtr swigCPtr;

	// Token: 0x04009CC6 RID: 40134
	protected bool swigCMemOwn;
}
