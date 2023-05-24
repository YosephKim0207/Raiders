using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFF RID: 2815
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Set Spot, Directional, or Point Light type.")]
	public class SetLightType : ComponentAction<Light>
	{
		// Token: 0x06003B6E RID: 15214 RVA: 0x0012BF3C File Offset: 0x0012A13C
		public override void Reset()
		{
			this.gameObject = null;
			this.lightType = LightType.Point;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0012BF58 File Offset: 0x0012A158
		public override void OnEnter()
		{
			this.DoSetLightType();
			base.Finish();
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x0012BF68 File Offset: 0x0012A168
		private void DoSetLightType()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.type = (LightType)this.lightType.Value;
			}
		}

		// Token: 0x04002D94 RID: 11668
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D95 RID: 11669
		[ObjectType(typeof(LightType))]
		public FsmEnum lightType;
	}
}
