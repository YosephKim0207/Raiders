using System;

// Token: 0x0200185E RID: 6238
public class AkBankCallbackInfo : IDisposable
{
	// Token: 0x06009394 RID: 37780 RVA: 0x003E353C File Offset: 0x003E173C
	internal AkBankCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009395 RID: 37781 RVA: 0x003E3554 File Offset: 0x003E1754
	public AkBankCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkBankCallbackInfo(), true)
	{
	}

	// Token: 0x06009396 RID: 37782 RVA: 0x003E3564 File Offset: 0x003E1764
	internal static IntPtr getCPtr(AkBankCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009397 RID: 37783 RVA: 0x003E357C File Offset: 0x003E177C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009398 RID: 37784 RVA: 0x003E358C File Offset: 0x003E178C
	~AkBankCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009399 RID: 37785 RVA: 0x003E35BC File Offset: 0x003E17BC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkBankCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015F9 RID: 5625
	// (get) Token: 0x0600939A RID: 37786 RVA: 0x003E3630 File Offset: 0x003E1830
	public uint bankID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_bankID_get(this.swigCPtr);
		}
	}

	// Token: 0x170015FA RID: 5626
	// (get) Token: 0x0600939B RID: 37787 RVA: 0x003E3640 File Offset: 0x003E1840
	public IntPtr inMemoryBankPtr
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_inMemoryBankPtr_get(this.swigCPtr);
		}
	}

	// Token: 0x170015FB RID: 5627
	// (get) Token: 0x0600939C RID: 37788 RVA: 0x003E3650 File Offset: 0x003E1850
	public AKRESULT loadResult
	{
		get
		{
			return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_loadResult_get(this.swigCPtr);
		}
	}

	// Token: 0x170015FC RID: 5628
	// (get) Token: 0x0600939D RID: 37789 RVA: 0x003E3660 File Offset: 0x003E1860
	public int memPoolId
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_memPoolId_get(this.swigCPtr);
		}
	}

	// Token: 0x04009AE5 RID: 39653
	private IntPtr swigCPtr;

	// Token: 0x04009AE6 RID: 39654
	protected bool swigCMemOwn;
}
