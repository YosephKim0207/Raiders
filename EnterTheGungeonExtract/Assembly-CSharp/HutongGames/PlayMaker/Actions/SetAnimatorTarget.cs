using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D4 RID: 2260
	[Tooltip("Sets an AvatarTarget and a targetNormalizedTime for the current state")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorTarget : FsmStateAction
	{
		// Token: 0x06003219 RID: 12825 RVA: 0x001082B0 File Offset: 0x001064B0
		public override void Reset()
		{
			this.gameObject = null;
			this.avatarTarget = AvatarTarget.Body;
			this.targetNormalizedTime = null;
			this.everyFrame = false;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x001082D0 File Offset: 0x001064D0
		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorMove = true;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x001082E0 File Offset: 0x001064E0
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
			this.SetTarget();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x00108350 File Offset: 0x00106550
		public override void DoAnimatorMove()
		{
			this.SetTarget();
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x00108358 File Offset: 0x00106558
		private void SetTarget()
		{
			if (this._animator != null)
			{
				this._animator.SetTarget(this.avatarTarget, this.targetNormalizedTime.Value);
			}
		}

		// Token: 0x0400233F RID: 9023
		[RequiredField]
		[Tooltip("The target.")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002340 RID: 9024
		[Tooltip("The avatar target")]
		public AvatarTarget avatarTarget;

		// Token: 0x04002341 RID: 9025
		[Tooltip("The current state Time that is queried")]
		public FsmFloat targetNormalizedTime;

		// Token: 0x04002342 RID: 9026
		[Tooltip("Repeat every frame during OnAnimatorMove. Useful when changing over time.")]
		public bool everyFrame;

		// Token: 0x04002343 RID: 9027
		private Animator _animator;
	}
}
