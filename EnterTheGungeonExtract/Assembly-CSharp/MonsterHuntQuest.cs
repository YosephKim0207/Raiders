using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011BB RID: 4539
[Serializable]
public class MonsterHuntQuest
{
	// Token: 0x06006544 RID: 25924 RVA: 0x00276794 File Offset: 0x00274994
	public bool IsQuestComplete()
	{
		return GameStatsManager.Instance.GetFlag(this.QuestFlag);
	}

	// Token: 0x06006545 RID: 25925 RVA: 0x002767A8 File Offset: 0x002749A8
	public bool ContainsEnemy(string enemyGuid)
	{
		for (int i = 0; i < this.ValidTargetMonsterGuids.Count; i++)
		{
			if (this.ValidTargetMonsterGuids[i] == enemyGuid)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006546 RID: 25926 RVA: 0x002767EC File Offset: 0x002749EC
	public void UnlockRewards()
	{
		for (int i = 0; i < this.FlagsToSetUponReward.Count; i++)
		{
			GameStatsManager.Instance.SetFlag(this.FlagsToSetUponReward[i], true);
		}
	}

	// Token: 0x04006102 RID: 24834
	[SerializeField]
	[LongEnum]
	public GungeonFlags QuestFlag;

	// Token: 0x04006103 RID: 24835
	[SerializeField]
	public string QuestIntroString;

	// Token: 0x04006104 RID: 24836
	[SerializeField]
	public string TargetStringKey;

	// Token: 0x04006105 RID: 24837
	[EnemyIdentifier]
	[SerializeField]
	public List<string> ValidTargetMonsterGuids = new List<string>();

	// Token: 0x04006106 RID: 24838
	[SerializeField]
	public int NumberKillsRequired;

	// Token: 0x04006107 RID: 24839
	[SerializeField]
	[LongEnum]
	public List<GungeonFlags> FlagsToSetUponReward;
}
