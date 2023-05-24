using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F3 RID: 2291
	[ActionTarget(typeof(AudioSource), "gameObject", false)]
	[ActionTarget(typeof(AudioClip), "oneShotClip", false)]
	[Tooltip("Plays the Audio Clip set with Set Audio Clip or in the Audio Source inspector on a Game Object. Optionally plays a one shot Audio Clip.")]
	[ActionCategory(ActionCategory.Audio)]
	public class AudioPlay : FsmStateAction
	{
		// Token: 0x06003291 RID: 12945 RVA: 0x00109B5C File Offset: 0x00107D5C
		public override void Reset()
		{
			this.gameObject = null;
			this.volume = 1f;
			this.oneShotClip = null;
			this.finishedEvent = null;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00109B84 File Offset: 0x00107D84
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				this.audio = ownerDefaultTarget.GetComponent<AudioSource>();
				if (this.audio != null)
				{
					AudioClip audioClip = this.oneShotClip.Value as AudioClip;
					if (audioClip == null)
					{
						this.audio.Play();
						if (!this.volume.IsNone)
						{
							this.audio.volume = this.volume.Value;
						}
						return;
					}
					if (!this.volume.IsNone)
					{
						this.audio.PlayOneShot(audioClip, this.volume.Value);
					}
					else
					{
						this.audio.PlayOneShot(audioClip);
					}
					return;
				}
			}
			base.Finish();
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00109C5C File Offset: 0x00107E5C
		public override void OnUpdate()
		{
			if (this.audio == null)
			{
				base.Finish();
			}
			else if (!this.audio.isPlaying)
			{
				base.Fsm.Event(this.finishedEvent);
				base.Finish();
			}
			else if (!this.volume.IsNone && this.volume.Value != this.audio.volume)
			{
				this.audio.volume = this.volume.Value;
			}
		}

		// Token: 0x040023B2 RID: 9138
		[Tooltip("The GameObject with an AudioSource component.")]
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023B3 RID: 9139
		[Tooltip("Set the volume.")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume;

		// Token: 0x040023B4 RID: 9140
		[Tooltip("Optionally play a 'one shot' AudioClip. NOTE: Volume cannot be adjusted while playing a 'one shot' AudioClip.")]
		[ObjectType(typeof(AudioClip))]
		public FsmObject oneShotClip;

		// Token: 0x040023B5 RID: 9141
		[Tooltip("Event to send when the AudioClip finishes playing.")]
		public FsmEvent finishedEvent;

		// Token: 0x040023B6 RID: 9142
		private AudioSource audio;
	}
}
