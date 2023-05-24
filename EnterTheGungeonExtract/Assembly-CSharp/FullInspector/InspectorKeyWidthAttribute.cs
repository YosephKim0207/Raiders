using System;

namespace FullInspector
{
	// Token: 0x02000600 RID: 1536
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorKeyWidthAttribute : Attribute
	{
		// Token: 0x060023F1 RID: 9201 RVA: 0x0009D034 File Offset: 0x0009B234
		public InspectorKeyWidthAttribute(float widthPercentage)
		{
			if (widthPercentage < 0f || widthPercentage >= 1f)
			{
				throw new ArgumentException("widthPercentage must be between [0,1]");
			}
			this.WidthPercentage = widthPercentage;
		}

		// Token: 0x040018FE RID: 6398
		public float WidthPercentage;
	}
}
