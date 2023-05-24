using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C3 RID: 2243
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the position and rotation of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime)).\nThe position and rotation are only valid when a frame has being evaluated after the SetTarget call")]
	public class GetAnimatorTarget : FsmStateActionAnimatorBase
	{
		// Token: 0x060031C6 RID: 12742 RVA: 0x00106EF8 File Offset: 0x001050F8
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.targetPosition = null;
			this.targetRotation = null;
			this.targetGameObject = null;
			this.everyFrame = false;
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x00106F24 File Offset: 0x00105124
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
			GameObject value = this.targetGameObject.Value;
			if (value != null)
			{
				this._transform = value.transform;
			}
			this.DoGetTarget();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x00106FB8 File Offset: 0x001051B8
		public override void OnActionUpdate()
		{
			this.DoGetTarget();
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x00106FC0 File Offset: 0x001051C0
		private void DoGetTarget()
		{
			if (this._animator == null)
			{
				return;
			}
			this.targetPosition.Value = this._animator.targetPosition;
			this.targetRotation.Value = this._animator.targetRotation;
			if (this._transform != null)
			{
				this._transform.position = this._animator.targetPosition;
				this._transform.rotation = this._animator.targetRotation;
			}
		}

		// Token: 0x040022E9 RID: 8937
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022EA RID: 8938
		[Tooltip("The target position")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 targetPosition;

		// Token: 0x040022EB RID: 8939
		[UIHint(UIHint.Variable)]
		[Tooltip("The target rotation")]
		public FsmQuaternion targetRotation;

		// Token: 0x040022EC RID: 8940
		[Tooltip("If set, apply the position and rotation to this gameObject")]
		public FsmGameObject targetGameObject;

		// Token: 0x040022ED RID: 8941
		private Animator _animator;

		// Token: 0x040022EE RID: 8942
		private Transform _transform;
	}
}
