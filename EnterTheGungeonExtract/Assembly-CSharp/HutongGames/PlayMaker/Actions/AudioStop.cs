using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F4 RID: 2292
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Stops playing the Audio Clip played by an Audio Source component on a Game Object.")]
	public class AudioStop : FsmStateAction
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x00109CFC File Offset: 0x00107EFC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00109D08 File Offset: 0x00107F08
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
				if (component != null)
				{
					component.Stop();
				}
			}
			base.Finish();
		}

		// Token: 0x040023B7 RID: 9143
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		[Tooltip("The GameObject with an AudioSource component.")]
		public FsmOwnerDefault gameObject;
	}
}
