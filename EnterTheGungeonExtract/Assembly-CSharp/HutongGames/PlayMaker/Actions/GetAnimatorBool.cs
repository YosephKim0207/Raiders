using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A4 RID: 2212
	[Tooltip("Gets the value of a bool parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorBool : FsmStateActionAnimatorBase
	{
		// Token: 0x06003133 RID: 12595 RVA: 0x00104D80 File Offset: 0x00102F80
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.result = null;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x00104DA0 File Offset: 0x00102FA0
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
			this._paramID = Animator.StringToHash(this.parameter.Value);
			this.GetParameter();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x00104E24 File Offset: 0x00103024
		public override void OnActionUpdate()
		{
			this.GetParameter();
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x00104E2C File Offset: 0x0010302C
		private void GetParameter()
		{
			if (this._animator != null)
			{
				this.result.Value = this._animator.GetBool(this._paramID);
			}
		}

		// Token: 0x04002241 RID: 8769
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002242 RID: 8770
		[Tooltip("The animator parameter")]
		[UIHint(UIHint.AnimatorBool)]
		[RequiredField]
		public FsmString parameter;

		// Token: 0x04002243 RID: 8771
		[UIHint(UIHint.Variable)]
		[Tooltip("The bool value of the animator parameter")]
		[ActionSection("Results")]
		[RequiredField]
		public FsmBool result;

		// Token: 0x04002244 RID: 8772
		private Animator _animator;

		// Token: 0x04002245 RID: 8773
		private int _paramID;
	}
}
