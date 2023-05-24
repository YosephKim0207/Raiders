using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CC RID: 2252
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the value of a int parameter")]
	public class SetAnimatorInt : FsmStateActionAnimatorBase
	{
		// Token: 0x060031F2 RID: 12786 RVA: 0x00107A34 File Offset: 0x00105C34
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.Value = null;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x00107A54 File Offset: 0x00105C54
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
			this.SetParameter();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x00107AD8 File Offset: 0x00105CD8
		public override void OnActionUpdate()
		{
			this.SetParameter();
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x00107AE0 File Offset: 0x00105CE0
		private void SetParameter()
		{
			if (this._animator != null)
			{
				this._animator.SetInteger(this._paramID, this.Value.Value);
			}
		}

		// Token: 0x04002318 RID: 8984
		[Tooltip("The target.")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002319 RID: 8985
		[Tooltip("The animator parameter")]
		[UIHint(UIHint.AnimatorInt)]
		[RequiredField]
		public FsmString parameter;

		// Token: 0x0400231A RID: 8986
		[Tooltip("The Int value to assign to the animator parameter")]
		public FsmInt Value;

		// Token: 0x0400231B RID: 8987
		private Animator _animator;

		// Token: 0x0400231C RID: 8988
		private int _paramID;
	}
}
