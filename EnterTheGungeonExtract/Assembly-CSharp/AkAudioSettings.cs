using System;

// Token: 0x0200185B RID: 6235
public class AkAudioSettings : IDisposable
{
	// Token: 0x06009373 RID: 37747 RVA: 0x003E3114 File Offset: 0x003E1314
	internal AkAudioSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009374 RID: 37748 RVA: 0x003E312C File Offset: 0x003E132C
	public AkAudioSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkAudioSettings(), true)
	{
	}

	// Token: 0x06009375 RID: 37749 RVA: 0x003E313C File Offset: 0x003E133C
	internal static IntPtr getCPtr(AkAudioSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009376 RID: 37750 RVA: 0x003E3154 File Offset: 0x003E1354
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009377 RID: 37751 RVA: 0x003E3164 File Offset: 0x003E1364
	~AkAudioSettings()
	{
		this.Dispose();
	}

	// Token: 0x06009378 RID: 37752 RVA: 0x003E3194 File Offset: 0x003E1394
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015F3 RID: 5619
	// (get) Token: 0x0600937A RID: 37754 RVA: 0x003E3218 File Offset: 0x003E1418
	// (set) Token: 0x06009379 RID: 37753 RVA: 0x003E3208 File Offset: 0x003E1408
	public uint uNumSamplesPerFrame
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerFrame_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerFrame_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015F4 RID: 5620
	// (get) Token: 0x0600937C RID: 37756 RVA: 0x003E3238 File Offset: 0x003E1438
	// (set) Token: 0x0600937B RID: 37755 RVA: 0x003E3228 File Offset: 0x003E1428
	public uint uNumSamplesPerSecond
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerSecond_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerSecond_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009ADF RID: 39647
	private IntPtr swigCPtr;

	// Token: 0x04009AE0 RID: 39648
	protected bool swigCMemOwn;
}
