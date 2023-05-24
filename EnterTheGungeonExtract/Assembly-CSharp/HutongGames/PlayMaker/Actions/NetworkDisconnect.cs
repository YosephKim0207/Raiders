using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2E RID: 2606
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Disconnect from the server.")]
	public class NetworkDisconnect : FsmStateAction
	{
		// Token: 0x060037B5 RID: 14261 RVA: 0x0011ED94 File Offset: 0x0011CF94
		public override void OnEnter()
		{
			Network.Disconnect();
			base.Finish();
		}
	}
}
