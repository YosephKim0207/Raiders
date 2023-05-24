using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BF RID: 2239
	[Tooltip("Gets the playback position in the recording buffer. When in playback mode (use  AnimatorStartPlayback), this value is used for controlling the current playback position in the buffer (in seconds). The value can range between recordingStartTime and recordingStopTime See Also: StartPlayback, StopPlayback.")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorPlayBackTime : FsmStateAction
	{
		// Token: 0x060031B2 RID: 12722 RVA: 0x00106B4C File Offset: 0x00104D4C
		public override void Reset()
		{
			this.gameObject = null;
			this.playBackTime = null;
			this.everyFrame = false;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00106B64 File Offset: 0x00104D64
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
			this.GetPlayBackTime();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x00106BD4 File Offset: 0x00104DD4
		public override void OnUpdate()
		{
			this.GetPlayBackTime();
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x00106BDC File Offset: 0x00104DDC
		private void GetPlayBackTime()
		{
			if (this._animator != null)
			{
				this.playBackTime.Value = this._animator.playbackTime;
			}
		}

		// Token: 0x040022D8 RID: 8920
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022D9 RID: 8921
		[UIHint(UIHint.Variable)]
		[Tooltip("The playBack time of the animator.")]
		[ActionSection("Result")]
		[RequiredField]
		public FsmFloat playBackTime;

		// Token: 0x040022DA RID: 8922
		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		// Token: 0x040022DB RID: 8923
		private Animator _animator;
	}
}
