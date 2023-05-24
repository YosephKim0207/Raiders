using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000948 RID: 2376
	[Tooltip("Stop this FSM. If this FSM was launched by a Run FSM action, it will trigger a Finish event in that state.")]
	[Note("Stop this FSM. If this FSM was launched by a Run FSM action, it will trigger a Finish event in that state.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class FinishFSM : FsmStateAction
	{
		// Token: 0x060033F8 RID: 13304 RVA: 0x0010ECE4 File Offset: 0x0010CEE4
		public override void OnEnter()
		{
			base.Fsm.Stop();
		}
	}
}
