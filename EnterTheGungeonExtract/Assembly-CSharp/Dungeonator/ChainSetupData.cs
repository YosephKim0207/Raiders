using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F33 RID: 3891
	[Serializable]
	public class ChainSetupData
	{
		// Token: 0x04004BD7 RID: 19415
		[SerializeField]
		public DungeonChain chain;

		// Token: 0x04004BD8 RID: 19416
		[SerializeField]
		public int minSubchainsToBuild;

		// Token: 0x04004BD9 RID: 19417
		[SerializeField]
		public int maxSubchainsToBuild = 3;

		// Token: 0x04004BDA RID: 19418
		[SerializeField]
		public ChainSetupData[] childChains;

		// Token: 0x04004BDB RID: 19419
		[SerializeField]
		public ChainSetupData.ExitPreferenceMetric exitMetric;

		// Token: 0x02000F34 RID: 3892
		public enum ExitPreferenceMetric
		{
			// Token: 0x04004BDD RID: 19421
			RANDOM,
			// Token: 0x04004BDE RID: 19422
			HORIZONTAL,
			// Token: 0x04004BDF RID: 19423
			VERTICAL,
			// Token: 0x04004BE0 RID: 19424
			FARTHEST,
			// Token: 0x04004BE1 RID: 19425
			NEAREST
		}
	}
}
