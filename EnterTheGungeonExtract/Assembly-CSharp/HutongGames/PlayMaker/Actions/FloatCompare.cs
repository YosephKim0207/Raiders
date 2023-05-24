using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094F RID: 2383
	[Tooltip("Sends Events based on the comparison of 2 Floats.")]
	[ActionCategory(ActionCategory.Logic)]
	public class FloatCompare : FsmStateAction
	{
		// Token: 0x06003416 RID: 13334 RVA: 0x0010F0F0 File Offset: 0x0010D2F0
		public override void Reset()
		{
			this.float1 = 0f;
			this.float2 = 0f;
			this.tolerance = 0f;
			this.equal = null;
			this.lessThan = null;
			this.greaterThan = null;
			this.everyFrame = false;
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x0010F14C File Offset: 0x0010D34C
		public override void OnEnter()
		{
			this.DoCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x0010F168 File Offset: 0x0010D368
		public override void OnUpdate()
		{
			this.DoCompare();
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x0010F170 File Offset: 0x0010D370
		private void DoCompare()
		{
			if (Mathf.Abs(this.float1.Value - this.float2.Value) <= this.tolerance.Value)
			{
				base.Fsm.Event(this.equal);
				return;
			}
			if (this.float1.Value < this.float2.Value)
			{
				base.Fsm.Event(this.lessThan);
				return;
			}
			if (this.float1.Value > this.float2.Value)
			{
				base.Fsm.Event(this.greaterThan);
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x0010F214 File Offset: 0x0010D414
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.equal) && FsmEvent.IsNullOrEmpty(this.lessThan) && FsmEvent.IsNullOrEmpty(this.greaterThan))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x04002531 RID: 9521
		[Tooltip("The first float variable.")]
		[RequiredField]
		public FsmFloat float1;

		// Token: 0x04002532 RID: 9522
		[Tooltip("The second float variable.")]
		[RequiredField]
		public FsmFloat float2;

		// Token: 0x04002533 RID: 9523
		[Tooltip("Tolerance for the Equal test (almost equal).\nNOTE: Floats that look the same are often not exactly the same, so you often need to use a small tolerance.")]
		[RequiredField]
		public FsmFloat tolerance;

		// Token: 0x04002534 RID: 9524
		[Tooltip("Event sent if Float 1 equals Float 2 (within Tolerance)")]
		public FsmEvent equal;

		// Token: 0x04002535 RID: 9525
		[Tooltip("Event sent if Float 1 is less than Float 2")]
		public FsmEvent lessThan;

		// Token: 0x04002536 RID: 9526
		[Tooltip("Event sent if Float 1 is greater than Float 2")]
		public FsmEvent greaterThan;

		// Token: 0x04002537 RID: 9527
		[Tooltip("Repeat every frame. Useful if the variables are changing and you're waiting for a particular result.")]
		public bool everyFrame;
	}
}
