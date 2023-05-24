using System;

namespace FullInspector
{
	// Token: 0x02000530 RID: 1328
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class InspectorButtonAttribute : Attribute
	{
		// Token: 0x06001FD8 RID: 8152 RVA: 0x0008EC60 File Offset: 0x0008CE60
		public InspectorButtonAttribute()
		{
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0008EC68 File Offset: 0x0008CE68
		[Obsolete("Please use InspectorName to set the name of the button")]
		public InspectorButtonAttribute(string displayName)
		{
		}

		// Token: 0x0400176A RID: 5994
		[Obsolete("Please use InspectorName to get the custom name of the button")]
		public string DisplayName;
	}
}
