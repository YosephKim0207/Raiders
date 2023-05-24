using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B1 RID: 2225
	[Tooltip("Gets the position, rotation and weights of an IK goal. A GameObject can be set to use for the position and rotation")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorIKGoal : FsmStateActionAnimatorBase
	{
		// Token: 0x06003171 RID: 12657 RVA: 0x00105C28 File Offset: 0x00103E28
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.iKGoal = null;
			this.goal = null;
			this.position = null;
			this.rotation = null;
			this.positionWeight = null;
			this.rotationWeight = null;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x00105C64 File Offset: 0x00103E64
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
			this.DoGetIKGoal();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x00105CF8 File Offset: 0x00103EF8
		public override void OnActionUpdate()
		{
			this.DoGetIKGoal();
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x00105D00 File Offset: 0x00103F00
		private void DoGetIKGoal()
		{
			if (this._animator == null)
			{
				return;
			}
			this._iKGoal = (AvatarIKGoal)this.iKGoal.Value;
			if (this._transform != null)
			{
				this._transform.position = this._animator.GetIKPosition(this._iKGoal);
				this._transform.rotation = this._animator.GetIKRotation(this._iKGoal);
			}
			if (!this.position.IsNone)
			{
				this.position.Value = this._animator.GetIKPosition(this._iKGoal);
			}
			if (!this.rotation.IsNone)
			{
				this.rotation.Value = this._animator.GetIKRotation(this._iKGoal);
			}
			if (!this.positionWeight.IsNone)
			{
				this.positionWeight.Value = this._animator.GetIKPositionWeight(this._iKGoal);
			}
			if (!this.rotationWeight.IsNone)
			{
				this.rotationWeight.Value = this._animator.GetIKRotationWeight(this._iKGoal);
			}
		}

		// Token: 0x0400228C RID: 8844
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400228D RID: 8845
		[ObjectType(typeof(AvatarIKGoal))]
		[Tooltip("The IK goal")]
		public FsmEnum iKGoal;

		// Token: 0x0400228E RID: 8846
		[Tooltip("The gameObject to apply ik goal position and rotation to")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmGameObject goal;

		// Token: 0x0400228F RID: 8847
		[Tooltip("Gets The position of the ik goal. If Goal GameObject define, position is used as an offset from Goal")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 position;

		// Token: 0x04002290 RID: 8848
		[UIHint(UIHint.Variable)]
		[Tooltip("Gets The rotation of the ik goal.If Goal GameObject define, rotation is used as an offset from Goal")]
		public FsmQuaternion rotation;

		// Token: 0x04002291 RID: 8849
		[Tooltip("Gets The translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal)")]
		[UIHint(UIHint.Variable)]
		public FsmFloat positionWeight;

		// Token: 0x04002292 RID: 8850
		[Tooltip("Gets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal)")]
		[UIHint(UIHint.Variable)]
		public FsmFloat rotationWeight;

		// Token: 0x04002293 RID: 8851
		private Animator _animator;

		// Token: 0x04002294 RID: 8852
		private Transform _transform;

		// Token: 0x04002295 RID: 8853
		private AvatarIKGoal _iKGoal;
	}
}
