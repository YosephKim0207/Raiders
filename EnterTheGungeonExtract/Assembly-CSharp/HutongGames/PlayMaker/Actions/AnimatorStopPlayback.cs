using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089D RID: 2205
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Stops the animator playback mode. When playback stops, the avatar resumes getting control from game logic")]
	public class AnimatorStopPlayback : FsmStateAction
	{
		// Token: 0x06003119 RID: 12569 RVA: 0x001048BC File Offset: 0x00102ABC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x001048C8 File Offset: 0x00102AC8
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
				component.StopPlayback();
			}
			base.Finish();
		}

		// Token: 0x04002227 RID: 8743
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
