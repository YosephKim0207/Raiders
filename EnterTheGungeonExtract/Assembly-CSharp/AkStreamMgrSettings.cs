using System;

// Token: 0x020018A8 RID: 6312
public class AkStreamMgrSettings : IDisposable
{
	// Token: 0x0600967A RID: 38522 RVA: 0x003E86C8 File Offset: 0x003E68C8
	internal AkStreamMgrSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600967B RID: 38523 RVA: 0x003E86E0 File Offset: 0x003E68E0
	public AkStreamMgrSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkStreamMgrSettings(), true)
	{
	}

	// Token: 0x0600967C RID: 38524 RVA: 0x003E86F0 File Offset: 0x003E68F0
	internal static IntPtr getCPtr(AkStreamMgrSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600967D RID: 38525 RVA: 0x003E8708 File Offset: 0x003E6908
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600967E RID: 38526 RVA: 0x003E8718 File Offset: 0x003E6918
	~AkStreamMgrSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600967F RID: 38527 RVA: 0x003E8748 File Offset: 0x003E6948
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkStreamMgrSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016CE RID: 5838
	// (get) Token: 0x06009681 RID: 38529 RVA: 0x003E87CC File Offset: 0x003E69CC
	// (set) Token: 0x06009680 RID: 38528 RVA: 0x003E87BC File Offset: 0x003E69BC
	public uint uMemorySize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkStreamMgrSettings_uMemorySize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkStreamMgrSettings_uMemorySize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CD2 RID: 40146
	private IntPtr swigCPtr;

	// Token: 0x04009CD3 RID: 40147
	protected bool swigCMemOwn;
}
