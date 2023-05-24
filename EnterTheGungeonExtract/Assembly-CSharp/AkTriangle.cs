using System;

// Token: 0x020018AA RID: 6314
public class AkTriangle : IDisposable
{
	// Token: 0x06009691 RID: 38545 RVA: 0x003E89D0 File Offset: 0x003E6BD0
	internal AkTriangle(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009692 RID: 38546 RVA: 0x003E89E8 File Offset: 0x003E6BE8
	public AkTriangle()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkTriangle(), true)
	{
	}

	// Token: 0x06009693 RID: 38547 RVA: 0x003E89F8 File Offset: 0x003E6BF8
	internal static IntPtr getCPtr(AkTriangle obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009694 RID: 38548 RVA: 0x003E8A10 File Offset: 0x003E6C10
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009695 RID: 38549 RVA: 0x003E8A20 File Offset: 0x003E6C20
	~AkTriangle()
	{
		this.Dispose();
	}

	// Token: 0x06009696 RID: 38550 RVA: 0x003E8A50 File Offset: 0x003E6C50
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkTriangle(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016CF RID: 5839
	// (get) Token: 0x06009698 RID: 38552 RVA: 0x003E8AD8 File Offset: 0x003E6CD8
	// (set) Token: 0x06009697 RID: 38551 RVA: 0x003E8AC4 File Offset: 0x003E6CC4
	public AkVector point0
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkTriangle_point0_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point0_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016D0 RID: 5840
	// (get) Token: 0x0600969A RID: 38554 RVA: 0x003E8B24 File Offset: 0x003E6D24
	// (set) Token: 0x06009699 RID: 38553 RVA: 0x003E8B10 File Offset: 0x003E6D10
	public AkVector point1
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkTriangle_point1_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point1_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016D1 RID: 5841
	// (get) Token: 0x0600969C RID: 38556 RVA: 0x003E8B70 File Offset: 0x003E6D70
	// (set) Token: 0x0600969B RID: 38555 RVA: 0x003E8B5C File Offset: 0x003E6D5C
	public AkVector point2
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkTriangle_point2_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point2_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016D2 RID: 5842
	// (get) Token: 0x0600969E RID: 38558 RVA: 0x003E8BB8 File Offset: 0x003E6DB8
	// (set) Token: 0x0600969D RID: 38557 RVA: 0x003E8BA8 File Offset: 0x003E6DA8
	public uint textureID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_textureID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_textureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016D3 RID: 5843
	// (get) Token: 0x060096A0 RID: 38560 RVA: 0x003E8BD8 File Offset: 0x003E6DD8
	// (set) Token: 0x0600969F RID: 38559 RVA: 0x003E8BC8 File Offset: 0x003E6DC8
	public uint reflectorChannelMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_reflectorChannelMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_reflectorChannelMask_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016D4 RID: 5844
	// (get) Token: 0x060096A2 RID: 38562 RVA: 0x003E8BF8 File Offset: 0x003E6DF8
	// (set) Token: 0x060096A1 RID: 38561 RVA: 0x003E8BE8 File Offset: 0x003E6DE8
	public string strName
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkTriangle_strName_get(this.swigCPtr));
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_strName_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CD6 RID: 40150
	private IntPtr swigCPtr;

	// Token: 0x04009CD7 RID: 40151
	protected bool swigCMemOwn;
}
