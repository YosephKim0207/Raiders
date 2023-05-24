using System;

namespace Dungeonator
{
	// Token: 0x02000EEA RID: 3818
	public enum PathFinderNodeType
	{
		// Token: 0x0400494C RID: 18764
		Start = 1,
		// Token: 0x0400494D RID: 18765
		End,
		// Token: 0x0400494E RID: 18766
		Open = 4,
		// Token: 0x0400494F RID: 18767
		Close = 8,
		// Token: 0x04004950 RID: 18768
		Current = 16,
		// Token: 0x04004951 RID: 18769
		Path = 32
	}
}
