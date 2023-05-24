using System;

// Token: 0x02001889 RID: 6281
public class AkMonitoringCallbackInfo : IDisposable
{
	// Token: 0x06009520 RID: 38176 RVA: 0x003E60A0 File Offset: 0x003E42A0
	internal AkMonitoringCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009521 RID: 38177 RVA: 0x003E60B8 File Offset: 0x003E42B8
	public AkMonitoringCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMonitoringCallbackInfo(), true)
	{
	}

	// Token: 0x06009522 RID: 38178 RVA: 0x003E60C8 File Offset: 0x003E42C8
	internal static IntPtr getCPtr(AkMonitoringCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009523 RID: 38179 RVA: 0x003E60E0 File Offset: 0x003E42E0
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009524 RID: 38180 RVA: 0x003E60F0 File Offset: 0x003E42F0
	~AkMonitoringCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009525 RID: 38181 RVA: 0x003E6120 File Offset: 0x003E4320
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMonitoringCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700166E RID: 5742
	// (get) Token: 0x06009526 RID: 38182 RVA: 0x003E6194 File Offset: 0x003E4394
	public AkMonitorErrorCode errorCode
	{
		get
		{
			return (AkMonitorErrorCode)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorCode_get(this.swigCPtr);
		}
	}

	// Token: 0x1700166F RID: 5743
	// (get) Token: 0x06009527 RID: 38183 RVA: 0x003E61A4 File Offset: 0x003E43A4
	public AkMonitorErrorLevel errorLevel
	{
		get
		{
			return (AkMonitorErrorLevel)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorLevel_get(this.swigCPtr);
		}
	}

	// Token: 0x17001670 RID: 5744
	// (get) Token: 0x06009528 RID: 38184 RVA: 0x003E61B4 File Offset: 0x003E43B4
	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001671 RID: 5745
	// (get) Token: 0x06009529 RID: 38185 RVA: 0x003E61C4 File Offset: 0x003E43C4
	public ulong gameObjID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_gameObjID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001672 RID: 5746
	// (get) Token: 0x0600952A RID: 38186 RVA: 0x003E61D4 File Offset: 0x003E43D4
	public string message
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_message_get(this.swigCPtr));
		}
	}

	// Token: 0x04009C32 RID: 39986
	private IntPtr swigCPtr;

	// Token: 0x04009C33 RID: 39987
	protected bool swigCMemOwn;
}
