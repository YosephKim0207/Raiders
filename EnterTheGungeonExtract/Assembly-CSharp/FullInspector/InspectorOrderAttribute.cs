using System;
using System.Reflection;
using FullSerializer.Internal;

namespace FullInspector
{
	// Token: 0x02000539 RID: 1337
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorOrderAttribute : Attribute
	{
		// Token: 0x06001FE3 RID: 8163 RVA: 0x0008ECE8 File Offset: 0x0008CEE8
		public InspectorOrderAttribute(double order)
		{
			this.Order = order;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0008ECF8 File Offset: 0x0008CEF8
		public static double GetInspectorOrder(MemberInfo memberInfo)
		{
			InspectorOrderAttribute attribute = fsPortableReflection.GetAttribute<InspectorOrderAttribute>(memberInfo);
			if (attribute != null)
			{
				return attribute.Order;
			}
			return double.MaxValue;
		}

		// Token: 0x04001770 RID: 6000
		public double Order;
	}
}
