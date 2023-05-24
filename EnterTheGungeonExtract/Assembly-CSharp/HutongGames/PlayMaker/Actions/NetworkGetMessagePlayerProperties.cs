using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A36 RID: 2614
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the network OnPlayerConnected or OnPlayerDisConnected message player info.")]
	public class NetworkGetMessagePlayerProperties : FsmStateAction
	{
		// Token: 0x060037D2 RID: 14290 RVA: 0x0011F264 File Offset: 0x0011D464
		public override void Reset()
		{
			this.IpAddress = null;
			this.port = null;
			this.guid = null;
			this.externalIPAddress = null;
			this.externalPort = null;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0011F28C File Offset: 0x0011D48C
		public override void OnEnter()
		{
			this.doGetOnPLayerConnectedProperties();
			base.Finish();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0011F29C File Offset: 0x0011D49C
		private void doGetOnPLayerConnectedProperties()
		{
			NetworkPlayer player = Fsm.EventData.Player;
			Debug.Log("hello " + player.ipAddress);
			this.IpAddress.Value = player.ipAddress;
			this.port.Value = player.port;
			this.guid.Value = player.guid;
			this.externalIPAddress.Value = player.externalIP;
			this.externalPort.Value = player.externalPort;
			base.Finish();
		}

		// Token: 0x040029D0 RID: 10704
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the IP address of this connected player.")]
		public FsmString IpAddress;

		// Token: 0x040029D1 RID: 10705
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the port of this connected player.")]
		public FsmInt port;

		// Token: 0x040029D2 RID: 10706
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GUID for this connected player, used when connecting with NAT punchthrough.")]
		public FsmString guid;

		// Token: 0x040029D3 RID: 10707
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made.")]
		public FsmString externalIPAddress;

		// Token: 0x040029D4 RID: 10708
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made.")]
		public FsmInt externalPort;
	}
}
