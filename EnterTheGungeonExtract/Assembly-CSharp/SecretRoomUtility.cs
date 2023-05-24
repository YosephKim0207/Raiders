using System;
using System.Collections.Generic;
using Dungeonator;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000F68 RID: 3944
public static class SecretRoomUtility
{
	// Token: 0x060054F6 RID: 21750 RVA: 0x002035B4 File Offset: 0x002017B4
	private static bool IsSolid(CellData cell)
	{
		return cell.type == CellType.WALL || cell.isSecretRoomCell;
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x002035D0 File Offset: 0x002017D0
	private static bool IsFaceWallHigher(int x, int y, CellData[][] t)
	{
		return SecretRoomUtility.IsSolid(t[x][y]) && SecretRoomUtility.IsSolid(t[x][y - 1]) && !SecretRoomUtility.IsSolid(t[x][y - 2]);
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x00203608 File Offset: 0x00201808
	private static bool IsFaceWallLower(int x, int y, CellData[][] t)
	{
		return SecretRoomUtility.IsSolid(t[x][y]) && !SecretRoomUtility.IsSolid(t[x][y - 1]);
	}

	// Token: 0x060054F9 RID: 21753 RVA: 0x00203630 File Offset: 0x00201830
	public static void ClearPerLevelData()
	{
		SecretRoomUtility.FloorHasComplexPuzzle = false;
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x00203638 File Offset: 0x00201838
	public static int GetIndexFromTupleArray(CellData current, List<Tuple<int, TilesetIndexMetadata>> list, int roomTypeIndex, float forcedRand = -1f)
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
		if (forcedRand >= 0f)
		{
			num2 = forcedRand * num;
		}
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

	// Token: 0x060054FB RID: 21755 RVA: 0x00203784 File Offset: 0x00201984
	private static bool IsSecretDoorTopBorder(CellData cellToCheck, DungeonData data)
	{
		if (cellToCheck.isSecretRoomCell)
		{
			if (data.cellData[cellToCheck.position.x][cellToCheck.position.y - 1].type == CellType.FLOOR && !data.cellData[cellToCheck.position.x][cellToCheck.position.y - 1].isSecretRoomCell)
			{
				return true;
			}
			if (data.cellData[cellToCheck.position.x][cellToCheck.position.y - 2].type == CellType.FLOOR && !data.cellData[cellToCheck.position.x][cellToCheck.position.y - 2].isSecretRoomCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x0020384C File Offset: 0x00201A4C
	private static int GetIndexGivenCell(IntVector2 position, List<IntVector2> cellRepresentation, DungeonData data, out int facewall, out int sidewall)
	{
		facewall = 0;
		sidewall = 0;
		TileIndexGrid borderGridForCellPosition = SecretRoomUtility.GetBorderGridForCellPosition(position, data);
		CellData cellData = data.cellData[position.x][position.y];
		List<CellData> cellNeighbors = data.GetCellNeighbors(cellData, false);
		if (cellNeighbors[1].type == CellType.FLOOR && !cellNeighbors[1].isSecretRoomCell)
		{
			sidewall = 1;
		}
		if (cellNeighbors[3].type == CellType.FLOOR && !cellNeighbors[3].isSecretRoomCell)
		{
			sidewall = -1;
		}
		if (cellNeighbors[2].type == CellType.FLOOR && !cellNeighbors[2].isSecretRoomCell)
		{
			facewall = 1;
			return SecretRoomUtility.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER], cellData.cellVisualData.roomVisualTypeIndex, -1f);
		}
		if (data.cellData[cellData.position.x][cellData.position.y - 2].type == CellType.FLOOR && !data.cellData[cellData.position.x][cellData.position.y - 2].isSecretRoomCell)
		{
			facewall = 2;
			return SecretRoomUtility.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER], cellData.cellVisualData.roomVisualTypeIndex, -1f);
		}
		bool[] array = new bool[4];
		for (int i = 0; i < 4; i++)
		{
			bool flag = SecretRoomUtility.IsFaceWallHigher(cellNeighbors[i].position.x, cellNeighbors[i].position.y, data.cellData) || SecretRoomUtility.IsFaceWallLower(cellNeighbors[i].position.x, cellNeighbors[i].position.y, data.cellData);
			bool flag2 = cellNeighbors[i].type != CellType.WALL && !cellNeighbors[i].isSecretRoomCell && SecretRoomUtility.IsSolid(data.cellData[cellNeighbors[i].position.x][cellNeighbors[i].position.y - 1]);
			if ((cellNeighbors[i].type != CellType.WALL || flag) && !cellNeighbors[i].isSecretRoomCell && !flag2)
			{
				array[i] = true;
			}
			if (SecretRoomUtility.IsSecretDoorTopBorder(cellNeighbors[i], data))
			{
				array[i] = true;
				facewall = 3;
			}
			if (array[i] && ((cellData.type != CellType.WALL && !cellData.IsTopWall()) || cellData.IsAnyFaceWall()))
			{
				facewall = 3;
			}
		}
		return borderGridForCellPosition.GetIndexGivenSides(array[0], array[1], array[2], array[3]);
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x00203B10 File Offset: 0x00201D10
	private static TileIndexGrid GetBorderGridForCellPosition(IntVector2 position, DungeonData data)
	{
		TileIndexGrid tileIndexGrid = GameManager.Instance.Dungeon.roomMaterialDefinitions[data.cellData[position.x][position.y].cellVisualData.roomVisualTypeIndex].roomCeilingBorderGrid;
		if (tileIndexGrid == null)
		{
			tileIndexGrid = GameManager.Instance.Dungeon.roomMaterialDefinitions[0].roomCeilingBorderGrid;
		}
		return tileIndexGrid;
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x00203B78 File Offset: 0x00201D78
	private static void BuildRoomCoverExitIndices(RoomHandler room, tk2dTileMap tileMap, DungeonData dungeonData, List<IntVector2> cellRepresentation, HashSet<SecretRoomUtility.IntVector2WithIndex> ceilingCells, HashSet<SecretRoomUtility.IntVector2WithIndex> borderCells, HashSet<SecretRoomUtility.IntVector2WithIndex> facewallCells)
	{
		for (int i = 0; i < room.area.instanceUsedExits.Count; i++)
		{
			PrototypeRoomExit prototypeRoomExit = room.area.instanceUsedExits[i];
			RuntimeRoomExitData runtimeRoomExitData = room.area.exitToLocalDataMap[prototypeRoomExit];
			PrototypeRoomExit exitConnectedToRoom = room.connectedRoomsByExit[prototypeRoomExit].GetExitConnectedToRoom(room);
			RuntimeRoomExitData runtimeRoomExitData2 = room.connectedRoomsByExit[prototypeRoomExit].area.exitToLocalDataMap[exitConnectedToRoom];
			int num = runtimeRoomExitData.TotalExitLength + runtimeRoomExitData2.TotalExitLength - 1;
			if (prototypeRoomExit.exitDirection == DungeonData.Direction.NORTH)
			{
				num += 2;
			}
			for (int j = 0; j < prototypeRoomExit.containedCells.Count; j++)
			{
				for (int k = 0; k < num; k++)
				{
					IntVector2 intVector = prototypeRoomExit.containedCells[j].ToIntVector2(VectorConversions.Round) + room.area.basePosition - IntVector2.One;
					List<IntVector2> list = new List<IntVector2>();
					if (prototypeRoomExit.exitDirection == DungeonData.Direction.NORTH)
					{
						intVector += IntVector2.Up * k;
						list.Add(intVector + IntVector2.Left);
						list.Add(intVector + IntVector2.Right);
					}
					else if (prototypeRoomExit.exitDirection == DungeonData.Direction.SOUTH)
					{
						intVector += IntVector2.Down * k;
						if (k < num - 2)
						{
							list.Add(intVector + IntVector2.Left);
							list.Add(intVector + IntVector2.Right);
						}
					}
					else if (prototypeRoomExit.exitDirection == DungeonData.Direction.EAST)
					{
						intVector += IntVector2.Right * k;
						list.Add(intVector + IntVector2.Up);
						list.Add(intVector + IntVector2.Up * 2);
						list.Add(intVector + IntVector2.Up * 3);
					}
					else
					{
						intVector += IntVector2.Left * k;
						list.Add(intVector + IntVector2.Up);
						list.Add(intVector + IntVector2.Up * 2);
						list.Add(intVector + IntVector2.Up * 3);
					}
					list.Add(intVector);
					for (int l = 0; l < list.Count; l++)
					{
						int num2 = 0;
						int num3 = 0;
						int indexGivenCell = SecretRoomUtility.GetIndexGivenCell(list[l], cellRepresentation, dungeonData, out num2, out num3);
						TileIndexGrid borderGridForCellPosition = SecretRoomUtility.GetBorderGridForCellPosition(list[l], dungeonData);
						if (num2 > 0)
						{
							SecretRoomUtility.IntVector2WithIndex intVector2WithIndex = new SecretRoomUtility.IntVector2WithIndex(list[l], indexGivenCell);
							intVector2WithIndex.facewallID = num2;
							intVector2WithIndex.sidewallID = num3;
							if (num2 == 1)
							{
								intVector2WithIndex.meshColor = new Color[]
								{
									new Color(0f, 0f, 1f),
									new Color(0f, 0f, 1f),
									new Color(0f, 0.5f, 1f),
									new Color(0f, 0.5f, 1f)
								};
								facewallCells.Add(intVector2WithIndex);
							}
							else if (num2 == 2)
							{
								intVector2WithIndex.meshColor = new Color[]
								{
									new Color(0f, 0.5f, 1f),
									new Color(0f, 0.5f, 1f),
									new Color(0f, 1f, 1f),
									new Color(0f, 1f, 1f)
								};
								facewallCells.Add(intVector2WithIndex);
							}
							else if (num2 == 3)
							{
								if (!borderGridForCellPosition.centerIndices.indices.Contains(intVector2WithIndex.index))
								{
									facewallCells.Add(intVector2WithIndex);
								}
								ceilingCells.Add(new SecretRoomUtility.IntVector2WithIndex(list[l], borderGridForCellPosition.centerIndices.GetIndexByWeight())
								{
									zOffset = 1.5f
								});
							}
						}
						else if (borderGridForCellPosition.centerIndices.indices.Contains(indexGivenCell))
						{
							ceilingCells.Add(new SecretRoomUtility.IntVector2WithIndex(list[l], indexGivenCell)
							{
								sidewallID = num3
							});
						}
						else
						{
							SecretRoomUtility.IntVector2WithIndex intVector2WithIndex2 = new SecretRoomUtility.IntVector2WithIndex(list[l], indexGivenCell);
							intVector2WithIndex2.sidewallID = num3;
							intVector2WithIndex2.zOffset += 1f;
							borderCells.Add(intVector2WithIndex2);
							SecretRoomUtility.IntVector2WithIndex intVector2WithIndex3 = new SecretRoomUtility.IntVector2WithIndex(list[l], borderGridForCellPosition.centerIndices.GetIndexByWeight());
							intVector2WithIndex3.sidewallID = num3;
							intVector2WithIndex3.zOffset += 0.75f;
							ceilingCells.Add(intVector2WithIndex3);
						}
					}
				}
			}
		}
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x002040DC File Offset: 0x002022DC
	private static Mesh BuildAOMesh(HashSet<SecretRoomUtility.IntVector2WithIndex> facewallIndices, out Material material)
	{
		material = null;
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector2> list3 = new List<Vector2>();
		List<Color> list4 = new List<Color>();
		tk2dSpriteCollectionData dungeonCollection = GameManager.Instance.Dungeon.tileIndices.dungeonCollection;
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		foreach (SecretRoomUtility.IntVector2WithIndex intVector2WithIndex in facewallIndices)
		{
			if (!hashSet.Contains(intVector2WithIndex.position))
			{
				hashSet.Add(intVector2WithIndex.position);
				int num = -1;
				Vector3 vector = Vector3.zero;
				if (intVector2WithIndex.facewallID == 1)
				{
					num = BuilderUtil.GetTileFromRawTile(GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorTileIndex);
				}
				else if (intVector2WithIndex.sidewallID == 1)
				{
					num = BuilderUtil.GetTileFromRawTile(GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallLeft);
					vector += Vector3.right + Vector3.down + Vector3.forward;
				}
				else if (intVector2WithIndex.sidewallID == -1)
				{
					num = BuilderUtil.GetTileFromRawTile(GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOFloorWallRight);
				}
				if (num != -1)
				{
					tk2dSpriteDefinition tk2dSpriteDefinition = dungeonCollection.spriteDefinitions[num];
					if (material == null)
					{
						material = tk2dSpriteDefinition.material;
					}
					int num2 = list.Count;
					Vector3 vector2 = intVector2WithIndex.position.ToVector3((float)(intVector2WithIndex.position.y - 1)) + vector;
					vector2.y -= 1f;
					Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
					for (int i = 0; i < array.Length; i++)
					{
						Vector3 vector3 = array[i];
						vector3 = vector3.WithZ(vector3.y);
						if (intVector2WithIndex.facewallID == 1)
						{
							vector3.z += 2f;
						}
						list.Add(vector2 + vector3 + intVector2WithIndex.GetOffset());
						list3.Add(tk2dSpriteDefinition.uvs[i]);
						list4.Add(intVector2WithIndex.meshColor[i % 4]);
					}
					for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
					{
						list2.Add(num2 + tk2dSpriteDefinition.indices[j]);
					}
					if (intVector2WithIndex.facewallID == 1)
					{
						int tileFromRawTile = BuilderUtil.GetTileFromRawTile(GameManager.Instance.Dungeon.tileIndices.aoTileIndices.AOBottomWallBaseTileIndex);
						tk2dSpriteDefinition = dungeonCollection.spriteDefinitions[tileFromRawTile];
						num2 = list.Count;
						vector2 = intVector2WithIndex.position.ToVector3((float)intVector2WithIndex.position.y);
						array = tk2dSpriteDefinition.ConstructExpensivePositions();
						for (int k = 0; k < array.Length; k++)
						{
							Vector3 vector4 = array[k];
							vector4 = vector4.WithZ(-vector4.y);
							if (intVector2WithIndex.facewallID == 1)
							{
								vector4.z += 2f;
							}
							list.Add(vector2 + vector4 + intVector2WithIndex.GetOffset());
							list3.Add(tk2dSpriteDefinition.uvs[k]);
							list4.Add(intVector2WithIndex.meshColor[k % 4]);
						}
						for (int l = 0; l < tk2dSpriteDefinition.indices.Length; l++)
						{
							list2.Add(num2 + tk2dSpriteDefinition.indices[l]);
						}
					}
				}
			}
		}
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.uv = list3.ToArray();
		mesh.colors = list4.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x00204528 File Offset: 0x00202728
	private static Mesh BuildTargetMesh(HashSet<SecretRoomUtility.IntVector2WithIndex> cellIndices, out Material material, bool facewall = false)
	{
		material = null;
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector2> list3 = new List<Vector2>();
		List<Color> list4 = new List<Color>();
		List<Vector3> list5 = new List<Vector3>();
		tk2dSpriteCollectionData dungeonCollection = GameManager.Instance.Dungeon.tileIndices.dungeonCollection;
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		foreach (SecretRoomUtility.IntVector2WithIndex intVector2WithIndex in cellIndices)
		{
			if (!hashSet.Contains(intVector2WithIndex.position))
			{
				hashSet.Add(intVector2WithIndex.position);
				int tileFromRawTile = BuilderUtil.GetTileFromRawTile(intVector2WithIndex.index);
				tk2dSpriteDefinition tk2dSpriteDefinition = dungeonCollection.spriteDefinitions[tileFromRawTile];
				if (material == null)
				{
					material = tk2dSpriteDefinition.material;
				}
				int count = list.Count;
				Vector3 vector = intVector2WithIndex.position.ToVector3((float)intVector2WithIndex.position.y);
				Vector3[] array = tk2dSpriteDefinition.ConstructExpensivePositions();
				for (int i = 0; i < array.Length; i++)
				{
					Vector3 vector2 = array[i];
					if (facewall)
					{
						if (intVector2WithIndex.facewallID > 2)
						{
							vector2 = vector2.WithZ(vector2.y);
							vector2.z -= 2.25f;
						}
						else
						{
							vector2 = vector2.WithZ(-vector2.y);
							if (intVector2WithIndex.facewallID == 1)
							{
								vector2.z += 2f;
							}
						}
					}
					else
					{
						vector2 = vector2.WithZ(vector2.y);
					}
					list.Add(vector + vector2 + intVector2WithIndex.GetOffset());
					list5.Add(Vector3.back);
					list3.Add(tk2dSpriteDefinition.uvs[i]);
					list4.Add(intVector2WithIndex.meshColor[i % 4]);
				}
				for (int j = 0; j < tk2dSpriteDefinition.indices.Length; j++)
				{
					list2.Add(count + tk2dSpriteDefinition.indices[j]);
				}
			}
		}
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.normals = list5.ToArray();
		mesh.uv = list3.ToArray();
		mesh.colors = list4.ToArray();
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x002047CC File Offset: 0x002029CC
	private static GameObject CreateObjectForMesh(Mesh meshTarget, Material materialTarget, float zHeight, Transform parentObject, bool ao = false)
	{
		GameObject gameObject = new GameObject("Secret Room Mesh");
		gameObject.transform.position = new Vector3(0f, 0f, zHeight);
		gameObject.transform.parent = parentObject;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter.sharedMesh = meshTarget;
		meshRenderer.sharedMaterial = materialTarget;
		DepthLookupManager.ProcessRenderer(meshRenderer, DepthLookupManager.GungeonSortingLayer.FOREGROUND);
		if (!ao)
		{
			gameObject.layer = LayerMask.NameToLayer("ShadowCaster");
		}
		return gameObject;
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x00204848 File Offset: 0x00202A48
	public static List<SecretRoomExitData> BuildRoomExitColliders(RoomHandler room)
	{
		List<SecretRoomExitData> list = new List<SecretRoomExitData>();
		if (!room.area.IsProceduralRoom)
		{
			for (int i = 0; i < room.area.instanceUsedExits.Count; i++)
			{
				if (!room.area.exitToLocalDataMap[room.area.instanceUsedExits[i]].oneWayDoor)
				{
					RuntimeExitDefinition runtimeExitDefinition = room.exitDefinitionsByExit[room.area.exitToLocalDataMap[room.area.instanceUsedExits[i]]];
					if (Dungeon.IsGenerating || runtimeExitDefinition.downstreamRoom == room || !(runtimeExitDefinition.downstreamRoom.area.prototypeRoom != null) || runtimeExitDefinition.downstreamRoom.area.prototypeRoom.category != PrototypeDungeonRoom.RoomCategory.SECRET)
					{
						GameObject gameObject = new GameObject("secret exit collider");
						SpeculativeRigidbody speculativeRigidbody = gameObject.AddComponent<SpeculativeRigidbody>();
						gameObject.AddComponent<PersistentVFXManagerBehaviour>();
						speculativeRigidbody.CollideWithTileMap = false;
						speculativeRigidbody.CollideWithOthers = true;
						speculativeRigidbody.PixelColliders = new List<PixelCollider>();
						PrototypeRoomExit prototypeRoomExit = room.area.instanceUsedExits[i];
						RuntimeRoomExitData runtimeRoomExitData = room.area.exitToLocalDataMap[prototypeRoomExit];
						RoomHandler roomHandler = room.connectedRoomsByExit[prototypeRoomExit];
						PrototypeRoomExit exitConnectedToRoom = roomHandler.GetExitConnectedToRoom(room);
						int num = roomHandler.area.exitToLocalDataMap[exitConnectedToRoom].TotalExitLength - 1;
						IntVector2 intVector = IntVector2.Zero;
						IntVector2 zero = IntVector2.Zero;
						int num2 = 0;
						if (prototypeRoomExit.exitDirection == DungeonData.Direction.NORTH)
						{
							intVector = room.area.basePosition + runtimeRoomExitData.ExitOrigin + IntVector2.Left + num * IntVector2.Up;
							zero = new IntVector2(2, 1);
							num2 = 8;
						}
						else if (prototypeRoomExit.exitDirection == DungeonData.Direction.EAST)
						{
							intVector = room.area.basePosition + runtimeRoomExitData.ExitOrigin + IntVector2.NegOne + num * IntVector2.Right;
							zero = new IntVector2(1, 4);
						}
						else if (prototypeRoomExit.exitDirection == DungeonData.Direction.WEST)
						{
							intVector = room.area.basePosition + runtimeRoomExitData.ExitOrigin + IntVector2.NegOne + num * IntVector2.Left;
							zero = new IntVector2(1, 4);
						}
						else if (prototypeRoomExit.exitDirection == DungeonData.Direction.SOUTH)
						{
							intVector = room.area.basePosition + runtimeRoomExitData.ExitOrigin + IntVector2.NegOne + num * IntVector2.Down;
							PixelCollider pixelCollider = new PixelCollider();
							pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
							pixelCollider.CollisionLayer = CollisionLayer.LowObstacle;
							pixelCollider.ManualOffsetX = 0;
							pixelCollider.ManualOffsetY = -16;
							pixelCollider.ManualWidth = 32;
							pixelCollider.ManualHeight = 16;
							speculativeRigidbody.PixelColliders.Add(pixelCollider);
							intVector += IntVector2.Up;
							zero = new IntVector2(2, 1);
						}
						gameObject.transform.position = intVector.ToVector3();
						PixelCollider pixelCollider2 = new PixelCollider();
						pixelCollider2.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
						pixelCollider2.CollisionLayer = CollisionLayer.HighObstacle;
						pixelCollider2.ManualWidth = 16 * zero.x;
						pixelCollider2.ManualHeight = 16 * zero.y + num2;
						speculativeRigidbody.PixelColliders.Add(pixelCollider2);
						speculativeRigidbody.ForceRegenerate(null, null);
						list.Add(new SecretRoomExitData(gameObject, prototypeRoomExit.exitDirection));
					}
				}
			}
		}
		else
		{
			Debug.LogError("no support for secret procedural rooms yet.");
		}
		return list;
	}

	// Token: 0x04004DFA RID: 19962
	public static Dictionary<TilesetIndexMetadata.TilesetFlagType, List<Tuple<int, TilesetIndexMetadata>>> metadataLookupTableRef;

	// Token: 0x04004DFB RID: 19963
	public static bool FloorHasComplexPuzzle;

	// Token: 0x02000F69 RID: 3945
	internal class IntVector2WithIndexEqualityComparer : IEqualityComparer<SecretRoomUtility.IntVector2WithIndex>
	{
		// Token: 0x06005505 RID: 21765 RVA: 0x00204C14 File Offset: 0x00202E14
		public bool Equals(SecretRoomUtility.IntVector2WithIndex v1, SecretRoomUtility.IntVector2WithIndex v2)
		{
			return v1.position == v2.position;
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x00204C30 File Offset: 0x00202E30
		public int GetHashCode(SecretRoomUtility.IntVector2WithIndex v1)
		{
			return v1.position.GetHashCode();
		}
	}

	// Token: 0x02000F6A RID: 3946
	internal class IntVector2WithIndex
	{
		// Token: 0x06005507 RID: 21767 RVA: 0x00204C44 File Offset: 0x00202E44
		public IntVector2WithIndex(IntVector2 vec, int i)
		{
			this.position = vec;
			this.index = i;
			this.meshColor = new Color[]
			{
				Color.black,
				Color.black,
				Color.black,
				Color.black
			};
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x00204CB8 File Offset: 0x00202EB8
		public IntVector2WithIndex(IntVector2 vec, int i, Color c)
		{
			this.position = vec;
			this.index = i;
			this.meshColor = new Color[] { c, c, c, c };
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x00204D1C File Offset: 0x00202F1C
		public IntVector2WithIndex(IntVector2 vec, int i, Color bottom, Color top)
		{
			this.position = vec;
			this.index = i;
			this.meshColor = new Color[] { bottom, bottom, top, top };
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x00204D80 File Offset: 0x00202F80
		public Vector3 GetOffset()
		{
			return new Vector3(0f, 0f, this.zOffset);
		}

		// Token: 0x04004DFC RID: 19964
		public IntVector2 position;

		// Token: 0x04004DFD RID: 19965
		public int index;

		// Token: 0x04004DFE RID: 19966
		public float zOffset;

		// Token: 0x04004DFF RID: 19967
		public Color[] meshColor;

		// Token: 0x04004E00 RID: 19968
		public int facewallID;

		// Token: 0x04004E01 RID: 19969
		public int sidewallID;
	}
}
