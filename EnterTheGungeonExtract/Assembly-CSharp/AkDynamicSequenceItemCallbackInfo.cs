using System;

// Token: 0x0200186D RID: 6253
public class AkDynamicSequenceItemCallbackInfo : AkCallbackInfo
{
	// Token: 0x060093FB RID: 37883 RVA: 0x003E4030 File Offset: 0x003E2230
	internal AkDynamicSequenceItemCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093FC RID: 37884 RVA: 0x003E4048 File Offset: 0x003E2248
	public AkDynamicSequenceItemCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkDynamicSequenceItemCallbackInfo(), true)
	{
	}

	// Token: 0x060093FD RID: 37885 RVA: 0x003E4058 File Offset: 0x003E2258
	internal static IntPtr getCPtr(AkDynamicSequenceItemCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093FE RID: 37886 RVA: 0x003E4070 File Offset: 0x003E2270
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093FF RID: 37887 RVA: 0x003E4088 File Offset: 0x003E2288
	~AkDynamicSequenceItemCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009400 RID: 37888 RVA: 0x003E40B8 File Offset: 0x003E22B8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkDynamicSequenceItemCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17001614 RID: 5652
	// (get) Token: 0x06009401 RID: 37889 RVA: 0x003E4134 File Offset: 0x003E2334
	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001615 RID: 5653
	// (get) Token: 0x06009402 RID: 37890 RVA: 0x003E4144 File Offset: 0x003E2344
	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_audioNodeID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001616 RID: 5654
	// (get) Token: 0x06009403 RID: 37891 RVA: 0x003E4154 File Offset: 0x003E2354
	public IntPtr pCustomInfo
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_pCustomInfo_get(this.swigCPtr);
		}
	}

	// Token: 0x04009B2F RID: 39727
	private IntPtr swigCPtr;
}
