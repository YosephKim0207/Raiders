using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200169B RID: 5787
public class RobotDave
{
	// Token: 0x060086F6 RID: 34550 RVA: 0x0037F468 File Offset: 0x0037D668
	public static RobotRoomFeature GetFeatureFromType(RobotDave.RobotFeatureType type)
	{
		switch (type)
		{
		case RobotDave.RobotFeatureType.MINES_SQUARE_CART:
			return new RobotRoomMineCartSquareFeature();
		case RobotDave.RobotFeatureType.MINES_DOUBLE_CART:
			return new RobotRoomMineCartSquareDoubleFeature();
		case RobotDave.RobotFeatureType.MINES_TURRET_CART:
			return new RobotRoomMineCartSquareTurretFeature();
		default:
			if (type == RobotDave.RobotFeatureType.FLAT_EXPANSE)
			{
				return new FlatExpanseFeature();
			}
			if (type == RobotDave.RobotFeatureType.COLUMN_SAWBLADE)
			{
				return new RobotRoomColumnFeature();
			}
			if (type == RobotDave.RobotFeatureType.PIT_BORDER)
			{
				return new RobotRoomSurroundingPitFeature();
			}
			if (type == RobotDave.RobotFeatureType.TRAP_PLUS)
			{
				return new RobotRoomTrapPlusFeature();
			}
			if (type == RobotDave.RobotFeatureType.TRAP_SQUARE)
			{
				return new RobotRoomTrapSquareFeature();
			}
			if (type == RobotDave.RobotFeatureType.CORNER_COLUMNS)
			{
				return new RobotRoomCornerColumnsFeature();
			}
			if (type == RobotDave.RobotFeatureType.PIT_INNER)
			{
				return new RobotRoomInnerPitFeature();
			}
			if (type == RobotDave.RobotFeatureType.TABLES_EDGE)
			{
				return new RobotRoomTablesFeature();
			}
			if (type == RobotDave.RobotFeatureType.ROLLING_LOG_VERTICAL)
			{
				return new RobotRoomRollingLogsVerticalFeature();
			}
			if (type == RobotDave.RobotFeatureType.ROLLING_LOG_HORIZONTAL)
			{
				return new RobotRoomRollingLogsHorizontalFeature();
			}
			if (type == RobotDave.RobotFeatureType.CASTLE_CHANDELIER)
			{
				return new RobotRoomChandelierFeature();
			}
			if (type == RobotDave.RobotFeatureType.MINES_CAVE_IN)
			{
				return new RobotRoomCaveInFeature();
			}
			if (type == RobotDave.RobotFeatureType.CONVEYOR_HORIZONTAL)
			{
				return new RobotRoomConveyorHorizontalFeature();
			}
			if (type != RobotDave.RobotFeatureType.CONVEYOR_VERTICAL)
			{
				return new FlatExpanseFeature();
			}
			return new RobotRoomConveyorVerticalFeature();
		}
	}

