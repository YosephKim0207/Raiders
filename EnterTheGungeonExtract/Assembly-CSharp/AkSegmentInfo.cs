using System;

// Token: 0x020018A0 RID: 6304
public class AkSegmentInfo : IDisposable
{
	// Token: 0x06009617 RID: 38423 RVA: 0x003E7B6C File Offset: 0x003E5D6C
	internal AkSegmentInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009618 RID: 38424 RVA: 0x003E7B84 File Offset: 0x003E5D84
	public AkSegmentInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSegmentInfo(), true)
	{
	}

	// Token: 0x06009619 RID: 38425 RVA: 0x003E7B94 File Offset: 0x003E5D94
	internal static IntPtr getCPtr(AkSegmentInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600961A RID: 38426 RVA: 0x003E7BAC File Offset: 0x003E5DAC
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600961B RID: 38427 RVA: 0x003E7BBC File Offset: 0x003E5DBC
	~AkSegmentInfo()
	{
		this.Dispose();
	}

	// Token: 0x0600961C RID: 38428 RVA: 0x003E7BEC File Offset: 0x003E5DEC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSegmentInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016B2 RID: 5810
	// (get) Token: 0x0600961E RID: 38430 RVA: 0x003E7C70 File Offset: 0x003E5E70
	// (set) Token: 0x0600961D RID: 38429 RVA: 0x003E7C60 File Offset: 0x003E5E60
	public int iCurrentPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iCurrentPosition_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iCurrentPosition_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B3 RID: 5811
	// (get) Token: 0x06009620 RID: 38432 RVA: 0x003E7C90 File Offset: 0x003E5E90
	// (set) Token: 0x0600961F RID: 38431 RVA: 0x003E7C80 File Offset: 0x003E5E80
	public int iPreEntryDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPreEntryDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPreEntryDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B4 RID: 5812
	// (get) Token: 0x06009622 RID: 38434 RVA: 0x003E7CB0 File Offset: 0x003E5EB0
	// (set) Token: 0x06009621 RID: 38433 RVA: 0x003E7CA0 File Offset: 0x003E5EA0
	public int iActiveDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iActiveDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iActiveDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B5 RID: 5813
	// (get) Token: 0x06009624 RID: 38436 RVA: 0x003E7CD0 File Offset: 0x003E5ED0
	// (set) Token: 0x06009623 RID: 38435 RVA: 0x003E7CC0 File Offset: 0x003E5EC0
	public int iPostExitDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPostExitDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPostExitDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B6 RID: 5814
	// (get) Token: 0x06009626 RID: 38438 RVA: 0x003E7CF0 File Offset: 0x003E5EF0
	// (set) Token: 0x06009625 RID: 38437 RVA: 0x003E7CE0 File Offset: 0x003E5EE0
	public int iRemainingLookAheadTime
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iRemainingLookAheadTime_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iRemainingLookAheadTime_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B7 RID: 5815
	// (get) Token: 0x06009628 RID: 38440 RVA: 0x003E7D10 File Offset: 0x003E5F10
	// (set) Token: 0x06009627 RID: 38439 RVA: 0x003E7D00 File Offset: 0x003E5F00
	public float fBeatDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBeatDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBeatDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B8 RID: 5816
	// (get) Token: 0x0600962A RID: 38442 RVA: 0x003E7D30 File Offset: 0x003E5F30
	// (set) Token: 0x06009629 RID: 38441 RVA: 0x003E7D20 File Offset: 0x003E5F20
	public float fBarDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBarDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBarDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B9 RID: 5817
	// (get) Token: 0x0600962C RID: 38444 RVA: 0x003E7D50 File Offset: 0x003E5F50
	// (set) Token: 0x0600962B RID: 38443 RVA: 0x003E7D40 File Offset: 0x003E5F40
	public float fGridDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridDuration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016BA RID: 5818
	// (get) Token: 0x0600962E RID: 38446 RVA: 0x003E7D70 File Offset: 0x003E5F70
	// (set) Token: 0x0600962D RID: 38445 RVA: 0x003E7D60 File Offset: 0x003E5F60
	public float fGridOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridOffset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridOffset_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CC1 RID: 40129
	private IntPtr swigCPtr;

	// Token: 0x04009CC2 RID: 40130
	protected bool swigCMemOwn;
}
