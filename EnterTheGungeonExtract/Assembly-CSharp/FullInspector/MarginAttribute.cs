using System;

namespace FullInspector
{
	// Token: 0x020005E7 RID: 1511
	[Obsolete("Please use [InspectorMargin] instead of [Margin]")]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MarginAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x060023BD RID: 9149 RVA: 0x0009CA30 File Offset: 0x0009AC30
		public MarginAttribute(int margin)
		{
			this.Margin = margin;
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x0009CA40 File Offset: 0x0009AC40
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018D0 RID: 6352
		public int Margin;

		// Token: 0x040018D1 RID: 6353
		public double Order;
	}
}
