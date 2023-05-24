using System;

// Token: 0x02001853 RID: 6227
public class _ArrayPoolDefault : IDisposable
{
	// Token: 0x0600933E RID: 37694 RVA: 0x003E2AC0 File Offset: 0x003E0CC0
	internal _ArrayPoolDefault(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600933F RID: 37695 RVA: 0x003E2AD8 File Offset: 0x003E0CD8
	public _ArrayPoolDefault()
		: this(AkSoundEnginePINVOKE.CSharp_new__ArrayPoolDefault(), true)
	{
	}

	// Token: 0x06009340 RID: 37696 RVA: 0x003E2AE8 File Offset: 0x003E0CE8
	internal static IntPtr getCPtr(_ArrayPoolDefault obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009341 RID: 37697 RVA: 0x003E2B00 File Offset: 0x003E0D00
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009342 RID: 37698 RVA: 0x003E2B10 File Offset: 0x003E0D10
	~_ArrayPoolDefault()
	{
		this.Dispose();
	}

	// Token: 0x06009343 RID: 37699 RVA: 0x003E2B40 File Offset: 0x003E0D40
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete__ArrayPoolDefault(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06009344 RID: 37700 RVA: 0x003E2BB4 File Offset: 0x003E0DB4
	public static int Get()
	{
		return AkSoundEnginePINVOKE.CSharp__ArrayPoolDefault_Get();
	}

	// Token: 0x04009AC5 RID: 39621
	private IntPtr swigCPtr;

	// Token: 0x04009AC6 RID: 39622
	protected bool swigCMemOwn;
}
