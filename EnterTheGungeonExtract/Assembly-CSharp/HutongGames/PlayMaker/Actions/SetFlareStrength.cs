using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD3 RID: 2771
	[Tooltip("Sets the intensity of all Flares in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetFlareStrength : FsmStateAction
	{
		// Token: 0x06003AAA RID: 15018 RVA: 0x00129CEC File Offset: 0x00127EEC
		public override void Reset()
		{
			this.flareStrength = 0.2f;
			this.everyFrame = false;
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00129D08 File Offset: 0x00127F08
		public override void OnEnter()
		{
			this.DoSetFlareStrength();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00129D24 File Offset: 0x00127F24
		public override void OnUpdate()
		{
			this.DoSetFlareStrength();
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00129D2C File Offset: 0x00127F2C
		private void DoSetFlareStrength()
		{
			RenderSettings.flareStrength = this.flareStrength.Value;
		}

		// Token: 0x04002CD2 RID: 11474
		[RequiredField]
		public FsmFloat flareStrength;

		// Token: 0x04002CD3 RID: 11475
		public bool everyFrame;
	}
}
