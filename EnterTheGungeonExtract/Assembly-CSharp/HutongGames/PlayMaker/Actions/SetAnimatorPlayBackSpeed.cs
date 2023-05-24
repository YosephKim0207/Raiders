using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D0 RID: 2256
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the playback speed of the Animator. 1 is normal playback speed")]
	public class SetAnimatorPlayBackSpeed : FsmStateAction
	{
		// Token: 0x06003206 RID: 12806 RVA: 0x00107FBC File Offset: 0x001061BC
		public override void Reset()
		{
			this.gameObject = null;
			this.playBackSpeed = null;
			this.everyFrame = false;
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x00107FD4 File Offset: 0x001061D4
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
			this.DoPlayBackSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x00108044 File Offset: 0x00106244
		public override void OnUpdate()
		{
			this.DoPlayBackSpeed();
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x0010804C File Offset: 0x0010624C
		private void DoPlayBackSpeed()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.speed = this.playBackSpeed.Value;
		}

		// Token: 0x04002330 RID: 9008
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002331 RID: 9009
		[Tooltip("If true, automaticly stabilize feet during transition and blending")]
		public FsmFloat playBackSpeed;

		// Token: 0x04002332 RID: 9010
		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		// Token: 0x04002333 RID: 9011
		private Animator _animator;
	}
}
