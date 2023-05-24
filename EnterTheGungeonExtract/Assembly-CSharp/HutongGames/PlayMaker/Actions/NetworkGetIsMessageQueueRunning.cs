using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A32 RID: 2610
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get if network messages are enabled or disabled.\n\nIf disabled no RPC call execution or network view synchronization takes place")]
	public class NetworkGetIsMessageQueueRunning : FsmStateAction
	{
		// Token: 0x060037C4 RID: 14276 RVA: 0x0011F024 File Offset: 0x0011D224
		public override void Reset()
		{
			this.result = null;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0011F030 File Offset: 0x0011D230
		public override void OnEnter()
		{
			this.result.Value = Network.isMessageQueueRunning;
			base.Finish();
		}

		// Token: 0x040029C2 RID: 10690
		[Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;
	}
}
