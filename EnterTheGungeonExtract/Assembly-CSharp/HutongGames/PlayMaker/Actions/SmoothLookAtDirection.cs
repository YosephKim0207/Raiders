using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B21 RID: 2849
	[Tooltip("Smoothly Rotates a Game Object so its forward vector points in the specified Direction. Lets you fire an event when minmagnitude is reached")]
	[ActionCategory(ActionCategory.Transform)]
	public class SmoothLookAtDirection : FsmStateAction
	{
		// Token: 0x06003C05 RID: 15365 RVA: 0x0012E3D0 File Offset: 0x0012C5D0
		public override void Reset()
		{
			this.gameObject = null;
			this.targetDirection = new FsmVector3
			{
				UseVariable = true
			};
			this.minMagnitude = 0.1f;
			this.upVector = new FsmVector3
			{
				UseVariable = true
			};
			this.keepVertical = true;
			this.speed = 5f;
			this.lateUpdate = true;
			this.finishEvent = null;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x0012E448 File Offset: 0x0012C648
		public override void OnEnter()
		{
			this.previousGo = null;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0012E454 File Offset: 0x0012C654
		public override void OnUpdate()
		{
			if (!this.lateUpdate)
			{
				this.DoSmoothLookAtDirection();
			}
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x0012E468 File Offset: 0x0012C668
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoSmoothLookAtDirection();
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0012E47C File Offset: 0x0012C67C
		private void DoSmoothLookAtDirection()
		{
			if (this.targetDirection.IsNone)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.previousGo != ownerDefaultTarget)
			{
				this.lastRotation = ownerDefaultTarget.transform.rotation;
				this.desiredRotation = this.lastRotation;
				this.previousGo = ownerDefaultTarget;
			}
			Vector3 value = this.targetDirection.Value;
			if (this.keepVertical.Value)
			{
				value.y = 0f;
			}
			bool flag = false;
			if (value.sqrMagnitude > this.minMagnitude.Value)
			{
				this.desiredRotation = Quaternion.LookRotation(value, (!this.upVector.IsNone) ? this.upVector.Value : Vector3.up);
			}
			else
			{
				flag = true;
			}
			this.lastRotation = Quaternion.Slerp(this.lastRotation, this.desiredRotation, this.speed.Value * Time.deltaTime);
			ownerDefaultTarget.transform.rotation = this.lastRotation;
			if (flag)
			{
				base.Fsm.Event(this.finishEvent);
				if (this.finish.Value)
				{
					base.Finish();
				}
			}
		}

		// Token: 0x04002E2E RID: 11822
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E2F RID: 11823
		[Tooltip("The direction to smoothly rotate towards.")]
		[RequiredField]
		public FsmVector3 targetDirection;

		// Token: 0x04002E30 RID: 11824
		[Tooltip("Only rotate if Target Direction Vector length is greater than this threshold.")]
		public FsmFloat minMagnitude;

		// Token: 0x04002E31 RID: 11825
		[Tooltip("Keep this vector pointing up as the GameObject rotates.")]
		public FsmVector3 upVector;

		// Token: 0x04002E32 RID: 11826
		[Tooltip("Eliminate any tilt up/down as the GameObject rotates.")]
		[RequiredField]
		public FsmBool keepVertical;

		// Token: 0x04002E33 RID: 11827
		[Tooltip("How quickly to rotate.")]
		[HasFloatSlider(0.5f, 15f)]
		[RequiredField]
		public FsmFloat speed;

		// Token: 0x04002E34 RID: 11828
		[Tooltip("Perform in LateUpdate. This can help eliminate jitters in some situations.")]
		public bool lateUpdate;

		// Token: 0x04002E35 RID: 11829
		[Tooltip("Event to send if the direction difference is less than Min Magnitude.")]
		public FsmEvent finishEvent;

		// Token: 0x04002E36 RID: 11830
		[Tooltip("Stop running the action if the direction difference is less than Min Magnitude.")]
		public FsmBool finish;

		// Token: 0x04002E37 RID: 11831
		private GameObject previousGo;

		// Token: 0x04002E38 RID: 11832
		private Quaternion lastRotation;

		// Token: 0x04002E39 RID: 11833
		private Quaternion desiredRotation;
	}
}
