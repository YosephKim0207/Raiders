using System;

// Token: 0x02001360 RID: 4960
[Serializable]
public class IndividualBossFloorEntry
{
	// Token: 0x06007069 RID: 28777 RVA: 0x002C954C File Offset: 0x002C774C
	public float GetWeightModifier()
	{
		int num = 0;
		for (int i = 0; i < this.TargetRoomTable.includedRooms.elements.Count; i++)
		{
			if (!(this.TargetRoomTable.includedRooms.elements[i].room == null))
			{
				int num2 = GameStatsManager.Instance.QueryRoomDifferentiator(this.TargetRoomTable.includedRooms.elements[i].room);
				num += num2;
			}
		}
		if (num <= 0)
		{
			if (GameStatsManager.Instance.LastBossEncounteredMap.ContainsKey(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId) && !BraveRandom.IgnoreGenerationDifferentiator && GameStatsManager.Instance.LastBossEncounteredMap[GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId] == this.TargetRoomTable.name)
			{
				return 0.5f;
			}
			return 1f;
		}
		else
		{
			if (num == 1)
			{
				return 0.5f;
			}
			if (num >= 2)
			{
				return 0.01f;
			}
			return 0.01f;
		}
	}

	// Token: 0x0600706A RID: 28778 RVA: 0x002C9674 File Offset: 0x002C7874
	public bool GlobalPrereqsValid()
	{
		for (int i = 0; i < this.GlobalBossPrerequisites.Length; i++)
		{
			if (!this.GlobalBossPrerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04006FDF RID: 28639
	public DungeonPrerequisite[] GlobalBossPrerequisites;

	// Token: 0x04006FE0 RID: 28640
	public float BossWeight = 1f;

	// Token: 0x04006FE1 RID: 28641
	public GenericRoomTable TargetRoomTable;
}
