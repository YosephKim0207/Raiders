using System;

namespace BraveDynamicTree
{
	// Token: 0x0200035A RID: 858
	public class b2TreeNode
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x00040A28 File Offset: 0x0003EC28
		public bool IsLeaf()
		{
			return this.child1 == -1;
		}

		// Token: 0x04000DEE RID: 3566
		public b2AABB fatAabb;

		// Token: 0x04000DEF RID: 3567
		public b2AABB tightAabb;

		// Token: 0x04000DF0 RID: 3568
		public SpeculativeRigidbody rigidbody;

		// Token: 0x04000DF1 RID: 3569
		public int parent;

		// Token: 0x04000DF2 RID: 3570
		public int next;

		// Token: 0x04000DF3 RID: 3571
		public int child1;

		// Token: 0x04000DF4 RID: 3572
		public int child2;

		// Token: 0x04000DF5 RID: 3573
		public int height;
	}
}
