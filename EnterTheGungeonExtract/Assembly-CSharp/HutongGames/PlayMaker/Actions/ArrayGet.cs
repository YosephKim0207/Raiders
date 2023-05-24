using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E0 RID: 2272
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Get a value at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
	public class ArrayGet : FsmStateAction
	{
		// Token: 0x0600324E RID: 12878 RVA: 0x00108AD0 File Offset: 0x00106CD0
		public override void Reset()
		{
			this.array = null;
			this.index = null;
			this.everyFrame = false;
			this.storeValue = null;
			this.indexOutOfRange = null;
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x00108AF8 File Offset: 0x00106CF8
		public override void OnEnter()
		{
			this.DoGetValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00108B14 File Offset: 0x00106D14
		public override void OnUpdate()
		{
			this.DoGetValue();
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x00108B1C File Offset: 0x00106D1C
		private void DoGetValue()
		{
			if (this.array.IsNone || this.storeValue.IsNone)
			{
				return;
			}
			if (this.index.Value >= 0 && this.index.Value < this.array.Length)
			{
				this.storeValue.SetValue(this.array.Get(this.index.Value));
			}
			else
			{
				base.Fsm.Event(this.indexOutOfRange);
			}
		}

		// Token: 0x04002363 RID: 9059
		[Tooltip("The Array Variable to use.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		// Token: 0x04002364 RID: 9060
		[Tooltip("The index into the array.")]
		public FsmInt index;

		// Token: 0x04002365 RID: 9061
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the value in a variable.")]
		[RequiredField]
		[MatchElementType("array")]
		public FsmVar storeValue;

		// Token: 0x04002366 RID: 9062
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002367 RID: 9063
		[Tooltip("The event to trigger if the index is out of range")]
		[ActionSection("Events")]
		public FsmEvent indexOutOfRange;
	}
}
