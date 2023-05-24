using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000F03 RID: 3843
	public class FlowCompositeMetastructure
	{
		// Token: 0x060051FA RID: 20986 RVA: 0x001D3ED0 File Offset: 0x001D20D0
		public bool ContainedInBidirectionalLoop(BuilderFlowNode node)
		{
			for (int i = 0; i < this.loopLists.Count; i++)
			{
				if (this.loopLists[i].Contains(node))
				{
					bool flag = true;
					for (int j = 0; j < this.loopLists[i].Count; j++)
					{
						if (this.loopLists[i][j].loopConnectedBuilderNode != null && this.loopLists[i][j].node.loopTargetIsOneWay)
						{
							flag = false;
						}
					}
					return flag;
				}
			}
			return false;
		}

		// Token: 0x04004A24 RID: 18980
		public List<List<BuilderFlowNode>> loopLists = new List<List<BuilderFlowNode>>();

		// Token: 0x04004A25 RID: 18981
		public List<List<BuilderFlowNode>> compositeLists = new List<List<BuilderFlowNode>>();

		// Token: 0x04004A26 RID: 18982
		public List<BuilderFlowNode> usedList = new List<BuilderFlowNode>();
	}
}
