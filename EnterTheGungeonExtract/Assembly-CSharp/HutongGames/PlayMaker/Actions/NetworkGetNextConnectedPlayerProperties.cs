using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3A RID: 2618
	[Tooltip("Get the next connected player properties. \nEach time this action is called it gets the next child of a GameObject.This lets you quickly loop through all the connected player to perform actions on them.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetNextConnectedPlayerProperties : FsmStateAction
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x0011F740 File Offset: 0x0011D940
		public override void Reset()
		{
			this.finishedEvent = null;
			this.loopEvent = null;
			this.index = null;
			this.IpAddress = null;
			this.port = null;
			this.guid = null;
			this.externalIPAddress = null;
			this.externalPort = null;
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0011F77C File Offset: 0x0011D97C
		public override void OnEnter()
		{
			this.DoGetNextPlayerProperties();
			base.Finish();
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0011F78C File Offset: 0x0011D98C
		private void DoGetNextPlayerProperties()
		{
			if (this.nextItemIndex >= Network.connections.Length)
			{
				base.Fsm.Event(this.finishedEvent);
				this.nextItemIndex = 0;
				return;
			}
			NetworkPlayer networkPlayer = Network.connections[this.nextItemIndex];
			this.index.Value = this.nextItemIndex;
			this.IpAddress.Value = networkPlayer.ipAddress;
			this.port.Value = networkPlayer.port;
			this.guid.Value = networkPlayer.guid;
			this.externalIPAddress.Value = networkPlayer.externalIP;
			this.externalPort.Value = networkPlayer.externalPort;
			if (this.nextItemIndex >= Network.connections.Length)
			{
				base.Fsm.Event(this.finishedEvent);
				this.nextItemIndex = 0;
				return;
			}
			this.nextItemIndex++;
			if (this.loopEvent != null)
			{
				base.Fsm.Event(this.loopEvent);
			}
		}

		// Token: 0x040029E9 RID: 10729
		[Tooltip("Event to send for looping.")]
		[ActionSection("Set up")]
		public FsmEvent loopEvent;

		// Token: 0x040029EA RID: 10730
		[Tooltip("Event to send when there are no more children.")]
		public FsmEvent finishedEvent;

		// Token: 0x040029EB RID: 10731
		[Tooltip("The player connection index.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmInt index;

		// Token: 0x040029EC RID: 10732
		[Tooltip("Get the IP address of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmString IpAddress;

		// Token: 0x040029ED RID: 10733
		[Tooltip("Get the port of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;

		// Token: 0x040029EE RID: 10734
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GUID for this player, used when connecting with NAT punchthrough.")]
		public FsmString guid;

		// Token: 0x040029EF RID: 10735
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made.")]
		public FsmString externalIPAddress;

		// Token: 0x040029F0 RID: 10736
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made.")]
		public FsmInt externalPort;

		// Token: 0x040029F1 RID: 10737
		private int nextItemIndex;
	}
}
