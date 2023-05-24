using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A1 RID: 2209
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the value of ApplyRootMotion of an avatar. If true, root is controlled by animations")]
	public class GetAnimatorApplyRootMotion : FsmStateAction
	{
		// Token: 0x06003126 RID: 12582 RVA: 0x00104A7C File Offset: 0x00102C7C
		public override void Reset()
		{
			this.gameObject = null;
			this.rootMotionApplied = null;
			this.rootMotionIsAppliedEvent = null;
			this.rootMotionIsNotAppliedEvent = null;
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x00104A9C File Offset: 0x00102C9C
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
			this.GetApplyMotionRoot();
			base.Finish();
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x00104B00 File Offset: 0x00102D00
		private void GetApplyMotionRoot()
		{
			if (this._animator != null)
			{
				bool applyRootMotion = this._animator.applyRootMotion;
				this.rootMotionApplied.Value = applyRootMotion;
				if (applyRootMotion)
				{
					base.Fsm.Event(this.rootMotionIsAppliedEvent);
				}
				else
				{
					base.Fsm.Event(this.rootMotionIsNotAppliedEvent);
				}
			}
		}

		// Token: 0x04002232 RID: 8754
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002233 RID: 8755
		[UIHint(UIHint.Variable)]
		[Tooltip("Is the rootMotionapplied. If true, root is controlled by animations")]
		[ActionSection("Results")]
		[RequiredField]
		public FsmBool rootMotionApplied;

		// Token: 0x04002234 RID: 8756
		[Tooltip("Event send if the root motion is applied")]
		public FsmEvent rootMotionIsAppliedEvent;

		// Token: 0x04002235 RID: 8757
		[Tooltip("Event send if the root motion is not applied")]
		public FsmEvent rootMotionIsNotAppliedEvent;

		// Token: 0x04002236 RID: 8758
		private Animator _animator;
	}
}
