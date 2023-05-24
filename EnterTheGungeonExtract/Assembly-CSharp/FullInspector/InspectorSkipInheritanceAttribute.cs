using System;

namespace FullInspector
{
	// Token: 0x020005E9 RID: 1513
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorSkipInheritanceAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x0009CA68 File Offset: 0x0009AC68
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return double.MinValue;
			}
		}
	}
}
