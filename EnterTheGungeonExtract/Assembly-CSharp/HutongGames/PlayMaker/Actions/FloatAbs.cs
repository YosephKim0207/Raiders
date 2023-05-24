using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094A RID: 2378
	[Tooltip("Sets a Float variable to its absolute value.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAbs : FsmStateAction
	{
		// Token: 0x060033FE RID: 13310 RVA: 0x0010EE30 File Offset: 0x0010D030
		public override void Reset()
		{
			this.floatVariable = null;
			this.everyFrame = false;
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x0010EE40 File Offset: 0x0010D040
		public override void OnEnter()
		{
			this.DoFloatAbs();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x0010EE5C File Offset: 0x0010D05C
		public override void OnUpdate()
		{
			this.DoFloatAbs();
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x0010EE64 File Offset: 0x0010D064
		private void DoFloatAbs()
		{
			this.floatVariable.Value = Mathf.Abs(this.floatVariable.Value);
		}

		// Token: 0x04002520 RID: 9504
		[Tooltip("The Float variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002521 RID: 9505
		[Tooltip("Repeat every frame. Useful if the Float variable is changing.")]
		public bool everyFrame;
	}
}
