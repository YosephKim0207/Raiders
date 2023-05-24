using System;

// Token: 0x0200189B RID: 6299
public class AkPropagationPathInfoProxy : AkPropagationPathInfo
{
	// Token: 0x060095EE RID: 38382 RVA: 0x003E7694 File Offset: 0x003E5894
	internal AkPropagationPathInfoProxy(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfoProxy_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095EF RID: 38383 RVA: 0x003E76AC File Offset: 0x003E58AC
	public AkPropagationPathInfoProxy()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPropagationPathInfoProxy(), true)
	{
	}

	// Token: 0x060095F0 RID: 38384 RVA: 0x003E76BC File Offset: 0x003E58BC
	internal static IntPtr getCPtr(AkPropagationPathInfoProxy obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060095F1 RID: 38385 RVA: 0x003E76D4 File Offset: 0x003E58D4
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfoProxy_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x060095F2 RID: 38386 RVA: 0x003E76EC File Offset: 0x003E58EC
	~AkPropagationPathInfoProxy()
	{
		this.Dispose();
	}

	// Token: 0x060095F3 RID: 38387 RVA: 0x003E771C File Offset: 0x003E591C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPropagationPathInfoProxy(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060095F4 RID: 38388 RVA: 0x003E7798 File Offset: 0x003E5998
	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfoProxy_GetSizeOf();
	}

	// Token: 0x060095F5 RID: 38389 RVA: 0x003E77A0 File Offset: 0x003E59A0
	public AkVector GetNodePoint(uint idx)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfoProxy_GetNodePoint(this.swigCPtr, idx);
		return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
	}

	// Token: 0x04009C61 RID: 40033
	private IntPtr swigCPtr;
}
