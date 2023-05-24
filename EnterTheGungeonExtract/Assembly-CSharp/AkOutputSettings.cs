using System;

// Token: 0x02001890 RID: 6288
public class AkOutputSettings : IDisposable
{
	// Token: 0x06009565 RID: 38245 RVA: 0x003E6890 File Offset: 0x003E4A90
	internal AkOutputSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009566 RID: 38246 RVA: 0x003E68A8 File Offset: 0x003E4AA8
	public AkOutputSettings()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_0(), true)
	{
	}

	// Token: 0x06009567 RID: 38247 RVA: 0x003E68B8 File Offset: 0x003E4AB8
	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice, AkChannelConfig in_channelConfig, AkPanningRule in_ePanning)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_1(in_szDeviceShareSet, in_idDevice, AkChannelConfig.getCPtr(in_channelConfig), (int)in_ePanning), true)
	{
	}

	// Token: 0x06009568 RID: 38248 RVA: 0x003E68D0 File Offset: 0x003E4AD0
	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice, AkChannelConfig in_channelConfig)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_2(in_szDeviceShareSet, in_idDevice, AkChannelConfig.getCPtr(in_channelConfig)), true)
	{
	}

	// Token: 0x06009569 RID: 38249 RVA: 0x003E68E8 File Offset: 0x003E4AE8
	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_3(in_szDeviceShareSet, in_idDevice), true)
	{
	}

	// Token: 0x0600956A RID: 38250 RVA: 0x003E68F8 File Offset: 0x003E4AF8
	public AkOutputSettings(string in_szDeviceShareSet)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_4(in_szDeviceShareSet), true)
	{
	}

	// Token: 0x0600956B RID: 38251 RVA: 0x003E6908 File Offset: 0x003E4B08
	internal static IntPtr getCPtr(AkOutputSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x0600956C RID: 38252 RVA: 0x003E6920 File Offset: 0x003E4B20
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x0600956D RID: 38253 RVA: 0x003E6930 File Offset: 0x003E4B30
	~AkOutputSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600956E RID: 38254 RVA: 0x003E6960 File Offset: 0x003E4B60
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkOutputSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001689 RID: 5769
	// (get) Token: 0x06009570 RID: 38256 RVA: 0x003E69E4 File Offset: 0x003E4BE4
	// (set) Token: 0x0600956F RID: 38255 RVA: 0x003E69D4 File Offset: 0x003E4BD4
	public uint audioDeviceShareset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkOutputSettings_audioDeviceShareset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_audioDeviceShareset_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700168A RID: 5770
	// (get) Token: 0x06009572 RID: 38258 RVA: 0x003E6A04 File Offset: 0x003E4C04
	// (set) Token: 0x06009571 RID: 38257 RVA: 0x003E69F4 File Offset: 0x003E4BF4
	public uint idDevice
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkOutputSettings_idDevice_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_idDevice_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700168B RID: 5771
	// (get) Token: 0x06009574 RID: 38260 RVA: 0x003E6A24 File Offset: 0x003E4C24
	// (set) Token: 0x06009573 RID: 38259 RVA: 0x003E6A14 File Offset: 0x003E4C14
	public AkPanningRule ePanningRule
	{
		get
		{
			return (AkPanningRule)AkSoundEnginePINVOKE.CSharp_AkOutputSettings_ePanningRule_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_ePanningRule_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x1700168C RID: 5772
	// (get) Token: 0x06009576 RID: 38262 RVA: 0x003E6A48 File Offset: 0x003E4C48
	// (set) Token: 0x06009575 RID: 38261 RVA: 0x003E6A34 File Offset: 0x003E4C34
	public AkChannelConfig channelConfig
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkOutputSettings_channelConfig_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkChannelConfig(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_channelConfig_set(this.swigCPtr, AkChannelConfig.getCPtr(value));
		}
	}

	// Token: 0x04009C40 RID: 40000
	private IntPtr swigCPtr;

	// Token: 0x04009C41 RID: 40001
	protected bool swigCMemOwn;
}
