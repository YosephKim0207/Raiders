using System;

// Token: 0x0200185C RID: 6236
public class AkAudioSourceChangeCallbackInfo : IDisposable
{
	// Token: 0x0600937D RID: 37757 RVA: 0x003E3248 File Offset: 0x003E1448
	internal AkAudioSourceChangeCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600937E RID: 37758 RVA: 0x003E3260 File Offset: 0x003E1460
	public AkAudioSourceChangeCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkAudioSourceChangeCallbackInfo(), true)
	{
	}

	// Token: 0x0600937F RID: 37759 RVA: 0x003E3270 File Offset: 0x003E1470
	internal static IntPtr getCPtr(AkAudioSourceChangeCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009380 RID: 37760 RVA: 0x003E3288 File Offset: 0x003E1488
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009381 RID: 37761 RVA: 0x003E3298 File Offset: 0x003E1498
	~AkAudioSourceChangeCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009382 RID: 37762 RVA: 0x003E32C8 File Offset: 0x003E14C8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioSourceChangeCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015F5 RID: 5621
	// (get) Token: 0x06009383 RID: 37763 RVA: 0x003E333C File Offset: 0x003E153C
	public bool bOtherAudioPlaying
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSourceChangeCallbackInfo_bOtherAudioPlaying_get(this.swigCPtr);
		}
	}

	// Token: 0x04009AE1 RID: 39649
	private IntPtr swigCPtr;

	// Token: 0x04009AE2 RID: 39650
	protected bool swigCMemOwn;
}
