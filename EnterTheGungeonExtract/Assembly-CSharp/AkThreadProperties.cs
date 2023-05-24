using System;

// Token: 0x020018B4 RID: 6324
public class AkThreadProperties : IDisposable
{
	// Token: 0x06009C22 RID: 39970 RVA: 0x003EB760 File Offset: 0x003E9960
	internal AkThreadProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009C23 RID: 39971 RVA: 0x003EB778 File Offset: 0x003E9978
	public AkThreadProperties()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkThreadProperties(), true)
	{
	}

	// Token: 0x06009C24 RID: 39972 RVA: 0x003EB788 File Offset: 0x003E9988
	internal static IntPtr getCPtr(AkThreadProperties obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009C25 RID: 39973 RVA: 0x003EB7A0 File Offset: 0x003E99A0
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009C26 RID: 39974 RVA: 0x003EB7B0 File Offset: 0x003E99B0
	~AkThreadProperties()
	{
		this.Dispose();
	}

	// Token: 0x06009C27 RID: 39975 RVA: 0x003EB7E0 File Offset: 0x003E99E0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkThreadProperties(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016EE RID: 5870
	// (get) Token: 0x06009C29 RID: 39977 RVA: 0x003EB864 File Offset: 0x003E9A64
	// (set) Token: 0x06009C28 RID: 39976 RVA: 0x003EB854 File Offset: 0x003E9A54
	public int nPriority
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_nPriority_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkThreadProperties_nPriority_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016EF RID: 5871
	// (get) Token: 0x06009C2B RID: 39979 RVA: 0x003EB884 File Offset: 0x003E9A84
	// (set) Token: 0x06009C2A RID: 39978 RVA: 0x003EB874 File Offset: 0x003E9A74
	public uint dwAffinityMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_dwAffinityMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkThreadProperties_dwAffinityMask_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016F0 RID: 5872
	// (get) Token: 0x06009C2D RID: 39981 RVA: 0x003EB8A4 File Offset: 0x003E9AA4
	// (set) Token: 0x06009C2C RID: 39980 RVA: 0x003EB894 File Offset: 0x003E9A94
	public uint uStackSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uStackSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uStackSize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009E30 RID: 40496
	private IntPtr swigCPtr;

	// Token: 0x04009E31 RID: 40497
	protected bool swigCMemOwn;
}
