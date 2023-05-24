using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A4A RID: 2634
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Remove the RPC function calls accociated with a Game Object.\n\nNOTE: The Game Object must have a NetworkView component attached.")]
	public class NetworkViewRemoveRPCs : ComponentAction<NetworkView>
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x001200C4 File Offset: 0x0011E2C4
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x001200D0 File Offset: 0x0011E2D0
		public override void OnEnter()
		{
			this.DoRemoveRPCsFromViewID();
			base.Finish();
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x001200E0 File Offset: 0x0011E2E0
		private void DoRemoveRPCsFromViewID()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				Network.RemoveRPCs(base.networkView.viewID);
			}
		}

		// Token: 0x04002A1B RID: 10779
		[Tooltip("Remove the RPC function calls accociated with this Game Object.\n\nNOTE: The GameObject must have a NetworkView component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		public FsmOwnerDefault gameObject;
	}
}
