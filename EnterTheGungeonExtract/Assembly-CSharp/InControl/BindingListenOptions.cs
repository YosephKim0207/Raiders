using System;

namespace InControl
{
	// Token: 0x02000689 RID: 1673
	public class BindingListenOptions
	{
		// Token: 0x0600260C RID: 9740 RVA: 0x000A320C File Offset: 0x000A140C
		public bool CallOnBindingFound(PlayerAction playerAction, BindingSource bindingSource)
		{
			return this.OnBindingFound == null || this.OnBindingFound(playerAction, bindingSource);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000A3228 File Offset: 0x000A1428
		public void CallOnBindingAdded(PlayerAction playerAction, BindingSource bindingSource)
		{
			if (this.OnBindingAdded != null)
			{
				this.OnBindingAdded(playerAction, bindingSource);
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000A3244 File Offset: 0x000A1444
		public void CallOnBindingRejected(PlayerAction playerAction, BindingSource bindingSource, BindingSourceRejectionType bindingSourceRejectionType)
		{
			if (this.OnBindingRejected != null)
			{
				this.OnBindingRejected(playerAction, bindingSource, bindingSourceRejectionType);
			}
		}

		// Token: 0x040019DF RID: 6623
		public bool IncludeControllers = true;

		// Token: 0x040019E0 RID: 6624
		public bool IncludeUnknownControllers;

		// Token: 0x040019E1 RID: 6625
		public bool IncludeNonStandardControls = true;

		// Token: 0x040019E2 RID: 6626
		public bool IncludeMouseButtons;

		// Token: 0x040019E3 RID: 6627
		public bool IncludeMouseScrollWheel;

		// Token: 0x040019E4 RID: 6628
		public bool IncludeKeys = true;

		// Token: 0x040019E5 RID: 6629
		public bool IncludeModifiersAsFirstClassKeys;

		// Token: 0x040019E6 RID: 6630
		public uint MaxAllowedBindings;

		// Token: 0x040019E7 RID: 6631
		public uint MaxAllowedBindingsPerType;

		// Token: 0x040019E8 RID: 6632
		public bool AllowDuplicateBindingsPerSet;

		// Token: 0x040019E9 RID: 6633
		public bool UnsetDuplicateBindingsOnSet;

		// Token: 0x040019EA RID: 6634
		public bool RejectRedundantBindings;

		// Token: 0x040019EB RID: 6635
		public BindingSource ReplaceBinding;

		// Token: 0x040019EC RID: 6636
		public Func<PlayerAction, BindingSource, bool> OnBindingFound;

		// Token: 0x040019ED RID: 6637
		public Action<PlayerAction, BindingSource> OnBindingAdded;

		// Token: 0x040019EE RID: 6638
		public Action<PlayerAction, BindingSource, BindingSourceRejectionType> OnBindingRejected;
	}
}
