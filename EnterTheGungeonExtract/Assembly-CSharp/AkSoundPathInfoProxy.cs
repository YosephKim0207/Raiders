using System;

// Token: 0x020018A3 RID: 6307
public class AkSoundPathInfoProxy : AkSoundPathInfo
{
	// Token: 0x06009647 RID: 38471 RVA: 0x003E80A8 File Offset: 0x003E62A8
	internal AkSoundPathInfoProxy(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009648 RID: 38472 RVA: 0x003E80C0 File Offset: 0x003E62C0
	public AkSoundPathInfoProxy()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkSoundPathInfoProxy(), true)
	{
	}

	// Token: 0x06009649 RID: 38473 RVA: 0x003E80D0 File Offset: 0x003E62D0
	internal static IntPtr getCPtr(AkSoundPathInfoProxy obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600964A RID: 38474 RVA: 0x003E80E8 File Offset: 0x003E62E8
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600964B RID: 38475 RVA: 0x003E8100 File Offset: 0x003E6300
	~AkSoundPathInfoProxy()
	{
		this.Dispose();
	}

	// Token: 0x0600964C RID: 38476 RVA: 0x003E8130 File Offset: 0x003E6330
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSoundPathInfoProxy(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600964D RID: 38477 RVA: 0x003E81AC File Offset: 0x003E63AC
	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_GetSizeOf();
	}

	// Token: 0x0600964E RID: 38478 RVA: 0x003E81B4 File Offset: 0x003E63B4
	public AkVector GetReflectionPoint(uint idx)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_GetReflectionPoint(this.swigCPtr, idx);
		return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
	}

	// Token: 0x0600964F RID: 38479 RVA: 0x003E81F0 File Offset: 0x003E63F0
	public AkTriangle GetTriangle(uint idx)
	{
		return new AkTriangle(AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_GetTriangle(this.swigCPtr, idx), false);
	}

	// Token: 0x04009CC7 RID: 40135
	private IntPtr swigCPtr;
}
