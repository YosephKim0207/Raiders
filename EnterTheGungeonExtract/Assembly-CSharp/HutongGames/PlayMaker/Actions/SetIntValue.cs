using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF5 RID: 2805
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of an Integer Variable.")]
	public class SetIntValue : FsmStateAction
	{
		// Token: 0x06003B43 RID: 15171 RVA: 0x0012BA64 File Offset: 0x00129C64
		public override void Reset()
		{
			this.intVariable = null;
			this.intValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0012BA7C File Offset: 0x00129C7C
		public override void OnEnter()
		{
			this.intVariable.Value = this.intValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0012BAA8 File Offset: 0x00129CA8
		public override void OnUpdate()
		{
			this.intVariable.Value = this.intValue.Value;
		}

		// Token: 0x04002D7B RID: 11643
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		// Token: 0x04002D7C RID: 11644
		[RequiredField]
		public FsmInt intValue;

		// Token: 0x04002D7D RID: 11645
		public bool everyFrame;
	}
}
