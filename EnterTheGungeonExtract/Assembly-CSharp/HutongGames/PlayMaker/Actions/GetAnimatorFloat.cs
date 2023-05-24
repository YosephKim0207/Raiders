using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AE RID: 2222
	[Tooltip("Gets the value of a float parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorFloat : FsmStateActionAnimatorBase
	{
		// Token: 0x06003163 RID: 12643 RVA: 0x001059D0 File Offset: 0x00103BD0
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.result = null;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x001059F0 File Offset: 0x00103BF0
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

		// Token: 0x06003165 RID: 12645 RVA: 0x00105A74 File Offset: 0x00103C74
		public override void OnActionUpdate()
		{
			this.GetParameter();
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x00105A7C File Offset: 0x00103C7C
		private void GetParameter()
		{
			if (this._animator != null)
			{
				this.result.Value = this._animator.GetFloat(this._paramID);
			}
		}

		// Token: 0x04002281 RID: 8833
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002282 RID: 8834
		[Tooltip("The animator parameter")]
		[UIHint(UIHint.AnimatorFloat)]
		[RequiredField]
		public FsmString parameter;

		// Token: 0x04002283 RID: 8835
		[UIHint(UIHint.Variable)]
		[Tooltip("The float value of the animator parameter")]
		[ActionSection("Results")]
		[RequiredField]
		public FsmFloat result;

		// Token: 0x04002284 RID: 8836
		private Animator _animator;

		// Token: 0x04002285 RID: 8837
		private int _paramID;
	}
}
