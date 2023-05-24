using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2A RID: 2602
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Unregister this server from the master server.\n\nDoes nothing if the server is not registered or has already unregistered.")]
	public class MasterServerUnregisterHost : FsmStateAction
	{
		// Token: 0x060037A8 RID: 14248 RVA: 0x0011EB10 File Offset: 0x0011CD10
		public override void OnEnter()
		{
			MasterServer.UnregisterHost();
			base.Finish();
		}
	}
}
