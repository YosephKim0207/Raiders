using System;
using System.Runtime.InteropServices;

// Token: 0x020018CE RID: 6350
public class AkSoundPathInfoArray : IDisposable
{
	// Token: 0x06009CBD RID: 40125 RVA: 0x003ED5E4 File Offset: 0x003EB7E4
	public AkSoundPathInfoArray(int count)
	{
		this.m_Count = count;
		this.m_Buffer = Marshal.AllocHGlobal(count * this.SIZE_OF_STRUCTURE);
	}

	// Token: 0x06009CBE RID: 40126 RVA: 0x003ED614 File Offset: 0x003EB814
	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_Count = 0;
		}
	}

	// Token: 0x06009CBF RID: 40127 RVA: 0x003ED648 File Offset: 0x003EB848
	~AkSoundPathInfoArray()
	{
		this.Dispose();
	}

	// Token: 0x06009CC0 RID: 40128 RVA: 0x003ED678 File Offset: 0x003EB878
	public void Reset()
	{
		this.m_Count = 0;
	}

	// Token: 0x06009CC1 RID: 40129 RVA: 0x003ED684 File Offset: 0x003EB884
	public AkSoundPathInfoProxy GetSoundPathInfo(int index)
	{
		if (index >= this.m_Count)
		{
			return null;
		}
		return new AkSoundPathInfoProxy(this.GetObjectPtr(index), false);
	}

	// Token: 0x06009CC2 RID: 40130 RVA: 0x003ED6A4 File Offset: 0x003EB8A4
	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	// Token: 0x06009CC3 RID: 40131 RVA: 0x003ED6AC File Offset: 0x003EB8AC
	public int Count()
	{
		return this.m_Count;
	}

	// Token: 0x06009CC4 RID: 40132 RVA: 0x003ED6B4 File Offset: 0x003EB8B4
	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF_STRUCTURE * index));
	}

	// Token: 0x04009E75 RID: 40565
	private readonly int SIZE_OF_STRUCTURE = AkSoundEnginePINVOKE.CSharp_AkSoundPathInfoProxy_GetSizeOf();

	// Token: 0x04009E76 RID: 40566
	private IntPtr m_Buffer;

	// Token: 0x04009E77 RID: 40567
	private int m_Count;
}
