using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C69 RID: 3177
	[ActionCategory(".Brave")]
	[Tooltip("Sets the variable currentMode to the given string.")]
	public class SetMode : FsmStateAction
	{
		// Token: 0x0600444F RID: 17487 RVA: 0x00161144 File Offset: 0x0015F344
		public override void Reset()
		{
			this.mode = null;
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x00161150 File Offset: 0x0015F350
		public override string ErrorCheck()
		{
			string text = string.Empty;
			text += BravePlayMakerUtility.CheckCurrentModeVariable(base.Fsm);
			if (!this.mode.Value.StartsWith("mode"))
			{
				text += "Let's be civil and start all mode names with \"mode\", okay?\n";
			}
			text += BravePlayMakerUtility.CheckEventExists(base.Fsm, this.mode.Value);
			return text + BravePlayMakerUtility.CheckGlobalTransitionExists(base.Fsm, this.mode.Value);
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x001611D8 File Offset: 0x0015F3D8
		public override void OnEnter()
		{
			FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
			fsmString.Value = this.mode.Value;
			if (this.jumpToMode.Value)
			{
				this.JumpToState();
			}
			base.Finish();
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x00161228 File Offset: 0x0015F428
		private void JumpToState()
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
			base.Fsm.Event(this.mode.Value);
		}

		// Token: 0x04003662 RID: 13922
		[Tooltip("Mode to set currentMode to.")]
		public FsmString mode;

		// Token: 0x04003663 RID: 13923
		[Tooltip("Travel immediately to the new mode.")]
		public FsmBool jumpToMode;
	}
}
