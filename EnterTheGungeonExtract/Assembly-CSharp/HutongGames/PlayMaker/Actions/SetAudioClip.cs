using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC5 RID: 2757
	[Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class SetAudioClip : ComponentAction<AudioSource>
	{
		// Token: 0x06003A6D RID: 14957 RVA: 0x001293A8 File Offset: 0x001275A8
		public override void Reset()
		{
			this.gameObject = null;
			this.audioClip = null;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x001293B8 File Offset: 0x001275B8
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.audio.clip = this.audioClip.Value as AudioClip;
			}
			base.Finish();
		}

		// Token: 0x04002C9E RID: 11422
		[Tooltip("The GameObject with the AudioSource component.")]
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C9F RID: 11423
		[Tooltip("The AudioClip to set.")]
		[ObjectType(typeof(AudioClip))]
		public FsmObject audioClip;
	}
}
