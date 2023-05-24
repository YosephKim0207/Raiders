using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B29 RID: 2857
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a string variable has changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class StringChanged : FsmStateAction
	{
		// Token: 0x06003C23 RID: 15395 RVA: 0x0012ECF4 File Offset: 0x0012CEF4
		public override void Reset()
		{
			this.stringVariable = null;
			this.changedEvent = null;
			this.storeResult = null;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x0012ED0C File Offset: 0x0012CF0C
		public override void OnEnter()
		{
			if (this.stringVariable.IsNone)
			{
				base.Finish();
				return;
			}
			this.previousValue = this.stringVariable.Value;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x0012ED38 File Offset: 0x0012CF38
		public override void OnUpdate()
		{
			if (this.stringVariable.Value != this.previousValue)
			{
				this.storeResult.Value = true;
				base.Fsm.Event(this.changedEvent);
			}
		}

		// Token: 0x04002E50 RID: 11856
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002E51 RID: 11857
		public FsmEvent changedEvent;

		// Token: 0x04002E52 RID: 11858
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002E53 RID: 11859
		private string previousValue;
	}
}
