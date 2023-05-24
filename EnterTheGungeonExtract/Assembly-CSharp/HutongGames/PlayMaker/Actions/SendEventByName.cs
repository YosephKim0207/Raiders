using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ABA RID: 2746
	[Tooltip("Sends an Event by name after an optional delay. NOTE: Use this over Send Event if you store events as string variables.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SendEventByName : FsmStateAction
	{
		// Token: 0x06003A41 RID: 14913 RVA: 0x001287A8 File Offset: 0x001269A8
		public override void Reset()
		{
			this.eventTarget = null;
			this.sendEvent = null;
			this.delay = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x001287C8 File Offset: 0x001269C8
		public override void OnEnter()
		{
			if (this.delay.Value < 0.001f)
			{
				base.Fsm.Event(this.eventTarget, this.sendEvent.Value);
				if (!this.everyFrame)
				{
					base.Finish();
				}
			}
			else
			{
				this.delayedEvent = base.Fsm.DelayedEvent(this.eventTarget, FsmEvent.GetFsmEvent(this.sendEvent.Value), this.delay.Value);
			}
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00128850 File Offset: 0x00126A50
		public override void OnUpdate()
		{
			if (!this.everyFrame)
			{
				if (DelayedEvent.WasSent(this.delayedEvent))
				{
					base.Finish();
				}
			}
			else
			{
				base.Fsm.Event(this.eventTarget, this.sendEvent.Value);
			}
		}

		// Token: 0x04002C71 RID: 11377
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;

		// Token: 0x04002C72 RID: 11378
		[RequiredField]
		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmString sendEvent;

		// Token: 0x04002C73 RID: 11379
		[HasFloatSlider(0f, 10f)]
		[Tooltip("Optional delay in seconds.")]
		public FsmFloat delay;

		// Token: 0x04002C74 RID: 11380
		[Tooltip("Repeat every frame. Rarely needed, but can be useful when sending events to other FSMs.")]
		public bool everyFrame;

		// Token: 0x04002C75 RID: 11381
		private DelayedEvent delayedEvent;
	}
}
