using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A30 RID: 2608
	[Tooltip("Get connected player properties.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetConnectedPlayerProperties : FsmStateAction
	{
		// Token: 0x060037BC RID: 14268 RVA: 0x0011EEE8 File Offset: 0x0011D0E8
		public override void Reset()
		{
			this.index = null;
			this.IpAddress = null;
			this.port = null;
			this.guid = null;
			this.externalIPAddress = null;
			this.externalPort = null;
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x0011EF14 File Offset: 0x0011D114
		public override void OnEnter()
		{
			this.getPlayerProperties();
			base.Finish();
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x0011EF24 File Offset: 0x0011D124
		private void getPlayerProperties()
		{
			int value = this.index.Value;
			if (value < 0 || value >= Network.connections.Length)
			{
				base.LogError("Player index out of range");
				return;
			}
			NetworkPlayer networkPlayer = Network.connections[value];
			this.IpAddress.Value = networkPlayer.ipAddress;
			this.port.Value = networkPlayer.port;
			this.guid.Value = networkPlayer.guid;
			this.externalIPAddress.Value = networkPlayer.externalIP;
			this.externalPort.Value = networkPlayer.externalPort;
		}

		// Token: 0x040029BA RID: 10682
		[Tooltip("The player connection index.")]
		[RequiredField]
		public FsmInt index;

		// Token: 0x040029BB RID: 10683
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the IP address of this player.")]
		[ActionSection("Result")]
		public FsmString IpAddress;

		// Token: 0x040029BC RID: 10684
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the port of this player.")]
		public FsmInt port;

		// Token: 0x040029BD RID: 10685
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GUID for this player, used when connecting with NAT punchthrough.")]
		public FsmString guid;

		// Token: 0x040029BE RID: 10686
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made.")]
		public FsmString externalIPAddress;

		// Token: 0x040029BF RID: 10687
		[Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made.")]
		[UIHint(UIHint.Variable)]
		public FsmInt externalPort;
	}
}
