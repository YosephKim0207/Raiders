using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D2 RID: 2258
	[Tooltip("Sets the playback speed of the Animator. 1 is normal playback speed")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorSpeed : FsmStateAction
	{
		// Token: 0x06003210 RID: 12816 RVA: 0x00108144 File Offset: 0x00106344
		public override void Reset()
		{
			this.gameObject = null;
			this.speed = null;
			this.everyFrame = false;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0010815C File Offset: 0x0010635C
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
			this.DoPlaybackSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x001081CC File Offset: 0x001063CC
		public override void OnUpdate()
		{
			this.DoPlaybackSpeed();
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x001081D4 File Offset: 0x001063D4
		private void DoPlaybackSpeed()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.speed = this.speed.Value;
		}

		// Token: 0x04002338 RID: 9016
		[RequiredField]
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002339 RID: 9017
		[Tooltip("The playBack speed")]
		public FsmFloat speed;

		// Token: 0x0400233A RID: 9018
		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		// Token: 0x0400233B RID: 9019
		private Animator _animator;
	}
}
