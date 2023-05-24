using System;
using UnityEngine;

namespace DungeonGenUtility
{
	// Token: 0x02000EE8 RID: 3816
	public class Edge
	{
		// Token: 0x06005148 RID: 20808 RVA: 0x001CD4C8 File Offset: 0x001CB6C8
		public Edge(Vector2 vert0, Vector2 vert1)
		{
			this.v0 = vert0;
			this.v1 = vert1;
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06005149 RID: 20809 RVA: 0x001CD4E0 File Offset: 0x001CB6E0
		private float Length
		{
			get
			{
				return Vector2.Distance(this.v0, this.v1);
			}
		}

		// Token: 0x04004942 RID: 18754
		private Vector2 v0;

		// Token: 0x04004943 RID: 18755
		private Vector2 v1;
	}
}
