using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A3D RID: 2621
	[Tooltip("Check if this machine has a public IP address.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkHavePublicIpAddress : FsmStateAction
	{
		// Token: 0x060037EC RID: 14316 RVA: 0x0011F90C File Offset: 0x0011DB0C
		public override void Reset()
		{
			this.havePublicIpAddress = null;
			this.publicIpAddressFoundEvent = null;
			this.publicIpAddressNotFoundEvent = null;
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x0011F924 File Offset: 0x0011DB24
		public override void OnEnter()
		{
			bool flag = Network.HavePublicAddress();
			this.havePublicIpAddress.Value = flag;
			if (flag && this.publicIpAddressFoundEvent != null)
			{
				base.Fsm.Event(this.publicIpAddressFoundEvent);
			}
			else if (!flag && this.publicIpAddressNotFoundEvent != null)
			{
				base.Fsm.Event(this.publicIpAddressNotFoundEvent);
			}
			base.Finish();
		}

		// Token: 0x040029F4 RID: 10740
		[UIHint(UIHint.Variable)]
		[Tooltip("True if this machine has a public IP address")]
		public FsmBool havePublicIpAddress;

		// Token: 0x040029F5 RID: 10741
		[Tooltip("Event to send if this machine has a public IP address")]
		public FsmEvent publicIpAddressFoundEvent;

		// Token: 0x040029F6 RID: 10742
		[Tooltip("Event to send if this machine has no public IP address")]
		public FsmEvent publicIpAddressNotFoundEvent;
	}
}
