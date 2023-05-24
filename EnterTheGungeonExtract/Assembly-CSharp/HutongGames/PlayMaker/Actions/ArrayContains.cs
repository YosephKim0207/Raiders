using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DD RID: 2269
	[Tooltip("Check if an Array contains a value. Optionally get its index.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayContains : FsmStateAction
	{
		// Token: 0x0600323B RID: 12859 RVA: 0x00108750 File Offset: 0x00106950
		public override void Reset()
		{
			this.array = null;
			this.value = null;
			this.index = null;
			this.isContained = null;
			this.isContainedEvent = null;
			this.isNotContainedEvent = null;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x0010877C File Offset: 0x0010697C
		public override void OnEnter()
		{
			this.DoCheckContainsValue();
			base.Finish();
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x0010878C File Offset: 0x0010698C
		private void DoCheckContainsValue()
		{
			this.value.UpdateValue();
			int num = Array.IndexOf<object>(this.array.Values, this.value.GetValue());
			bool flag = num != -1;
			this.isContained.Value = flag;
			this.index.Value = num;
			if (flag)
			{
				base.Fsm.Event(this.isContainedEvent);
			}
			else
			{
				base.Fsm.Event(this.isNotContainedEvent);
			}
		}

		// Token: 0x04002355 RID: 9045
		[Tooltip("The Array Variable to use.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x04002356 RID: 9046
		[Tooltip("The value to check against in the array.")]
		[RequiredField]
		[MatchElementType("array")]
		public FsmVar value;

		// Token: 0x04002357 RID: 9047
		[UIHint(UIHint.Variable)]
		[Tooltip("The index of the value in the array.")]
		[ActionSection("Result")]
		public FsmInt index;

		// Token: 0x04002358 RID: 9048
		[UIHint(UIHint.Variable)]
		[Tooltip("Store in a bool whether it contains that element or not (described below)")]
		public FsmBool isContained;

		// Token: 0x04002359 RID: 9049
		[Tooltip("Event sent if the array contains that element (described below)")]
		public FsmEvent isContainedEvent;

		// Token: 0x0400235A RID: 9050
		[Tooltip("Event sent if the array does not contains that element (described below)")]
		public FsmEvent isNotContainedEvent;
	}
}
