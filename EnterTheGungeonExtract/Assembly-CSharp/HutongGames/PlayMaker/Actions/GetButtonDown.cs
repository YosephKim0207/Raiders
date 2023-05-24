using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096B RID: 2411
	[Tooltip("Sends an Event when a Button is pressed.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetButtonDown : FsmStateAction
	{
		// Token: 0x0600348C RID: 13452 RVA: 0x001107A0 File Offset: 0x0010E9A0
		public override void Reset()
		{
			this.buttonName = "Fire1";
			this.sendEvent = null;
			this.storeResult = null;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x001107C0 File Offset: 0x0010E9C0
		public override void OnUpdate()
		{
			bool buttonDown = Input.GetButtonDown(this.buttonName.Value);
			if (buttonDown)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = buttonDown;
		}

		// Token: 0x040025BF RID: 9663
		[RequiredField]
		[Tooltip("The name of the button. Set in the Unity Input Manager.")]
		public FsmString buttonName;

		// Token: 0x040025C0 RID: 9664
		[Tooltip("Event to send if the button is pressed.")]
		public FsmEvent sendEvent;

		// Token: 0x040025C1 RID: 9665
		[Tooltip("Set to True if the button is pressed.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
	}
}
