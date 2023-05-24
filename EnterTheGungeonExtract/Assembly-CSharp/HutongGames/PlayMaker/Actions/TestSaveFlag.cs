using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C79 RID: 3193
	[Tooltip("Sends Events based on the value of a player save flag.")]
	[ActionCategory(".Brave")]
	public class TestSaveFlag : FsmStateAction
	{
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x00162564 File Offset: 0x00160764
		public bool Success
		{
			get
			{
				return this.m_success;
			}
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0016256C File Offset: 0x0016076C
		public override void Reset()
		{
			this.successType = TestSaveFlag.SuccessType.SetMode;
			this.flagValues = new GungeonFlags[0];
			this.values = new FsmBool[0];
			this.Event = null;
			this.mode = string.Empty;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x001625A4 File Offset: 0x001607A4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			for (int i = 0; i < this.flagValues.Length; i++)
			{
				if (this.flagValues[i] == GungeonFlags.NONE)
				{
					text += "Flag Value is NONE. This is a mistake.";
				}
			}
			if (this.successType == TestSaveFlag.SuccessType.SetMode)
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

		// Token: 0x06004491 RID: 17553 RVA: 0x00162668 File Offset: 0x00160868
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

		// Token: 0x06004492 RID: 17554 RVA: 0x001626A0 File Offset: 0x001608A0
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

		// Token: 0x06004493 RID: 17555 RVA: 0x001626C4 File Offset: 0x001608C4
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

		// Token: 0x06004494 RID: 17556 RVA: 0x00162770 File Offset: 0x00160970
		private void DoCheck()
		{
			this.m_success = true;
			for (int i = 0; i < this.flagValues.Length; i++)
			{
				if (GameStatsManager.Instance.GetFlag(this.flagValues[i]) != this.values[i].Value)
				{
					this.m_success = false;
					break;
				}
			}
			if (this.m_success)
			{
				if (this.successType == TestSaveFlag.SuccessType.SendEvent)
				{
					base.Fsm.Event(this.Event);
				}
				else if (this.successType == TestSaveFlag.SuccessType.SetMode)
				{
					FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
					fsmString.Value = this.mode.Value;
				}
				base.Finish();
			}
		}

		// Token: 0x04003698 RID: 13976
		public TestSaveFlag.SuccessType successType;

		// Token: 0x04003699 RID: 13977
		public GungeonFlags[] flagValues;

		// Token: 0x0400369A RID: 13978
		public FsmBool[] values;

		// Token: 0x0400369B RID: 13979
		[Tooltip("The event to send if the proceeding tests all pass.")]
		public new FsmEvent Event;

		// Token: 0x0400369C RID: 13980
		[Tooltip("The name of the mode to set 'currentMode' to if the proceeding tests all pass.")]
		public FsmString mode;

		// Token: 0x0400369D RID: 13981
		public FsmBool everyFrame;

		// Token: 0x0400369E RID: 13982
		private bool m_success;

		// Token: 0x02000C7A RID: 3194
		public enum SuccessType
		{
			// Token: 0x040036A0 RID: 13984
			SetMode,
			// Token: 0x040036A1 RID: 13985
			SendEvent
		}
	}
}
