using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A34 RID: 2612
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the local network player properties")]
	public class NetworkGetLocalPlayerProperties : FsmStateAction
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x0011F17C File Offset: 0x0011D37C
		public override void Reset()
		{
			this.IpAddress = null;
			this.port = null;
			this.guid = null;
			this.externalIPAddress = null;
			this.externalPort = null;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0011F1A4 File Offset: 0x0011D3A4
		public override void OnEnter()
		{
			this.IpAddress.Value = Network.player.ipAddress;
			this.port.Value = Network.player.port;
			this.guid.Value = Network.player.guid;
			this.externalIPAddress.Value = Network.player.externalIP;
			this.externalPort.Value = Network.player.externalPort;
			base.Finish();
		}

		// Token: 0x040029CA RID: 10698
		[Tooltip("The IP address of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmString IpAddress;

		// Token: 0x040029CB RID: 10699
		[UIHint(UIHint.Variable)]
		[Tooltip("The port of this player.")]
		public FsmInt port;

		// Token: 0x040029CC RID: 10700
		[UIHint(UIHint.Variable)]
		[Tooltip("The GUID for this player, used when connecting with NAT punchthrough.")]
		public FsmString guid;

		// Token: 0x040029CD RID: 10701
		[UIHint(UIHint.Variable)]
		[Tooltip("The external IP address of the network interface. This will only be populated after some external connection has been made.")]
		public FsmString externalIPAddress;

		// Token: 0x040029CE RID: 10702
		[Tooltip("Returns the external port of the network interface. This will only be populated after some external connection has been made.")]
		[UIHint(UIHint.Variable)]
		public FsmInt externalPort;
	}
}
