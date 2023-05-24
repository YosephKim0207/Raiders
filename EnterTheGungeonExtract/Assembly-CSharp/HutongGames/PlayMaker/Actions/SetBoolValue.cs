using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACA RID: 2762
	[Tooltip("Sets the value of a Bool Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class SetBoolValue : FsmStateAction
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x00129644 File Offset: 0x00127844
		public override void Reset()
		{
			this.boolVariable = null;
			this.boolValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0012965C File Offset: 0x0012785C
		public override void OnEnter()
		{
			this.boolVariable.Value = this.boolValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00129688 File Offset: 0x00127888
		public override void OnUpdate()
		{
			this.boolVariable.Value = this.boolValue.Value;
		}

		// Token: 0x04002CAB RID: 11435
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x04002CAC RID: 11436
		[RequiredField]
		public FsmBool boolValue;

		// Token: 0x04002CAD RID: 11437
		public bool everyFrame;
	}
}
