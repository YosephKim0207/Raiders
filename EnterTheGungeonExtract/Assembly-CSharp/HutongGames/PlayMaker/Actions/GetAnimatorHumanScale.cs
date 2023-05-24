using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B0 RID: 2224
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns the scale of the current Avatar for a humanoid rig, (1 by default if the rig is generic).\n The scale is relative to Unity's Default Avatar")]
	public class GetAnimatorHumanScale : FsmStateAction
	{
		// Token: 0x0600316D RID: 12653 RVA: 0x00105B80 File Offset: 0x00103D80
		public override void Reset()
		{
			this.gameObject = null;
			this.humanScale = null;
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x00105B90 File Offset: 0x00103D90
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
			this.DoGetHumanScale();
			base.Finish();
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x00105BF4 File Offset: 0x00103DF4
		private void DoGetHumanScale()
		{
			if (this._animator == null)
			{
				return;
			}
			this.humanScale.Value = this._animator.humanScale;
		}

		// Token: 0x04002289 RID: 8841
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400228A RID: 8842
		[Tooltip("the scale of the current Avatar")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmFloat humanScale;

		// Token: 0x0400228B RID: 8843
		private Animator _animator;
	}
}
