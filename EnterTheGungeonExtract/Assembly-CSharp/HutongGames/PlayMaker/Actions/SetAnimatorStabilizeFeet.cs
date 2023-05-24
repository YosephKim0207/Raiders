using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D3 RID: 2259
	[Tooltip("If true, automaticaly stabilize feet during transition and blending")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorStabilizeFeet : FsmStateAction
	{
		// Token: 0x06003215 RID: 12821 RVA: 0x00108208 File Offset: 0x00106408
		public override void Reset()
		{
			this.gameObject = null;
			this.stabilizeFeet = null;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x00108218 File Offset: 0x00106418
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
			this.DoStabilizeFeet();
			base.Finish();
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x0010827C File Offset: 0x0010647C
		private void DoStabilizeFeet()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.stabilizeFeet = this.stabilizeFeet.Value;
		}

		// Token: 0x0400233C RID: 9020
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400233D RID: 9021
		[Tooltip("If true, automaticaly stabilize feet during transition and blending")]
		public FsmBool stabilizeFeet;

		// Token: 0x0400233E RID: 9022
		private Animator _animator;
	}
}
