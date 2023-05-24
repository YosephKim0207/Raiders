using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D1 RID: 2257
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the playback position in the recording buffer. When in playback mode (use AnimatorStartPlayback), this value is used for controlling the current playback position in the buffer (in seconds). The value can range between recordingStartTime and recordingStopTime ")]
	public class SetAnimatorPlayBackTime : FsmStateAction
	{
		// Token: 0x0600320B RID: 12811 RVA: 0x00108080 File Offset: 0x00106280
		public override void Reset()
		{
			this.gameObject = null;
			this.playbackTime = null;
			this.everyFrame = false;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x00108098 File Offset: 0x00106298
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
			this.DoPlaybackTime();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x00108108 File Offset: 0x00106308
		public override void OnUpdate()
		{
			this.DoPlaybackTime();
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x00108110 File Offset: 0x00106310
		private void DoPlaybackTime()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.playbackTime = this.playbackTime.Value;
		}

		// Token: 0x04002334 RID: 9012
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002335 RID: 9013
		[Tooltip("The playBack time")]
		public FsmFloat playbackTime;

		// Token: 0x04002336 RID: 9014
		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		// Token: 0x04002337 RID: 9015
		private Animator _animator;
	}
}
