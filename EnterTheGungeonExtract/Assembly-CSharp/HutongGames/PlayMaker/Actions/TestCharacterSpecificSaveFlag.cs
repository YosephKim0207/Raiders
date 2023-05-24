using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C74 RID: 3188
	[ActionCategory(".Brave")]
	[Tooltip("Sends Events based on the value of a player save flag.")]
	public class TestCharacterSpecificSaveFlag : FsmStateAction
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004479 RID: 17529 RVA: 0x00162110 File Offset: 0x00160310
		public bool Success
		{
			get
			{
				return this.m_success;
			}
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x00162118 File Offset: 0x00160318
		public override void Reset()
		{
			this.successType = TestCharacterSpecificSaveFlag.SuccessType.SetMode;
			this.flagValues = new CharacterSpecificGungeonFlags[0];
			this.values = new FsmBool[0];
			this.Event = null;
			this.mode = string.Empty;
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x00162150 File Offset: 0x00160350
		public override string ErrorCheck()
		{
			string text = string.Empty;
			for (int i = 0; i < this.flagValues.Length; i++)
			{
				if (this.flagValues[i] == CharacterSpecificGungeonFlags.NONE)
				{
					text += "Flag Value is NONE. This is a mistake.";
				}
			}
			if (this.successType == TestCharacterSpecificSaveFlag.SuccessType.SetMode)
			{
				text += BravePlayMakerUtility.CheckCurrentModeVariable(base.Fsm);
				if (!this.mode.Value.StartsWith("mode"))
				{
					text += "Let's be civil and start all mode names with \"mode\", okay?\n";
				}
				text += BravePlayMakerUtility.CheckEventExists(base.Fsm, this.mode.Value);
				text += BravePlayMakerUtility.CheckGlobalTransitionExists(base.Fsm, this.mode.Value);
			}
			return text;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x00162214 File Offset: 0x00160414
		public override void OnEnter()
		{
			if (this.ShouldSkip())
			{
				this.m_success = true;
				base.Finish();
				return;
			}
			this.DoCheck();
			if (!this.everyFrame.Value)
			{
				base.Finish();
			}
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0016224C File Offset: 0x0016044C
		public override void OnUpdate()
		{
			if (this.ShouldSkip())
			{
				this.m_success = true;
				base.Finish();
				return;
			}
			this.DoCheck();
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x00162270 File Offset: 0x00160470
		private bool ShouldSkip()
		{
			for (int i = 0; i < base.State.Actions.Length; i++)
			{
				if (base.State.Actions[i] == this)
				{
					return false;
				}
				if (base.State.Actions[i] is TestSaveFlag && (base.State.Actions[i] as TestSaveFlag).Success)
				{
					return true;
				}
				if (base.State.Actions[i] is TestCharacterSpecificSaveFlag && (base.State.Actions[i] as TestCharacterSpecificSaveFlag).Success)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0016231C File Offset: 0x0016051C
		private void DoCheck()
		{
			this.m_success = true;
			for (int i = 0; i < this.flagValues.Length; i++)
			{
				if (GameStatsManager.Instance.GetCharacterSpecificFlag(this.flagValues[i]) != this.values[i].Value)
				{
					this.m_success = false;
					break;
				}
			}
			if (this.m_success)
			{
				if (this.successType == TestCharacterSpecificSaveFlag.SuccessType.SendEvent)
				{
					base.Fsm.Event(this.Event);
				}
				else if (this.successType == TestCharacterSpecificSaveFlag.SuccessType.SetMode)
				{
					FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
					fsmString.Value = this.mode.Value;
				}
				base.Finish();
			}
		}

		// Token: 0x04003686 RID: 13958
		public TestCharacterSpecificSaveFlag.SuccessType successType;

		// Token: 0x04003687 RID: 13959
		public CharacterSpecificGungeonFlags[] flagValues;

		// Token: 0x04003688 RID: 13960
		public FsmBool[] values;

		// Token: 0x04003689 RID: 13961
		[Tooltip("The event to send if the proceeding tests all pass.")]
		public new FsmEvent Event;

		// Token: 0x0400368A RID: 13962
		[Tooltip("The name of the mode to set 'currentMode' to if the proceeding tests all pass.")]
		public FsmString mode;

		// Token: 0x0400368B RID: 13963
		public FsmBool everyFrame;

		// Token: 0x0400368C RID: 13964
		private bool m_success;

		// Token: 0x02000C75 RID: 3189
		public enum SuccessType
		{
			// Token: 0x0400368E RID: 13966
			SetMode,
			// Token: 0x0400368F RID: 13967
			SendEvent
		}
	}
}
