using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094D RID: 2381
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a Float variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class FloatChanged : FsmStateAction
	{
		// Token: 0x0600340D RID: 13325 RVA: 0x0010EFC4 File Offset: 0x0010D1C4
		public override void Reset()
		{
			this.floatVariable = null;
			this.changedEvent = null;
			this.storeResult = null;
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x0010EFDC File Offset: 0x0010D1DC
		public override void OnEnter()
		{
			if (this.floatVariable.IsNone)
			{
				base.Finish();
				return;
			}
			this.previousValue = this.floatVariable.Value;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x0010F008 File Offset: 0x0010D208
		public override void OnUpdate()
		{
			this.storeResult.Value = false;
			if (this.floatVariable.Value != this.previousValue)
			{
				this.previousValue = this.floatVariable.Value;
				this.storeResult.Value = true;
				base.Fsm.Event(this.changedEvent);
			}
		}

		// Token: 0x04002529 RID: 9513
		[Tooltip("The Float variable to watch for a change.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x0400252A RID: 9514
		[Tooltip("Event to send if the float variable changes.")]
		public FsmEvent changedEvent;

		// Token: 0x0400252B RID: 9515
		[Tooltip("Set to True if the float variable changes.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x0400252C RID: 9516
		private float previousValue;
	}
}
