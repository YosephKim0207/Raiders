using System;

// Token: 0x0200189D RID: 6301
public class AkRamp : IDisposable
{
	// Token: 0x060095F6 RID: 38390 RVA: 0x003E77DC File Offset: 0x003E59DC
	internal AkRamp(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095F7 RID: 38391 RVA: 0x003E77F4 File Offset: 0x003E59F4
	public AkRamp()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkRamp__SWIG_0(), true)
	{
	}

	// Token: 0x060095F8 RID: 38392 RVA: 0x003E7804 File Offset: 0x003E5A04
	public AkRamp(float in_fPrev, float in_fNext)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkRamp__SWIG_1(in_fPrev, in_fNext), true)
	{
	}

	// Token: 0x060095F9 RID: 38393 RVA: 0x003E7814 File Offset: 0x003E5A14
	internal static IntPtr getCPtr(AkRamp obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060095FA RID: 38394 RVA: 0x003E782C File Offset: 0x003E5A2C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095FB RID: 38395 RVA: 0x003E783C File Offset: 0x003E5A3C
	~AkRamp()
	{
		this.Dispose();
	}

	// Token: 0x060095FC RID: 38396 RVA: 0x003E786C File Offset: 0x003E5A6C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkRamp(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016A8 RID: 5800
	// (get) Token: 0x060095FE RID: 38398 RVA: 0x003E78F0 File Offset: 0x003E5AF0
	// (set) Token: 0x060095FD RID: 38397 RVA: 0x003E78E0 File Offset: 0x003E5AE0
	public float fPrev
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRamp_fPrev_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRamp_fPrev_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016A9 RID: 5801
	// (get) Token: 0x06009600 RID: 38400 RVA: 0x003E7910 File Offset: 0x003E5B10
	// (set) Token: 0x060095FF RID: 38399 RVA: 0x003E7900 File Offset: 0x003E5B00
	public float fNext
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRamp_fNext_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRamp_fNext_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C68 RID: 40040
	private IntPtr swigCPtr;

	// Token: 0x04009C69 RID: 40041
	protected bool swigCMemOwn;
}
