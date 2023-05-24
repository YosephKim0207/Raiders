using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000899 RID: 2201
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress")]
	public class AnimatorMatchTarget : FsmStateAction
	{
		// Token: 0x06003109 RID: 12553 RVA: 0x001044A0 File Offset: 0x001026A0
		public override void Reset()
		{
			this.gameObject = null;
			this.bodyPart = AvatarTarget.Root;
			this.target = null;
			this.targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.targetRotation = new FsmQuaternion
			{
				UseVariable = true
			};
			this.positionWeight = Vector3.one;
			this.rotationWeight = 0f;
			this.startNormalizedTime = null;
			this.targetNormalizedTime = null;
			this.everyFrame = true;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00104520 File Offset: 0x00102720
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator == null)
			{
				base.Finish();
				return;
			}
			GameObject value = this.target.Value;
			if (value != null)
			{
				this._transform = value.transform;
			}
			this.DoMatchTarget();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x001045B4 File Offset: 0x001027B4
		public override void OnUpdate()
		{
			this.DoMatchTarget();
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x001045BC File Offset: 0x001027BC
		private void DoMatchTarget()
		{
			if (this._animator == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			if (this._transform != null)
			{
				vector = this._transform.position;
				quaternion = this._transform.rotation;
			}
			if (!this.targetPosition.IsNone)
			{
				vector += this.targetPosition.Value;
			}
			if (!this.targetRotation.IsNone)
			{
				quaternion *= this.targetRotation.Value;
			}
			MatchTargetWeightMask matchTargetWeightMask = new MatchTargetWeightMask(this.positionWeight.Value, this.rotationWeight.Value);
			this._animator.MatchTarget(vector, quaternion, this.bodyPart, matchTargetWeightMask, this.startNormalizedTime.Value, this.targetNormalizedTime.Value);
		}

		// Token: 0x04002212 RID: 8722
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002213 RID: 8723
		[Tooltip("The body part that is involved in the match")]
		public AvatarTarget bodyPart;

		// Token: 0x04002214 RID: 8724
		[Tooltip("The gameObject target to match")]
		public FsmGameObject target;

		// Token: 0x04002215 RID: 8725
		[Tooltip("The position of the ik goal. If Goal GameObject set, position is used as an offset from Goal")]
		public FsmVector3 targetPosition;

		// Token: 0x04002216 RID: 8726
		[Tooltip("The rotation of the ik goal.If Goal GameObject set, rotation is used as an offset from Goal")]
		public FsmQuaternion targetRotation;

		// Token: 0x04002217 RID: 8727
		[Tooltip("The MatchTargetWeightMask Position XYZ weight")]
		public FsmVector3 positionWeight;

		// Token: 0x04002218 RID: 8728
		[Tooltip("The MatchTargetWeightMask Rotation weight")]
		public FsmFloat rotationWeight;

		// Token: 0x04002219 RID: 8729
		[Tooltip("Start time within the animation clip (0 - beginning of clip, 1 - end of clip)")]
		public FsmFloat startNormalizedTime;

		// Token: 0x0400221A RID: 8730
		[Tooltip("End time within the animation clip (0 - beginning of clip, 1 - end of clip), values greater than 1 can be set to trigger a match after a certain number of loops. Ex: 2.3 means at 30% of 2nd loop")]
		public FsmFloat targetNormalizedTime;

		// Token: 0x0400221B RID: 8731
		[Tooltip("Should always be true")]
		public bool everyFrame;

		// Token: 0x0400221C RID: 8732
		private Animator _animator;

		// Token: 0x0400221D RID: 8733
		private Transform _transform;
	}
}
