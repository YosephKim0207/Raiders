using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018BA RID: 6330
public class AkAuxSendArray : IDisposable
{
	// Token: 0x06009C47 RID: 40007 RVA: 0x003EBAE4 File Offset: 0x003E9CE4
	public AkAuxSendArray()
	{
		this.m_Buffer = Marshal.AllocHGlobal(4 * this.SIZE_OF_AKAUXSENDVALUE);
		this.m_Count = 0;
	}

	// Token: 0x170016F1 RID: 5873
	public AkAuxSendValue this[int index]
	{
		get
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkAuxSendArray");
			}
			return new AkAuxSendValue(this.GetObjectPtr(index), false);
		}
	}

	// Token: 0x170016F2 RID: 5874
	// (get) Token: 0x06009C49 RID: 40009 RVA: 0x003EBB3C File Offset: 0x003E9D3C
	public bool isFull
	{
		get
		{
			return this.m_Count >= 4 || this.m_Buffer == IntPtr.Zero;
		}
	}

	// Token: 0x06009C4A RID: 40010 RVA: 0x003EBB60 File Offset: 0x003E9D60
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_Count = 0;
		}
	}

	// Token: 0x06009C4B RID: 40011 RVA: 0x003EBB94 File Offset: 0x003E9D94
	~AkAuxSendArray()
	{
		this.Dispose();
	}

	// Token: 0x06009C4C RID: 40012 RVA: 0x003EBBC4 File Offset: 0x003E9DC4
	public void Reset()
	{
		this.m_Count = 0;
	}

	// Token: 0x06009C4D RID: 40013 RVA: 0x003EBBD0 File Offset: 0x003E9DD0
	public bool Add(GameObject in_listenerGameObj, uint in_AuxBusID, float in_fValue)
	{
		if (this.isFull)
		{
			return false;
		}
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.GetObjectPtr(this.m_Count), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_AuxBusID, in_fValue);
		this.m_Count++;
		return true;
	}

	// Token: 0x06009C4E RID: 40014 RVA: 0x003EBC08 File Offset: 0x003E9E08
	public bool Add(uint in_AuxBusID, float in_fValue)
	{
		if (this.isFull)
		{
			return false;
		}
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.GetObjectPtr(this.m_Count), ulong.MaxValue, in_AuxBusID, in_fValue);
		this.m_Count++;
		return true;
	}

	// Token: 0x06009C4F RID: 40015 RVA: 0x003EBC3C File Offset: 0x003E9E3C
	public bool Contains(GameObject in_listenerGameObj, uint in_AuxBusID)
	{
		if (this.m_Buffer == IntPtr.Zero)
		{
			return false;
		}
		for (int i = 0; i < this.m_Count; i++)
		{
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.GetObjectPtr(i), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_AuxBusID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009C50 RID: 40016 RVA: 0x003EBC94 File Offset: 0x003E9E94
	public bool Contains(uint in_AuxBusID)
	{
		if (this.m_Buffer == IntPtr.Zero)
		{
			return false;
		}
		for (int i = 0; i < this.m_Count; i++)
		{
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.GetObjectPtr(i), 18446744073709551615UL, in_AuxBusID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009C51 RID: 40017 RVA: 0x003EBCE8 File Offset: 0x003E9EE8
	public AKRESULT SetValues(GameObject gameObject)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_SetGameObjectAuxSendValues(this.m_Buffer, AkSoundEngine.GetAkGameObjectID(gameObject), (uint)this.m_Count);
	}

	// Token: 0x06009C52 RID: 40018 RVA: 0x003EBD04 File Offset: 0x003E9F04
	public AKRESULT GetValues(GameObject gameObject)
	{
		uint num = 4U;
		AKRESULT akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetGameObjectAuxSendValues(this.m_Buffer, AkSoundEngine.GetAkGameObjectID(gameObject), ref num);
		this.m_Count = (int)num;
		return akresult;
	}

	// Token: 0x06009C53 RID: 40019 RVA: 0x003EBD30 File Offset: 0x003E9F30
	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	// Token: 0x06009C54 RID: 40020 RVA: 0x003EBD38 File Offset: 0x003E9F38
	public int Count()
	{
		return this.m_Count;
	}

	// Token: 0x06009C55 RID: 40021 RVA: 0x003EBD40 File Offset: 0x003E9F40
	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF_AKAUXSENDVALUE * index));
	}

	// Token: 0x04009E3D RID: 40509
	private const int MAX_COUNT = 4;

	// Token: 0x04009E3E RID: 40510
	private readonly int SIZE_OF_AKAUXSENDVALUE = AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetSizeOf();

	// Token: 0x04009E3F RID: 40511
	private IntPtr m_Buffer;

	// Token: 0x04009E40 RID: 40512
	private int m_Count;
}
