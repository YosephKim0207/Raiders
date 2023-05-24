using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenUtility
{
	// Token: 0x02000EE7 RID: 3815
	public class DungeonGraph
	{
		// Token: 0x06005147 RID: 20807 RVA: 0x001CD4A8 File Offset: 0x001CB6A8
		public DungeonGraph()
		{
			this.vertices = new List<Vector2>();
			this.edges = new List<Edge>();
		}

		// Token: 0x04004940 RID: 18752
		public List<Vector2> vertices;

		// Token: 0x04004941 RID: 18753
		public List<Edge> edges;
	}
}
