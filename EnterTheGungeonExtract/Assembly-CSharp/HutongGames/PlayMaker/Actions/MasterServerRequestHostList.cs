using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A28 RID: 2600
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Request a host list from the master server.\n\nUse MasterServer Get Host Data to get info on each host in the host list.")]
	public class MasterServerRequestHostList : FsmStateAction
	{
		// Token: 0x0600379E RID: 14238 RVA: 0x0011EA04 File Offset: 0x0011CC04
		public override void Reset()
		{
			this.gameTypeName = null;
			this.HostListArrivedEvent = null;
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0011EA14 File Offset: 0x0011CC14
		public override void OnEnter()
		{
			this.DoMasterServerRequestHost();
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x0011EA1C File Offset: 0x0011CC1C
		public override void OnUpdate()
		{
			this.WatchServerRequestHost();
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x0011EA24 File Offset: 0x0011CC24
		private void DoMasterServerRequestHost()
		{
			MasterServer.ClearHostList();
			MasterServer.RequestHostList(this.gameTypeName.Value);
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x0011EA3C File Offset: 0x0011CC3C
		private void WatchServerRequestHost()
		{
			if (MasterServer.PollHostList().Length != 0)
			{
				base.Fsm.Event(this.HostListArrivedEvent);
				base.Finish();
			}
		}

		// Token: 0x040029A3 RID: 10659
		[Tooltip("The unique game type name.")]
		[RequiredField]
		public FsmString gameTypeName;

		// Token: 0x040029A4 RID: 10660
		[Tooltip("Event sent when the host list has arrived. NOTE: The action will not Finish until the host list arrives.")]
		public FsmEvent HostListArrivedEvent;
	}
}
