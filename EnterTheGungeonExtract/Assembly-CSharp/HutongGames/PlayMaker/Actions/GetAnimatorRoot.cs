using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C1 RID: 2241
	[Tooltip("Gets the avatar body mass center position and rotation.Optionally accept a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorRoot : FsmStateActionAnimatorBase
	{
		// Token: 0x060031BC RID: 12732 RVA: 0x00106CDC File Offset: 0x00104EDC
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.rootPosition = null;
			this.rootRotation = null;
			this.bodyGameObject = null;
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x00106D00 File Offset: 0x00104F00
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
			GameObject value = this.bodyGameObject.Value;
			if (value != null)
			{
				this._transform = value.transform;
			}
			this.DoGetBodyPosition();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x00106D94 File Offset: 0x00104F94
		public override void OnActionUpdate()
		{
			this.DoGetBodyPosition();
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x00106D9C File Offset: 0x00104F9C
		private void DoGetBodyPosition()
		{
			if (this._animator == null)
			{
				return;
			}
			this.rootPosition.Value = this._animator.rootPosition;
			this.rootRotation.Value = this._animator.rootRotation;
			if (this._transform != null)
			{
				this._transform.position = this._animator.rootPosition;
				this._transform.rotation = this._animator.rootRotation;
			}
		}

		// Token: 0x040022E0 RID: 8928
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022E1 RID: 8929
		[UIHint(UIHint.Variable)]
		[Tooltip("The avatar body mass center")]
		[ActionSection("Results")]
		public FsmVector3 rootPosition;

		// Token: 0x040022E2 RID: 8930
		[Tooltip("The avatar body mass center")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion rootRotation;

		// Token: 0x040022E3 RID: 8931
		[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
		public FsmGameObject bodyGameObject;

		// Token: 0x040022E4 RID: 8932
		private Animator _animator;

		// Token: 0x040022E5 RID: 8933
		private Transform _transform;
	}
}
