using System;

// Token: 0x020018A4 RID: 6308
public class AkSoundPropagationPathParams : IDisposable
{
	// Token: 0x06009650 RID: 38480 RVA: 0x003E8214 File Offset: 0x003E6414
	internal AkSoundPropagationPathParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009651 RID: 38481 RVA: 0x003E822C File Offset: 0x003E642C
	public AkSoundPropagationPathParams()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSoundPropagationPathParams(), true)
	{
	}

	// Token: 0x06009652 RID: 38482 RVA: 0x003E823C File Offset: 0x003E643C
	internal static IntPtr getCPtr(AkSoundPropagationPathParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009653 RID: 38483 RVA: 0x003E8254 File Offset: 0x003E6454
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009654 RID: 38484 RVA: 0x003E8264 File Offset: 0x003E6464
	~AkSoundPropagationPathParams()
	{
		this.Dispose();
	}

	// Token: 0x06009655 RID: 38485 RVA: 0x003E8294 File Offset: 0x003E6494
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSoundPropagationPathParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016C2 RID: 5826
	// (get) Token: 0x06009657 RID: 38487 RVA: 0x003E831C File Offset: 0x003E651C
	// (set) Token: 0x06009656 RID: 38486 RVA: 0x003E8308 File Offset: 0x003E6508
	public AkVector listenerPos
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_listenerPos_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_listenerPos_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016C3 RID: 5827
	// (get) Token: 0x06009659 RID: 38489 RVA: 0x003E8368 File Offset: 0x003E6568
	// (set) Token: 0x06009658 RID: 38488 RVA: 0x003E8354 File Offset: 0x003E6554
	public AkVector emitterPos
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_emitterPos_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_emitterPos_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016C4 RID: 5828
	// (get) Token: 0x0600965B RID: 38491 RVA: 0x003E83B0 File Offset: 0x003E65B0
	// (set) Token: 0x0600965A RID: 38490 RVA: 0x003E83A0 File Offset: 0x003E65A0
	public uint numValidPaths
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_numValidPaths_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSoundPropagationPathParams_numValidPaths_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CC8 RID: 40136
	private IntPtr swigCPtr;

	// Token: 0x04009CC9 RID: 40137
	protected bool swigCMemOwn;
}
