using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000999 RID: 2457
	[Tooltip("Sends an Event when the specified Mouse Button is released. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonUp : FsmStateAction
	{
		// Token: 0x06003554 RID: 13652 RVA: 0x00112FD4 File Offset: 0x001111D4
		public override void Reset()
		{
			this.button = MouseButton.Left;
			this.sendEvent = null;
			this.storeResult = null;
			this.inUpdateOnly = true;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00112FF4 File Offset: 0x001111F4
		public override void OnEnter()
		{
			if (!this.inUpdateOnly)
			{
				this.DoGetMouseButtonUp();
			}
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x00113008 File Offset: 0x00111208
		public override void OnUpdate()
		{
			this.DoGetMouseButtonUp();
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x00113010 File Offset: 0x00111210
		public void DoGetMouseButtonUp()
		{
			bool mouseButtonUp = Input.GetMouseButtonUp((int)this.button);
			if (mouseButtonUp)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = mouseButtonUp;
		}

		// Token: 0x040026B3 RID: 9907
		[Tooltip("The mouse button to test.")]
		[RequiredField]
		public MouseButton button;

		// Token: 0x040026B4 RID: 9908
		[Tooltip("Event to send if the mouse button is down.")]
		public FsmEvent sendEvent;

		// Token: 0x040026B5 RID: 9909
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the pressed state in a Bool Variable.")]
		public FsmBool storeResult;

		// Token: 0x040026B6 RID: 9910
		[Tooltip("Uncheck to run when entering the state.")]
		public bool inUpdateOnly;
	}
}
