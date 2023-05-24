using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B13 RID: 2835
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the strength of the shadows cast by a Light.")]
	public class SetShadowStrength : ComponentAction<Light>
	{
		// Token: 0x06003BC4 RID: 15300 RVA: 0x0012D37C File Offset: 0x0012B57C
		public override void Reset()
		{
			this.gameObject = null;
			this.shadowStrength = 0.8f;
			this.everyFrame = false;
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x0012D39C File Offset: 0x0012B59C
		public override void OnEnter()
		{
			this.DoSetShadowStrength();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x0012D3B8 File Offset: 0x0012B5B8
		public override void OnUpdate()
		{
			this.DoSetShadowStrength();
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x0012D3C0 File Offset: 0x0012B5C0
		private void DoSetShadowStrength()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.shadowStrength = this.shadowStrength.Value;
			}
		}

		// Token: 0x04002DE8 RID: 11752
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DE9 RID: 11753
		public FsmFloat shadowStrength;

		// Token: 0x04002DEA RID: 11754
		public bool everyFrame;
	}
}
