using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF4 RID: 2804
	[Tooltip("Sets the value of an integer variable using a float value.")]
	[ActionCategory(ActionCategory.Math)]
	public class SetIntFromFloat : FsmStateAction
	{
		// Token: 0x06003B3F RID: 15167 RVA: 0x0012B9FC File Offset: 0x00129BFC
		public override void Reset()
		{
			this.intVariable = null;
			this.floatValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0012BA14 File Offset: 0x00129C14
		public override void OnEnter()
		{
			this.intVariable.Value = (int)this.floatValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x0012BA40 File Offset: 0x00129C40
		public override void OnUpdate()
		{
			this.intVariable.Value = (int)this.floatValue.Value;
		}

		// Token: 0x04002D78 RID: 11640
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		// Token: 0x04002D79 RID: 11641
		public FsmFloat floatValue;

		// Token: 0x04002D7A RID: 11642
		public bool everyFrame;
	}
}
