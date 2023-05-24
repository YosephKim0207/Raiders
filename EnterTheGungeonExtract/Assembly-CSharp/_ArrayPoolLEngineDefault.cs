using System;

// Token: 0x02001854 RID: 6228
public class _ArrayPoolLEngineDefault : IDisposable
{
	// Token: 0x06009345 RID: 37701 RVA: 0x003E2BBC File Offset: 0x003E0DBC
	internal _ArrayPoolLEngineDefault(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009346 RID: 37702 RVA: 0x003E2BD4 File Offset: 0x003E0DD4
	public _ArrayPoolLEngineDefault()
		: this(AkSoundEnginePINVOKE.CSharp_new__ArrayPoolLEngineDefault(), true)
	{
	}

	// Token: 0x06009347 RID: 37703 RVA: 0x003E2BE4 File Offset: 0x003E0DE4
	internal static IntPtr getCPtr(_ArrayPoolLEngineDefault obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009348 RID: 37704 RVA: 0x003E2BFC File Offset: 0x003E0DFC
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009349 RID: 37705 RVA: 0x003E2C0C File Offset: 0x003E0E0C
	~_ArrayPoolLEngineDefault()
	{
		this.Dispose();
	}

	// Token: 0x0600934A RID: 37706 RVA: 0x003E2C3C File Offset: 0x003E0E3C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete__ArrayPoolLEngineDefault(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600934B RID: 37707 RVA: 0x003E2CB0 File Offset: 0x003E0EB0
	public static int Get()
	{
		return AkSoundEnginePINVOKE.CSharp__ArrayPoolLEngineDefault_Get();
	}

	// Token: 0x04009AC7 RID: 39623
	private IntPtr swigCPtr;

	// Token: 0x04009AC8 RID: 39624
	protected bool swigCMemOwn;
}
