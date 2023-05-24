using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000917 RID: 2327
	[Tooltip("Converts a Bool value to a String value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToString : FsmStateAction
	{
		// Token: 0x0600333D RID: 13117 RVA: 0x0010C95C File Offset: 0x0010AB5C
		public override void Reset()
		{
			this.boolVariable = null;
			this.stringVariable = null;
			this.falseString = "False";
			this.trueString = "True";
			this.everyFrame = false;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x0010C994 File Offset: 0x0010AB94
		public override void OnEnter()
		{
			this.DoConvertBoolToString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0010C9B0 File Offset: 0x0010ABB0
		public override void OnUpdate()
		{
			this.DoConvertBoolToString();
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x0010C9B8 File Offset: 0x0010ABB8
		private void DoConvertBoolToString()
		{
			this.stringVariable.Value = ((!this.boolVariable.Value) ? this.falseString.Value : this.trueString.Value);
		}

		// Token: 0x0400246E RID: 9326
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x0400246F RID: 9327
		[Tooltip("The String variable to set based on the Bool variable value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002470 RID: 9328
		[Tooltip("String value if Bool variable is false.")]
		public FsmString falseString;

		// Token: 0x04002471 RID: 9329
		[Tooltip("String value if Bool variable is true.")]
		public FsmString trueString;

		// Token: 0x04002472 RID: 9330
		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;
	}
}
