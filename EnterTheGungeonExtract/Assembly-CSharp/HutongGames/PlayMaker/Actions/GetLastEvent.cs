using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000990 RID: 2448
	[Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetLastEvent : FsmStateAction
	{
		// Token: 0x06003530 RID: 13616 RVA: 0x001129D0 File Offset: 0x00110BD0
		public override void Reset()
		{
			this.storeEvent = null;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x001129DC File Offset: 0x00110BDC
		public override void OnEnter()
		{
			this.storeEvent.Value = ((base.Fsm.LastTransition != null) ? base.Fsm.LastTransition.EventName : "START");
			base.Finish();
		}

		// Token: 0x04002696 RID: 9878
		[UIHint(UIHint.Variable)]
		public FsmString storeEvent;
	}
}
