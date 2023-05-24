using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A15 RID: 2581
	[Tooltip("Kill all queued delayed events. Normally delayed events are automatically killed when the active state is exited, but you can override this behaviour in FSM settings. If you choose to keep delayed events you can use this action to kill them when needed.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[Note("Kill all queued delayed events.")]
	public class KillDelayedEvents : FsmStateAction
	{
		// Token: 0x0600374F RID: 14159 RVA: 0x0011D0B4 File Offset: 0x0011B2B4
		public override void OnEnter()
		{
			base.Fsm.KillDelayedEvents();
			base.Finish();
		}
	}
}
