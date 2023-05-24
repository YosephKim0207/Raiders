using System;

// Token: 0x02001893 RID: 6291
public class AkPlaylist : AkPlaylistArray
{
	// Token: 0x06009577 RID: 38263 RVA: 0x003E6A80 File Offset: 0x003E4C80
	internal AkPlaylist(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkPlaylist_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009578 RID: 38264 RVA: 0x003E6A98 File Offset: 0x003E4C98
	public AkPlaylist()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylist(), true)
	{
	}

	// Token: 0x06009579 RID: 38265 RVA: 0x003E6AA8 File Offset: 0x003E4CA8
	internal static IntPtr getCPtr(AkPlaylist obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600957A RID: 38266 RVA: 0x003E6AC0 File Offset: 0x003E4CC0
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkPlaylist_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600957B RID: 38267 RVA: 0x003E6AD8 File Offset: 0x003E4CD8
	~AkPlaylist()
	{
		this.Dispose();
	}

	// Token: 0x0600957C RID: 38268 RVA: 0x003E6B08 File Offset: 0x003E4D08
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPlaylist(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600957D RID: 38269 RVA: 0x003E6B84 File Offset: 0x003E4D84
	public AKRESULT Enqueue(uint in_audioNodeID, int in_msDelay, IntPtr in_pCustomInfo, uint in_cExternals, AkExternalSourceInfo in_pExternalSources)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylist_Enqueue__SWIG_0(this.swigCPtr, in_audioNodeID, in_msDelay, in_pCustomInfo, in_cExternals, AkExternalSourceInfo.getCPtr(in_pExternalSources));
	}

	// Token: 0x0600957E RID: 38270 RVA: 0x003E6BA0 File Offset: 0x003E4DA0
	public AKRESULT Enqueue(uint in_audioNodeID, int in_msDelay, IntPtr in_pCustomInfo, uint in_cExternals)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylist_Enqueue__SWIG_1(this.swigCPtr, in_audioNodeID, in_msDelay, in_pCustomInfo, in_cExternals);
	}

	// Token: 0x0600957F RID: 38271 RVA: 0x003E6BB4 File Offset: 0x003E4DB4
	public AKRESULT Enqueue(uint in_audioNodeID, int in_msDelay, IntPtr in_pCustomInfo)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylist_Enqueue__SWIG_2(this.swigCPtr, in_audioNodeID, in_msDelay, in_pCustomInfo);
	}

	// Token: 0x06009580 RID: 38272 RVA: 0x003E6BC4 File Offset: 0x003E4DC4
	public AKRESULT Enqueue(uint in_audioNodeID, int in_msDelay)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylist_Enqueue__SWIG_3(this.swigCPtr, in_audioNodeID, in_msDelay);
	}

	// Token: 0x06009581 RID: 38273 RVA: 0x003E6BD4 File Offset: 0x003E4DD4
	public AKRESULT Enqueue(uint in_audioNodeID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylist_Enqueue__SWIG_4(this.swigCPtr, in_audioNodeID);
	}

	// Token: 0x04009C48 RID: 40008
	private IntPtr swigCPtr;
}
