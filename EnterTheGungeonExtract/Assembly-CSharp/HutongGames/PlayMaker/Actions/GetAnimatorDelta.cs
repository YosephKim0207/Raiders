using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AC RID: 2220
	[Tooltip("Gets the avatar delta position and rotation for the last evaluated frame.")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorDelta : FsmStateActionAnimatorBase
	{
		// Token: 0x0600315A RID: 12634 RVA: 0x00105854 File Offset: 0x00103A54
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.deltaPosition = null;
			this.deltaRotation = null;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x00105874 File Offset: 0x00103A74
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
			this.DoGetDeltaPosition();
			base.Finish();
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001058D8 File Offset: 0x00103AD8
		public override void OnActionUpdate()
		{
			this.DoGetDeltaPosition();
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x001058E0 File Offset: 0x00103AE0
		private void DoGetDeltaPosition()
		{
			if (this._animator == null)
			{
				return;
			}
			this.deltaPosition.Value = this._animator.deltaPosition;
			this.deltaRotation.Value = this._animator.deltaRotation;
		}

		// Token: 0x04002279 RID: 8825
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400227A RID: 8826
		[Tooltip("The avatar delta position for the last evaluated frame")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 deltaPosition;

		// Token: 0x0400227B RID: 8827
		[Tooltip("The avatar delta position for the last evaluated frame")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion deltaRotation;

		// Token: 0x0400227C RID: 8828
		private Transform _transform;

		// Token: 0x0400227D RID: 8829
		private Animator _animator;
	}
}
