using System;

namespace DaikonForge.Editor
{
	// Token: 0x02000525 RID: 1317
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class InspectorAttribute : Attribute, IComparable<InspectorAttribute>
	{
		// Token: 0x06001FA3 RID: 8099 RVA: 0x0008DFC4 File Offset: 0x0008C1C4
		public InspectorAttribute(string group)
		{
			this.Group = group;
			this.Order = int.MaxValue;
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0008DFE0 File Offset: 0x0008C1E0
		public InspectorAttribute(string category, int order)
		{
			this.Group = category;
			this.Order = order;
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0008DFF8 File Offset: 0x0008C1F8
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x0008E000 File Offset: 0x0008C200
		public string Group { get; set; }

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x0008E00C File Offset: 0x0008C20C
		// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x0008E014 File Offset: 0x0008C214
		public int Order { get; set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0008E020 File Offset: 0x0008C220
		// (set) Token: 0x06001FAA RID: 8106 RVA: 0x0008E028 File Offset: 0x0008C228
		public string Label { get; set; }

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x0008E034 File Offset: 0x0008C234
		// (set) Token: 0x06001FAC RID: 8108 RVA: 0x0008E03C File Offset: 0x0008C23C
		public string BackingField { get; set; }

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x0008E048 File Offset: 0x0008C248
		// (set) Token: 0x06001FAE RID: 8110 RVA: 0x0008E050 File Offset: 0x0008C250
		public string Tooltip { get; set; }

		// Token: 0x06001FAF RID: 8111 RVA: 0x0008E05C File Offset: 0x0008C25C
		public override string ToString()
		{
			string text = "{0} {1} - {2}";
			object group = this.Group;
			object obj = this.Order;
			string text2;
			if ((text2 = this.Label) == null)
			{
				text2 = this.BackingField ?? "(Unknown)";
			}
			return string.Format(text, group, obj, text2);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0008E098 File Offset: 0x0008C298
		public int CompareTo(InspectorAttribute other)
		{
			if (!string.Equals(this.Group, other.Group))
			{
				return this.Group.CompareTo(other.Group);
			}
			if (this.Order != other.Order)
			{
				return this.Order.CompareTo(other.Order);
			}
			string text = this.Label ?? this.BackingField;
			string text2 = other.Label ?? other.BackingField;
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				return text.CompareTo(text2);
			}
			return 0;
		}
	}
}
