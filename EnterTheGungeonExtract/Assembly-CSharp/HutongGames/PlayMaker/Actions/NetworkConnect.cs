using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2C RID: 2604
	[Tooltip("Connect to a server.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkConnect : FsmStateAction
	{
		// Token: 0x060037AE RID: 14254 RVA: 0x0011EC44 File Offset: 0x0011CE44
		public override void Reset()
		{
			this.remoteIP = "127.0.0.1";
			this.remotePort = 25001;
			this.password = string.Empty;
			this.errorEvent = null;
			this.errorString = null;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0011EC84 File Offset: 0x0011CE84
		public override void OnEnter()
		{
			NetworkConnectionError networkConnectionError = Network.Connect(this.remoteIP.Value, this.remotePort.Value, this.password.Value);
			if (networkConnectionError != NetworkConnectionError.NoError)
			{
				this.errorString.Value = networkConnectionError.ToString();
				base.LogError(this.errorString.Value);
				base.Fsm.Event(this.errorEvent);
			}
			base.Finish();
		}

		// Token: 0x040029AC RID: 10668
		[Tooltip("IP address of the host. Either a dotted IP address or a domain name.")]
		[RequiredField]
		public FsmString remoteIP;

		// Token: 0x040029AD RID: 10669
		[RequiredField]
		[Tooltip("The port on the remote machine to connect to.")]
		public FsmInt remotePort;

		// Token: 0x040029AE RID: 10670
		[Tooltip("Optional password for the server.")]
		public FsmString password;

		// Token: 0x040029AF RID: 10671
		[Tooltip("Event to send in case of an error connecting to the server.")]
		[ActionSection("Errors")]
		public FsmEvent errorEvent;

		// Token: 0x040029B0 RID: 10672
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the error string in a variable.")]
		public FsmString errorString;
	}
}
