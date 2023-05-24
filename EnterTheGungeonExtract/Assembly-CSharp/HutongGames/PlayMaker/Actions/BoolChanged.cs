using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FE RID: 2302
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a Bool Variable has changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class BoolChanged : FsmStateAction
	{
		// Token: 0x060032BB RID: 12987 RVA: 0x0010A614 File Offset: 0x00108814
		public override void Reset()
		{
			this.boolVariable = null;
			this.changedEvent = null;
			this.storeResult = null;
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x0010A62C File Offset: 0x0010882C
		public override void OnEnter()
		{
			if (this.boolVariable.IsNone)
			{
				base.Finish();
				return;
			}
			this.previousValue = this.boolVariable.Value;
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x0010A658 File Offset: 0x00108858
		public override void OnUpdate()
		{
			this.storeResult.Value = false;
			if (this.boolVariable.Value != this.previousValue)
			{
				this.storeResult.Value = true;
				base.Fsm.Event(this.changedEvent);
			}
		}

		// Token: 0x040023E3 RID: 9187
		[Tooltip("The Bool variable to watch for changes.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x040023E4 RID: 9188
		[Tooltip("Event to send if the variable changes.")]
		public FsmEvent changedEvent;

		// Token: 0x040023E5 RID: 9189
		[Tooltip("Set to True if changed.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040023E6 RID: 9190
		private bool previousValue;
	}
}
