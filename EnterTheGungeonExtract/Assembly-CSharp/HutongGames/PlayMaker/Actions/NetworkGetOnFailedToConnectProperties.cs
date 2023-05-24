using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A39 RID: 2617
	[Tooltip("Get the network OnFailedToConnect or MasterServer OnFailedToConnectToMasterServer connection error message.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetOnFailedToConnectProperties : FsmStateAction
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x0011F418 File Offset: 0x0011D618
		public override void Reset()
		{
			this.errorLabel = null;
			this.NoErrorEvent = null;
			this.RSAPublicKeyMismatchEvent = null;
			this.InvalidPasswordEvent = null;
			this.ConnectionFailedEvent = null;
			this.TooManyConnectedPlayersEvent = null;
			this.ConnectionBannedEvent = null;
			this.AlreadyConnectedToServerEvent = null;
			this.AlreadyConnectedToAnotherServerEvent = null;
			this.CreateSocketOrThreadFailureEvent = null;
			this.IncorrectParametersEvent = null;
			this.EmptyConnectTargetEvent = null;
			this.InternalDirectConnectFailedEvent = null;
			this.NATTargetNotConnectedEvent = null;
			this.NATTargetConnectionLostEvent = null;
			this.NATPunchthroughFailedEvent = null;
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x0011F498 File Offset: 0x0011D698
		public override void OnEnter()
		{
			this.doGetNetworkErrorInfo();
			base.Finish();
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x0011F4A8 File Offset: 0x0011D6A8
		private void doGetNetworkErrorInfo()
		{
			NetworkConnectionError connectionError = Fsm.EventData.ConnectionError;
			this.errorLabel.Value = connectionError.ToString();
			switch (connectionError + 5)
			{
			case NetworkConnectionError.NoError:
				if (this.InternalDirectConnectFailedEvent != null)
				{
					base.Fsm.Event(this.InternalDirectConnectFailedEvent);
				}
				break;
			case (NetworkConnectionError)1:
				if (this.EmptyConnectTargetEvent != null)
				{
					base.Fsm.Event(this.EmptyConnectTargetEvent);
				}
				break;
			case (NetworkConnectionError)2:
				if (this.IncorrectParametersEvent != null)
				{
					base.Fsm.Event(this.IncorrectParametersEvent);
				}
				break;
			case (NetworkConnectionError)3:
				if (this.CreateSocketOrThreadFailureEvent != null)
				{
					base.Fsm.Event(this.CreateSocketOrThreadFailureEvent);
				}
				break;
			case (NetworkConnectionError)4:
				if (this.AlreadyConnectedToAnotherServerEvent != null)
				{
					base.Fsm.Event(this.AlreadyConnectedToAnotherServerEvent);
				}
				break;
			case (NetworkConnectionError)5:
				if (this.NoErrorEvent != null)
				{
					base.Fsm.Event(this.NoErrorEvent);
				}
				break;
			default:
				switch (connectionError)
				{
				case NetworkConnectionError.ConnectionFailed:
					if (this.ConnectionFailedEvent != null)
					{
						base.Fsm.Event(this.ConnectionFailedEvent);
					}
					break;
				case NetworkConnectionError.AlreadyConnectedToServer:
					if (this.AlreadyConnectedToServerEvent != null)
					{
						base.Fsm.Event(this.AlreadyConnectedToServerEvent);
					}
					break;
				default:
					switch (connectionError)
					{
					case NetworkConnectionError.NATTargetNotConnected:
						if (this.NATTargetNotConnectedEvent != null)
						{
							base.Fsm.Event(this.NATTargetNotConnectedEvent);
						}
						break;
					case NetworkConnectionError.NATTargetConnectionLost:
						if (this.NATTargetConnectionLostEvent != null)
						{
							base.Fsm.Event(this.NATTargetConnectionLostEvent);
						}
						break;
					case NetworkConnectionError.NATPunchthroughFailed:
						if (this.NATPunchthroughFailedEvent != null)
						{
							base.Fsm.Event(this.NoErrorEvent);
						}
						break;
					}
					break;
				case NetworkConnectionError.TooManyConnectedPlayers:
					if (this.TooManyConnectedPlayersEvent != null)
					{
						base.Fsm.Event(this.TooManyConnectedPlayersEvent);
					}
					break;
				case NetworkConnectionError.RSAPublicKeyMismatch:
					if (this.RSAPublicKeyMismatchEvent != null)
					{
						base.Fsm.Event(this.RSAPublicKeyMismatchEvent);
					}
					break;
				case NetworkConnectionError.ConnectionBanned:
					if (this.ConnectionBannedEvent != null)
					{
						base.Fsm.Event(this.ConnectionBannedEvent);
					}
					break;
				case NetworkConnectionError.InvalidPassword:
					if (this.InvalidPasswordEvent != null)
					{
						base.Fsm.Event(this.InvalidPasswordEvent);
					}
					break;
				}
				break;
			}
		}

		// Token: 0x040029D9 RID: 10713
		[Tooltip("Error label")]
		[UIHint(UIHint.Variable)]
		public FsmString errorLabel;

		// Token: 0x040029DA RID: 10714
		[Tooltip("No error occurred.")]
		public FsmEvent NoErrorEvent;

		// Token: 0x040029DB RID: 10715
		[Tooltip("We presented an RSA public key which does not match what the system we connected to is using.")]
		public FsmEvent RSAPublicKeyMismatchEvent;

		// Token: 0x040029DC RID: 10716
		[Tooltip("The server is using a password and has refused our connection because we did not set the correct password.")]
		public FsmEvent InvalidPasswordEvent;

		// Token: 0x040029DD RID: 10717
		[Tooltip("onnection attempt failed, possibly because of internal connectivity problems.")]
		public FsmEvent ConnectionFailedEvent;

		// Token: 0x040029DE RID: 10718
		[Tooltip("The server is at full capacity, failed to connect.")]
		public FsmEvent TooManyConnectedPlayersEvent;

		// Token: 0x040029DF RID: 10719
		[Tooltip("We are banned from the system we attempted to connect to (likely temporarily).")]
		public FsmEvent ConnectionBannedEvent;

		// Token: 0x040029E0 RID: 10720
		[Tooltip("We are already connected to this particular server (can happen after fast disconnect/reconnect).")]
		public FsmEvent AlreadyConnectedToServerEvent;

		// Token: 0x040029E1 RID: 10721
		[Tooltip("Cannot connect to two servers at once. Close the connection before connecting again.")]
		public FsmEvent AlreadyConnectedToAnotherServerEvent;

		// Token: 0x040029E2 RID: 10722
		[Tooltip("Internal error while attempting to initialize network interface. Socket possibly already in use.")]
		public FsmEvent CreateSocketOrThreadFailureEvent;

		// Token: 0x040029E3 RID: 10723
		[Tooltip("Incorrect parameters given to Connect function.")]
		public FsmEvent IncorrectParametersEvent;

		// Token: 0x040029E4 RID: 10724
		[Tooltip("No host target given in Connect.")]
		public FsmEvent EmptyConnectTargetEvent;

		// Token: 0x040029E5 RID: 10725
		[Tooltip("Client could not connect internally to same network NAT enabled server.")]
		public FsmEvent InternalDirectConnectFailedEvent;

		// Token: 0x040029E6 RID: 10726
		[Tooltip("The NAT target we are trying to connect to is not connected to the facilitator server.")]
		public FsmEvent NATTargetNotConnectedEvent;

		// Token: 0x040029E7 RID: 10727
		[Tooltip("Connection lost while attempting to connect to NAT target.")]
		public FsmEvent NATTargetConnectionLostEvent;

		// Token: 0x040029E8 RID: 10728
		[Tooltip("NAT punchthrough attempt has failed. The cause could be a too restrictive NAT implementation on either endpoints.")]
		public FsmEvent NATPunchthroughFailedEvent;
	}
}
