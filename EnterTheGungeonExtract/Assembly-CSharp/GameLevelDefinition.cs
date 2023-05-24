using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012EB RID: 4843
[Serializable]
public class GameLevelDefinition
{
	// Token: 0x06006CB9 RID: 27833 RVA: 0x002AD004 File Offset: 0x002AB204
	public DungeonFlowLevelEntry LovinglySelectDungeonFlow()
	{
		List<DungeonFlowLevelEntry> list = new List<DungeonFlowLevelEntry>();
		float num = 0f;
		List<DungeonFlowLevelEntry> list2 = new List<DungeonFlowLevelEntry>();
		float num2 = 0f;
		for (int i = 0; i < this.flowEntries.Count; i++)
		{
			bool flag = true;
			for (int j = 0; j < this.flowEntries[i].prerequisites.Length; j++)
			{
				if (!this.flowEntries[i].prerequisites[j].CheckConditionsFulfilled())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (GameStatsManager.Instance.QueryFlowDifferentiator(this.flowEntries[i].flowPath) > 0)
				{
					num2 += this.flowEntries[i].weight;
					list2.Add(this.flowEntries[i]);
				}
				else
				{
					if (this.flowEntries[i].forceUseIfAvailable)
					{
						return this.flowEntries[i];
					}
					num += this.flowEntries[i].weight;
					list.Add(this.flowEntries[i]);
				}
			}
		}
		if (list.Count <= 0 && list2.Count > 0)
		{
			list = list2;
			num = num2;
		}
		if (list.Count <= 0)
		{
			return null;
		}
		float num3 = UnityEngine.Random.value * num;
		float num4 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num4 += list[k].weight;
			if (num4 >= num3)
			{
				return list[k];
			}
		}
		return null;
	}

	// Token: 0x040069CA RID: 27082
	public string dungeonSceneName;

	// Token: 0x040069CB RID: 27083
	public string dungeonPrefabPath;

	// Token: 0x040069CC RID: 27084
	public float priceMultiplier = 1f;

	// Token: 0x040069CD RID: 27085
	public float secretDoorHealthMultiplier = 1f;

	// Token: 0x040069CE RID: 27086
	public float enemyHealthMultiplier = 1f;

	// Token: 0x040069CF RID: 27087
	public float damageCap = -1f;

	// Token: 0x040069D0 RID: 27088
	public float bossDpsCap = -1f;

	// Token: 0x040069D1 RID: 27089
	public List<DungeonFlowLevelEntry> flowEntries;

	// Token: 0x040069D2 RID: 27090
	public List<int> predefinedSeeds;

	// Token: 0x040069D3 RID: 27091
	[NonSerialized]
	public DungeonFlowLevelEntry lastSelectedFlowEntry;
}
