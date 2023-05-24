using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A6 RID: 2214
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the current State information on a specified layer")]
	public class GetAnimatorCurrentStateInfo : FsmStateActionAnimatorBase
	{
		// Token: 0x0600313C RID: 12604 RVA: 0x00104F60 File Offset: 0x00103160
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.name = null;
			this.nameHash = null;
			this.tagHash = null;
			this.length = null;
			this.normalizedTime = null;
			this.isStateLooping = null;
			this.loopCount = null;
			this.currentLoopProgress = null;
			this.everyFrame = false;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x00104FC0 File Offset: 0x001031C0
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
			this.GetLayerInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x00105030 File Offset: 0x00103230
		public override void OnActionUpdate()
		{
			this.GetLayerInfo();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x00105038 File Offset: 0x00103238
		private void GetLayerInfo()
		{
			if (this._animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this.layerIndex.Value);
				if (!this.nameHash.IsNone)
				{
					this.nameHash.Value = currentAnimatorStateInfo.nameHash;
				}
				if (!this.name.IsNone)
				{
					this.name.Value = this._animator.GetLayerName(this.layerIndex.Value);
				}
				if (!this.tagHash.IsNone)
				{
					this.tagHash.Value = currentAnimatorStateInfo.tagHash;
				}
				if (!this.length.IsNone)
				{
					this.length.Value = currentAnimatorStateInfo.length;
				}
				if (!this.isStateLooping.IsNone)
				{
					this.isStateLooping.Value = currentAnimatorStateInfo.loop;
				}
				if (!this.normalizedTime.IsNone)
				{
					this.normalizedTime.Value = currentAnimatorStateInfo.normalizedTime;
				}
				if (!this.loopCount.IsNone || !this.currentLoopProgress.IsNone)
				{
					this.loopCount.Value = (int)Math.Truncate((double)currentAnimatorStateInfo.normalizedTime);
					this.currentLoopProgress.Value = currentAnimatorStateInfo.normalizedTime - (float)this.loopCount.Value;
				}
			}
		}

		// Token: 0x0400224B RID: 8779
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The target.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400224C RID: 8780
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x0400224D RID: 8781
		[Tooltip("The layer's name.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmString name;

		// Token: 0x0400224E RID: 8782
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		[UIHint(UIHint.Variable)]
		public FsmInt nameHash;

		// Token: 0x0400224F RID: 8783
		[Tooltip("The layer's tag hash")]
		[UIHint(UIHint.Variable)]
		public FsmInt tagHash;

		// Token: 0x04002250 RID: 8784
		[Tooltip("Is the state looping. All animations in the state must be looping")]
		[UIHint(UIHint.Variable)]
		public FsmBool isStateLooping;

		// Token: 0x04002251 RID: 8785
		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		[UIHint(UIHint.Variable)]
		public FsmFloat length;

		// Token: 0x04002252 RID: 8786
		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		[UIHint(UIHint.Variable)]
		public FsmFloat normalizedTime;

		// Token: 0x04002253 RID: 8787
		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmInt loopCount;

		// Token: 0x04002254 RID: 8788
		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmFloat currentLoopProgress;

		// Token: 0x04002255 RID: 8789
		private Animator _animator;
	}
}
