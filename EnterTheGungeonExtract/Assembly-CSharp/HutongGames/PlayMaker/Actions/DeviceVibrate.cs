using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000936 RID: 2358
	[Tooltip("Causes the device to vibrate for half a second.")]
	[ActionCategory(ActionCategory.Device)]
	public class DeviceVibrate : FsmStateAction
	{
		// Token: 0x060033AD RID: 13229 RVA: 0x0010DCD8 File Offset: 0x0010BED8
		public override void Reset()
		{
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x0010DCDC File Offset: 0x0010BEDC
		public override void OnEnter()
		{
			base.Finish();
		}
	}
}
