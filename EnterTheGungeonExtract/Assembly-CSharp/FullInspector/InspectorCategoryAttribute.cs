using System;

namespace FullInspector
{
	// Token: 0x02000531 RID: 1329
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class InspectorCategoryAttribute : Attribute
	{
		// Token: 0x06001FDA RID: 8154 RVA: 0x0008EC70 File Offset: 0x0008CE70
		public InspectorCategoryAttribute(string category)
		{
			this.Category = category;
		}

		// Token: 0x0400176B RID: 5995
		public string Category;
	}
}
