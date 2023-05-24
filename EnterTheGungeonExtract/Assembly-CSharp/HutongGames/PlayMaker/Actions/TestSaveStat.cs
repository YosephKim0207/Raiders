using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C7B RID: 3195
	[ActionCategory(".Brave")]
	[Tooltip("Sends Events based on the data in a player save.")]
	public class TestSaveStat : FsmStateAction
	{
		// Token: 0x06004496 RID: 17558 RVA: 0x0016283C File Offset: 0x00160A3C
		public override void Reset()
		{
			this.saveType = TestSaveStat.SaveType.Stat;
			this.stat = TrackedStats.BULLETS_FIRED;
			this.statGroup = TestSaveStat.StatGroup.Global;
			this.minValue = 0f;
			this.encounterGuid = string.Empty;
			this.Event = null;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0016287C File Offset: 0x00160A7C
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.saveType == TestSaveStat.SaveType.Stat)
			{
				if (this.minValue.Value <= 0f)
				{
					text += "Min Value must be greater than 0.\n";
				}
			}
			else if (this.saveType == TestSaveStat.SaveType.EncounteredTrackable)
			{
				if (EncounterDatabase.GetEntry(this.encounterGuid.Value) == null)
				{
					text += "Invalid encounter ID.\n";
				}
			}
			else if (this.saveType == TestSaveStat.SaveType.EncounteredRoom && string.IsNullOrEmpty(this.encounterId.Value))
			{
				text += "Invalid room ID.\n";
			}
			return text;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x00162920 File Offset: 0x00160B20
		public override void OnEnter()
		{
			this.DoCheck();
			base.Finish();
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x00162930 File Offset: 0x00160B30
		private void DoCheck()
		{
			float num = -1f;
			if (this.saveType == TestSaveStat.SaveType.Stat)
			{
				if (this.statGroup == TestSaveStat.StatGroup.Global)
				{
					num = GameStatsManager.Instance.GetPlayerStatValue(this.stat);
				}
				else if (this.statGroup == TestSaveStat.StatGroup.Character)
				{
					num = GameStatsManager.Instance.GetCharacterStatValue(this.stat);
				}
				else if (this.statGroup == TestSaveStat.StatGroup.Session)
				{
					num = GameStatsManager.Instance.GetSessionStatValue(this.stat);
				}
			}
			else if (this.saveType == TestSaveStat.SaveType.EncounteredTrackable)
			{
				num = (float)GameStatsManager.Instance.QueryEncounterable(this.encounterGuid.Value);
			}
			else if (this.saveType == TestSaveStat.SaveType.EncounteredRoom)
			{
				num = (float)GameStatsManager.Instance.QueryRoomEncountered(this.encounterId.Value);
			}
			if (num >= this.minValue.Value)
			{
				base.Fsm.Event(this.Event);
			}
		}

		// Token: 0x040036A2 RID: 13986
		[Tooltip("Type of save data to lookup.")]
		public TestSaveStat.SaveType saveType;

		// Token: 0x040036A3 RID: 13987
		[Tooltip("Stat to check")]
		public TrackedStats stat;

		// Token: 0x040036A4 RID: 13988
		public TestSaveStat.StatGroup statGroup;

		// Token: 0x040036A5 RID: 13989
		[Tooltip("Stat must be greather than or equal to this value to pass the test.")]
		public FsmFloat minValue;

		// Token: 0x040036A6 RID: 13990
		[Tooltip("The ID of the encounterable object.")]
		public FsmString encounterId;

		// Token: 0x040036A7 RID: 13991
		[Tooltip("The ID of the encounterable object.")]
		public FsmString encounterGuid;

		// Token: 0x040036A8 RID: 13992
		[Tooltip("The event to send if the test passes.")]
		public new FsmEvent Event;

		// Token: 0x02000C7C RID: 3196
		public enum SaveType
		{
			// Token: 0x040036AA RID: 13994
			Stat,
			// Token: 0x040036AB RID: 13995
			EncounteredTrackable,
			// Token: 0x040036AC RID: 13996
			EncounteredRoom
		}

		// Token: 0x02000C7D RID: 3197
		public enum StatGroup
		{
			// Token: 0x040036AE RID: 13998
			Global,
			// Token: 0x040036AF RID: 13999
			Character,
			// Token: 0x040036B0 RID: 14000
			Session
		}
	}
}
