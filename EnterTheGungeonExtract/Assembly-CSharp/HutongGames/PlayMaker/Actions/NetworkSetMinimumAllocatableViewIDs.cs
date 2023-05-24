using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A47 RID: 2631
	[Tooltip("Set the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.\n\nThe ViewID pools are given to each player as he connects and are refreshed with new numbers if the player runs out. The server and clients should be in sync regarding this value.\n\nSetting this higher only on the server has the effect that he sends more view ID numbers to clients, than they really want.\n\nSetting this higher only on clients means they request more view IDs more often, for example twice in a row, as the pools received from the server don't contain enough numbers. ")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkSetMinimumAllocatableViewIDs : FsmStateAction
	{
		// Token: 0x0600380F RID: 14351 RVA: 0x0011FF64 File Offset: 0x0011E164
		public override void Reset()
		{
			this.minimumViewIDs = 100;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x0011FF74 File Offset: 0x0011E174
		public override void OnEnter()
		{
			Network.minimumAllocatableViewIDs = this.minimumViewIDs.Value;
			base.Finish();
		}

		// Token: 0x04002A14 RID: 10772
		[Tooltip("The minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.")]
		public FsmInt minimumViewIDs;
	}
}
