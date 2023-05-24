using System;

namespace FullInspector
{
	// Token: 0x0200053B RID: 1339
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorHideIfAttribute : Attribute
	{
		// Token: 0x06001FE6 RID: 8166 RVA: 0x0008ED34 File Offset: 0x0008CF34
		public InspectorHideIfAttribute(string conditionalMemberName)
		{
			this.ConditionalMemberName = conditionalMemberName;
		}

		// Token: 0x04001772 RID: 6002
		public string ConditionalMemberName;
	}
}
