using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C63 RID: 3171
	[Tooltip("When all other actions on this state are finished, send a RESTART event.")]
	[ActionCategory(".Brave")]
	public class RestartWhenFinished : FsmStateAction, INonFinishingState
	{
		// Token: 0x06004437 RID: 17463 RVA: 0x001609A8 File Offset: 0x0015EBA8
		public override string ErrorCheck()
		{
			string empty = string.Empty;
			base.Fsm.GetEvent("RESTART");
			return empty + BravePlayMakerUtility.CheckGlobalTransitionExists(base.Fsm, "RESTART");
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x001609E4 File Offset: 0x0015EBE4
		public override void OnEnter()
		{
			if (BravePlayMakerUtility.AllOthersAreFinished(this))
			{
				this.GoToStartState();
			}
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x001609F8 File Offset: 0x0015EBF8
		public override void OnUpdate()
		{
			if (BravePlayMakerUtility.AllOthersAreFinished(this))
			{
				this.GoToStartState();
			}
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x00160A0C File Offset: 0x0015EC0C
		private void GoToStartState()
		{
			if (base.Fsm.SuppressGlobalTransitions)
			{
				foreach (FsmStateAction fsmStateAction in base.State.Actions)
				{
					if (fsmStateAction is ResumeGlobalTransitions)
					{
						base.Fsm.SuppressGlobalTransitions = false;
						break;
					}
				}
			}
			base.Fsm.Event("RESTART");
			base.Finish();
		}
	}
}
