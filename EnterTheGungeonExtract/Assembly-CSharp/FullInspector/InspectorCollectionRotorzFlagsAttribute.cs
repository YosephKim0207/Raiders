using System;
using FullInspector.Rotorz.ReorderableList;

namespace FullInspector
{
	// Token: 0x020005F9 RID: 1529
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionRotorzFlagsAttribute : Attribute
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x0009CF68 File Offset: 0x0009B168
		// (set) Token: 0x060023E3 RID: 9187 RVA: 0x0009CF74 File Offset: 0x0009B174
		public bool DisableReordering
		{
			get
			{
				return this.HasFlag(ReorderableListFlags.DisableReordering);
			}
			set
			{
				this.UpdateFlag(value, ReorderableListFlags.DisableReordering);
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x0009CF80 File Offset: 0x0009B180
		// (set) Token: 0x060023E5 RID: 9189 RVA: 0x0009CF8C File Offset: 0x0009B18C
		public bool HideAddButton
		{
			get
			{
				return this.HasFlag(ReorderableListFlags.HideAddButton);
			}
			set
			{
				this.UpdateFlag(value, ReorderableListFlags.HideAddButton);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x0009CF98 File Offset: 0x0009B198
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x0009CFA4 File Offset: 0x0009B1A4
		public bool HideRemoveButtons
		{
			get
			{
				return this.HasFlag(ReorderableListFlags.HideRemoveButtons);
			}
			set
			{
				this.UpdateFlag(value, ReorderableListFlags.HideRemoveButtons);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x0009CFB0 File Offset: 0x0009B1B0
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x0009CFBC File Offset: 0x0009B1BC
		public bool ShowIndices
		{
			get
			{
				return this.HasFlag(ReorderableListFlags.ShowIndices);
			}
			set
			{
				this.UpdateFlag(value, ReorderableListFlags.ShowIndices);
			}
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x0009CFC8 File Offset: 0x0009B1C8
		private void UpdateFlag(bool shouldSet, ReorderableListFlags flag)
		{
			if (shouldSet)
			{
				this.Flags |= flag;
			}
			else
			{
				this.Flags &= ~flag;
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0009CFF4 File Offset: 0x0009B1F4
		private bool HasFlag(ReorderableListFlags flag)
		{
			return (this.Flags & flag) != (ReorderableListFlags)0;
		}

		// Token: 0x040018F3 RID: 6387
		public ReorderableListFlags Flags;
	}
}
