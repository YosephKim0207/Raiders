using System;

namespace FullInspector
{
	// Token: 0x02000536 RID: 1334
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorNameAttribute : Attribute
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x0008ECC8 File Offset: 0x0008CEC8
		public InspectorNameAttribute(string displayName)
		{
			this.DisplayName = displayName;
		}

		// Token: 0x0400176F RID: 5999
		public string DisplayName;
	}
}
