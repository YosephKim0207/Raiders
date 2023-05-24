using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A22 RID: 2594
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Clear the host list which was received by MasterServer Request Host List")]
	public class MasterServerClearHostList : FsmStateAction
	{
		// Token: 0x0600378A RID: 14218 RVA: 0x0011E540 File Offset: 0x0011C740
		public override void OnEnter()
		{
			MasterServer.ClearHostList();
			base.Finish();
		}
	}
}
