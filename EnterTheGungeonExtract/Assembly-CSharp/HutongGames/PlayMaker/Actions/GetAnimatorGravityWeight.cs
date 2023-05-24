using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AF RID: 2223
	[Tooltip("Returns The current gravity weight based on current animations that are played")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorGravityWeight : FsmStateActionAnimatorBase
	{
		// Token: 0x06003168 RID: 12648 RVA: 0x00105AB4 File Offset: 0x00103CB4
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.gravityWeight = null;
			this.everyFrame = false;
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x00105AD4 File Offset: 0x00103CD4
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
			this.DoGetGravityWeight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x00105B44 File Offset: 0x00103D44
		public override void OnActionUpdate()
		{
			this.DoGetGravityWeight();
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x00105B4C File Offset: 0x00103D4C
		private void DoGetGravityWeight()
		{
			if (this._animator == null)
			{
				return;
			}
			this.gravityWeight.Value = this._animator.gravityWeight;
		}

		// Token: 0x04002286 RID: 8838
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002287 RID: 8839
		[Tooltip("The current gravity weight based on current animations that are played")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmFloat gravityWeight;

		// Token: 0x04002288 RID: 8840
		private Animator _animator;
	}
}
