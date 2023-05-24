using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD5 RID: 2773
	[Tooltip("Sets the color of the Fog in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetFogColor : FsmStateAction
	{
		// Token: 0x06003AB3 RID: 15027 RVA: 0x00129DAC File Offset: 0x00127FAC
		public override void Reset()
		{
			this.fogColor = Color.white;
			this.everyFrame = false;
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00129DC8 File Offset: 0x00127FC8
		public override void OnEnter()
		{
			this.DoSetFogColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00129DE4 File Offset: 0x00127FE4
		public override void OnUpdate()
		{
			this.DoSetFogColor();
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00129DEC File Offset: 0x00127FEC
		private void DoSetFogColor()
		{
			RenderSettings.fogColor = this.fogColor.Value;
		}

		// Token: 0x04002CD7 RID: 11479
		[RequiredField]
		public FsmColor fogColor;

		// Token: 0x04002CD8 RID: 11480
		public bool everyFrame;
	}
}
