using System;

namespace FullInspector
{
	// Token: 0x020005EA RID: 1514
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorTextAreaAttribute : Attribute
	{
		// Token: 0x060023C3 RID: 9155 RVA: 0x0009CA74 File Offset: 0x0009AC74
		public InspectorTextAreaAttribute()
			: this(250f)
		{
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0009CA84 File Offset: 0x0009AC84
		public InspectorTextAreaAttribute(float height)
		{
			this.Height = height;
		}

		// Token: 0x040018D4 RID: 6356
		public float Height;
	}
}
