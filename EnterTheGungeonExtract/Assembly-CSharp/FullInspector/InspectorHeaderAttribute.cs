using System;

namespace FullInspector
{
	// Token: 0x020005E5 RID: 1509
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorHeaderAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x0009C9F4 File Offset: 0x0009ABF4
		public InspectorHeaderAttribute(string header)
		{
			this.Header = header;
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x0009CA14 File Offset: 0x0009AC14
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018CE RID: 6350
		public double Order = 75.0;

		// Token: 0x040018CF RID: 6351
		public string Header;
	}
}
