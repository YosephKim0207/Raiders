using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A43 RID: 2627
	[Tooltip("Enable or disable the processing of network messages.\n\nIf this is disabled no RPC call execution or network view synchronization takes place.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkSetIsMessageQueueRunning : FsmStateAction
	{
		// Token: 0x06003802 RID: 14338 RVA: 0x0011FE50 File Offset: 0x0011E050
		public override void Reset()
		{
			this.isMessageQueueRunning = null;
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x0011FE5C File Offset: 0x0011E05C
		public override void OnEnter()
		{
			Network.isMessageQueueRunning = this.isMessageQueueRunning.Value;
			base.Finish();
		}

		// Token: 0x04002A10 RID: 10768
		[Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		public FsmBool isMessageQueueRunning;
	}
}
