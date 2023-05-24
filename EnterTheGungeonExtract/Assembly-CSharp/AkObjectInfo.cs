using System;

// Token: 0x0200188E RID: 6286
public class AkObjectInfo : IDisposable
{
	// Token: 0x0600954F RID: 38223 RVA: 0x003E6608 File Offset: 0x003E4808
	internal AkObjectInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009550 RID: 38224 RVA: 0x003E6620 File Offset: 0x003E4820
	public AkObjectInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkObjectInfo(), true)
	{
	}

	// Token: 0x06009551 RID: 38225 RVA: 0x003E6630 File Offset: 0x003E4830
	internal static IntPtr getCPtr(AkObjectInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009552 RID: 38226 RVA: 0x003E6648 File Offset: 0x003E4848
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009553 RID: 38227 RVA: 0x003E6658 File Offset: 0x003E4858
	~AkObjectInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009554 RID: 38228 RVA: 0x003E6688 File Offset: 0x003E4888
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkObjectInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001684 RID: 5764
	// (get) Token: 0x06009556 RID: 38230 RVA: 0x003E670C File Offset: 0x003E490C
	// (set) Token: 0x06009555 RID: 38229 RVA: 0x003E66FC File Offset: 0x003E48FC
	public uint objID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_objID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_objID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001685 RID: 5765
	// (get) Token: 0x06009558 RID: 38232 RVA: 0x003E672C File Offset: 0x003E492C
	// (set) Token: 0x06009557 RID: 38231 RVA: 0x003E671C File Offset: 0x003E491C
	public uint parentID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_parentID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_parentID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001686 RID: 5766
	// (get) Token: 0x0600955A RID: 38234 RVA: 0x003E674C File Offset: 0x003E494C
	// (set) Token: 0x06009559 RID: 38233 RVA: 0x003E673C File Offset: 0x003E493C
	public int iDepth
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_iDepth_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_iDepth_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009C3C RID: 39996
	private IntPtr swigCPtr;

	// Token: 0x04009C3D RID: 39997
	protected bool swigCMemOwn;
}
