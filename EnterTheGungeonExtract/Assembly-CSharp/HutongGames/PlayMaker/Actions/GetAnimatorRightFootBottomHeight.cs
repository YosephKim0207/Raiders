using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C0 RID: 2240
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Get the right foot bottom height.")]
	public class GetAnimatorRightFootBottomHeight : FsmStateAction
	{
		// Token: 0x060031B7 RID: 12727 RVA: 0x00106C10 File Offset: 0x00104E10
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.rightFootHeight = null;
			this.everyFrame = false;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x00106C30 File Offset: 0x00104E30
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
			this._getRightFootBottonHeight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x00106CA0 File Offset: 0x00104EA0
		public override void OnLateUpdate()
		{
			this._getRightFootBottonHeight();
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x00106CA8 File Offset: 0x00104EA8
		private void _getRightFootBottonHeight()
		{
			if (this._animator != null)
			{
				this.rightFootHeight.Value = this._animator.rightFeetBottomHeight;
			}
		}

		// Token: 0x040022DC RID: 8924
		[RequiredField]
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022DD RID: 8925
		[ActionSection("Result")]
		[Tooltip("The right foot bottom height.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat rightFootHeight;

		// Token: 0x040022DE RID: 8926
		[Tooltip("Repeat every frame during LateUpdate. Useful when value is subject to change over time.")]
		public bool everyFrame;

		// Token: 0x040022DF RID: 8927
		private Animator _animator;
	}
}
