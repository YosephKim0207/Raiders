using System;

// Token: 0x02001884 RID: 6276
public class AkMIDIEventCallbackInfo : AkEventCallbackInfo
{
	// Token: 0x06009501 RID: 38145 RVA: 0x003E5D50 File Offset: 0x003E3F50
	internal AkMIDIEventCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009502 RID: 38146 RVA: 0x003E5D68 File Offset: 0x003E3F68
	public AkMIDIEventCallbackInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEventCallbackInfo(), true)
	{
	}

	// Token: 0x06009503 RID: 38147 RVA: 0x003E5D78 File Offset: 0x003E3F78
	internal static IntPtr getCPtr(AkMIDIEventCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009504 RID: 38148 RVA: 0x003E5D90 File Offset: 0x003E3F90
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009505 RID: 38149 RVA: 0x003E5DA8 File Offset: 0x003E3FA8
	~AkMIDIEventCallbackInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009506 RID: 38150 RVA: 0x003E5DD8 File Offset: 0x003E3FD8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEventCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700165F RID: 5727
	// (get) Token: 0x06009507 RID: 38151 RVA: 0x003E5E54 File Offset: 0x003E4054
	public byte byChan
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byChan_get(this.swigCPtr);
		}
	}

	// Token: 0x17001660 RID: 5728
	// (get) Token: 0x06009508 RID: 38152 RVA: 0x003E5E64 File Offset: 0x003E4064
	public byte byParam1
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byParam1_get(this.swigCPtr);
		}
	}

	// Token: 0x17001661 RID: 5729
	// (get) Token: 0x06009509 RID: 38153 RVA: 0x003E5E74 File Offset: 0x003E4074
	public byte byParam2
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byParam2_get(this.swigCPtr);
		}
	}

	// Token: 0x17001662 RID: 5730
	// (get) Token: 0x0600950A RID: 38154 RVA: 0x003E5E84 File Offset: 0x003E4084
	public AkMIDIEventTypes byType
	{
		get
		{
			return (AkMIDIEventTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byType_get(this.swigCPtr);
		}
	}

	// Token: 0x17001663 RID: 5731
	// (get) Token: 0x0600950B RID: 38155 RVA: 0x003E5E94 File Offset: 0x003E4094
	public byte byOnOffNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byOnOffNote_get(this.swigCPtr);
		}
	}

	// Token: 0x17001664 RID: 5732
	// (get) Token: 0x0600950C RID: 38156 RVA: 0x003E5EA4 File Offset: 0x003E40A4
	public byte byVelocity
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byVelocity_get(this.swigCPtr);
		}
	}

	// Token: 0x17001665 RID: 5733
	// (get) Token: 0x0600950D RID: 38157 RVA: 0x003E5EB4 File Offset: 0x003E40B4
	public AkMIDICcTypes byCc
	{
		get
		{
			return (AkMIDICcTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byCc_get(this.swigCPtr);
		}
	}

	// Token: 0x17001666 RID: 5734
	// (get) Token: 0x0600950E RID: 38158 RVA: 0x003E5EC4 File Offset: 0x003E40C4
	public byte byCcValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byCcValue_get(this.swigCPtr);
		}
	}

	// Token: 0x17001667 RID: 5735
	// (get) Token: 0x0600950F RID: 38159 RVA: 0x003E5ED4 File Offset: 0x003E40D4
	public byte byValueLsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byValueLsb_get(this.swigCPtr);
		}
	}

	// Token: 0x17001668 RID: 5736
	// (get) Token: 0x06009510 RID: 38160 RVA: 0x003E5EE4 File Offset: 0x003E40E4
	public byte byValueMsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byValueMsb_get(this.swigCPtr);
		}
	}

	// Token: 0x17001669 RID: 5737
	// (get) Token: 0x06009511 RID: 38161 RVA: 0x003E5EF4 File Offset: 0x003E40F4
	public byte byAftertouchNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byAftertouchNote_get(this.swigCPtr);
		}
	}

	// Token: 0x1700166A RID: 5738
	// (get) Token: 0x06009512 RID: 38162 RVA: 0x003E5F04 File Offset: 0x003E4104
	public byte byNoteAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byNoteAftertouchValue_get(this.swigCPtr);
		}
	}

	// Token: 0x1700166B RID: 5739
	// (get) Token: 0x06009513 RID: 38163 RVA: 0x003E5F14 File Offset: 0x003E4114
	public byte byChanAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byChanAftertouchValue_get(this.swigCPtr);
		}
	}

	// Token: 0x1700166C RID: 5740
	// (get) Token: 0x06009514 RID: 38164 RVA: 0x003E5F24 File Offset: 0x003E4124
	public byte byProgramNum
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byProgramNum_get(this.swigCPtr);
		}
	}

	// Token: 0x04009BC9 RID: 39881
	private IntPtr swigCPtr;
}
