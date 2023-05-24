using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FF RID: 2303
	[Tooltip("Flips the value of a Bool Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class BoolFlip : FsmStateAction
	{
		// Token: 0x060032BF RID: 12991 RVA: 0x0010A6AC File Offset: 0x001088AC
		public override void Reset()
		{
			this.boolVariable = null;
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x0010A6B8 File Offset: 0x001088B8
		public override void OnEnter()
		{
			this.boolVariable.Value = !this.boolVariable.Value;
			base.Finish();
		}

		// Token: 0x040023E7 RID: 9191
		[Tooltip("Bool variable to flip.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;
	}
}
