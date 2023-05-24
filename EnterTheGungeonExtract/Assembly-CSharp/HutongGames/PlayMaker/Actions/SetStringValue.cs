using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B15 RID: 2837
	[Tooltip("Sets the value of a String Variable.")]
	[ActionCategory(ActionCategory.String)]
	public class SetStringValue : FsmStateAction
	{
		// Token: 0x06003BCD RID: 15309 RVA: 0x0012D458 File Offset: 0x0012B658
		public override void Reset()
		{
			this.stringVariable = null;
			this.stringValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0012D470 File Offset: 0x0012B670
		public override void OnEnter()
		{
			this.DoSetStringValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x0012D48C File Offset: 0x0012B68C
		public override void OnUpdate()
		{
			this.DoSetStringValue();
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0012D494 File Offset: 0x0012B694
		private void DoSetStringValue()
		{
			if (this.stringVariable == null)
			{
				return;
			}
			if (this.stringValue == null)
			{
				return;
			}
			this.stringVariable.Value = this.stringValue.Value;
		}

		// Token: 0x04002DED RID: 11757
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002DEE RID: 11758
		[UIHint(UIHint.TextArea)]
		public FsmString stringValue;

		// Token: 0x04002DEF RID: 11759
		public bool everyFrame;
	}
}
