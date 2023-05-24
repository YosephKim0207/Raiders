using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089C RID: 2204
	[Tooltip("Sets the animator in recording mode, and allocates a circular buffer of size frameCount. After this call, the recorder starts collecting up to frameCount frames in the buffer. Note it is not possible to start playback until a call to StopRecording is made")]
	[ActionCategory(ActionCategory.Animator)]
	public class AnimatorStartRecording : FsmStateAction
	{
		// Token: 0x06003116 RID: 12566 RVA: 0x00104840 File Offset: 0x00102A40
		public override void Reset()
		{
			this.gameObject = null;
			this.frameCount = 0;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x00104858 File Offset: 0x00102A58
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
				component.StartRecording(this.frameCount.Value);
			}
			base.Finish();
		}

		// Token: 0x04002225 RID: 8741
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002226 RID: 8742
		[Tooltip("The number of frames (updates) that will be recorded. If frameCount is 0, the recording will continue until the user calls StopRecording. The maximum value for frameCount is 10000.")]
		[RequiredField]
		public FsmInt frameCount;
	}
}
