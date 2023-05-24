using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000918 RID: 2328
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an Enum value to a String value.")]
	public class ConvertEnumToString : FsmStateAction
	{
		// Token: 0x06003342 RID: 13122 RVA: 0x0010C9F8 File Offset: 0x0010ABF8
		public override void Reset()
		{
			this.enumVariable = null;
			this.stringVariable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x0010CA10 File Offset: 0x0010AC10
		public override void OnEnter()
		{
			this.DoConvertEnumToString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0010CA2C File Offset: 0x0010AC2C
		public override void OnUpdate()
		{
			this.DoConvertEnumToString();
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0010CA34 File Offset: 0x0010AC34
		private void DoConvertEnumToString()
		{
			this.stringVariable.Value = ((this.enumVariable.Value == null) ? string.Empty : this.enumVariable.Value.ToString());
		}

		// Token: 0x04002473 RID: 9331
		[Tooltip("The Enum variable to convert.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmEnum enumVariable;

		// Token: 0x04002474 RID: 9332
		[Tooltip("The String variable to store the converted value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002475 RID: 9333
		[Tooltip("Repeat every frame. Useful if the Enum variable is changing.")]
		public bool everyFrame;
	}
}
