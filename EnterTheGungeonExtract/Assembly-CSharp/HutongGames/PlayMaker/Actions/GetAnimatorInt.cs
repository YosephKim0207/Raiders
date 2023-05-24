using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B2 RID: 2226
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the value of an int parameter")]
	public class GetAnimatorInt : FsmStateActionAnimatorBase
	{
		// Token: 0x06003176 RID: 12662 RVA: 0x00105E38 File Offset: 0x00104038
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.result = null;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x00105E58 File Offset: 0x00104058
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

		// Token: 0x06003178 RID: 12664 RVA: 0x00105EDC File Offset: 0x001040DC
		public override void OnActionUpdate()
		{
			this.GetParameter();
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x00105EE4 File Offset: 0x001040E4
		private void GetParameter()
		{
			if (this._animator != null)
			{
				this.result.Value = this._animator.GetInteger(this._paramID);
			}
		}

		// Token: 0x04002296 RID: 8854
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002297 RID: 8855
		[RequiredField]
		[Tooltip("The animator parameter")]
		[UIHint(UIHint.AnimatorInt)]
		public FsmString parameter;

		// Token: 0x04002298 RID: 8856
		[ActionSection("Results")]
		[Tooltip("The int value of the animator parameter")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt result;

		// Token: 0x04002299 RID: 8857
		private Animator _animator;

		// Token: 0x0400229A RID: 8858
		private int _paramID;
	}
}
