using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2B RID: 2603
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Close the connection to another system.\n\nConnection index defines which system to close the connection to (from the Network connections array).\nCan define connection to close via Guid if index is unknown. \nIf we are a client the only possible connection to close is the server connection, if we are a server the target player will be kicked off. \n\nSend Disconnection Notification enables or disables notifications being sent to the other end. If disabled the connection is dropped, if not a disconnect notification is reliably sent to the remote party and there after the connection is dropped.")]
	public class NetworkCloseConnection : FsmStateAction
	{
		// Token: 0x060037AA RID: 14250 RVA: 0x0011EB28 File Offset: 0x0011CD28
		public override void Reset()
		{
			this.connectionIndex = 0;
			this.connectionGUID = null;
			this.sendDisconnectionNotification = true;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0011EB44 File Offset: 0x0011CD44
		public override void OnEnter()
		{
			int num = 0;
			int num2;
			if (!this.connectionIndex.IsNone)
			{
				num = this.connectionIndex.Value;
			}
			else if (!this.connectionGUID.IsNone && this.getIndexFromGUID(this.connectionGUID.Value, out num2))
			{
				num = num2;
			}
			if (num < 0 || num > Network.connections.Length)
			{
				base.LogError("Connection index out of range: " + num);
			}
			else
			{
				Network.CloseConnection(Network.connections[num], this.sendDisconnectionNotification);
			}
			base.Finish();
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0011EBF0 File Offset: 0x0011CDF0
		private bool getIndexFromGUID(string guid, out int guidIndex)
		{
			for (int i = 0; i < Network.connections.Length; i++)
			{
				if (guid.Equals(Network.connections[i].guid))
				{
					guidIndex = i;
					return true;
				}
			}
			guidIndex = 0;
			return false;
		}

		// Token: 0x040029A9 RID: 10665
		[Tooltip("Connection index to close")]
		[UIHint(UIHint.Variable)]
		public FsmInt connectionIndex;

		// Token: 0x040029AA RID: 10666
		[UIHint(UIHint.Variable)]
		[Tooltip("Connection GUID to close. Used If Index is not set.")]
		public FsmString connectionGUID;

		// Token: 0x040029AB RID: 10667
		[Tooltip("If True, send Disconnection Notification")]
		public bool sendDisconnectionNotification;
	}
}
