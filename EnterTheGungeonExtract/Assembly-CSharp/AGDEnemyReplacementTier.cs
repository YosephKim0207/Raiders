using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001305 RID: 4869
[Serializable]
public class AGDEnemyReplacementTier
{
	// Token: 0x06006DC9 RID: 28105 RVA: 0x002B3234 File Offset: 0x002B1434
	public bool ExcludeForPrereqs()
	{
		return !DungeonPrerequisite.CheckConditionsFulfilled(this.Prereqs);
	}

	// Token: 0x06006DCA RID: 28106 RVA: 0x002B3244 File Offset: 0x002B1444
	public bool ExcludeRoomForColumns(DungeonData data, RoomHandler room)
	{
		if (!this.RoomMustHaveColumns)
		{
			return false;
		}
		for (int i = 0; i < room.area.dimensions.x; i++)
		{
			for (int j = 0; j < room.area.dimensions.y; j++)
			{
				CellData cellData = data[room.area.basePosition.x + i, room.area.basePosition.y + j];
				if (cellData != null && cellData.type == CellType.WALL && cellData.isRoomInternal)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06006DCB RID: 28107 RVA: 0x002B32EC File Offset: 0x002B14EC
	public bool ExcludeRoomForEnemies(RoomHandler room, List<AIActor> activeEnemies)
	{
		if (this.RoomCantContain.Count <= 0)
		{
			return false;
		}
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && this.RoomCantContain.Contains(aiactor.EnemyGuid))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006DCC RID: 28108 RVA: 0x002B3350 File Offset: 0x002B1550
	public bool ExcludeRoom(RoomHandler room)
	{
		return (this.RoomMinSize > 0 && (room.area.dimensions.x < this.RoomMinSize || room.area.dimensions.y < this.RoomMinSize)) || (this.RoomMinEnemyCount > 0 && room.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) < this.RoomMinEnemyCount) || (this.RoomMaxEnemyCount > 0 && room.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > this.RoomMaxEnemyCount);
	}

	// Token: 0x04006ADD RID: 27357
	public string Name;

	// Token: 0x04006ADE RID: 27358
	public DungeonPrerequisite[] Prereqs;

	// Token: 0x04006ADF RID: 27359
	[EnumFlags]
	public GlobalDungeonData.ValidTilesets TargetTileset;

	// Token: 0x04006AE0 RID: 27360
	public float ChanceToReplace = 0.2f;

	// Token: 0x04006AE1 RID: 27361
	public int MaxPerFloor = -1;

	// Token: 0x04006AE2 RID: 27362
	public int MaxPerRun = -1;

	// Token: 0x04006AE3 RID: 27363
	public bool TargetAllSignatureEnemies;

	// Token: 0x04006AE4 RID: 27364
	public bool TargetAllNonSignatureEnemies;

	// Token: 0x04006AE5 RID: 27365
	[EnemyIdentifier]
	public List<string> TargetGuids;

	// Token: 0x04006AE6 RID: 27366
	[EnemyIdentifier]
	public List<string> ReplacementGuids;

	// Token: 0x04006AE7 RID: 27367
	[Header("Exclusion Rules")]
	public bool RoomMustHaveColumns;

	// Token: 0x04006AE8 RID: 27368
	public int RoomMinEnemyCount = -1;

	// Token: 0x04006AE9 RID: 27369
	public int RoomMaxEnemyCount = -1;

	// Token: 0x04006AEA RID: 27370
	public int RoomMinSize = -1;

	// Token: 0x04006AEB RID: 27371
	[EnemyIdentifier]
	public List<string> RoomCantContain;

	// Token: 0x04006AEC RID: 27372
	[Header("Extras")]
	public bool RemoveAllOtherEnemies;
}
