using System;

// Token: 0x0200188F RID: 6287
public class AkObstructionOcclusionValues : IDisposable
{
	// Token: 0x0600955B RID: 38235 RVA: 0x003E675C File Offset: 0x003E495C
	internal AkObstructionOcclusionValues(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600955C RID: 38236 RVA: 0x003E6774 File Offset: 0x003E4974
	public AkObstructionOcclusionValues()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkObstructionOcclusionValues(), true)
	{
	}

	// Token: 0x0600955D RID: 38237 RVA: 0x003E6784 File Offset: 0x003E4984
	internal static IntPtr getCPtr(AkObstructionOcclusionValues obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600955E RID: 38238 RVA: 0x003E679C File Offset: 0x003E499C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600955F RID: 38239 RVA: 0x003E67AC File Offset: 0x003E49AC
	~AkObstructionOcclusionValues()
	{
		this.Dispose();
	}

	// Token: 0x06009560 RID: 38240 RVA: 0x003E67DC File Offset: 0x003E49DC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkObstructionOcclusionValues(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001687 RID: 5767
	// (get) Token: 0x06009562 RID: 38242 RVA: 0x003E6860 File Offset: 0x003E4A60
	// (set) Token: 0x06009561 RID: 38241 RVA: 0x003E6850 File Offset: 0x003E4A50
	public float occlusion
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_occlusion_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_occlusion_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001688 RID: 5768
	// (get) Token: 0x06009564 RID: 38244 RVA: 0x003E6880 File Offset: 0x003E4A80
	// (set) Token: 0x06009563 RID: 38243 RVA: 0x003E6870 File Offset: 0x003E4A70
	public float obstruction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_obstruction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_obstruction_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C3E RID: 39998
	private IntPtr swigCPtr;

	// Token: 0x04009C3F RID: 39999
	protected bool swigCMemOwn;
}
