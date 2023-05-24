using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A31 RID: 2609
	[Tooltip("Get the number of connected players.\n\nOn a client this returns 1 (the server).")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetConnectionsCount : FsmStateAction
	{
		// Token: 0x060037C0 RID: 14272 RVA: 0x0011EFD0 File Offset: 0x0011D1D0
		public override void Reset()
		{
			this.connectionsCount = null;
			this.everyFrame = true;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0011EFE0 File Offset: 0x0011D1E0
		public override void OnEnter()
		{
			this.connectionsCount.Value = Network.connections.Length;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0011F008 File Offset: 0x0011D208
		public override void OnUpdate()
		{
			this.connectionsCount.Value = Network.connections.Length;
		}

		// Token: 0x040029C0 RID: 10688
		[Tooltip("Number of connected players.")]
		[UIHint(UIHint.Variable)]
		public FsmInt connectionsCount;

		// Token: 0x040029C1 RID: 10689
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
