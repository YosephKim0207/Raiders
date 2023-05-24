using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089E RID: 2206
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Stops the animator record mode. It will lock the recording buffer's contents in its current state. The data get saved for subsequent playback with StartPlayback.")]
	public class AnimatorStopRecording : FsmStateAction
	{
		// Token: 0x0600311C RID: 12572 RVA: 0x00104924 File Offset: 0x00102B24
		public override void Reset()
		{
			this.gameObject = null;
			this.recorderStartTime = null;
			this.recorderStopTime = null;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x0010493C File Offset: 0x00102B3C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			Animator component = ownerDefaultTarget.GetComponent<Animator>();
			if (component != null)
			{
				component.StopRecording();
				this.recorderStartTime.Value = component.recorderStartTime;
				this.recorderStopTime.Value = component.recorderStopTime;
			}
			base.Finish();
		}

		// Token: 0x04002228 RID: 8744
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002229 RID: 8745
		[Tooltip("The recorder StartTime")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmFloat recorderStartTime;

		// Token: 0x0400222A RID: 8746
		[Tooltip("The recorder StopTime")]
		[UIHint(UIHint.Variable)]
		public FsmFloat recorderStopTime;
	}
}
