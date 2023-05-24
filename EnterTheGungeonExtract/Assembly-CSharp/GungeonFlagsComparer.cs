using System;
using System.Collections.Generic;

// Token: 0x020014FD RID: 5373
public class GungeonFlagsComparer : IEqualityComparer<GungeonFlags>
{
	// Token: 0x06007AA0 RID: 31392 RVA: 0x00312C50 File Offset: 0x00310E50
	public bool Equals(GungeonFlags x, GungeonFlags y)
	{
		return x == y;
	}

	// Token: 0x06007AA1 RID: 31393 RVA: 0x00312C58 File Offset: 0x00310E58
	public int GetHashCode(GungeonFlags obj)
	{
		return (int)obj;
	}
}
