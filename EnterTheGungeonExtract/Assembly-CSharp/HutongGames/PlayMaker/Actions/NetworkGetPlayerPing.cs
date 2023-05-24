using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A2F RID: 2607
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the last average ping time to the given player in milliseconds. \nIf the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds.")]
	public class NetworkGetPlayerPing : FsmStateAction
	{
		// Token: 0x060037B7 RID: 14263 RVA: 0x0011EDB4 File Offset: 0x0011CFB4
		public override void Reset()
		{
			this.playerIndex = null;
			this.averagePing = null;
			this.PlayerNotFoundEvent = null;
			this.PlayerFoundEvent = null;
			this.cachePlayerReference = true;
			this.everyFrame = false;
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0011EDE0 File Offset: 0x0011CFE0
		public override void OnEnter()
		{
			if (this.cachePlayerReference)
			{
				this._player = Network.connections[this.playerIndex.Value];
			}
			this.GetAveragePing();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0011EE30 File Offset: 0x0011D030
		public override void OnUpdate()
		{
			this.GetAveragePing();
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x0011EE38 File Offset: 0x0011D038
		private void GetAveragePing()
		{
			if (!this.cachePlayerReference)
			{
				this._player = Network.connections[this.playerIndex.Value];
			}
			int num = Network.GetAveragePing(this._player);
			if (!this.averagePing.IsNone)
			{
				this.averagePing.Value = num;
			}
			if (num == -1 && this.PlayerNotFoundEvent != null)
			{
				base.Fsm.Event(this.PlayerNotFoundEvent);
			}
			if (num != -1 && this.PlayerFoundEvent != null)
			{
				base.Fsm.Event(this.PlayerFoundEvent);
			}
		}

		// Token: 0x040029B3 RID: 10675
		[UIHint(UIHint.Variable)]
		[Tooltip("The Index of the player in the network connections list.")]
		[ActionSection("Setup")]
		[RequiredField]
		public FsmInt playerIndex;

		// Token: 0x040029B4 RID: 10676
		[Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
		public bool cachePlayerReference = true;

		// Token: 0x040029B5 RID: 10677
		public bool everyFrame;

		// Token: 0x040029B6 RID: 10678
		[RequiredField]
		[ActionSection("Result")]
		[Tooltip("Get the last average ping time to the given player in milliseconds.")]
		[UIHint(UIHint.Variable)]
		public FsmInt averagePing;

		// Token: 0x040029B7 RID: 10679
		[Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
		public FsmEvent PlayerNotFoundEvent;

		// Token: 0x040029B8 RID: 10680
		[Tooltip("Event to send if the player is found (pings back).")]
		public FsmEvent PlayerFoundEvent;

		// Token: 0x040029B9 RID: 10681
		private NetworkPlayer _player;
	}
}
