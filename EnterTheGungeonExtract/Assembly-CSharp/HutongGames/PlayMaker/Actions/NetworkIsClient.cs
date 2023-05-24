using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A40 RID: 2624
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if your peer type is client.")]
	public class NetworkIsClient : FsmStateAction
	{
		// Token: 0x060037F5 RID: 14325 RVA: 0x0011FC4C File Offset: 0x0011DE4C
		public override void Reset()
		{
			this.isClient = null;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x0011FC58 File Offset: 0x0011DE58
		public override void OnEnter()
		{
			this.DoCheckIsClient();
			base.Finish();
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x0011FC68 File Offset: 0x0011DE68
		private void DoCheckIsClient()
		{
			this.isClient.Value = Network.isClient;
			if (Network.isClient && this.isClientEvent != null)
			{
				base.Fsm.Event(this.isClientEvent);
			}
			else if (!Network.isClient && this.isNotClientEvent != null)
			{
				base.Fsm.Event(this.isNotClientEvent);
			}
		}

		// Token: 0x04002A05 RID: 10757
		[UIHint(UIHint.Variable)]
		[Tooltip("True if running as client.")]
		public FsmBool isClient;

		// Token: 0x04002A06 RID: 10758
		[Tooltip("Event to send if running as client.")]
		public FsmEvent isClientEvent;

		// Token: 0x04002A07 RID: 10759
		[Tooltip("Event to send if not running as client.")]
		public FsmEvent isNotClientEvent;
	}
}
