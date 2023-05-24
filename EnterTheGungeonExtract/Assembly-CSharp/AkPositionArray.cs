using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018CC RID: 6348
public class AkPositionArray : IDisposable
{
	// Token: 0x06009CAE RID: 40110 RVA: 0x003ED214 File Offset: 0x003EB414
	public AkPositionArray(uint in_Count)
	{
		this.m_Buffer = Marshal.AllocHGlobal((int)(in_Count * 4U * 9U));
		this.m_Current = this.m_Buffer;
		this.m_MaxCount = in_Count;
		this.Count = 0U;
	}

	// Token: 0x170016F7 RID: 5879
	// (get) Token: 0x06009CAF RID: 40111 RVA: 0x003ED248 File Offset: 0x003EB448
	// (set) Token: 0x06009CB0 RID: 40112 RVA: 0x003ED250 File Offset: 0x003EB450
	public uint Count { get; private set; }

	// Token: 0x06009CB1 RID: 40113 RVA: 0x003ED25C File Offset: 0x003EB45C
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_MaxCount = 0U;
		}
	}

	// Token: 0x06009CB2 RID: 40114 RVA: 0x003ED290 File Offset: 0x003EB490
	~AkPositionArray()
	{
		this.Dispose();
	}

	// Token: 0x06009CB3 RID: 40115 RVA: 0x003ED2C0 File Offset: 0x003EB4C0
	public void Reset()
	{
		this.m_Current = this.m_Buffer;
		this.Count = 0U;
	}

	// Token: 0x06009CB4 RID: 40116 RVA: 0x003ED2D8 File Offset: 0x003EB4D8
	public void Add(Vector3 in_Pos, Vector3 in_Forward, Vector3 in_Top)
	{
		if (this.Count >= this.m_MaxCount)
		{
			throw new IndexOutOfRangeException("Out of range access in AkPositionArray");
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
		this.Count += 1U;
	}

	// Token: 0x04009E6E RID: 40558
	public IntPtr m_Buffer;

	// Token: 0x04009E6F RID: 40559
	private IntPtr m_Current;

	// Token: 0x04009E70 RID: 40560
	private uint m_MaxCount;
}
