using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B2 RID: 2482
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets a sub-string from a String Variable.")]
	public class GetSubstring : FsmStateAction
	{
		// Token: 0x060035C2 RID: 13762 RVA: 0x00114024 File Offset: 0x00112224
		public override void Reset()
		{
			this.stringVariable = null;
			this.startIndex = 0;
			this.length = 1;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x00114054 File Offset: 0x00112254
		public override void OnEnter()
		{
			this.DoGetSubstring();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x00114070 File Offset: 0x00112270
		public override void OnUpdate()
		{
			this.DoGetSubstring();
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x00114078 File Offset: 0x00112278
		private void DoGetSubstring()
		{
			if (this.stringVariable == null)
			{
				return;
			}
			if (this.storeResult == null)
			{
				return;
			}
			this.storeResult.Value = this.stringVariable.Value.Substring(this.startIndex.Value, this.length.Value);
		}

		// Token: 0x04002708 RID: 9992
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		// Token: 0x04002709 RID: 9993
		[RequiredField]
		public FsmInt startIndex;

		// Token: 0x0400270A RID: 9994
		[RequiredField]
		public FsmInt length;

		// Token: 0x0400270B RID: 9995
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		// Token: 0x0400270C RID: 9996
		public bool everyFrame;
	}
}
