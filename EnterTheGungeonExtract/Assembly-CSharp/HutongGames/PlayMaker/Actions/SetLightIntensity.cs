using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFC RID: 2812
	[Tooltip("Sets the Intensity of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightIntensity : ComponentAction<Light>
	{
		// Token: 0x06003B5F RID: 15199 RVA: 0x0012BD8C File Offset: 0x00129F8C
		public override void Reset()
		{
			this.gameObject = null;
			this.lightIntensity = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0012BDAC File Offset: 0x00129FAC
		public override void OnEnter()
		{
			this.DoSetLightIntensity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0012BDC8 File Offset: 0x00129FC8
		public override void OnUpdate()
		{
			this.DoSetLightIntensity();
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x0012BDD0 File Offset: 0x00129FD0
		private void DoSetLightIntensity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.intensity = this.lightIntensity.Value;
			}
		}

		// Token: 0x04002D8B RID: 11659
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D8C RID: 11660
		public FsmFloat lightIntensity;

		// Token: 0x04002D8D RID: 11661
		public bool everyFrame;
	}
}
