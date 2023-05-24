using System;

// Token: 0x02001855 RID: 6229
public class _ArrayPoolSpatialAudio : IDisposable
{
	// Token: 0x0600934C RID: 37708 RVA: 0x003E2CB8 File Offset: 0x003E0EB8
	internal _ArrayPoolSpatialAudio(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600934D RID: 37709 RVA: 0x003E2CD0 File Offset: 0x003E0ED0
	public _ArrayPoolSpatialAudio()
		: this(AkSoundEnginePINVOKE.CSharp_new__ArrayPoolSpatialAudio(), true)
	{
	}

	// Token: 0x0600934E RID: 37710 RVA: 0x003E2CE0 File Offset: 0x003E0EE0
	internal static IntPtr getCPtr(_ArrayPoolSpatialAudio obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600934F RID: 37711 RVA: 0x003E2CF8 File Offset: 0x003E0EF8
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009350 RID: 37712 RVA: 0x003E2D08 File Offset: 0x003E0F08
	~_ArrayPoolSpatialAudio()
	{
		this.Dispose();
	}

	// Token: 0x06009351 RID: 37713 RVA: 0x003E2D38 File Offset: 0x003E0F38
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete__ArrayPoolSpatialAudio(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06009352 RID: 37714 RVA: 0x003E2DAC File Offset: 0x003E0FAC
	public static int Get()
	{
		return AkSoundEnginePINVOKE.CSharp__ArrayPoolSpatialAudio_Get();
	}

	// Token: 0x04009AC9 RID: 39625
	private IntPtr swigCPtr;

	// Token: 0x04009ACA RID: 39626
	protected bool swigCMemOwn;
}
