using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CD RID: 2253
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("If true, additionnal layers affects the mass center")]
	public class SetAnimatorLayersAffectMassCenter : FsmStateAction
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x00107B18 File Offset: 0x00105D18
		public override void Reset()
		{
			this.gameObject = null;
			this.affectMassCenter = null;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x00107B28 File Offset: 0x00105D28
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
			this.SetAffectMassCenter();
			base.Finish();
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x00107B8C File Offset: 0x00105D8C
		private void SetAffectMassCenter()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.layersAffectMassCenter = this.affectMassCenter.Value;
		}

		// Token: 0x0400231D RID: 8989
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400231E RID: 8990
		[Tooltip("If true, additionnal layers affects the mass center")]
		public FsmBool affectMassCenter;

		// Token: 0x0400231F RID: 8991
		private Animator _animator;
	}
}
