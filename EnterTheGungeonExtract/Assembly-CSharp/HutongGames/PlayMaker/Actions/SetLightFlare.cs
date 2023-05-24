using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AFB RID: 2811
	[Tooltip("Sets the Flare effect used by a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightFlare : ComponentAction<Light>
	{
		// Token: 0x06003B5B RID: 15195 RVA: 0x0012BD28 File Offset: 0x00129F28
		public override void Reset()
		{
			this.gameObject = null;
			this.lightFlare = null;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0012BD38 File Offset: 0x00129F38
		public override void OnEnter()
		{
			this.DoSetLightRange();
			base.Finish();
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0012BD48 File Offset: 0x00129F48
		private void DoSetLightRange()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.light.flare = this.lightFlare;
			}
		}

		// Token: 0x04002D89 RID: 11657
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D8A RID: 11658
		public Flare lightFlare;
	}
}
