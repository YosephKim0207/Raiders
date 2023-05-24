using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E1 RID: 2273
	[Tooltip("Each time this action is called it gets the next item from a Array. \nThis lets you quickly loop through all the items of an array to perform actions on them.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayGetNext : FsmStateAction
	{
		// Token: 0x06003253 RID: 12883 RVA: 0x00108BB8 File Offset: 0x00106DB8
		public override void Reset()
		{
			this.array = null;
			this.startIndex = null;
			this.endIndex = null;
			this.currentIndex = null;
			this.loopEvent = null;
			this.finishedEvent = null;
			this.result = null;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00108BEC File Offset: 0x00106DEC
		public override void OnEnter()
		{
			if (this.nextItemIndex == 0 && this.startIndex.Value > 0)
			{
				this.nextItemIndex = this.startIndex.Value;
			}
			this.DoGetNextItem();
			base.Finish();
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x00108C28 File Offset: 0x00106E28
		private void DoGetNextItem()
		{
			if (this.nextItemIndex >= this.array.Length)
			{
				this.nextItemIndex = 0;
				this.currentIndex.Value = this.array.Length - 1;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.result.SetValue(this.array.Get(this.nextItemIndex));
			if (this.nextItemIndex >= this.array.Length)
			{
				this.nextItemIndex = 0;
				this.currentIndex.Value = this.array.Length - 1;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			if (this.endIndex.Value > 0 && this.nextItemIndex >= this.endIndex.Value)
			{
				this.nextItemIndex = 0;
				this.currentIndex.Value = this.endIndex.Value;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.nextItemIndex++;
			this.currentIndex.Value = this.nextItemIndex - 1;
			if (this.loopEvent != null)
			{
				base.Fsm.Event(this.loopEvent);
			}
		}

		// Token: 0x04002368 RID: 9064
		[Tooltip("The Array Variable to use.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		// Token: 0x04002369 RID: 9065
		[Tooltip("From where to start iteration, leave as 0 to start from the beginning")]
		public FsmInt startIndex;

		// Token: 0x0400236A RID: 9066
		[Tooltip("When to end iteration, leave as 0 to iterate until the end")]
		public FsmInt endIndex;

		// Token: 0x0400236B RID: 9067
		[Tooltip("Event to send to get the next item.")]
		public FsmEvent loopEvent;

		// Token: 0x0400236C RID: 9068
		[Tooltip("Event to send when there are no more items.")]
		public FsmEvent finishedEvent;

		// Token: 0x0400236D RID: 9069
		[MatchElementType("array")]
		[ActionSection("Result")]
		public FsmVar result;

		// Token: 0x0400236E RID: 9070
		[UIHint(UIHint.Variable)]
		public FsmInt currentIndex;

		// Token: 0x0400236F RID: 9071
		private int nextItemIndex;
	}
}
