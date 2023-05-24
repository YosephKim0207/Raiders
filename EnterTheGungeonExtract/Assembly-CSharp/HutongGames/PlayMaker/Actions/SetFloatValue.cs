using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD4 RID: 2772
	[Tooltip("Sets the value of a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class SetFloatValue : FsmStateAction
	{
		// Token: 0x06003AAF RID: 15023 RVA: 0x00129D48 File Offset: 0x00127F48
		public override void Reset()
		{
			this.floatVariable = null;
			this.floatValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00129D60 File Offset: 0x00127F60
		public override void OnEnter()
		{
			this.floatVariable.Value = this.floatValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00129D8C File Offset: 0x00127F8C
		public override void OnUpdate()
		{
			this.floatVariable.Value = this.floatValue.Value;
		}

		// Token: 0x04002CD4 RID: 11476
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		// Token: 0x04002CD5 RID: 11477
		[RequiredField]
		public FsmFloat floatValue;

		// Token: 0x04002CD6 RID: 11478
		public bool everyFrame;
	}
}
