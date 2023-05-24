using System;

namespace FullInspector
{
	// Token: 0x020005FA RID: 1530
	[Obsolete("Use [InspectorCollectionRotorzFlags(ShowIndices=true)] instead")]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionShowIndicesAttribute : Attribute
	{
	}
}
