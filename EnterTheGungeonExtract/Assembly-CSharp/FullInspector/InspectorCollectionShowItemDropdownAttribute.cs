using System;

namespace FullInspector
{
	// Token: 0x020005FB RID: 1531
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionShowItemDropdownAttribute : Attribute
	{
		// Token: 0x040018F4 RID: 6388
		public bool IsCollapsedByDefault = true;
	}
}
