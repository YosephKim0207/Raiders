using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A4C RID: 2636
	[Tooltip("Start a server.")]
	[ActionCategory(ActionCategory.Network)]
	public class StartServer : FsmStateAction
	{
		// Token: 0x06003822 RID: 14370 RVA: 0x00120170 File Offset: 0x0011E370
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

		// Token: 0x06003823 RID: 14371 RVA: 0x001201DC File Offset: 0x0011E3DC
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

		// Token: 0x04002A1D RID: 10781
		[Tooltip("The number of allowed incoming connections/number of players allowed in the game.")]
		[RequiredField]
		public FsmInt connections;

		// Token: 0x04002A1E RID: 10782
		[Tooltip("The port number we want to listen to.")]
		[RequiredField]
		public FsmInt listenPort;

		// Token: 0x04002A1F RID: 10783
		[Tooltip("Sets the password for the server. This must be matched in the NetworkConnect action.")]
		public FsmString incomingPassword;

		// Token: 0x04002A20 RID: 10784
		[Tooltip("Sets the NAT punchthrough functionality.")]
		public FsmBool useNAT;

		// Token: 0x04002A21 RID: 10785
		[Tooltip("Unity handles the network layer by providing secure connections if you wish to use them. \nMost games will want to use secure connections. However, they add up to 15 bytes per packet and take time to compute so you may wish to limit usage to deployed games only.")]
		public FsmBool useSecurityLayer;

		// Token: 0x04002A22 RID: 10786
		[Tooltip("Run the server in the background, even if it doesn't have focus.")]
		public FsmBool runInBackground;

		// Token: 0x04002A23 RID: 10787
		[ActionSection("Errors")]
		[Tooltip("Event to send in case of an error creating the server.")]
		public FsmEvent errorEvent;

		// Token: 0x04002A24 RID: 10788
		[Tooltip("Store the error string in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmString errorString;
	}
}
