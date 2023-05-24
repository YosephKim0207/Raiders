using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A35 RID: 2613
	[Tooltip("Get the maximum amount of connections/players allowed.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetMaximumConnections : FsmStateAction
	{
		// Token: 0x060037CF RID: 14287 RVA: 0x0011F238 File Offset: 0x0011D438
		public override void Reset()
		{
			this.result = null;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0011F244 File Offset: 0x0011D444
		public override void OnEnter()
		{
			this.result.Value = Network.maxConnections;
			base.Finish();
		}

		// Token: 0x040029CF RID: 10703
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the maximum amount of connections/players allowed.")]
		public FsmInt result;
	}
}
