using System;

// Token: 0x02000370 RID: 880
public static class Tuple
{
	// Token: 0x06000E91 RID: 3729 RVA: 0x000449A0 File Offset: 0x00042BA0
	public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second)
	{
		return new Tuple<T1, T2>(item1, second);
	}
}
