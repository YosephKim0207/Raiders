using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3C RID: 2620
	[Tooltip("Get the current network time (seconds).")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetTime : FsmStateAction
	{
		// Token: 0x060037E9 RID: 14313 RVA: 0x0011F8DC File Offset: 0x0011DADC
		public override void Reset()
		{
			this.time = null;
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x0011F8E8 File Offset: 0x0011DAE8
		public override void OnEnter()
		{
			this.time.Value = (float)Network.time;
			base.Finish();
		}

		// Token: 0x040029F3 RID: 10739
		[Tooltip("The network time.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat time;
	}
}
