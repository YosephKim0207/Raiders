using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B3 RID: 2227
	[Tooltip("Returns true if the current rig is humanoid, false if it is generic. Can also sends events")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorIsHuman : FsmStateAction
	{
		// Token: 0x0600317B RID: 12667 RVA: 0x00105F1C File Offset: 0x0010411C
		public override void Reset()
		{
			this.gameObject = null;
			this.isHuman = null;
			this.isHumanEvent = null;
			this.isGenericEvent = null;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x00105F3C File Offset: 0x0010413C
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
			this.DoCheckIsHuman();
			base.Finish();
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00105FA0 File Offset: 0x001041A0
		private void DoCheckIsHuman()
		{
			if (this._animator == null)
			{
				return;
			}
			bool flag = this._animator.isHuman;
			if (!this.isHuman.IsNone)
			{
				this.isHuman.Value = flag;
			}
			if (flag)
			{
				base.Fsm.Event(this.isHumanEvent);
			}
			else
			{
				base.Fsm.Event(this.isGenericEvent);
			}
		}

		// Token: 0x0400229B RID: 8859
		[RequiredField]
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400229C RID: 8860
		[Tooltip("True if the current rig is humanoid, False if it is generic")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmBool isHuman;

		// Token: 0x0400229D RID: 8861
		[Tooltip("Event send if rig is humanoid")]
		public FsmEvent isHumanEvent;

		// Token: 0x0400229E RID: 8862
		[Tooltip("Event send if rig is generic")]
		public FsmEvent isGenericEvent;

		// Token: 0x0400229F RID: 8863
		private Animator _animator;
	}
}
