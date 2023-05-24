using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A9 RID: 2217
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the current transition information on a specified layer. Only valid when during a transition.")]
	public class GetAnimatorCurrentTransitionInfo : FsmStateActionAnimatorBase
	{
		// Token: 0x0600314B RID: 12619 RVA: 0x00105444 File Offset: 0x00103644
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.name = null;
			this.nameHash = null;
			this.userNameHash = null;
			this.normalizedTime = null;
			this.everyFrame = false;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x00105480 File Offset: 0x00103680
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
			this.GetTransitionInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x001054F0 File Offset: 0x001036F0
		public override void OnActionUpdate()
		{
			this.GetTransitionInfo();
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x001054F8 File Offset: 0x001036F8
		private void GetTransitionInfo()
		{
			if (this._animator != null)
			{
				AnimatorTransitionInfo animatorTransitionInfo = this._animator.GetAnimatorTransitionInfo(this.layerIndex.Value);
				if (!this.name.IsNone)
				{
					this.name.Value = this._animator.GetLayerName(this.layerIndex.Value);
				}
				if (!this.nameHash.IsNone)
				{
					this.nameHash.Value = animatorTransitionInfo.nameHash;
				}
				if (!this.userNameHash.IsNone)
				{
					this.userNameHash.Value = animatorTransitionInfo.userNameHash;
				}
				if (!this.normalizedTime.IsNone)
				{
					this.normalizedTime.Value = animatorTransitionInfo.normalizedTime;
				}
			}
		}

		// Token: 0x04002264 RID: 8804
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002265 RID: 8805
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x04002266 RID: 8806
		[Tooltip("The unique name of the Transition")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmString name;

		// Token: 0x04002267 RID: 8807
		[Tooltip("The unique name of the Transition")]
		[UIHint(UIHint.Variable)]
		public FsmInt nameHash;

		// Token: 0x04002268 RID: 8808
		[Tooltip("The user-specidied name of the Transition")]
		[UIHint(UIHint.Variable)]
		public FsmInt userNameHash;

		// Token: 0x04002269 RID: 8809
		[Tooltip("Normalized time of the Transition")]
		[UIHint(UIHint.Variable)]
		public FsmFloat normalizedTime;

		// Token: 0x0400226A RID: 8810
		private Animator _animator;
	}
}
