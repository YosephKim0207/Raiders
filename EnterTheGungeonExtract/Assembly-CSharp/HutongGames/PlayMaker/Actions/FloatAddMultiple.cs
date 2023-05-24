using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094C RID: 2380
	[Tooltip("Adds multipe float variables to float variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAddMultiple : FsmStateAction
	{
		// Token: 0x06003408 RID: 13320 RVA: 0x0010EF38 File Offset: 0x0010D138
		public override void Reset()
		{
			this.floatVariables = null;
			this.addTo = null;
			this.everyFrame = false;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x0010EF50 File Offset: 0x0010D150
		public override void OnEnter()
		{
			this.DoFloatAdd();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x0010EF6C File Offset: 0x0010D16C
		public override void OnUpdate()
		{
			this.DoFloatAdd();
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x0010EF74 File Offset: 0x0010D174
		private void DoFloatAdd()
		{
			for (int i = 0; i < this.floatVariables.Length; i++)
			{
				this.addTo.Value += this.floatVariables[i].Value;
			}
		}

		// Token: 0x04002526 RID: 9510
		[Tooltip("The float variables to add.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat[] floatVariables;

		// Token: 0x04002527 RID: 9511
		[Tooltip("Add to this variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat addTo;

		// Token: 0x04002528 RID: 9512
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
