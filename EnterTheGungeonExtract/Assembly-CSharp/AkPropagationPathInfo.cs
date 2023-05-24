using System;

// Token: 0x0200189A RID: 6298
public class AkPropagationPathInfo : IDisposable
{
	// Token: 0x060095DC RID: 38364 RVA: 0x003E74B4 File Offset: 0x003E56B4
	internal AkPropagationPathInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095DD RID: 38365 RVA: 0x003E74CC File Offset: 0x003E56CC
	public AkPropagationPathInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPropagationPathInfo(), true)
	{
	}

	// Token: 0x060095DE RID: 38366 RVA: 0x003E74DC File Offset: 0x003E56DC
	internal static IntPtr getCPtr(AkPropagationPathInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060095DF RID: 38367 RVA: 0x003E74F4 File Offset: 0x003E56F4
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095E0 RID: 38368 RVA: 0x003E7504 File Offset: 0x003E5704
	~AkPropagationPathInfo()
	{
		this.Dispose();
	}

	// Token: 0x060095E1 RID: 38369 RVA: 0x003E7534 File Offset: 0x003E5734
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPropagationPathInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016A2 RID: 5794
	// (get) Token: 0x060095E3 RID: 38371 RVA: 0x003E75BC File Offset: 0x003E57BC
	// (set) Token: 0x060095E2 RID: 38370 RVA: 0x003E75A8 File Offset: 0x003E57A8
	public AkVector nodePoint
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_nodePoint_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_nodePoint_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016A3 RID: 5795
	// (get) Token: 0x060095E5 RID: 38373 RVA: 0x003E7604 File Offset: 0x003E5804
	// (set) Token: 0x060095E4 RID: 38372 RVA: 0x003E75F4 File Offset: 0x003E57F4
	public uint numNodes
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_numNodes_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_numNodes_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A4 RID: 5796
	// (get) Token: 0x060095E7 RID: 38375 RVA: 0x003E7624 File Offset: 0x003E5824
	// (set) Token: 0x060095E6 RID: 38374 RVA: 0x003E7614 File Offset: 0x003E5814
	public float length
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_length_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_length_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A5 RID: 5797
	// (get) Token: 0x060095E9 RID: 38377 RVA: 0x003E7644 File Offset: 0x003E5844
	// (set) Token: 0x060095E8 RID: 38376 RVA: 0x003E7634 File Offset: 0x003E5834
	public float gain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_gain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_gain_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A6 RID: 5798
	// (get) Token: 0x060095EB RID: 38379 RVA: 0x003E7664 File Offset: 0x003E5864
	// (set) Token: 0x060095EA RID: 38378 RVA: 0x003E7654 File Offset: 0x003E5854
	public float dryDiffractionAngle
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_dryDiffractionAngle_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_dryDiffractionAngle_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A7 RID: 5799
	// (get) Token: 0x060095ED RID: 38381 RVA: 0x003E7684 File Offset: 0x003E5884
	// (set) Token: 0x060095EC RID: 38380 RVA: 0x003E7674 File Offset: 0x003E5874
	public float wetDiffractionAngle
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_wetDiffractionAngle_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_wetDiffractionAngle_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C5E RID: 40030
	private IntPtr swigCPtr;

	// Token: 0x04009C5F RID: 40031
	protected bool swigCMemOwn;

	// Token: 0x04009C60 RID: 40032
	public const uint kMaxNodes = 8U;
}
