using System;

namespace FullInspector
{
	// Token: 0x02000533 RID: 1331
	[Obsolete("Please use InspectorShowIfAttribute instead")]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorDisplayIfAttribute : Attribute
	{
		// Token: 0x06001FDC RID: 8156 RVA: 0x0008EC88 File Offset: 0x0008CE88
		public InspectorDisplayIfAttribute(string conditionalMemberName)
		{
			this.ConditionalMemberName = conditionalMemberName;
		}

		// Token: 0x0400176C RID: 5996
		public string ConditionalMemberName;
	}
}
