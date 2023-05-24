using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000915 RID: 2325
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Bool value to a Float value.")]
	public class ConvertBoolToFloat : FsmStateAction
	{
		// Token: 0x06003333 RID: 13107 RVA: 0x0010C82C File Offset: 0x0010AA2C
		public override void Reset()
		{
			this.boolVariable = null;
			this.floatVariable = null;
			this.falseValue = 0f;
			this.trueValue = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x0010C864 File Offset: 0x0010AA64
		public override void OnEnter()
		{
			this.DoConvertBoolToFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x0010C880 File Offset: 0x0010AA80
		public override void OnUpdate()
		{
			this.DoConvertBoolToFloat();
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x0010C888 File Offset: 0x0010AA88
		private void DoConvertBoolToFloat()
		{
			this.floatVariable.Value = ((!this.boolVariable.Value) ? this.falseValue.Value : this.trueValue.Value);
		}

		// Token: 0x04002464 RID: 9316
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x04002465 RID: 9317
		[Tooltip("The Float variable to set based on the Bool variable value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002466 RID: 9318
		[Tooltip("Float value if Bool variable is false.")]
		public FsmFloat falseValue;

		// Token: 0x04002467 RID: 9319
		[Tooltip("Float value if Bool variable is true.")]
		public FsmFloat trueValue;

		// Token: 0x04002468 RID: 9320
		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;
	}
}
