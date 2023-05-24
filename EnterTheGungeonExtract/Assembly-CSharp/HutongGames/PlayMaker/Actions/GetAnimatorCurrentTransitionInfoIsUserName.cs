using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AB RID: 2219
	[Tooltip("Check the active Transition user-specified name on a specified layer.")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorCurrentTransitionInfoIsUserName : FsmStateActionAnimatorBase
	{
		// Token: 0x06003155 RID: 12629 RVA: 0x0010570C File Offset: 0x0010390C
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.userName = null;
			this.nameMatch = null;
			this.nameMatchEvent = null;
			this.nameDoNotMatchEvent = null;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00105740 File Offset: 0x00103940
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
			this.IsName();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x001057B0 File Offset: 0x001039B0
		public override void OnActionUpdate()
		{
			this.IsName();
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x001057B8 File Offset: 0x001039B8
		private void IsName()
		{
			if (this._animator != null)
			{
				bool flag = this._animator.GetAnimatorTransitionInfo(this.layerIndex.Value).IsUserName(this.userName.Value);
				if (!this.nameMatch.IsNone)
				{
					this.nameMatch.Value = flag;
				}
				if (flag)
				{
					base.Fsm.Event(this.nameMatchEvent);
				}
				else
				{
					base.Fsm.Event(this.nameDoNotMatchEvent);
				}
			}
		}

		// Token: 0x04002272 RID: 8818
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002273 RID: 8819
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x04002274 RID: 8820
		[Tooltip("The user-specified name to check the transition against.")]
		public FsmString userName;

		// Token: 0x04002275 RID: 8821
		[Tooltip("True if name matches")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool nameMatch;

		// Token: 0x04002276 RID: 8822
		[Tooltip("Event send if name matches")]
		public FsmEvent nameMatchEvent;

		// Token: 0x04002277 RID: 8823
		[Tooltip("Event send if name doesn't match")]
		public FsmEvent nameDoNotMatchEvent;

		// Token: 0x04002278 RID: 8824
		private Animator _animator;
	}
}
