using System;

namespace FullInspector
{
	// Token: 0x020005E8 RID: 1512
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorMarginAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x060023BF RID: 9151 RVA: 0x0009CA48 File Offset: 0x0009AC48
		public InspectorMarginAttribute(int margin)
		{
			this.Margin = margin;
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x0009CA58 File Offset: 0x0009AC58
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018D2 RID: 6354
		public int Margin;

		// Token: 0x040018D3 RID: 6355
		public double Order;
	}
}
