using System;
using System.Collections.Generic;
using Dungeonator;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02001200 RID: 4608
public class SecretRoomBuilder
{
	// Token: 0x060066F6 RID: 26358 RVA: 0x00281CEC File Offset: 0x0027FEEC
	private static HashSet<IntVector2> GetRoomCeilingCells(RoomHandler room)
	{
		List<IntVector2> cellRepresentationIncFacewalls = room.area.prototypeRoom.GetCellRepresentationIncFacewalls();
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		List<IntVector2> list = new List<IntVector2>(IntVector2.CardinalsAndOrdinals);
		foreach (IntVector2 intVector in cellRepresentationIncFacewalls)
		{
			hashSet.Add(room.area.basePosition + intVector);
			foreach (IntVector2 intVector2 in list)
			{
				hashSet.Add(room.area.basePosition + intVector + intVector2);
			}
		}
		list.Add(IntVector2.Up * 2);
		list.Add(IntVector2.Up * 3);
		list.Add(IntVector2.Up * 2 + IntVector2.Right);
		list.Add(IntVector2.Up * 3 + IntVector2.Right);
		list.Add(IntVector2.Up * 2 + IntVector2.Left);
		list.Add(IntVector2.Up * 3 + IntVector2.Left);
		foreach (PrototypeRoomExit prototypeRoomExit in room.area.instanceUsedExits)
		{
			RuntimeExitDefinition runtimeExitDefinition = room.exitDefinitionsByExit[room.area.exitToLocalDataMap[prototypeRoomExit]];
			if (!room.area.exitToLocalDataMap[prototypeRoomExit].oneWayDoor)
			{
				DungeonData.Direction direction = ((runtimeExitDefinition.upstreamRoom != room) ? runtimeExitDefinition.downstreamExit.referencedExit.exitDirection : runtimeExitDefinition.upstreamExit.referencedExit.exitDirection);
				IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(direction);
				HashSet<IntVector2> cellsForRoom = runtimeExitDefinition.GetCellsForRoom(room);
				bool flag = !Dungeon.IsGenerating && runtimeExitDefinition.upstreamRoom.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.SECRET && runtimeExitDefinition.downstreamRoom.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.SECRET;
				if (flag)
				{
					int num = int.MaxValue;
					foreach (IntVector2 intVector3 in cellsForRoom)
					{
						num = Mathf.Min(num, intVector3.y);
					}
					foreach (IntVector2 intVector4 in runtimeExitDefinition.GetCellsForOtherRoom(room))
					{
						if (num - intVector4.y <= 4)
						{
							cellsForRoom.Add(intVector4);
						}
					}
				}
				foreach (IntVector2 intVector5 in cellsForRoom)
				{
					hashSet.Add(intVector5);
					foreach (IntVector2 intVector6 in list)
					{
						if ((intVector6.x != 0 && intVector6.x == intVector2FromDirection.x) || (intVector6.y != 0 && intVector6.y == intVector2FromDirection.y))
						{
							if (flag)
							{
								if (room == runtimeExitDefinition.upstreamRoom)
								{
									BraveUtility.DrawDebugSquare(intVector5.ToVector2() + intVector6.ToVector2(), Color.yellow, 1000f);
								}
								else if (room == runtimeExitDefinition.downstreamRoom)
								{
									BraveUtility.DrawDebugSquare(intVector5.ToVector2() + intVector6.ToVector2() + new Vector2(0.1f, 0.1f), intVector5.ToVector2() + intVector6.ToVector2() + new Vector2(0.9f, 0.9f), Color.cyan, 1000f);
								}
							}
						}
						else
						{
							hashSet.Add(intVector5 + intVector6);
						}
					}
				}
				if (direction != DungeonData.Direction.SOUTH)
				{
					RoomHandler roomHandler = ((runtimeExitDefinition.upstreamRoom != room) ? runtimeExitDefinition.upstreamRoom : runtimeExitDefinition.downstreamRoom);
					foreach (IntVector2 intVector7 in runtimeExitDefinition.GetCellsForRoom(roomHandler))
					{
						hashSet.Add(intVector7);
						foreach (IntVector2 intVector8 in list)
						{
							if ((intVector8.x == 0 || intVector8.x != intVector2FromDirection.x) && (intVector8.y == 0 || Mathf.Sign((float)intVector8.y) != (float)intVector2FromDirection.y))
							{
								hashSet.Add(intVector7 + intVector8);
							}
						}
					}
				}
			}
		}
		return hashSet;
	}

