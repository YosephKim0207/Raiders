using System;

// Token: 0x0200188C RID: 6284
public class AkMusicSettings : IDisposable
{
	// Token: 0x06009535 RID: 38197 RVA: 0x003E632C File Offset: 0x003E452C
	internal AkMusicSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009536 RID: 38198 RVA: 0x003E6344 File Offset: 0x003E4544
	public AkMusicSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMusicSettings(), true)
	{
	}

	// Token: 0x06009537 RID: 38199 RVA: 0x003E6354 File Offset: 0x003E4554
	internal static IntPtr getCPtr(AkMusicSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009538 RID: 38200 RVA: 0x003E636C File Offset: 0x003E456C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009539 RID: 38201 RVA: 0x003E637C File Offset: 0x003E457C
	~AkMusicSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600953A RID: 38202 RVA: 0x003E63AC File Offset: 0x003E45AC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMusicSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001677 RID: 5751
	// (get) Token: 0x0600953C RID: 38204 RVA: 0x003E6430 File Offset: 0x003E4630
	// (set) Token: 0x0600953B RID: 38203 RVA: 0x003E6420 File Offset: 0x003E4620
	public float fStreamingLookAheadRatio
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSettings_fStreamingLookAheadRatio_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMusicSettings_fStreamingLookAheadRatio_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C39 RID: 39993
	private IntPtr swigCPtr;

	// Token: 0x04009C3A RID: 39994
	protected bool swigCMemOwn;
}
