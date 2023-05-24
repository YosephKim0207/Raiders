using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA5 RID: 3237
	[Tooltip("Checks whether or not the player has a certain amount of money.")]
	[ActionCategory(".NPCs")]
	public class MonsterHuntSwitch : FsmStateAction
	{
		// Token: 0x0600452B RID: 17707 RVA: 0x001668AC File Offset: 0x00164AAC
		public override void Reset()
		{
			this.NeedsNewHunt = null;
			this.HuntIncomplete = null;
			this.HuntComplete = null;
			this.everyFrame = false;
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x001668CC File Offset: 0x00164ACC
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.NeedsNewHunt) && FsmEvent.IsNullOrEmpty(this.HuntIncomplete) && FsmEvent.IsNullOrEmpty(this.HuntComplete))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x0016690C File Offset: 0x00164B0C
		public override void OnEnter()
		{
			this.DoCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00166928 File Offset: 0x00164B28
		public override void OnUpdate()
		{
			this.DoCompare();
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00166930 File Offset: 0x00164B30
		private void DoCompare()
		{
			if (GameStatsManager.Instance.huntProgress.CurrentActiveMonsterHuntID <= -1)
			{
				if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE) && !GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_REWARD_GREY_MAUSER))
				{
					base.Fsm.Event(this.HuntComplete);
				}
				else
				{
					base.Fsm.Event(this.NeedsNewHunt);
				}
			}
			else if (GameStatsManager.Instance.huntProgress.CurrentActiveMonsterHuntProgress >= GameStatsManager.Instance.huntProgress.ActiveQuest.NumberKillsRequired)
			{
				base.Fsm.Event(this.HuntComplete);
			}
			else
			{
				base.Fsm.Event(this.HuntIncomplete);
			}
		}

		// Token: 0x04003754 RID: 14164
		public FsmEvent NeedsNewHunt;

		// Token: 0x04003755 RID: 14165
		public FsmEvent HuntIncomplete;

		// Token: 0x04003756 RID: 14166
		public FsmEvent HuntComplete;

		// Token: 0x04003757 RID: 14167
		public bool everyFrame;
	}
}
