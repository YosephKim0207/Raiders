using System;

// Token: 0x0200188B RID: 6283
public class AkMusicPlaylistCallbackInfo : AkEventCallbackInfo
{
	// Token: 0x0600952B RID: 38187 RVA: 0x003E61E8 File Offset: 0x003E43E8
	internal AkMusicPlaylistCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600952C RID: 38188 RVA: 0x003E6200 File Offset: 0x003E4400
	public AkMusicPlaylistCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMusicPlaylistCallbackInfo(), true)
	{
	}

	// Token: 0x0600952D RID: 38189 RVA: 0x003E6210 File Offset: 0x003E4410
	internal static IntPtr getCPtr(AkMusicPlaylistCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600952E RID: 38190 RVA: 0x003E6228 File Offset: 0x003E4428
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600952F RID: 38191 RVA: 0x003E6240 File Offset: 0x003E4440
	~AkMusicPlaylistCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009530 RID: 38192 RVA: 0x003E6270 File Offset: 0x003E4470
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMusicPlaylistCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17001673 RID: 5747
	// (get) Token: 0x06009531 RID: 38193 RVA: 0x003E62EC File Offset: 0x003E44EC
	public uint playlistID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_playlistID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001674 RID: 5748
	// (get) Token: 0x06009532 RID: 38194 RVA: 0x003E62FC File Offset: 0x003E44FC
	public uint uNumPlaylistItems
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_uNumPlaylistItems_get(this.swigCPtr);
		}
	}

	// Token: 0x17001675 RID: 5749
	// (get) Token: 0x06009533 RID: 38195 RVA: 0x003E630C File Offset: 0x003E450C
	public uint uPlaylistSelection
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_uPlaylistSelection_get(this.swigCPtr);
		}
	}

	// Token: 0x17001676 RID: 5750
	// (get) Token: 0x06009534 RID: 38196 RVA: 0x003E631C File Offset: 0x003E451C
	public uint uPlaylistItemDone
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicPlaylistCallbackInfo_uPlaylistItemDone_get(this.swigCPtr);
		}
	}

	// Token: 0x04009C38 RID: 39992
	private IntPtr swigCPtr;
}
