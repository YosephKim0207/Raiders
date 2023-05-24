using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A2 RID: 2210
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the avatar body mass center position and rotation. Optionally accepts a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
	public class GetAnimatorBody : FsmStateActionAnimatorBase
	{
		// Token: 0x0600312A RID: 12586 RVA: 0x00104B6C File Offset: 0x00102D6C
		public override void Reset()
		{
			this.gameObject = null;
			this.bodyPosition = null;
			this.bodyRotation = null;
			this.bodyGameObject = null;
			this.everyFrame = false;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x00104B94 File Offset: 0x00102D94
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

		// Token: 0x0600312C RID: 12588 RVA: 0x00104C28 File Offset: 0x00102E28
		public override void OnActionUpdate()
		{
			this.DoGetBodyPosition();
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x00104C30 File Offset: 0x00102E30
		private void DoGetBodyPosition()
		{
			if (this._animator == null)
			{
				return;
			}
			this.bodyPosition.Value = this._animator.bodyPosition;
			this.bodyRotation.Value = this._animator.bodyRotation;
			if (this._transform != null)
			{
				this._transform.position = this._animator.bodyPosition;
				this._transform.rotation = this._animator.bodyRotation;
			}
		}

		// Token: 0x04002237 RID: 8759
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002238 RID: 8760
		[Tooltip("The avatar body mass center")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmVector3 bodyPosition;

		// Token: 0x04002239 RID: 8761
		[Tooltip("The avatar body mass center")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion bodyRotation;

		// Token: 0x0400223A RID: 8762
		[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
		public FsmGameObject bodyGameObject;

		// Token: 0x0400223B RID: 8763
		private Animator _animator;

		// Token: 0x0400223C RID: 8764
		private Transform _transform;
	}
}
