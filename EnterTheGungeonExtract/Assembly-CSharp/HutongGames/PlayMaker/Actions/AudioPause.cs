using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F2 RID: 2290
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Pauses playing the Audio Clip played by an Audio Source component on a Game Object.")]
	public class AudioPause : FsmStateAction
	{
		// Token: 0x0600328E RID: 12942 RVA: 0x00109AFC File Offset: 0x00107CFC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00109B08 File Offset: 0x00107D08
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
				if (component != null)
				{
					component.Pause();
				}
			}
			base.Finish();
		}

		// Token: 0x040023B1 RID: 9137
		[Tooltip("The GameObject with an Audio Source component.")]
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;
	}
}
