using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F43 RID: 3907
public class TK2DInteriorDecorator
{
	// Token: 0x0600541B RID: 21531 RVA: 0x001F5124 File Offset: 0x001F3324
	public TK2DInteriorDecorator(TK2DDungeonAssembler assembler)
	{
		this.m_assembler = assembler;
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001F514C File Offset: 0x001F334C
	private void DecorateRoomExit(RoomHandler r, RuntimeRoomExitData usedExit, Dungeon d, tk2dTileMap map)
	{
		RoomHandler roomHandler = r.connectedRoomsByExit[usedExit.referencedExit];
		if (usedExit.referencedExit.exitDirection == DungeonData.Direction.NORTH)
		{
			IntVector2 intVector = r.area.basePosition + usedExit.ExitOrigin - IntVector2.One;
			int num = 0;
			while (d.data.isFaceWallLower(intVector.x - num - 1, intVector.y))
			{
				num++;
			}
			int num2 = 0;
			while (d.data.isFaceWallLower(intVector.x + usedExit.referencedExit.ExitCellCount + num2, intVector.y))
			{
				num2++;
			}
			int num3 = Math.Min(num, num2);
			int num4 = 0;
			if (num3 > 0)
			{
				for (int i = 0; i < 3; i++)
				{
					IntVector2 intVector2 = IntVector2.Zero;
					StampDataBase stampDataBase;
					if (i == 0 || i == 2)
					{
						stampDataBase = d.stampData.GetStampDataComplex(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL, DungeonTileStampData.StampSpace.BOTH_SPACES, DungeonTileStampData.StampCategory.STRUCTURAL, roomHandler.opulence, r.RoomVisualSubtype, num3);
					}
					else
					{
						stampDataBase = d.stampData.GetStampDataComplex(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL, DungeonTileStampData.StampSpace.OBJECT_SPACE, DungeonTileStampData.StampCategory.MUNDANE, roomHandler.opulence, r.RoomVisualSubtype, num3);
						intVector2 = IntVector2.Up;
					}
					IntVector2 intVector3 = intVector + IntVector2.Down + IntVector2.Left * (stampDataBase.width + num4) + intVector2;
					IntVector2 intVector4 = intVector + IntVector2.Down + IntVector2.Right * (usedExit.referencedExit.ExitCellCount + num4) + intVector2;
					if (stampDataBase is TileStampData)
					{
						this.m_assembler.ApplyTileStamp(intVector3.x, intVector3.y, stampDataBase as TileStampData, d, map, -1);
						this.m_assembler.ApplyTileStamp(intVector4.x, intVector4.y, stampDataBase as TileStampData, d, map, -1);
					}
					else if (stampDataBase is SpriteStampData)
					{
						this.m_assembler.ApplySpriteStamp(intVector3.x, intVector3.y, stampDataBase as SpriteStampData, d, map);
						this.m_assembler.ApplySpriteStamp(intVector4.x, intVector4.y, stampDataBase as SpriteStampData, d, map);
					}
					else if (stampDataBase is ObjectStampData)
					{
						Debug.Log("object instantiate");
						TK2DDungeonAssembler.ApplyObjectStamp(intVector3.x, intVector3.y, stampDataBase as ObjectStampData, d, map, false, false);
						TK2DDungeonAssembler.ApplyObjectStamp(intVector4.x, intVector4.y, stampDataBase as ObjectStampData, d, map, false, false);
					}
					num3 -= stampDataBase.width;
					num4 += stampDataBase.width;
					if (num3 <= 0)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001F5428 File Offset: 0x001F3628
	public static void PlaceLightDecorationForCell(Dungeon d, tk2dTileMap map, CellData currentCell, IntVector2 currentPosition)
	{
		if (currentCell.cellVisualData.containsLight && currentCell.cellVisualData.lightDirection != DungeonData.Direction.SOUTH && currentCell.cellVisualData.lightDirection != (DungeonData.Direction)(-1))
		{
			DungeonTileStampData.StampPlacementRule stampPlacementRule = DungeonTileStampData.StampPlacementRule.ON_LOWER_FACEWALL;
			bool flag = false;
			if (currentCell.cellVisualData.lightDirection == DungeonData.Direction.EAST)
			{
				stampPlacementRule = DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS;
				flag = true;
			}
			else if (currentCell.cellVisualData.lightDirection == DungeonData.Direction.WEST)
			{
				stampPlacementRule = DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS;
			}
			LightStampData lightStampData = ((stampPlacementRule != DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS) ? currentCell.cellVisualData.facewallLightStampData : currentCell.cellVisualData.sidewallLightStampData);
			if (lightStampData != null)
			{
				GameObject gameObject = TK2DDungeonAssembler.ApplyObjectStamp(currentPosition.x, currentPosition.y, lightStampData, d, map, flag, true);
				if (gameObject)
				{
					TorchController component = gameObject.GetComponent<TorchController>();
					if (component)
					{
						component.Cell = currentCell;
					}
				}
				else if (currentCell.cellVisualData.lightObject != null)
				{
					ShadowSystem componentInChildren = currentCell.cellVisualData.lightObject.GetComponentInChildren<ShadowSystem>();
					if (componentInChildren)
					{
						for (int i = 0; i < componentInChildren.PersonalCookies.Count; i++)
						{
							componentInChildren.PersonalCookies[i].enabled = false;
							componentInChildren.PersonalCookies.RemoveAt(i);
							i--;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x001F5584 File Offset: 0x001F3784
	public void PlaceLightDecoration(Dungeon d, tk2dTileMap map)
	{
		for (int i = 0; i < d.data.Width; i++)
		{
			for (int j = 1; j < d.data.Height; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				CellData cellData = d.data[intVector];
				if (cellData != null)
				{
					TK2DInteriorDecorator.PlaceLightDecorationForCell(d, map, cellData, intVector);
				}
			}
		}
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x001F55F4 File Offset: 0x001F37F4
	protected bool IsValidPondCell(CellData cell, RoomHandler parentRoom, Dungeon d)
	{
		return cell != null && (parentRoom.ContainsPosition(cell.position) && cell.type == CellType.FLOOR && !cell.doesDamage && !cell.HasNonTopWallWallNeighbor() && !cell.HasPitNeighbor(d.data) && !cell.isOccupied && !cell.cellVisualData.floorTileOverridden && cell.cellVisualData.roomVisualTypeIndex == parentRoom.RoomVisualSubtype);
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001F5684 File Offset: 0x001F3884
	protected bool IsValidChannelCell(CellData cell, RoomHandler parentRoom, Dungeon d)
	{
		return cell != null && (parentRoom.ContainsPosition(cell.position) && cell.type == CellType.FLOOR && !cell.doesDamage && !cell.HasPitNeighbor(d.data) && !cell.isOccupied && !cell.cellVisualData.floorTileOverridden && cell.cellVisualData.roomVisualTypeIndex == parentRoom.RoomVisualSubtype);
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001F5708 File Offset: 0x001F3908
	public void DigChannels(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		if (!d.roomMaterialDefinitions[r.RoomVisualSubtype].supportsChannels)
		{
			return;
		}
		if (d.roomMaterialDefinitions[r.RoomVisualSubtype].channelGrids.Length == 0)
		{
			return;
		}
		DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[r.RoomVisualSubtype];
		TileIndexGrid tileIndexGrid = dungeonMaterial.channelGrids[UnityEngine.Random.Range(0, d.roomMaterialDefinitions[r.RoomVisualSubtype].channelGrids.Length)];
		if (tileIndexGrid == null)
		{
			return;
		}
		int num = UnityEngine.Random.Range(dungeonMaterial.minChannelPools, dungeonMaterial.maxChannelPools);
		List<IntVector2> list = new List<IntVector2>();
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = 0; i < num; i++)
		{
			HashSet<IntVector2> hashSet2 = new HashSet<IntVector2>();
			int num2 = UnityEngine.Random.Range(2, 5);
			int num3 = UnityEngine.Random.Range(2, 5);
			int num4 = UnityEngine.Random.Range(0, r.area.dimensions.x - num2);
			int num5 = UnityEngine.Random.Range(0, r.area.dimensions.y - num3);
			IntVector2 intVector = r.area.basePosition + new IntVector2(num4 + num2 / 2, num5 + num3 / 2);
			bool flag = false;
			if (num4 >= 0 && num5 >= 0)
			{
				for (int j = num4; j < num4 + num2; j++)
				{
					for (int k = num5; k < num5 + num3; k++)
					{
						IntVector2 intVector2 = r.area.basePosition + new IntVector2(j, k);
						CellData cellData = d.data[intVector2];
						if (!this.IsValidPondCell(cellData, r, d))
						{
							flag = true;
							goto IL_1A3;
						}
						hashSet2.Add(intVector2);
					}
				}
			}
			IL_1A3:
			if (!flag && hashSet2.Count > 5)
			{
				list.Add(intVector);
				foreach (IntVector2 intVector3 in hashSet2)
				{
					hashSet.Add(intVector3);
				}
			}
			else if (UnityEngine.Random.value < dungeonMaterial.channelTenacity)
			{
				i--;
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			int num6 = UnityEngine.Random.Range(1, 4);
			for (int m = 0; m < num6; m++)
			{
				HashSet<IntVector2> hashSet3 = new HashSet<IntVector2>();
				IntVector2 intVector4 = list[l];
				IntVector2 intVector5 = intVector4;
				bool flag2;
				do
				{
					int num7 = UnityEngine.Random.Range(3, 8);
					List<IntVector2> list2 = new List<IntVector2>(IntVector2.Cardinals);
					IntVector2 intVector6 = list2[UnityEngine.Random.Range(0, 4)];
					list2.Remove(intVector6);
					list2.Remove(intVector6 * -1);
					flag2 = false;
					for (int n = 0; n < num7; n++)
					{
						IntVector2 intVector7 = intVector5 + intVector6;
						CellData cellData2 = d.data[intVector7];
						if (cellData2 == null || cellData2.type == CellType.WALL)
						{
							flag2 = true;
							break;
						}
						if (this.IsValidChannelCell(cellData2, r, d) && !hashSet3.Contains(intVector7))
						{
							if (list2.Count < 3)
							{
								list2 = new List<IntVector2>(IntVector2.Cardinals);
								list2.Remove(intVector6);
								list2.Remove(intVector6 * -1);
							}
							intVector5 = intVector7;
							hashSet.Add(intVector7);
							hashSet3.Add(intVector7);
						}
						else
						{
							if (list2.Count <= 1)
							{
								flag2 = true;
								break;
							}
							intVector6 = list2[UnityEngine.Random.Range(0, list2.Count)];
							list2.Remove(intVector6);
							list2.Remove(intVector6 * -1);
						}
					}
				}
				while (!flag2);
			}
		}
		IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
		foreach (IntVector2 intVector8 in hashSet)
		{
			bool[] array = new bool[8];
			int num8 = 0;
			for (int num9 = 0; num9 < array.Length; num9++)
			{
				array[num9] = !hashSet.Contains(intVector8 + cardinalsAndOrdinals[num9]);
				if (array[num9])
				{
					num8++;
				}
			}
			if (num8 == 1)
			{
				for (int num10 = 0; num10 < array.Length; num10 += 2)
				{
					if (d.data[intVector8 + cardinalsAndOrdinals[num10]].type == CellType.WALL)
					{
						array[num10] = true;
						num8++;
					}
				}
			}
			int indexGivenSides = tileIndexGrid.GetIndexGivenSides(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
			map.SetTile(intVector8.x, intVector8.y, GlobalDungeonData.patternLayerIndex, indexGivenSides);
			d.data[intVector8].cellVisualData.floorType = CellVisualData.CellFloorType.Water;
			d.data[intVector8].cellVisualData.IsChannel = true;
		}
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x001F5C78 File Offset: 0x001F3E78
	public void ProcessHardcodedUpholstery(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[r.RoomVisualSubtype];
		if (dungeonMaterial.carpetGrids.Length == 0)
		{
			return;
		}
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		TileIndexGrid tileIndexGrid = dungeonMaterial.carpetGrids[UnityEngine.Random.Range(0, dungeonMaterial.carpetGrids.Length)];
		for (int i = 0; i < r.area.dimensions.x; i++)
		{
			for (int j = 0; j < r.area.dimensions.y; j++)
			{
				IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
				CellData cellData = d.data[intVector];
				if (cellData != null)
				{
					if (cellData.type == CellType.FLOOR && cellData.parentRoom == r && cellData.cellVisualData.IsPhantomCarpet && !cellData.HasPitNeighbor(d.data))
					{
						hashSet.Add(intVector);
						BraveUtility.DrawDebugSquare(cellData.position.ToVector2(), cellData.position.ToVector2() + Vector2.one, Color.yellow, 1000f);
					}
				}
			}
		}
		this.ApplyCarpetedHashset(hashSet, tileIndexGrid, d, map);
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001F5DBC File Offset: 0x001F3FBC
	public void UpholsterRoom(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[r.RoomVisualSubtype];
		if (!dungeonMaterial.supportsUpholstery)
		{
			return;
		}
		if (dungeonMaterial.carpetGrids.Length == 0)
		{
			return;
		}
		TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[r.RoomVisualSubtype].carpetGrids[UnityEngine.Random.Range(0, d.roomMaterialDefinitions[r.RoomVisualSubtype].carpetGrids.Length)];
		if (tileIndexGrid == null)
		{
			return;
		}
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		if (dungeonMaterial.carpetIsMainFloor)
		{
			for (int i = 0; i < r.area.dimensions.x; i++)
			{
				for (int j = 0; j < r.area.dimensions.y; j++)
				{
					IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
					CellData cellData = d.data[intVector];
					if (cellData != null && cellData.type == CellType.FLOOR && cellData.parentRoom == r && !cellData.doesDamage && !cellData.cellVisualData.floorTileOverridden && cellData.cellVisualData.roomVisualTypeIndex == r.RoomVisualSubtype)
					{
						bool flag = cellData.HasWallNeighbor(true, false) || cellData.HasPitNeighbor(d.data);
						bool flag2 = cellData.HasPhantomCarpetNeighbor(true);
						if (!flag && !flag2)
						{
							hashSet.Add(intVector);
						}
					}
				}
			}
			hashSet = Carpetron.PostprocessFullRoom(hashSet);
		}
		else
		{
			bool flag3 = true;
			List<Tuple<IntVector2, IntVector2>> list = new List<Tuple<IntVector2, IntVector2>>();
			Tuple<IntVector2, IntVector2> tuple = null;
			int num = 1;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				num = 2;
			}
			while (flag3)
			{
				Tuple<IntVector2, IntVector2> tuple2 = Carpetron.MaxSubmatrix(d.data.cellData, r.area.basePosition, r.area.dimensions, false, false, false, r.RoomVisualSubtype);
				IntVector2 intVector2 = tuple2.Second + IntVector2.One - tuple2.First;
				int num2 = intVector2.x * intVector2.y;
				if (num2 < 15 || intVector2.x < 3 || intVector2.y < 3)
				{
					break;
				}
				if (tuple != null)
				{
					IntVector2 intVector3 = tuple.Second + IntVector2.One - tuple.First;
					if (intVector3 != intVector2)
					{
						num--;
						if (num <= 0)
						{
							break;
						}
					}
				}
				for (int k = tuple2.First.x; k < tuple2.Second.x + 1; k++)
				{
					for (int l = tuple2.First.y; l < tuple2.Second.y + 1; l++)
					{
						IntVector2 intVector4 = r.area.basePosition + new IntVector2(k, l);
						d.data[intVector4].cellVisualData.floorTileOverridden = true;
					}
				}
				list.Add(tuple2);
				tuple = tuple2;
			}
			if (list.Count == 1)
			{
				Tuple<IntVector2, IntVector2> tuple3 = null;
				list[0] = Carpetron.PostprocessSubmatrix(list[0], out tuple3);
				if (tuple3 != null)
				{
					list.Add(tuple3);
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				Tuple<IntVector2, IntVector2> tuple4 = list[m];
				for (int n = tuple4.First.x; n < tuple4.Second.x + 1; n++)
				{
					for (int num3 = tuple4.First.y; num3 < tuple4.Second.y + 1; num3++)
					{
						IntVector2 intVector5 = r.area.basePosition + new IntVector2(n, num3);
						hashSet.Add(intVector5);
					}
				}
			}
		}
		this.ApplyCarpetedHashset(hashSet, tileIndexGrid, d, map);
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001F61DC File Offset: 0x001F43DC
	private void ApplyCarpetedHashset(HashSet<IntVector2> cellsToEncarpet, TileIndexGrid carpetGrid, Dungeon d, tk2dTileMap map)
	{
		IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
		Dictionary<IntVector2, int> dictionary = new Dictionary<IntVector2, int>(new IntVector2EqualityComparer());
		if (carpetGrid.CenterIndicesAreStrata)
		{
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			HashSet<IntVector2> hashSet2 = new HashSet<IntVector2>();
			HashSet<IntVector2> hashSet3 = new HashSet<IntVector2>();
			foreach (IntVector2 intVector in cellsToEncarpet)
			{
				bool[] array = new bool[8];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = !cellsToEncarpet.Contains(intVector + cardinalsAndOrdinals[i]);
				}
				if (array[0] || array[1] || array[2] || array[3] || array[4] || array[5] || array[6] || array[7])
				{
					hashSet2.Add(intVector);
				}
			}
			int num = 0;
			while (hashSet2.Count > 0)
			{
				foreach (IntVector2 intVector2 in hashSet2)
				{
					hashSet.Add(intVector2);
					for (int j = 0; j < 8; j++)
					{
						IntVector2 intVector3 = intVector2 + cardinalsAndOrdinals[j];
						if (!hashSet.Contains(intVector3) && !hashSet2.Contains(intVector3) && !hashSet3.Contains(intVector3) && cellsToEncarpet.Contains(intVector3))
						{
							hashSet3.Add(intVector3);
							dictionary.Add(intVector3, carpetGrid.centerIndices.indices[Mathf.Clamp(num, 0, carpetGrid.centerIndices.indices.Count - 1)]);
						}
					}
				}
				hashSet2 = hashSet3;
				hashSet3 = new HashSet<IntVector2>();
				num++;
			}
			if (num < 3)
			{
				dictionary.Clear();
			}
		}
		foreach (IntVector2 intVector4 in cellsToEncarpet)
		{
			bool[] array2 = new bool[8];
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = !cellsToEncarpet.Contains(intVector4 + cardinalsAndOrdinals[k]);
			}
			bool flag = !array2[0] && !array2[1] && !array2[2] && !array2[3] && !array2[4] && !array2[5] && !array2[6] && !array2[7];
			int num2;
			if (dictionary.ContainsKey(intVector4))
			{
				num2 = dictionary[intVector4];
			}
			else if (flag && carpetGrid.CenterIndicesAreStrata)
			{
				num2 = carpetGrid.centerIndices.indices[0];
			}
			else
			{
				num2 = carpetGrid.GetIndexGivenSides(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6], array2[7]);
			}
			map.SetTile(intVector4.x, intVector4.y, GlobalDungeonData.patternLayerIndex, num2);
			d.data[intVector4].cellVisualData.floorType = CellVisualData.CellFloorType.Carpet;
			d.data[intVector4].cellVisualData.floorTileOverridden = true;
		}
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x001F65C4 File Offset: 0x001F47C4
	public void HandleRoomDecorationMinimal(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		this.roomUsedStamps.Clear();
		if (r.area.prototypeRoom == null)
		{
			return;
		}
		if (this.viableCategorySets == null)
		{
			this.BuildStampLookupTable(d);
			this.BuildValidPlacementLists();
		}
		this.ProcessHardcodedUpholstery(r, d, map);
		this.roomUsedStamps.Clear();
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x001F6620 File Offset: 0x001F4820
	public void HandleRoomDecoration(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		this.roomUsedStamps.Clear();
		this.ProcessHardcodedUpholstery(r, d, map);
		if (r.area.prototypeRoom == null || !r.area.prototypeRoom.preventAddedDecoLayering)
		{
			this.UpholsterRoom(r, d, map);
			if (!r.ForcePreventChannels)
			{
				this.DigChannels(r, d, map);
			}
		}
		if (this.viableCategorySets == null)
		{
			this.BuildStampLookupTable(d);
			this.BuildValidPlacementLists();
		}
		for (int i = 0; i < r.area.instanceUsedExits.Count; i++)
		{
			PrototypeRoomExit prototypeRoomExit = r.area.instanceUsedExits[i];
			RoomHandler roomHandler = r.connectedRoomsByExit[prototypeRoomExit];
			if (roomHandler != null && (!(roomHandler.area.prototypeRoom != null) || roomHandler.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.SECRET))
			{
				this.DecorateRoomExit(r, r.area.exitToLocalDataMap[prototypeRoomExit], d, map);
			}
		}
		List<TK2DInteriorDecorator.WallExpanse> list = r.GatherExpanses(DungeonData.Direction.NORTH, false, false, false);
		for (int j = 0; j < list.Count; j++)
		{
			TK2DInteriorDecorator.WallExpanse wallExpanse = list[j];
			TK2DInteriorDecorator.WallExpanse? wallExpanse2 = null;
			int num = -1;
			for (int k = j + 1; k < list.Count; k++)
			{
				TK2DInteriorDecorator.WallExpanse wallExpanse3 = list[k];
				if (wallExpanse.basePosition.y == wallExpanse3.basePosition.y && wallExpanse.width == wallExpanse3.width)
				{
					bool flag = true;
					for (int l = 0; l < wallExpanse3.width; l++)
					{
						if (d.data[r.area.basePosition + wallExpanse.basePosition + IntVector2.Up + IntVector2.Right * l].cellVisualData.forcedMatchingStyle != d.data[r.area.basePosition + wallExpanse3.basePosition + IntVector2.Up + IntVector2.Right * l].cellVisualData.forcedMatchingStyle)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						wallExpanse2 = new TK2DInteriorDecorator.WallExpanse?(wallExpanse3);
						num = k;
					}
				}
			}
			if (wallExpanse2 != null)
			{
				wallExpanse.hasMirror = true;
				wallExpanse.mirroredExpanseBasePosition = wallExpanse2.Value.basePosition;
				wallExpanse.mirroredExpanseWidth = wallExpanse2.Value.width;
				list.RemoveAt(num);
				list[j] = wallExpanse;
			}
		}
		this.wallPlacementOffsets = new Dictionary<DungeonTileStampData.StampPlacementRule, IntVector2>();
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_LEFT_CORNER, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_RIGHT_CORNER, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ON_LOWER_FACEWALL, IntVector2.Up);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ON_UPPER_FACEWALL, IntVector2.Up * 2);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ON_ANY_FLOOR, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ON_ANY_CEILING, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS, IntVector2.Left);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS, IntVector2.Zero);
		this.wallPlacementOffsets.Add(DungeonTileStampData.StampPlacementRule.ON_TOPWALL, IntVector2.Zero);
		for (int m = 0; m < list.Count; m++)
		{
			this.expanseUsedStamps.Clear();
			TK2DInteriorDecorator.WallExpanse wallExpanse4 = list[m];
			if (wallExpanse4.hasMirror)
			{
				this.DecorateExpanseRandom(wallExpanse4, r, d, map);
			}
			else if (wallExpanse4.width > 2)
			{
				float num2 = UnityEngine.Random.value;
				for (int n = 0; n < wallExpanse4.width; n++)
				{
					if (d.data[r.area.basePosition + wallExpanse4.basePosition + IntVector2.Up + IntVector2.Right * n].cellVisualData.forcedMatchingStyle != DungeonTileStampData.IntermediaryMatchingStyle.ANY)
					{
						num2 = 1000f;
					}
				}
				if (num2 < d.stampData.SymmetricFrameChance)
				{
					this.DecorateExpanseSymmetricFrame(1, wallExpanse4, r, d, map);
				}
				else if (num2 >= d.stampData.SymmetricFrameChance && num2 < d.stampData.SymmetricFrameChance + d.stampData.SymmetricCompleteChance)
				{
					this.DecorateExpanseSymmetric(wallExpanse4, r, d, map);
				}
				else
				{
					this.DecorateExpanseRandom(wallExpanse4, r, d, map);
				}
			}
			else
			{
				this.DecorateExpanseRandom(wallExpanse4, r, d, map);
			}
		}
		this.DecorateCeilingCorners(r, d, map);
		List<TK2DInteriorDecorator.WallExpanse> list2 = r.GatherExpanses(DungeonData.Direction.EAST, false, false, false);
		List<TK2DInteriorDecorator.WallExpanse> list3 = r.GatherExpanses(DungeonData.Direction.WEST, false, false, false);
		for (int num3 = 0; num3 < list2.Count; num3++)
		{
			TK2DInteriorDecorator.WallExpanse wallExpanse5 = list2[num3];
			if (wallExpanse5.width > 1)
			{
				wallExpanse5.width--;
				list2[num3] = wallExpanse5;
			}
			else
			{
				list2.RemoveAt(num3);
				num3--;
			}
		}
		for (int num4 = 0; num4 < list3.Count; num4++)
		{
			TK2DInteriorDecorator.WallExpanse wallExpanse6 = list3[num4];
			if (wallExpanse6.width > 1)
			{
				wallExpanse6.width--;
				list3[num4] = wallExpanse6;
			}
			else
			{
				list3.RemoveAt(num4);
				num4--;
			}
		}
		int num5 = 0;
		while (num5 < list2.Count)
		{
			this.expanseUsedStamps.Clear();
			TK2DInteriorDecorator.WallExpanse wallExpanse7 = list2[num5];
			TK2DInteriorDecorator.WallExpanse? wallExpanse8 = null;
			for (int num6 = 0; num6 < list3.Count; num6++)
			{
				TK2DInteriorDecorator.WallExpanse wallExpanse9 = list3[num6];
				if (wallExpanse9.basePosition.y == wallExpanse7.basePosition.y && wallExpanse9.width == wallExpanse7.width)
				{
					wallExpanse8 = new TK2DInteriorDecorator.WallExpanse?(wallExpanse9);
					list3.RemoveAt(num6);
					break;
				}
			}
			int num7 = 1;
			for (;;)
			{
				int num8 = wallExpanse7.width - num7;
				if (num8 == 0)
				{
					break;
				}
				IntVector2 intVector = r.area.basePosition + wallExpanse7.basePosition + num7 * IntVector2.Up;
				StampDataBase stampDataBase = null;
				TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode = this.DecorateWallSection(intVector, num8, r, d, map, this.validEasternPlacements, wallExpanse7, out stampDataBase, Mathf.Max(0.55f, r.RoomMaterial.stampFailChance), true);
				if (decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
				{
					break;
				}
				if (stampDataBase == null || decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
				{
					num7++;
				}
				else
				{
					if (wallExpanse8 != null)
					{
						IntVector2 intVector2 = r.area.basePosition + wallExpanse8.Value.basePosition + (wallExpanse7.width - num8) * IntVector2.Up;
						StampDataBase stampDataBase2 = null;
						this.DecorateWallSection(intVector2, num8, r, d, map, this.validWesternPlacements, wallExpanse8.Value, out stampDataBase2, 0f, true);
					}
					num7 += stampDataBase.height;
				}
			}
			IL_75E:
			num5++;
			continue;
			goto IL_75E;
		}
		int num9 = 0;
		while (num9 < list3.Count)
		{
			this.expanseUsedStamps.Clear();
			TK2DInteriorDecorator.WallExpanse wallExpanse10 = list3[num9];
			int num10 = 1;
			for (;;)
			{
				int num11 = wallExpanse10.width - num10;
				if (num11 == 0)
				{
					break;
				}
				IntVector2 intVector3 = r.area.basePosition + wallExpanse10.basePosition + num10 * IntVector2.Up;
				StampDataBase stampDataBase3 = null;
				TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode2 = this.DecorateWallSection(intVector3, num11, r, d, map, this.validWesternPlacements, wallExpanse10, out stampDataBase3, Mathf.Max(0.55f, r.RoomMaterial.stampFailChance), true);
				if (decorateErrorCode2 == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
				{
					break;
				}
				if (stampDataBase3 == null || decorateErrorCode2 == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
				{
					num10++;
				}
				else
				{
					num10 += stampDataBase3.height;
				}
			}
			IL_83F:
			num9++;
			continue;
			goto IL_83F;
		}
		List<TK2DInteriorDecorator.WallExpanse> list4 = r.GatherExpanses(DungeonData.Direction.SOUTH, true, false, false);
		int num12 = 0;
		while (num12 < list4.Count)
		{
			this.expanseUsedStamps.Clear();
			TK2DInteriorDecorator.WallExpanse wallExpanse11 = list4[num12];
			int num13 = 1;
			for (;;)
			{
				int num14 = Mathf.FloorToInt((float)(wallExpanse11.width - 2 * num13) / 2f);
				if (num14 == 0)
				{
					break;
				}
				IntVector2 intVector4 = r.area.basePosition + wallExpanse11.basePosition + num13 * IntVector2.Right;
				StampDataBase stampDataBase4 = null;
				TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode3 = this.DecorateWallSection(intVector4, num14, r, d, map, this.validSouthernPlacements, wallExpanse11, out stampDataBase4, 0.5f, false);
				if (decorateErrorCode3 == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
				{
					break;
				}
				if (stampDataBase4 == null || decorateErrorCode3 == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
				{
					num13++;
				}
				else
				{
					IntVector2 intVector5 = r.area.basePosition + wallExpanse11.basePosition + (wallExpanse11.width - num13 - stampDataBase4.width) * IntVector2.Right + this.wallPlacementOffsets[stampDataBase4.placementRule];
					this.m_assembler.ApplyStampGeneric(intVector5.x, intVector5.y, stampDataBase4, d, map, false, GlobalDungeonData.aboveBorderLayerIndex);
					num13 += stampDataBase4.width;
					if (stampDataBase4.width == 1)
					{
						num13 += 2;
					}
				}
			}
			IL_9B1:
			num12++;
			continue;
			goto IL_9B1;
		}
		for (int num15 = 2; num15 < r.area.dimensions.x - 2; num15++)
		{
			for (int num16 = 2; num16 < r.area.dimensions.y - 2; num16++)
			{
				IntVector2 intVector6 = r.area.basePosition + new IntVector2(num15, num16);
				CellData cellData = d.data.cellData[intVector6.x][intVector6.y];
				if (cellData != null)
				{
					if (cellData.type == CellType.FLOOR)
					{
						if (!cellData.cellVisualData.floorTileOverridden)
						{
							if (!cellData.cellVisualData.preventFloorStamping)
							{
								StampDataBase stampDataBase5 = null;
								float num17 = 0.8f;
								if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
								{
									num17 = 0.99f;
								}
								this.DecorateFloorSquare(intVector6, r, d, map, out stampDataBase5, num17);
							}
						}
					}
				}
			}
		}
		this.roomUsedStamps.Clear();
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x001F7104 File Offset: 0x001F5304
	private void DecorateCeilingCorners(RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		if (d.tileIndices.edgeDecorationTiles == null)
		{
			return;
		}
		if (r == d.data.Entrance)
		{
			return;
		}
		if (r == d.data.Exit)
		{
			return;
		}
		CellArea area = r.area;
		for (int i = 0; i < area.dimensions.x; i++)
		{
			for (int j = 0; j < area.dimensions.y; j++)
			{
				IntVector2 intVector = area.basePosition + new IntVector2(i, j);
				CellData cellData = d.data.cellData[intVector.x][intVector.y];
				if (cellData != null && cellData.type != CellType.WALL)
				{
					if (cellData.diagonalWallType == DiagonalWallType.NONE)
					{
						List<CellData> cellNeighbors = d.data.GetCellNeighbors(cellData, false);
						bool flag = cellNeighbors[0] != null && cellNeighbors[0].type == CellType.WALL && cellNeighbors[0].diagonalWallType == DiagonalWallType.NONE;
						bool flag2 = cellNeighbors[1] != null && cellNeighbors[1].type == CellType.WALL && cellNeighbors[1].diagonalWallType == DiagonalWallType.NONE;
						bool flag3 = cellNeighbors[2] != null && cellNeighbors[2].type == CellType.WALL && cellNeighbors[2].diagonalWallType == DiagonalWallType.NONE;
						bool flag4 = cellNeighbors[3] != null && cellNeighbors[3].type == CellType.WALL && cellNeighbors[3].diagonalWallType == DiagonalWallType.NONE;
						int indexGivenSides = d.tileIndices.edgeDecorationTiles.GetIndexGivenSides(flag, flag2, flag3, flag4);
						bool flag5 = UnityEngine.Random.value < 0.25f;
						if (indexGivenSides != -1 && flag5)
						{
							int num = ((!flag) ? 1 : 2);
							map.SetTile(intVector.x, intVector.y + num, GlobalDungeonData.aboveBorderLayerIndex, indexGivenSides);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001F7330 File Offset: 0x001F5530
	private void DecorateExpanseSymmetricFrame(int frameIterations, TK2DInteriorDecorator.WallExpanse expanse, RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		int num = 0;
		for (int i = 0; i < frameIterations; i++)
		{
			int num2 = Mathf.FloorToInt((float)(expanse.width - 2 * num) / 2f);
			if (num2 == 0)
			{
				break;
			}
			IntVector2 intVector = r.area.basePosition + expanse.basePosition + num * IntVector2.Right;
			StampDataBase stampDataBase = null;
			TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode = this.DecorateWallSection(intVector, num2, r, d, map, this.validNorthernPlacements, expanse, out stampDataBase, r.RoomMaterial.stampFailChance, false);
			if (decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
			{
				break;
			}
			if (stampDataBase == null || decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
			{
				num++;
			}
			else
			{
				if (stampDataBase.indexOfSymmetricPartner != -1)
				{
					stampDataBase = d.stampData.objectStamps[stampDataBase.indexOfSymmetricPartner];
				}
				IntVector2 intVector2 = r.area.basePosition + expanse.basePosition + (expanse.width - num - stampDataBase.width) * IntVector2.Right + this.wallPlacementOffsets[stampDataBase.placementRule];
				if (!stampDataBase.preventRoomRepeats)
				{
					this.m_assembler.ApplyStampGeneric(intVector2.x, intVector2.y, stampDataBase, d, map, false, -1);
				}
				else
				{
					StampDataBase stampDataBase2 = d.stampData.AttemptGetSimilarStampForRoomDuplication(stampDataBase, this.roomUsedStamps, r.RoomVisualSubtype);
					if (stampDataBase2 != null)
					{
						this.m_assembler.ApplyStampGeneric(intVector2.x, intVector2.y, stampDataBase2, d, map, false, -1);
						this.roomUsedStamps.Add(stampDataBase2);
					}
				}
				if (this.DEBUG_DRAW)
				{
					BraveUtility.DrawDebugSquare(intVector.ToVector2(), (intVector + IntVector2.Up + stampDataBase.width * IntVector2.Right).ToVector2(), Color.red, 1000f);
					BraveUtility.DrawDebugSquare(intVector2.ToVector2(), (intVector2 + IntVector2.Up + stampDataBase.width * IntVector2.Right).ToVector2(), Color.red, 1000f);
				}
				num += stampDataBase.width;
			}
		}
		int num3 = expanse.width - 2 * num;
		if (num3 > 0)
		{
			TK2DInteriorDecorator.WallExpanse wallExpanse = new TK2DInteriorDecorator.WallExpanse(expanse.basePosition + num * IntVector2.Right, num3);
			this.DecorateExpanseRandom(wallExpanse, r, d, map);
		}
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x001F75B8 File Offset: 0x001F57B8
	private void DecorateExpanseSymmetric(TK2DInteriorDecorator.WallExpanse expanse, RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		int num = 0;
		for (;;)
		{
			int num2 = Mathf.FloorToInt((float)(expanse.width - 2 * num) / 2f);
			if (num2 == 0)
			{
				break;
			}
			IntVector2 intVector = r.area.basePosition + expanse.basePosition + num * IntVector2.Right;
			StampDataBase stampDataBase = null;
			TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode = this.DecorateWallSection(intVector, num2, r, d, map, this.validNorthernPlacements, expanse, out stampDataBase, r.RoomMaterial.stampFailChance, false);
			if (decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
			{
				break;
			}
			if (stampDataBase == null || decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
			{
				num++;
			}
			else
			{
				if (stampDataBase.indexOfSymmetricPartner != -1)
				{
					stampDataBase = d.stampData.objectStamps[stampDataBase.indexOfSymmetricPartner];
				}
				IntVector2 intVector2 = r.area.basePosition + expanse.basePosition + (expanse.width - num - stampDataBase.width) * IntVector2.Right + this.wallPlacementOffsets[stampDataBase.placementRule];
				if (!stampDataBase.preventRoomRepeats)
				{
					this.m_assembler.ApplyStampGeneric(intVector2.x, intVector2.y, stampDataBase, d, map, false, -1);
				}
				else
				{
					StampDataBase stampDataBase2 = d.stampData.AttemptGetSimilarStampForRoomDuplication(stampDataBase, this.roomUsedStamps, r.RoomVisualSubtype);
					if (stampDataBase2 != null)
					{
						this.m_assembler.ApplyStampGeneric(intVector2.x, intVector2.y, stampDataBase2, d, map, false, -1);
						this.roomUsedStamps.Add(stampDataBase2);
					}
				}
				if (this.DEBUG_DRAW)
				{
					BraveUtility.DrawDebugSquare(intVector.ToVector2(), (intVector + IntVector2.Up + stampDataBase.width * IntVector2.Right).ToVector2(), Color.yellow, 1000f);
					BraveUtility.DrawDebugSquare(intVector2.ToVector2(), (intVector2 + IntVector2.Up + stampDataBase.width * IntVector2.Right).ToVector2(), Color.yellow, 1000f);
				}
				num += stampDataBase.width;
			}
		}
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x001F77E0 File Offset: 0x001F59E0
	private void DecorateExpanseRandom(TK2DInteriorDecorator.WallExpanse expanse, RoomHandler r, Dungeon d, tk2dTileMap map)
	{
		int num = 0;
		for (;;)
		{
			int num2 = expanse.width - num;
			if (num2 == 0)
			{
				break;
			}
			IntVector2 intVector = r.area.basePosition + expanse.basePosition + num * IntVector2.Right;
			StampDataBase stampDataBase = null;
			TK2DInteriorDecorator.DecorateErrorCode decorateErrorCode = this.DecorateWallSection(intVector, num2, r, d, map, this.validNorthernPlacements, expanse, out stampDataBase, r.RoomMaterial.stampFailChance, false);
			if (decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE)
			{
				break;
			}
			if (stampDataBase == null || decorateErrorCode == TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE)
			{
				num++;
			}
			else
			{
				if (expanse.hasMirror)
				{
					IntVector2 intVector2 = r.area.basePosition + expanse.GetPositionInMirroredExpanse(num, stampDataBase.width);
					Debug.DrawLine(intVector2.ToVector3(), intVector2.ToVector3() + new Vector3(1f, 1f, 0f), Color.cyan, 1000f);
					if (stampDataBase.indexOfSymmetricPartner != -1)
					{
						stampDataBase = d.stampData.objectStamps[stampDataBase.indexOfSymmetricPartner];
					}
					IntVector2 intVector3 = intVector2 + this.wallPlacementOffsets[stampDataBase.placementRule];
					if (!stampDataBase.preventRoomRepeats)
					{
						this.m_assembler.ApplyStampGeneric(intVector3.x, intVector3.y, stampDataBase, d, map, false, -1);
					}
					else
					{
						StampDataBase stampDataBase2 = d.stampData.AttemptGetSimilarStampForRoomDuplication(stampDataBase, this.roomUsedStamps, r.RoomVisualSubtype);
						if (stampDataBase2 != null)
						{
							this.m_assembler.ApplyStampGeneric(intVector3.x, intVector3.y, stampDataBase2, d, map, false, -1);
							this.roomUsedStamps.Add(stampDataBase2);
						}
					}
				}
				if (this.DEBUG_DRAW)
				{
					BraveUtility.DrawDebugSquare(intVector.ToVector2(), (intVector + IntVector2.Up + stampDataBase.width * IntVector2.Right).ToVector2(), Color.magenta, 1000f);
				}
				num += stampDataBase.width;
			}
		}
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x001F79E8 File Offset: 0x001F5BE8
	private DungeonTileStampData.StampSpace GetValidSpaceForSection(IntVector2 basePosition, int viableWidth, Dungeon d)
	{
		List<DungeonTileStampData.StampSpace> list = new List<DungeonTileStampData.StampSpace>();
		list.Add(DungeonTileStampData.StampSpace.OBJECT_SPACE);
		bool flag = true;
		for (int i = 0; i < viableWidth; i++)
		{
			IntVector2 intVector = basePosition + IntVector2.Up + IntVector2.Right * i;
			CellVisualData cellVisualData = d.data.cellData[intVector.x][intVector.y].cellVisualData;
			if (cellVisualData.containsWallSpaceStamp)
			{
				flag = false;
				break;
			}
			intVector += IntVector2.Up;
			cellVisualData = d.data.cellData[intVector.x][intVector.y].cellVisualData;
			if (cellVisualData.containsWallSpaceStamp)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			list.Add(DungeonTileStampData.StampSpace.WALL_SPACE);
			list.Add(DungeonTileStampData.StampSpace.BOTH_SPACES);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x001F7AD0 File Offset: 0x001F5CD0
	private void BuildValidPlacementLists()
	{
		this.validNorthernPlacements = new List<DungeonTileStampData.StampPlacementRule>();
		this.validNorthernPlacements.Add(DungeonTileStampData.StampPlacementRule.ABOVE_UPPER_FACEWALL);
		this.validNorthernPlacements.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL);
		this.validNorthernPlacements.Add(DungeonTileStampData.StampPlacementRule.ON_LOWER_FACEWALL);
		this.validNorthernPlacements.Add(DungeonTileStampData.StampPlacementRule.ON_UPPER_FACEWALL);
		this.validEasternPlacements = new List<DungeonTileStampData.StampPlacementRule>();
		this.validEasternPlacements.Add(DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS);
		this.validEasternPlacements.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL);
		this.validWesternPlacements = new List<DungeonTileStampData.StampPlacementRule>();
		this.validWesternPlacements.Add(DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS);
		this.validWesternPlacements.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL);
		this.validSouthernPlacements = new List<DungeonTileStampData.StampPlacementRule>();
		this.validSouthernPlacements.Add(DungeonTileStampData.StampPlacementRule.ON_TOPWALL);
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x001F7B78 File Offset: 0x001F5D78
	private void BuildStampLookupTable(Dungeon d)
	{
		this.viableCategorySets = new List<TK2DInteriorDecorator.ViableStampCategorySet>();
		for (int i = 0; i < d.stampData.stamps.Length; i++)
		{
			StampDataBase stampDataBase = d.stampData.stamps[i];
			TK2DInteriorDecorator.ViableStampCategorySet viableStampCategorySet = new TK2DInteriorDecorator.ViableStampCategorySet(stampDataBase.stampCategory, stampDataBase.placementRule, stampDataBase.occupySpace);
			if (!this.viableCategorySets.Contains(viableStampCategorySet))
			{
				this.viableCategorySets.Add(viableStampCategorySet);
			}
		}
		for (int j = 0; j < d.stampData.spriteStamps.Length; j++)
		{
			StampDataBase stampDataBase2 = d.stampData.spriteStamps[j];
			TK2DInteriorDecorator.ViableStampCategorySet viableStampCategorySet2 = new TK2DInteriorDecorator.ViableStampCategorySet(stampDataBase2.stampCategory, stampDataBase2.placementRule, stampDataBase2.occupySpace);
			if (!this.viableCategorySets.Contains(viableStampCategorySet2))
			{
				this.viableCategorySets.Add(viableStampCategorySet2);
			}
		}
		for (int k = 0; k < d.stampData.objectStamps.Length; k++)
		{
			StampDataBase stampDataBase3 = d.stampData.objectStamps[k];
			TK2DInteriorDecorator.ViableStampCategorySet viableStampCategorySet3 = new TK2DInteriorDecorator.ViableStampCategorySet(stampDataBase3.stampCategory, stampDataBase3.placementRule, stampDataBase3.occupySpace);
			if (!this.viableCategorySets.Contains(viableStampCategorySet3))
			{
				this.viableCategorySets.Add(viableStampCategorySet3);
			}
		}
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x001F7CC8 File Offset: 0x001F5EC8
	private TK2DInteriorDecorator.ViableStampCategorySet GetCategorySet(List<DungeonTileStampData.StampPlacementRule> validRules)
	{
		List<TK2DInteriorDecorator.ViableStampCategorySet> list = new List<TK2DInteriorDecorator.ViableStampCategorySet>();
		for (int i = 0; i < this.viableCategorySets.Count; i++)
		{
			if (validRules.Contains(this.viableCategorySets[i].placement))
			{
				list.Add(this.viableCategorySets[i]);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x001F7D44 File Offset: 0x001F5F44
	private bool CheckExpanseStampValidity(TK2DInteriorDecorator.WallExpanse expanse, StampDataBase stamp)
	{
		if (stamp.preventRoomRepeats && this.roomUsedStamps.Contains(stamp))
		{
			return false;
		}
		int preferredIntermediaryStamps = stamp.preferredIntermediaryStamps;
		for (int i = 0; i < preferredIntermediaryStamps; i++)
		{
			int num = this.expanseUsedStamps.Count - (1 + i);
			if (num < 0)
			{
				break;
			}
			if (stamp.intermediaryMatchingStyle == DungeonTileStampData.IntermediaryMatchingStyle.ANY)
			{
				if (this.expanseUsedStamps[num] == stamp)
				{
					return false;
				}
			}
			else if (this.expanseUsedStamps[num].intermediaryMatchingStyle == stamp.intermediaryMatchingStyle)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x001F7DE8 File Offset: 0x001F5FE8
	private bool DecorateFloorSquare(IntVector2 basePosition, RoomHandler r, Dungeon d, tk2dTileMap map, out StampDataBase placedStamp, float failChance = 0.2f)
	{
		if (UnityEngine.Random.value < failChance)
		{
			placedStamp = null;
			return true;
		}
		placedStamp = null;
		List<DungeonTileStampData.StampPlacementRule> list = new List<DungeonTileStampData.StampPlacementRule>();
		list.Add(DungeonTileStampData.StampPlacementRule.ON_ANY_FLOOR);
		TK2DInteriorDecorator.ViableStampCategorySet categorySet = this.GetCategorySet(list);
		if (categorySet == null)
		{
			return false;
		}
		StampDataBase stampDataComplex = d.stampData.GetStampDataComplex(list, categorySet.space, categorySet.category, r.opulence, r.RoomVisualSubtype, 1);
		if (stampDataComplex == null)
		{
			return false;
		}
		IntVector2 intVector = basePosition + this.wallPlacementOffsets[stampDataComplex.placementRule];
		this.m_assembler.ApplyStampGeneric(intVector.x, intVector.y, stampDataComplex, d, map, false, -1);
		placedStamp = stampDataComplex;
		return true;
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x001F7E94 File Offset: 0x001F6094
	private TK2DInteriorDecorator.DecorateErrorCode DecorateWallSection(IntVector2 basePosition, int viableWidth, RoomHandler r, Dungeon d, tk2dTileMap map, List<DungeonTileStampData.StampPlacementRule> validRules, TK2DInteriorDecorator.WallExpanse expanse, out StampDataBase placedStamp, float failChance = 0.2f, bool excludeWallSpace = false)
	{
		if (GameManager.Options.DebrisQuantity == GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			failChance = Mathf.Min(failChance * 2f, 0.75f);
		}
		if (UnityEngine.Random.value < failChance)
		{
			placedStamp = null;
			return TK2DInteriorDecorator.DecorateErrorCode.FAILED_CHANCE;
		}
		StampDataBase stampDataBase = null;
		if (validRules.Contains(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL))
		{
			if (d.data.GetCellTypeSafe(basePosition + IntVector2.Left) == CellType.WALL)
			{
				validRules.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_LEFT_CORNER);
			}
			if (d.data.GetCellTypeSafe(basePosition + IntVector2.Right) == CellType.WALL)
			{
				validRules.Add(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_RIGHT_CORNER);
			}
		}
		int i = 0;
		while (i < 10)
		{
			if (!d.data.CheckInBoundsAndValid(basePosition) || !d.data.CheckInBoundsAndValid(basePosition + IntVector2.Up))
			{
				stampDataBase = null;
				break;
			}
			if (d.data[basePosition + IntVector2.Up].cellVisualData.forcedMatchingStyle == DungeonTileStampData.IntermediaryMatchingStyle.ANY)
			{
				stampDataBase = d.stampData.GetStampDataSimple(validRules, r.opulence, r.RoomVisualSubtype, viableWidth, excludeWallSpace, this.roomUsedStamps);
				if (stampDataBase == null || !stampDataBase.requiresForcedMatchingStyle)
				{
					goto IL_20F;
				}
			}
			else
			{
				BraveUtility.DrawDebugSquare((basePosition + IntVector2.Up).ToVector2() + new Vector2(0.2f, 0.2f), (basePosition + IntVector2.Up + IntVector2.One).ToVector2() + new Vector2(-0.2f, -0.2f), Color.red, 1000f);
				stampDataBase = d.stampData.GetStampDataSimpleWithForcedRule(validRules, d.data[basePosition + IntVector2.Up].cellVisualData.forcedMatchingStyle, r.opulence, r.RoomVisualSubtype, viableWidth, excludeWallSpace);
				if (stampDataBase != null && stampDataBase.intermediaryMatchingStyle != d.data[basePosition + IntVector2.Up].cellVisualData.forcedMatchingStyle)
				{
					break;
				}
				goto IL_20F;
			}
			IL_247:
			i++;
			continue;
			IL_20F:
			if (stampDataBase == null)
			{
				break;
			}
			if (excludeWallSpace && stampDataBase.width > 1)
			{
				goto IL_247;
			}
			if (this.CheckExpanseStampValidity(expanse, stampDataBase))
			{
				break;
			}
			stampDataBase = null;
			goto IL_247;
		}
		validRules.Remove(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_LEFT_CORNER);
		validRules.Remove(DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL_RIGHT_CORNER);
		if (stampDataBase == null)
		{
			placedStamp = null;
			return TK2DInteriorDecorator.DecorateErrorCode.FAILED_SPACE;
		}
		this.expanseUsedStamps.Add(stampDataBase);
		this.roomUsedStamps.Add(stampDataBase);
		IntVector2 intVector = basePosition + this.wallPlacementOffsets[stampDataBase.placementRule];
		bool flag = stampDataBase.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || stampDataBase.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS || stampDataBase.placementRule == DungeonTileStampData.StampPlacementRule.ON_TOPWALL;
		int num = ((!flag) ? (-1) : GlobalDungeonData.aboveBorderLayerIndex);
		this.m_assembler.ApplyStampGeneric(intVector.x, intVector.y, stampDataBase, d, map, false, num);
		placedStamp = stampDataBase;
		return TK2DInteriorDecorator.DecorateErrorCode.ALL_OK;
	}

	// Token: 0x04004CD6 RID: 19670
	private TK2DDungeonAssembler m_assembler;

	// Token: 0x04004CD7 RID: 19671
	private Dictionary<DungeonTileStampData.StampPlacementRule, IntVector2> wallPlacementOffsets;

	// Token: 0x04004CD8 RID: 19672
	private List<TK2DInteriorDecorator.ViableStampCategorySet> viableCategorySets;

	// Token: 0x04004CD9 RID: 19673
	private List<DungeonTileStampData.StampPlacementRule> validNorthernPlacements;

	// Token: 0x04004CDA RID: 19674
	private List<DungeonTileStampData.StampPlacementRule> validEasternPlacements;

	// Token: 0x04004CDB RID: 19675
	private List<DungeonTileStampData.StampPlacementRule> validWesternPlacements;

	// Token: 0x04004CDC RID: 19676
	private List<DungeonTileStampData.StampPlacementRule> validSouthernPlacements;

	// Token: 0x04004CDD RID: 19677
	private bool DEBUG_DRAW;

	// Token: 0x04004CDE RID: 19678
	private List<StampDataBase> roomUsedStamps = new List<StampDataBase>();

	// Token: 0x04004CDF RID: 19679
	private List<StampDataBase> expanseUsedStamps = new List<StampDataBase>();

	// Token: 0x02000F44 RID: 3908
	protected class ViableStampCategorySet
	{
		// Token: 0x06005432 RID: 21554 RVA: 0x001F81A8 File Offset: 0x001F63A8
		public ViableStampCategorySet(DungeonTileStampData.StampCategory c, DungeonTileStampData.StampPlacementRule p, DungeonTileStampData.StampSpace s)
		{
			this.category = c;
			this.placement = p;
			this.space = s;
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x001F81C8 File Offset: 0x001F63C8
		public override int GetHashCode()
		{
			return 1597 * this.category.GetHashCode() + 5347 * this.placement.GetHashCode() + 13 * this.space.GetHashCode();
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x001F821C File Offset: 0x001F641C
		public override bool Equals(object obj)
		{
			if (obj is TK2DInteriorDecorator.ViableStampCategorySet)
			{
				TK2DInteriorDecorator.ViableStampCategorySet viableStampCategorySet = obj as TK2DInteriorDecorator.ViableStampCategorySet;
				return viableStampCategorySet.category == this.category && viableStampCategorySet.space == this.space && viableStampCategorySet.placement == this.placement;
			}
			return false;
		}

		// Token: 0x04004CE0 RID: 19680
		public DungeonTileStampData.StampCategory category;

		// Token: 0x04004CE1 RID: 19681
		public DungeonTileStampData.StampPlacementRule placement;

		// Token: 0x04004CE2 RID: 19682
		public DungeonTileStampData.StampSpace space;
	}

	// Token: 0x02000F45 RID: 3909
	public enum DecorateErrorCode
	{
		// Token: 0x04004CE4 RID: 19684
		ALL_OK,
		// Token: 0x04004CE5 RID: 19685
		FAILED_SPACE,
		// Token: 0x04004CE6 RID: 19686
		FAILED_CHANCE
	}

	// Token: 0x02000F46 RID: 3910
	public struct WallExpanse
	{
		// Token: 0x06005435 RID: 21557 RVA: 0x001F8270 File Offset: 0x001F6470
		public WallExpanse(IntVector2 bp, int w)
		{
			this.basePosition = bp;
			this.width = w;
			this.hasMirror = false;
			this.mirroredExpanseBasePosition = IntVector2.Zero;
			this.mirroredExpanseWidth = 0;
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x001F829C File Offset: 0x001F649C
		public IntVector2 GetPositionInMirroredExpanse(int basePlacement, int stampWidth)
		{
			IntVector2 intVector = this.mirroredExpanseBasePosition + this.mirroredExpanseWidth * IntVector2.Right;
			return intVector + (basePlacement + stampWidth) * IntVector2.Left;
		}

		// Token: 0x04004CE7 RID: 19687
		public IntVector2 basePosition;

		// Token: 0x04004CE8 RID: 19688
		public int width;

		// Token: 0x04004CE9 RID: 19689
		public bool hasMirror;

		// Token: 0x04004CEA RID: 19690
		public IntVector2 mirroredExpanseBasePosition;

		// Token: 0x04004CEB RID: 19691
		public int mirroredExpanseWidth;
	}
}
