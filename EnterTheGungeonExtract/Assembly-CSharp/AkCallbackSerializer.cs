using System;

// Token: 0x02001862 RID: 6242
public class AkCallbackSerializer : IDisposable
{
	// Token: 0x060093A6 RID: 37798 RVA: 0x003E3784 File Offset: 0x003E1984
	internal AkCallbackSerializer(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093A7 RID: 37799 RVA: 0x003E379C File Offset: 0x003E199C
	public AkCallbackSerializer()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkCallbackSerializer(), true)
	{
	}

	// Token: 0x060093A8 RID: 37800 RVA: 0x003E37AC File Offset: 0x003E19AC
	internal static IntPtr getCPtr(AkCallbackSerializer obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093A9 RID: 37801 RVA: 0x003E37C4 File Offset: 0x003E19C4
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093AA RID: 37802 RVA: 0x003E37D4 File Offset: 0x003E19D4
	~AkCallbackSerializer()
	{
		this.Dispose();
	}

	// Token: 0x060093AB RID: 37803 RVA: 0x003E3804 File Offset: 0x003E1A04
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkCallbackSerializer(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060093AC RID: 37804 RVA: 0x003E3878 File Offset: 0x003E1A78
	public static AKRESULT Init(IntPtr in_pMemory, uint in_uSize)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Init(in_pMemory, in_uSize);
	}

	// Token: 0x060093AD RID: 37805 RVA: 0x003E3884 File Offset: 0x003E1A84
	public static void Term()
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Term();
	}

	// Token: 0x060093AE RID: 37806 RVA: 0x003E388C File Offset: 0x003E1A8C
	public static IntPtr Lock()
	{
		return AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Lock();
	}

	// Token: 0x060093AF RID: 37807 RVA: 0x003E3894 File Offset: 0x003E1A94
	public static void SetLocalOutput(uint in_uErrorLevel)
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_SetLocalOutput(in_uErrorLevel);
	}

	// Token: 0x060093B0 RID: 37808 RVA: 0x003E389C File Offset: 0x003E1A9C
	public static void Unlock()
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Unlock();
	}

	// Token: 0x060093B1 RID: 37809 RVA: 0x003E38A4 File Offset: 0x003E1AA4
	public static AKRESULT AudioSourceChangeCallbackFunc(bool in_bOtherAudioPlaying, object in_pCookie)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_AudioSourceChangeCallbackFunc(in_bOtherAudioPlaying, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x04009AF0 RID: 39664
	private IntPtr swigCPtr;

	// Token: 0x04009AF1 RID: 39665
	protected bool swigCMemOwn;
}
