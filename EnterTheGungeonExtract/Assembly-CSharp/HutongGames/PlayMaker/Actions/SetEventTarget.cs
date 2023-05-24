using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD2 RID: 2770
	[Tooltip("Sets the target FSM for all subsequent events sent by this state. The default 'Self' sends events to this FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetEventTarget : FsmStateAction
	{
		// Token: 0x06003AA7 RID: 15015 RVA: 0x00129CC4 File Offset: 0x00127EC4
		public override void Reset()
		{
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00129CC8 File Offset: 0x00127EC8
		public override void OnEnter()
		{
			base.Fsm.EventTarget = this.eventTarget;
			base.Finish();
		}

		// Token: 0x04002CD1 RID: 11473
		public FsmEventTarget eventTarget;
	}
}
