using System;

// Token: 0x020018AC RID: 6316
public class AkVector : IDisposable
{
	// Token: 0x060096AC RID: 38572 RVA: 0x003E8D38 File Offset: 0x003E6F38
	internal AkVector(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096AD RID: 38573 RVA: 0x003E8D50 File Offset: 0x003E6F50
	public AkVector()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkVector(), true)
	{
	}

	// Token: 0x060096AE RID: 38574 RVA: 0x003E8D60 File Offset: 0x003E6F60
	internal static IntPtr getCPtr(AkVector obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060096AF RID: 38575 RVA: 0x003E8D78 File Offset: 0x003E6F78
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096B0 RID: 38576 RVA: 0x003E8D88 File Offset: 0x003E6F88
	~AkVector()
	{
		this.Dispose();
	}

	// Token: 0x060096B1 RID: 38577 RVA: 0x003E8DB8 File Offset: 0x003E6FB8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkVector(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060096B2 RID: 38578 RVA: 0x003E8E2C File Offset: 0x003E702C
	public void Zero()
	{
		AkSoundEnginePINVOKE.CSharp_AkVector_Zero(this.swigCPtr);
	}

	// Token: 0x170016D5 RID: 5845
	// (get) Token: 0x060096B4 RID: 38580 RVA: 0x003E8E4C File Offset: 0x003E704C
	// (set) Token: 0x060096B3 RID: 38579 RVA: 0x003E8E3C File Offset: 0x003E703C
	public float X
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_X_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_X_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016D6 RID: 5846
	// (get) Token: 0x060096B6 RID: 38582 RVA: 0x003E8E6C File Offset: 0x003E706C
	// (set) Token: 0x060096B5 RID: 38581 RVA: 0x003E8E5C File Offset: 0x003E705C
	public float Y
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_Y_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_Y_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016D7 RID: 5847
	// (get) Token: 0x060096B8 RID: 38584 RVA: 0x003E8E8C File Offset: 0x003E708C
	// (set) Token: 0x060096B7 RID: 38583 RVA: 0x003E8E7C File Offset: 0x003E707C
	public float Z
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_Z_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_Z_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CD9 RID: 40153
	private IntPtr swigCPtr;

	// Token: 0x04009CDA RID: 40154
	protected bool swigCMemOwn;
}
