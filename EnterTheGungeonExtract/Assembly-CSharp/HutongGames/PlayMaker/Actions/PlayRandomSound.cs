using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A85 RID: 2693
	[Tooltip("Plays a Random Audio Clip at a position defined by a Game Object or a Vector3. If a position is defined, it takes priority over the game object. You can set the relative weight of the clips to control how often they are selected.")]
	[ActionCategory(ActionCategory.Audio)]
	public class PlayRandomSound : FsmStateAction
	{
		// Token: 0x0600392A RID: 14634 RVA: 0x0012508C File Offset: 0x0012328C
		public override void Reset()
		{
			this.gameObject = null;
			this.position = new FsmVector3
			{
				UseVariable = true
			};
			this.audioClips = new AudioClip[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.volume = 1f;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x00125104 File Offset: 0x00123304
		public override void OnEnter()
		{
			this.DoPlayRandomClip();
			base.Finish();
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00125114 File Offset: 0x00123314
		private void DoPlayRandomClip()
		{
			if (this.audioClips.Length == 0)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				AudioClip audioClip = this.audioClips[randomWeightedIndex];
				if (audioClip != null)
				{
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
			}
		}

		// Token: 0x04002B7B RID: 11131
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B7C RID: 11132
		public FsmVector3 position;

		// Token: 0x04002B7D RID: 11133
		[CompoundArray("Audio Clips", "Audio Clip", "Weight")]
		public AudioClip[] audioClips;

		// Token: 0x04002B7E RID: 11134
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002B7F RID: 11135
		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume = 1f;
	}
}
