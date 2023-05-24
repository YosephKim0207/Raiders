using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ABF RID: 2751
	[Tooltip("Sends the next event on the state each time the state is entered.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SequenceEvent : FsmStateAction
	{
		// Token: 0x06003A51 RID: 14929 RVA: 0x00128E8C File Offset: 0x0012708C
		public override void Reset()
		{
			this.delay = null;
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00128E98 File Offset: 0x00127098
		public override void OnEnter()
		{
			int num = base.State.Transitions.Length;
			if (num > 0)
			{
				FsmEvent fsmEvent = base.State.Transitions[this.eventIndex].FsmEvent;
				if (this.delay.Value < 0.001f)
				{
					base.Fsm.Event(fsmEvent);
					base.Finish();
				}
				else
				{
					this.delayedEvent = base.Fsm.DelayedEvent(fsmEvent, this.delay.Value);
				}
				this.eventIndex++;
				if (this.eventIndex == num)
				{
					this.eventIndex = 0;
				}
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00128F3C File Offset: 0x0012713C
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(this.delayedEvent))
			{
				base.Finish();
			}
		}

		// Token: 0x04002C8A RID: 11402
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		// Token: 0x04002C8B RID: 11403
		private DelayedEvent delayedEvent;

		// Token: 0x04002C8C RID: 11404
		private int eventIndex;
	}
}
