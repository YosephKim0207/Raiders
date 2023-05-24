using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A29 RID: 2601
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the IP address, port, update rate and dedicated server flag of the master server.")]
	public class MasterServerSetProperties : FsmStateAction
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x0011EA6C File Offset: 0x0011CC6C
		public override void Reset()
		{
			this.ipAddress = "127.0.0.1";
			this.port = 10002;
			this.updateRate = 60;
			this.dedicatedServer = false;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x0011EAA8 File Offset: 0x0011CCA8
		public override void OnEnter()
		{
			this.SetMasterServerProperties();
			base.Finish();
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x0011EAB8 File Offset: 0x0011CCB8
		private void SetMasterServerProperties()
		{
			MasterServer.ipAddress = this.ipAddress.Value;
			MasterServer.port = this.port.Value;
			MasterServer.updateRate = this.updateRate.Value;
			MasterServer.dedicatedServer = this.dedicatedServer.Value;
		}

		// Token: 0x040029A5 RID: 10661
		[Tooltip("Set the IP address of the master server.")]
		public FsmString ipAddress;

		// Token: 0x040029A6 RID: 10662
		[Tooltip("Set the connection port of the master server.")]
		public FsmInt port;

		// Token: 0x040029A7 RID: 10663
		[Tooltip("Set the minimum update rate for master server host information update. Default is 60 seconds.")]
		public FsmInt updateRate;

		// Token: 0x040029A8 RID: 10664
		[Tooltip("Set if this machine is a dedicated server.")]
		public FsmBool dedicatedServer;
	}
}
