using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B4 RID: 2228
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns true if the specified layer is in a transition. Can also send events")]
	public class GetAnimatorIsLayerInTransition : FsmStateActionAnimatorBase
	{
		// Token: 0x0600317F RID: 12671 RVA: 0x0010601C File Offset: 0x0010421C
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.isInTransition = null;
			this.isInTransitionEvent = null;
			this.isNotInTransitionEvent = null;
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x00106040 File Offset: 0x00104240
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
			this.DoCheckIsInTransition();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001060B0 File Offset: 0x001042B0
		public override void OnActionUpdate()
		{
			this.DoCheckIsInTransition();
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001060B8 File Offset: 0x001042B8
		private void DoCheckIsInTransition()
		{
			if (this._animator == null)
			{
				return;
			}
			bool flag = this._animator.IsInTransition(this.layerIndex.Value);
			if (!this.isInTransition.IsNone)
			{
				this.isInTransition.Value = flag;
			}
			if (flag)
			{
				base.Fsm.Event(this.isInTransitionEvent);
			}
			else
			{
				base.Fsm.Event(this.isNotInTransitionEvent);
			}
		}

		// Token: 0x040022A0 RID: 8864
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022A1 RID: 8865
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x040022A2 RID: 8866
		[ActionSection("Results")]
		[Tooltip("True if automatic matching is active")]
		[UIHint(UIHint.Variable)]
		public FsmBool isInTransition;

		// Token: 0x040022A3 RID: 8867
		[Tooltip("Event send if automatic matching is active")]
		public FsmEvent isInTransitionEvent;

		// Token: 0x040022A4 RID: 8868
		[Tooltip("Event send if automatic matching is not active")]
		public FsmEvent isNotInTransitionEvent;

		// Token: 0x040022A5 RID: 8869
		private Animator _animator;
	}
}