	// Token: 0x060086F7 RID: 34551 RVA: 0x0037F56C File Offset: 0x0037D76C
	public static DungeonPlaceableBehaviour GetPitTrap()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[1].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086F8 RID: 34552 RVA: 0x0037F5BC File Offset: 0x0037D7BC
	public static DungeonPlaceableBehaviour GetSpikesTrap()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[2].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086F9 RID: 34553 RVA: 0x0037F60C File Offset: 0x0037D80C
	public static DungeonPlaceableBehaviour GetFloorFlameTrap()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[3].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FA RID: 34554 RVA: 0x0037F65C File Offset: 0x0037D85C
	public static DungeonPlaceableBehaviour GetSawbladePrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[0].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FB RID: 34555 RVA: 0x0037F6AC File Offset: 0x0037D8AC
	public static DungeonPlaceableBehaviour GetMineCartTurretPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[11].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FC RID: 34556 RVA: 0x0037F700 File Offset: 0x0037D900
	public static DungeonPlaceableBehaviour GetMineCartPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[6].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FD RID: 34557 RVA: 0x0037F750 File Offset: 0x0037D950
	public static DungeonPlaceableBehaviour GetCaveInPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[7].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FE RID: 34558 RVA: 0x0037F7A0 File Offset: 0x0037D9A0
	public static DungeonPlaceableBehaviour GetChandelierPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[8].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x060086FF RID: 34559 RVA: 0x0037F7F0 File Offset: 0x0037D9F0
	public static DungeonPlaceableBehaviour GetHorizontalConveyorPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[9].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x06008700 RID: 34560 RVA: 0x0037F844 File Offset: 0x0037DA44
	public static DungeonPlaceableBehaviour GetVerticalConveyorPrefab()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		return RobotDave.m_trapData.variantTiers[10].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x06008701 RID: 34561 RVA: 0x0037F898 File Offset: 0x0037DA98
	public static DungeonPlaceableBehaviour GetRollingLogVertical()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		bool flag = false;
		ResizableCollider component = RobotDave.m_trapData.variantTiers[(!flag) ? 4 : 12].nonDatabasePlaceable.GetComponent<ResizableCollider>();
		if (component)
		{
			return component;
		}
		return RobotDave.m_trapData.variantTiers[(!flag) ? 4 : 12].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x06008702 RID: 34562 RVA: 0x0037F92C File Offset: 0x0037DB2C
	public static DungeonPlaceableBehaviour GetRollingLogHorizontal()
	{
		if (RobotDave.m_trapData == null)
		{
			RobotDave.m_trapData = (DungeonPlaceable)BraveResources.Load("RobotDaveTraps", ".asset");
		}
		bool flag = false;
		ResizableCollider component = RobotDave.m_trapData.variantTiers[(!flag) ? 5 : 13].nonDatabasePlaceable.GetComponent<ResizableCollider>();
		if (component)
		{
			return component;
		}
		return RobotDave.m_trapData.variantTiers[(!flag) ? 5 : 13].nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
	}

	// Token: 0x06008703 RID: 34563 RVA: 0x0037F9C0 File Offset: 0x0037DBC0
	public static DungeonPlaceable GetHorizontalTable()
	{
		if (RobotDave.m_horizontalTable == null)
		{
			RobotDave.m_horizontalTable = (DungeonPlaceable)BraveResources.Load("RobotTableHorizontal", ".asset");
		}
		return RobotDave.m_horizontalTable;
	}

	// Token: 0x06008704 RID: 34564 RVA: 0x0037F9F0 File Offset: 0x0037DBF0
	public static DungeonPlaceable GetVerticalTable()
	{
		if (RobotDave.m_verticalTable == null)
		{
			RobotDave.m_verticalTable = (DungeonPlaceable)BraveResources.Load("RobotTableVertical", ".asset");
		}
		return RobotDave.m_verticalTable;
	}

	// Token: 0x06008705 RID: 34565 RVA: 0x0037FA20 File Offset: 0x0037DC20
	protected static void ResetForNewProcess()
	{
		RobotDave.m_trapData = null;
		RobotDave.m_horizontalTable = null;
		RobotDave.m_verticalTable = null;
		RobotRoomSurroundingPitFeature.BeenUsed = false;
	}

	// Token: 0x06008706 RID: 34566 RVA: 0x0037FA3C File Offset: 0x0037DC3C
	public static void ApplyFeatureToDwarfRegion(PrototypeDungeonRoom extantRoom, IntVector2 basePosition, IntVector2 dimensions, RobotDaveIdea idea, RobotDave.RobotFeatureType specificFeature, int targetObjectLayer)
	{
		RobotDave.ResetForNewProcess();
		RobotDave.ClearDataForRegion(extantRoom, idea, basePosition, dimensions, targetObjectLayer);
		RobotRoomFeature robotRoomFeature = ((specificFeature == RobotDave.RobotFeatureType.NONE) ? RobotDave.SelectFeatureForZone(idea, basePosition, dimensions, false, 1) : RobotDave.GetFeatureFromType(specificFeature));
		robotRoomFeature.LocalBasePosition = basePosition;
		robotRoomFeature.LocalDimensions = dimensions;
		RobotRoomFeature robotRoomFeature2 = null;
		if (specificFeature == RobotDave.RobotFeatureType.NONE && robotRoomFeature.CanContainOtherFeature())
		{
			IntVector2 intVector = robotRoomFeature.LocalBasePosition + new IntVector2(robotRoomFeature.RequiredInsetForOtherFeature(), robotRoomFeature.RequiredInsetForOtherFeature());
			IntVector2 intVector2 = robotRoomFeature.LocalDimensions - new IntVector2(robotRoomFeature.RequiredInsetForOtherFeature() * 2, robotRoomFeature.RequiredInsetForOtherFeature() * 2);
			robotRoomFeature2 = RobotDave.SelectFeatureForZone(idea, intVector, intVector2, true, 1);
			robotRoomFeature2.LocalBasePosition = intVector;
			robotRoomFeature2.LocalDimensions = intVector2;
		}
		robotRoomFeature.Develop(extantRoom, idea, targetObjectLayer);
		if (robotRoomFeature2 != null)
		{
			robotRoomFeature2.Develop(extantRoom, idea, targetObjectLayer);
		}
	}

