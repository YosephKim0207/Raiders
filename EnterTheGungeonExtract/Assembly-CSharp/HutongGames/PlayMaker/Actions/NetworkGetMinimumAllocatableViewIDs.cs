using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A37 RID: 2615
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.\n\nThe ViewID pools are given to each player as he connects and are refreshed with new numbers if the player runs out. The server and clients should be in sync regarding this value.\n\nSetting this higher only on the server has the effect that he sends more view ID numbers to clients, than they really want.\n\nSetting this higher only on clients means they request more view IDs more often, for example twice in a row, as the pools received from the server don't contain enough numbers. ")]
	public class NetworkGetMinimumAllocatableViewIDs : FsmStateAction
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x0011F334 File Offset: 0x0011D534
		public override void Reset()
		{
			this.result = null;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0011F340 File Offset: 0x0011D540
		public override void OnEnter()
		{
			this.result.Value = Network.minimumAllocatableViewIDs;
			base.Finish();
		}

		// Token: 0x040029D5 RID: 10709
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.")]
		public FsmInt result;
	}
}
