using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000955 RID: 2389
	[Tooltip("Sends Events based on the sign of a Float.")]
	[ActionCategory(ActionCategory.Logic)]
	public class FloatSignTest : FsmStateAction
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x0010F5E4 File Offset: 0x0010D7E4
		public override void Reset()
		{
			this.floatValue = 0f;
			this.isPositive = null;
			this.isNegative = null;
			this.everyFrame = false;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x0010F60C File Offset: 0x0010D80C
		public override void OnEnter()
		{
			this.DoSignTest();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0010F628 File Offset: 0x0010D828
		public override void OnUpdate()
		{
			this.DoSignTest();
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x0010F630 File Offset: 0x0010D830
		private void DoSignTest()
		{
			if (this.floatValue == null)
			{
				return;
			}
			base.Fsm.Event((this.floatValue.Value >= 0f) ? this.isPositive : this.isNegative);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x0010F670 File Offset: 0x0010D870
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.isPositive) && FsmEvent.IsNullOrEmpty(this.isNegative))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x04002553 RID: 9555
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to test.")]
		[RequiredField]
		public FsmFloat floatValue;

		// Token: 0x04002554 RID: 9556
		[Tooltip("Event to send if the float variable is positive.")]
		public FsmEvent isPositive;

		// Token: 0x04002555 RID: 9557
		[Tooltip("Event to send if the float variable is negative.")]
		public FsmEvent isNegative;

		// Token: 0x04002556 RID: 9558
		[Tooltip("Repeat every frame. Useful if the variable is changing and you're waiting for a particular result.")]
		public bool everyFrame;
	}
}
