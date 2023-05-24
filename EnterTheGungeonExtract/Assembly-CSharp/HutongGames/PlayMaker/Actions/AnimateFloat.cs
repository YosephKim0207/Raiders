using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000882 RID: 2178
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Float Variable using an Animation Curve.")]
	public class AnimateFloat : FsmStateAction
	{
		// Token: 0x06003088 RID: 12424 RVA: 0x000FF498 File Offset: 0x000FD698
		public override void Reset()
		{
			this.animCurve = null;
			this.floatVariable = null;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000FF4B8 File Offset: 0x000FD6B8
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.currentTime = 0f;
			if (this.animCurve != null && this.animCurve.curve != null && this.animCurve.curve.keys.Length > 0)
			{
				this.endTime = this.animCurve.curve.keys[this.animCurve.curve.length - 1].time;
				this.looping = ActionHelpers.IsLoopingWrapMode(this.animCurve.curve.postWrapMode);
				this.floatVariable.Value = this.animCurve.curve.Evaluate(0f);
				return;
			}
			base.Finish();
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000FF588 File Offset: 0x000FD788
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
			if (this.animCurve != null && this.animCurve.curve != null && this.floatVariable != null)
			{
				this.floatVariable.Value = this.animCurve.curve.Evaluate(this.currentTime);
			}
			if (this.currentTime >= this.endTime)
			{
				if (!this.looping)
				{
					base.Finish();
				}
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
		}

		// Token: 0x0400212D RID: 8493
		[Tooltip("The animation curve to use.")]
		[RequiredField]
		public FsmAnimationCurve animCurve;

		// Token: 0x0400212E RID: 8494
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to set.")]
		public FsmFloat floatVariable;

		// Token: 0x0400212F RID: 8495
		[Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;

		// Token: 0x04002130 RID: 8496
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x04002131 RID: 8497
		private float startTime;

		// Token: 0x04002132 RID: 8498
		private float currentTime;

		// Token: 0x04002133 RID: 8499
		private float endTime;

		// Token: 0x04002134 RID: 8500
		private bool looping;
	}
}
