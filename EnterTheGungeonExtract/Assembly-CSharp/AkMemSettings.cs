using System;

// Token: 0x02001879 RID: 6265
public class AkMemSettings : IDisposable
{
	// Token: 0x06009489 RID: 38025 RVA: 0x003E4F68 File Offset: 0x003E3168
	internal AkMemSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600948A RID: 38026 RVA: 0x003E4F80 File Offset: 0x003E3180
	public AkMemSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMemSettings(), true)
	{
	}

	// Token: 0x0600948B RID: 38027 RVA: 0x003E4F90 File Offset: 0x003E3190
	internal static IntPtr getCPtr(AkMemSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600948C RID: 38028 RVA: 0x003E4FA8 File Offset: 0x003E31A8
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600948D RID: 38029 RVA: 0x003E4FB8 File Offset: 0x003E31B8
	~AkMemSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600948E RID: 38030 RVA: 0x003E4FE8 File Offset: 0x003E31E8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMemSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700163E RID: 5694
	// (get) Token: 0x06009490 RID: 38032 RVA: 0x003E506C File Offset: 0x003E326C
	// (set) Token: 0x0600948F RID: 38031 RVA: 0x003E505C File Offset: 0x003E325C
	public uint uMaxNumPools
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMemSettings_uMaxNumPools_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMemSettings_uMaxNumPools_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700163F RID: 5695
	// (get) Token: 0x06009492 RID: 38034 RVA: 0x003E508C File Offset: 0x003E328C
	// (set) Token: 0x06009491 RID: 38033 RVA: 0x003E507C File Offset: 0x003E327C
	public uint uDebugFlags
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMemSettings_uDebugFlags_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMemSettings_uDebugFlags_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B4F RID: 39759
	private IntPtr swigCPtr;

	// Token: 0x04009B50 RID: 39760
	protected bool swigCMemOwn;
}
