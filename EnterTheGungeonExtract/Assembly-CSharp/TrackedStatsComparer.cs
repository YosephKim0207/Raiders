using System;
using System.Collections.Generic;

// Token: 0x020014FB RID: 5371
public class TrackedStatsComparer : IEqualityComparer<TrackedStats>
{
	// Token: 0x06007A51 RID: 31313 RVA: 0x0031046C File Offset: 0x0030E66C
	public bool Equals(TrackedStats x, TrackedStats y)
	{
		return x == y;
	}

	// Token: 0x06007A52 RID: 31314 RVA: 0x00310474 File Offset: 0x0030E674
	public int GetHashCode(TrackedStats obj)
	{
		return (int)obj;
	}
}
