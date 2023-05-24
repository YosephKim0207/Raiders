using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A25 RID: 2597
	[Tooltip("Get the next host data from the master server. \nEach time this action is called it gets the next connected host.This lets you quickly loop through all the connected hosts to get information on each one.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerGetNextHostData : FsmStateAction
	{
		// Token: 0x06003792 RID: 14226 RVA: 0x0011E6FC File Offset: 0x0011C8FC
		public override void Reset()
		{
			this.finishedEvent = null;
			this.loopEvent = null;
			this.index = null;
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

		// Token: 0x06003793 RID: 14227 RVA: 0x0011E764 File Offset: 0x0011C964
		public override void OnEnter()
		{
			this.DoGetNextHostData();
			base.Finish();
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0011E774 File Offset: 0x0011C974
		private void DoGetNextHostData()
		{
			if (this.nextItemIndex >= MasterServer.PollHostList().Length)
			{
				this.nextItemIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			HostData hostData = MasterServer.PollHostList()[this.nextItemIndex];
			this.index.Value = this.nextItemIndex;
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
			if (this.nextItemIndex >= MasterServer.PollHostList().Length)
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

		// Token: 0x0400298B RID: 10635
		[Tooltip("Event to send for looping.")]
		[ActionSection("Set up")]
		public FsmEvent loopEvent;

		// Token: 0x0400298C RID: 10636
		[Tooltip("Event to send when there are no more hosts.")]
		public FsmEvent finishedEvent;

		// Token: 0x0400298D RID: 10637
		[UIHint(UIHint.Variable)]
		[Tooltip("The index into the MasterServer Host List")]
		[ActionSection("Result")]
		public FsmInt index;

		// Token: 0x0400298E RID: 10638
		[Tooltip("Does this server require NAT punchthrough?")]
		[UIHint(UIHint.Variable)]
		public FsmBool useNat;

		// Token: 0x0400298F RID: 10639
		[UIHint(UIHint.Variable)]
		[Tooltip("The type of the game (e.g., 'MyUniqueGameType')")]
		public FsmString gameType;

		// Token: 0x04002990 RID: 10640
		[Tooltip("The name of the game (e.g., 'John Does's Game')")]
		[UIHint(UIHint.Variable)]
		public FsmString gameName;

		// Token: 0x04002991 RID: 10641
		[UIHint(UIHint.Variable)]
		[Tooltip("Currently connected players")]
		public FsmInt connectedPlayers;

		// Token: 0x04002992 RID: 10642
		[UIHint(UIHint.Variable)]
		[Tooltip("Maximum players limit")]
		public FsmInt playerLimit;

		// Token: 0x04002993 RID: 10643
		[UIHint(UIHint.Variable)]
		[Tooltip("Server IP address.")]
		public FsmString ipAddress;

		// Token: 0x04002994 RID: 10644
		[UIHint(UIHint.Variable)]
		[Tooltip("Server port")]
		public FsmInt port;

		// Token: 0x04002995 RID: 10645
		[UIHint(UIHint.Variable)]
		[Tooltip("Does the server require a password?")]
		public FsmBool passwordProtected;

		// Token: 0x04002996 RID: 10646
		[UIHint(UIHint.Variable)]
		[Tooltip("A miscellaneous comment (can hold data)")]
		public FsmString comment;

		// Token: 0x04002997 RID: 10647
		[UIHint(UIHint.Variable)]
		[Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
		public FsmString guid;

		// Token: 0x04002998 RID: 10648
		private int nextItemIndex;

		// Token: 0x04002999 RID: 10649
		private bool noMoreItems;
	}
}
