using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B14 RID: 2836
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the global Skybox.")]
	public class SetSkybox : FsmStateAction
	{
		// Token: 0x06003BC9 RID: 15305 RVA: 0x0012D40C File Offset: 0x0012B60C
		public override void Reset()
		{
			this.skybox = null;
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x0012D418 File Offset: 0x0012B618
		public override void OnEnter()
		{
			RenderSettings.skybox = this.skybox.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x0012D43C File Offset: 0x0012B63C
		public override void OnUpdate()
		{
			RenderSettings.skybox = this.skybox.Value;
		}

		// Token: 0x04002DEB RID: 11755
		public FsmMaterial skybox;

		// Token: 0x04002DEC RID: 11756
		[Tooltip("Repeat every frame. Useful if the Skybox is changing.")]
		public bool everyFrame;
	}
}
