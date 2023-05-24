using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF3 RID: 2803
	[Tooltip("Sets the size of light halos.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetHaloStrength : FsmStateAction
	{
		// Token: 0x06003B3A RID: 15162 RVA: 0x0012B9A0 File Offset: 0x00129BA0
		public override void Reset()
		{
			this.haloStrength = 0.5f;
			this.everyFrame = false;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0012B9BC File Offset: 0x00129BBC
		public override void OnEnter()
		{
			this.DoSetHaloStrength();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x0012B9D8 File Offset: 0x00129BD8
		public override void OnUpdate()
		{
			this.DoSetHaloStrength();
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x0012B9E0 File Offset: 0x00129BE0
		private void DoSetHaloStrength()
		{
			RenderSettings.haloStrength = this.haloStrength.Value;
		}

		// Token: 0x04002D76 RID: 11638
		[RequiredField]
		public FsmFloat haloStrength;

		// Token: 0x04002D77 RID: 11639
		public bool everyFrame;
	}
}
