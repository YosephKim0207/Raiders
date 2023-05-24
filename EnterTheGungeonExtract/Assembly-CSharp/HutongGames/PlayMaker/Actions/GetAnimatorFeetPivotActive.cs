using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008AD RID: 2221
	[Tooltip("Returns the feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorFeetPivotActive : FsmStateAction
	{
		// Token: 0x0600315F RID: 12639 RVA: 0x00105928 File Offset: 0x00103B28
		public override void Reset()
		{
			this.gameObject = null;
			this.feetPivotActive = null;
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00105938 File Offset: 0x00103B38
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
			this.DoGetFeetPivotActive();
			base.Finish();
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x0010599C File Offset: 0x00103B9C
		private void DoGetFeetPivotActive()
		{
			if (this._animator == null)
			{
				return;
			}
			this.feetPivotActive.Value = this._animator.feetPivotActive;
		}

		// Token: 0x0400227E RID: 8830
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400227F RID: 8831
		[Tooltip("The feet pivot Blending. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat feetPivotActive;

		// Token: 0x04002280 RID: 8832
		private Animator _animator;
	}
}
