using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000998 RID: 2456
	[Tooltip("Sends an Event when the specified Mouse Button is pressed. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonDown : FsmStateAction
	{
		// Token: 0x0600354F RID: 13647 RVA: 0x00112F54 File Offset: 0x00111154
		public override void Reset()
		{
			this.button = MouseButton.Left;
			this.sendEvent = null;
			this.storeResult = null;
			this.inUpdateOnly = true;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x00112F74 File Offset: 0x00111174
		public override void OnEnter()
		{
			if (!this.inUpdateOnly)
			{
				this.DoGetMouseButtonDown();
			}
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x00112F88 File Offset: 0x00111188
		public override void OnUpdate()
		{
			this.DoGetMouseButtonDown();
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x00112F90 File Offset: 0x00111190
		private void DoGetMouseButtonDown()
		{
			bool mouseButtonDown = Input.GetMouseButtonDown((int)this.button);
			if (mouseButtonDown)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = mouseButtonDown;
		}

		// Token: 0x040026AF RID: 9903
		[RequiredField]
		[Tooltip("The mouse button to test.")]
		public MouseButton button;

		// Token: 0x040026B0 RID: 9904
		[Tooltip("Event to send if the mouse button is down.")]
		public FsmEvent sendEvent;

		// Token: 0x040026B1 RID: 9905
		[Tooltip("Store the button state in a Bool Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040026B2 RID: 9906
		[Tooltip("Uncheck to run when entering the state.")]
		public bool inUpdateOnly;
	}
}
