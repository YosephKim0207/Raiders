using System;

// Token: 0x020018A9 RID: 6313
public class AkTransform : IDisposable
{
	// Token: 0x06009682 RID: 38530 RVA: 0x003E87DC File Offset: 0x003E69DC
	internal AkTransform(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009683 RID: 38531 RVA: 0x003E87F4 File Offset: 0x003E69F4
	public AkTransform()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkTransform(), true)
	{
	}

	// Token: 0x06009684 RID: 38532 RVA: 0x003E8804 File Offset: 0x003E6A04
	internal static IntPtr getCPtr(AkTransform obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009685 RID: 38533 RVA: 0x003E881C File Offset: 0x003E6A1C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009686 RID: 38534 RVA: 0x003E882C File Offset: 0x003E6A2C
	~AkTransform()
	{
		this.Dispose();
	}

	// Token: 0x06009687 RID: 38535 RVA: 0x003E885C File Offset: 0x003E6A5C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkTransform(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06009688 RID: 38536 RVA: 0x003E88D0 File Offset: 0x003E6AD0
	public AkVector Position()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_Position(this.swigCPtr), false);
	}

	// Token: 0x06009689 RID: 38537 RVA: 0x003E88F0 File Offset: 0x003E6AF0
	public AkVector OrientationFront()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationFront(this.swigCPtr), false);
	}

	// Token: 0x0600968A RID: 38538 RVA: 0x003E8910 File Offset: 0x003E6B10
	public AkVector OrientationTop()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationTop(this.swigCPtr), false);
	}

	// Token: 0x0600968B RID: 38539 RVA: 0x003E8930 File Offset: 0x003E6B30
	public void Set(AkVector in_position, AkVector in_orientationFront, AkVector in_orientationTop)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_position), AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop));
	}

	// Token: 0x0600968C RID: 38540 RVA: 0x003E8950 File Offset: 0x003E6B50
	public void Set(float in_positionX, float in_positionY, float in_positionZ, float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_1(this.swigCPtr, in_positionX, in_positionY, in_positionZ, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ);
	}

	// Token: 0x0600968D RID: 38541 RVA: 0x003E8978 File Offset: 0x003E6B78
	public void SetPosition(AkVector in_position)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_position));
	}

	// Token: 0x0600968E RID: 38542 RVA: 0x003E898C File Offset: 0x003E6B8C
	public void SetPosition(float in_x, float in_y, float in_z)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_1(this.swigCPtr, in_x, in_y, in_z);
	}

	// Token: 0x0600968F RID: 38543 RVA: 0x003E899C File Offset: 0x003E6B9C
	public void SetOrientation(AkVector in_orientationFront, AkVector in_orientationTop)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop));
	}

	// Token: 0x06009690 RID: 38544 RVA: 0x003E89B8 File Offset: 0x003E6BB8
	public void SetOrientation(float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_1(this.swigCPtr, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ);
	}

	// Token: 0x04009CD4 RID: 40148
	private IntPtr swigCPtr;

	// Token: 0x04009CD5 RID: 40149
	protected bool swigCMemOwn;
}
