using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C2 RID: 2242
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the playback speed of the Animator. 1 is normal playback speed")]
	public class GetAnimatorSpeed : FsmStateActionAnimatorBase
	{
		// Token: 0x060031C1 RID: 12737 RVA: 0x00106E2C File Offset: 0x0010502C
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.speed = null;
			this.everyFrame = false;
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x00106E4C File Offset: 0x0010504C
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
			this.GetPlaybackSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x00106EBC File Offset: 0x001050BC
		public override void OnActionUpdate()
		{
			this.GetPlaybackSpeed();
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x00106EC4 File Offset: 0x001050C4
		private void GetPlaybackSpeed()
		{
			if (this._animator != null)
			{
				this.speed.Value = this._animator.speed;
			}
		}

		// Token: 0x040022E6 RID: 8934
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022E7 RID: 8935
		[RequiredField]
		[Tooltip("The playBack speed of the animator. 1 is normal playback speed")]
		[UIHint(UIHint.Variable)]
		public FsmFloat speed;

		// Token: 0x040022E8 RID: 8936
		private Animator _animator;
	}
}
