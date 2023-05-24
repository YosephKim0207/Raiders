using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C70 RID: 3184
	[ActionCategory(".Brave")]
	[Tooltip("Prevents the FSM from firing global transitions.")]
	public class SuppressGlobalTransitions : FsmStateAction, INonFinishingState
	{
		// Token: 0x0600446F RID: 17519 RVA: 0x00161DDC File Offset: 0x0015FFDC
		public override void OnEnter()
		{
			base.Fsm.SuppressGlobalTransitions = true;
			base.Finish();
		}
	}
}