	// Token: 0x06008707 RID: 34567 RVA: 0x0037FB0C File Offset: 0x0037DD0C
	public static void DwarfProcessIdea(PrototypeDungeonRoom extantRoom, RobotDaveIdea idea, IntVector2 desiredDimensions)
	{
		RobotDave.ResetForNewProcess();
		RobotDave.ProcessBasicRoomData(extantRoom, idea, desiredDimensions);
		List<RobotRoomFeature> list = RobotDave.RequestRidiculousNumberOfFeatures(extantRoom, idea, false);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Develop(extantRoom, idea, -1);
		}
		RobotDave.PlaceEnemiesInRoom(extantRoom, idea);
	}

	// Token: 0x06008708 RID: 34568 RVA: 0x0037FB5C File Offset: 0x0037DD5C
	public static PrototypeDungeonRoom RuntimeProcessIdea(RobotDaveIdea idea, IntVector2 desiredDimensions)
	{
		RobotDave.ResetForNewProcess();
		PrototypeDungeonRoom prototypeDungeonRoom = ScriptableObject.CreateInstance<PrototypeDungeonRoom>();
		RobotDave.ProcessBasicRoomData(prototypeDungeonRoom, idea, desiredDimensions);
		List<RobotRoomFeature> list = RobotDave.RequestRidiculousNumberOfFeatures(prototypeDungeonRoom, idea, true);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Develop(prototypeDungeonRoom, idea, -1);
		}
		RobotDave.PlaceEnemiesInRoom(prototypeDungeonRoom, idea);
		prototypeDungeonRoom.roomEvents.Add(new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM));
		prototypeDungeonRoom.roomEvents.Add(new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM));
		return prototypeDungeonRoom;
	}

	// Token: 0x06008709 RID: 34569 RVA: 0x0037FBD8 File Offset: 0x0037DDD8
	protected static PrototypePlacedObjectData PlacePlaceable(DungeonPlaceable item, PrototypeDungeonRoom room, IntVector2 position)
	{
		if (item == null || room == null)
		{
			return null;
		}
		if (room.CheckRegionOccupiedExcludeWallsAndPits(position.x, position.y, item.GetWidth(), item.GetHeight(), false))
		{
			return null;
		}
		Vector2 vector = position.ToVector2();
		PrototypePlacedObjectData prototypePlacedObjectData = new PrototypePlacedObjectData();
		prototypePlacedObjectData.fieldData = new List<PrototypePlacedObjectFieldData>();
		prototypePlacedObjectData.instancePrerequisites = new DungeonPrerequisite[0];
		prototypePlacedObjectData.placeableContents = item;
		prototypePlacedObjectData.contentsBasePosition = vector;
		int count = room.placedObjects.Count;
		room.placedObjects.Add(prototypePlacedObjectData);
		room.placedObjectPositions.Add(vector);
		for (int i = 0; i < item.GetWidth(); i++)
		{
			for (int j = 0; j < item.GetHeight(); j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(position.x + i, position.y + j);
				prototypeDungeonRoomCellData.placedObjectRUBELIndex = count;
			}
		}
		return prototypePlacedObjectData;
	}

	// Token: 0x0600870A RID: 34570 RVA: 0x0037FCD4 File Offset: 0x0037DED4
	private static void PlaceEnemiesInRoom(PrototypeDungeonRoom room, RobotDaveIdea idea)
	{
		if (idea.ValidEasyEnemyPlaceables == null || idea.ValidEasyEnemyPlaceables.Length == 0)
		{
			return;
		}
		int num = room.Width * room.Height;
		int num2 = Mathf.CeilToInt((float)num / 45f);
		num2 = Mathf.Clamp(num2, 1, 6);
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
		{
			num2 = Mathf.Min(num2, 3);
			if (UnityEngine.Random.value < 0.1f)
			{
				return;
			}
		}
		int num3 = 0;
		if (num2 > 3 && idea.ValidHardEnemyPlaceables != null && idea.ValidHardEnemyPlaceables.Length > 0 && UnityEngine.Random.value < 0.5f)
		{
			num3++;
			num2 -= 2;
		}
		if (num2 > 3)
		{
			num2 = UnityEngine.Random.Range(3, num2 + 1);
		}
		int num4 = Mathf.FloorToInt((float)room.Width / 5f);
		int num5 = Mathf.FloorToInt((float)room.Height / 5f);
		int num6 = Mathf.CeilToInt((float)num4 / 2f);
		int num7 = Mathf.CeilToInt((float)num5 / 2f);
		List<IntVector2> list = new List<IntVector2>();
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					int num8 = UnityEngine.Random.Range(-num6, num6 + 1);
					int num9 = UnityEngine.Random.Range(-num7, num7 + 1);
					list.Add(new IntVector2(num4 * (i + 1) + num8, num5 * (j + 1) + num9));
				}
			}
		}
		list = list.GenerationShuffle<IntVector2>();
		for (int l = 0; l < num2; l++)
		{
			DungeonPlaceable dungeonPlaceable = idea.ValidEasyEnemyPlaceables[UnityEngine.Random.Range(0, idea.ValidEasyEnemyPlaceables.Length)];
			for (int m = 0; m < list.Count; m++)
			{
				PrototypePlacedObjectData prototypePlacedObjectData = RobotDave.PlacePlaceable(dungeonPlaceable, room, list[m]);
				if (prototypePlacedObjectData != null)
				{
					break;
				}
			}
		}
		for (int n = 0; n < num3; n++)
		{
			DungeonPlaceable dungeonPlaceable2 = idea.ValidHardEnemyPlaceables[UnityEngine.Random.Range(0, idea.ValidHardEnemyPlaceables.Length)];
			for (int num10 = 0; num10 < list.Count; num10++)
			{
				PrototypePlacedObjectData prototypePlacedObjectData2 = RobotDave.PlacePlaceable(dungeonPlaceable2, room, list[num10]);
				if (prototypePlacedObjectData2 != null)
				{
					break;
				}
			}
		}
	}

	// Token: 0x0600870B RID: 34571 RVA: 0x0037FF40 File Offset: 0x0037E140
	private static RobotRoomFeature SelectFeatureForZone(RobotDaveIdea idea, IntVector2 basePos, IntVector2 dim, bool isInternal, int numFeatures)
	{
		List<RobotRoomFeature> list = new List<RobotRoomFeature>();
		FlatExpanseFeature flatExpanseFeature = new FlatExpanseFeature();
		if (flatExpanseFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(flatExpanseFeature);
		}
		RobotRoomColumnFeature robotRoomColumnFeature = new RobotRoomColumnFeature();
		if (robotRoomColumnFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomColumnFeature);
		}
		RobotRoomSurroundingPitFeature robotRoomSurroundingPitFeature = new RobotRoomSurroundingPitFeature();
		if (robotRoomSurroundingPitFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomSurroundingPitFeature);
		}
		RobotRoomTrapPlusFeature robotRoomTrapPlusFeature = new RobotRoomTrapPlusFeature();
		if (robotRoomTrapPlusFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomTrapPlusFeature);
		}
		RobotRoomTrapSquareFeature robotRoomTrapSquareFeature = new RobotRoomTrapSquareFeature();
		if (robotRoomTrapSquareFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomTrapSquareFeature);
		}
		RobotRoomCornerColumnsFeature robotRoomCornerColumnsFeature = new RobotRoomCornerColumnsFeature();
		if (robotRoomCornerColumnsFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomCornerColumnsFeature);
		}
		RobotRoomInnerPitFeature robotRoomInnerPitFeature = new RobotRoomInnerPitFeature();
		if (robotRoomInnerPitFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomInnerPitFeature);
		}
		RobotRoomTablesFeature robotRoomTablesFeature = new RobotRoomTablesFeature();
		if (robotRoomTablesFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomTablesFeature);
		}
		RobotRoomRollingLogsVerticalFeature robotRoomRollingLogsVerticalFeature = new RobotRoomRollingLogsVerticalFeature();
		if (robotRoomRollingLogsVerticalFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomRollingLogsVerticalFeature);
		}
		RobotRoomRollingLogsHorizontalFeature robotRoomRollingLogsHorizontalFeature = new RobotRoomRollingLogsHorizontalFeature();
		if (robotRoomRollingLogsHorizontalFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomRollingLogsHorizontalFeature);
		}
		RobotRoomMineCartSquareFeature robotRoomMineCartSquareFeature = new RobotRoomMineCartSquareFeature();
		if (robotRoomMineCartSquareFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomMineCartSquareFeature);
		}
		RobotRoomCaveInFeature robotRoomCaveInFeature = new RobotRoomCaveInFeature();
		if (robotRoomCaveInFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomCaveInFeature);
		}
		RobotRoomMineCartSquareDoubleFeature robotRoomMineCartSquareDoubleFeature = new RobotRoomMineCartSquareDoubleFeature();
		if (robotRoomMineCartSquareDoubleFeature.AcceptableInIdea(idea, dim, isInternal, numFeatures))
		{
			list.Add(robotRoomMineCartSquareDoubleFeature);
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600870C RID: 34572 RVA: 0x0038010C File Offset: 0x0037E30C
	private static List<RobotRoomFeature> RequestRidiculousNumberOfFeatures(PrototypeDungeonRoom room, RobotDaveIdea idea, bool isRuntime)
	{
		List<RobotRoomFeature> list = new List<RobotRoomFeature>();
		int num = room.Width * room.Height;
		int num2;
		if (num <= 49)
		{
			num2 = 1;
		}
		else
		{
			float value = UnityEngine.Random.value;
			if (value < 0.5f)
			{
				num2 = 1;
			}
			else
			{
				float num3 = (float)room.Width / ((float)room.Height * 1f);
				if (num < 100 || num3 > 1.75f || num3 < 0.6f)
				{
					num2 = 2;
				}
				else if (value < 0.75f)
				{
					num2 = 2;
				}
				else
				{
					num2 = 2;
				}
			}
		}
		List<IntVector2> list2 = new List<IntVector2>();
		List<IntVector2> list3 = new List<IntVector2>();
		if (num2 == 1)
		{
			list2.Add(IntVector2.Zero);
			list3.Add(new IntVector2(room.Width, room.Height));
		}
		else if (num2 == 2)
		{
			float num4 = (float)room.Width / 2f;
			float num5 = (float)room.Height / 2f;
			if (room.Width > room.Height)
			{
				list2.Add(IntVector2.Zero);
				list3.Add(new IntVector2(Mathf.FloorToInt(num4), room.Height));
				list2.Add(new IntVector2(Mathf.FloorToInt(num4), 0));
				list3.Add(new IntVector2(Mathf.CeilToInt(num4), room.Height));
			}
			else
			{
				list2.Add(IntVector2.Zero);
				list3.Add(new IntVector2(room.Width, Mathf.FloorToInt(num5)));
				list2.Add(new IntVector2(0, Mathf.FloorToInt(num5)));
				list3.Add(new IntVector2(room.Width, Mathf.CeilToInt(num5)));
			}
		}
		else if (num2 == 4)
		{
			float num6 = (float)room.Width / 2f;
			float num7 = (float)room.Height / 2f;
			bool flag = UnityEngine.Random.value < 0.5f;
			int num8 = ((!flag) ? Mathf.CeilToInt(num6) : Mathf.FloorToInt(num6));
			int num9 = (flag ? Mathf.CeilToInt(num6) : Mathf.FloorToInt(num6));
			int num10 = ((!flag) ? Mathf.CeilToInt(num7) : Mathf.FloorToInt(num7));
			int num11 = (flag ? Mathf.CeilToInt(num7) : Mathf.FloorToInt(num7));
			list2.Add(IntVector2.Zero);
			list3.Add(new IntVector2(num8, num10));
			list2.Add(new IntVector2(num8, 0));
			list3.Add(new IntVector2(num9, num10));
			list2.Add(new IntVector2(0, num10));
			list3.Add(new IntVector2(num8, num11));
			list2.Add(new IntVector2(num8, num10));
			list3.Add(new IntVector2(num9, num11));
		}
		for (int i = 0; i < num2; i++)
		{
			IntVector2 intVector = list2[i];
			IntVector2 intVector2 = list3[i];
			RobotRoomFeature robotRoomFeature = RobotDave.SelectFeatureForZone(idea, intVector, intVector2, false, num2);
			if (robotRoomFeature != null)
			{
				robotRoomFeature.LocalBasePosition = intVector;
				robotRoomFeature.LocalDimensions = intVector2;
				robotRoomFeature.Use();
				list.Add(robotRoomFeature);
			}
		}
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			if (list[j].CanContainOtherFeature())
			{
				IntVector2 intVector3 = list[j].LocalBasePosition + new IntVector2(list[j].RequiredInsetForOtherFeature(), list[j].RequiredInsetForOtherFeature());
				IntVector2 intVector4 = list[j].LocalDimensions - new IntVector2(list[j].RequiredInsetForOtherFeature() * 2, list[j].RequiredInsetForOtherFeature() * 2);
				RobotRoomFeature robotRoomFeature2 = RobotDave.SelectFeatureForZone(idea, intVector3, intVector4, true, num2);
				if (robotRoomFeature2 != null)
				{
					robotRoomFeature2.LocalBasePosition = intVector3;
					robotRoomFeature2.LocalDimensions = intVector4;
					robotRoomFeature2.Use();
					list.Add(robotRoomFeature2);
				}
			}
		}
		return list;
	}

	// Token: 0x0600870D RID: 34573 RVA: 0x0038051C File Offset: 0x0037E71C
	private static void ClearDataForRegion(PrototypeDungeonRoom room, RobotDaveIdea idea, IntVector2 basePosition, IntVector2 desiredDimensions, int targetObjectLayer)
	{
		for (int i = basePosition.x; i < basePosition.x + desiredDimensions.x; i++)
		{
			for (int j = basePosition.y; j < basePosition.y + desiredDimensions.y; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(i, j);
				prototypeDungeonRoomCellData.state = CellType.FLOOR;
				if (targetObjectLayer == -1)
				{
					prototypeDungeonRoomCellData.placedObjectRUBELIndex = -1;
				}
				else if (prototypeDungeonRoomCellData.additionalPlacedObjectIndices.Count > targetObjectLayer)
				{
					prototypeDungeonRoomCellData.additionalPlacedObjectIndices[targetObjectLayer] = -1;
				}
				prototypeDungeonRoomCellData.doesDamage = false;
				prototypeDungeonRoomCellData.damageDefinition = default(CellDamageDefinition);
				prototypeDungeonRoomCellData.appearance = new PrototypeDungeonRoomCellAppearance();
			}
		}
		if (targetObjectLayer == -1)
		{
			for (int k = 0; k < room.placedObjects.Count; k++)
			{
				Vector2 vector = room.placedObjectPositions[k];
				if (vector.x >= (float)basePosition.x && vector.x < (float)(basePosition.x + desiredDimensions.x) && vector.y >= (float)basePosition.y && vector.y < (float)(basePosition.y + desiredDimensions.y))
				{
					if (room.placedObjects[k].assignedPathIDx >= 0)
					{
						room.RemovePathAt(room.placedObjects[k].assignedPathIDx);
					}
					room.placedObjectPositions.RemoveAt(k);
					room.placedObjects.RemoveAt(k);
					k--;
				}
			}
		}
		else
		{
			PrototypeRoomObjectLayer prototypeRoomObjectLayer = room.additionalObjectLayers[targetObjectLayer];
			for (int l = 0; l < prototypeRoomObjectLayer.placedObjects.Count; l++)
			{
				Vector2 vector2 = prototypeRoomObjectLayer.placedObjectBasePositions[l];
				if (vector2.x >= (float)basePosition.x && vector2.x < (float)(basePosition.x + desiredDimensions.x) && vector2.y >= (float)basePosition.y && vector2.y < (float)(basePosition.y + desiredDimensions.y))
				{
					if (prototypeRoomObjectLayer.placedObjects[l].assignedPathIDx >= 0)
					{
						room.RemovePathAt(prototypeRoomObjectLayer.placedObjects[l].assignedPathIDx);
					}
					prototypeRoomObjectLayer.placedObjectBasePositions.RemoveAt(l);
					prototypeRoomObjectLayer.placedObjects.RemoveAt(l);
					l--;
				}
			}
		}
	}

	// Token: 0x0600870E RID: 34574 RVA: 0x003807BC File Offset: 0x0037E9BC
	private static void ProcessBasicRoomData(PrototypeDungeonRoom room, RobotDaveIdea idea, IntVector2 desiredDimensions)
	{
		room.category = PrototypeDungeonRoom.RoomCategory.NORMAL;
		room.Width = desiredDimensions.x;
		room.Height = desiredDimensions.y;
		for (int i = 0; i < room.Width; i++)
		{
			for (int j = 0; j < room.Height; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(i, j);
				prototypeDungeonRoomCellData.state = CellType.FLOOR;
				prototypeDungeonRoomCellData.placedObjectRUBELIndex = -1;
				prototypeDungeonRoomCellData.doesDamage = false;
				prototypeDungeonRoomCellData.damageDefinition = default(CellDamageDefinition);
				prototypeDungeonRoomCellData.appearance = new PrototypeDungeonRoomCellAppearance();
			}
		}
		room.exitData = new PrototypeRoomExitData();
		room.pits = new List<PrototypeRoomPitEntry>();
		room.placedObjects = new List<PrototypePlacedObjectData>();
		room.placedObjectPositions = new List<Vector2>();
		room.additionalObjectLayers = new List<PrototypeRoomObjectLayer>();
		room.eventTriggerAreas = new List<PrototypeEventTriggerArea>();
		room.roomEvents = new List<RoomEventDefinition>();
		room.paths = new List<SerializedPath>();
		room.prerequisites = new List<DungeonPrerequisite>();
		room.excludedOtherRooms = new List<PrototypeDungeonRoom>();
		room.rectangularFeatures = new List<PrototypeRectangularFeature>();
	}

	// Token: 0x04008C29 RID: 35881
	private static DungeonPlaceable m_trapData;

	// Token: 0x04008C2A RID: 35882
	private static DungeonPlaceable m_horizontalTable;

	// Token: 0x04008C2B RID: 35883
	private static DungeonPlaceable m_verticalTable;

	// Token: 0x0200169C RID: 5788
	public enum RobotFeatureType
	{
		// Token: 0x04008C2D RID: 35885
		NONE,
		// Token: 0x04008C2E RID: 35886
		FLAT_EXPANSE = 5,
		// Token: 0x04008C2F RID: 35887
		COLUMN_SAWBLADE = 10,
		// Token: 0x04008C30 RID: 35888
		PIT_BORDER = 15,
		// Token: 0x04008C31 RID: 35889
		TRAP_PLUS = 20,
		// Token: 0x04008C32 RID: 35890
		TRAP_SQUARE = 25,
		// Token: 0x04008C33 RID: 35891
		CORNER_COLUMNS = 30,
		// Token: 0x04008C34 RID: 35892
		PIT_INNER = 35,
		// Token: 0x04008C35 RID: 35893
		TABLES_EDGE = 40,
		// Token: 0x04008C36 RID: 35894
		ROLLING_LOG_VERTICAL = 45,
		// Token: 0x04008C37 RID: 35895
		ROLLING_LOG_HORIZONTAL = 50,
		// Token: 0x04008C38 RID: 35896
		CASTLE_CHANDELIER = 60,
		// Token: 0x04008C39 RID: 35897
		MINES_CAVE_IN = 70,
		// Token: 0x04008C3A RID: 35898
		MINES_SQUARE_CART = 75,
		// Token: 0x04008C3B RID: 35899
		MINES_DOUBLE_CART,
		// Token: 0x04008C3C RID: 35900
		MINES_TURRET_CART,
		// Token: 0x04008C3D RID: 35901
		CONVEYOR_HORIZONTAL = 110,
		// Token: 0x04008C3E RID: 35902
		CONVEYOR_VERTICAL = 115
	}
}
