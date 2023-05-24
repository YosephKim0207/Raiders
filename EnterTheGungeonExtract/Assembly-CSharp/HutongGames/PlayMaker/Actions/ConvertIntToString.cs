using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200091D RID: 2333
	[Tooltip("Converts an Integer value to a String value with an optional format.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertIntToString : FsmStateAction
	{
		// Token: 0x06003356 RID: 13142 RVA: 0x0010CC80 File Offset: 0x0010AE80
		public override void Reset()
		{
			this.intVariable = null;
			this.stringVariable = null;
			this.everyFrame = false;
			this.format = null;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x0010CCA0 File Offset: 0x0010AEA0
		public override void OnEnter()
		{
			this.DoConvertIntToString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x0010CCBC File Offset: 0x0010AEBC
		public override void OnUpdate()
		{
			this.DoConvertIntToString();
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x0010CCC4 File Offset: 0x0010AEC4
		private void DoConvertIntToString()
		{
			if (this.format.IsNone || string.IsNullOrEmpty(this.format.Value))
			{
				this.stringVariable.Value = this.intVariable.Value.ToString();
			}
			else
			{
				this.stringVariable.Value = this.intVariable.Value.ToString(this.format.Value);
			}
		}

		// Token: 0x04002485 RID: 9349
		[Tooltip("The Int variable to convert.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x04002486 RID: 9350
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("A String variable to store the converted value.")]
		public FsmString stringVariable;

		// Token: 0x04002487 RID: 9351
		[Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
		public FsmString format;

		// Token: 0x04002488 RID: 9352
		[Tooltip("Repeat every frame. Useful if the Int variable is changing.")]
		public bool everyFrame;
	}
}
