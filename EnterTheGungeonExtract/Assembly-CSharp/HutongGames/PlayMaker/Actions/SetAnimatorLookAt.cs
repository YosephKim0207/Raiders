using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CF RID: 2255
	[Tooltip("Sets look at position and weights. A GameObject can be set to control the look at position, or it can be manually expressed.")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorLookAt : FsmStateAction
	{
		// Token: 0x06003200 RID: 12800 RVA: 0x00107C98 File Offset: 0x00105E98
		public override void Reset()
		{
			this.gameObject = null;
			this.target = null;
			this.targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.weight = 1f;
			this.bodyWeight = 0.3f;
			this.headWeight = 0.6f;
			this.eyesWeight = 1f;
			this.clampWeight = 0.5f;
			this.everyFrame = false;
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x00107D20 File Offset: 0x00105F20
		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorIK = true;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x00107D30 File Offset: 0x00105F30
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
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x00107DAC File Offset: 0x00105FAC
		public override void DoAnimatorIK(int layerIndex)
		{
			this.DoSetLookAt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x00107DC8 File Offset: 0x00105FC8
		private void DoSetLookAt()
		{
			if (this._animator == null)
			{
				return;
			}
			if (this._transform != null)
			{
				if (this.targetPosition.IsNone)
				{
					this._animator.SetLookAtPosition(this._transform.position);
				}
				else
				{
					this._animator.SetLookAtPosition(this._transform.position + this.targetPosition.Value);
				}
			}
			else if (!this.targetPosition.IsNone)
			{
				this._animator.SetLookAtPosition(this.targetPosition.Value);
			}
			if (!this.clampWeight.IsNone)
			{
				this._animator.SetLookAtWeight(this.weight.Value, this.bodyWeight.Value, this.headWeight.Value, this.eyesWeight.Value, this.clampWeight.Value);
			}
			else if (!this.eyesWeight.IsNone)
			{
				this._animator.SetLookAtWeight(this.weight.Value, this.bodyWeight.Value, this.headWeight.Value, this.eyesWeight.Value);
			}
			else if (!this.headWeight.IsNone)
			{
				this._animator.SetLookAtWeight(this.weight.Value, this.bodyWeight.Value, this.headWeight.Value);
			}
			else if (!this.bodyWeight.IsNone)
			{
				this._animator.SetLookAtWeight(this.weight.Value, this.bodyWeight.Value);
			}
			else if (!this.weight.IsNone)
			{
				this._animator.SetLookAtWeight(this.weight.Value);
			}
		}

		// Token: 0x04002325 RID: 8997
		[Tooltip("The target. An Animator component is required.")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002326 RID: 8998
		[Tooltip("The gameObject to look at")]
		public FsmGameObject target;

		// Token: 0x04002327 RID: 8999
		[Tooltip("The lookat position. If Target GameObject set, targetPosition is used as an offset from Target")]
		public FsmVector3 targetPosition;

		// Token: 0x04002328 RID: 9000
		[Tooltip("The global weight of the LookAt, multiplier for other parameters. Range from 0 to 1")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat weight;

		// Token: 0x04002329 RID: 9001
		[Tooltip("determines how much the body is involved in the LookAt. Range from 0 to 1")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat bodyWeight;

		// Token: 0x0400232A RID: 9002
		[Tooltip("determines how much the head is involved in the LookAt. Range from 0 to 1")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat headWeight;

		// Token: 0x0400232B RID: 9003
		[Tooltip("determines how much the eyes are involved in the LookAt. Range from 0 to 1")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat eyesWeight;

		// Token: 0x0400232C RID: 9004
		[Tooltip("0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat clampWeight;

		// Token: 0x0400232D RID: 9005
		[Tooltip("Repeat every frame during OnAnimatorIK(). Useful for changing over time.")]
		public bool everyFrame;

		// Token: 0x0400232E RID: 9006
		private Animator _animator;

		// Token: 0x0400232F RID: 9007
		private Transform _transform;
	}
}
