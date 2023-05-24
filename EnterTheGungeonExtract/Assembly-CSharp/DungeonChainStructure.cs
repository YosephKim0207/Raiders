using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x02000E6A RID: 3690
public class DungeonChainStructure
{
	// Token: 0x040044EA RID: 17642
	public FlowNodeBuildData parentNode;

	// Token: 0x040044EB RID: 17643
	public FlowNodeBuildData optionalRequiredNode;

	// Token: 0x040044EC RID: 17644
	public List<FlowNodeBuildData> containedNodes = new List<FlowNodeBuildData>();

	// Token: 0x040044ED RID: 17645
	public int previousLoopDistanceMetric = int.MaxValue;
}
