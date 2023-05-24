using System;

namespace Dungeonator
{
	// Token: 0x02000EEB RID: 3819
	public enum HeuristicFormula
	{
		// Token: 0x04004953 RID: 18771
		Manhattan = 1,
		// Token: 0x04004954 RID: 18772
		MaxDXDY,
		// Token: 0x04004955 RID: 18773
		DiagonalShortCut,
		// Token: 0x04004956 RID: 18774
		Euclidean,
		// Token: 0x04004957 RID: 18775
		EuclideanNoSQR,
		// Token: 0x04004958 RID: 18776
		Custom1
	}
}
