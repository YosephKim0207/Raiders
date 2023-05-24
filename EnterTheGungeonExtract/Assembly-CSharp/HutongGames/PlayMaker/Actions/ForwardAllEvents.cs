using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000959 RID: 2393
	[Tooltip("Forwards all event received by this FSM to another target. Optionally specify a list of events to ignore.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class ForwardAllEvents : FsmStateAction
	{
		// Token: 0x06003442 RID: 13378 RVA: 0x0010F904 File Offset: 0x0010DB04
		public override void Reset()
		{
			this.forwardTo = new FsmEventTarget
			{
				target = FsmEventTarget.EventTarget.FSMComponent
			};
			this.exceptThese = new FsmEvent[] { FsmEvent.Finished };
			this.eatEvents = true;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x0010F940 File Offset: 0x0010DB40
		public override bool Event(FsmEvent fsmEvent)
		{
			if (this.exceptThese != null)
			{
				foreach (FsmEvent fsmEvent2 in this.exceptThese)
				{
					if (fsmEvent2 == fsmEvent)
					{
						return false;
					}
				}
			}
			base.Fsm.Event(this.forwardTo, fsmEvent);
			return this.eatEvents;
		}

		// Token: 0x04002564 RID: 9572
		[Tooltip("Forward to this target.")]
		public FsmEventTarget forwardTo;

		// Token: 0x04002565 RID: 9573
		[Tooltip("Don't forward these events.")]
		public FsmEvent[] exceptThese;

		// Token: 0x04002566 RID: 9574
		[Tooltip("Should this action eat the events or pass them on.")]
		public bool eatEvents;
	}
}
