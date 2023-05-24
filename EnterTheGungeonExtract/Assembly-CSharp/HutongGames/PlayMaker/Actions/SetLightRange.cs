using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFD RID: 2813
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Range of a Light.")]
	public class SetLightRange : ComponentAction<Light>
	{
		// Token: 0x06003B64 RID: 15204 RVA: 0x0012BE1C File Offset: 0x0012A01C
		public override void Reset()
		{
			this.gameObject = null;
			this.lightRange = 20f;
			this.everyFrame = false;
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0012BE3C File Offset: 0x0012A03C
		public override void OnEnter()
		{
			this.DoSetLightRange();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0012BE58 File Offset: 0x0012A058
		public override void OnUpdate()
		{
			this.DoSetLightRange();
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0012BE60 File Offset: 0x0012A060
		private void DoSetLightRange()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.range = this.lightRange.Value;
			}
		}

		// Token: 0x04002D8E RID: 11662
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D8F RID: 11663
		public FsmFloat lightRange;

		// Token: 0x04002D90 RID: 11664
		public bool everyFrame;
	}
}
