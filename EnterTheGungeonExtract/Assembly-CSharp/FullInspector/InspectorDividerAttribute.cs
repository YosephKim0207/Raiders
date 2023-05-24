using System;

namespace FullInspector
{
	// Token: 0x020005E4 RID: 1508
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorDividerAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x0009C9EC File Offset: 0x0009ABEC
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018CD RID: 6349
		public double Order = 50.0;
	}
}
