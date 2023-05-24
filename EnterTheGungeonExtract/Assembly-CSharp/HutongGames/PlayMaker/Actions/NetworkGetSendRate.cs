using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3B RID: 2619
	[Tooltip("Store the current send rate for all NetworkViews")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetSendRate : FsmStateAction
	{
		// Token: 0x060037E5 RID: 14309 RVA: 0x0011F8A4 File Offset: 0x0011DAA4
		public override void Reset()
		{
			this.sendRate = null;
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0011F8B0 File Offset: 0x0011DAB0
		public override void OnEnter()
		{
			this.DoGetSendRate();
			base.Finish();
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x0011F8C0 File Offset: 0x0011DAC0
		private void DoGetSendRate()
		{
			this.sendRate.Value = Network.sendRate;
		}

		// Token: 0x040029F2 RID: 10738
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the current send rate for NetworkViews")]
		[RequiredField]
		public FsmFloat sendRate;
	}
}
