using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BB RID: 2235
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Get the left foot bottom height.")]
	public class GetAnimatorLeftFootBottomHeight : FsmStateAction
	{
		// Token: 0x0600319E RID: 12702 RVA: 0x00106674 File Offset: 0x00104874
		public override void Reset()
		{
			this.gameObject = null;
			this.leftFootHeight = null;
			this.everyFrame = false;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x0010668C File Offset: 0x0010488C
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
			this._getLeftFootBottonHeight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001066FC File Offset: 0x001048FC
		public override void OnLateUpdate()
		{
			this._getLeftFootBottonHeight();
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x00106704 File Offset: 0x00104904
		private void _getLeftFootBottonHeight()
		{
			if (this._animator != null)
			{
				this.leftFootHeight.Value = this._animator.leftFeetBottomHeight;
			}
		}

		// Token: 0x040022C1 RID: 8897
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022C2 RID: 8898
		[RequiredField]
		[ActionSection("Result")]
		[Tooltip("the left foot bottom height.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat leftFootHeight;

		// Token: 0x040022C3 RID: 8899
		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		// Token: 0x040022C4 RID: 8900
		private Animator _animator;
	}
}
