using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094E RID: 2382
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Clamps the value of Float Variable to a Min/Max range.")]
	public class FloatClamp : FsmStateAction
	{
		// Token: 0x06003411 RID: 13329 RVA: 0x0010F070 File Offset: 0x0010D270
		public override void Reset()
		{
			this.floatVariable = null;
			this.minValue = null;
			this.maxValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x0010F090 File Offset: 0x0010D290
		public override void OnEnter()
		{
			this.DoClamp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x0010F0AC File Offset: 0x0010D2AC
		public override void OnUpdate()
		{
			this.DoClamp();
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x0010F0B4 File Offset: 0x0010D2B4
		private void DoClamp()
		{
			this.floatVariable.Value = Mathf.Clamp(this.floatVariable.Value, this.minValue.Value, this.maxValue.Value);
		}

		// Token: 0x0400252D RID: 9517
		[Tooltip("Float variable to clamp.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x0400252E RID: 9518
		[RequiredField]
		[Tooltip("The minimum value.")]
		public FsmFloat minValue;

		// Token: 0x0400252F RID: 9519
		[Tooltip("The maximum value.")]
		[RequiredField]
		public FsmFloat maxValue;

		// Token: 0x04002530 RID: 9520
		[Tooltip("Repeate every frame. Useful if the float variable is changing.")]
		public bool everyFrame;
	}
}
