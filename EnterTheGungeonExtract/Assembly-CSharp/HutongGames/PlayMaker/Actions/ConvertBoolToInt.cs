using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000916 RID: 2326
	[Tooltip("Converts a Bool value to an Integer value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToInt : FsmStateAction
	{
		// Token: 0x06003338 RID: 13112 RVA: 0x0010C8C8 File Offset: 0x0010AAC8
		public override void Reset()
		{
			this.boolVariable = null;
			this.intVariable = null;
			this.falseValue = 0;
			this.trueValue = 1;
			this.everyFrame = false;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x0010C8F8 File Offset: 0x0010AAF8
		public override void OnEnter()
		{
			this.DoConvertBoolToInt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x0010C914 File Offset: 0x0010AB14
		public override void OnUpdate()
		{
			this.DoConvertBoolToInt();
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x0010C91C File Offset: 0x0010AB1C
		private void DoConvertBoolToInt()
		{
			this.intVariable.Value = ((!this.boolVariable.Value) ? this.falseValue.Value : this.trueValue.Value);
		}

		// Token: 0x04002469 RID: 9321
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x0400246A RID: 9322
		[Tooltip("The Integer variable to set based on the Bool variable value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x0400246B RID: 9323
		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt falseValue;

		// Token: 0x0400246C RID: 9324
		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt trueValue;

		// Token: 0x0400246D RID: 9325
		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;
	}
}
