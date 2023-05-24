using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CB RID: 2251
	[Tooltip("Sets the position, rotation and weights of an IK goal. A GameObject can be set to control the position and rotation, or it can be manually expressed.")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorIKGoal : FsmStateAction
	{
		// Token: 0x060031EC RID: 12780 RVA: 0x00107778 File Offset: 0x00105978
		public override void Reset()
		{
			this.gameObject = null;
			this.goal = null;
			this.position = new FsmVector3
			{
				UseVariable = true
			};
			this.rotation = new FsmQuaternion
			{
				UseVariable = true
			};
			this.positionWeight = 1f;
			this.rotationWeight = 1f;
			this.everyFrame = false;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x001077E4 File Offset: 0x001059E4
		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorIK = true;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x001077F4 File Offset: 0x001059F4
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
			GameObject value = this.goal.Value;
			if (value != null)
			{
				this._transform = value.transform;
			}
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x00107870 File Offset: 0x00105A70
		public override void DoAnimatorIK(int layerIndex)
		{
			this.DoSetIKGoal();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x0010788C File Offset: 0x00105A8C
		private void DoSetIKGoal()
		{
			if (this._animator == null)
			{
				return;
			}
			if (this._transform != null)
			{
				if (this.position.IsNone)
				{
					this._animator.SetIKPosition(this.iKGoal, this._transform.position);
				}
				else
				{
					this._animator.SetIKPosition(this.iKGoal, this._transform.position + this.position.Value);
				}
				if (this.rotation.IsNone)
				{
					this._animator.SetIKRotation(this.iKGoal, this._transform.rotation);
				}
				else
				{
					this._animator.SetIKRotation(this.iKGoal, this._transform.rotation * this.rotation.Value);
				}
			}
			else
			{
				if (!this.position.IsNone)
				{
					this._animator.SetIKPosition(this.iKGoal, this.position.Value);
				}
				if (!this.rotation.IsNone)
				{
					this._animator.SetIKRotation(this.iKGoal, this.rotation.Value);
				}
			}
			if (!this.positionWeight.IsNone)
			{
				this._animator.SetIKPositionWeight(this.iKGoal, this.positionWeight.Value);
			}
			if (!this.rotationWeight.IsNone)
			{
				this._animator.SetIKRotationWeight(this.iKGoal, this.rotationWeight.Value);
			}
		}

		// Token: 0x0400230E RID: 8974
		[Tooltip("The target.")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400230F RID: 8975
		[Tooltip("The IK goal")]
		public AvatarIKGoal iKGoal;

		// Token: 0x04002310 RID: 8976
		[Tooltip("The gameObject target of the ik goal")]
		public FsmGameObject goal;

		// Token: 0x04002311 RID: 8977
		[Tooltip("The position of the ik goal. If Goal GameObject set, position is used as an offset from Goal")]
		public FsmVector3 position;

		// Token: 0x04002312 RID: 8978
		[Tooltip("The rotation of the ik goal.If Goal GameObject set, rotation is used as an offset from Goal")]
		public FsmQuaternion rotation;

		// Token: 0x04002313 RID: 8979
		[Tooltip("The translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal)")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat positionWeight;

		// Token: 0x04002314 RID: 8980
		[Tooltip("Sets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal)")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat rotationWeight;

		// Token: 0x04002315 RID: 8981
		[Tooltip("Repeat every frame. Useful when changing over time.")]
		public bool everyFrame;

		// Token: 0x04002316 RID: 8982
		private Animator _animator;

		// Token: 0x04002317 RID: 8983
		private Transform _transform;
	}
}
