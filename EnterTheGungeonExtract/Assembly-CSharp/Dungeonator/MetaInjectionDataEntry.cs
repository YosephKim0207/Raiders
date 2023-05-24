using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EA1 RID: 3745
	[Serializable]
	public class MetaInjectionDataEntry
	{
		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06004F50 RID: 20304 RVA: 0x001B7FF8 File Offset: 0x001B61F8
		public float ModifiedChanceToTrigger
		{
			get
			{
				if (this.UsesUnlockedChanceToTrigger && GameStatsManager.HasInstance)
				{
					for (int i = this.UnlockedChancesToTrigger.Length - 1; i >= 0; i--)
					{
						MetaInjectionUnlockedChanceEntry metaInjectionUnlockedChanceEntry = this.UnlockedChancesToTrigger[i];
						bool flag = true;
						for (int j = 0; j < metaInjectionUnlockedChanceEntry.prerequisites.Length; j++)
						{
							if (!metaInjectionUnlockedChanceEntry.prerequisites[j].CheckConditionsFulfilled())
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							Debug.LogError(string.Concat(new object[] { "chance to trigger: ", i, "|", metaInjectionUnlockedChanceEntry.ChanceToTrigger }));
							return metaInjectionUnlockedChanceEntry.ChanceToTrigger;
						}
					}
				}
				return this.OverallChanceToTrigger;
			}
		}

		// Token: 0x040046B2 RID: 18098
		public SharedInjectionData injectionData;

		// Token: 0x040046B3 RID: 18099
		public float OverallChanceToTrigger = 1f;

		// Token: 0x040046B4 RID: 18100
		public bool UsesUnlockedChanceToTrigger;

		// Token: 0x040046B5 RID: 18101
		public MetaInjectionUnlockedChanceEntry[] UnlockedChancesToTrigger;

		// Token: 0x040046B6 RID: 18102
		public int MinToAppearPerRun;

		// Token: 0x040046B7 RID: 18103
		public int MaxToAppearPerRun = 2;

		// Token: 0x040046B8 RID: 18104
		public bool UsesWeightedNumberToAppearPerRun;

		// Token: 0x040046B9 RID: 18105
		public WeightedIntCollection WeightedNumberToAppear;

		// Token: 0x040046BA RID: 18106
		public bool AllowBonusSecret;

		// Token: 0x040046BB RID: 18107
		[ShowInInspectorIf("AllowBonusSecret", false)]
		public float ChanceForBonusSecret = 0.5f;

		// Token: 0x040046BC RID: 18108
		public bool IsPartOfExcludedCastleSet;

		// Token: 0x040046BD RID: 18109
		[EnumFlags]
		public GlobalDungeonData.ValidTilesets validTilesets;
	}
}
