using System;

// Token: 0x02001859 RID: 6233
public class AkAudioFormat : IDisposable
{
	// Token: 0x06009353 RID: 37715 RVA: 0x003E2DB4 File Offset: 0x003E0FB4
	internal AkAudioFormat(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009354 RID: 37716 RVA: 0x003E2DCC File Offset: 0x003E0FCC
	public AkAudioFormat()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkAudioFormat(), true)
	{
	}

	// Token: 0x06009355 RID: 37717 RVA: 0x003E2DDC File Offset: 0x003E0FDC
	internal static IntPtr getCPtr(AkAudioFormat obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009356 RID: 37718 RVA: 0x003E2DF4 File Offset: 0x003E0FF4
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009357 RID: 37719 RVA: 0x003E2E04 File Offset: 0x003E1004
	~AkAudioFormat()
	{
		this.Dispose();
	}

	// Token: 0x06009358 RID: 37720 RVA: 0x003E2E34 File Offset: 0x003E1034
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioFormat(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015EC RID: 5612
	// (get) Token: 0x0600935A RID: 37722 RVA: 0x003E2EB8 File Offset: 0x003E10B8
	// (set) Token: 0x06009359 RID: 37721 RVA: 0x003E2EA8 File Offset: 0x003E10A8
	public uint uSampleRate
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uSampleRate_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uSampleRate_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015ED RID: 5613
	// (get) Token: 0x0600935C RID: 37724 RVA: 0x003E2EDC File Offset: 0x003E10DC
	// (set) Token: 0x0600935B RID: 37723 RVA: 0x003E2EC8 File Offset: 0x003E10C8
	public AkChannelConfig channelConfig
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkAudioFormat_channelConfig_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkChannelConfig(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_channelConfig_set(this.swigCPtr, AkChannelConfig.getCPtr(value));
		}
	}

	// Token: 0x170015EE RID: 5614
	// (get) Token: 0x0600935E RID: 37726 RVA: 0x003E2F24 File Offset: 0x003E1124
	// (set) Token: 0x0600935D RID: 37725 RVA: 0x003E2F14 File Offset: 0x003E1114
	public uint uBitsPerSample
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uBitsPerSample_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uBitsPerSample_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015EF RID: 5615
	// (get) Token: 0x06009360 RID: 37728 RVA: 0x003E2F44 File Offset: 0x003E1144
	// (set) Token: 0x0600935F RID: 37727 RVA: 0x003E2F34 File Offset: 0x003E1134
	public uint uBlockAlign
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uBlockAlign_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uBlockAlign_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015F0 RID: 5616
	// (get) Token: 0x06009362 RID: 37730 RVA: 0x003E2F64 File Offset: 0x003E1164
	// (set) Token: 0x06009361 RID: 37729 RVA: 0x003E2F54 File Offset: 0x003E1154
	public uint uTypeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uTypeID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uTypeID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015F1 RID: 5617
	// (get) Token: 0x06009364 RID: 37732 RVA: 0x003E2F84 File Offset: 0x003E1184
	// (set) Token: 0x06009363 RID: 37731 RVA: 0x003E2F74 File Offset: 0x003E1174
	public uint uInterleaveID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uInterleaveID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioFormat_uInterleaveID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06009365 RID: 37733 RVA: 0x003E2F94 File Offset: 0x003E1194
	public uint GetNumChannels()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_GetNumChannels(this.swigCPtr);
	}

	// Token: 0x06009366 RID: 37734 RVA: 0x003E2FA4 File Offset: 0x003E11A4
	public uint GetBitsPerSample()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_GetBitsPerSample(this.swigCPtr);
	}

	// Token: 0x06009367 RID: 37735 RVA: 0x003E2FB4 File Offset: 0x003E11B4
	public uint GetBlockAlign()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_GetBlockAlign(this.swigCPtr);
	}

	// Token: 0x06009368 RID: 37736 RVA: 0x003E2FC4 File Offset: 0x003E11C4
	public uint GetTypeID()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_GetTypeID(this.swigCPtr);
	}

	// Token: 0x06009369 RID: 37737 RVA: 0x003E2FD4 File Offset: 0x003E11D4
	public uint GetInterleaveID()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_GetInterleaveID(this.swigCPtr);
	}

	// Token: 0x0600936A RID: 37738 RVA: 0x003E2FE4 File Offset: 0x003E11E4
	public void SetAll(uint in_uSampleRate, AkChannelConfig in_channelConfig, uint in_uBitsPerSample, uint in_uBlockAlign, uint in_uTypeID, uint in_uInterleaveID)
	{
		AkSoundEnginePINVOKE.CSharp_AkAudioFormat_SetAll(this.swigCPtr, in_uSampleRate, AkChannelConfig.getCPtr(in_channelConfig), in_uBitsPerSample, in_uBlockAlign, in_uTypeID, in_uInterleaveID);
	}

	// Token: 0x0600936B RID: 37739 RVA: 0x003E3000 File Offset: 0x003E1200
	public bool IsChannelConfigSupported()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAudioFormat_IsChannelConfigSupported(this.swigCPtr);
	}

	// Token: 0x04009ADB RID: 39643
	private IntPtr swigCPtr;

	// Token: 0x04009ADC RID: 39644
	protected bool swigCMemOwn;
}
