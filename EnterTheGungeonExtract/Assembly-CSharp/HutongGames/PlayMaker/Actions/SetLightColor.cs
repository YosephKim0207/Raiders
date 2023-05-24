using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF9 RID: 2809
	[Tooltip("Sets the Color of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightColor : ComponentAction<Light>
	{
		// Token: 0x06003B52 RID: 15186 RVA: 0x0012BC2C File Offset: 0x00129E2C
		public override void Reset()
		{
			this.gameObject = null;
			this.lightColor = Color.white;
			this.everyFrame = false;
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0012BC4C File Offset: 0x00129E4C
		public override void OnEnter()
		{
			this.DoSetLightColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x0012BC68 File Offset: 0x00129E68
		public override void OnUpdate()
		{
			this.DoSetLightColor();
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x0012BC70 File Offset: 0x00129E70
		private void DoSetLightColor()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.color = this.lightColor.Value;
			}
		}

		// Token: 0x04002D84 RID: 11652
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D85 RID: 11653
		[RequiredField]
		public FsmColor lightColor;

		// Token: 0x04002D86 RID: 11654
		public bool everyFrame;
	}
}
