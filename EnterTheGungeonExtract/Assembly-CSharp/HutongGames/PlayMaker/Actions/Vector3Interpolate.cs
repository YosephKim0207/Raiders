using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B62 RID: 2914
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Interpolates between 2 Vector3 values over a specified Time.")]
	public class Vector3Interpolate : FsmStateAction
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x00131F0C File Offset: 0x0013010C
		public override void Reset()
		{
			this.mode = InterpolationType.Linear;
			this.fromVector = new FsmVector3
			{
				UseVariable = true
			};
			this.toVector = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.storeResult = null;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x00131F70 File Offset: 0x00130170
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

		// Token: 0x06003D14 RID: 15636 RVA: 0x00131FC0 File Offset: 0x001301C0
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
			this.storeResult.Value = Vector3.Lerp(this.fromVector.Value, this.toVector.Value, num);
			if (num >= 1f)
			{
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
				base.Finish();
			}
		}

		// Token: 0x04002F5A RID: 12122
		public InterpolationType mode;

		// Token: 0x04002F5B RID: 12123
		[RequiredField]
		public FsmVector3 fromVector;

		// Token: 0x04002F5C RID: 12124
		[RequiredField]
		public FsmVector3 toVector;

		// Token: 0x04002F5D RID: 12125
		[RequiredField]
		public FsmFloat time;

		// Token: 0x04002F5E RID: 12126
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		// Token: 0x04002F5F RID: 12127
		public FsmEvent finishEvent;

		// Token: 0x04002F60 RID: 12128
		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		// Token: 0x04002F61 RID: 12129
		private float startTime;

		// Token: 0x04002F62 RID: 12130
		private float currentTime;
	}
}
