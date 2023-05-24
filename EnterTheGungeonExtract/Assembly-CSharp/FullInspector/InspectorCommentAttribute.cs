using System;

namespace FullInspector
{
	// Token: 0x020005E2 RID: 1506
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorCommentAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x060023B3 RID: 9139 RVA: 0x0009C98C File Offset: 0x0009AB8C
		public InspectorCommentAttribute(string comment)
			: this(fiSettings.DefaultCommentType, comment)
		{
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0009C99C File Offset: 0x0009AB9C
		public InspectorCommentAttribute(CommentType type, string comment)
		{
			this.Type = type;
			this.Comment = comment;
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x0009C9C4 File Offset: 0x0009ABC4
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018CA RID: 6346
		public string Comment;

		// Token: 0x040018CB RID: 6347
		public CommentType Type;

		// Token: 0x040018CC RID: 6348
		public double Order = 100.0;
	}
}
