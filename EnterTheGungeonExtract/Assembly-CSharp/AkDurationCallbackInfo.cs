using System;

// Token: 0x0200186C RID: 6252
public class AkDurationCallbackInfo : AkEventCallbackInfo
{
	// Token: 0x060093F0 RID: 37872 RVA: 0x003E3EDC File Offset: 0x003E20DC
	internal AkDurationCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093F1 RID: 37873 RVA: 0x003E3EF4 File Offset: 0x003E20F4
	public AkDurationCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkDurationCallbackInfo(), true)
	{
	}

	// Token: 0x060093F2 RID: 37874 RVA: 0x003E3F04 File Offset: 0x003E2104
	internal static IntPtr getCPtr(AkDurationCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093F3 RID: 37875 RVA: 0x003E3F1C File Offset: 0x003E211C
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093F4 RID: 37876 RVA: 0x003E3F34 File Offset: 0x003E2134
	~AkDurationCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x060093F5 RID: 37877 RVA: 0x003E3F64 File Offset: 0x003E2164
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkDurationCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700160F RID: 5647
	// (get) Token: 0x060093F6 RID: 37878 RVA: 0x003E3FE0 File Offset: 0x003E21E0
	public float fDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_fDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x17001610 RID: 5648
	// (get) Token: 0x060093F7 RID: 37879 RVA: 0x003E3FF0 File Offset: 0x003E21F0
	public float fEstimatedDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_fEstimatedDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x17001611 RID: 5649
	// (get) Token: 0x060093F8 RID: 37880 RVA: 0x003E4000 File Offset: 0x003E2200
	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_audioNodeID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001612 RID: 5650
	// (get) Token: 0x060093F9 RID: 37881 RVA: 0x003E4010 File Offset: 0x003E2210
	public uint mediaID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_mediaID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001613 RID: 5651
	// (get) Token: 0x060093FA RID: 37882 RVA: 0x003E4020 File Offset: 0x003E2220
	public bool bStreaming
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_bStreaming_get(this.swigCPtr);
		}
	}

	// Token: 0x04009B2E RID: 39726
	private IntPtr swigCPtr;
}
