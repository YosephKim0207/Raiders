using System;
using System.Runtime.InteropServices;

// Token: 0x020018CF RID: 6351
public class AkTriangleArray : IDisposable
{
	// Token: 0x06009CC5 RID: 40133 RVA: 0x003ED6D0 File Offset: 0x003EB8D0
	public AkTriangleArray(int count)
	{
		this.m_Count = count;
		this.m_Buffer = Marshal.AllocHGlobal(count * this.SIZE_OF_AKTRIANGLE);
		if (this.m_Buffer != IntPtr.Zero)
		{
			for (int i = 0; i < count; i++)
			{
				AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_Clear(this.GetObjectPtr(i));
			}
		}
	}

	// Token: 0x06009CC6 RID: 40134 RVA: 0x003ED73C File Offset: 0x003EB93C
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			for (int i = 0; i < this.m_Count; i++)
			{
				AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_DeleteName(this.GetObjectPtr(i));
			}
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_Count = 0;
		}
	}

	// Token: 0x06009CC7 RID: 40135 RVA: 0x003ED7A0 File Offset: 0x003EB9A0
	~AkTriangleArray()
	{
		this.Dispose();
	}

	// Token: 0x06009CC8 RID: 40136 RVA: 0x003ED7D0 File Offset: 0x003EB9D0
	public void Reset()
	{
		this.m_Count = 0;
	}

	// Token: 0x06009CC9 RID: 40137 RVA: 0x003ED7DC File Offset: 0x003EB9DC
	public AkTriangle GetTriangle(int index)
	{
		if (index >= this.m_Count)
		{
			return null;
		}
		return new AkTriangle(this.GetObjectPtr(index), false);
	}

	// Token: 0x06009CCA RID: 40138 RVA: 0x003ED7FC File Offset: 0x003EB9FC
	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	// Token: 0x06009CCB RID: 40139 RVA: 0x003ED804 File Offset: 0x003EBA04
	public int Count()
	{
		return this.m_Count;
	}

	// Token: 0x06009CCC RID: 40140 RVA: 0x003ED80C File Offset: 0x003EBA0C
	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF_AKTRIANGLE * index));
	}

	// Token: 0x04009E78 RID: 40568
	private readonly int SIZE_OF_AKTRIANGLE = AkSoundEnginePINVOKE.CSharp_AkTriangleProxy_GetSizeOf();

	// Token: 0x04009E79 RID: 40569
	private IntPtr m_Buffer;

	// Token: 0x04009E7A RID: 40570
	private int m_Count;
}
