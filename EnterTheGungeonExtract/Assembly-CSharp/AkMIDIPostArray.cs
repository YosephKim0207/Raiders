using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018CB RID: 6347
public class AkMIDIPostArray
{
	// Token: 0x06009CA5 RID: 40101 RVA: 0x003ED0A0 File Offset: 0x003EB2A0
	public AkMIDIPostArray(int size)
	{
		this.m_Count = size;
		this.m_Buffer = Marshal.AllocHGlobal(this.m_Count * this.SIZE_OF);
	}

	// Token: 0x170016F6 RID: 5878
	public AkMIDIPost this[int index]
	{
		get
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
			}
			return new AkMIDIPost(this.GetObjectPtr(index), false);
		}
		set
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
			}
			AkSoundEnginePINVOKE.CSharp_AkMIDIPost_Clone(this.GetObjectPtr(index), AkMIDIPost.getCPtr(value));
		}
	}

	// Token: 0x06009CA8 RID: 40104 RVA: 0x003ED134 File Offset: 0x003EB334
	~AkMIDIPostArray()
	{
		Marshal.FreeHGlobal(this.m_Buffer);
		this.m_Buffer = IntPtr.Zero;
	}

	// Token: 0x06009CA9 RID: 40105 RVA: 0x003ED174 File Offset: 0x003EB374
	public void PostOnEvent(uint in_eventID, GameObject gameObject)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AkSoundEngine.PreGameObjectAPICall(gameObject, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.m_Buffer, in_eventID, akGameObjectID, (uint)this.m_Count);
	}

	// Token: 0x06009CAA RID: 40106 RVA: 0x003ED1A4 File Offset: 0x003EB3A4
	public void PostOnEvent(uint in_eventID, GameObject gameObject, int count)
	{
		if (count >= this.m_Count)
		{
			throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
		}
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AkSoundEngine.PreGameObjectAPICall(gameObject, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.m_Buffer, in_eventID, akGameObjectID, (uint)count);
	}

	// Token: 0x06009CAB RID: 40107 RVA: 0x003ED1E8 File Offset: 0x003EB3E8
	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	// Token: 0x06009CAC RID: 40108 RVA: 0x003ED1F0 File Offset: 0x003EB3F0
	public int Count()
	{
		return this.m_Count;
	}

	// Token: 0x06009CAD RID: 40109 RVA: 0x003ED1F8 File Offset: 0x003EB3F8
	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF * index));
	}

	// Token: 0x04009E6B RID: 40555
	private readonly int m_Count;

	// Token: 0x04009E6C RID: 40556
	private readonly int SIZE_OF = AkSoundEnginePINVOKE.CSharp_AkMIDIPost_GetSizeOf();

	// Token: 0x04009E6D RID: 40557
	private IntPtr m_Buffer = IntPtr.Zero;
}
