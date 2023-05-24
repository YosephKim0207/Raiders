using System;

namespace FullInspector
{
	// Token: 0x020005E6 RID: 1510
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorHidePrimaryAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x0009CA24 File Offset: 0x0009AC24
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return double.MaxValue;
			}
		}
	}
}
