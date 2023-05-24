using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD0 RID: 2768
	[ActionCategory(ActionCategory.Enum)]
	[Tooltip("Sets the value of an Enum Variable.")]
	public class SetEnumValue : FsmStateAction
	{
		// Token: 0x06003A9F RID: 15007 RVA: 0x00129A10 File Offset: 0x00127C10
		public override void Reset()
		{
			this.enumVariable = null;
			this.enumValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00129A28 File Offset: 0x00127C28
		public override void OnEnter()
		{
			this.DoSetEnumValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00129A44 File Offset: 0x00127C44
		public override void OnUpdate()
		{
			this.DoSetEnumValue();
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00129A4C File Offset: 0x00127C4C
		private void DoSetEnumValue()
		{
			this.enumVariable.Value = this.enumValue.Value;
		}

		// Token: 0x04002CC1 RID: 11457
		[Tooltip("The Enum Variable to set.")]
		[UIHint(UIHint.Variable)]
		public FsmEnum enumVariable;

		// Token: 0x04002CC2 RID: 11458
		[Tooltip("The Enum value to set the variable to.")]
		[MatchFieldType("enumVariable")]
		public FsmEnum enumValue;

		// Token: 0x04002CC3 RID: 11459
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
