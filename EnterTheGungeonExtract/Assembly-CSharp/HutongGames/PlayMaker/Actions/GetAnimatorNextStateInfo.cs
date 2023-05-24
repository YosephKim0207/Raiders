using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BC RID: 2236
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the next State information on a specified layer")]
	public class GetAnimatorNextStateInfo : FsmStateActionAnimatorBase
	{
		// Token: 0x060031A3 RID: 12707 RVA: 0x00106738 File Offset: 0x00104938
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
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00106794 File Offset: 0x00104994
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

		// Token: 0x060031A5 RID: 12709 RVA: 0x00106804 File Offset: 0x00104A04
		public override void OnActionUpdate()
		{
			this.GetLayerInfo();
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0010680C File Offset: 0x00104A0C
		private void GetLayerInfo()
		{
			if (this._animator != null)
			{
				AnimatorStateInfo nextAnimatorStateInfo = this._animator.GetNextAnimatorStateInfo(this.layerIndex.Value);
				if (!this.nameHash.IsNone)
				{
					this.nameHash.Value = nextAnimatorStateInfo.nameHash;
				}
				if (!this.name.IsNone)
				{
					this.name.Value = this._animator.GetLayerName(this.layerIndex.Value);
				}
				if (!this.tagHash.IsNone)
				{
					this.tagHash.Value = nextAnimatorStateInfo.tagHash;
				}
				if (!this.length.IsNone)
				{
					this.length.Value = nextAnimatorStateInfo.length;
				}
				if (!this.isStateLooping.IsNone)
				{
					this.isStateLooping.Value = nextAnimatorStateInfo.loop;
				}
				if (!this.normalizedTime.IsNone)
				{
					this.normalizedTime.Value = nextAnimatorStateInfo.normalizedTime;
				}
				if (!this.loopCount.IsNone || !this.currentLoopProgress.IsNone)
				{
					this.loopCount.Value = (int)Math.Truncate((double)nextAnimatorStateInfo.normalizedTime);
					this.currentLoopProgress.Value = nextAnimatorStateInfo.normalizedTime - (float)this.loopCount.Value;
				}
			}
		}

		// Token: 0x040022C5 RID: 8901
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022C6 RID: 8902
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x040022C7 RID: 8903
		[Tooltip("The layer's name.")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmString name;

		// Token: 0x040022C8 RID: 8904
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		[UIHint(UIHint.Variable)]
		public FsmInt nameHash;

		// Token: 0x040022C9 RID: 8905
		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's tag hash")]
		public FsmInt tagHash;

		// Token: 0x040022CA RID: 8906
		[UIHint(UIHint.Variable)]
		[Tooltip("Is the state looping. All animations in the state must be looping")]
		public FsmBool isStateLooping;

		// Token: 0x040022CB RID: 8907
		[UIHint(UIHint.Variable)]
		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		public FsmFloat length;

		// Token: 0x040022CC RID: 8908
		[UIHint(UIHint.Variable)]
		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		public FsmFloat normalizedTime;

		// Token: 0x040022CD RID: 8909
		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmInt loopCount;

		// Token: 0x040022CE RID: 8910
		[UIHint(UIHint.Variable)]
		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		public FsmFloat currentLoopProgress;

		// Token: 0x040022CF RID: 8911
		private Animator _animator;
	}
}
