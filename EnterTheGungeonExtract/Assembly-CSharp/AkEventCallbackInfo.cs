using System;

// Token: 0x02001870 RID: 6256
public class AkEventCallbackInfo : AkCallbackInfo
{
	// Token: 0x06009418 RID: 37912 RVA: 0x003E4338 File Offset: 0x003E2538
	internal AkEventCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009419 RID: 37913 RVA: 0x003E4350 File Offset: 0x003E2550
	public AkEventCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkEventCallbackInfo(), true)
	{
	}

	// Token: 0x0600941A RID: 37914 RVA: 0x003E4360 File Offset: 0x003E2560
	internal static IntPtr getCPtr(AkEventCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600941B RID: 37915 RVA: 0x003E4378 File Offset: 0x003E2578
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600941C RID: 37916 RVA: 0x003E4390 File Offset: 0x003E2590
	~AkEventCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x0600941D RID: 37917 RVA: 0x003E43C0 File Offset: 0x003E25C0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkEventCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700161E RID: 5662
	// (get) Token: 0x0600941E RID: 37918 RVA: 0x003E443C File Offset: 0x003E263C
	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	// Token: 0x1700161F RID: 5663
	// (get) Token: 0x0600941F RID: 37919 RVA: 0x003E444C File Offset: 0x003E264C
	public uint eventID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_eventID_get(this.swigCPtr);
		}
	}

	// Token: 0x04009B35 RID: 39733
	private IntPtr swigCPtr;
}
