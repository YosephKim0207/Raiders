using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096C RID: 2412
	[Tooltip("Sends an Event when a Button is released.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetButtonUp : FsmStateAction
	{
		// Token: 0x0600348F RID: 13455 RVA: 0x0011080C File Offset: 0x0010EA0C
		public override void Reset()
		{
			this.buttonName = "Fire1";
			this.sendEvent = null;
			this.storeResult = null;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x0011082C File Offset: 0x0010EA2C
		public override void OnUpdate()
		{
			bool buttonUp = Input.GetButtonUp(this.buttonName.Value);
			if (buttonUp)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = buttonUp;
		}

		// Token: 0x040025C2 RID: 9666
		[RequiredField]
		[Tooltip("The name of the button. Set in the Unity Input Manager.")]
		public FsmString buttonName;

		// Token: 0x040025C3 RID: 9667
		[Tooltip("Event to send if the button is released.")]
		public FsmEvent sendEvent;

		// Token: 0x040025C4 RID: 9668
		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if the button is released.")]
		public FsmBool storeResult;
	}
}
