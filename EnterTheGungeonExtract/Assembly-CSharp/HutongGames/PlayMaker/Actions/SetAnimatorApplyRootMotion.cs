using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C5 RID: 2245
	[Tooltip("Set Apply Root Motion: If true, Root is controlled by animations")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorApplyRootMotion : FsmStateAction
	{
		// Token: 0x060031D0 RID: 12752 RVA: 0x0010711C File Offset: 0x0010531C
		public override void Reset()
		{
			this.gameObject = null;
			this.applyRootMotion = null;
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0010712C File Offset: 0x0010532C
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
			this.DoApplyRootMotion();
			base.Finish();
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00107190 File Offset: 0x00105390
		private void DoApplyRootMotion()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.applyRootMotion = this.applyRootMotion.Value;
		}

		// Token: 0x040022F3 RID: 8947
		[RequiredField]
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022F4 RID: 8948
		[Tooltip("If true, Root is controlled by animations")]
		public FsmBool applyRootMotion;

		// Token: 0x040022F5 RID: 8949
		private Animator _animator;
	}
}
