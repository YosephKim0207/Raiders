using System;
using System.Collections.Generic;

// Token: 0x0200036E RID: 878
public class IntVector2EqualityComparer : IEqualityComparer<IntVector2>
{
	// Token: 0x06000E76 RID: 3702 RVA: 0x000445DC File Offset: 0x000427DC
	public bool Equals(IntVector2 a, IntVector2 b)
	{
		return a.x == b.x && a.y == b.y;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x00044604 File Offset: 0x00042804
	public int GetHashCode(IntVector2 obj)
	{
		int num = 17;
		num = num * 23 + obj.x.GetHashCode();
		return num * 23 + obj.y.GetHashCode();
	}
}
