using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A41 RID: 2625
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if your peer type is server.")]
	public class NetworkIsServer : FsmStateAction
	{
		// Token: 0x060037F9 RID: 14329 RVA: 0x0011FCE0 File Offset: 0x0011DEE0
		public override void Reset()
		{
			this.isServer = null;
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x0011FCEC File Offset: 0x0011DEEC
		public override void OnEnter()
		{
			this.DoCheckIsServer();
			base.Finish();
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x0011FCFC File Offset: 0x0011DEFC
		private void DoCheckIsServer()
		{
			this.isServer.Value = Network.isServer;
			if (Network.isServer && this.isServerEvent != null)
			{
				base.Fsm.Event(this.isServerEvent);
			}
			else if (!Network.isServer && this.isNotServerEvent != null)
			{
				base.Fsm.Event(this.isNotServerEvent);
			}
		}

		// Token: 0x04002A08 RID: 10760
		[UIHint(UIHint.Variable)]
		[Tooltip("True if running as server.")]
		public FsmBool isServer;

		// Token: 0x04002A09 RID: 10761
		[Tooltip("Event to send if running as server.")]
		public FsmEvent isServerEvent;

		// Token: 0x04002A0A RID: 10762
		[Tooltip("Event to send if not running as server.")]
		public FsmEvent isNotServerEvent;
	}
}
