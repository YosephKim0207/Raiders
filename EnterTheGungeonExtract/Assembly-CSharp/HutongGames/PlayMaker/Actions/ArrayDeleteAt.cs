using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DE RID: 2270
	[Tooltip("Delete the item at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayDeleteAt : FsmStateAction
	{
		// Token: 0x0600323F RID: 12863 RVA: 0x00108818 File Offset: 0x00106A18
		public override void Reset()
		{
			this.array = null;
			this.index = null;
			this.indexOutOfRangeEvent = null;
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x00108830 File Offset: 0x00106A30
		public override void OnEnter()
		{
			this.DoDeleteAt();
			base.Finish();
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x00108840 File Offset: 0x00106A40
		private void DoDeleteAt()
		{
			if (this.index.Value >= 0 && this.index.Value < this.array.Length)
			{
				List<object> list = new List<object>(this.array.Values);
				list.RemoveAt(this.index.Value);
				this.array.Values = list.ToArray();
			}
			else
			{
				base.Fsm.Event(this.indexOutOfRangeEvent);
			}
		}

		// Token: 0x0400235B RID: 9051
		[Tooltip("The Array Variable to use.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		// Token: 0x0400235C RID: 9052
		[Tooltip("The index into the array.")]
		public FsmInt index;

		// Token: 0x0400235D RID: 9053
		[Tooltip("The event to trigger if the index is out of range")]
		[ActionSection("Result")]
		public FsmEvent indexOutOfRangeEvent;
	}
}
