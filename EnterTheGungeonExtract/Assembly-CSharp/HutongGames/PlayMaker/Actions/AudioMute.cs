using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F1 RID: 2289
	[Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class AudioMute : FsmStateAction
	{
		// Token: 0x0600328B RID: 12939 RVA: 0x00109A84 File Offset: 0x00107C84
		public override void Reset()
		{
			this.gameObject = null;
			this.mute = false;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x00109A9C File Offset: 0x00107C9C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
				if (component != null)
				{
					component.mute = this.mute.Value;
				}
			}
			base.Finish();
		}

		// Token: 0x040023AF RID: 9135
		[RequiredField]
		[Tooltip("The GameObject with an Audio Source component.")]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023B0 RID: 9136
		[Tooltip("Check to mute, uncheck to unmute.")]
		[RequiredField]
		public FsmBool mute;
	}
}
