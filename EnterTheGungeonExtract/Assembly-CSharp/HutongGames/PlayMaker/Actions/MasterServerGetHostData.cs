using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A24 RID: 2596
	[Tooltip("Get host data from the master server.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerGetHostData : FsmStateAction
	{
		// Token: 0x0600378E RID: 14222 RVA: 0x0011E57C File Offset: 0x0011C77C
		public override void Reset()
		{
			this.hostIndex = null;
			this.useNat = null;
			this.gameType = null;
			this.gameName = null;
			this.connectedPlayers = null;
			this.playerLimit = null;
			this.ipAddress = null;
			this.port = null;
			this.passwordProtected = null;
			this.comment = null;
			this.guid = null;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0011E5D8 File Offset: 0x0011C7D8
		public override void OnEnter()
		{
			this.GetHostData();
			base.Finish();
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0011E5E8 File Offset: 0x0011C7E8
		private void GetHostData()
		{
			int num = MasterServer.PollHostList().Length;
			int value = this.hostIndex.Value;
			if (value < 0 || value >= num)
			{
				base.LogError("MasterServer Host index out of range!");
				return;
			}
			HostData hostData = MasterServer.PollHostList()[value];
			if (hostData == null)
			{
				base.LogError("MasterServer HostData could not found at index " + value);
				return;
			}
			this.useNat.Value = hostData.useNat;
			this.gameType.Value = hostData.gameType;
			this.gameName.Value = hostData.gameName;
			this.connectedPlayers.Value = hostData.connectedPlayers;
			this.playerLimit.Value = hostData.playerLimit;
			this.ipAddress.Value = hostData.ip[0];
			this.port.Value = hostData.port;
			this.passwordProtected.Value = hostData.passwordProtected;
			this.comment.Value = hostData.comment;
			this.guid.Value = hostData.guid;
		}

		// Token: 0x04002980 RID: 10624
		[Tooltip("The index into the MasterServer Host List")]
		[RequiredField]
		public FsmInt hostIndex;

		// Token: 0x04002981 RID: 10625
		[UIHint(UIHint.Variable)]
		[Tooltip("Does this server require NAT punchthrough?")]
		public FsmBool useNat;

		// Token: 0x04002982 RID: 10626
		[UIHint(UIHint.Variable)]
		[Tooltip("The type of the game (e.g., 'MyUniqueGameType')")]
		public FsmString gameType;

		// Token: 0x04002983 RID: 10627
		[UIHint(UIHint.Variable)]
		[Tooltip("The name of the game (e.g., 'John Does's Game')")]
		public FsmString gameName;

		// Token: 0x04002984 RID: 10628
		[UIHint(UIHint.Variable)]
		[Tooltip("Currently connected players")]
		public FsmInt connectedPlayers;

		// Token: 0x04002985 RID: 10629
		[Tooltip("Maximum players limit")]
		[UIHint(UIHint.Variable)]
		public FsmInt playerLimit;

		// Token: 0x04002986 RID: 10630
		[UIHint(UIHint.Variable)]
		[Tooltip("Server IP address.")]
		public FsmString ipAddress;

		// Token: 0x04002987 RID: 10631
		[UIHint(UIHint.Variable)]
		[Tooltip("Server port")]
		public FsmInt port;

		// Token: 0x04002988 RID: 10632
		[UIHint(UIHint.Variable)]
		[Tooltip("Does the server require a password?")]
		public FsmBool passwordProtected;

		// Token: 0x04002989 RID: 10633
		[UIHint(UIHint.Variable)]
		[Tooltip("A miscellaneous comment (can hold data)")]
		public FsmString comment;

		// Token: 0x0400298A RID: 10634
		[UIHint(UIHint.Variable)]
		[Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
		public FsmString guid;
	}
}
