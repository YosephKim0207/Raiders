using System;

namespace FullInspector
{
	// Token: 0x02000537 RID: 1335
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[Obsolete("Please use [InspectorNullable] instead of [InspectorNotDefaultConstructed]")]
	public sealed class InspectorNotDefaultConstructedAttribute : Attribute
	{
	}
}
