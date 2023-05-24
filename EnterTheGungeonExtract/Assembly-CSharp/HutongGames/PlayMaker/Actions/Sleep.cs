using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1E RID: 2846
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Forces a Game Object's Rigid Body to Sleep at least one frame.")]
	public class Sleep : ComponentAction<Rigidbody>
	{
		// Token: 0x06003BF9 RID: 15353 RVA: 0x0012DE64 File Offset: 0x0012C064
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0012DE70 File Offset: 0x0012C070
		public override void OnEnter()
		{
			this.DoSleep();
			base.Finish();
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0012DE80 File Offset: 0x0012C080
		private void DoSleep()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.Sleep();
			}
		}

		// Token: 0x04002E17 RID: 11799
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
	}
}
