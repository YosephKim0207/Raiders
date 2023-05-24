using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFE RID: 2814
	[Tooltip("Sets the Spot Angle of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightSpotAngle : ComponentAction<Light>
	{
		// Token: 0x06003B69 RID: 15209 RVA: 0x0012BEAC File Offset: 0x0012A0AC
		public override void Reset()
		{
			this.gameObject = null;
			this.lightSpotAngle = 20f;
			this.everyFrame = false;
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0012BECC File Offset: 0x0012A0CC
		public override void OnEnter()
		{
			this.DoSetLightRange();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0012BEE8 File Offset: 0x0012A0E8
		public override void OnUpdate()
		{
			this.DoSetLightRange();
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0012BEF0 File Offset: 0x0012A0F0
		private void DoSetLightRange()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.spotAngle = this.lightSpotAngle.Value;
			}
		}

		// Token: 0x04002D91 RID: 11665
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D92 RID: 11666
		public FsmFloat lightSpotAngle;

		// Token: 0x04002D93 RID: 11667
		public bool everyFrame;
	}
}
