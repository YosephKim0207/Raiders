using System;

// Token: 0x02000EA6 RID: 3750
public class IntComparer : IComparable
{
	// Token: 0x06004F5D RID: 20317 RVA: 0x001B85C4 File Offset: 0x001B67C4
	public IntComparer(int value)
	{
		this.m_value = value;
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06004F5E RID: 20318 RVA: 0x001B85D4 File Offset: 0x001B67D4
	// (set) Token: 0x06004F5F RID: 20319 RVA: 0x001B85DC File Offset: 0x001B67DC
	public int m_value { get; private set; }

	// Token: 0x06004F60 RID: 20320 RVA: 0x001B85E8 File Offset: 0x001B67E8
	int IComparable.CompareTo(object ob)
	{
		IntComparer intComparer = (IntComparer)ob;
		return this.m_value - intComparer.m_value;
	}
}
