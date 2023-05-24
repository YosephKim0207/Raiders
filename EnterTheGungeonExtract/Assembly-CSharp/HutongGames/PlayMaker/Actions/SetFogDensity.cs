using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD6 RID: 2774
	[Tooltip("Sets the density of the Fog in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetFogDensity : FsmStateAction
	{
		// Token: 0x06003AB8 RID: 15032 RVA: 0x00129E08 File Offset: 0x00128008
		public override void Reset()
		{
			this.fogDensity = 0.5f;
			this.everyFrame = false;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00129E24 File Offset: 0x00128024
		public override void OnEnter()
		{
			this.DoSetFogDensity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00129E40 File Offset: 0x00128040
		public override void OnUpdate()
		{
			this.DoSetFogDensity();
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x00129E48 File Offset: 0x00128048
		private void DoSetFogDensity()
		{
			RenderSettings.fogDensity = this.fogDensity.Value;
		}

		// Token: 0x04002CD9 RID: 11481
		[RequiredField]
		public FsmFloat fogDensity;

		// Token: 0x04002CDA RID: 11482
		public bool everyFrame;
	}
}
