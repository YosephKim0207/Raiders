using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9F RID: 2719
	[Tooltip("Sends a Random State Event after an optional delay. Use this to transition to a random state from the current state.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class RandomEvent : FsmStateAction
	{
		// Token: 0x060039B8 RID: 14776 RVA: 0x001266D8 File Offset: 0x001248D8
		public override void Reset()
		{
			this.delay = null;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x001266E4 File Offset: 0x001248E4
		public override void OnEnter()
		{
			if (base.State.Transitions.Length == 0)
			{
				return;
			}
			if (this.lastEventIndex == -1)
			{
				this.lastEventIndex = UnityEngine.Random.Range(0, base.State.Transitions.Length);
			}
			if (this.delay.Value < 0.001f)
			{
				base.Fsm.Event(this.GetRandomEvent());
				base.Finish();
			}
			else
			{
				this.delayedEvent = base.Fsm.DelayedEvent(this.GetRandomEvent(), this.delay.Value);
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x0012677C File Offset: 0x0012497C
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(this.delayedEvent))
			{
				base.Finish();
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00126794 File Offset: 0x00124994
		private FsmEvent GetRandomEvent()
		{
			do
			{
				this.randomEventIndex = UnityEngine.Random.Range(0, base.State.Transitions.Length);
			}
			while (this.noRepeat.Value && base.State.Transitions.Length > 1 && this.randomEventIndex == this.lastEventIndex);
			this.lastEventIndex = this.randomEventIndex;
			return base.State.Transitions[this.randomEventIndex].FsmEvent;
		}

		// Token: 0x04002BDE RID: 11230
		[Tooltip("Delay before sending the event.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		// Token: 0x04002BDF RID: 11231
		[Tooltip("Don't repeat the same event twice in a row.")]
		public FsmBool noRepeat;

		// Token: 0x04002BE0 RID: 11232
		private DelayedEvent delayedEvent;

		// Token: 0x04002BE1 RID: 11233
		private int randomEventIndex;

		// Token: 0x04002BE2 RID: 11234
		private int lastEventIndex = -1;
	}
}
