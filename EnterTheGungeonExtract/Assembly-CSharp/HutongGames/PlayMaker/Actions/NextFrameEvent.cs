using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A4D RID: 2637
	[Tooltip("Sends an Event in the next frame. Useful if you want to loop states every frame.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class NextFrameEvent : FsmStateAction
	{
		// Token: 0x06003825 RID: 14373 RVA: 0x0012029C File Offset: 0x0011E49C
		public override void Reset()
		{
			this.sendEvent = null;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x001202A8 File Offset: 0x0011E4A8
		public override void OnEnter()
		{
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x001202AC File Offset: 0x0011E4AC
		public override void OnUpdate()
		{
			base.Finish();
			base.Fsm.Event(this.sendEvent);
		}

		// Token: 0x04002A25 RID: 10789
		[RequiredField]
		public FsmEvent sendEvent;
	}
}
