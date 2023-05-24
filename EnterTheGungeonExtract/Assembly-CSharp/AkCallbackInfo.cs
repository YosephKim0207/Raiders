using System;

// Token: 0x02001861 RID: 6241
public class AkCallbackInfo : IDisposable
{
	// Token: 0x0600939E RID: 37790 RVA: 0x003E3670 File Offset: 0x003E1870
	internal AkCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600939F RID: 37791 RVA: 0x003E3688 File Offset: 0x003E1888
	public AkCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkCallbackInfo(), true)
	{
	}

	// Token: 0x060093A0 RID: 37792 RVA: 0x003E3698 File Offset: 0x003E1898
	internal static IntPtr getCPtr(AkCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093A1 RID: 37793 RVA: 0x003E36B0 File Offset: 0x003E18B0
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093A2 RID: 37794 RVA: 0x003E36C0 File Offset: 0x003E18C0
	~AkCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x060093A3 RID: 37795 RVA: 0x003E36F0 File Offset: 0x003E18F0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015FD RID: 5629
	// (get) Token: 0x060093A4 RID: 37796 RVA: 0x003E3764 File Offset: 0x003E1964
	public IntPtr pCookie
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_pCookie_get(this.swigCPtr);
		}
	}

	// Token: 0x170015FE RID: 5630
	// (get) Token: 0x060093A5 RID: 37797 RVA: 0x003E3774 File Offset: 0x003E1974
	public ulong gameObjID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_gameObjID_get(this.swigCPtr);
		}
	}

	// Token: 0x04009AEE RID: 39662
	private IntPtr swigCPtr;

	// Token: 0x04009AEF RID: 39663
	protected bool swigCMemOwn;
}
