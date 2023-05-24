using System;

// Token: 0x02001897 RID: 6295
public class AkPositioningInfo : IDisposable
{
	// Token: 0x060095B2 RID: 38322 RVA: 0x003E7180 File Offset: 0x003E5380
	internal AkPositioningInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095B3 RID: 38323 RVA: 0x003E7198 File Offset: 0x003E5398
	public AkPositioningInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPositioningInfo(), true)
	{
	}

	// Token: 0x060095B4 RID: 38324 RVA: 0x003E71A8 File Offset: 0x003E53A8
	internal static IntPtr getCPtr(AkPositioningInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060095B5 RID: 38325 RVA: 0x003E71C0 File Offset: 0x003E53C0
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095B6 RID: 38326 RVA: 0x003E71D0 File Offset: 0x003E53D0
	~AkPositioningInfo()
	{
		this.Dispose();
	}

	// Token: 0x060095B7 RID: 38327 RVA: 0x003E7200 File Offset: 0x003E5400
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPositioningInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001690 RID: 5776
	// (get) Token: 0x060095B9 RID: 38329 RVA: 0x003E7284 File Offset: 0x003E5484
	// (set) Token: 0x060095B8 RID: 38328 RVA: 0x003E7274 File Offset: 0x003E5474
	public float fCenterPct
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fCenterPct_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fCenterPct_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001691 RID: 5777
	// (get) Token: 0x060095BB RID: 38331 RVA: 0x003E72A4 File Offset: 0x003E54A4
	// (set) Token: 0x060095BA RID: 38330 RVA: 0x003E7294 File Offset: 0x003E5494
	public AkPannerType pannerType
	{
		get
		{
			return (AkPannerType)AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_pannerType_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_pannerType_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17001692 RID: 5778
	// (get) Token: 0x060095BD RID: 38333 RVA: 0x003E72C4 File Offset: 0x003E54C4
	// (set) Token: 0x060095BC RID: 38332 RVA: 0x003E72B4 File Offset: 0x003E54B4
	public AkPositionSourceType posSourceType
	{
		get
		{
			return (AkPositionSourceType)AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_posSourceType_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_posSourceType_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17001693 RID: 5779
	// (get) Token: 0x060095BF RID: 38335 RVA: 0x003E72E4 File Offset: 0x003E54E4
	// (set) Token: 0x060095BE RID: 38334 RVA: 0x003E72D4 File Offset: 0x003E54D4
	public bool bUpdateEachFrame
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUpdateEachFrame_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUpdateEachFrame_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001694 RID: 5780
	// (get) Token: 0x060095C1 RID: 38337 RVA: 0x003E7304 File Offset: 0x003E5504
	// (set) Token: 0x060095C0 RID: 38336 RVA: 0x003E72F4 File Offset: 0x003E54F4
	public Ak3DSpatializationMode e3DSpatializationMode
	{
		get
		{
			return (Ak3DSpatializationMode)AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_e3DSpatializationMode_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_e3DSpatializationMode_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17001695 RID: 5781
	// (get) Token: 0x060095C3 RID: 38339 RVA: 0x003E7324 File Offset: 0x003E5524
	// (set) Token: 0x060095C2 RID: 38338 RVA: 0x003E7314 File Offset: 0x003E5514
	public bool bUseAttenuation
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUseAttenuation_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUseAttenuation_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001696 RID: 5782
	// (get) Token: 0x060095C5 RID: 38341 RVA: 0x003E7344 File Offset: 0x003E5544
	// (set) Token: 0x060095C4 RID: 38340 RVA: 0x003E7334 File Offset: 0x003E5534
	public bool bUseConeAttenuation
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUseConeAttenuation_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_bUseConeAttenuation_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001697 RID: 5783
	// (get) Token: 0x060095C7 RID: 38343 RVA: 0x003E7364 File Offset: 0x003E5564
	// (set) Token: 0x060095C6 RID: 38342 RVA: 0x003E7354 File Offset: 0x003E5554
	public float fInnerAngle
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fInnerAngle_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fInnerAngle_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001698 RID: 5784
	// (get) Token: 0x060095C9 RID: 38345 RVA: 0x003E7384 File Offset: 0x003E5584
	// (set) Token: 0x060095C8 RID: 38344 RVA: 0x003E7374 File Offset: 0x003E5574
	public float fOuterAngle
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fOuterAngle_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fOuterAngle_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001699 RID: 5785
	// (get) Token: 0x060095CB RID: 38347 RVA: 0x003E73A4 File Offset: 0x003E55A4
	// (set) Token: 0x060095CA RID: 38346 RVA: 0x003E7394 File Offset: 0x003E5594
	public float fConeMaxAttenuation
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fConeMaxAttenuation_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fConeMaxAttenuation_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169A RID: 5786
	// (get) Token: 0x060095CD RID: 38349 RVA: 0x003E73C4 File Offset: 0x003E55C4
	// (set) Token: 0x060095CC RID: 38348 RVA: 0x003E73B4 File Offset: 0x003E55B4
	public float LPFCone
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_LPFCone_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_LPFCone_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169B RID: 5787
	// (get) Token: 0x060095CF RID: 38351 RVA: 0x003E73E4 File Offset: 0x003E55E4
	// (set) Token: 0x060095CE RID: 38350 RVA: 0x003E73D4 File Offset: 0x003E55D4
	public float HPFCone
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_HPFCone_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_HPFCone_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169C RID: 5788
	// (get) Token: 0x060095D1 RID: 38353 RVA: 0x003E7404 File Offset: 0x003E5604
	// (set) Token: 0x060095D0 RID: 38352 RVA: 0x003E73F4 File Offset: 0x003E55F4
	public float fMaxDistance
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fMaxDistance_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fMaxDistance_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169D RID: 5789
	// (get) Token: 0x060095D3 RID: 38355 RVA: 0x003E7424 File Offset: 0x003E5624
	// (set) Token: 0x060095D2 RID: 38354 RVA: 0x003E7414 File Offset: 0x003E5614
	public float fVolDryAtMaxDist
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolDryAtMaxDist_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolDryAtMaxDist_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169E RID: 5790
	// (get) Token: 0x060095D5 RID: 38357 RVA: 0x003E7444 File Offset: 0x003E5644
	// (set) Token: 0x060095D4 RID: 38356 RVA: 0x003E7434 File Offset: 0x003E5634
	public float fVolAuxGameDefAtMaxDist
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolAuxGameDefAtMaxDist_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolAuxGameDefAtMaxDist_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700169F RID: 5791
	// (get) Token: 0x060095D7 RID: 38359 RVA: 0x003E7464 File Offset: 0x003E5664
	// (set) Token: 0x060095D6 RID: 38358 RVA: 0x003E7454 File Offset: 0x003E5654
	public float fVolAuxUserDefAtMaxDist
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolAuxUserDefAtMaxDist_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_fVolAuxUserDefAtMaxDist_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A0 RID: 5792
	// (get) Token: 0x060095D9 RID: 38361 RVA: 0x003E7484 File Offset: 0x003E5684
	// (set) Token: 0x060095D8 RID: 38360 RVA: 0x003E7474 File Offset: 0x003E5674
	public float LPFValueAtMaxDist
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_LPFValueAtMaxDist_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_LPFValueAtMaxDist_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A1 RID: 5793
	// (get) Token: 0x060095DB RID: 38363 RVA: 0x003E74A4 File Offset: 0x003E56A4
	// (set) Token: 0x060095DA RID: 38362 RVA: 0x003E7494 File Offset: 0x003E5694
	public float HPFValueAtMaxDist
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_HPFValueAtMaxDist_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPositioningInfo_HPFValueAtMaxDist_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C55 RID: 40021
	private IntPtr swigCPtr;

	// Token: 0x04009C56 RID: 40022
	protected bool swigCMemOwn;
}
