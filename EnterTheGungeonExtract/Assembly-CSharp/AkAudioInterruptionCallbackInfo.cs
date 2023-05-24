using System;

// Token: 0x0200185A RID: 6234
public class AkAudioInterruptionCallbackInfo : IDisposable
{
	// Token: 0x0600936C RID: 37740 RVA: 0x003E3010 File Offset: 0x003E1210
	internal AkAudioInterruptionCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600936D RID: 37741 RVA: 0x003E3028 File Offset: 0x003E1228
	public AkAudioInterruptionCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkAudioInterruptionCallbackInfo(), true)
	{
	}

	// Token: 0x0600936E RID: 37742 RVA: 0x003E3038 File Offset: 0x003E1238
	internal static IntPtr getCPtr(AkAudioInterruptionCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600936F RID: 37743 RVA: 0x003E3050 File Offset: 0x003E1250
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009370 RID: 37744 RVA: 0x003E3060 File Offset: 0x003E1260
	~AkAudioInterruptionCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009371 RID: 37745 RVA: 0x003E3090 File Offset: 0x003E1290
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioInterruptionCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015F2 RID: 5618
	// (get) Token: 0x06009372 RID: 37746 RVA: 0x003E3104 File Offset: 0x003E1304
	public bool bEnterInterruption
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioInterruptionCallbackInfo_bEnterInterruption_get(this.swigCPtr);
		}
	}

	// Token: 0x04009ADD RID: 39645
	private IntPtr swigCPtr;

	// Token: 0x04009ADE RID: 39646
	protected bool swigCMemOwn;
}
