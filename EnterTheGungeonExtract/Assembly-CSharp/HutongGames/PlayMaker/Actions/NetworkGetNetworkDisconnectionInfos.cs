using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A38 RID: 2616
	[Tooltip("Get the network OnDisconnectedFromServer.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetNetworkDisconnectionInfos : FsmStateAction
	{
		// Token: 0x060037D9 RID: 14297 RVA: 0x0011F360 File Offset: 0x0011D560
		public override void Reset()
		{
			this.disconnectionLabel = null;
			this.lostConnectionEvent = null;
			this.disConnectedEvent = null;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x0011F378 File Offset: 0x0011D578
		public override void OnEnter()
		{
			this.doGetNetworkDisconnectionInfo();
			base.Finish();
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x0011F388 File Offset: 0x0011D588
		private void doGetNetworkDisconnectionInfo()
		{
			NetworkDisconnection disconnectionInfo = Fsm.EventData.DisconnectionInfo;
			this.disconnectionLabel.Value = disconnectionInfo.ToString();
			if (disconnectionInfo != NetworkDisconnection.Disconnected)
			{
				if (disconnectionInfo == NetworkDisconnection.LostConnection)
				{
					if (this.lostConnectionEvent != null)
					{
						base.Fsm.Event(this.lostConnectionEvent);
					}
				}
			}
			else if (this.disConnectedEvent != null)
			{
				base.Fsm.Event(this.disConnectedEvent);
			}
		}

		// Token: 0x040029D6 RID: 10710
		[UIHint(UIHint.Variable)]
		[Tooltip("Disconnection label")]
		public FsmString disconnectionLabel;

		// Token: 0x040029D7 RID: 10711
		[Tooltip("The connection to the system has been lost, no reliable packets could be delivered.")]
		public FsmEvent lostConnectionEvent;

		// Token: 0x040029D8 RID: 10712
		[Tooltip("The connection to the system has been closed.")]
		public FsmEvent disConnectedEvent;
	}
}
