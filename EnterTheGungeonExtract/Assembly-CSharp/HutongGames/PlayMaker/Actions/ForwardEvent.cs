using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095A RID: 2394
	[Tooltip("Forward an event received by this FSM to another target.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class ForwardEvent : FsmStateAction
	{
		// Token: 0x06003445 RID: 13381 RVA: 0x0010F9A0 File Offset: 0x0010DBA0
		public override void Reset()
		{
			this.forwardTo = new FsmEventTarget
			{
				target = FsmEventTarget.EventTarget.FSMComponent
			};
			this.eventsToForward = null;
			this.eatEvents = true;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x0010F9D0 File Offset: 0x0010DBD0
		public override bool Event(FsmEvent fsmEvent)
		{
			if (this.eventsToForward != null)
			{
				foreach (FsmEvent fsmEvent2 in this.eventsToForward)
				{
					if (fsmEvent2 == fsmEvent)
					{
						base.Fsm.Event(this.forwardTo, fsmEvent);
						return this.eatEvents;
					}
				}
			}
			return false;
		}

		// Token: 0x04002567 RID: 9575
		[Tooltip("Forward to this target.")]
		public FsmEventTarget forwardTo;

		// Token: 0x04002568 RID: 9576
		[Tooltip("The events to forward.")]
		public FsmEvent[] eventsToForward;

		// Token: 0x04002569 RID: 9577
		[Tooltip("Should this action eat the events or pass them on.")]
		public bool eatEvents;
	}
}
