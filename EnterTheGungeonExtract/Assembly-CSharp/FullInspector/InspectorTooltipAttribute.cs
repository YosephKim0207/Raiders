using System;

namespace FullInspector
{
	// Token: 0x0200053C RID: 1340
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorTooltipAttribute : Attribute
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x0008ED44 File Offset: 0x0008CF44
		public InspectorTooltipAttribute(string tooltip)
		{
			this.Tooltip = tooltip;
		}

		// Token: 0x04001773 RID: 6003
		public string Tooltip;
	}
}
