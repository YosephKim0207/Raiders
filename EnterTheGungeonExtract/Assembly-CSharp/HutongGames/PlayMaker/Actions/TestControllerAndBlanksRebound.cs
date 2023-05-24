using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD1 RID: 3281
	[ActionCategory(".NPCs")]
	public class TestControllerAndBlanksRebound : FsmStateAction
	{
		// Token: 0x060045AE RID: 17838 RVA: 0x00169BDC File Offset: 0x00167DDC
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.isSwitch = null;
			this.everyFrame = false;
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00169BFC File Offset: 0x00167DFC
		private void HandleEvents()
		{
			if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
			{
				if (GameManager.Options.additionalBlankControl != GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN && GameManager.Options.additionalBlankControl != GameOptions.ControllerBlankControl.NONE)
				{
					base.Fsm.Event(this.isTrue);
				}
				else
				{
					base.Fsm.Event(this.isFalse);
				}
			}
			else if (BraveInput.PrimaryPlayerInstance != null && !BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				if (GameManager.Options.additionalBlankControl != GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN)
				{
					base.Fsm.Event(this.isTrue);
				}
				else
				{
					base.Fsm.Event(this.isFalse);
				}
			}
			else
			{
				base.Fsm.Event(this.isFalse);
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00169CDC File Offset: 0x00167EDC
		public override void OnEnter()
		{
			this.HandleEvents();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00169CF8 File Offset: 0x00167EF8
		public override void OnUpdate()
		{
			this.HandleEvents();
		}

		// Token: 0x040037F9 RID: 14329
		[Tooltip("Event to send if the player is in the foyer.")]
		public FsmEvent isTrue;

		// Token: 0x040037FA RID: 14330
		[Tooltip("Event to send if the player is not in the foyer.")]
		public FsmEvent isFalse;

		// Token: 0x040037FB RID: 14331
		[Tooltip("Event to send if the player is using a Switch")]
		public FsmEvent isSwitch;

		// Token: 0x040037FC RID: 14332
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
