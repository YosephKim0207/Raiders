using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AA RID: 2218
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Check the active Transition name on a specified layer. Format is 'CURRENT_STATE -> NEXT_STATE'.")]
	public class GetAnimatorCurrentTransitionInfoIsName : FsmStateActionAnimatorBase
	{
		// Token: 0x06003150 RID: 12624 RVA: 0x001055CC File Offset: 0x001037CC
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.name = null;
			this.nameMatch = null;
			this.nameMatchEvent = null;
			this.nameDoNotMatchEvent = null;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x00105600 File Offset: 0x00103800
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

		// Token: 0x06003152 RID: 12626 RVA: 0x00105670 File Offset: 0x00103870
		public override void OnActionUpdate()
		{
			this.IsName();
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00105678 File Offset: 0x00103878
		private void IsName()
		{
			if (this._animator != null)
			{
				if (this._animator.GetAnimatorTransitionInfo(this.layerIndex.Value).IsName(this.name.Value))
				{
					this.nameMatch.Value = true;
					base.Fsm.Event(this.nameMatchEvent);
				}
				else
				{
					this.nameMatch.Value = false;
					base.Fsm.Event(this.nameDoNotMatchEvent);
				}
			}
		}

		// Token: 0x0400226B RID: 8811
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400226C RID: 8812
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x0400226D RID: 8813
		[Tooltip("The name to check the transition against.")]
		public FsmString name;

		// Token: 0x0400226E RID: 8814
		[Tooltip("True if name matches")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool nameMatch;

		// Token: 0x0400226F RID: 8815
		[Tooltip("Event send if name matches")]
		public FsmEvent nameMatchEvent;

		// Token: 0x04002270 RID: 8816
		[Tooltip("Event send if name doesn't match")]
		public FsmEvent nameDoNotMatchEvent;

		// Token: 0x04002271 RID: 8817
		private Animator _animator;
	}
}
