using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200091C RID: 2332
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an Integer value to a Float value.")]
	public class ConvertIntToFloat : FsmStateAction
	{
		// Token: 0x06003351 RID: 13137 RVA: 0x0010CC20 File Offset: 0x0010AE20
		public override void Reset()
		{
			this.intVariable = null;
			this.floatVariable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x0010CC38 File Offset: 0x0010AE38
		public override void OnEnter()
		{
			this.DoConvertIntToFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x0010CC54 File Offset: 0x0010AE54
		public override void OnUpdate()
		{
			this.DoConvertIntToFloat();
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x0010CC5C File Offset: 0x0010AE5C
		private void DoConvertIntToFloat()
		{
			this.floatVariable.Value = (float)this.intVariable.Value;
		}

		// Token: 0x04002482 RID: 9346
		[Tooltip("The Integer variable to convert to a float.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x04002483 RID: 9347
		[Tooltip("Store the result in a Float variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002484 RID: 9348
		[Tooltip("Repeat every frame. Useful if the Integer variable is changing.")]
		public bool everyFrame;
	}
}
