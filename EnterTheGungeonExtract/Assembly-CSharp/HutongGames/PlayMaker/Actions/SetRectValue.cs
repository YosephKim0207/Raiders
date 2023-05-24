using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B10 RID: 2832
	[ActionCategory(ActionCategory.Rect)]
	[Tooltip("Sets the value of a Rect Variable.")]
	public class SetRectValue : FsmStateAction
	{
		// Token: 0x06003BB4 RID: 15284 RVA: 0x0012CF80 File Offset: 0x0012B180
		public override void Reset()
		{
			this.rectVariable = null;
			this.rectValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0012CF98 File Offset: 0x0012B198
		public override void OnEnter()
		{
			this.rectVariable.Value = this.rectValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0012CFC4 File Offset: 0x0012B1C4
		public override void OnUpdate()
		{
			this.rectVariable.Value = this.rectValue.Value;
		}

		// Token: 0x04002DD5 RID: 11733
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmRect rectVariable;

		// Token: 0x04002DD6 RID: 11734
		[RequiredField]
		public FsmRect rectValue;

		// Token: 0x04002DD7 RID: 11735
		public bool everyFrame;
	}
}
