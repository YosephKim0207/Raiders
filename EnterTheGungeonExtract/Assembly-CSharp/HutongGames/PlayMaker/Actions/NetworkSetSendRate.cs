using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A48 RID: 2632
	[Tooltip("Set the send rate for all networkViews. Default is 15")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkSetSendRate : FsmStateAction
	{
		// Token: 0x06003812 RID: 14354 RVA: 0x0011FF94 File Offset: 0x0011E194
		public override void Reset()
		{
			this.sendRate = 15f;
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x0011FFA8 File Offset: 0x0011E1A8
		public override void OnEnter()
		{
			this.DoSetSendRate();
			base.Finish();
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x0011FFB8 File Offset: 0x0011E1B8
		private void DoSetSendRate()
		{
			Network.sendRate = this.sendRate.Value;
		}

		// Token: 0x04002A15 RID: 10773
		[Tooltip("The send rate for all networkViews")]
		[RequiredField]
		public FsmFloat sendRate;
	}
}
