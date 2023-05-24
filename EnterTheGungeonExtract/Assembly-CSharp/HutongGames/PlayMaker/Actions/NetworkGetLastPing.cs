using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A33 RID: 2611
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the last ping time to the given player in milliseconds. \nIf the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds.")]
	public class NetworkGetLastPing : FsmStateAction
	{
		// Token: 0x060037C7 RID: 14279 RVA: 0x0011F058 File Offset: 0x0011D258
		public override void Reset()
		{
			this.playerIndex = null;
			this.lastPing = null;
			this.PlayerNotFoundEvent = null;
			this.PlayerFoundEvent = null;
			this.cachePlayerReference = true;
			this.everyFrame = false;
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0011F084 File Offset: 0x0011D284
		public override void OnEnter()
		{
			if (this.cachePlayerReference)
			{
				this._player = Network.connections[this.playerIndex.Value];
			}
			this.GetLastPing();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0011F0D4 File Offset: 0x0011D2D4
		public override void OnUpdate()
		{
			this.GetLastPing();
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x0011F0DC File Offset: 0x0011D2DC
		private void GetLastPing()
		{
			if (!this.cachePlayerReference)
			{
				this._player = Network.connections[this.playerIndex.Value];
			}
			int num = Network.GetLastPing(this._player);
			this.lastPing.Value = num;
			if (num == -1 && this.PlayerNotFoundEvent != null)
			{
				base.Fsm.Event(this.PlayerNotFoundEvent);
			}
			if (num != -1 && this.PlayerFoundEvent != null)
			{
				base.Fsm.Event(this.PlayerFoundEvent);
			}
		}

		// Token: 0x040029C3 RID: 10691
		[ActionSection("Setup")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Index of the player in the network connections list.")]
		public FsmInt playerIndex;

		// Token: 0x040029C4 RID: 10692
		[Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
		public bool cachePlayerReference = true;

		// Token: 0x040029C5 RID: 10693
		public bool everyFrame;

		// Token: 0x040029C6 RID: 10694
		[Tooltip("Get the last ping time to the given player in milliseconds.")]
		[RequiredField]
		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		public FsmInt lastPing;

		// Token: 0x040029C7 RID: 10695
		[Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
		public FsmEvent PlayerNotFoundEvent;

		// Token: 0x040029C8 RID: 10696
		[Tooltip("Event to send if the player is found (pings back).")]
		public FsmEvent PlayerFoundEvent;

		// Token: 0x040029C9 RID: 10697
		private NetworkPlayer _player;
	}
}
