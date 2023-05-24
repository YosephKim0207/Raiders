using System;

namespace FullInspector
{
	// Token: 0x020005F8 RID: 1528
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionPagerAttribute : Attribute
	{
		// Token: 0x060023DB RID: 9179 RVA: 0x0009CEE4 File Offset: 0x0009B0E4
		public InspectorCollectionPagerAttribute()
		{
			this.PageMinimumCollectionLength = fiSettings.DefaultPageMinimumCollectionLength;
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0009CEF8 File Offset: 0x0009B0F8
		public InspectorCollectionPagerAttribute(int pageMinimumCollectionLength)
		{
			this.PageMinimumCollectionLength = pageMinimumCollectionLength;
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0009CF28 File Offset: 0x0009B128
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x0009CF08 File Offset: 0x0009B108
		public bool AlwaysHide
		{
			get
			{
				return this.PageMinimumCollectionLength < 0;
			}
			set
			{
				if (value)
				{
					this.PageMinimumCollectionLength = -1;
				}
				else
				{
					this.PageMinimumCollectionLength = fiSettings.DefaultPageMinimumCollectionLength;
				}
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x0009CF54 File Offset: 0x0009B154
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x0009CF34 File Offset: 0x0009B134
		public bool AlwaysShow
		{
			get
			{
				return this.PageMinimumCollectionLength == 0;
			}
			set
			{
				if (value)
				{
					this.PageMinimumCollectionLength = 0;
				}
				else
				{
					this.PageMinimumCollectionLength = fiSettings.DefaultPageMinimumCollectionLength;
				}
			}
		}

		// Token: 0x040018F2 RID: 6386
		public int PageMinimumCollectionLength;
	}
}
