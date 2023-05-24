using System;
using System.Collections.Generic;

// Token: 0x020014FA RID: 5370
public class TrackedMaximumsComparer : IEqualityComparer<TrackedMaximums>
{
	// Token: 0x06007A4E RID: 31310 RVA: 0x00310458 File Offset: 0x0030E658
	public bool Equals(TrackedMaximums x, TrackedMaximums y)
	{
		return x == y;
	}

	// Token: 0x06007A4F RID: 31311 RVA: 0x00310460 File Offset: 0x0030E660
	public int GetHashCode(TrackedMaximums obj)
	{
		return (int)obj;
	}
}
