using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B5 RID: 2229
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns true if automatic matching is active. Can also send events")]
	public class GetAnimatorIsMatchingTarget : FsmStateActionAnimatorBase
	{
		// Token: 0x06003184 RID: 12676 RVA: 0x00106140 File Offset: 0x00104340
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.isMatchingActive = null;
			this.matchingActivatedEvent = null;
			this.matchingDeactivedEvent = null;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x00106164 File Offset: 0x00104364
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
			this.DoCheckIsMatchingActive();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001061D4 File Offset: 0x001043D4
		public override void OnActionUpdate()
		{
			this.DoCheckIsMatchingActive();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x001061DC File Offset: 0x001043DC
		private void DoCheckIsMatchingActive()
		{
			if (this._animator == null)
			{
				return;
			}
			bool isMatchingTarget = this._animator.isMatchingTarget;
			this.isMatchingActive.Value = isMatchingTarget;
			if (isMatchingTarget)
			{
				base.Fsm.Event(this.matchingActivatedEvent);
			}
			else
			{
				base.Fsm.Event(this.matchingDeactivedEvent);
			}
		}

		// Token: 0x040022A6 RID: 8870
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022A7 RID: 8871
		[UIHint(UIHint.Variable)]
		[Tooltip("True if automatic matching is active")]
		[ActionSection("Results")]
		public FsmBool isMatchingActive;

		// Token: 0x040022A8 RID: 8872
		[Tooltip("Event send if automatic matching is active")]
		public FsmEvent matchingActivatedEvent;

		// Token: 0x040022A9 RID: 8873
		[Tooltip("Event send if automatic matching is not active")]
		public FsmEvent matchingDeactivedEvent;

		// Token: 0x040022AA RID: 8874
		private Animator _animator;
	}
}
