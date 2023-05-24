using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA2 RID: 3234
	[ActionCategory(".Brave")]
	[Tooltip("Sends Events based on the value of a player save flag.")]
	public class MassiveSaveFlagSwitchboard : FsmStateAction
	{
		// Token: 0x06004522 RID: 17698 RVA: 0x0016668C File Offset: 0x0016488C
		public override void Reset()
		{
			this.entries = new MassiveSaveFlagEntry[0];
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0016669C File Offset: 0x0016489C
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x001666B0 File Offset: 0x001648B0
		public override void OnEnter()
		{
			this.DoCheck();
			base.Finish();
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x001666C0 File Offset: 0x001648C0
		private void DoCheck()
		{
			for (int i = 0; i < this.entries.Length; i++)
			{
				if (GameStatsManager.Instance.GetFlag(this.entries[i].RequiredFlag) == this.entries[i].RequiredFlagState && !GameStatsManager.Instance.GetFlag(this.entries[i].CompletedFlag))
				{
					if (this.entries[i].CompletedFlag != GungeonFlags.CREST_NPC_SGDQ2018 || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
					{
						FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
						fsmString.Value = this.entries[i].mode;
						break;
					}
				}
			}
		}

		// Token: 0x0400374E RID: 14158
		public MassiveSaveFlagEntry[] entries;

		// Token: 0x02000CA3 RID: 3235
		public enum SuccessType
		{
			// Token: 0x04003750 RID: 14160
			SetMode,
			// Token: 0x04003751 RID: 14161
			SendEvent
		}
	}
}
