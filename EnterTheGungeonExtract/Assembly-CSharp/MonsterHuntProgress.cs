using System;
using System.Text;
using FullSerializer;
using UnityEngine;

// Token: 0x020011BA RID: 4538
[fsObject]
public class MonsterHuntProgress
{
	// Token: 0x0600653A RID: 25914 RVA: 0x002762D8 File Offset: 0x002744D8
	public void OnLoaded()
	{
		if (MonsterHuntProgress.Data == null)
		{
			MonsterHuntProgress.Data = (MonsterHuntData)BraveResources.Load("Monster Hunt Data", ".asset");
		}
		if (this.CurrentActiveMonsterHuntID != -1)
		{
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE) && GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_REWARD_GREY_MAUSER))
			{
				if (this.CurrentActiveMonsterHuntID < 0 || this.CurrentActiveMonsterHuntID >= MonsterHuntProgress.Data.ProceduralQuests.Count)
				{
					this.CurrentActiveMonsterHuntID = -1;
					this.CurrentActiveMonsterHuntProgress = 0;
				}
				else
				{
					this.ActiveQuest = MonsterHuntProgress.Data.ProceduralQuests[this.CurrentActiveMonsterHuntID];
				}
			}
			else if (this.CurrentActiveMonsterHuntID < 0 || this.CurrentActiveMonsterHuntID >= MonsterHuntProgress.Data.OrderedQuests.Count)
			{
				this.CurrentActiveMonsterHuntID = -1;
				this.CurrentActiveMonsterHuntProgress = 0;
			}
			else
			{
				this.ActiveQuest = MonsterHuntProgress.Data.OrderedQuests[this.CurrentActiveMonsterHuntID];
			}
		}
		else
		{
			this.CurrentActiveMonsterHuntProgress = 0;
		}
	}

	// Token: 0x0600653B RID: 25915 RVA: 0x002763FC File Offset: 0x002745FC
	public int TriggerNextQuest()
	{
		int num = 0;
		if (this.ActiveQuest != null)
		{
			this.ActiveQuest.UnlockRewards();
			num = 5;
		}
		for (int i = 0; i < MonsterHuntProgress.Data.OrderedQuests.Count; i++)
		{
			if (!GameStatsManager.Instance.GetFlag(MonsterHuntProgress.Data.OrderedQuests[i].QuestFlag))
			{
				this.ActiveQuest = MonsterHuntProgress.Data.OrderedQuests[i];
				this.CurrentActiveMonsterHuntID = i;
				this.CurrentActiveMonsterHuntProgress = 0;
				return num;
			}
		}
		int num2 = UnityEngine.Random.Range(0, MonsterHuntProgress.Data.ProceduralQuests.Count);
		this.ActiveQuest = MonsterHuntProgress.Data.ProceduralQuests[num2];
		this.CurrentActiveMonsterHuntID = num2;
		this.CurrentActiveMonsterHuntProgress = 0;
		return num;
	}

	// Token: 0x0600653C RID: 25916 RVA: 0x002764D0 File Offset: 0x002746D0
	public void ProcessStatuesKill()
	{
		if (this.ActiveQuest != null && this.ActiveQuest.QuestFlag == GungeonFlags.FRIFLE_MONSTERHUNT_14_COMPLETE)
		{
			if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
			{
				return;
			}
			this.CurrentActiveMonsterHuntProgress++;
			if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
			{
				this.Complete();
			}
		}
	}

	// Token: 0x0600653D RID: 25917 RVA: 0x00276540 File Offset: 0x00274740
	public void ForceIncrementKillCount()
	{
		if (this.ActiveQuest != null)
		{
			if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
			{
				return;
			}
			this.CurrentActiveMonsterHuntProgress++;
			if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
			{
				this.Complete();
			}
		}
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x0027659C File Offset: 0x0027479C
	public void ProcessKill(AIActor target)
	{
		if (this.ActiveQuest != null)
		{
			if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
			{
				return;
			}
			if (this.ActiveQuest.ContainsEnemy(target.EnemyGuid))
			{
				this.CurrentActiveMonsterHuntProgress++;
				if (this.CurrentActiveMonsterHuntProgress >= this.ActiveQuest.NumberKillsRequired)
				{
					this.Complete();
				}
			}
		}
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x0027660C File Offset: 0x0027480C
	public string GetReplacementString()
	{
		return StringTableManager.GetEnemiesString(this.ActiveQuest.TargetStringKey, -1);
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x00276620 File Offset: 0x00274820
	public string GetDisplayString()
	{
		if (this.CurrentActiveMonsterHuntID < 0)
		{
			return string.Empty;
		}
		if (this.m_sb == null)
		{
			this.m_sb = new StringBuilder();
		}
		this.m_sb.Length = 0;
		string enemiesString = StringTableManager.GetEnemiesString(this.ActiveQuest.TargetStringKey, -1);
		this.m_sb.Append(enemiesString);
		this.m_sb.Append(" ");
		this.m_sb.Append(this.CurrentActiveMonsterHuntProgress);
		this.m_sb.Append("/");
		this.m_sb.Append(this.ActiveQuest.NumberKillsRequired);
		return this.m_sb.ToString();
	}

	// Token: 0x06006541 RID: 25921 RVA: 0x002766D8 File Offset: 0x002748D8
	public bool IsQuestComplete()
	{
		return GameStatsManager.Instance.GetFlag(this.ActiveQuest.QuestFlag);
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x002766F0 File Offset: 0x002748F0
	public void Complete()
	{
		GameStatsManager.Instance.SetFlag(this.ActiveQuest.QuestFlag, true);
		if (GameUIRoot.Instance != null && GameUIRoot.Instance.notificationController != null)
		{
			tk2dSprite component = (ResourceCache.Acquire("Global VFX/Frifle_VictoryIcon") as GameObject).GetComponent<tk2dSprite>();
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetString("#HUNT_COMPLETE_HEADER"), StringTableManager.GetString("#HUNT_COMPLETE_BODY"), component.Collection, component.spriteId, UINotificationController.NotificationColor.GOLD, false, false);
		}
	}

	// Token: 0x040060FD RID: 24829
	[fsIgnore]
	private static MonsterHuntData Data;

	// Token: 0x040060FE RID: 24830
	[fsIgnore]
	public MonsterHuntQuest ActiveQuest;

	// Token: 0x040060FF RID: 24831
	[fsProperty]
	public int CurrentActiveMonsterHuntID = -1;

	// Token: 0x04006100 RID: 24832
	[fsProperty]
	public int CurrentActiveMonsterHuntProgress;

	// Token: 0x04006101 RID: 24833
	[fsIgnore]
	private StringBuilder m_sb;
}
