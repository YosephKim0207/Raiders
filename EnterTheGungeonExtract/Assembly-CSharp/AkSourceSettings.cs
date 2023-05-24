using System;

// Token: 0x020018A6 RID: 6310
public class AkSourceSettings : IDisposable
{
	// Token: 0x0600965C RID: 38492 RVA: 0x003E83C0 File Offset: 0x003E65C0
	internal AkSourceSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600965D RID: 38493 RVA: 0x003E83D8 File Offset: 0x003E65D8
	public AkSourceSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSourceSettings(), true)
	{
	}

	// Token: 0x0600965E RID: 38494 RVA: 0x003E83E8 File Offset: 0x003E65E8
	internal static IntPtr getCPtr(AkSourceSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600965F RID: 38495 RVA: 0x003E8400 File Offset: 0x003E6600
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009660 RID: 38496 RVA: 0x003E8410 File Offset: 0x003E6610
	~AkSourceSettings()
	{
		this.Dispose();
	}

	// Token: 0x06009661 RID: 38497 RVA: 0x003E8440 File Offset: 0x003E6640
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSourceSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016C5 RID: 5829
	// (get) Token: 0x06009663 RID: 38499 RVA: 0x003E84C4 File Offset: 0x003E66C4
	// (set) Token: 0x06009662 RID: 38498 RVA: 0x003E84B4 File Offset: 0x003E66B4
	public uint sourceID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_sourceID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_sourceID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016C6 RID: 5830
	// (get) Token: 0x06009665 RID: 38501 RVA: 0x003E84E4 File Offset: 0x003E66E4
	// (set) Token: 0x06009664 RID: 38500 RVA: 0x003E84D4 File Offset: 0x003E66D4
	public IntPtr pMediaMemory
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_pMediaMemory_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_pMediaMemory_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016C7 RID: 5831
	// (get) Token: 0x06009667 RID: 38503 RVA: 0x003E8504 File Offset: 0x003E6704
	// (set) Token: 0x06009666 RID: 38502 RVA: 0x003E84F4 File Offset: 0x003E66F4
	public uint uMediaSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_uMediaSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_uMediaSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CCE RID: 40142
	private IntPtr swigCPtr;

	// Token: 0x04009CCF RID: 40143
	protected bool swigCMemOwn;
}