	// Token: 0x060066F7 RID: 26359 RVA: 0x0028234C File Offset: 0x0028054C
	private static bool IsFaceWallHigher(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return !cells.Contains(new IntVector2(x, y)) && ((data.cellData[x][y].type == CellType.WALL || data.cellData[x][y].isSecretRoomCell) && data.cellData[x][y - 2].type != CellType.WALL && !data.cellData[x][y - 2].isSecretRoomCell);
	}

	// Token: 0x060066F8 RID: 26360 RVA: 0x002823C8 File Offset: 0x002805C8
	private static bool IsTopWall(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return data.cellData[x][y].type != CellType.WALL && (data.cellData[x][y - 1].type == CellType.WALL || cells.Contains(new IntVector2(x, y - 1))) && !cells.Contains(new IntVector2(x, y + 1));
	}

	// Token: 0x060066F9 RID: 26361 RVA: 0x0028242C File Offset: 0x0028062C
	private static bool IsWall(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return cells.Contains(new IntVector2(x, y)) || data[x, y].type == CellType.WALL;
	}

	// Token: 0x060066FA RID: 26362 RVA: 0x00282458 File Offset: 0x00280658
	private static bool IsTopWallOrSecret(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return data[x, y].type != CellType.WALL && !data[x, y].isSecretRoomCell && SecretRoomBuilder.IsWallOrSecret(x, y - 1, data, cells);
	}

	// Token: 0x060066FB RID: 26363 RVA: 0x00282490 File Offset: 0x00280690
	private static bool IsWallOrSecret(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return data[x, y].type == CellType.WALL || data[x, y].isSecretRoomCell || cells.Contains(new IntVector2(x, y));
	}

	// Token: 0x060066FC RID: 26364 RVA: 0x002824C8 File Offset: 0x002806C8
	private static bool IsFaceWallHigherOrSecret(int x, int y, DungeonData data, HashSet<IntVector2> cells)
	{
		return SecretRoomBuilder.IsFaceWallHigher(x, y, data, cells);
	}

