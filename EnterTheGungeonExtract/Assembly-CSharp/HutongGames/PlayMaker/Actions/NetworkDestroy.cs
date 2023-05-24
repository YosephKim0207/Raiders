using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2D RID: 2605
	[Tooltip("Destroy the object across the network.\n\nThe object is destroyed locally and remotely.\n\nOptionally remove any RPCs accociated with the object.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkDestroy : ComponentAction<NetworkView>
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x0011ED08 File Offset: 0x0011CF08
		public override void Reset()
		{
			this.gameObject = null;
			this.removeRPCs = true;
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0011ED20 File Offset: 0x0011CF20
		public override void OnEnter()
		{
			this.DoDestroy();
			base.Finish();
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x0011ED30 File Offset: 0x0011CF30
		private void DoDestroy()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (this.removeRPCs.Value)
			{
				Network.RemoveRPCs(base.networkView.owner);
			}
			Network.DestroyPlayerObjects(base.networkView.owner);
		}

		// Token: 0x040029B1 RID: 10673
		[Tooltip("The Game Object to destroy.\nNOTE: The Game Object must have a NetworkView attached.")]
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040029B2 RID: 10674
		[Tooltip("Remove all RPC calls associated with the Game Object.")]
		public FsmBool removeRPCs;
	}
}
