using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2B RID: 2859
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a String contains another String.")]
	public class StringContains : FsmStateAction
	{
		// Token: 0x06003C2C RID: 15404 RVA: 0x0012EE7C File Offset: 0x0012D07C
		public override void Reset()
		{
			this.stringVariable = null;
			this.containsString = string.Empty;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0012EEB4 File Offset: 0x0012D0B4
		public override void OnEnter()
		{
			this.DoStringContains();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0012EED0 File Offset: 0x0012D0D0
		public override void OnUpdate()
		{
			this.DoStringContains();
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0012EED8 File Offset: 0x0012D0D8
		private void DoStringContains()
		{
			if (this.stringVariable.IsNone || this.containsString.IsNone)
			{
				return;
			}
			bool flag = this.stringVariable.Value.Contains(this.containsString.Value);
			if (this.storeResult != null)
			{
				this.storeResult.Value = flag;
			}
			if (flag && this.trueEvent != null)
			{
				base.Fsm.Event(this.trueEvent);
				return;
			}
			if (!flag && this.falseEvent != null)
			{
				base.Fsm.Event(this.falseEvent);
			}
		}

		// Token: 0x04002E5A RID: 11866
		[Tooltip("The String variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002E5B RID: 11867
		[Tooltip("Test if the String variable contains this string.")]
		[RequiredField]
		public FsmString containsString;

		// Token: 0x04002E5C RID: 11868
		[Tooltip("Event to send if true.")]
		public FsmEvent trueEvent;

		// Token: 0x04002E5D RID: 11869
		[Tooltip("Event to send if false.")]
		public FsmEvent falseEvent;

		// Token: 0x04002E5E RID: 11870
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;

		// Token: 0x04002E5F RID: 11871
		[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;
	}
}
