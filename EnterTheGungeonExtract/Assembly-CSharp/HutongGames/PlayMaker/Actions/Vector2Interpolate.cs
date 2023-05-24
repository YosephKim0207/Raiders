using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B52 RID: 2898
	[Tooltip("Interpolates between 2 Vector2 values over a specified Time.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Interpolate : FsmStateAction
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x001310E4 File Offset: 0x0012F2E4
		public override void Reset()
		{
			this.mode = InterpolationType.Linear;
			this.fromVector = new FsmVector2
			{
				UseVariable = true
			};
			this.toVector = new FsmVector2
			{
				UseVariable = true
			};
			this.time = 1f;
			this.storeResult = null;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00131148 File Offset: 0x0012F348
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
				this.storeResult.Value = this.fromVector.Value;
			}
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00131198 File Offset: 0x0012F398
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
					num = Mathf.SmoothStep(0f, 1f, num);
				}
			}
			this.storeResult.Value = Vector2.Lerp(this.fromVector.Value, this.toVector.Value, num);
			if (num > 1f)
			{
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
				base.Finish();
			}
		}

		// Token: 0x04002F13 RID: 12051
		[Tooltip("The interpolation type")]
		public InterpolationType mode;

		// Token: 0x04002F14 RID: 12052
		[Tooltip("The vector to interpolate from")]
		[RequiredField]
		public FsmVector2 fromVector;

		// Token: 0x04002F15 RID: 12053
		[RequiredField]
		[Tooltip("The vector to interpolate to")]
		public FsmVector2 toVector;

		// Token: 0x04002F16 RID: 12054
		[Tooltip("the interpolate time")]
		[RequiredField]
		public FsmFloat time;

		// Token: 0x04002F17 RID: 12055
		[RequiredField]
		[Tooltip("the interpolated result")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeResult;

		// Token: 0x04002F18 RID: 12056
		[Tooltip("This event is fired when the interpolation is done.")]
		public FsmEvent finishEvent;

		// Token: 0x04002F19 RID: 12057
		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		// Token: 0x04002F1A RID: 12058
		private float startTime;

		// Token: 0x04002F1B RID: 12059
		private float currentTime;
	}
}
