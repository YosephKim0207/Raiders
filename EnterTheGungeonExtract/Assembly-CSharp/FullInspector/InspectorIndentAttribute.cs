using System;

namespace FullInspector
{
	// Token: 0x02000535 RID: 1333
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorIndentAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x0008ECC0 File Offset: 0x0008CEC0
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x0400176E RID: 5998
		public double Order = 100.0;
	}
}
