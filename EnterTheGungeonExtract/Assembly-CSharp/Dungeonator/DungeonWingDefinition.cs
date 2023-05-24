using System;

namespace Dungeonator
{
	// Token: 0x02000EBB RID: 3771
	[Serializable]
	public class DungeonWingDefinition
	{
		// Token: 0x040047C8 RID: 18376
		public WeightedIntCollection includedMaterialIndices;

		// Token: 0x040047C9 RID: 18377
		public float weight = 1f;

		// Token: 0x040047CA RID: 18378
		public bool canBeCriticalPath;

		// Token: 0x040047CB RID: 18379
		public bool canBeNoncriticalPath;
	}
}
