using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3E RID: 2622
	[Tooltip("Launch a server.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkInitializeServer : FsmStateAction
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x0011F99C File Offset: 0x0011DB9C
		public override void Reset()
		{
			this.connections = 32;
			this.listenPort = 25001;
			this.incomingPassword = string.Empty;
			this.errorEvent = null;
			this.errorString = null;
			this.useNAT = false;
			this.useSecurityLayer = false;
			this.runInBackground = true;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0011FA08 File Offset: 0x0011DC08
		public override void OnEnter()
		{
			Network.incomingPassword = this.incomingPassword.Value;
			if (this.useSecurityLayer.Value)
			{
				Network.InitializeSecurity();
			}
			if (this.runInBackground.Value)
			{
				Application.runInBackground = true;
			}
			NetworkConnectionError networkConnectionError = Network.InitializeServer(this.connections.Value, this.listenPort.Value, this.useNAT.Value);
			if (networkConnectionError != NetworkConnectionError.NoError)
			{
				this.errorString.Value = networkConnectionError.ToString();
				base.LogError(this.errorString.Value);
				base.Fsm.Event(this.errorEvent);
			}
			base.Finish();
		}

		// Token: 0x040029F7 RID: 10743
		[Tooltip("The number of allowed incoming connections/number of players allowed in the game.")]
		[RequiredField]
		public FsmInt connections;

		// Token: 0x040029F8 RID: 10744
		[RequiredField]
		[Tooltip("The port number we want to listen to.")]
		public FsmInt listenPort;

		// Token: 0x040029F9 RID: 10745
		[Tooltip("Sets the password for the server. This must be matched in the NetworkConnect action.")]
		public FsmString incomingPassword;

		// Token: 0x040029FA RID: 10746
		[Tooltip("Sets the NAT punchthrough functionality.")]
		public FsmBool useNAT;

		// Token: 0x040029FB RID: 10747
		[Tooltip("Unity handles the network layer by providing secure connections if you wish to use them. \nMost games will want to use secure connections. However, they add up to 15 bytes per packet and take time to compute so you may wish to limit usage to deployed games only.")]
		public FsmBool useSecurityLayer;

		// Token: 0x040029FC RID: 10748
		[Tooltip("Run the server in the background, even if it doesn't have focus.")]
		public FsmBool runInBackground;

		// Token: 0x040029FD RID: 10749
		[Tooltip("Event to send in case of an error creating the server.")]
		[ActionSection("Errors")]
		public FsmEvent errorEvent;

		// Token: 0x040029FE RID: 10750
		[Tooltip("Store the error string in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmString errorString;
	}
}
