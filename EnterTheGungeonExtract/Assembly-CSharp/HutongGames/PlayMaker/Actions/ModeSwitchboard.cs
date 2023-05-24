using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C5F RID: 3167
	[Tooltip("Switchboard to jump to different NPC modes.")]
	[ActionCategory(".Brave")]
	public class ModeSwitchboard : FsmStateAction
	{
		// Token: 0x0600442E RID: 17454 RVA: 0x001603D4 File Offset: 0x0015E5D4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			text += BravePlayMakerUtility.CheckCurrentModeVariable(base.Fsm);
			FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
			text += BravePlayMakerUtility.CheckEventExists(base.Fsm, fsmString.Value);
			return text + BravePlayMakerUtility.CheckGlobalTransitionExists(base.Fsm, fsmString.Value);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x00160440 File Offset: 0x0015E640
		public override void OnEnter()
		{
			FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
			base.Fsm.Event(fsmString.Value);
			base.Finish();
		}
	}
}
