using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C55 RID: 3157
	[ActionCategory(".Brave")]
	[Tooltip("Checks what controller is being used.")]
	public class ControlMethod : FsmStateAction
	{
		// Token: 0x06004405 RID: 17413 RVA: 0x0015F7B4 File Offset: 0x0015D9B4
		public override void Reset()
		{
			this.keyboardAndMouse = null;
			this.controller = null;
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0015F7C4 File Offset: 0x0015D9C4
		public override void OnEnter()
		{
			if (BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false))
			{
				base.Fsm.Event(this.keyboardAndMouse);
			}
			else
			{
				base.Fsm.Event(this.controller);
			}
			base.Finish();
		}

		// Token: 0x0400361C RID: 13852
		[Tooltip("Event to send if the keyboard and mouse are being used.")]
		public FsmEvent keyboardAndMouse;

		// Token: 0x0400361D RID: 13853
		[Tooltip("Event to send when a controller is being used.")]
		public FsmEvent controller;
	}
}
