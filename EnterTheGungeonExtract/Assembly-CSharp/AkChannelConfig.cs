using System;

// Token: 0x02001864 RID: 6244
public class AkChannelConfig : IDisposable
{
	// Token: 0x060093B2 RID: 37810 RVA: 0x003E38C8 File Offset: 0x003E1AC8
	internal AkChannelConfig(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093B3 RID: 37811 RVA: 0x003E38E0 File Offset: 0x003E1AE0
	public AkChannelConfig()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkChannelConfig__SWIG_0(), true)
	{
	}

	// Token: 0x060093B4 RID: 37812 RVA: 0x003E38F0 File Offset: 0x003E1AF0
	public AkChannelConfig(uint in_uNumChannels, uint in_uChannelMask)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkChannelConfig__SWIG_1(in_uNumChannels, in_uChannelMask), true)
	{
	}

	// Token: 0x060093B5 RID: 37813 RVA: 0x003E3900 File Offset: 0x003E1B00
	internal static IntPtr getCPtr(AkChannelConfig obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093B6 RID: 37814 RVA: 0x003E3918 File Offset: 0x003E1B18
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093B7 RID: 37815 RVA: 0x003E3928 File Offset: 0x003E1B28
	~AkChannelConfig()
	{
		this.Dispose();
	}

	// Token: 0x060093B8 RID: 37816 RVA: 0x003E3958 File Offset: 0x003E1B58
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkChannelConfig(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015FF RID: 5631
	// (get) Token: 0x060093BA RID: 37818 RVA: 0x003E39DC File Offset: 0x003E1BDC
	// (set) Token: 0x060093B9 RID: 37817 RVA: 0x003E39CC File Offset: 0x003E1BCC
	public uint uNumChannels
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_uNumChannels_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelConfig_uNumChannels_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001600 RID: 5632
	// (get) Token: 0x060093BC RID: 37820 RVA: 0x003E39FC File Offset: 0x003E1BFC
	// (set) Token: 0x060093BB RID: 37819 RVA: 0x003E39EC File Offset: 0x003E1BEC
	public uint eConfigType
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_eConfigType_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelConfig_eConfigType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001601 RID: 5633
	// (get) Token: 0x060093BE RID: 37822 RVA: 0x003E3A1C File Offset: 0x003E1C1C
	// (set) Token: 0x060093BD RID: 37821 RVA: 0x003E3A0C File Offset: 0x003E1C0C
	public uint uChannelMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_uChannelMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelConfig_uChannelMask_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060093BF RID: 37823 RVA: 0x003E3A2C File Offset: 0x003E1C2C
	public void Clear()
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_Clear(this.swigCPtr);
	}

	// Token: 0x060093C0 RID: 37824 RVA: 0x003E3A3C File Offset: 0x003E1C3C
	public void SetStandard(uint in_uChannelMask)
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_SetStandard(this.swigCPtr, in_uChannelMask);
	}

	// Token: 0x060093C1 RID: 37825 RVA: 0x003E3A4C File Offset: 0x003E1C4C
	public void SetStandardOrAnonymous(uint in_uNumChannels, uint in_uChannelMask)
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_SetStandardOrAnonymous(this.swigCPtr, in_uNumChannels, in_uChannelMask);
	}

	// Token: 0x060093C2 RID: 37826 RVA: 0x003E3A5C File Offset: 0x003E1C5C
	public void SetAnonymous(uint in_uNumChannels)
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_SetAnonymous(this.swigCPtr, in_uNumChannels);
	}

	// Token: 0x060093C3 RID: 37827 RVA: 0x003E3A6C File Offset: 0x003E1C6C
	public void SetAmbisonic(uint in_uNumChannels)
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_SetAmbisonic(this.swigCPtr, in_uNumChannels);
	}

	// Token: 0x060093C4 RID: 37828 RVA: 0x003E3A7C File Offset: 0x003E1C7C
	public bool IsValid()
	{
		return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_IsValid(this.swigCPtr);
	}

	// Token: 0x060093C5 RID: 37829 RVA: 0x003E3A8C File Offset: 0x003E1C8C
	public uint Serialize()
	{
		return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_Serialize(this.swigCPtr);
	}

	// Token: 0x060093C6 RID: 37830 RVA: 0x003E3A9C File Offset: 0x003E1C9C
	public void Deserialize(uint in_uChannelConfig)
	{
		AkSoundEnginePINVOKE.CSharp_AkChannelConfig_Deserialize(this.swigCPtr, in_uChannelConfig);
	}

	// Token: 0x060093C7 RID: 37831 RVA: 0x003E3AAC File Offset: 0x003E1CAC
	public AkChannelConfig RemoveLFE()
	{
		return new AkChannelConfig(AkSoundEnginePINVOKE.CSharp_AkChannelConfig_RemoveLFE(this.swigCPtr), true);
	}

	// Token: 0x060093C8 RID: 37832 RVA: 0x003E3ACC File Offset: 0x003E1CCC
	public AkChannelConfig RemoveCenter()
	{
		return new AkChannelConfig(AkSoundEnginePINVOKE.CSharp_AkChannelConfig_RemoveCenter(this.swigCPtr), true);
	}

	// Token: 0x060093C9 RID: 37833 RVA: 0x003E3AEC File Offset: 0x003E1CEC
	public bool IsChannelConfigSupported()
	{
		return AkSoundEnginePINVOKE.CSharp_AkChannelConfig_IsChannelConfigSupported(this.swigCPtr);
	}

	// Token: 0x04009B0C RID: 39692
	private IntPtr swigCPtr;

	// Token: 0x04009B0D RID: 39693
	protected bool swigCMemOwn;
}
