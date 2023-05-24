using System;

// Token: 0x02000371 RID: 881
public sealed class Tuple<T1, T2>
{
	// Token: 0x06000E92 RID: 3730 RVA: 0x000449AC File Offset: 0x00042BAC
	public Tuple(T1 first, T2 second)
	{
		this.First = first;
		this.Second = second;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x000449C4 File Offset: 0x00042BC4
	public override string ToString()
	{
		return string.Format("[{0}, {1}]", this.First, this.Second);
	}

	// Token: 0x04000E4D RID: 3661
	public T1 First;

	// Token: 0x04000E4E RID: 3662
	public T2 Second;
}
