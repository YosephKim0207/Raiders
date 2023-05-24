using System;

// Token: 0x02001895 RID: 6293
public class AkPlaylistItem : IDisposable
{
	// Token: 0x060095A2 RID: 38306 RVA: 0x003E6FC8 File Offset: 0x003E51C8
	internal AkPlaylistItem(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095A3 RID: 38307 RVA: 0x003E6FE0 File Offset: 0x003E51E0
	public AkPlaylistItem()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistItem__SWIG_0(), true)
	{
	}

	// Token: 0x060095A4 RID: 38308 RVA: 0x003E6FF0 File Offset: 0x003E51F0
	public AkPlaylistItem(AkPlaylistItem in_rCopy)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistItem__SWIG_1(AkPlaylistItem.getCPtr(in_rCopy)), true)
	{
	}

	// Token: 0x060095A5 RID: 38309 RVA: 0x003E7004 File Offset: 0x003E5204
	internal static IntPtr getCPtr(AkPlaylistItem obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060095A6 RID: 38310 RVA: 0x003E701C File Offset: 0x003E521C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095A7 RID: 38311 RVA: 0x003E702C File Offset: 0x003E522C
	~AkPlaylistItem()
	{
		this.Dispose();
	}

	// Token: 0x060095A8 RID: 38312 RVA: 0x003E705C File Offset: 0x003E525C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPlaylistItem(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060095A9 RID: 38313 RVA: 0x003E70D0 File Offset: 0x003E52D0
	public AkPlaylistItem Assign(AkPlaylistItem in_rCopy)
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_Assign(this.swigCPtr, AkPlaylistItem.getCPtr(in_rCopy)), false);
	}

	// Token: 0x060095AA RID: 38314 RVA: 0x003E70F8 File Offset: 0x003E52F8
	public bool IsEqualTo(AkPlaylistItem in_rCopy)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_IsEqualTo(this.swigCPtr, AkPlaylistItem.getCPtr(in_rCopy));
	}

	// Token: 0x060095AB RID: 38315 RVA: 0x003E710C File Offset: 0x003E530C
	public AKRESULT SetExternalSources(uint in_nExternalSrc, AkExternalSourceInfo in_pExternalSrc)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_SetExternalSources(this.swigCPtr, in_nExternalSrc, AkExternalSourceInfo.getCPtr(in_pExternalSrc));
	}

	// Token: 0x1700168D RID: 5773
	// (get) Token: 0x060095AD RID: 38317 RVA: 0x003E7130 File Offset: 0x003E5330
	// (set) Token: 0x060095AC RID: 38316 RVA: 0x003E7120 File Offset: 0x003E5320
	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_audioNodeID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_audioNodeID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700168E RID: 5774
	// (get) Token: 0x060095AF RID: 38319 RVA: 0x003E7150 File Offset: 0x003E5350
	// (set) Token: 0x060095AE RID: 38318 RVA: 0x003E7140 File Offset: 0x003E5340
	public int msDelay
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_msDelay_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_msDelay_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700168F RID: 5775
	// (get) Token: 0x060095B1 RID: 38321 RVA: 0x003E7170 File Offset: 0x003E5370
	// (set) Token: 0x060095B0 RID: 38320 RVA: 0x003E7160 File Offset: 0x003E5360
	public IntPtr pCustomInfo
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_pCustomInfo_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_pCustomInfo_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C4B RID: 40011
	private IntPtr swigCPtr;

	// Token: 0x04009C4C RID: 40012
	protected bool swigCMemOwn;
}
