using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D5 RID: 2261
	[Tooltip("Sets a trigger parameter to active. Triggers are parameters that act mostly like booleans, but get reset to inactive when they are used in a transition.")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorTrigger : FsmStateAction
	{
		// Token: 0x0600321F RID: 12831 RVA: 0x00108390 File Offset: 0x00106590
		public override void Reset()
		{
			this.gameObject = null;
			this.trigger = null;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x001083A0 File Offset: 0x001065A0
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
			this.SetTrigger();
			base.Finish();
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x00108404 File Offset: 0x00106604
		private void SetTrigger()
		{
			if (this._animator != null)
			{
				this._animator.SetTrigger(this.trigger.Value);
			}
		}

		// Token: 0x04002344 RID: 9028
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002345 RID: 9029
		[Tooltip("The trigger name")]
		[UIHint(UIHint.AnimatorTrigger)]
		[RequiredField]
		public FsmString trigger;

		// Token: 0x04002346 RID: 9030
		private Animator _animator;
	}
}
