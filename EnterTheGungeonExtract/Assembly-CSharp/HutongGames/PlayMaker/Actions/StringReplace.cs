using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2C RID: 2860
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Replace a substring with a new String.")]
	public class StringReplace : FsmStateAction
	{
		// Token: 0x06003C31 RID: 15409 RVA: 0x0012EF88 File Offset: 0x0012D188
		public override void Reset()
		{
			this.stringVariable = null;
			this.replace = string.Empty;
			this.with = string.Empty;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0012EFC0 File Offset: 0x0012D1C0
		public override void OnEnter()
		{
			this.DoReplace();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0012EFDC File Offset: 0x0012D1DC
		public override void OnUpdate()
		{
			this.DoReplace();
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0012EFE4 File Offset: 0x0012D1E4
		private void DoReplace()
		{
			if (this.stringVariable == null)
			{
				return;
			}
			if (this.storeResult == null)
			{
				return;
			}
			this.storeResult.Value = this.stringVariable.Value.Replace(this.replace.Value, this.with.Value);
		}

		// Token: 0x04002E60 RID: 11872
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002E61 RID: 11873
		public FsmString replace;

		// Token: 0x04002E62 RID: 11874
		public FsmString with;

		// Token: 0x04002E63 RID: 11875
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		// Token: 0x04002E64 RID: 11876
		public bool everyFrame;
	}
}
