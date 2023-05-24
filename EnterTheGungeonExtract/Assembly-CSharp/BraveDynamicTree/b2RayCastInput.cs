using System;
using UnityEngine;

namespace BraveDynamicTree
{
	// Token: 0x0200035B RID: 859
	public struct b2RayCastInput
	{
		// Token: 0x06000D97 RID: 3479 RVA: 0x00040A34 File Offset: 0x0003EC34
		public b2RayCastInput(Vector2 p1, Vector2 p2)
		{
			this.p1 = p1;
			this.p2 = p2;
			this.maxFraction = 1f;
		}

		// Token: 0x04000DF6 RID: 3574
		public Vector2 p1;

		// Token: 0x04000DF7 RID: 3575
		public Vector2 p2;

		// Token: 0x04000DF8 RID: 3576
		public float maxFraction;
	}
}
