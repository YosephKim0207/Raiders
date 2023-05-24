using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000951 RID: 2385
	[Tooltip("Interpolates between 2 Float values over a specified Time.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatInterpolate : FsmStateAction
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x0010F2CC File Offset: 0x0010D4CC
		public override void Reset()
		{
			this.mode = InterpolationType.Linear;
			this.fromFloat = null;
			this.toFloat = null;
			this.time = 1f;
			this.storeResult = null;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x0010F308 File Offset: 0x0010D508
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.currentTime = 0f;
			if (this.storeResult == null)
			{
				base.Finish();
			}
			else
			{
				this.storeResult.Value = this.fromFloat.Value;
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0010F358 File Offset: 0x0010D558
		public override void OnUpdate()
		{
			if (this.realTime)
			{
				this.currentTime = FsmTime.RealtimeSinceStartup - this.startTime;
			}
			else
			{
				this.currentTime += Time.deltaTime;
			}
			float num = this.currentTime / this.time.Value;
			InterpolationType interpolationType = this.mode;
			if (interpolationType != InterpolationType.Linear)
			{
				if (interpolationType == InterpolationType.EaseInOut)
				{
					this.storeResult.Value = Mathf.SmoothStep(this.fromFloat.Value, this.toFloat.Value, num);
				}
			}
			else
			{
				this.storeResult.Value = Mathf.Lerp(this.fromFloat.Value, this.toFloat.Value, num);
			}
			if (num > 1f)
			{
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
				base.Finish();
			}
		}

		// Token: 0x0400253B RID: 9531
		[Tooltip("Interpolation mode: Linear or EaseInOut.")]
		public InterpolationType mode;

		// Token: 0x0400253C RID: 9532
		[RequiredField]
		[Tooltip("Interpolate from this value.")]
		public FsmFloat fromFloat;

		// Token: 0x0400253D RID: 9533
		[Tooltip("Interpolate to this value.")]
		[RequiredField]
		public FsmFloat toFloat;

		// Token: 0x0400253E RID: 9534
		[Tooltip("Interpolate over this amount of time in seconds.")]
		[RequiredField]
		public FsmFloat time;

		// Token: 0x0400253F RID: 9535
		[RequiredField]
		[Tooltip("Store the current value in a float variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		// Token: 0x04002540 RID: 9536
		[Tooltip("Event to send when the interpolation is finished.")]
		public FsmEvent finishEvent;

		// Token: 0x04002541 RID: 9537
		[Tooltip("Ignore TimeScale. Useful if the game is paused (Time scaled to 0).")]
		public bool realTime;

		// Token: 0x04002542 RID: 9538
		private float startTime;

		// Token: 0x04002543 RID: 9539
		private float currentTime;
	}
}
