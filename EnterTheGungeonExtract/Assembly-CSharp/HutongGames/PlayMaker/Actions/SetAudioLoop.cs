using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC6 RID: 2758
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets looping on the AudioSource component on a Game Object.")]
	public class SetAudioLoop : ComponentAction<AudioSource>
	{
		// Token: 0x06003A70 RID: 14960 RVA: 0x0012940C File Offset: 0x0012760C
		public override void Reset()
		{
			this.gameObject = null;
			this.loop = false;
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00129424 File Offset: 0x00127624
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.audio.loop = this.loop.Value;
			}
			base.Finish();
		}

		// Token: 0x04002CA0 RID: 11424
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CA1 RID: 11425
		public FsmBool loop;
	}
}
