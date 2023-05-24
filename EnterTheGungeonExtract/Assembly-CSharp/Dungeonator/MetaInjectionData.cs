using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EA4 RID: 3748
	public class MetaInjectionData : ScriptableObject
	{
		// Token: 0x06004F55 RID: 20309 RVA: 0x001B8104 File Offset: 0x001B6304
		public static void ClearBlueprint()
		{
			MetaInjectionData.BlueprintGenerated = false;
			MetaInjectionData.CastleExcludedSetUsed = false;
			if (MetaInjectionData.CurrentRunBlueprint != null)
			{
				MetaInjectionData.CurrentRunBlueprint.Clear();
			}
			MetaInjectionData.CellGeneratedForCurrentBlueprint = false;
			if (MetaInjectionData.InjectionSetsUsedThisRun != null)
			{
				MetaInjectionData.InjectionSetsUsedThisRun.Clear();
			}
			MetaInjectionData.KeybulletsAssignedToFloors.Clear();
			MetaInjectionData.ChanceBulletsAssignedToFloors.Clear();
			MetaInjectionData.WallMimicsAssignedToFloors.Clear();
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x001B816C File Offset: 0x001B636C
		public static int GetNumKeybulletMenForFloor(GlobalDungeonData.ValidTilesets tileset)
		{
			if (MetaInjectionData.KeybulletsAssignedToFloors.ContainsKey(tileset))
			{
				return MetaInjectionData.KeybulletsAssignedToFloors[tileset];
			}
			return 0;
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x001B818C File Offset: 0x001B638C
		public static int GetNumChanceBulletMenForFloor(GlobalDungeonData.ValidTilesets tileset)
		{
			if (MetaInjectionData.ChanceBulletsAssignedToFloors.ContainsKey(tileset))
			{
				return MetaInjectionData.ChanceBulletsAssignedToFloors[tileset];
			}
			return 0;
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001B81AC File Offset: 0x001B63AC
		public static int GetNumWallMimicsForFloor(GlobalDungeonData.ValidTilesets tileset)
		{
			if (MetaInjectionData.WallMimicsAssignedToFloors.ContainsKey(tileset))
			{
				return MetaInjectionData.WallMimicsAssignedToFloors[tileset];
			}
			return 0;
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001B81CC File Offset: 0x001B63CC
		public void PreprocessRun(bool doDebug = false)
		{
			if (MetaInjectionData.CurrentRunBlueprint == null)
			{
				MetaInjectionData.CurrentRunBlueprint = new Dictionary<GlobalDungeonData.ValidTilesets, List<RuntimeInjectionMetadata>>();
			}
			MetaInjectionData.CurrentRunBlueprint.Clear();
			MetaInjectionData.KeybulletsAssignedToFloors.Clear();
			MetaInjectionData.ChanceBulletsAssignedToFloors.Clear();
			MetaInjectionData.WallMimicsAssignedToFloors.Clear();
			RewardManager rewardManager = GameManager.Instance.RewardManager;
			rewardManager.KeybulletsChances.Select("keybulletmen", MetaInjectionData.KeybulletsAssignedToFloors);
			rewardManager.ChanceBulletChances.Select("chancebulletmen", MetaInjectionData.ChanceBulletsAssignedToFloors);
			rewardManager.WallMimicChances.Select("wallmimics", MetaInjectionData.WallMimicsAssignedToFloors);
			GlobalDungeonData.ValidTilesets[] array = Enum.GetValues(typeof(GlobalDungeonData.ValidTilesets)) as GlobalDungeonData.ValidTilesets[];
			for (int i = 0; i < this.entries.Count; i++)
			{
				float modifiedChanceToTrigger = this.entries[i].ModifiedChanceToTrigger;
				if (modifiedChanceToTrigger >= 1f || UnityEngine.Random.value <= modifiedChanceToTrigger)
				{
					int num = BraveRandom.GenerationRandomRange(this.entries[i].MinToAppearPerRun, this.entries[i].MaxToAppearPerRun + 1);
					if (this.entries[i].UsesWeightedNumberToAppearPerRun)
					{
						num = this.entries[i].WeightedNumberToAppear.SelectByWeight();
					}
					List<GlobalDungeonData.ValidTilesets> list = new List<GlobalDungeonData.ValidTilesets>();
					for (int j = 0; j < array.Length; j++)
					{
						if (!this.entries[i].IsPartOfExcludedCastleSet || !MetaInjectionData.CastleExcludedSetUsed || array[j] != GlobalDungeonData.ValidTilesets.CASTLEGEON)
						{
							if ((this.entries[i].validTilesets | array[j]) == this.entries[i].validTilesets)
							{
								list.Add(array[j]);
							}
						}
					}
					List<int> list2 = Enumerable.Range(0, list.Count).ToList<int>();
					list2 = list2.GenerationShuffle<int>();
					for (int k = 0; k < num; k++)
					{
						GlobalDungeonData.ValidTilesets validTilesets = list[list2[k]];
						if (!MetaInjectionData.CurrentRunBlueprint.ContainsKey(validTilesets))
						{
							MetaInjectionData.CurrentRunBlueprint.Add(validTilesets, new List<RuntimeInjectionMetadata>());
						}
						if (validTilesets == GlobalDungeonData.ValidTilesets.CASTLEGEON && this.entries[i].IsPartOfExcludedCastleSet)
						{
							MetaInjectionData.CastleExcludedSetUsed = true;
						}
						MetaInjectionData.CurrentRunBlueprint[validTilesets].Add(new RuntimeInjectionMetadata(this.entries[i].injectionData));
					}
					if (this.entries[i].AllowBonusSecret && num < list2.Count && BraveRandom.GenerationRandomValue() < this.entries[i].ChanceForBonusSecret)
					{
						GlobalDungeonData.ValidTilesets validTilesets2 = list[list2[num]];
						if (!MetaInjectionData.CurrentRunBlueprint.ContainsKey(validTilesets2))
						{
							MetaInjectionData.CurrentRunBlueprint.Add(validTilesets2, new List<RuntimeInjectionMetadata>());
						}
						RuntimeInjectionMetadata runtimeInjectionMetadata = new RuntimeInjectionMetadata(this.entries[i].injectionData);
						runtimeInjectionMetadata.forceSecret = true;
						MetaInjectionData.CurrentRunBlueprint[validTilesets2].Add(runtimeInjectionMetadata);
					}
				}
			}
			if (GameStatsManager.Instance.isChump)
			{
				MetaInjectionData.ForceEarlyChest = false;
			}
			else if (UnityEngine.Random.value < rewardManager.EarlyChestChanceIfNotChump)
			{
				MetaInjectionData.ForceEarlyChest = true;
			}
			MetaInjectionData.BlueprintGenerated = true;
		}

		// Token: 0x040046C5 RID: 18117
		public static bool CastleExcludedSetUsed = false;

		// Token: 0x040046C6 RID: 18118
		public static bool BlueprintGenerated = false;

		// Token: 0x040046C7 RID: 18119
		public static Dictionary<GlobalDungeonData.ValidTilesets, List<RuntimeInjectionMetadata>> CurrentRunBlueprint;

		// Token: 0x040046C8 RID: 18120
		public static List<ProceduralFlowModifierData> InjectionSetsUsedThisRun = new List<ProceduralFlowModifierData>();

		// Token: 0x040046C9 RID: 18121
		public static Dictionary<GlobalDungeonData.ValidTilesets, int> KeybulletsAssignedToFloors = new Dictionary<GlobalDungeonData.ValidTilesets, int>();

		// Token: 0x040046CA RID: 18122
		public static Dictionary<GlobalDungeonData.ValidTilesets, int> ChanceBulletsAssignedToFloors = new Dictionary<GlobalDungeonData.ValidTilesets, int>();

		// Token: 0x040046CB RID: 18123
		public static Dictionary<GlobalDungeonData.ValidTilesets, int> WallMimicsAssignedToFloors = new Dictionary<GlobalDungeonData.ValidTilesets, int>();

		// Token: 0x040046CC RID: 18124
		public static bool ForceEarlyChest = false;

		// Token: 0x040046CD RID: 18125
		public static bool CellGeneratedForCurrentBlueprint = false;

		// Token: 0x040046CE RID: 18126
		public List<MetaInjectionDataEntry> entries;
	}
}
