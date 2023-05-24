using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018C8 RID: 6344
public class AkChannelEmitterArray : IDisposable
{
	// Token: 0x06009C91 RID: 40081 RVA: 0x003ECC94 File Offset: 0x003EAE94
	public AkChannelEmitterArray(uint in_Count)
	{
		this.m_Buffer = Marshal.AllocHGlobal((int)(in_Count * 40U));
		this.m_Current = this.m_Buffer;
		this.m_MaxCount = in_Count;
		this.Count = 0U;
	}

	// Token: 0x170016F4 RID: 5876
	// (get) Token: 0x06009C92 RID: 40082 RVA: 0x003ECCC8 File Offset: 0x003EAEC8
	// (set) Token: 0x06009C93 RID: 40083 RVA: 0x003ECCD0 File Offset: 0x003EAED0
	public uint Count { get; private set; }

	// Token: 0x06009C94 RID: 40084 RVA: 0x003ECCDC File Offset: 0x003EAEDC
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_MaxCount = 0U;
		}
	}

	// Token: 0x06009C95 RID: 40085 RVA: 0x003ECD10 File Offset: 0x003EAF10
	~AkChannelEmitterArray()
	{
		this.Dispose();
	}

	// Token: 0x06009C96 RID: 40086 RVA: 0x003ECD40 File Offset: 0x003EAF40
	public void Reset()
	{
		this.m_Current = this.m_Buffer;
		this.Count = 0U;
	}

	// Token: 0x06009C97 RID: 40087 RVA: 0x003ECD58 File Offset: 0x003EAF58
	public void Add(Vector3 in_Pos, Vector3 in_Forward, Vector3 in_Top, uint in_ChannelMask)
	{
		if (this.Count >= this.m_MaxCount)
		{
			throw new IndexOutOfRangeException("Out of range access in AkChannelEmitterArray");
		}
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, (int)in_ChannelMask);
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		this.Count += 1U;
	}

	// Token: 0x04009E64 RID: 40548
	public IntPtr m_Buffer;

	// Token: 0x04009E65 RID: 40549
	private IntPtr m_Current;

	// Token: 0x04009E66 RID: 40550
	private uint m_MaxCount;
}
