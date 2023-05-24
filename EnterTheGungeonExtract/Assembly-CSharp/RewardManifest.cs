using System;
using System.Collections.Generic;

// Token: 0x02001491 RID: 5265
public static class RewardManifest
{
	// Token: 0x060077CA RID: 30666 RVA: 0x002FD430 File Offset: 0x002FB630
	public static void Initialize(RewardManager manager)
	{
		manager.SeededRunManifests = new Dictionary<GlobalDungeonData.ValidTilesets, FloorRewardManifest>();
		GlobalDungeonData.ValidTilesets[] array = (GlobalDungeonData.ValidTilesets[])Enum.GetValues(typeof(GlobalDungeonData.ValidTilesets));
		for (int i = 0; i < manager.FloorRewardData.Count; i++)
		{
			FloorRewardData floorRewardData = manager.FloorRewardData[i];
			foreach (GlobalDungeonData.ValidTilesets validTilesets in array)
			{
				if ((floorRewardData.AssociatedTilesets & validTilesets) == validTilesets)
				{
					FloorRewardManifest floorRewardManifest = RewardManifest.GenerateManifestForFloor(manager, floorRewardData);
					if (!manager.SeededRunManifests.ContainsKey(validTilesets))
					{
						manager.SeededRunManifests.Add(validTilesets, floorRewardManifest);
					}
				}
			}
		}
	}

	// Token: 0x060077CB RID: 30667 RVA: 0x002FD4E0 File Offset: 0x002FB6E0
	public static void Reinitialize(RewardManager manager)
	{
		GlobalDungeonData.ValidTilesets[] array = (GlobalDungeonData.ValidTilesets[])Enum.GetValues(typeof(GlobalDungeonData.ValidTilesets));
		for (int i = 0; i < manager.FloorRewardData.Count; i++)
		{
			FloorRewardData floorRewardData = manager.FloorRewardData[i];
			foreach (GlobalDungeonData.ValidTilesets validTilesets in array)
			{
				if ((floorRewardData.AssociatedTilesets & validTilesets) == validTilesets)
				{
					FloorRewardManifest floorRewardManifest = RewardManifest.GenerateManifestForFloor(manager, floorRewardData);
					if (manager.SeededRunManifests.ContainsKey(validTilesets))
					{
						RewardManifest.RegenerateManifest(manager, manager.SeededRunManifests[validTilesets]);
					}
				}
			}
		}
	}

	// Token: 0x060077CC RID: 30668 RVA: 0x002FD584 File Offset: 0x002FB784
	public static void ClearManifest(RewardManager manager)
	{
		manager.SeededRunManifests.Clear();
	}

	// Token: 0x060077CD RID: 30669 RVA: 0x002FD594 File Offset: 0x002FB794
	private static FloorRewardManifest GenerateManifestForFloor(RewardManager manager, FloorRewardData sourceData)
	{
		FloorRewardManifest floorRewardManifest = new FloorRewardManifest();
		floorRewardManifest.Initialize(manager);
		return floorRewardManifest;
	}

	// Token: 0x060077CE RID: 30670 RVA: 0x002FD5B0 File Offset: 0x002FB7B0
	private static void RegenerateManifest(RewardManager manager, FloorRewardManifest targetData)
	{
		targetData.Reinitialize(manager);
	}
}
