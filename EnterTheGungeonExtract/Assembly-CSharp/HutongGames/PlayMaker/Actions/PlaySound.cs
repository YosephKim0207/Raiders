using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A86 RID: 2694
	[Tooltip("Plays an Audio Clip at a position defined by a Game Object or Vector3. If a position is defined, it takes priority over the game object. This action doesn't require an Audio Source component, but offers less control than Audio actions.")]
	[ActionCategory(ActionCategory.Audio)]
	public class PlaySound : FsmStateAction
	{
		// Token: 0x0600392E RID: 14638 RVA: 0x001251DC File Offset: 0x001233DC
		public override void Reset()
		{
			this.gameObject = null;
			this.position = new FsmVector3
			{
				UseVariable = true
			};
			this.clip = null;
			this.volume = 1f;
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0012521C File Offset: 0x0012341C
		public override void OnEnter()
		{
			this.DoPlaySound();
			base.Finish();
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0012522C File Offset: 0x0012342C
		private void DoPlaySound()
		{
			AudioClip audioClip = this.clip.Value as AudioClip;
			if (audioClip == null)
			{
				base.LogWarning("Missing Audio Clip!");
				return;
			}
			if (!this.position.IsNone)
			{
				AudioSource.PlayClipAtPoint(audioClip, this.position.Value, this.volume.Value);
			}
			else
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
				if (ownerDefaultTarget == null)
				{
					return;
				}
				AudioSource.PlayClipAtPoint(audioClip, ownerDefaultTarget.transform.position, this.volume.Value);
			}
		}

		// Token: 0x04002B80 RID: 11136
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B81 RID: 11137
		public FsmVector3 position;

		// Token: 0x04002B82 RID: 11138
		[Title("Audio Clip")]
		[RequiredField]
		[ObjectType(typeof(AudioClip))]
		public FsmObject clip;

		// Token: 0x04002B83 RID: 11139
		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume = 1f;
	}
}
