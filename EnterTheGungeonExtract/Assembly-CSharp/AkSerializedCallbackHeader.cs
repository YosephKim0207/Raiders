using System;

// Token: 0x020018A1 RID: 6305
public class AkSerializedCallbackHeader : IDisposable
{
	// Token: 0x0600962F RID: 38447 RVA: 0x003E7D80 File Offset: 0x003E5F80
	internal AkSerializedCallbackHeader(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009630 RID: 38448 RVA: 0x003E7D98 File Offset: 0x003E5F98
	public AkSerializedCallbackHeader()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSerializedCallbackHeader(), true)
	{
	}

	// Token: 0x06009631 RID: 38449 RVA: 0x003E7DA8 File Offset: 0x003E5FA8
	internal static IntPtr getCPtr(AkSerializedCallbackHeader obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009632 RID: 38450 RVA: 0x003E7DC0 File Offset: 0x003E5FC0
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009633 RID: 38451 RVA: 0x003E7DD0 File Offset: 0x003E5FD0
	~AkSerializedCallbackHeader()
	{
		this.Dispose();
	}

	// Token: 0x06009634 RID: 38452 RVA: 0x003E7E00 File Offset: 0x003E6000
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSerializedCallbackHeader(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016BB RID: 5819
	// (get) Token: 0x06009635 RID: 38453 RVA: 0x003E7E74 File Offset: 0x003E6074
	public IntPtr pPackage
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pPackage_get(this.swigCPtr);
		}
	}

	// Token: 0x170016BC RID: 5820
	// (get) Token: 0x06009636 RID: 38454 RVA: 0x003E7E84 File Offset: 0x003E6084
	public AkSerializedCallbackHeader pNext
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pNext_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkSerializedCallbackHeader(intPtr, false) : null;
		}
	}

	// Token: 0x170016BD RID: 5821
	// (get) Token: 0x06009637 RID: 38455 RVA: 0x003E7EBC File Offset: 0x003E60BC
	public AkCallbackType eType
	{
		get
		{
			return (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_eType_get(this.swigCPtr);
		}
	}

	// Token: 0x06009638 RID: 38456 RVA: 0x003E7ECC File Offset: 0x003E60CC
	public IntPtr GetData()
	{
		return AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_GetData(this.swigCPtr);
	}

	// Token: 0x04009CC3 RID: 40131
	private IntPtr swigCPtr;

	// Token: 0x04009CC4 RID: 40132
	protected bool swigCMemOwn;
}
