using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000934 RID: 2356
	[Tooltip("Sends an Event based on the Orientation of the mobile device.")]
	[ActionCategory(ActionCategory.Device)]
	public class DeviceOrientationEvent : FsmStateAction
	{
		// Token: 0x060033A5 RID: 13221 RVA: 0x0010DC04 File Offset: 0x0010BE04
		public override void Reset()
		{
			this.orientation = DeviceOrientation.Portrait;
			this.sendEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x0010DC1C File Offset: 0x0010BE1C
		public override void OnEnter()
		{
			this.DoDetectDeviceOrientation();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x0010DC38 File Offset: 0x0010BE38
		public override void OnUpdate()
		{
			this.DoDetectDeviceOrientation();
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x0010DC40 File Offset: 0x0010BE40
		private void DoDetectDeviceOrientation()
		{
			if (Input.deviceOrientation == this.orientation)
			{
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x040024CA RID: 9418
		[Tooltip("Note: If device is physically situated between discrete positions, as when (for example) rotated diagonally, system will report Unknown orientation.")]
		public DeviceOrientation orientation;

		// Token: 0x040024CB RID: 9419
		[Tooltip("The event to send if the device orientation matches Orientation.")]
		public FsmEvent sendEvent;

		// Token: 0x040024CC RID: 9420
		[Tooltip("Repeat every frame. Useful if you want to wait for the orientation to be true.")]
		public bool everyFrame;
	}
}
