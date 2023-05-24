using System;

namespace FullInspector
{
	// Token: 0x02000534 RID: 1332
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public sealed class InspectorDropdownNameAttribute : Attribute
	{
		// Token: 0x06001FDD RID: 8157 RVA: 0x0008EC98 File Offset: 0x0008CE98
		public InspectorDropdownNameAttribute(string displayName)
		{
			this.DisplayName = displayName;
		}

		// Token: 0x0400176D RID: 5997
		public string DisplayName;
	}
}
