using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A7 RID: 2215
	[Tooltip("Check the current State name on a specified layer, this is more than the layer name, it holds the current state as well.")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorCurrentStateInfoIsName : FsmStateActionAnimatorBase
	{
		// Token: 0x06003141 RID: 12609 RVA: 0x001051A8 File Offset: 0x001033A8
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.name = null;
			this.nameMatchEvent = null;
			this.nameDoNotMatchEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001051DC File Offset: 0x001033DC
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

		// Token: 0x06003143 RID: 12611 RVA: 0x0010524C File Offset: 0x0010344C
		public override void OnActionUpdate()
		{
			this.IsName();
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x00105254 File Offset: 0x00103454
		private void IsName()
		{
			if (this._animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this.layerIndex.Value);
				if (!this.isMatching.IsNone)
				{
					this.isMatching.Value = currentAnimatorStateInfo.IsName(this.name.Value);
				}
				if (currentAnimatorStateInfo.IsName(this.name.Value))
				{
					base.Fsm.Event(this.nameMatchEvent);
				}
				else
				{
					base.Fsm.Event(this.nameDoNotMatchEvent);
				}
			}
		}

		// Token: 0x04002256 RID: 8790
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002257 RID: 8791
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x04002258 RID: 8792
		[Tooltip("The name to check the layer against.")]
		public FsmString name;

		// Token: 0x04002259 RID: 8793
		[Tooltip("True if name matches")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool isMatching;

		// Token: 0x0400225A RID: 8794
		[Tooltip("Event send if name matches")]
		public FsmEvent nameMatchEvent;

		// Token: 0x0400225B RID: 8795
		[Tooltip("Event send if name doesn't match")]
		public FsmEvent nameDoNotMatchEvent;

		// Token: 0x0400225C RID: 8796
		private Animator _animator;
	}
}
