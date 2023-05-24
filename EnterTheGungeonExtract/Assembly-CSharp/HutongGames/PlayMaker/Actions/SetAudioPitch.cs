using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC7 RID: 2759
	[Tooltip("Sets the Pitch of the Audio Clip played by the AudioSource component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class SetAudioPitch : ComponentAction<AudioSource>
	{
		// Token: 0x06003A73 RID: 14963 RVA: 0x00129474 File Offset: 0x00127674
		public override void Reset()
		{
			this.gameObject = null;
			this.pitch = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00129494 File Offset: 0x00127694
		public override void OnEnter()
		{
			this.DoSetAudioPitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x001294B0 File Offset: 0x001276B0
		public override void OnUpdate()
		{
			this.DoSetAudioPitch();
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x001294B8 File Offset: 0x001276B8
		private void DoSetAudioPitch()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget) && !this.pitch.IsNone)
			{
				base.audio.pitch = this.pitch.Value;
			}
		}

		// Token: 0x04002CA2 RID: 11426
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CA3 RID: 11427
		public FsmFloat pitch;

		// Token: 0x04002CA4 RID: 11428
		public bool everyFrame;
	}
}
