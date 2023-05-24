using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200135D RID: 4957
public class BossManager : ScriptableObject
{
	// Token: 0x06007060 RID: 28768 RVA: 0x002C9048 File Offset: 0x002C7248
	private BossFloorEntry GetBossDataForFloor(GlobalDungeonData.ValidTilesets targetTileset)
	{
		BossFloorEntry bossFloorEntry = null;
		for (int i = 0; i < this.BossFloorData.Count; i++)
		{
			if ((this.BossFloorData[i].AssociatedTilesets | targetTileset) == this.BossFloorData[i].AssociatedTilesets)
			{
				bossFloorEntry = this.BossFloorData[i];
			}
		}
		if (bossFloorEntry == null)
		{
			bossFloorEntry = this.BossFloorData[0];
		}
		return bossFloorEntry;
	}

	// Token: 0x06007061 RID: 28769 RVA: 0x002C90C0 File Offset: 0x002C72C0
	public PrototypeDungeonRoom SelectBossRoom()
	{
		if (BossManager.PriorFloorSelectedBossRoom != null)
		{
			return BossManager.PriorFloorSelectedBossRoom;
		}
		GenericRoomTable genericRoomTable = this.SelectBossTable();
		if (genericRoomTable == null)
		{
			genericRoomTable = this.GetBossDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId).Bosses[0].TargetRoomTable;
		}
		if (!BossManager.HasOverriddenCoreBoss)
		{
			for (int i = 0; i < this.OverrideBosses.Count; i++)
			{
				if (this.OverrideBosses[i].GlobalPrereqsValid(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId) && BraveRandom.GenerationRandomValue() < this.OverrideBosses[i].ChanceToOverride)
				{
					BossManager.HasOverriddenCoreBoss = true;
					Debug.Log("Boss overridden: " + this.OverrideBosses[i].Annotation);
					genericRoomTable = this.OverrideBosses[i].TargetRoomTable;
					break;
				}
			}
		}
		if (GameStatsManager.Instance.LastBossEncounteredMap.ContainsKey(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId))
		{
			GameStatsManager.Instance.LastBossEncounteredMap[GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId] = genericRoomTable.name;
		}
		else
		{
			GameStatsManager.Instance.LastBossEncounteredMap.Add(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId, genericRoomTable.name);
		}
		WeightedRoom weightedRoom = genericRoomTable.SelectByWeight();
		if (weightedRoom == null && genericRoomTable != null && genericRoomTable.includedRooms.elements.Count > 0)
		{
			weightedRoom = genericRoomTable.includedRooms.elements[0];
		}
		if (weightedRoom == null)
		{
			Debug.LogError("BOSS FAILED TO SELECT");
			return null;
		}
		BossManager.PriorFloorSelectedBossRoom = weightedRoom.room;
		return weightedRoom.room;
	}

	// Token: 0x06007062 RID: 28770 RVA: 0x002C92AC File Offset: 0x002C74AC
	public GenericRoomTable SelectBossTable()
	{
		BossFloorEntry bossDataForFloor = this.GetBossDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
		IndividualBossFloorEntry individualBossFloorEntry = bossDataForFloor.SelectBoss();
		return individualBossFloorEntry.TargetRoomTable;
	}

	// Token: 0x04006FD3 RID: 28627
	public static bool HasOverriddenCoreBoss;

	// Token: 0x04006FD4 RID: 28628
	public static PrototypeDungeonRoom PriorFloorSelectedBossRoom;

	// Token: 0x04006FD5 RID: 28629
	[SerializeField]
	public List<BossFloorEntry> BossFloorData;

	// Token: 0x04006FD6 RID: 28630
	[SerializeField]
	public List<OverrideBossFloorEntry> OverrideBosses;
}
