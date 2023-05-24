using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C9 RID: 2249
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Activates feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
	public class SetAnimatorFeetPivotActive : FsmStateAction
	{
		// Token: 0x060031E3 RID: 12771 RVA: 0x0010757C File Offset: 0x0010577C
		public override void Reset()
		{
			this.gameObject = null;
			this.feetPivotActive = null;
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x0010758C File Offset: 0x0010578C
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
			this.DoFeetPivotActive();
			base.Finish();
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x001075F0 File Offset: 0x001057F0
		private void DoFeetPivotActive()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.feetPivotActive = this.feetPivotActive.Value;
		}

		// Token: 0x04002305 RID: 8965
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002306 RID: 8966
		[Tooltip("Activates feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
		public FsmFloat feetPivotActive;

		// Token: 0x04002307 RID: 8967
		private Animator _animator;
	}
}
