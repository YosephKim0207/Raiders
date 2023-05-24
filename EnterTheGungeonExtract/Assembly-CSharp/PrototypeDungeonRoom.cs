using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000F49 RID: 3913
[Serializable]
public class PrototypeDungeonRoom : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x0600543B RID: 21563 RVA: 0x001F855C File Offset: 0x001F675C
	private static Vector2 MirrorPosition(Vector2 position, PrototypeDungeonRoom room)
	{
		int num = Mathf.RoundToInt(position.x);
		int num2 = room.Width - 1 - num;
		return new Vector2((float)num2, position.y);
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x001F8590 File Offset: 0x001F6790
	public static bool GameObjectCanBeMirrored(GameObject g)
	{
		return !g.GetComponent<ConveyorBelt>() && !g.GetComponent<ForgeFlamePipeController>() && !g.GetComponent<ForgeCrushDoorController>() && !g.GetComponent<ForgeHammerController>() && !g.GetComponentInChildren<ProjectileTrapController>();
	}

	// Token: 0x0600543D RID: 21565 RVA: 0x001F85F0 File Offset: 0x001F67F0
	private static bool CanPlacedObjectBeMirrored(PrototypePlacedObjectData data)
	{
		if (Mathf.Abs(data.xMPxOffset) > 0)
		{
			return false;
		}
		if (data.placeableContents)
		{
			return data.placeableContents.IsValidMirrorPlaceable();
		}
		if (data.nonenemyBehaviour)
		{
			return PrototypeDungeonRoom.GameObjectCanBeMirrored(data.nonenemyBehaviour.gameObject);
		}
		return !data.unspecifiedContents || PrototypeDungeonRoom.GameObjectCanBeMirrored(data.unspecifiedContents);
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001F866C File Offset: 0x001F686C
	public static bool IsValidMirrorTarget(PrototypeDungeonRoom target)
	{
		if (target.category == PrototypeDungeonRoom.RoomCategory.BOSS || target.category == PrototypeDungeonRoom.RoomCategory.ENTRANCE || target.category == PrototypeDungeonRoom.RoomCategory.EXIT || target.category == PrototypeDungeonRoom.RoomCategory.REWARD || target.category == PrototypeDungeonRoom.RoomCategory.SPECIAL || target.category == PrototypeDungeonRoom.RoomCategory.SECRET)
		{
			return false;
		}
		if (target.PreventMirroring || target.IsLostWoodsRoom)
		{
			return false;
		}
		if (target.precludeAllTilemapDrawing || target.drawPrecludedCeilingTiles)
		{
			return false;
		}
		if (target.overriddenTilesets != (GlobalDungeonData.ValidTilesets)0)
		{
			return false;
		}
		if (target.paths.Count > 0)
		{
			return false;
		}
		for (int i = 0; i < target.placedObjects.Count; i++)
		{
			if (!PrototypeDungeonRoom.CanPlacedObjectBeMirrored(target.placedObjects[i]))
			{
				return false;
			}
		}
		for (int j = 0; j < target.additionalObjectLayers.Count; j++)
		{
			for (int k = 0; k < target.additionalObjectLayers[j].placedObjects.Count; k++)
			{
				if (!PrototypeDungeonRoom.CanPlacedObjectBeMirrored(target.additionalObjectLayers[j].placedObjects[k]))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600543F RID: 21567 RVA: 0x001F87B0 File Offset: 0x001F69B0
	public static PrototypeDungeonRoom MirrorRoom(PrototypeDungeonRoom sourceRoom)
	{
		IntVector2 intVector = new IntVector2(sourceRoom.m_width, sourceRoom.m_height);
		PrototypeDungeonRoom prototypeDungeonRoom = ScriptableObject.CreateInstance<PrototypeDungeonRoom>();
		prototypeDungeonRoom.MirrorSource = sourceRoom;
		prototypeDungeonRoom.category = sourceRoom.category;
		prototypeDungeonRoom.subCategoryNormal = sourceRoom.subCategoryNormal;
		prototypeDungeonRoom.subCategoryBoss = sourceRoom.subCategoryBoss;
		prototypeDungeonRoom.subCategorySecret = sourceRoom.subCategorySecret;
		prototypeDungeonRoom.subCategorySpecial = sourceRoom.subCategorySpecial;
		prototypeDungeonRoom.usesProceduralLighting = sourceRoom.usesProceduralLighting;
		prototypeDungeonRoom.usesProceduralDecoration = sourceRoom.usesProceduralDecoration;
		prototypeDungeonRoom.cullProceduralDecorationOnWeakPlatforms = sourceRoom.cullProceduralDecorationOnWeakPlatforms;
		prototypeDungeonRoom.allowFloorDecoration = sourceRoom.allowFloorDecoration;
		prototypeDungeonRoom.allowWallDecoration = sourceRoom.allowWallDecoration;
		prototypeDungeonRoom.preventAddedDecoLayering = sourceRoom.preventAddedDecoLayering;
		prototypeDungeonRoom.precludeAllTilemapDrawing = sourceRoom.precludeAllTilemapDrawing;
		prototypeDungeonRoom.drawPrecludedCeilingTiles = sourceRoom.drawPrecludedCeilingTiles;
		prototypeDungeonRoom.preventFacewallAO = sourceRoom.preventFacewallAO;
		prototypeDungeonRoom.preventBorders = sourceRoom.preventBorders;
		prototypeDungeonRoom.usesCustomAmbientLight = sourceRoom.usesCustomAmbientLight;
		prototypeDungeonRoom.customAmbientLight = sourceRoom.customAmbientLight;
		prototypeDungeonRoom.ForceAllowDuplicates = sourceRoom.ForceAllowDuplicates;
		prototypeDungeonRoom.doorTopDecorable = sourceRoom.doorTopDecorable;
		prototypeDungeonRoom.requiredInjectionData = sourceRoom.requiredInjectionData;
		prototypeDungeonRoom.injectionFlags = sourceRoom.injectionFlags;
		prototypeDungeonRoom.IsLostWoodsRoom = sourceRoom.IsLostWoodsRoom;
		prototypeDungeonRoom.UseCustomMusicState = sourceRoom.UseCustomMusicState;
		prototypeDungeonRoom.OverrideMusicState = sourceRoom.OverrideMusicState;
		prototypeDungeonRoom.UseCustomMusic = sourceRoom.UseCustomMusic;
		prototypeDungeonRoom.CustomMusicEvent = sourceRoom.CustomMusicEvent;
		prototypeDungeonRoom.RequiredCurseLevel = sourceRoom.RequiredCurseLevel;
		prototypeDungeonRoom.InvalidInCoop = sourceRoom.InvalidInCoop;
		prototypeDungeonRoom.overrideRoomVisualType = sourceRoom.overrideRoomVisualType;
		prototypeDungeonRoom.overrideRoomVisualTypeForSecretRooms = sourceRoom.overrideRoomVisualTypeForSecretRooms;
		prototypeDungeonRoom.rewardChestSpawnPosition = sourceRoom.rewardChestSpawnPosition;
		prototypeDungeonRoom.rewardChestSpawnPosition.x = intVector.x - (prototypeDungeonRoom.rewardChestSpawnPosition.x + 1);
		prototypeDungeonRoom.associatedMinimapIcon = sourceRoom.associatedMinimapIcon;
		prototypeDungeonRoom.overriddenTilesets = sourceRoom.overriddenTilesets;
		prototypeDungeonRoom.excludedOtherRooms = new List<PrototypeDungeonRoom>();
		for (int i = 0; i < sourceRoom.excludedOtherRooms.Count; i++)
		{
			prototypeDungeonRoom.excludedOtherRooms.Add(sourceRoom.excludedOtherRooms[i]);
		}
		prototypeDungeonRoom.prerequisites = new List<DungeonPrerequisite>();
		for (int j = 0; j < sourceRoom.prerequisites.Count; j++)
		{
			prototypeDungeonRoom.prerequisites.Add(sourceRoom.prerequisites[j]);
		}
		prototypeDungeonRoom.m_width = sourceRoom.m_width;
		prototypeDungeonRoom.m_height = sourceRoom.m_height;
		prototypeDungeonRoom.m_serializedCellType = new CellType[sourceRoom.m_serializedCellType.Length];
		prototypeDungeonRoom.m_serializedCellDataIndices = new List<int>();
		prototypeDungeonRoom.m_serializedCellDataData = new List<PrototypeDungeonRoomCellData>();
		for (int k = 0; k < sourceRoom.m_serializedCellType.Length; k++)
		{
			int num = k;
			int num2 = num % sourceRoom.m_width;
			int num3 = Mathf.FloorToInt((float)(num / sourceRoom.m_width));
			int num4 = sourceRoom.m_width - (num2 + 1);
			int num5 = num3 * sourceRoom.m_width + num4;
			prototypeDungeonRoom.m_serializedCellType[num5] = sourceRoom.m_serializedCellType[k];
		}
		for (int l = 0; l < sourceRoom.m_serializedCellDataIndices.Count; l++)
		{
			int num6 = sourceRoom.m_serializedCellDataIndices[l];
			int num7 = num6 % sourceRoom.m_width;
			int num8 = Mathf.FloorToInt((float)(num6 / sourceRoom.m_width));
			int num9 = sourceRoom.m_width - (num7 + 1);
			int num10 = num8 * sourceRoom.m_width + num9;
			prototypeDungeonRoom.m_serializedCellDataIndices.Add(num10);
			PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = sourceRoom.m_serializedCellDataData[l];
			PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = new PrototypeDungeonRoomCellData(prototypeDungeonRoomCellData.str, prototypeDungeonRoomCellData.state);
			prototypeDungeonRoomCellData2.MirrorData(prototypeDungeonRoomCellData);
			prototypeDungeonRoom.m_serializedCellDataData.Add(prototypeDungeonRoomCellData2);
		}
		List<int> list = new List<int>(prototypeDungeonRoom.m_serializedCellDataIndices);
		list.Sort();
		List<PrototypeDungeonRoomCellData> list2 = new List<PrototypeDungeonRoomCellData>(prototypeDungeonRoom.m_serializedCellDataData);
		for (int m = 0; m < list.Count; m++)
		{
			int num11 = list[m];
			int num12 = prototypeDungeonRoom.m_serializedCellDataIndices.IndexOf(num11);
			list2[m] = prototypeDungeonRoom.m_serializedCellDataData[num12];
		}
		prototypeDungeonRoom.m_serializedCellDataIndices = list;
		prototypeDungeonRoom.m_serializedCellDataData = list2;
		prototypeDungeonRoom.exitData = new PrototypeRoomExitData();
		prototypeDungeonRoom.exitData.MirrorData(sourceRoom.exitData, intVector);
		prototypeDungeonRoom.pits = new List<PrototypeRoomPitEntry>();
		for (int n = 0; n < sourceRoom.pits.Count; n++)
		{
			prototypeDungeonRoom.pits.Add(sourceRoom.pits[n].CreateMirror(intVector));
		}
		prototypeDungeonRoom.eventTriggerAreas = new List<PrototypeEventTriggerArea>();
		for (int num13 = 0; num13 < sourceRoom.eventTriggerAreas.Count; num13++)
		{
			prototypeDungeonRoom.eventTriggerAreas.Add(sourceRoom.eventTriggerAreas[num13].CreateMirror(intVector));
		}
		prototypeDungeonRoom.roomEvents = new List<RoomEventDefinition>();
		for (int num14 = 0; num14 < sourceRoom.roomEvents.Count; num14++)
		{
			prototypeDungeonRoom.roomEvents.Add(new RoomEventDefinition(sourceRoom.roomEvents[num14].condition, sourceRoom.roomEvents[num14].action));
		}
		prototypeDungeonRoom.placedObjects = new List<PrototypePlacedObjectData>();
		for (int num15 = 0; num15 < sourceRoom.placedObjects.Count; num15++)
		{
			prototypeDungeonRoom.placedObjects.Add(sourceRoom.placedObjects[num15].CreateMirror(intVector));
		}
		prototypeDungeonRoom.placedObjectPositions = new List<Vector2>();
		for (int num16 = 0; num16 < sourceRoom.placedObjectPositions.Count; num16++)
		{
			Vector2 vector = sourceRoom.placedObjectPositions[num16];
			vector.x = (float)intVector.x - (vector.x + (float)prototypeDungeonRoom.placedObjects[num16].GetWidth(true));
			prototypeDungeonRoom.placedObjectPositions.Add(vector);
		}
		prototypeDungeonRoom.additionalObjectLayers = new List<PrototypeRoomObjectLayer>();
		for (int num17 = 0; num17 < sourceRoom.additionalObjectLayers.Count; num17++)
		{
			prototypeDungeonRoom.additionalObjectLayers.Add(PrototypeRoomObjectLayer.CreateMirror(sourceRoom.additionalObjectLayers[num17], intVector));
		}
		prototypeDungeonRoom.rectangularFeatures = new List<PrototypeRectangularFeature>();
		for (int num18 = 0; num18 < sourceRoom.rectangularFeatures.Count; num18++)
		{
			prototypeDungeonRoom.rectangularFeatures.Add(PrototypeRectangularFeature.CreateMirror(sourceRoom.rectangularFeatures[num18], intVector));
		}
		prototypeDungeonRoom.paths = new List<SerializedPath>();
		for (int num19 = 0; num19 < sourceRoom.paths.Count; num19++)
		{
			prototypeDungeonRoom.paths.Add(SerializedPath.CreateMirror(sourceRoom.paths[num19], intVector, sourceRoom));
		}
		prototypeDungeonRoom.OnAfterDeserialize();
		prototypeDungeonRoom.UpdatePrecalculatedData();
		return prototypeDungeonRoom;
	}

	// Token: 0x06005440 RID: 21568 RVA: 0x001F8E90 File Offset: 0x001F7090
	public void RemovePathAt(int id)
	{
		this.paths.RemoveAt(id);
		for (int i = 0; i < this.placedObjects.Count; i++)
		{
			if (this.placedObjects[i].assignedPathIDx == id)
			{
				this.placedObjects[i].assignedPathIDx = -1;
			}
			else if (this.placedObjects[i].assignedPathIDx > id)
			{
				this.placedObjects[i].assignedPathIDx = this.placedObjects[i].assignedPathIDx - 1;
			}
		}
		foreach (PrototypeRoomObjectLayer prototypeRoomObjectLayer in this.additionalObjectLayers)
		{
			for (int j = 0; j < prototypeRoomObjectLayer.placedObjects.Count; j++)
			{
				if (prototypeRoomObjectLayer.placedObjects[j].assignedPathIDx == id)
				{
					prototypeRoomObjectLayer.placedObjects[j].assignedPathIDx = -1;
				}
				else if (prototypeRoomObjectLayer.placedObjects[j].assignedPathIDx > id)
				{
					prototypeRoomObjectLayer.placedObjects[j].assignedPathIDx = prototypeRoomObjectLayer.placedObjects[j].assignedPathIDx - 1;
				}
			}
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06005441 RID: 21569 RVA: 0x001F8FFC File Offset: 0x001F71FC
	public bool ContainsEnemies
	{
		get
		{
			if (this.placedObjects != null)
			{
				for (int i = 0; i < this.placedObjects.Count; i++)
				{
					PrototypePlacedObjectData prototypePlacedObjectData = this.placedObjects[i];
					if (prototypePlacedObjectData.placeableContents != null && prototypePlacedObjectData.placeableContents.ContainsEnemy)
					{
						return true;
					}
					if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
					{
						EnemyDatabaseEntry entry = EnemyDatabase.GetEntry(prototypePlacedObjectData.enemyBehaviourGuid);
						if (entry != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06005442 RID: 21570 RVA: 0x001F9088 File Offset: 0x001F7288
	public int MinDifficultyRating
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.placedObjects.Count; i++)
			{
				PrototypePlacedObjectData prototypePlacedObjectData = this.placedObjects[i];
				if (prototypePlacedObjectData == null)
				{
					Debug.LogError("Null object on room: " + base.name);
				}
				else
				{
					if (prototypePlacedObjectData.placeableContents != null)
					{
						num += prototypePlacedObjectData.placeableContents.GetMinimumDifficulty();
					}
					if (prototypePlacedObjectData.nonenemyBehaviour != null)
					{
						num += prototypePlacedObjectData.nonenemyBehaviour.GetMinimumDifficulty();
					}
					if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
					{
						num = num;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06005443 RID: 21571 RVA: 0x001F9134 File Offset: 0x001F7334
	public int MaxDifficultyRating
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.placedObjects.Count; i++)
			{
				PrototypePlacedObjectData prototypePlacedObjectData = this.placedObjects[i];
				if (prototypePlacedObjectData.placeableContents != null)
				{
					num += prototypePlacedObjectData.placeableContents.GetMaximumDifficulty();
				}
				if (prototypePlacedObjectData.nonenemyBehaviour != null)
				{
					num += prototypePlacedObjectData.nonenemyBehaviour.GetMaximumDifficulty();
				}
				if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
				{
					num = num;
				}
			}
			return num;
		}
	}

	// Token: 0x06005444 RID: 21572 RVA: 0x001F91C0 File Offset: 0x001F73C0
	public void OnBeforeSerialize()
	{
		this.m_serializedCellType = new CellType[this.m_cellData.Length];
		this.m_serializedCellDataIndices = new List<int>();
		this.m_serializedCellDataData = new List<PrototypeDungeonRoomCellData>();
		for (int i = 0; i < this.m_cellData.Length; i++)
		{
			PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.m_cellData[i];
			this.m_serializedCellType[i] = prototypeDungeonRoomCellData.state;
			if (prototypeDungeonRoomCellData.HasChanges())
			{
				this.m_serializedCellDataIndices.Add(i);
				this.m_serializedCellDataData.Add(prototypeDungeonRoomCellData);
			}
		}
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001F924C File Offset: 0x001F744C
	public void OnAfterDeserialize()
	{
		if (this.m_OLDcellData != null && this.m_OLDcellData.Length > 0)
		{
			this.m_cellData = this.m_OLDcellData;
			this.m_OLDcellData = new PrototypeDungeonRoomCellData[0];
			return;
		}
		this.m_cellData = new PrototypeDungeonRoomCellData[this.m_serializedCellType.Length];
		int num = 0;
		for (int i = 0; i < this.m_serializedCellType.Length; i++)
		{
			if (num < this.m_serializedCellDataIndices.Count && this.m_serializedCellDataIndices[num] == i)
			{
				this.m_cellData[i] = this.m_serializedCellDataData[num++];
			}
			else
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = new PrototypeDungeonRoomCellData();
				prototypeDungeonRoomCellData.appearance = new PrototypeDungeonRoomCellAppearance();
				prototypeDungeonRoomCellData.state = this.m_serializedCellType[i];
				this.m_cellData[i] = prototypeDungeonRoomCellData;
			}
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06005446 RID: 21574 RVA: 0x001F9324 File Offset: 0x001F7524
	// (set) Token: 0x06005447 RID: 21575 RVA: 0x001F932C File Offset: 0x001F752C
	public PrototypeDungeonRoomCellData[] FullCellData
	{
		get
		{
			return this.m_cellData;
		}
		set
		{
			this.m_cellData = value;
		}
	}

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06005448 RID: 21576 RVA: 0x001F9338 File Offset: 0x001F7538
	// (set) Token: 0x06005449 RID: 21577 RVA: 0x001F9340 File Offset: 0x001F7540
	public int Width
	{
		get
		{
			return this.m_width;
		}
		set
		{
			this.RecalculateCellDataArray(value, this.m_height, 0, 0);
			this.m_width = value;
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x0600544A RID: 21578 RVA: 0x001F9358 File Offset: 0x001F7558
	// (set) Token: 0x0600544B RID: 21579 RVA: 0x001F9360 File Offset: 0x001F7560
	public int Height
	{
		get
		{
			return this.m_height;
		}
		set
		{
			this.RecalculateCellDataArray(this.m_width, value, 0, 0);
			this.m_height = value;
		}
	}

	// Token: 0x0600544C RID: 21580 RVA: 0x001F9378 File Offset: 0x001F7578
	public bool CheckPrerequisites()
	{
		if (this.InvalidInCoop && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			return false;
		}
		if (this.RequiredCurseLevel > 0)
		{
			int totalCurse = PlayerStats.GetTotalCurse();
			if (totalCurse < this.RequiredCurseLevel)
			{
				return false;
			}
		}
		for (int i = 0; i < this.prerequisites.Count; i++)
		{
			if (!this.prerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600544D RID: 21581 RVA: 0x001F93F8 File Offset: 0x001F75F8
	public PrototypeEventTriggerArea AddEventTriggerArea(IEnumerable<IntVector2> cells)
	{
		PrototypeEventTriggerArea prototypeEventTriggerArea = new PrototypeEventTriggerArea(cells);
		this.eventTriggerAreas.Add(prototypeEventTriggerArea);
		return prototypeEventTriggerArea;
	}

	// Token: 0x0600544E RID: 21582 RVA: 0x001F941C File Offset: 0x001F761C
	public List<PrototypeEventTriggerArea> GetEventTriggerAreasAtPosition(IntVector2 position)
	{
		List<PrototypeEventTriggerArea> list = null;
		foreach (PrototypeEventTriggerArea prototypeEventTriggerArea in this.eventTriggerAreas)
		{
			if (prototypeEventTriggerArea.triggerCells.Contains(position.ToVector2()))
			{
				if (list == null)
				{
					list = new List<PrototypeEventTriggerArea>();
				}
				list.Add(prototypeEventTriggerArea);
			}
		}
		return list;
	}

	// Token: 0x0600544F RID: 21583 RVA: 0x001F94A0 File Offset: 0x001F76A0
	public void RemoveEventTriggerArea(PrototypeEventTriggerArea peta)
	{
		int num = this.eventTriggerAreas.IndexOf(peta);
		if (num < 0)
		{
			return;
		}
		this.eventTriggerAreas.Remove(peta);
		foreach (PrototypePlacedObjectData prototypePlacedObjectData in this.placedObjects)
		{
			for (int i = 0; i < prototypePlacedObjectData.linkedTriggerAreaIDs.Count; i++)
			{
				if (prototypePlacedObjectData.linkedTriggerAreaIDs[i] == num)
				{
					prototypePlacedObjectData.linkedTriggerAreaIDs.RemoveAt(i);
					i--;
				}
				else if (prototypePlacedObjectData.linkedTriggerAreaIDs[i] > num)
				{
					prototypePlacedObjectData.linkedTriggerAreaIDs[i] = prototypePlacedObjectData.linkedTriggerAreaIDs[i] - 1;
				}
			}
		}
		foreach (PrototypeRoomObjectLayer prototypeRoomObjectLayer in this.additionalObjectLayers)
		{
			foreach (PrototypePlacedObjectData prototypePlacedObjectData2 in prototypeRoomObjectLayer.placedObjects)
			{
				for (int j = 0; j < prototypePlacedObjectData2.linkedTriggerAreaIDs.Count; j++)
				{
					if (prototypePlacedObjectData2.linkedTriggerAreaIDs[j] == num)
					{
						prototypePlacedObjectData2.linkedTriggerAreaIDs.RemoveAt(j);
						j--;
					}
					else if (prototypePlacedObjectData2.linkedTriggerAreaIDs[j] > num)
					{
						prototypePlacedObjectData2.linkedTriggerAreaIDs[j] = prototypePlacedObjectData2.linkedTriggerAreaIDs[j] - 1;
					}
				}
			}
		}
	}

	// Token: 0x06005450 RID: 21584 RVA: 0x001F9694 File Offset: 0x001F7894
	public bool DoesUnsealOnClear()
	{
		for (int i = 0; i < this.roomEvents.Count; i++)
		{
			if (this.roomEvents[i].condition == RoomEventTriggerCondition.ON_ENEMIES_CLEARED && this.roomEvents[i].action == RoomEventTriggerAction.UNSEAL_ROOM)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x001F96F0 File Offset: 0x001F78F0
	public bool ContainsPit()
	{
		for (int i = 0; i < this.m_cellData.Length; i++)
		{
			if (this.m_cellData[i].state == CellType.PIT)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001F972C File Offset: 0x001F792C
	public PrototypeRoomPitEntry GetPitEntryFromPosition(IntVector2 position)
	{
		Vector2 vector = position.ToVector2();
		foreach (PrototypeRoomPitEntry prototypeRoomPitEntry in this.pits)
		{
			if (prototypeRoomPitEntry.containedCells.Contains(vector))
			{
				return prototypeRoomPitEntry;
			}
		}
		return null;
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001F97A4 File Offset: 0x001F79A4
	public void RedefineAllPitEntries()
	{
		this.pits = new List<PrototypeRoomPitEntry>();
		for (int i = 0; i < this.Width; i++)
		{
			for (int j = 0; j < this.Height; j++)
			{
				if (this.ForceGetCellDataAtPoint(i, j).state == CellType.PIT)
				{
					this.HandlePitCellsAddition(new IntVector2[]
					{
						new IntVector2(i, j)
					});
				}
			}
		}
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x001F981C File Offset: 0x001F7A1C
	public void HandlePitCellsAddition(IEnumerable<IntVector2> cells)
	{
		if (this.pits == null)
		{
			this.pits = new List<PrototypeRoomPitEntry>();
		}
		List<Vector2> list = new List<Vector2>();
		foreach (IntVector2 intVector in cells)
		{
			list.Add(intVector.ToVector2());
		}
		for (int i = this.pits.Count - 1; i >= 0; i--)
		{
			if (this.pits[i].IsAdjoining(list))
			{
				list.AddRange(this.pits[i].containedCells);
				this.pits.RemoveAt(i);
			}
		}
		this.pits.Add(new PrototypeRoomPitEntry(list));
	}

	// Token: 0x06005455 RID: 21589 RVA: 0x001F98FC File Offset: 0x001F7AFC
	public void HandlePitCellsRemoval(IEnumerable<IntVector2> cells)
	{
		if (this.pits == null)
		{
			this.pits = new List<PrototypeRoomPitEntry>();
		}
		HashSet<Vector2> hashSet = new HashSet<Vector2>();
		foreach (PrototypeRoomPitEntry prototypeRoomPitEntry in this.pits)
		{
			foreach (Vector2 vector in prototypeRoomPitEntry.containedCells)
			{
				hashSet.Add(vector);
			}
		}
		this.pits.Clear();
		foreach (IntVector2 intVector in cells)
		{
			hashSet.Remove(intVector.ToVector2());
		}
		List<Vector2> list = new List<Vector2>(hashSet);
		while (list.Count > 0)
		{
			Vector2 vector2 = list[0];
			list.RemoveAt(0);
			PrototypeRoomPitEntry prototypeRoomPitEntry2 = new PrototypeRoomPitEntry(vector2);
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (prototypeRoomPitEntry2.IsAdjoining(list[i]))
					{
						flag = true;
						prototypeRoomPitEntry2.containedCells.Add(list[i]);
						list.RemoveAt(i);
					}
				}
			}
			this.pits.Add(prototypeRoomPitEntry2);
		}
	}

	// Token: 0x06005456 RID: 21590 RVA: 0x001F9AC0 File Offset: 0x001F7CC0
	public void ClearAllObjectData()
	{
		for (int i = 0; i < this.m_cellData.Length; i++)
		{
			PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.m_cellData[i];
			prototypeDungeonRoomCellData.placedObjectRUBELIndex = -1;
			prototypeDungeonRoomCellData.additionalPlacedObjectIndices.Clear();
		}
	}

	// Token: 0x06005457 RID: 21591 RVA: 0x001F9B04 File Offset: 0x001F7D04
	public void DeleteRow(int yRow)
	{
		for (int i = 0; i < this.m_width; i++)
		{
			for (int j = yRow + 1; j < this.m_height; j++)
			{
				this.m_cellData[(j - 1) * this.m_width + i] = this.m_cellData[j * this.m_width + i];
			}
		}
		this.exitData.HandleRowColumnShift(-1, 0, yRow + 1, -1, this);
		this.Height--;
		this.TranslateAllObjectBasePositions(0, -1, 0, this.Width, yRow + 1, this.Height + 1);
	}

	// Token: 0x06005458 RID: 21592 RVA: 0x001F9BA0 File Offset: 0x001F7DA0
	public void DeleteColumn(int xCol)
	{
		for (int i = 0; i < this.m_height; i++)
		{
			for (int j = xCol + 1; j < this.m_width; j++)
			{
				this.m_cellData[i * this.m_width + (j - 1)] = this.m_cellData[i * this.m_width + j];
			}
		}
		this.Width--;
		this.exitData.HandleRowColumnShift(xCol + 1, -1, -1, 0, this);
		this.TranslateAllObjectBasePositions(-1, 0, xCol + 1, this.Width + 1, 0, this.Height);
	}

	// Token: 0x06005459 RID: 21593 RVA: 0x001F9C3C File Offset: 0x001F7E3C
	public bool CheckRegionOccupied(int xPos, int yPos, int w, int h)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.ForceGetCellDataAtPoint(xPos + i, yPos + j);
				if (prototypeDungeonRoomCellData == null)
				{
					return true;
				}
				if (prototypeDungeonRoomCellData.IsOccupied)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600545A RID: 21594 RVA: 0x001F9C94 File Offset: 0x001F7E94
	public bool CheckRegionOccupiedExcludeWallsAndPits(int xPos, int yPos, int w, int h, bool includeTopwalls = true)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.ForceGetCellDataAtPoint(xPos + i, yPos + j);
				if (prototypeDungeonRoomCellData == null)
				{
					return true;
				}
				if (prototypeDungeonRoomCellData.state != CellType.FLOOR)
				{
					return true;
				}
				if (prototypeDungeonRoomCellData.IsOccupied)
				{
					return true;
				}
				if (!includeTopwalls)
				{
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = this.ForceGetCellDataAtPoint(xPos + i, yPos + j - 1);
					if (prototypeDungeonRoomCellData2 == null || prototypeDungeonRoomCellData2.state == CellType.WALL)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001F9D24 File Offset: 0x001F7F24
	public bool CheckRegionOccupied(int xPos, int yPos, int w, int h, int objectLayerIndex)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (this.ForceGetCellDataAtPoint(xPos + i, yPos + j).IsOccupiedAtLayer(objectLayerIndex))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x001F9D74 File Offset: 0x001F7F74
	public List<IntVector2> GetCellRepresentation(IntVector2 worldBasePosition)
	{
		List<IntVector2> list = new List<IntVector2>();
		for (int i = 0; i < this.m_height; i++)
		{
			for (int j = 0; j < this.m_width; j++)
			{
				PrototypeDungeonRoomCellData cellDataAtPoint = this.GetCellDataAtPoint(j, i);
				if (cellDataAtPoint != null)
				{
					if (cellDataAtPoint.state == CellType.FLOOR || cellDataAtPoint.state == CellType.PIT || (cellDataAtPoint.state == CellType.WALL && cellDataAtPoint.breakable))
					{
						IntVector2 intVector = worldBasePosition + new IntVector2(j, i);
						list.Add(intVector);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x0600545D RID: 21597 RVA: 0x001F9E10 File Offset: 0x001F8010
	public void UpdatePrecalculatedData()
	{
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = 0; i < this.m_height; i++)
		{
			for (int j = 0; j < this.m_width; j++)
			{
				PrototypeDungeonRoomCellData cellDataAtPoint = this.GetCellDataAtPoint(j, i);
				if (cellDataAtPoint != null)
				{
					if (cellDataAtPoint.state == CellType.FLOOR || cellDataAtPoint.state == CellType.PIT || (cellDataAtPoint.state == CellType.WALL && cellDataAtPoint.breakable))
					{
						IntVector2 intVector = new IntVector2(j, i);
						hashSet.Add(intVector);
						hashSet.Add(intVector + IntVector2.Up);
						hashSet.Add(intVector + IntVector2.Up * 2);
						hashSet.Add(new IntVector2(intVector.x + 1, intVector.y));
						hashSet.Add(new IntVector2(intVector.x + 1, intVector.y + 1));
						hashSet.Add(new IntVector2(intVector.x + 1, intVector.y + 2));
						hashSet.Add(new IntVector2(intVector.x - 1, intVector.y));
						hashSet.Add(new IntVector2(intVector.x - 1, intVector.y + 1));
						hashSet.Add(new IntVector2(intVector.x - 1, intVector.y + 2));
						hashSet.Add(new IntVector2(intVector.x, intVector.y + 3));
						hashSet.Add(new IntVector2(intVector.x, intVector.y - 1));
						hashSet.Add(new IntVector2(intVector.x - 1, intVector.y - 1));
						hashSet.Add(new IntVector2(intVector.x + 1, intVector.y - 1));
						hashSet.Add(new IntVector2(intVector.x - 1, intVector.y + 3));
						hashSet.Add(new IntVector2(intVector.x + 1, intVector.y + 3));
					}
				}
			}
		}
		UnityEngine.Random.InitState(base.name.GetHashCode());
		List<IntVector2> list = new List<IntVector2>(hashSet).Shuffle<IntVector2>();
		this.m_cachedRepresentationIncFacewalls = list;
	}

	// Token: 0x0600545E RID: 21598 RVA: 0x001FA058 File Offset: 0x001F8258
	public List<IntVector2> GetCellRepresentationIncFacewalls()
	{
		if (this.m_cachedRepresentationIncFacewalls != null && this.m_cachedRepresentationIncFacewalls.Count > 0)
		{
			return this.m_cachedRepresentationIncFacewalls;
		}
		Debug.LogError("PROTOTYPE DUNGEON ROOM: " + base.name + " IS MISSING PRECALCULATED DATA.");
		return null;
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x001FA098 File Offset: 0x001F8298
	public PrototypeDungeonRoomCellData GetCellDataAtPoint(int ix, int iy)
	{
		return this.ForceGetCellDataAtPoint(ix, iy);
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x001FA0B0 File Offset: 0x001F82B0
	public PrototypeDungeonRoomCellData ForceGetCellDataAtPoint(int ix, int iy)
	{
		if (this.m_cellData == null || this.m_cellData.Length != this.m_width * this.m_height)
		{
			this.InitializeArray(this.m_width, this.m_height);
		}
		if (iy < 0 || ix < 0 || ix >= this.m_width || iy >= this.m_height)
		{
			return null;
		}
		if (iy * this.m_width + ix < 0 || iy * this.m_width + ix >= this.m_cellData.Length)
		{
			return null;
		}
		return this.m_cellData[iy * this.m_width + ix];
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001FA158 File Offset: 0x001F8358
	public PrototypeRoomExit GetExitDataAtPoint(int ix, int iy)
	{
		return this.exitData[ix, iy];
	}

	// Token: 0x06005462 RID: 21602 RVA: 0x001FA168 File Offset: 0x001F8368
	private bool IsValidCellDataPosition(int ix, int iy)
	{
		return iy >= 0 && iy < this.m_height && ix >= 0 && ix < this.m_width;
	}

	// Token: 0x06005463 RID: 21603 RVA: 0x001FA190 File Offset: 0x001F8390
	public bool ProcessExitPosition(int ix, int iy)
	{
		return this.exitData.ProcessExitPosition(ix, iy, this);
	}

	// Token: 0x06005464 RID: 21604 RVA: 0x001FA1A0 File Offset: 0x001F83A0
	public bool HasFloorNeighbor(int ix, int iy)
	{
		if (this.m_cellData == null || this.m_cellData.Length != this.m_width * this.m_height)
		{
			this.InitializeArray(this.m_width, this.m_height);
		}
		if (ix == -1 || iy == -1 || ix == this.m_width || iy == this.m_height)
		{
			return this.BoundaryHasFloorNeighbor(ix, iy);
		}
		return (iy < this.m_height - 1 && this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].state == CellType.FLOOR) || (iy > 0 && this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].state == CellType.FLOOR) || (ix < this.m_width - 1 && this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].state == CellType.FLOOR) || (ix > 0 && this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].state == CellType.FLOOR);
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x001FA2FC File Offset: 0x001F84FC
	public bool HasBreakableNeighbor(int ix, int iy)
	{
		if (this.m_cellData == null || this.m_cellData.Length != this.m_width * this.m_height)
		{
			this.InitializeArray(this.m_width, this.m_height);
		}
		return (iy < this.m_height - 1 && this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].breakable) || (iy > 0 && this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].breakable) || (ix < this.m_width - 1 && this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].breakable) || (ix > 0 && this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].breakable);
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x001FA424 File Offset: 0x001F8624
	public bool HasNonWallNeighbor(int ix, int iy)
	{
		if (this.m_cellData == null || this.m_cellData.Length != this.m_width * this.m_height)
		{
			this.InitializeArray(this.m_width, this.m_height);
		}
		return (this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].state != CellType.WALL) || (this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].state != CellType.WALL) || (this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].state != CellType.WALL);
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x001FA528 File Offset: 0x001F8728
	public bool HasNonWallNeighborWithDiagonals(int ix, int iy)
	{
		if (this.m_cellData == null || this.m_cellData.Length != this.m_width * this.m_height)
		{
			this.InitializeArray(this.m_width, this.m_height);
		}
		return (this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].state != CellType.WALL) || (this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].state != CellType.WALL) || (this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix + 1, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + (ix + 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix + 1, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + (ix + 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix - 1, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + (ix - 1)].state != CellType.WALL) || (this.IsValidCellDataPosition(ix - 1, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + (ix - 1)].state != CellType.WALL);
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x001FA6F8 File Offset: 0x001F88F8
	private bool BoundaryHasFloorNeighbor(int ix, int iy)
	{
		return (ix == -1 && this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].state == CellType.FLOOR) || (ix == this.m_width && this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].state == CellType.FLOOR) || (iy == -1 && this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].state == CellType.FLOOR) || (iy == this.m_height && this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].state == CellType.FLOOR);
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x001FA7E8 File Offset: 0x001F89E8
	public DungeonData.Direction GetFloorDirection(int ix, int iy)
	{
		if (iy < this.m_height - 1 && this.IsValidCellDataPosition(ix, iy + 1) && this.m_cellData[(iy + 1) * this.m_width + ix].state == CellType.FLOOR)
		{
			return DungeonData.Direction.NORTH;
		}
		if (iy > 0 && this.IsValidCellDataPosition(ix, iy - 1) && this.m_cellData[(iy - 1) * this.m_width + ix].state == CellType.FLOOR)
		{
			return DungeonData.Direction.SOUTH;
		}
		if (ix < this.m_width - 1 && this.IsValidCellDataPosition(ix + 1, iy) && this.m_cellData[iy * this.m_width + (ix + 1)].state == CellType.FLOOR)
		{
			return DungeonData.Direction.EAST;
		}
		if (ix > 0 && this.IsValidCellDataPosition(ix - 1, iy) && this.m_cellData[iy * this.m_width + (ix - 1)].state == CellType.FLOOR)
		{
			return DungeonData.Direction.WEST;
		}
		return DungeonData.Direction.SOUTHWEST;
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x001FA8DC File Offset: 0x001F8ADC
	private void InitializeArray(int w, int h)
	{
		this.m_cellData = new PrototypeDungeonRoomCellData[w * h];
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				this.m_cellData[j * w + i] = new PrototypeDungeonRoomCellData(string.Empty, CellType.FLOOR);
			}
		}
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x001FA934 File Offset: 0x001F8B34
	public void TranslateAndResize(int newWidth, int newHeight, int xTrans, int yTrans)
	{
		this.RecalculateCellDataArray(newWidth, newHeight, xTrans, yTrans);
		int num = Math.Max(this.m_width, newWidth);
		int num2 = Math.Max(this.m_height, newHeight);
		this.m_width = newWidth;
		this.m_height = newHeight;
		this.exitData.TranslateAllExits(xTrans, yTrans, this);
		this.TranslateAllObjectBasePositions(xTrans, yTrans, 0, num, 0, num2);
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x001FA990 File Offset: 0x001F8B90
	private void TranslateAllObjectBasePositions(int deltaX, int deltaY, int startX, int endX, int startY, int endY)
	{
		foreach (PrototypePlacedObjectData prototypePlacedObjectData in this.placedObjects)
		{
			if (prototypePlacedObjectData.contentsBasePosition.x >= (float)startX && prototypePlacedObjectData.contentsBasePosition.x < (float)endX && prototypePlacedObjectData.contentsBasePosition.y >= (float)startY && prototypePlacedObjectData.contentsBasePosition.y < (float)endY)
			{
				prototypePlacedObjectData.contentsBasePosition += new Vector2((float)deltaX, (float)deltaY);
			}
		}
		for (int i = 0; i < this.placedObjectPositions.Count; i++)
		{
			if (this.placedObjectPositions[i].x >= (float)startX && this.placedObjectPositions[i].x < (float)endX && this.placedObjectPositions[i].y >= (float)startY && this.placedObjectPositions[i].y < (float)endY)
			{
				this.placedObjectPositions[i] = this.placedObjectPositions[i] + new Vector2((float)deltaX, (float)deltaY);
			}
		}
		foreach (PrototypeRoomObjectLayer prototypeRoomObjectLayer in this.additionalObjectLayers)
		{
			foreach (PrototypePlacedObjectData prototypePlacedObjectData2 in prototypeRoomObjectLayer.placedObjects)
			{
				if (prototypePlacedObjectData2.contentsBasePosition.x >= (float)startX && prototypePlacedObjectData2.contentsBasePosition.x < (float)endX && prototypePlacedObjectData2.contentsBasePosition.y >= (float)startY && prototypePlacedObjectData2.contentsBasePosition.y < (float)endY)
				{
					prototypePlacedObjectData2.contentsBasePosition += new Vector2((float)deltaX, (float)deltaY);
				}
			}
			for (int j = 0; j < prototypeRoomObjectLayer.placedObjectBasePositions.Count; j++)
			{
				if (prototypeRoomObjectLayer.placedObjectBasePositions[j].x >= (float)startX && prototypeRoomObjectLayer.placedObjectBasePositions[j].x < (float)endX && prototypeRoomObjectLayer.placedObjectBasePositions[j].y >= (float)startY && prototypeRoomObjectLayer.placedObjectBasePositions[j].y < (float)endY)
				{
					prototypeRoomObjectLayer.placedObjectBasePositions[j] = prototypeRoomObjectLayer.placedObjectBasePositions[j] + new Vector2((float)deltaX, (float)deltaY);
				}
			}
		}
		this.ClearAndRebuildObjectCellData();
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x001FACE8 File Offset: 0x001F8EE8
	public List<PrototypeRoomExit> GetExitsMatchingDirection(DungeonData.Direction dir, PrototypeRoomExit.ExitType exitType)
	{
		List<PrototypeRoomExit> list = new List<PrototypeRoomExit>();
		for (int i = 0; i < this.exitData.exits.Count; i++)
		{
			if (this.exitData.exits[i].exitDirection == dir)
			{
				if (exitType == PrototypeRoomExit.ExitType.NO_RESTRICTION)
				{
					list.Add(this.exitData.exits[i]);
				}
				else if (exitType == PrototypeRoomExit.ExitType.EXIT_ONLY && this.exitData.exits[i].exitType != PrototypeRoomExit.ExitType.ENTRANCE_ONLY)
				{
					list.Add(this.exitData.exits[i]);
				}
				else if (exitType == PrototypeRoomExit.ExitType.ENTRANCE_ONLY && this.exitData.exits[i].exitType != PrototypeRoomExit.ExitType.EXIT_ONLY)
				{
					list.Add(this.exitData.exits[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x001FADD4 File Offset: 0x001F8FD4
	public List<Tuple<PrototypeRoomExit, PrototypeRoomExit>> GetExitPairsMatchingDirections(DungeonData.Direction dir1, DungeonData.Direction dir2)
	{
		List<Tuple<PrototypeRoomExit, PrototypeRoomExit>> list = new List<Tuple<PrototypeRoomExit, PrototypeRoomExit>>();
		for (int i = 0; i < this.exitData.exits.Count; i++)
		{
			PrototypeRoomExit prototypeRoomExit = this.exitData.exits[i];
			for (int j = 0; j < this.exitData.exits.Count; j++)
			{
				PrototypeRoomExit prototypeRoomExit2 = this.exitData.exits[j];
				if (prototypeRoomExit.exitDirection == dir1 && prototypeRoomExit2.exitDirection == dir2)
				{
					list.Add(new Tuple<PrototypeRoomExit, PrototypeRoomExit>(prototypeRoomExit, prototypeRoomExit2));
				}
			}
		}
		return list;
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x001FAE78 File Offset: 0x001F9078
	public void ClearAndRebuildObjectCellData()
	{
		if (this.m_cellData == null)
		{
			return;
		}
		foreach (PrototypeDungeonRoomCellData prototypeDungeonRoomCellData in this.m_cellData)
		{
			prototypeDungeonRoomCellData.placedObjectRUBELIndex = -1;
			prototypeDungeonRoomCellData.additionalPlacedObjectIndices.Clear();
		}
		this.RebuildObjectCellData();
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x001FAEC8 File Offset: 0x001F90C8
	public void RebuildObjectCellData()
	{
		using (List<PrototypePlacedObjectData>.Enumerator enumerator = this.placedObjects.GetEnumerator())
		{
			IL_183:
			while (enumerator.MoveNext())
			{
				PrototypePlacedObjectData prototypePlacedObjectData = enumerator.Current;
				Vector2 contentsBasePosition = prototypePlacedObjectData.contentsBasePosition;
				int i = 0;
				while (i < this.Height)
				{
					for (int j = 0; j < this.Width; j++)
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.ForceGetCellDataAtPoint(j, i);
						if (prototypeDungeonRoomCellData != null && prototypeDungeonRoomCellData.placedObjectRUBELIndex >= 0 && this.placedObjects[prototypeDungeonRoomCellData.placedObjectRUBELIndex] == prototypePlacedObjectData)
						{
							if (!(contentsBasePosition != vector))
							{
								goto IL_17E;
							}
							prototypeDungeonRoomCellData.placedObjectRUBELIndex = -1;
						}
					}
					i++;
					continue;
					IL_17E:
					goto IL_183;
				}
				if (prototypePlacedObjectData == null)
				{
					Debug.LogError("null object data at placed object index!");
				}
				else
				{
					int num = (int)contentsBasePosition.x;
					while ((float)num < contentsBasePosition.x + (float)prototypePlacedObjectData.GetWidth(false))
					{
						int num2 = (int)contentsBasePosition.y;
						while ((float)num2 < contentsBasePosition.y + (float)prototypePlacedObjectData.GetHeight(false))
						{
							if (num2 * this.Width + num >= 0 && num2 * this.Width + num < this.m_cellData.Length)
							{
								PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = this.ForceGetCellDataAtPoint(num, num2);
								if (prototypeDungeonRoomCellData2 != null)
								{
									prototypeDungeonRoomCellData2.placedObjectRUBELIndex = this.placedObjects.IndexOf(prototypePlacedObjectData);
								}
							}
							num2++;
						}
						num++;
					}
				}
			}
		}
		foreach (PrototypeRoomObjectLayer prototypeRoomObjectLayer in this.additionalObjectLayers)
		{
			int num3 = this.additionalObjectLayers.IndexOf(prototypeRoomObjectLayer);
			int k = 0;
			IL_3E0:
			while (k < prototypeRoomObjectLayer.placedObjects.Count)
			{
				PrototypePlacedObjectData prototypePlacedObjectData2 = prototypeRoomObjectLayer.placedObjects[k];
				Vector2 contentsBasePosition2 = prototypePlacedObjectData2.contentsBasePosition;
				int l = 0;
				while (l < this.Height)
				{
					for (int m = 0; m < this.Width; m++)
					{
						Vector2 vector2 = new Vector2((float)m, (float)l);
						PrototypeDungeonRoomCellData prototypeDungeonRoomCellData3 = this.ForceGetCellDataAtPoint(m, l);
						if (prototypeDungeonRoomCellData3 != null)
						{
							int num4 = ((prototypeDungeonRoomCellData3.additionalPlacedObjectIndices.Count <= num3) ? (-1) : prototypeDungeonRoomCellData3.additionalPlacedObjectIndices[num3]);
							if (num4 >= 0 && prototypeRoomObjectLayer.placedObjects[num4] == prototypePlacedObjectData2)
							{
								if (!(contentsBasePosition2 != vector2))
								{
									goto IL_3D5;
								}
								prototypeDungeonRoomCellData3.additionalPlacedObjectIndices[num3] = -1;
							}
						}
					}
					l++;
					continue;
					IL_3D5:
					IL_3DA:
					k++;
					goto IL_3E0;
				}
				if (prototypePlacedObjectData2 == null)
				{
					Debug.LogError("null object data at placed object index in layer: " + this.additionalObjectLayers.IndexOf(prototypeRoomObjectLayer));
					goto IL_3DA;
				}
				int num5 = (int)contentsBasePosition2.x;
				while ((float)num5 < contentsBasePosition2.x + (float)prototypePlacedObjectData2.GetWidth(false))
				{
					int num6 = (int)contentsBasePosition2.y;
					while ((float)num6 < contentsBasePosition2.y + (float)prototypePlacedObjectData2.GetHeight(false))
					{
						if (num6 * this.Width + num5 >= 0 && num6 * this.Width + num5 < this.m_cellData.Length)
						{
							PrototypeDungeonRoomCellData prototypeDungeonRoomCellData4 = this.ForceGetCellDataAtPoint(num5, num6);
							if (prototypeDungeonRoomCellData4 != null)
							{
								if (prototypeDungeonRoomCellData4.additionalPlacedObjectIndices.Count <= num3)
								{
									while (prototypeDungeonRoomCellData4.additionalPlacedObjectIndices.Count <= num3)
									{
										prototypeDungeonRoomCellData4.additionalPlacedObjectIndices.Add(-1);
									}
								}
								prototypeDungeonRoomCellData4.additionalPlacedObjectIndices[num3] = prototypeRoomObjectLayer.placedObjects.IndexOf(prototypePlacedObjectData2);
							}
						}
						num6++;
					}
					num5++;
				}
				goto IL_3D5;
			}
		}
	}

	// Token: 0x06005471 RID: 21617 RVA: 0x001FB31C File Offset: 0x001F951C
	private void RecalculateCellDataArray(int newWidth, int newHeight, int xTrans = 0, int yTrans = 0)
	{
		if (this.m_cellData == null)
		{
			this.InitializeArray(newWidth, newHeight);
		}
		else
		{
			PrototypeDungeonRoomCellData[] array = new PrototypeDungeonRoomCellData[newWidth * newHeight];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					if (i < newWidth && j < newHeight)
					{
						int num = i + xTrans;
						int num2 = j + yTrans;
						if (num >= 0 && num < newWidth && num2 >= 0 && num2 < newHeight)
						{
							array[num2 * newWidth + num] = this.ForceGetCellDataAtPoint(i, j);
						}
					}
				}
			}
			for (int k = 0; k < newWidth; k++)
			{
				for (int l = 0; l < newHeight; l++)
				{
					if (array[l * newWidth + k] == null)
					{
						array[l * newWidth + k] = new PrototypeDungeonRoomCellData(string.Empty, CellType.WALL);
					}
				}
			}
			this.m_cellData = array;
		}
	}

	// Token: 0x04004CF8 RID: 19704
	[HideInInspector]
	public int RoomId = -1;

	// Token: 0x04004CF9 RID: 19705
	[SerializeField]
	public string QAID;

	// Token: 0x04004CFA RID: 19706
	[SerializeField]
	public string GUID;

	// Token: 0x04004CFB RID: 19707
	[SerializeField]
	public bool PreventMirroring;

	// Token: 0x04004CFC RID: 19708
	[NonSerialized]
	public PrototypeDungeonRoom MirrorSource;

	// Token: 0x04004CFD RID: 19709
	public PrototypeDungeonRoom.RoomCategory category = PrototypeDungeonRoom.RoomCategory.NORMAL;

	// Token: 0x04004CFE RID: 19710
	public PrototypeDungeonRoom.RoomNormalSubCategory subCategoryNormal;

	// Token: 0x04004CFF RID: 19711
	public PrototypeDungeonRoom.RoomBossSubCategory subCategoryBoss;

	// Token: 0x04004D00 RID: 19712
	public PrototypeDungeonRoom.RoomSpecialSubCategory subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;

	// Token: 0x04004D01 RID: 19713
	public PrototypeDungeonRoom.RoomSecretSubCategory subCategorySecret;

	// Token: 0x04004D02 RID: 19714
	public PrototypeRoomExitData exitData;

	// Token: 0x04004D03 RID: 19715
	public List<PrototypeRoomPitEntry> pits;

	// Token: 0x04004D04 RID: 19716
	public List<PrototypePlacedObjectData> placedObjects;

	// Token: 0x04004D05 RID: 19717
	public List<Vector2> placedObjectPositions;

	// Token: 0x04004D06 RID: 19718
	public List<PrototypeRoomObjectLayer> additionalObjectLayers = new List<PrototypeRoomObjectLayer>();

	// Token: 0x04004D07 RID: 19719
	[NonSerialized]
	public List<PrototypeRoomObjectLayer> runtimeAdditionalObjectLayers;

	// Token: 0x04004D08 RID: 19720
	public List<PrototypeEventTriggerArea> eventTriggerAreas;

	// Token: 0x04004D09 RID: 19721
	public List<RoomEventDefinition> roomEvents;

	// Token: 0x04004D0A RID: 19722
	public List<SerializedPath> paths = new List<SerializedPath>();

	// Token: 0x04004D0B RID: 19723
	public GlobalDungeonData.ValidTilesets overriddenTilesets;

	// Token: 0x04004D0C RID: 19724
	public List<DungeonPrerequisite> prerequisites;

	// Token: 0x04004D0D RID: 19725
	public int RequiredCurseLevel = -1;

	// Token: 0x04004D0E RID: 19726
	public bool InvalidInCoop;

	// Token: 0x04004D0F RID: 19727
	public List<PrototypeDungeonRoom> excludedOtherRooms = new List<PrototypeDungeonRoom>();

	// Token: 0x04004D10 RID: 19728
	public List<PrototypeRectangularFeature> rectangularFeatures = new List<PrototypeRectangularFeature>();

	// Token: 0x04004D11 RID: 19729
	public bool usesProceduralLighting = true;

	// Token: 0x04004D12 RID: 19730
	public bool usesProceduralDecoration = true;

	// Token: 0x04004D13 RID: 19731
	public bool cullProceduralDecorationOnWeakPlatforms;

	// Token: 0x04004D14 RID: 19732
	public bool allowFloorDecoration = true;

	// Token: 0x04004D15 RID: 19733
	public bool allowWallDecoration = true;

	// Token: 0x04004D16 RID: 19734
	public bool preventAddedDecoLayering;

	// Token: 0x04004D17 RID: 19735
	public bool precludeAllTilemapDrawing;

	// Token: 0x04004D18 RID: 19736
	public bool drawPrecludedCeilingTiles;

	// Token: 0x04004D19 RID: 19737
	public bool preventFacewallAO;

	// Token: 0x04004D1A RID: 19738
	public bool preventBorders;

	// Token: 0x04004D1B RID: 19739
	public bool usesCustomAmbientLight;

	// Token: 0x04004D1C RID: 19740
	[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
	public Color customAmbientLight = Color.white;

	// Token: 0x04004D1D RID: 19741
	public bool ForceAllowDuplicates;

	// Token: 0x04004D1E RID: 19742
	public GameObject doorTopDecorable;

	// Token: 0x04004D1F RID: 19743
	public SharedInjectionData requiredInjectionData;

	// Token: 0x04004D20 RID: 19744
	public RuntimeInjectionFlags injectionFlags;

	// Token: 0x04004D21 RID: 19745
	public bool IsLostWoodsRoom;

	// Token: 0x04004D22 RID: 19746
	public bool UseCustomMusicState;

	// Token: 0x04004D23 RID: 19747
	public DungeonFloorMusicController.DungeonMusicState OverrideMusicState = DungeonFloorMusicController.DungeonMusicState.CALM;

	// Token: 0x04004D24 RID: 19748
	public bool UseCustomMusic;

	// Token: 0x04004D25 RID: 19749
	public string CustomMusicEvent;

	// Token: 0x04004D26 RID: 19750
	public bool UseCustomMusicSwitch;

	// Token: 0x04004D27 RID: 19751
	public string CustomMusicSwitch;

	// Token: 0x04004D28 RID: 19752
	public int overrideRoomVisualType = -1;

	// Token: 0x04004D29 RID: 19753
	public bool overrideRoomVisualTypeForSecretRooms;

	// Token: 0x04004D2A RID: 19754
	public IntVector2 rewardChestSpawnPosition = new IntVector2(-1, -1);

	// Token: 0x04004D2B RID: 19755
	public GameObject associatedMinimapIcon;

	// Token: 0x04004D2C RID: 19756
	[SerializeField]
	private int m_width = 5;

	// Token: 0x04004D2D RID: 19757
	[SerializeField]
	private int m_height = 5;

	// Token: 0x04004D2E RID: 19758
	[NonSerialized]
	private PrototypeDungeonRoomCellData[] m_cellData;

	// Token: 0x04004D2F RID: 19759
	[FormerlySerializedAs("m_cellData")]
	[SerializeField]
	private PrototypeDungeonRoomCellData[] m_OLDcellData;

	// Token: 0x04004D30 RID: 19760
	[SerializeField]
	private CellType[] m_serializedCellType;

	// Token: 0x04004D31 RID: 19761
	[SerializeField]
	private List<int> m_serializedCellDataIndices;

	// Token: 0x04004D32 RID: 19762
	[SerializeField]
	private List<PrototypeDungeonRoomCellData> m_serializedCellDataData;

	// Token: 0x04004D33 RID: 19763
	[SerializeField]
	[HideInInspector]
	private List<IntVector2> m_cachedRepresentationIncFacewalls;

	// Token: 0x02000F4A RID: 3914
	public enum RoomCategory
	{
		// Token: 0x04004D35 RID: 19765
		CONNECTOR,
		// Token: 0x04004D36 RID: 19766
		HUB,
		// Token: 0x04004D37 RID: 19767
		NORMAL,
		// Token: 0x04004D38 RID: 19768
		BOSS,
		// Token: 0x04004D39 RID: 19769
		REWARD,
		// Token: 0x04004D3A RID: 19770
		SPECIAL,
		// Token: 0x04004D3B RID: 19771
		SECRET,
		// Token: 0x04004D3C RID: 19772
		ENTRANCE,
		// Token: 0x04004D3D RID: 19773
		EXIT
	}

	// Token: 0x02000F4B RID: 3915
	public enum RoomNormalSubCategory
	{
		// Token: 0x04004D3F RID: 19775
		COMBAT,
		// Token: 0x04004D40 RID: 19776
		TRAP
	}

	// Token: 0x02000F4C RID: 3916
	public enum RoomBossSubCategory
	{
		// Token: 0x04004D42 RID: 19778
		FLOOR_BOSS,
		// Token: 0x04004D43 RID: 19779
		MINI_BOSS
	}

	// Token: 0x02000F4D RID: 3917
	public enum RoomSpecialSubCategory
	{
		// Token: 0x04004D45 RID: 19781
		UNSPECIFIED_SPECIAL,
		// Token: 0x04004D46 RID: 19782
		STANDARD_SHOP,
		// Token: 0x04004D47 RID: 19783
		WEIRD_SHOP,
		// Token: 0x04004D48 RID: 19784
		MAIN_STORY,
		// Token: 0x04004D49 RID: 19785
		NPC_STORY,
		// Token: 0x04004D4A RID: 19786
		CATACOMBS_BRIDGE_ROOM
	}

	// Token: 0x02000F4E RID: 3918
	public enum RoomSecretSubCategory
	{
		// Token: 0x04004D4C RID: 19788
		UNSPECIFIED_SECRET
	}
}
