using System;
using System.Runtime.InteropServices;

// Token: 0x020018CD RID: 6349
public class AkPropagationPathInfoArray : IDisposable
{
	// Token: 0x06009CB5 RID: 40117 RVA: 0x003ED4F8 File Offset: 0x003EB6F8
	public AkPropagationPathInfoArray(int count)
	{
		this.m_Count = count;
		this.m_Buffer = Marshal.AllocHGlobal(count * this.SIZE_OF_STRUCTURE);
	}

	// Token: 0x06009CB6 RID: 40118 RVA: 0x003ED528 File Offset: 0x003EB728
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_Count = 0;
		}
	}

	// Token: 0x06009CB7 RID: 40119 RVA: 0x003ED55C File Offset: 0x003EB75C
	~AkPropagationPathInfoArray()
	{
		this.Dispose();
	}

	// Token: 0x06009CB8 RID: 40120 RVA: 0x003ED58C File Offset: 0x003EB78C
	public void Reset()
	{
		this.m_Count = 0;
	}

	// Token: 0x06009CB9 RID: 40121 RVA: 0x003ED598 File Offset: 0x003EB798
	public AkPropagationPathInfoProxy GetPropagationPathInfo(int index)
	{
		if (index >= this.m_Count)
		{
			return null;
		}
		return new AkPropagationPathInfoProxy(this.GetObjectPtr(index), false);
	}

	// Token: 0x06009CBA RID: 40122 RVA: 0x003ED5B8 File Offset: 0x003EB7B8
	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	// Token: 0x06009CBB RID: 40123 RVA: 0x003ED5C0 File Offset: 0x003EB7C0
	public int Count()
	{
		return this.m_Count;
	}

	// Token: 0x06009CBC RID: 40124 RVA: 0x003ED5C8 File Offset: 0x003EB7C8
	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF_STRUCTURE * index));
	}

	// Token: 0x04009E72 RID: 40562
	private readonly int SIZE_OF_STRUCTURE = AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfoProxy_GetSizeOf();

	// Token: 0x04009E73 RID: 40563
	private IntPtr m_Buffer;

	// Token: 0x04009E74 RID: 40564
	private int m_Count;
}
