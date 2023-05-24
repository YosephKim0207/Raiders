using System;

// Token: 0x02001878 RID: 6264
public class AkMarkerCallbackInfo : AkEventCallbackInfo
{
	// Token: 0x06009480 RID: 38016 RVA: 0x003E4E30 File Offset: 0x003E3030
	internal AkMarkerCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009481 RID: 38017 RVA: 0x003E4E48 File Offset: 0x003E3048
	public AkMarkerCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMarkerCallbackInfo(), true)
	{
	}

	// Token: 0x06009482 RID: 38018 RVA: 0x003E4E58 File Offset: 0x003E3058
	internal static IntPtr getCPtr(AkMarkerCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009483 RID: 38019 RVA: 0x003E4E70 File Offset: 0x003E3070
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009484 RID: 38020 RVA: 0x003E4E88 File Offset: 0x003E3088
	~AkMarkerCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009485 RID: 38021 RVA: 0x003E4EB8 File Offset: 0x003E30B8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMarkerCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700163B RID: 5691
	// (get) Token: 0x06009486 RID: 38022 RVA: 0x003E4F34 File Offset: 0x003E3134
	public uint uIdentifier
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_uIdentifier_get(this.swigCPtr);
		}
	}

	// Token: 0x1700163C RID: 5692
	// (get) Token: 0x06009487 RID: 38023 RVA: 0x003E4F44 File Offset: 0x003E3144
	public uint uPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_uPosition_get(this.swigCPtr);
		}
	}

	// Token: 0x1700163D RID: 5693
	// (get) Token: 0x06009488 RID: 38024 RVA: 0x003E4F54 File Offset: 0x003E3154
	public string strLabel
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_strLabel_get(this.swigCPtr));
		}
	}

	// Token: 0x04009B4E RID: 39758
	private IntPtr swigCPtr;
}
