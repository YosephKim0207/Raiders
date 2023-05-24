using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A42 RID: 2626
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Send Events based on the status of the network interface peer type: Disconneced, Server, Client, Connecting.")]
	public class NetworkPeerTypeSwitch : FsmStateAction
	{
		// Token: 0x060037FD RID: 14333 RVA: 0x0011FD74 File Offset: 0x0011DF74
		public override void Reset()
		{
			this.isDisconnected = null;
			this.isServer = null;
			this.isClient = null;
			this.isConnecting = null;
			this.everyFrame = false;
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x0011FD9C File Offset: 0x0011DF9C
		public override void OnEnter()
		{
			this.DoNetworkPeerTypeSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0011FDB8 File Offset: 0x0011DFB8
		public override void OnUpdate()
		{
			this.DoNetworkPeerTypeSwitch();
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0011FDC0 File Offset: 0x0011DFC0
		private void DoNetworkPeerTypeSwitch()
		{
			switch (Network.peerType)
			{
			case NetworkPeerType.Disconnected:
				base.Fsm.Event(this.isDisconnected);
				break;
			case NetworkPeerType.Server:
				base.Fsm.Event(this.isServer);
				break;
			case NetworkPeerType.Client:
				base.Fsm.Event(this.isClient);
				break;
			case NetworkPeerType.Connecting:
				base.Fsm.Event(this.isConnecting);
				break;
			}
		}

		// Token: 0x04002A0B RID: 10763
		[Tooltip("Event to send if no client connection running. Server not initialized.")]
		public FsmEvent isDisconnected;

		// Token: 0x04002A0C RID: 10764
		[Tooltip("Event to send if running as server.")]
		public FsmEvent isServer;

		// Token: 0x04002A0D RID: 10765
		[Tooltip("Event to send if running as client.")]
		public FsmEvent isClient;

		// Token: 0x04002A0E RID: 10766
		[Tooltip("Event to send attempting to connect to a server.")]
		public FsmEvent isConnecting;

		// Token: 0x04002A0F RID: 10767
		[Tooltip("Repeat every frame. Useful if you're waiting for a particular network state.")]
		public bool everyFrame;
	}
}
