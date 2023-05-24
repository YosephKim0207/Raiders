using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BE RID: 2238
	[Tooltip("Gets the playback speed of the Animator. 1 is normal playback speed")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorPlayBackSpeed : FsmStateAction
	{
		// Token: 0x060031AD RID: 12717 RVA: 0x00106A88 File Offset: 0x00104C88
		public override void Reset()
		{
			this.gameObject = null;
			this.playBackSpeed = null;
			this.everyFrame = false;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x00106AA0 File Offset: 0x00104CA0
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
			this.GetPlayBackSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00106B10 File Offset: 0x00104D10
		public override void OnUpdate()
		{
			this.GetPlayBackSpeed();
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x00106B18 File Offset: 0x00104D18
		private void GetPlayBackSpeed()
		{
			if (this._animator != null)
			{
				this.playBackSpeed.Value = this._animator.speed;
			}
		}

		// Token: 0x040022D4 RID: 8916
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022D5 RID: 8917
		[UIHint(UIHint.Variable)]
		[Tooltip("The playBack speed of the animator. 1 is normal playback speed")]
		[RequiredField]
		public FsmFloat playBackSpeed;

		// Token: 0x040022D6 RID: 8918
		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		// Token: 0x040022D7 RID: 8919
		private Animator _animator;
	}
}
