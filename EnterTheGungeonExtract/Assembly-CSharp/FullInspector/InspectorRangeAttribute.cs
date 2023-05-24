using System;

namespace FullInspector
{
	// Token: 0x02000608 RID: 1544
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorRangeAttribute : Attribute
	{
		// Token: 0x06002420 RID: 9248 RVA: 0x0009D6D0 File Offset: 0x0009B8D0
		public InspectorRangeAttribute(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x0400190B RID: 6411
		public float Min;

		// Token: 0x0400190C RID: 6412
		public float Max;

		// Token: 0x0400190D RID: 6413
		public float Step = float.NaN;
	}
}
