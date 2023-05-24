using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C64 RID: 3172
	[ActionCategory(".Brave")]
	[Tooltip("Allows the FSM to fire global transitions again.")]
	public class ResumeGlobalTransitions : FsmStateAction, INonFinishingState
	{
		// Token: 0x0600443C RID: 17468 RVA: 0x00160A88 File Offset: 0x0015EC88
		public override void OnEnter()
		{
			if (BravePlayMakerUtility.AllOthersAreFinished(this))
			{
				base.Fsm.SuppressGlobalTransitions = false;
				base.Finish();
			}
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x00160AA8 File Offset: 0x0015ECA8
		public override void OnUpdate()
		{
			if (BravePlayMakerUtility.AllOthersAreFinished(this))
			{
				base.Fsm.SuppressGlobalTransitions = false;
				base.Finish();
			}
		}
	}
}
