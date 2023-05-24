using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB9 RID: 2745
	[ActionTarget(typeof(GameObject), "eventTarget", false)]
	[Tooltip("Sends an Event after an optional delay. NOTE: To send events between FSMs they must be marked as Global in the Events Browser.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "eventTarget", false)]
	public class SendEvent : FsmStateAction
	{
		// Token: 0x06003A3D RID: 14909 RVA: 0x001286C8 File Offset: 0x001268C8
		public override void Reset()
		{
			this.eventTarget = null;
			this.sendEvent = null;
			this.delay = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x001286E8 File Offset: 0x001268E8
		public override void OnEnter()
		{
			if (this.delay.Value < 0.001f)
			{
				base.Fsm.Event(this.eventTarget, this.sendEvent);
				if (!this.everyFrame)
				{
					base.Finish();
				}
			}
			else
			{
				this.delayedEvent = base.Fsm.DelayedEvent(this.eventTarget, this.sendEvent, this.delay.Value);
			}
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00128760 File Offset: 0x00126960
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
				base.Fsm.Event(this.eventTarget, this.sendEvent);
			}
		}

		// Token: 0x04002C6C RID: 11372
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;

		// Token: 0x04002C6D RID: 11373
		[RequiredField]
		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmEvent sendEvent;

		// Token: 0x04002C6E RID: 11374
		[HasFloatSlider(0f, 10f)]
		[Tooltip("Optional delay in seconds.")]
		public FsmFloat delay;

		// Token: 0x04002C6F RID: 11375
		[Tooltip("Repeat every frame. Rarely needed, but can be useful when sending events to other FSMs.")]
		public bool everyFrame;

		// Token: 0x04002C70 RID: 11376
		private DelayedEvent delayedEvent;
	}
}
