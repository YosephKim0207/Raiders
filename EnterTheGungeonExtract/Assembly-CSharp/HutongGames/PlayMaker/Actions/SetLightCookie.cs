using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFA RID: 2810
	[Tooltip("Sets the Texture projected by a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightCookie : ComponentAction<Light>
	{
		// Token: 0x06003B57 RID: 15191 RVA: 0x0012BCBC File Offset: 0x00129EBC
		public override void Reset()
		{
			this.gameObject = null;
			this.lightCookie = null;
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0012BCCC File Offset: 0x00129ECC
		public override void OnEnter()
		{
			this.DoSetLightCookie();
			base.Finish();
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0012BCDC File Offset: 0x00129EDC
		private void DoSetLightCookie()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.cookie = this.lightCookie.Value;
			}
		}

		// Token: 0x04002D87 RID: 11655
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D88 RID: 11656
		public FsmTexture lightCookie;
	}
}
