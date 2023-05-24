using System;

// Token: 0x020018AB RID: 6315
public class AkTriangleProxy : AkTriangle
{
	// Token: 0x060096A3 RID: 38563 RVA: 0x003E8C0C File Offset: 0x003E6E0C
	internal AkTriangleProxy(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096A4 RID: 38564 RVA: 0x003E8C24 File Offset: 0x003E6E24
	public AkTriangleProxy()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkTriangleProxy(), true)
	{
	}

	// Token: 0x060096A5 RID: 38565 RVA: 0x003E8C34 File Offset: 0x003E6E34
	internal static IntPtr getCPtr(AkTriangleProxy obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060096A6 RID: 38566 RVA: 0x003E8C4C File Offset: 0x003E6E4C
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x060096A7 RID: 38567 RVA: 0x003E8C64 File Offset: 0x003E6E64
	~AkTriangleProxy()
	{
		this.Dispose();
	}

	// Token: 0x060096A8 RID: 38568 RVA: 0x003E8C94 File Offset: 0x003E6E94
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkTriangleProxy(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060096A9 RID: 38569 RVA: 0x003E8D10 File Offset: 0x003E6F10
	public void Clear()
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_Clear(this.swigCPtr);
	}

	// Token: 0x060096AA RID: 38570 RVA: 0x003E8D20 File Offset: 0x003E6F20
	public void DeleteName()
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_DeleteName(this.swigCPtr);
	}

	// Token: 0x060096AB RID: 38571 RVA: 0x003E8D30 File Offset: 0x003E6F30
	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_GetSizeOf();
	}

	// Token: 0x04009CD8 RID: 40152
	private IntPtr swigCPtr;
}
