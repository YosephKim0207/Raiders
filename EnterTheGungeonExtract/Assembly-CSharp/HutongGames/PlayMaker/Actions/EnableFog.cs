using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093F RID: 2367
	[Tooltip("Enables/Disables Fog in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class EnableFog : FsmStateAction
	{
		// Token: 0x060033CD RID: 13261 RVA: 0x0010E410 File Offset: 0x0010C610
		public override void Reset()
		{
			this.enableFog = true;
			this.everyFrame = false;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x0010E428 File Offset: 0x0010C628
		public override void OnEnter()
		{
			RenderSettings.fog = this.enableFog.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0010E44C File Offset: 0x0010C64C
		public override void OnUpdate()
		{
			RenderSettings.fog = this.enableFog.Value;
		}

		// Token: 0x040024F1 RID: 9457
		[Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enableFog;

		// Token: 0x040024F2 RID: 9458
		[Tooltip("Repeat every frame. Useful if the Enable Fog setting is changing.")]
		public bool everyFrame;
	}
}
