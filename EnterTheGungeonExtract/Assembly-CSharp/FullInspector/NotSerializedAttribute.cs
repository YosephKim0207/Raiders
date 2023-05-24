using System;

namespace FullInspector
{
	// Token: 0x0200053D RID: 1341
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class NotSerializedAttribute : Attribute
	{
	}
}
