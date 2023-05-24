using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE7 RID: 2791
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets the global sound volume.")]
	public class SetGameVolume : FsmStateAction
	{
		// Token: 0x06003B0C RID: 15116 RVA: 0x0012B414 File Offset: 0x00129614
		public override void Reset()
		{
			this.volume = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0012B430 File Offset: 0x00129630
		public override void OnEnter()
		{
			AudioListener.volume = this.volume.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0012B454 File Offset: 0x00129654
		public override void OnUpdate()
		{
			AudioListener.volume = this.volume.Value;
		}

		// Token: 0x04002D59 RID: 11609
		[RequiredField]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume;

		// Token: 0x04002D5A RID: 11610
		public bool everyFrame;
	}
}
