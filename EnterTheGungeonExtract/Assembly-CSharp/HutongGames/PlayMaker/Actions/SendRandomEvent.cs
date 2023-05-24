using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ABE RID: 2750
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random Event picked from an array of Events. Optionally set the relative weight of each event.")]
	public class SendRandomEvent : FsmStateAction
	{
		// Token: 0x06003A4D RID: 14925 RVA: 0x00128D8C File Offset: 0x00126F8C
		public override void Reset()
		{
			this.events = new FsmEvent[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.delay = null;
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00128DE0 File Offset: 0x00126FE0
		public override void OnEnter()
		{
			if (this.events.Length > 0)
			{
				int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
				if (randomWeightedIndex != -1)
				{
					if (this.delay.Value < 0.001f)
					{
						base.Fsm.Event(this.events[randomWeightedIndex]);
						base.Finish();
					}
					else
					{
						this.delayedEvent = base.Fsm.DelayedEvent(this.events[randomWeightedIndex], this.delay.Value);
					}
					return;
				}
			}
			base.Finish();
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00128E6C File Offset: 0x0012706C
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(this.delayedEvent))
			{
				base.Finish();
			}
		}

		// Token: 0x04002C86 RID: 11398
		[CompoundArray("Events", "Event", "Weight")]
		public FsmEvent[] events;

		// Token: 0x04002C87 RID: 11399
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002C88 RID: 11400
		public FsmFloat delay;

		// Token: 0x04002C89 RID: 11401
		private DelayedEvent delayedEvent;
	}
}
