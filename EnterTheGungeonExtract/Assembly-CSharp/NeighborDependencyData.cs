using System;
using System.Collections.Generic;

// Token: 0x02000BD9 RID: 3033
[Serializable]
public class NeighborDependencyData
{
	// Token: 0x06004017 RID: 16407 RVA: 0x00145808 File Offset: 0x00143A08
	public NeighborDependencyData(List<IndexNeighborDependency> bcs)
	{
		this.neighborDependencies = bcs;
	}

	// Token: 0x04003311 RID: 13073
	public List<IndexNeighborDependency> neighborDependencies;
}
