using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000935 RID: 2357
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends an Event when the mobile device is shaken.")]
	public class DeviceShakeEvent : FsmStateAction
	{
		// Token: 0x060033AA RID: 13226 RVA: 0x0010DC6C File Offset: 0x0010BE6C
		public override void Reset()
		{
			this.shakeThreshold = 3f;
			this.sendEvent = null;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x0010DC88 File Offset: 0x0010BE88
		public override void OnUpdate()
		{
			if (Input.acceleration.sqrMagnitude > this.shakeThreshold.Value * this.shakeThreshold.Value)
			{
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x040024CD RID: 9421
		[Tooltip("Amount of acceleration required to trigger the event. Higher numbers require a harder shake.")]
		[RequiredField]
		public FsmFloat shakeThreshold;

		// Token: 0x040024CE RID: 9422
		[Tooltip("Event to send when Shake Threshold is exceded.")]
		[RequiredField]
		public FsmEvent sendEvent;
	}
}
