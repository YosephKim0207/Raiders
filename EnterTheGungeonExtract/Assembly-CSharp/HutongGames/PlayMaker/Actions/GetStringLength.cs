using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B0 RID: 2480
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets the Length of a String.")]
	public class GetStringLength : FsmStateAction
	{
		// Token: 0x060035B8 RID: 13752 RVA: 0x00113EE8 File Offset: 0x001120E8
		public override void Reset()
		{
			this.stringVariable = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x00113F00 File Offset: 0x00112100
		public override void OnEnter()
		{
			this.DoGetStringLength();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x00113F1C File Offset: 0x0011211C
		public override void OnUpdate()
		{
			this.DoGetStringLength();
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x00113F24 File Offset: 0x00112124
		private void DoGetStringLength()
		{
			if (this.stringVariable == null)
			{
				return;
			}
			if (this.storeResult == null)
			{
				return;
			}
			this.storeResult.Value = this.stringVariable.Value.Length;
		}

		// Token: 0x04002701 RID: 9985
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		// Token: 0x04002702 RID: 9986
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;

		// Token: 0x04002703 RID: 9987
		public bool everyFrame;
	}
}
