using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A26 RID: 2598
	[Tooltip("Get the IP address, port, update rate and dedicated server flag of the master server and store in variables.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerGetProperties : FsmStateAction
	{
		// Token: 0x06003796 RID: 14230 RVA: 0x0011E8D4 File Offset: 0x0011CAD4
		public override void Reset()
		{
			this.ipAddress = null;
			this.port = null;
			this.updateRate = null;
			this.dedicatedServer = null;
			this.isDedicatedServerEvent = null;
			this.isNotDedicatedServerEvent = null;
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x0011E900 File Offset: 0x0011CB00
		public override void OnEnter()
		{
			this.GetMasterServerProperties();
			base.Finish();
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0011E910 File Offset: 0x0011CB10
		private void GetMasterServerProperties()
		{
			this.ipAddress.Value = MasterServer.ipAddress;
			this.port.Value = MasterServer.port;
			this.updateRate.Value = MasterServer.updateRate;
			bool flag = MasterServer.dedicatedServer;
			this.dedicatedServer.Value = flag;
			if (flag && this.isDedicatedServerEvent != null)
			{
				base.Fsm.Event(this.isDedicatedServerEvent);
			}
			if (!flag && this.isNotDedicatedServerEvent != null)
			{
				base.Fsm.Event(this.isNotDedicatedServerEvent);
			}
		}

		// Token: 0x0400299A RID: 10650
		[UIHint(UIHint.Variable)]
		[Tooltip("The IP address of the master server.")]
		public FsmString ipAddress;

		// Token: 0x0400299B RID: 10651
		[UIHint(UIHint.Variable)]
		[Tooltip("The connection port of the master server.")]
		public FsmInt port;

		// Token: 0x0400299C RID: 10652
		[Tooltip("The minimum update rate for master server host information update. Default is 60 seconds")]
		[UIHint(UIHint.Variable)]
		public FsmInt updateRate;

		// Token: 0x0400299D RID: 10653
		[UIHint(UIHint.Variable)]
		[Tooltip("Flag to report if this machine is a dedicated server.")]
		public FsmBool dedicatedServer;

		// Token: 0x0400299E RID: 10654
		[Tooltip("Event sent if this machine is a dedicated server")]
		public FsmEvent isDedicatedServerEvent;

		// Token: 0x0400299F RID: 10655
		[Tooltip("Event sent if this machine is not a dedicated server")]
		public FsmEvent isNotDedicatedServerEvent;
	}
}
