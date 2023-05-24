using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B9 RID: 2233
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns if additionnal layers affects the mass center")]
	public class GetAnimatorLayersAffectMassCenter : FsmStateAction
	{
		// Token: 0x06003195 RID: 12693 RVA: 0x001064B0 File Offset: 0x001046B0
		public override void Reset()
		{
			this.gameObject = null;
			this.affectMassCenter = null;
			this.affectMassCenterEvent = null;
			this.doNotAffectMassCenterEvent = null;
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x001064D0 File Offset: 0x001046D0
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
			this.CheckAffectMassCenter();
			base.Finish();
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x00106534 File Offset: 0x00104734
		private void CheckAffectMassCenter()
		{
			if (this._animator == null)
			{
				return;
			}
			bool layersAffectMassCenter = this._animator.layersAffectMassCenter;
			this.affectMassCenter.Value = layersAffectMassCenter;
			if (layersAffectMassCenter)
			{
				base.Fsm.Event(this.affectMassCenterEvent);
			}
			else
			{
				base.Fsm.Event(this.doNotAffectMassCenterEvent);
			}
		}

		// Token: 0x040022B8 RID: 8888
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022B9 RID: 8889
		[RequiredField]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		[Tooltip("If true, additionnal layers affects the mass center")]
		public FsmBool affectMassCenter;

		// Token: 0x040022BA RID: 8890
		[Tooltip("Event send if additionnal layers affects the mass center")]
		public FsmEvent affectMassCenterEvent;

		// Token: 0x040022BB RID: 8891
		[Tooltip("Event send if additionnal layers do no affects the mass center")]
		public FsmEvent doNotAffectMassCenterEvent;

		// Token: 0x040022BC RID: 8892
		private Animator _animator;
	}
}
