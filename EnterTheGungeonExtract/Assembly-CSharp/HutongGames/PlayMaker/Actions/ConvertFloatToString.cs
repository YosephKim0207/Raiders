using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200091B RID: 2331
	[Tooltip("Converts a Float value to a String value with optional format.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertFloatToString : FsmStateAction
	{
		// Token: 0x0600334C RID: 13132 RVA: 0x0010CB50 File Offset: 0x0010AD50
		public override void Reset()
		{
			this.floatVariable = null;
			this.stringVariable = null;
			this.everyFrame = false;
			this.format = null;
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x0010CB70 File Offset: 0x0010AD70
		public override void OnEnter()
		{
			this.DoConvertFloatToString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x0010CB8C File Offset: 0x0010AD8C
		public override void OnUpdate()
		{
			this.DoConvertFloatToString();
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0010CB94 File Offset: 0x0010AD94
		private void DoConvertFloatToString()
		{
			if (this.format.IsNone || string.IsNullOrEmpty(this.format.Value))
			{
				this.stringVariable.Value = this.floatVariable.Value.ToString();
			}
			else
			{
				this.stringVariable.Value = this.floatVariable.Value.ToString(this.format.Value);
			}
		}

		// Token: 0x0400247E RID: 9342
		[Tooltip("The float variable to convert.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x0400247F RID: 9343
		[Tooltip("A string variable to store the converted value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002480 RID: 9344
		[Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
		public FsmString format;

		// Token: 0x04002481 RID: 9345
		[Tooltip("Repeat every frame. Useful if the float variable is changing.")]
		public bool everyFrame;
	}
}
