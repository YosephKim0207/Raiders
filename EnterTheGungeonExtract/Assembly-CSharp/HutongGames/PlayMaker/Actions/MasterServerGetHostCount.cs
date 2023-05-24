using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A23 RID: 2595
	[Tooltip("Get the number of hosts on the master server.\n\nUse MasterServer Get Host Data to get host data at a specific index.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerGetHostCount : FsmStateAction
	{
		// Token: 0x0600378C RID: 14220 RVA: 0x0011E558 File Offset: 0x0011C758
		public override void OnEnter()
		{
			this.count.Value = MasterServer.PollHostList().Length;
			base.Finish();
		}

		// Token: 0x0400297F RID: 10623
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of hosts on the MasterServer.")]
		[RequiredField]
		public FsmInt count;
	}
}
