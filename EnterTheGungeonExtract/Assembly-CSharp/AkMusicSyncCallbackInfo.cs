using System;

// Token: 0x0200188D RID: 6285
public class AkMusicSyncCallbackInfo : AkCallbackInfo
{
	// Token: 0x0600953D RID: 38205 RVA: 0x003E6440 File Offset: 0x003E4640
	internal AkMusicSyncCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600953E RID: 38206 RVA: 0x003E6458 File Offset: 0x003E4658
	public AkMusicSyncCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMusicSyncCallbackInfo(), true)
	{
	}

	// Token: 0x0600953F RID: 38207 RVA: 0x003E6468 File Offset: 0x003E4668
	internal static IntPtr getCPtr(AkMusicSyncCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009540 RID: 38208 RVA: 0x003E6480 File Offset: 0x003E4680
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009541 RID: 38209 RVA: 0x003E6498 File Offset: 0x003E4698
	~AkMusicSyncCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009542 RID: 38210 RVA: 0x003E64C8 File Offset: 0x003E46C8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMusicSyncCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17001678 RID: 5752
	// (get) Token: 0x06009543 RID: 38211 RVA: 0x003E6544 File Offset: 0x003E4744
	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	// Token: 0x17001679 RID: 5753
	// (get) Token: 0x06009544 RID: 38212 RVA: 0x003E6554 File Offset: 0x003E4754
	public int segmentInfo_iCurrentPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iCurrentPosition_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167A RID: 5754
	// (get) Token: 0x06009545 RID: 38213 RVA: 0x003E6564 File Offset: 0x003E4764
	public int segmentInfo_iPreEntryDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPreEntryDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167B RID: 5755
	// (get) Token: 0x06009546 RID: 38214 RVA: 0x003E6574 File Offset: 0x003E4774
	public int segmentInfo_iActiveDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iActiveDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167C RID: 5756
	// (get) Token: 0x06009547 RID: 38215 RVA: 0x003E6584 File Offset: 0x003E4784
	public int segmentInfo_iPostExitDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPostExitDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167D RID: 5757
	// (get) Token: 0x06009548 RID: 38216 RVA: 0x003E6594 File Offset: 0x003E4794
	public int segmentInfo_iRemainingLookAheadTime
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iRemainingLookAheadTime_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167E RID: 5758
	// (get) Token: 0x06009549 RID: 38217 RVA: 0x003E65A4 File Offset: 0x003E47A4
	public float segmentInfo_fBeatDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBeatDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x1700167F RID: 5759
	// (get) Token: 0x0600954A RID: 38218 RVA: 0x003E65B4 File Offset: 0x003E47B4
	public float segmentInfo_fBarDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBarDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x17001680 RID: 5760
	// (get) Token: 0x0600954B RID: 38219 RVA: 0x003E65C4 File Offset: 0x003E47C4
	public float segmentInfo_fGridDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridDuration_get(this.swigCPtr);
		}
	}

	// Token: 0x17001681 RID: 5761
	// (get) Token: 0x0600954C RID: 38220 RVA: 0x003E65D4 File Offset: 0x003E47D4
	public float segmentInfo_fGridOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridOffset_get(this.swigCPtr);
		}
	}

	// Token: 0x17001682 RID: 5762
	// (get) Token: 0x0600954D RID: 38221 RVA: 0x003E65E4 File Offset: 0x003E47E4
	public AkCallbackType musicSyncType
	{
		get
		{
			return (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_musicSyncType_get(this.swigCPtr);
		}
	}

	// Token: 0x17001683 RID: 5763
	// (get) Token: 0x0600954E RID: 38222 RVA: 0x003E65F4 File Offset: 0x003E47F4
	public string userCueName
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_userCueName_get(this.swigCPtr));
		}
	}

	// Token: 0x04009C3B RID: 39995
	private IntPtr swigCPtr;
}
