using System;

// Token: 0x0200189F RID: 6303
public class AkRoomParams : IDisposable
{
	// Token: 0x06009601 RID: 38401 RVA: 0x003E7920 File Offset: 0x003E5B20
	internal AkRoomParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009602 RID: 38402 RVA: 0x003E7938 File Offset: 0x003E5B38
	public AkRoomParams()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkRoomParams(), true)
	{
	}

	// Token: 0x06009603 RID: 38403 RVA: 0x003E7948 File Offset: 0x003E5B48
	internal static IntPtr getCPtr(AkRoomParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009604 RID: 38404 RVA: 0x003E7960 File Offset: 0x003E5B60
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009605 RID: 38405 RVA: 0x003E7970 File Offset: 0x003E5B70
	~AkRoomParams()
	{
		this.Dispose();
	}

	// Token: 0x06009606 RID: 38406 RVA: 0x003E79A0 File Offset: 0x003E5BA0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkRoomParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170016AA RID: 5802
	// (get) Token: 0x06009608 RID: 38408 RVA: 0x003E7A28 File Offset: 0x003E5C28
	// (set) Token: 0x06009607 RID: 38407 RVA: 0x003E7A14 File Offset: 0x003E5C14
	public AkVector Up
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkRoomParams_Up_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_Up_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016AB RID: 5803
	// (get) Token: 0x0600960A RID: 38410 RVA: 0x003E7A74 File Offset: 0x003E5C74
	// (set) Token: 0x06009609 RID: 38409 RVA: 0x003E7A60 File Offset: 0x003E5C60
	public AkVector Front
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkRoomParams_Front_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_Front_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	// Token: 0x170016AC RID: 5804
	// (get) Token: 0x0600960C RID: 38412 RVA: 0x003E7ABC File Offset: 0x003E5CBC
	// (set) Token: 0x0600960B RID: 38411 RVA: 0x003E7AAC File Offset: 0x003E5CAC
	public uint ReverbAuxBus
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbAuxBus_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbAuxBus_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016AD RID: 5805
	// (get) Token: 0x0600960E RID: 38414 RVA: 0x003E7ADC File Offset: 0x003E5CDC
	// (set) Token: 0x0600960D RID: 38413 RVA: 0x003E7ACC File Offset: 0x003E5CCC
	public float ReverbLevel
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbLevel_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbLevel_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016AE RID: 5806
	// (get) Token: 0x06009610 RID: 38416 RVA: 0x003E7AFC File Offset: 0x003E5CFC
	// (set) Token: 0x0600960F RID: 38415 RVA: 0x003E7AEC File Offset: 0x003E5CEC
	public float WallOcclusion
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_WallOcclusion_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_WallOcclusion_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016AF RID: 5807
	// (get) Token: 0x06009612 RID: 38418 RVA: 0x003E7B1C File Offset: 0x003E5D1C
	// (set) Token: 0x06009611 RID: 38417 RVA: 0x003E7B0C File Offset: 0x003E5D0C
	public int Priority
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_Priority_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_Priority_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B0 RID: 5808
	// (get) Token: 0x06009614 RID: 38420 RVA: 0x003E7B3C File Offset: 0x003E5D3C
	// (set) Token: 0x06009613 RID: 38419 RVA: 0x003E7B2C File Offset: 0x003E5D2C
	public float RoomGameObj_AuxSendLevelToSelf
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170016B1 RID: 5809
	// (get) Token: 0x06009616 RID: 38422 RVA: 0x003E7B5C File Offset: 0x003E5D5C
	// (set) Token: 0x06009615 RID: 38421 RVA: 0x003E7B4C File Offset: 0x003E5D4C
	public bool RoomGameObj_KeepRegistered
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_KeepRegistered_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_KeepRegistered_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009CBF RID: 40127
	private IntPtr swigCPtr;

	// Token: 0x04009CC0 RID: 40128
	protected bool swigCMemOwn;
}
