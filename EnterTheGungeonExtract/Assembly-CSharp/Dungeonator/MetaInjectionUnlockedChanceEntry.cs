using System;

namespace Dungeonator
{
	// Token: 0x02000EA2 RID: 3746
	[Serializable]
	public class MetaInjectionUnlockedChanceEntry
	{
		// Token: 0x040046BE RID: 18110
		public DungeonPrerequisite[] prerequisites;

		// Token: 0x040046BF RID: 18111
		public float ChanceToTrigger = 1f;
	}
}
