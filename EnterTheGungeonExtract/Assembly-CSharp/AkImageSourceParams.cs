using System;

// Token: 0x02001874 RID: 6260
public class AkImageSourceParams : IDisposable
{
	// Token: 0x06009435 RID: 37941 RVA: 0x003E464C File Offset: 0x003E284C
	internal AkImageSourceParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009436 RID: 37942 RVA: 0x003E4664 File Offset: 0x003E2864
	public AkImageSourceParams()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceParams__SWIG_0(), true)
	{
	}

	// Token: 0x06009437 RID: 37943 RVA: 0x003E4674 File Offset: 0x003E2874
	public AkImageSourceParams(AkVector in_sourcePosition, float in_fDistanceScalingFactor, float in_fLevel)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceParams__SWIG_1(AkVector.getCPtr(in_sourcePosition), in_fDistanceScalingFactor, in_fLevel), true)
	{
	}

	// Token: 0x06009438 RID: 37944 RVA: 0x003E468C File Offset: 0x003E288C
	internal static IntPtr getCPtr(AkImageSourceParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009439 RID: 37945 RVA: 0x003E46A4 File Offset: 0x003E28A4
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600943A RID: 37946 RVA: 0x003E46B4 File Offset: 0x003E28B4
	~AkImageSourceParams()
	{
		this.Dispose();
	}

	// Token: 0x0600943B RID: 37947 RVA: 0x003E46E4 File Offset: 0x003E28E4
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkImageSourceParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001626 RID: 5670
	// (get) Token: 0x0600943D RID: 37949 RVA: 0x003E476C File Offset: 0x003E296C
	// (set) Token: 0x0600943C RID: 37948 RVA: 0x003E4758 File Offset: 0x003E2958
	public AkVector sourcePosition
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_sourcePosition_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_sourcePosition_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x17001627 RID: 5671
	// (get) Token: 0x0600943F RID: 37951 RVA: 0x003E47B4 File Offset: 0x003E29B4
	// (set) Token: 0x0600943E RID: 37950 RVA: 0x003E47A4 File Offset: 0x003E29A4
	public float fDistanceScalingFactor
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDistanceScalingFactor_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDistanceScalingFactor_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001628 RID: 5672
	// (get) Token: 0x06009441 RID: 37953 RVA: 0x003E47D4 File Offset: 0x003E29D4
	// (set) Token: 0x06009440 RID: 37952 RVA: 0x003E47C4 File Offset: 0x003E29C4
	public float fLevel
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fLevel_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fLevel_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B46 RID: 39750
	private IntPtr swigCPtr;

	// Token: 0x04009B47 RID: 39751
	protected bool swigCMemOwn;
}
