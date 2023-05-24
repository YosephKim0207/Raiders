using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C58 RID: 3160
	[Tooltip("Responds to trigger events with Speculative Rigidbodies.")]
	[ActionCategory(".Brave")]
	public class DwarfEventAreaEvent : FsmStateAction
	{
		// Token: 0x0600440D RID: 17421 RVA: 0x0015F8B0 File Offset: 0x0015DAB0
		public override void Reset()
		{
			this.eventIndices = new FsmInt[0];
			this.events = new FsmEvent[0];
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0015F8CC File Offset: 0x0015DACC
		public override void OnEnter()
		{
			this.m_eventListener = base.Owner.GetComponent<DwarfEventListener>();
			if (this.m_eventListener)
			{
				DwarfEventListener eventListener = this.m_eventListener;
				eventListener.OnTrigger = (Action<int>)Delegate.Combine(eventListener.OnTrigger, new Action<int>(this.OnTrigger));
			}
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x0015F924 File Offset: 0x0015DB24
		public override void OnExit()
		{
			if (this.m_eventListener)
			{
				DwarfEventListener eventListener = this.m_eventListener;
				eventListener.OnTrigger = (Action<int>)Delegate.Remove(eventListener.OnTrigger, new Action<int>(this.OnTrigger));
			}
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x0015F960 File Offset: 0x0015DB60
		private void OnTrigger(int index)
		{
			for (int i = 0; i < this.eventIndices.Length; i++)
			{
				if (this.eventIndices[i].Value == index)
				{
					base.Fsm.Event(this.events[i]);
				}
			}
		}

		// Token: 0x04003621 RID: 13857
		[CompoundArray("Events", "Trigger Index", "Send Event")]
		[Tooltip("Event to play when the corresponding trigger detects a collision.")]
		public FsmInt[] eventIndices;

		// Token: 0x04003622 RID: 13858
		public FsmEvent[] events;

		// Token: 0x04003623 RID: 13859
		private DwarfEventListener m_eventListener;
	}
}
