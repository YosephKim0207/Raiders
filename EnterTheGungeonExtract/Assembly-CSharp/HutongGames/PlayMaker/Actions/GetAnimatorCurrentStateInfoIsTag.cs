using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A8 RID: 2216
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Does tag match the tag of the active state in the statemachine")]
	public class GetAnimatorCurrentStateInfoIsTag : FsmStateActionAnimatorBase
	{
		// Token: 0x06003146 RID: 12614 RVA: 0x001052FC File Offset: 0x001034FC
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.tag = null;
			this.tagMatch = null;
			this.tagMatchEvent = null;
			this.tagDoNotMatchEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x00105338 File Offset: 0x00103538
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
			this.IsTag();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x001053A8 File Offset: 0x001035A8
		public override void OnActionUpdate()
		{
			this.IsTag();
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x001053B0 File Offset: 0x001035B0
		private void IsTag()
		{
			if (this._animator != null)
			{
				if (this._animator.GetCurrentAnimatorStateInfo(this.layerIndex.Value).IsTag(this.tag.Value))
				{
					this.tagMatch.Value = true;
					base.Fsm.Event(this.tagMatchEvent);
				}
				else
				{
					this.tagMatch.Value = false;
					base.Fsm.Event(this.tagDoNotMatchEvent);
				}
			}
		}

		// Token: 0x0400225D RID: 8797
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400225E RID: 8798
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x0400225F RID: 8799
		[Tooltip("The tag to check the layer against.")]
		public FsmString tag;

		// Token: 0x04002260 RID: 8800
		[Tooltip("True if tag matches")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool tagMatch;

		// Token: 0x04002261 RID: 8801
		[Tooltip("Event send if tag matches")]
		public FsmEvent tagMatchEvent;

		// Token: 0x04002262 RID: 8802
		[Tooltip("Event send if tag matches")]
		public FsmEvent tagDoNotMatchEvent;

		// Token: 0x04002263 RID: 8803
		private Animator _animator;
	}
}
