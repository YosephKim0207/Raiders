using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC8 RID: 2760
	[Tooltip("Sets the Volume of the Audio Clip played by the AudioSource component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class SetAudioVolume : ComponentAction<AudioSource>
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x00129514 File Offset: 0x00127714
		public override void Reset()
		{
			this.gameObject = null;
			this.volume = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00129534 File Offset: 0x00127734
		public override void OnEnter()
		{
			this.DoSetAudioVolume();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00129550 File Offset: 0x00127750
		public override void OnUpdate()
		{
			this.DoSetAudioVolume();
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00129558 File Offset: 0x00127758
		private void DoSetAudioVolume()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget) && !this.volume.IsNone)
			{
				base.audio.volume = this.volume.Value;
			}
		}

		// Token: 0x04002CA5 RID: 11429
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CA6 RID: 11430
		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume;

		// Token: 0x04002CA7 RID: 11431
		public bool everyFrame;
	}
}
