using System;

namespace FullInspector
{
	// Token: 0x020005E0 RID: 1504
	[Obsolete("Use [InspectorComment] instead of [Comment]")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
	public class CommentAttribute : Attribute, IInspectorAttributeOrder
	{
		// Token: 0x060023B0 RID: 9136 RVA: 0x0009C950 File Offset: 0x0009AB50
		public CommentAttribute(string comment)
			: this(CommentType.Info, comment)
		{
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0009C95C File Offset: 0x0009AB5C
		public CommentAttribute(CommentType type, string comment)
		{
			this.Type = type;
			this.Comment = comment;
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x0009C984 File Offset: 0x0009AB84
		double IInspectorAttributeOrder.Order
		{
			get
			{
				return this.Order;
			}
		}

		// Token: 0x040018C2 RID: 6338
		public string Comment;

		// Token: 0x040018C3 RID: 6339
		public CommentType Type;

		// Token: 0x040018C4 RID: 6340
		public double Order = 100.0;
	}
}
