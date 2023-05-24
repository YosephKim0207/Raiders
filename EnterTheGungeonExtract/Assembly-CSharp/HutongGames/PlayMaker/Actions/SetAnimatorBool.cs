using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C7 RID: 2247
	[Tooltip("Sets the value of a bool parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorBool : FsmStateActionAnimatorBase
	{
		// Token: 0x060031DA RID: 12762 RVA: 0x001073E4 File Offset: 0x001055E4
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.Value = null;
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x00107404 File Offset: 0x00105604
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

		// Token: 0x060031DC RID: 12764 RVA: 0x00107488 File Offset: 0x00105688
		public override void OnActionUpdate()
		{
			this.SetParameter();
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x00107490 File Offset: 0x00105690
		private void SetParameter()
		{
			if (this._animator != null)
			{
				this._animator.SetBool(this._paramID, this.Value.Value);
			}
		}

		// Token: 0x040022FD RID: 8957
		[Tooltip("The target")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022FE RID: 8958
		[Tooltip("The animator parameter")]
		[UIHint(UIHint.AnimatorBool)]
		[RequiredField]
		public FsmString parameter;

		// Token: 0x040022FF RID: 8959
		[Tooltip("The Bool value to assign to the animator parameter")]
		public FsmBool Value;

		// Token: 0x04002300 RID: 8960
		private Animator _animator;

		// Token: 0x04002301 RID: 8961
		private int _paramID;
	}
}