	// Token: 0x060066FD RID: 26365 RVA: 0x002824D4 File Offset: 0x002806D4
	public static int GetIndexFromTupleArray(CellData current, List<Tuple<int, TilesetIndexMetadata>> list, int roomTypeIndex)
	{
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			Tuple<int, TilesetIndexMetadata> tuple = list[i];
			if (!tuple.Second.usesAnimSequence)
			{
				if (tuple.Second.dungeonRoomSubType == roomTypeIndex || tuple.Second.secondRoomSubType == roomTypeIndex || tuple.Second.thirdRoomSubType == roomTypeIndex)
				{
					num += tuple.Second.weight;
				}
			}
		}
		float num2 = current.UniqueHash * num;
		for (int j = 0; j < list.Count; j++)
		{
			Tuple<int, TilesetIndexMetadata> tuple2 = list[j];
			if (!tuple2.Second.usesAnimSequence)
			{
				if (tuple2.Second.dungeonRoomSubType == roomTypeIndex || tuple2.Second.secondRoomSubType == roomTypeIndex || tuple2.Second.thirdRoomSubType == roomTypeIndex)
				{
					num2 -= tuple2.Second.weight;
					if (num2 <= 0f)
					{
						return tuple2.First;
					}
				}
			}
		}
		return list[0].First;
	}

	// Token: 0x060066FE RID: 26366 RVA: 0x00282610 File Offset: 0x00280810
	private static TileIndexGrid GetBorderGridForCellPosition(IntVector2 position, DungeonData data)
	{
		TileIndexGrid tileIndexGrid = GameManager.Instance.Dungeon.roomMaterialDefinitions[data.cellData[position.x][position.y].cellVisualData.roomVisualTypeIndex].roomCeilingBorderGrid;
		if (tileIndexGrid == null)
		{
			tileIndexGrid = GameManager.Instance.Dungeon.roomMaterialDefinitions[0].roomCeilingBorderGrid;
		}
		return tileIndexGrid;
	}

	// Token: 0x060066FF RID: 26367 RVA: 0x00282678 File Offset: 0x00280878
	private static void AddCeilingTileAtPosition(IntVector2 position, TileIndexGrid indexGrid, List<Vector3> verts, List<int> tris, List<Vector2> uvs, List<Color> colors, out Material ceilingMaterial, tk2dSpriteCollectionData spriteData)
	{
		int indexByWeight = indexGrid.centerIndices.GetIndexByWeight();
		int tileFromRawTile = BuilderUtil.GetTileFromRawTile(indexByWeight);
		tk2dSpriteDefinition tk2dSpriteDefinition = spriteData.spriteDefinitions[tileFromRawTile];
		ceilingMaterial = tk2dSpriteDefinition.material;
		int count = verts.Count;
		Vector3 vector = position.ToVector3((float)position.y - 2.4f);
		Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector2 = array[i].WithZ(array[i].y);
			verts.Add(vector + vector2);
			uvs.Add(tk2dSpriteDefinition.uvs[i]);
			colors.Add(Color.black);
		}
		for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
		{
			tris.Add(count + tk2dSpriteDefinition.indices[j]);
		}
	}

	// Token: 0x06006700 RID: 26368 RVA: 0x00282770 File Offset: 0x00280970
	private static void AddTileAtPosition(IntVector2 position, int index, List<Vector3> verts, List<int> tris, List<Vector2> uvs, List<Color> colors, out Material targetMaterial, tk2dSpriteCollectionData spriteData, float zOffset, bool tilted, Color topColor, Color bottomColor)
	{
		int tileFromRawTile = BuilderUtil.GetTileFromRawTile(index);
		tk2dSpriteDefinition tk2dSpriteDefinition = spriteData.spriteDefinitions[tileFromRawTile];
		targetMaterial = tk2dSpriteDefinition.material;
		int count = verts.Count;
		Vector3 vector = position.ToVector3((float)position.y + zOffset);
		Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector2 = ((!tilted) ? array[i].WithZ(array[i].y) : array[i].WithZ(-array[i].y));
			verts.Add(vector + vector2);
			uvs.Add(tk2dSpriteDefinition.uvs[i]);
		}
		colors.Add(bottomColor);
		colors.Add(bottomColor);
		colors.Add(topColor);
		colors.Add(topColor);
		for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
		{
			tris.Add(count + tk2dSpriteDefinition.indices[j]);
		}
	}

	// Token: 0x06006701 RID: 26369 RVA: 0x0028289C File Offset: 0x00280A9C
	private static void AddTileAtPosition(IntVector2 position, int index, List<Vector3> verts, List<int> tris, List<Vector2> uvs, List<Color> colors, ref Material targetMaterial, tk2dSpriteCollectionData spriteData, float zOffset, bool tilted = false)
	{
		int tileFromRawTile = BuilderUtil.GetTileFromRawTile(index);
		if (tileFromRawTile < 0 || tileFromRawTile >= spriteData.spriteDefinitions.Length)
		{
			Debug.Log(tileFromRawTile.ToString() + " index is out of bounds in SecretRoomBuilder, of indices: " + spriteData.spriteDefinitions.Length.ToString());
			return;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = spriteData.spriteDefinitions[tileFromRawTile];
		targetMaterial = tk2dSpriteDefinition.material;
		int count = verts.Count;
		Vector3 vector = position.ToVector3((float)position.y + zOffset);
		Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector2 = ((!tilted) ? array[i].WithZ(array[i].y) : array[i].WithZ(-array[i].y));
			verts.Add(vector + vector2);
			uvs.Add(tk2dSpriteDefinition.uvs[i]);
			colors.Add(Color.black);
		}
		for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
		{
			tris.Add(count + tk2dSpriteDefinition.indices[j]);
		}
	}

	// Token: 0x06006702 RID: 26370 RVA: 0x002829FC File Offset: 0x00280BFC
	private static GameObject GenerateRoomDoorMesh(RuntimeExitDefinition exit, RoomHandler room, DungeonData dungeonData)
	{
		DungeonData.Direction directionFromRoom = exit.GetDirectionFromRoom(room);
		IntVector2 intVector;
		if (exit.upstreamRoom == room)
		{
			intVector = exit.GetDownstreamBasePosition();
		}
		else
		{
			intVector = exit.GetUpstreamBasePosition();
		}
		DungeonData.Direction direction = directionFromRoom;
		IntVector2 intVector2 = intVector;
		return SecretRoomBuilder.GenerateWallMesh(direction, intVector2, "secret room door object", dungeonData, false);
	}

	// Token: 0x06006703 RID: 26371 RVA: 0x00282A48 File Offset: 0x00280C48
	public static GameObject GenerateWallMesh(DungeonData.Direction exitDirection, IntVector2 exitBasePosition, string objectName = "secret room door object", DungeonData dungeonData = null, bool abridged = false)
	{
		if (dungeonData == null)
		{
			dungeonData = GameManager.Instance.Dungeon.data;
		}
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<Vector2> list6 = new List<Vector2>();
		List<Color> list7 = new List<Color>();
		Material material = null;
		Material material2 = null;
		Material material3 = null;
		Material material4 = null;
		tk2dSpriteCollectionData dungeonCollection = GameManager.Instance.Dungeon.tileIndices.dungeonCollection;
		TileIndexGrid borderGridForCellPosition = SecretRoomBuilder.GetBorderGridForCellPosition(exitBasePosition, dungeonData);
		CellData cellData = dungeonData[exitBasePosition];
		switch (exitDirection)
		{
		case DungeonData.Direction.NORTH:
		{
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Right, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition, borderGridForCellPosition.leftCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Right, borderGridForCellPosition.rightCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			int num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down, num, list, list4, list6, list7, out material3, dungeonCollection, -0.4f, true, new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down + IntVector2.Right, num, list, list4, list6, list7, out material3, dungeonCollection, -0.4f, true, new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down * 2, num, list, list4, list6, list7, out material3, dungeonCollection, 1.6f, true, new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down * 2 + IntVector2.Right, num, list, list4, list6, list7, out material3, dungeonCollection, 1.6f, true, new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f));
			break;
		}
		case DungeonData.Direction.EAST:
		{
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Down, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Zero, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 3, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up, borderGridForCellPosition.bottomCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition.verticalIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			if (!abridged)
			{
				SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 3, borderGridForCellPosition.topCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			}
			Color color = new Color(0f, 0f, 1f, 0f);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down + IntVector2.Right, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallLeft, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color, color);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Right, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallLeft, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color, color);
			if (!abridged)
			{
				SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up + IntVector2.Right, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallLeft, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color, color);
			}
			break;
		}
		case DungeonData.Direction.SOUTH:
		{
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 2 + IntVector2.Right, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition.leftCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 2 + IntVector2.Right, borderGridForCellPosition.rightCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			int num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up, num, list, list4, list6, list7, out material3, dungeonCollection, -0.4f, true, new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up + IntVector2.Right, num, list, list4, list6, list7, out material3, dungeonCollection, -0.4f, true, new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition, num, list, list4, list6, list7, out material3, dungeonCollection, 1.6f, true, new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f));
			num = SecretRoomBuilder.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER], cellData.cellVisualData.roomVisualTypeIndex);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Right, num, list, list4, list6, list7, out material3, dungeonCollection, 1.6f, true, new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f));
			Color color2 = new Color(0f, 0f, 1f, 0f);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOBottomWallBaseTileIndex, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color2, color2);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Right, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOBottomWallBaseTileIndex, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color2, color2);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorTileIndex, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, false, color2, color2);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down + IntVector2.Right, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorTileIndex, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, false, color2, color2);
			break;
		}
		case DungeonData.Direction.WEST:
		{
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Down, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Zero, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			if (!abridged)
			{
				SecretRoomBuilder.AddCeilingTileAtPosition(exitBasePosition + IntVector2.Up * 3, borderGridForCellPosition, list, list2, list6, list7, out material, dungeonCollection);
			}
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up, borderGridForCellPosition.bottomCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 2, borderGridForCellPosition.verticalIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			if (!abridged)
			{
				SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up * 3, borderGridForCellPosition.topCapIndices.GetIndexByWeight(), list, list3, list6, list7, ref material2, dungeonCollection, -2.45f, false);
			}
			Color color3 = new Color(0f, 0f, 1f, 0f);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Down + IntVector2.Left, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallRight, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color3, color3);
			SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Left, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallRight, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color3, color3);
			if (!abridged)
			{
				SecretRoomBuilder.AddTileAtPosition(exitBasePosition + IntVector2.Up + IntVector2.Left, GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallRight, list, list5, list6, list7, out material4, dungeonCollection, 1.55f, true, color3, color3);
			}
			break;
		}
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		for (int i = 0; i < list.Count; i++)
		{
			vector = Vector3.Min(vector, list[i]);
		}
		vector.x = (float)Mathf.FloorToInt(vector.x);
		vector.y = (float)Mathf.FloorToInt(vector.y);
		vector.z = (float)Mathf.FloorToInt(vector.z);
		for (int j = 0; j < list.Count; j++)
		{
			list[j] -= vector;
		}
		mesh.vertices = list.ToArray();
		mesh.uv = list6.ToArray();
		mesh.colors = list7.ToArray();
		mesh.subMeshCount = 4;
		mesh.SetTriangles(list2.ToArray(), 0);
		mesh.SetTriangles(list3.ToArray(), 1);
		mesh.SetTriangles(list4.ToArray(), 2);
		mesh.SetTriangles(list5.ToArray(), 3);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		GameObject gameObject = new GameObject(objectName);
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		gameObject.transform.position = vector;
		meshFilter.mesh = mesh;
		meshRenderer.materials = new Material[] { material, material2, material3, material4 };
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("ShadowCaster"));
		return gameObject;
	}

	// Token: 0x06006704 RID: 26372 RVA: 0x002836D8 File Offset: 0x002818D8
	public static GameObject GenerateRoomCeilingMesh(HashSet<IntVector2> cells, string objectName = "secret room ceiling object", DungeonData dungeonData = null, bool mimicCheck = false)
	{
		if (dungeonData == null)
		{
			dungeonData = GameManager.Instance.Dungeon.data;
		}
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<Vector2> list4 = new List<Vector2>();
		Material material = null;
		Material material2 = null;
		tk2dSpriteCollectionData dungeonCollection = GameManager.Instance.Dungeon.tileIndices.dungeonCollection;
		Vector3 vector = new Vector3(0f, 0f, -3.01f);
		Vector3 vector2 = new Vector3(0f, 0f, -3.02f);
		foreach (IntVector2 intVector in cells)
		{
			TileIndexGrid borderGridForCellPosition = SecretRoomBuilder.GetBorderGridForCellPosition(intVector, dungeonData);
			int indexByWeight = borderGridForCellPosition.centerIndices.GetIndexByWeight();
			int tileFromRawTile = BuilderUtil.GetTileFromRawTile(indexByWeight);
			tk2dSpriteDefinition tk2dSpriteDefinition = dungeonCollection.spriteDefinitions[tileFromRawTile];
			if (material == null)
			{
				material = tk2dSpriteDefinition.material;
			}
			int count = list.Count;
			Vector3 vector3 = intVector.ToVector3((float)intVector.y);
			Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector4 = array[i].WithZ(array[i].y);
				list.Add(vector3 + vector4 + vector);
				list4.Add(tk2dSpriteDefinition.uvs[i]);
			}
			for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
			{
				list2.Add(count + tk2dSpriteDefinition.indices[j]);
			}
			int x = intVector.x;
			int y = intVector.y;
			bool flag = SecretRoomBuilder.IsTopWall(x, y, dungeonData, cells);
			bool flag2 = SecretRoomBuilder.IsTopWall(x + 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWall(x, y, dungeonData, cells) && (SecretRoomBuilder.IsWall(x, y + 1, dungeonData, cells) || SecretRoomBuilder.IsTopWall(x, y + 1, dungeonData, cells));
			bool flag3 = (!SecretRoomBuilder.IsWall(x + 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWall(x + 1, y, dungeonData, cells)) || SecretRoomBuilder.IsFaceWallHigher(x + 1, y, dungeonData, cells);
			bool flag4 = y > 3 && SecretRoomBuilder.IsFaceWallHigher(x + 1, y - 1, dungeonData, cells) && !SecretRoomBuilder.IsFaceWallHigher(x, y - 1, dungeonData, cells);
			bool flag5 = y > 3 && SecretRoomBuilder.IsFaceWallHigher(x, y - 1, dungeonData, cells);
			bool flag6 = y > 3 && SecretRoomBuilder.IsFaceWallHigher(x - 1, y - 1, dungeonData, cells) && !SecretRoomBuilder.IsFaceWallHigher(x, y - 1, dungeonData, cells);
			bool flag7 = (!SecretRoomBuilder.IsWall(x - 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWall(x - 1, y, dungeonData, cells)) || SecretRoomBuilder.IsFaceWallHigher(x - 1, y, dungeonData, cells);
			bool flag8 = SecretRoomBuilder.IsTopWall(x - 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWall(x, y, dungeonData, cells) && (SecretRoomBuilder.IsWall(x, y + 1, dungeonData, cells) || SecretRoomBuilder.IsTopWall(x, y + 1, dungeonData, cells));
			if (mimicCheck)
			{
				flag = SecretRoomBuilder.IsTopWallOrSecret(x, y, dungeonData, cells);
				flag2 = SecretRoomBuilder.IsTopWallOrSecret(x + 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWallOrSecret(x, y, dungeonData, cells) && (SecretRoomBuilder.IsWallOrSecret(x, y + 1, dungeonData, cells) || SecretRoomBuilder.IsTopWallOrSecret(x, y + 1, dungeonData, cells));
				flag3 = (!SecretRoomBuilder.IsWallOrSecret(x + 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWallOrSecret(x + 1, y, dungeonData, cells)) || SecretRoomBuilder.IsFaceWallHigherOrSecret(x + 1, y, dungeonData, cells);
				flag4 = y > 3 && SecretRoomBuilder.IsFaceWallHigherOrSecret(x + 1, y - 1, dungeonData, cells) && !SecretRoomBuilder.IsFaceWallHigherOrSecret(x, y - 1, dungeonData, cells);
				flag5 = y > 3 && SecretRoomBuilder.IsFaceWallHigherOrSecret(x, y - 1, dungeonData, cells);
				flag6 = y > 3 && SecretRoomBuilder.IsFaceWallHigherOrSecret(x - 1, y - 1, dungeonData, cells) && !SecretRoomBuilder.IsFaceWallHigherOrSecret(x, y - 1, dungeonData, cells);
				flag7 = (!SecretRoomBuilder.IsWallOrSecret(x - 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWallOrSecret(x - 1, y, dungeonData, cells)) || SecretRoomBuilder.IsFaceWallHigherOrSecret(x - 1, y, dungeonData, cells);
				flag8 = SecretRoomBuilder.IsTopWallOrSecret(x - 1, y, dungeonData, cells) && !SecretRoomBuilder.IsTopWallOrSecret(x, y, dungeonData, cells) && (SecretRoomBuilder.IsWallOrSecret(x, y + 1, dungeonData, cells) || SecretRoomBuilder.IsTopWallOrSecret(x, y + 1, dungeonData, cells));
			}
			if (flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7 || flag8)
			{
				int num = borderGridForCellPosition.GetIndexGivenSides(flag, flag2, flag3, flag4, flag5, flag6, flag7, flag8);
				if (borderGridForCellPosition.UsesRatChunkBorders)
				{
					bool flag9 = y > 3;
					if (flag9)
					{
						flag9 = !dungeonData[x, y - 1].HasFloorNeighbor(dungeonData, false, true);
					}
					TileIndexGrid.RatChunkResult ratChunkResult = TileIndexGrid.RatChunkResult.NONE;
					num = borderGridForCellPosition.GetRatChunkIndexGivenSides(flag, flag2, flag3, flag4, flag5, flag6, flag7, flag8, flag9, out ratChunkResult);
				}
				int tileFromRawTile2 = BuilderUtil.GetTileFromRawTile(num);
				tk2dSpriteDefinition tk2dSpriteDefinition2 = dungeonCollection.spriteDefinitions[tileFromRawTile2];
				if (material2 == null)
				{
					material2 = tk2dSpriteDefinition2.material;
				}
				int count2 = list.Count;
				Vector3 vector5 = intVector.ToVector3((float)intVector.y);
				Vector3[] array2 = tk2dSpriteDefinition2.ConstructExpensivePositions();
				for (int k = 0; k < array2.Length; k++)
				{
					Vector3 vector6 = array2[k].WithZ(array2[k].y);
					list.Add(vector5 + vector6 + vector2);
					list4.Add(tk2dSpriteDefinition2.uvs[k]);
				}
				for (int l = 0; l < tk2dSpriteDefinition2.indices.Length; l++)
				{
					list3.Add(count2 + tk2dSpriteDefinition2.indices[l]);
				}
			}
		}
		Vector3 vector7 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		for (int m = 0; m < list.Count; m++)
		{
			vector7 = Vector3.Min(vector7, list[m]);
		}
		for (int n = 0; n < list.Count; n++)
		{
			list[n] -= vector7;
		}
		mesh.vertices = list.ToArray();
		mesh.uv = list4.ToArray();
		mesh.subMeshCount = 2;
		mesh.SetTriangles(list2.ToArray(), 0);
		mesh.SetTriangles(list3.ToArray(), 1);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		GameObject gameObject = new GameObject(objectName);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		gameObject.transform.position = vector7;
		meshFilter.mesh = mesh;
		meshRenderer.materials = new Material[] { material, material2 };
		return gameObject;
	}

	// Token: 0x06006705 RID: 26373 RVA: 0x00283E8C File Offset: 0x0028208C
	private static HashSet<IntVector2> CorrectForDoubledSecretRoomness(RoomHandler room, DungeonData data)
	{
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		if (room.area.instanceUsedExits.Count == 1)
		{
			RuntimeExitDefinition runtimeExitDefinition = room.exitDefinitionsByExit[room.area.exitToLocalDataMap[room.area.instanceUsedExits[0]]];
			if (runtimeExitDefinition.downstreamRoom == room && runtimeExitDefinition.upstreamRoom.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				List<IntVector2> cellRepresentationIncFacewalls = runtimeExitDefinition.upstreamRoom.area.prototypeRoom.GetCellRepresentationIncFacewalls();
				List<IntVector2> list = new List<IntVector2>(IntVector2.CardinalsAndOrdinals);
				foreach (IntVector2 intVector in cellRepresentationIncFacewalls)
				{
					hashSet.Add(runtimeExitDefinition.upstreamRoom.area.basePosition + intVector);
					foreach (IntVector2 intVector2 in list)
					{
						hashSet.Add(runtimeExitDefinition.upstreamRoom.area.basePosition + intVector + intVector2);
					}
				}
			}
		}
		List<IntVector2> list2 = new List<IntVector2>();
		foreach (IntVector2 intVector3 in hashSet)
		{
			if (data[intVector3] != null && data[intVector3].isSecretRoomCell && !data[intVector3].isExitCell)
			{
				data[intVector3].isSecretRoomCell = false;
			}
			else
			{
				list2.Add(intVector3);
			}
		}
		foreach (IntVector2 intVector4 in list2)
		{
			hashSet.Remove(intVector4);
		}
		return hashSet;
	}

	// Token: 0x06006706 RID: 26374 RVA: 0x002840D4 File Offset: 0x002822D4
	public static GameObject BuildRoomCover(RoomHandler room, tk2dTileMap tileMap, DungeonData dungeonData)
	{
		HashSet<IntVector2> hashSet = null;
		if (!Dungeon.IsGenerating)
		{
			hashSet = SecretRoomBuilder.CorrectForDoubledSecretRoomness(room, dungeonData);
		}
		HashSet<IntVector2> roomCeilingCells = SecretRoomBuilder.GetRoomCeilingCells(room);
		GameObject gameObject = SecretRoomBuilder.GenerateRoomCeilingMesh(roomCeilingCells, "secret room ceiling object", dungeonData, false);
		List<SecretRoomDoorBeer> list = new List<SecretRoomDoorBeer>();
		for (int i = 0; i < room.area.instanceUsedExits.Count; i++)
		{
			PrototypeRoomExit prototypeRoomExit = room.area.instanceUsedExits[i];
			if (!room.area.exitToLocalDataMap[prototypeRoomExit].oneWayDoor)
			{
				RuntimeExitDefinition runtimeExitDefinition = room.exitDefinitionsByExit[room.area.exitToLocalDataMap[prototypeRoomExit]];
				if (Dungeon.IsGenerating || runtimeExitDefinition.downstreamRoom == room || !(runtimeExitDefinition.downstreamRoom.area.prototypeRoom != null) || runtimeExitDefinition.downstreamRoom.area.prototypeRoom.category != PrototypeDungeonRoom.RoomCategory.SECRET)
				{
					GameObject gameObject2 = SecretRoomBuilder.GenerateRoomDoorMesh(runtimeExitDefinition, room, dungeonData);
					SecretRoomDoorBeer secretRoomDoorBeer = gameObject2.AddComponent<SecretRoomDoorBeer>();
					secretRoomDoorBeer.exitDef = room.exitDefinitionsByExit[room.area.exitToLocalDataMap[prototypeRoomExit]];
					secretRoomDoorBeer.linkedRoom = room.connectedRoomsByExit[prototypeRoomExit];
					list.Add(secretRoomDoorBeer);
				}
			}
		}
		GameObject gameObject3 = new GameObject("Secret Room");
		gameObject3.transform.position = gameObject.transform.position;
		gameObject.transform.parent = gameObject3.transform;
		SecretRoomManager secretRoomManager = gameObject3.AddComponent<SecretRoomManager>();
		List<IntVector2> list2 = new List<IntVector2>(roomCeilingCells);
		secretRoomManager.InitializeCells(list2);
		List<SecretRoomExitData> list3 = SecretRoomUtility.BuildRoomExitColliders(room);
		for (int j = 0; j < list3.Count; j++)
		{
			list[j].collider = list3[j];
		}
		secretRoomManager.ceilingRenderer = gameObject.GetComponent<Renderer>();
		for (int k = 0; k < list.Count; k++)
		{
			secretRoomManager.doorObjects.Add(list[k]);
		}
		secretRoomManager.room = room;
		room.secretRoomManager = secretRoomManager;
		string roomName = room.GetRoomName();
		if (!string.IsNullOrEmpty(roomName) && roomName.Contains("SewersEntrance"))
		{
			secretRoomManager.revealStyle = SecretRoomManager.SecretRoomRevealStyle.FireplacePuzzle;
			secretRoomManager.InitializeForStyle();
		}
		else if (SecretRoomUtility.FloorHasComplexPuzzle || GameManager.Instance.Dungeon.SecretRoomComplexTriggers.Count == 0)
		{
			secretRoomManager.InitializeForStyle();
		}
		else
		{
			SecretRoomUtility.FloorHasComplexPuzzle = true;
			secretRoomManager.revealStyle = SecretRoomManager.SecretRoomRevealStyle.ComplexPuzzle;
			secretRoomManager.InitializeForStyle();
		}
		if (hashSet != null && hashSet.Count > 0)
		{
			foreach (IntVector2 intVector in hashSet)
			{
				dungeonData[intVector].isSecretRoomCell = true;
			}
		}
		return null;
	}

	// Token: 0x040062CC RID: 25292
	private const float CEILING_HEIGHT_OFFSET = -3.01f;

	// Token: 0x040062CD RID: 25293
	private const float BORDER_HEIGHT_OFFSET = -3.02f;
}
