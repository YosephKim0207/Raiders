using System;

namespace FullInspector
{
	// Token: 0x0200053A RID: 1338
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorShowIfAttribute : Attribute
	{
		// Token: 0x06001FE5 RID: 8165 RVA: 0x0008ED24 File Offset: 0x0008CF24
		public InspectorShowIfAttribute(string conditionalMemberName)
		{
			this.ConditionalMemberName = conditionalMemberName;
		}

		// Token: 0x04001771 RID: 6001
		public string ConditionalMemberName;
	}
}
