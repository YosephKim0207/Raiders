using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000F41 RID: 3905
public class TK2DDungeonAssembler
{
	// Token: 0x060053DD RID: 21469 RVA: 0x001EC028 File Offset: 0x001EA228
	private bool HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType flagType, int roomType)
	{
		if (this.m_metadataLookupTable[flagType] == null)
		{
			return false;
		}
		List<Tuple<int, TilesetIndexMetadata>> list = this.m_metadataLookupTable[flagType];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Second.dungeonRoomSubType == roomType || list[i].Second.secondRoomSubType == roomType || list[i].Second.thirdRoomSubType == roomType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x001EC0B4 File Offset: 0x001EA2B4
	public void Initialize(TileIndices indices)
	{
		this.m_metadataLookupTable = new Dictionary<TilesetIndexMetadata.TilesetFlagType, List<Tuple<int, TilesetIndexMetadata>>>();
		TilesetIndexMetadata.TilesetFlagType[] array = (TilesetIndexMetadata.TilesetFlagType[])Enum.GetValues(typeof(TilesetIndexMetadata.TilesetFlagType));
		for (int i = 0; i < array.Length; i++)
		{
			this.m_metadataLookupTable.Add(array[i], indices.dungeonCollection.GetIndicesForTileType(array[i]));
		}
		SecretRoomUtility.metadataLookupTableRef = this.m_metadataLookupTable;
		this.t = indices;
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x001EC124 File Offset: 0x001EA324
	public bool BCheck(Dungeon d, int ix, int iy, int thresh)
	{
		return d.data.CheckInBounds(new IntVector2(ix, iy), 3 + thresh);
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x001EC144 File Offset: 0x001EA344
	public bool BCheck(Dungeon d, int ix, int iy)
	{
		return this.BCheck(d, ix, iy, 0);
	}

	// Token: 0x060053E1 RID: 21473 RVA: 0x001EC150 File Offset: 0x001EA350
	public static void RuntimeResizeTileMap(tk2dTileMap tileMap, int w, int h, int partitionSizeX, int partitionSizeY)
	{
		foreach (Layer layer in tileMap.Layers)
		{
			layer.DestroyGameData(tileMap);
			if (layer.gameObject != null)
			{
				tk2dUtil.DestroyImmediate(layer.gameObject);
				layer.gameObject = null;
			}
		}
		Layer[] array = new Layer[tileMap.Layers.Length];
		for (int j = 0; j < tileMap.Layers.Length; j++)
		{
			Layer layer2 = tileMap.Layers[j];
			array[j] = new Layer(layer2.hash, w, h, partitionSizeX, partitionSizeY);
			Layer layer3 = array[j];
			if (!layer2.IsEmpty)
			{
				int num = Mathf.Min(tileMap.height, h);
				int num2 = Mathf.Min(tileMap.width, w);
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < num2; l++)
					{
						layer3.SetRawTile(l, k, layer2.GetRawTile(l, k));
					}
				}
				layer3.Optimize();
			}
		}
		bool flag = tileMap.ColorChannel != null && !tileMap.ColorChannel.IsEmpty;
		ColorChannel colorChannel = new ColorChannel(w, h, partitionSizeX, partitionSizeY);
		if (flag)
		{
			int num3 = Mathf.Min(tileMap.height, h) + 1;
			int num4 = Mathf.Min(tileMap.width, w) + 1;
			for (int m = 0; m < num3; m++)
			{
				for (int n = 0; n < num4; n++)
				{
					colorChannel.SetColor(n, m, tileMap.ColorChannel.GetColor(n, m));
				}
			}
			colorChannel.Optimize();
		}
		tileMap.ColorChannel = colorChannel;
		tileMap.Layers = array;
		tileMap.width = w;
		tileMap.height = h;
		tileMap.partitionSizeX = partitionSizeX;
		tileMap.partitionSizeY = partitionSizeY;
		tileMap.ForceBuild();
	}

	// Token: 0x060053E2 RID: 21474 RVA: 0x001EC344 File Offset: 0x001EA544
	private StampIndexVariant GetIndexFromStampArray(CellData current, List<StampIndexVariant> list)
	{
		float num = current.UniqueHash;
		foreach (StampIndexVariant stampIndexVariant in list)
		{
			num -= stampIndexVariant.likelihood;
			if (num <= 0f)
			{
				return stampIndexVariant;
			}
		}
		return list[0];
	}

	// Token: 0x060053E3 RID: 21475 RVA: 0x001EC3C0 File Offset: 0x001EA5C0
	private TileIndexVariant GetIndexFromTileArray(CellData current, List<TileIndexVariant> list)
	{
		float uniqueHash = current.UniqueHash;
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i].likelihood;
		}
		float num2 = uniqueHash * num;
		for (int j = 0; j < list.Count; j++)
		{
			num2 -= list[j].likelihood;
			if (num2 <= 0f)
			{
				return list[j];
			}
		}
		return list[0];
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x001EC44C File Offset: 0x001EA64C
	private int GetIndexFromTupleArray(CellData current, List<Tuple<int, TilesetIndexMetadata>> list, int roomTypeIndex)
	{
		float uniqueHash = current.UniqueHash;
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Second.dungeonRoomSubType == roomTypeIndex || list[i].Second.secondRoomSubType == roomTypeIndex || list[i].Second.thirdRoomSubType == roomTypeIndex)
			{
				num += list[i].Second.weight;
			}
		}
		float num2 = uniqueHash * num;
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].Second.dungeonRoomSubType == roomTypeIndex || list[j].Second.secondRoomSubType == roomTypeIndex || list[j].Second.thirdRoomSubType == roomTypeIndex)
			{
				num2 -= list[j].Second.weight;
				if (num2 <= 0f)
				{
					return list[j].First;
				}
			}
		}
		return list[0].First;
	}

	// Token: 0x060053E5 RID: 21477 RVA: 0x001EC584 File Offset: 0x001EA784
	private TilesetIndexMetadata GetMetadataFromTupleArray(CellData current, List<Tuple<int, TilesetIndexMetadata>> list, int roomTypeIndex, out int index)
	{
		if (list == null)
		{
			index = -1;
			return null;
		}
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			Tuple<int, TilesetIndexMetadata> tuple = list[i];
			if (tuple.Second.dungeonRoomSubType == -1 || tuple.Second.dungeonRoomSubType == roomTypeIndex || tuple.Second.secondRoomSubType == roomTypeIndex || tuple.Second.thirdRoomSubType == roomTypeIndex)
			{
				num += tuple.Second.weight;
			}
		}
		float num2 = UnityEngine.Random.value * num;
		for (int j = 0; j < list.Count; j++)
		{
			Tuple<int, TilesetIndexMetadata> tuple2 = list[j];
			if (tuple2.Second.dungeonRoomSubType == -1 || tuple2.Second.dungeonRoomSubType == roomTypeIndex || tuple2.Second.secondRoomSubType == roomTypeIndex || tuple2.Second.thirdRoomSubType == roomTypeIndex)
			{
				num2 -= tuple2.Second.weight;
				if (num2 <= 0f)
				{
					index = tuple2.First;
					return tuple2.Second;
				}
			}
		}
		index = list[0].First;
		return list[0].Second;
	}

	// Token: 0x060053E6 RID: 21478 RVA: 0x001EC6DC File Offset: 0x001EA8DC
	public void ClearData(tk2dTileMap map)
	{
		for (int i = 0; i < map.Layers.Length; i++)
		{
			map.DeleteSprites(i, 0, 0, map.width - 1, map.height - 1);
		}
	}

	// Token: 0x060053E7 RID: 21479 RVA: 0x001EC71C File Offset: 0x001EA91C
	private void BuildBorderIndicesForCell(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (this.t.placeBorders)
		{
			if (current.nearestRoom != null && current.nearestRoom.area.prototypeRoom != null && current.nearestRoom.area.prototypeRoom.preventBorders)
			{
				return;
			}
			if (this.BCheck(d, ix, iy, -2) && (current.type == CellType.WALL || d.data.isTopWall(ix, iy)) && !d.data.isFaceWallHigher(ix, iy) && !d.data.isFaceWallLower(ix, iy))
			{
				this.BuildBorderIndex(current, d, map, ix, iy);
			}
			if (this.BCheck(d, ix, iy, -2) && (current.type != CellType.WALL || d.data.isAnyFaceWall(ix, iy)) && !d.data.isTopWall(ix, iy) && d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].outerCeilingBorderGrid != null)
			{
				this.BuildOuterBorderIndex(current, d, map, ix, iy);
			}
		}
	}

	// Token: 0x060053E8 RID: 21480 RVA: 0x001EC858 File Offset: 0x001EAA58
	public void ClearTileIndicesForCell(Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		CellData cellData = ((!d.data.CheckInBoundsAndValid(ix, iy)) ? null : d.data[ix, iy]);
		int num = ((cellData == null) ? ix : cellData.positionInTilemap.x);
		int num2 = ((cellData == null) ? iy : cellData.positionInTilemap.y);
		for (int i = 0; i < map.Layers.Length; i++)
		{
			map.Layers[i].SetTile(num, num2, -1);
		}
		if (TK2DTilemapChunkAnimator.PositionToAnimatorMap.ContainsKey(cellData.positionInTilemap))
		{
			for (int j = 0; j < TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellData.positionInTilemap].Count; j++)
			{
				TilemapAnimatorTileManager tilemapAnimatorTileManager = TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellData.positionInTilemap][j];
				if (tilemapAnimatorTileManager.animator)
				{
					TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellData.positionInTilemap].RemoveAt(j);
					j--;
					UnityEngine.Object.Destroy(tilemapAnimatorTileManager.animator.gameObject);
					tilemapAnimatorTileManager.animator = null;
				}
			}
		}
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x001EC98C File Offset: 0x001EAB8C
	public void BuildTileIndicesForCell(Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		CellData cellData = d.data.cellData[ix][iy];
		if (cellData == null)
		{
			return;
		}
		this.BuildOcclusionPartitionIndex(cellData, d, map, ix, iy);
		cellData.isOccludedByTopWall = d.data.isTopWall(ix, iy);
		if (cellData.cellVisualData.hasAlreadyBeenTilemapped)
		{
			return;
		}
		if (cellData.cellVisualData.precludeAllTileDrawing)
		{
			return;
		}
		bool flag = this.BCheck(d, ix, iy, 3) && d.data[ix, iy - 2] != null && d.data[ix, iy - 2].isExitCell;
		if (cellData.nearestRoom != null && cellData.nearestRoom.PrecludeTilemapDrawing && (!cellData.nearestRoom.DrawPrecludedCeilingTiles || (!cellData.isExitCell && !flag)))
		{
			if (cellData.nearestRoom.DrawPrecludedCeilingTiles)
			{
				this.BuildCollisionIndex(cellData, d, map, ix, iy);
				this.BuildBorderIndicesForCell(cellData, d, map, ix, iy);
			}
			cellData.cellVisualData.precludeAllTileDrawing = true;
			return;
		}
		if (cellData.parentRoom != null && cellData.parentRoom.PrecludeTilemapDrawing && (!cellData.nearestRoom.DrawPrecludedCeilingTiles || (!cellData.isExitCell && !flag)))
		{
			if (cellData.parentRoom.DrawPrecludedCeilingTiles)
			{
				this.BuildCollisionIndex(cellData, d, map, ix, iy);
				this.BuildBorderIndicesForCell(cellData, d, map, ix, iy);
			}
			cellData.cellVisualData.precludeAllTileDrawing = true;
			return;
		}
		DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[cellData.cellVisualData.roomVisualTypeIndex];
		if (dungeonMaterial.overrideStoneFloorType && cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Stone)
		{
			cellData.cellVisualData.floorType = dungeonMaterial.overrideFloorType;
		}
		bool flag2 = cellData.type == CellType.FLOOR || d.data.isFaceWallLower(ix, iy);
		if (flag2)
		{
			this.BuildFloorIndex(cellData, d, map, ix, iy);
		}
		this.BuildDecoIndices(cellData, d, map, ix, iy);
		if (flag2)
		{
			this.BuildFloorEdgeBorderTiles(cellData, d, map, ix, iy);
		}
		this.BuildFeatureEdgeBorderTiles(cellData, d, map, ix, iy);
		this.BuildCollisionIndex(cellData, d, map, ix, iy);
		if (this.BCheck(d, ix, iy, -2))
		{
			this.ProcessFacewallIndices(cellData, d, map, ix, iy);
		}
		this.BuildBorderIndicesForCell(cellData, d, map, ix, iy);
		TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[cellData.cellVisualData.roomVisualTypeIndex].pitBorderFlatGrid;
		TileIndexGrid additionalPitBorderFlatGrid = dungeonMaterial.additionalPitBorderFlatGrid;
		PrototypeRoomPitEntry.PitBorderType pitBorderType = cellData.GetPitBorderType(d.data);
		if (pitBorderType == PrototypeRoomPitEntry.PitBorderType.FLAT)
		{
			tileIndexGrid = dungeonMaterial.pitBorderFlatGrid;
		}
		else if (pitBorderType == PrototypeRoomPitEntry.PitBorderType.RAISED)
		{
			tileIndexGrid = dungeonMaterial.pitBorderRaisedGrid;
		}
		int num = ((pitBorderType != PrototypeRoomPitEntry.PitBorderType.RAISED) ? GlobalDungeonData.patternLayerIndex : GlobalDungeonData.actorCollisionLayerIndex);
		int num2 = num;
		bool walls_ARE_PITS = GameManager.Instance.Dungeon.debugSettings.WALLS_ARE_PITS;
		if (cellData.type == CellType.FLOOR)
		{
			if (d.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON && d.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				this.BuildShadowIndex(cellData, d, map, ix, iy);
			}
			if (tileIndexGrid != null)
			{
				this.HandlePitBorderTilePlacement(cellData, tileIndexGrid, map.Layers[num], map, d);
			}
			if (additionalPitBorderFlatGrid != null)
			{
				this.HandlePitBorderTilePlacement(cellData, additionalPitBorderFlatGrid, map.Layers[num2], map, d);
			}
		}
		else if (cellData.type == CellType.PIT && d.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON && d.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
		{
			this.BuildPitShadowIndex(cellData, d, map, ix, iy);
		}
		if (cellData.type == CellType.PIT || (walls_ARE_PITS && cellData.isExitCell))
		{
			TileIndexGrid tileIndexGrid2 = dungeonMaterial.pitLayoutGrid;
			if (tileIndexGrid2 == null)
			{
				tileIndexGrid2 = d.roomMaterialDefinitions[0].pitLayoutGrid;
			}
			map.data.Layers[GlobalDungeonData.pitLayerIndex].ForceNonAnimating = true;
			this.HandlePitTilePlacement(cellData, tileIndexGrid2, map.Layers[GlobalDungeonData.pitLayerIndex], d);
			if (tileIndexGrid != null)
			{
				this.HandlePitBorderTilePlacement(cellData, tileIndexGrid, map.Layers[num], map, d);
			}
			if (additionalPitBorderFlatGrid != null)
			{
				this.HandlePitBorderTilePlacement(cellData, additionalPitBorderFlatGrid, map.Layers[num2], map, d);
			}
		}
		if (d.data.isTopDiagonalWall(ix, iy))
		{
			if (cellData.diagonalWallType == DiagonalWallType.NORTHEAST)
			{
				this.AssignSpecificColorsToTile(cellData.positionInTilemap.x, cellData.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0.5f, 1f), new Color(0f, 1f, 1f), new Color(0f, 1f, 1f), new Color(0f, 1f, 1f), map);
			}
			else if (cellData.diagonalWallType == DiagonalWallType.NORTHWEST)
			{
				this.AssignSpecificColorsToTile(cellData.positionInTilemap.x, cellData.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f), new Color(0f, 1f, 1f), new Color(0f, 1f, 1f), map);
			}
		}
		if (cellData.cellVisualData.pathTilesetGridIndex > -1)
		{
			TileIndexGrid tileIndexGrid3 = d.pathGridDefinitions[cellData.cellVisualData.pathTilesetGridIndex];
			this.HandlePathTilePlacement(cellData, d, map, tileIndexGrid3);
		}
		if (cellData.cellVisualData.UsesCustomIndexOverride01)
		{
			map.SetTile(cellData.positionInTilemap.x, cellData.positionInTilemap.y, cellData.cellVisualData.CustomIndexOverride01Layer, cellData.cellVisualData.CustomIndexOverride01);
		}
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
		{
			this.BuildOcclusionLayerCenterJungle(cellData, d, map, ix, iy);
		}
		if (cellData.distanceFromNearestRoom < 4f && cellData.nearestRoom.area.PrototypeLostWoodsRoom)
		{
			this.HandleLostWoodsMirroring(cellData, d, map, ix, iy);
		}
		cellData.hasBeenGenerated = true;
	}

	// Token: 0x060053EA RID: 21482 RVA: 0x001ECFF0 File Offset: 0x001EB1F0
	private bool CheckLostWoodsCellValidity(Dungeon d, IntVector2 p1, IntVector2 p2)
	{
		CellData cellData = d.data[p1];
		CellData cellData2 = d.data[p2];
		return cellData != null && cellData2 != null && cellData2.isExitCell == cellData.isExitCell && cellData2.IsAnyFaceWall() == cellData.IsAnyFaceWall() && cellData2.IsTopWall() == cellData.IsTopWall() && cellData2.type == cellData.type;
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001ED074 File Offset: 0x001EB274
	private void HandleLostWoodsMirroring(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		RoomHandler nearestRoom = current.nearestRoom;
		IntVector2 intVector = new IntVector2(ix - current.nearestRoom.area.basePosition.x, iy - current.nearestRoom.area.basePosition.y);
		for (int i = 0; i < d.data.rooms.Count; i++)
		{
			RoomHandler roomHandler = d.data.rooms[i];
			if (roomHandler != nearestRoom && roomHandler.area.PrototypeLostWoodsRoom)
			{
				CellData cellData = d.data[intVector + roomHandler.area.basePosition];
				if (cellData != null && current != null)
				{
					if (cellData.isExitCell == current.isExitCell)
					{
						if (cellData.IsAnyFaceWall() == current.IsAnyFaceWall())
						{
							if (cellData.IsTopWall() == current.IsTopWall())
							{
								if (cellData.type == current.type)
								{
									if (this.CheckLostWoodsCellValidity(d, current.position + new IntVector2(0, 1), cellData.position + new IntVector2(0, 1)))
									{
										if (this.CheckLostWoodsCellValidity(d, current.position + new IntVector2(0, -1), cellData.position + new IntVector2(0, -1)))
										{
											if (this.CheckLostWoodsCellValidity(d, current.position + new IntVector2(1, 0), cellData.position + new IntVector2(1, 0)))
											{
												if (this.CheckLostWoodsCellValidity(d, current.position + new IntVector2(-1, 0), cellData.position + new IntVector2(-1, 0)))
												{
													if (!cellData.cellVisualData.hasAlreadyBeenTilemapped)
													{
														cellData.cellVisualData.hasAlreadyBeenTilemapped = true;
														for (int j = 0; j < map.Layers.Length; j++)
														{
															map.Layers[j].SetTile(cellData.positionInTilemap.x, cellData.positionInTilemap.y, map.Layers[j].GetTile(current.positionInTilemap.x, current.positionInTilemap.y));
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060053EC RID: 21484 RVA: 0x001ED2F8 File Offset: 0x001EB4F8
	private void HandlePathTilePlacement(CellData current, Dungeon d, tk2dTileMap map, TileIndexGrid pathGrid)
	{
		List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
		bool[] array = new bool[8];
		for (int i = 0; i < array.Length; i++)
		{
			if (current.cellVisualData.pathTilesetGridIndex == cellNeighbors[i].cellVisualData.pathTilesetGridIndex)
			{
				array[i] = true;
			}
		}
		int num = pathGrid.GetIndexGivenSides(!array[0], !array[2], !array[4], !array[6]);
		int num2 = GlobalDungeonData.patternLayerIndex;
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON && current.type != CellType.PIT)
		{
			if (array[0] == array[4] && array[0] != array[2] && array[0] != array[6])
			{
				num += ((!array[0]) ? 2 : 1);
			}
		}
		else if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
		{
			num2 = GlobalDungeonData.killLayerIndex;
			if (cellNeighbors[4] != null && !array[4] && cellNeighbors[4].type == CellType.PIT)
			{
				int num3 = pathGrid.PathPitPosts.indices[cellNeighbors[4].cellVisualData.roomVisualTypeIndex];
				if (array[0] && array[2])
				{
					num3 = pathGrid.PathPitPostsBL.indices[cellNeighbors[4].cellVisualData.roomVisualTypeIndex];
				}
				else if (array[0] && array[6])
				{
					num3 = pathGrid.PathPitPostsBR.indices[cellNeighbors[4].cellVisualData.roomVisualTypeIndex];
				}
				map.Layers[GlobalDungeonData.killLayerIndex].SetTile(cellNeighbors[4].positionInTilemap.x, cellNeighbors[4].positionInTilemap.y, num3);
			}
		}
		map.Layers[num2].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num);
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001ED4FC File Offset: 0x001EB6FC
	private void BuildFeatureEdgeBorderTiles(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
		{
			TileIndexGrid exteriorFacadeBorderGrid = d.roomMaterialDefinitions[1].exteriorFacadeBorderGrid;
			List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
			bool[] array = new bool[8];
			for (int i = 0; i < array.Length; i++)
			{
				if (cellNeighbors[i] != null)
				{
					array[i] = cellNeighbors[i].cellVisualData.IsFeatureCell || cellNeighbors[i].cellVisualData.IsFeatureAdditional;
				}
			}
			int indexGivenEightSides = exteriorFacadeBorderGrid.GetIndexGivenEightSides(array);
			if (indexGivenEightSides != -1)
			{
				map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenEightSides);
			}
		}
	}

	// Token: 0x060053EE RID: 21486 RVA: 0x001ED5D0 File Offset: 0x001EB7D0
	private void BuildFloorEdgeBorderTiles(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current.type != CellType.FLOOR && !d.data.isFaceWallLower(ix, iy))
		{
			return;
		}
		TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].roomFloorBorderGrid;
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON && current.cellVisualData.IsFacewallForInteriorTransition)
		{
			tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.InteriorTransitionIndex].exteriorFacadeBorderGrid;
		}
		if (tileIndexGrid != null)
		{
			if (current.diagonalWallType == DiagonalWallType.NONE || !d.data.isFaceWallLower(ix, iy))
			{
				List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
				bool[] array = new bool[8];
				for (int i = 0; i < array.Length; i++)
				{
					if (cellNeighbors[i] != null)
					{
						array[i] = cellNeighbors[i].type == CellType.WALL && !d.data.isTopWall(cellNeighbors[i].position.x, cellNeighbors[i].position.y + 1) && cellNeighbors[i].diagonalWallType == DiagonalWallType.NONE;
						bool flag = cellNeighbors[i].isSecretRoomCell || (d.data[cellNeighbors[i].position + IntVector2.Up].IsTopWall() && d.data[cellNeighbors[i].position + IntVector2.Up].isSecretRoomCell);
						array[i] = array[i] || flag != current.isSecretRoomCell;
					}
				}
				int indexGivenEightSides = tileIndexGrid.GetIndexGivenEightSides(array);
				if (indexGivenEightSides != -1)
				{
					map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenEightSides);
				}
			}
			else
			{
				int indexByWeight = tileIndexGrid.quadNubs.GetIndexByWeight();
				if (indexByWeight != -1)
				{
					map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexByWeight);
				}
			}
		}
	}

	// Token: 0x060053EF RID: 21487 RVA: 0x001ED818 File Offset: 0x001EBA18
	private void AssignSpecificColorsToTile(int ix, int iy, int layer, Color32 bottomLeft, Color32 bottomRight, Color32 topLeft, Color32 topRight, tk2dTileMap map)
	{
		if (!map.HasColorChannel())
		{
			map.CreateColorChannel();
		}
		ColorChannel colorChannel = map.ColorChannel;
		map.data.Layers[layer].useColor = true;
		colorChannel.SetTileColorGradient(ix, iy, bottomLeft, bottomRight, topLeft, topRight);
	}

	// Token: 0x060053F0 RID: 21488 RVA: 0x001ED864 File Offset: 0x001EBA64
	private void AssignColorGradientToTile(int ix, int iy, int layer, Color32 bottom, Color32 top, tk2dTileMap map)
	{
		if (!map.HasColorChannel())
		{
			map.CreateColorChannel();
		}
		ColorChannel colorChannel = map.ColorChannel;
		map.data.Layers[layer].useColor = true;
		colorChannel.SetTileColorGradient(ix, iy, bottom, bottom, top, top);
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x001ED8B0 File Offset: 0x001EBAB0
	private void AssignColorOverrideToTile(int ix, int iy, int layer, Color32 color, tk2dTileMap map)
	{
		if (!map.HasColorChannel())
		{
			map.CreateColorChannel();
		}
		ColorChannel colorChannel = map.ColorChannel;
		map.data.Layers[layer].useColor = true;
		colorChannel.SetTileColorOverride(ix, iy, color);
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001ED8F8 File Offset: 0x001EBAF8
	private void ClearAllIndices(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		for (int i = 0; i < map.Layers.Length; i++)
		{
			map.Layers[i].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, -1);
		}
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x001ED944 File Offset: 0x001EBB44
	private bool CheckHasValidFloorGridForRoomSubType(List<TileIndexGrid> indexGrids, int roomType)
	{
		for (int i = 0; i < indexGrids.Count; i++)
		{
			if (indexGrids[i].roomTypeRestriction == -1 || indexGrids[i].roomTypeRestriction == roomType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x001ED990 File Offset: 0x001EBB90
	private RoomInternalMaterialTransition GetMaterialTransitionFromSubtypes(Dungeon d, int roomType, int cellType)
	{
		if (!d.roomMaterialDefinitions[roomType].usesInternalMaterialTransitions)
		{
			return null;
		}
		if (roomType == cellType)
		{
			return null;
		}
		for (int i = 0; i < d.roomMaterialDefinitions[roomType].internalMaterialTransitions.Length; i++)
		{
			if (d.roomMaterialDefinitions[roomType].internalMaterialTransitions[i].materialTransition == cellType)
			{
				return d.roomMaterialDefinitions[roomType].internalMaterialTransitions[i];
			}
		}
		return null;
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001EDA08 File Offset: 0x001EBC08
	private void BuildFloorIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current.cellVisualData.inheritedOverrideIndex != -1)
		{
			map.Layers[(!current.cellVisualData.inheritedOverrideIndexIsFloor) ? GlobalDungeonData.patternLayerIndex : GlobalDungeonData.floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, current.cellVisualData.inheritedOverrideIndex);
		}
		if (current.cellVisualData.inheritedOverrideIndex == -1 || !current.cellVisualData.inheritedOverrideIndexIsFloor)
		{
			bool flag = true;
			TileIndexGrid randomGridFromArray = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].GetRandomGridFromArray(d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].floorSquares);
			if (randomGridFromArray == null)
			{
				flag = false;
			}
			if (flag)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (!this.BCheck(d, ix + i, iy + j))
						{
							flag = false;
						}
						if (d.data.isWall(ix + i, iy + j) || d.data.isAnyFaceWall(ix + i, iy + j))
						{
							flag = false;
						}
						CellData cellData = d.data.cellData[ix + i][iy + j];
						if (cellData.HasWallNeighbor(true, false) || cellData.HasPitNeighbor(d.data))
						{
							flag = false;
						}
						if (cellData.cellVisualData.roomVisualTypeIndex != current.cellVisualData.roomVisualTypeIndex)
						{
							flag = false;
						}
						if (cellData.cellVisualData.inheritedOverrideIndex != -1)
						{
							flag = false;
						}
						if (cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Ice)
						{
							flag = false;
						}
						if (cellData.doesDamage)
						{
							flag = false;
						}
						if (!flag)
						{
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag && current.UniqueHash < d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].floorSquareDensity)
			{
				TileIndexGrid tileIndexGrid = randomGridFromArray;
				int num = ((current.UniqueHash >= 0.025f) ? 3 : 2);
				if (tileIndexGrid.topIndices.indices[0] == -1)
				{
					num = 2;
				}
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < num; l++)
					{
						bool flag2 = l == num - 1;
						bool flag3 = l == 0;
						bool flag4 = k == num - 1;
						bool flag5 = k == 0;
						int indexGivenSides = tileIndexGrid.GetIndexGivenSides(flag2, flag4, flag3, flag5);
						if (this.BCheck(d, ix + k, iy + l))
						{
							if (!d.data.isFaceWallLower(ix + k, iy + l))
							{
								if (d.data.cellData[ix + k][iy + l].type != CellType.PIT)
								{
									CellData cellData2 = d.data.cellData[ix + k][iy + l];
									cellData2.cellVisualData.inheritedOverrideIndex = indexGivenSides;
									cellData2.cellVisualData.inheritedOverrideIndexIsFloor = true;
									map.Layers[GlobalDungeonData.floorLayerIndex].SetTile(cellData2.positionInTilemap.x, cellData2.positionInTilemap.y, indexGivenSides);
								}
							}
						}
					}
				}
			}
			else if (current.cellVisualData.floorType == CellVisualData.CellFloorType.Ice && d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].supportsIceSquares)
			{
				TileIndexGrid randomGridFromArray2 = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].GetRandomGridFromArray(d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].iceGrids);
				List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
				bool flag6 = cellNeighbors[0].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag7 = cellNeighbors[1].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag8 = cellNeighbors[2].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag9 = cellNeighbors[3].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag10 = cellNeighbors[4].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag11 = cellNeighbors[5].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag12 = cellNeighbors[6].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				bool flag13 = cellNeighbors[7].cellVisualData.floorType != CellVisualData.CellFloorType.Ice;
				int indexGivenSides2 = randomGridFromArray2.GetIndexGivenSides(flag6, flag7, flag8, flag9, flag10, flag11, flag12, flag13);
				map.Layers[GlobalDungeonData.floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenSides2);
				map.Layers[GlobalDungeonData.patternLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenSides2);
			}
			else if (current.doesDamage && d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].supportsLavaOrLavalikeSquares)
			{
				TileIndexGrid randomGridFromArray3 = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].GetRandomGridFromArray(d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].lavaGrids);
				List<CellData> cellNeighbors2 = d.data.GetCellNeighbors(current, true);
				bool flag14 = !cellNeighbors2[0].doesDamage;
				bool flag15 = !cellNeighbors2[1].doesDamage;
				bool flag16 = !cellNeighbors2[2].doesDamage;
				bool flag17 = !cellNeighbors2[3].doesDamage;
				bool flag18 = !cellNeighbors2[4].doesDamage;
				bool flag19 = !cellNeighbors2[5].doesDamage;
				bool flag20 = !cellNeighbors2[6].doesDamage;
				bool flag21 = !cellNeighbors2[7].doesDamage;
				int indexGivenSides3 = randomGridFromArray3.GetIndexGivenSides(flag14, flag15, flag16, flag17, flag18, flag19, flag20, flag21);
				map.Layers[GlobalDungeonData.floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenSides3);
				map.Layers[GlobalDungeonData.patternLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenSides3);
			}
			else
			{
				RoomInternalMaterialTransition roomInternalMaterialTransition = ((current != null && current.parentRoom != null) ? this.GetMaterialTransitionFromSubtypes(d, current.parentRoom.RoomVisualSubtype, current.cellVisualData.roomVisualTypeIndex) : null);
				if (roomInternalMaterialTransition != null)
				{
					List<CellData> cellNeighbors3 = d.data.GetCellNeighbors(current, true);
					bool flag22 = cellNeighbors3[0].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag23 = cellNeighbors3[1].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag24 = cellNeighbors3[2].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag25 = cellNeighbors3[3].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag26 = cellNeighbors3[4].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag27 = cellNeighbors3[5].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag28 = cellNeighbors3[6].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag29 = cellNeighbors3[7].cellVisualData.roomVisualTypeIndex == current.parentRoom.RoomVisualSubtype;
					bool flag30 = flag22 || flag23 || flag24 || flag25 || flag26 || flag27 || flag28 || flag29;
					int num2 = this.GetIndexFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE], current.cellVisualData.roomVisualTypeIndex);
					if (flag30)
					{
						num2 = roomInternalMaterialTransition.transitionGrid.GetIndexGivenSides(flag22, flag23, flag24, flag25, flag26, flag27, flag28, flag29);
					}
					map.Layers[GlobalDungeonData.floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num2);
				}
				else
				{
					int indexFromTupleArray = this.GetIndexFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE], current.cellVisualData.roomVisualTypeIndex);
					map.Layers[GlobalDungeonData.floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexFromTupleArray);
				}
			}
		}
		if (d.data.HasDoorAtPosition(new IntVector2(ix, iy)) || d.data[ix, iy].cellVisualData.doorFeetOverrideMode != 0)
		{
			DungeonDoorController dungeonDoorController = null;
			IntVector2 intVector = new IntVector2(ix, iy);
			if (d.data.doors.ContainsKey(intVector))
			{
				dungeonDoorController = d.data.doors[intVector];
			}
			if (d.data[ix, iy].cellVisualData.doorFeetOverrideMode == 1 || (dungeonDoorController != null && dungeonDoorController.northSouth))
			{
				int num3 = -1;
				this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DOOR_FEET_NS], -1, out num3);
				map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
				map.Layers[GlobalDungeonData.patternLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
			}
			else if (d.data[ix, iy].cellVisualData.doorFeetOverrideMode == 2 || (dungeonDoorController != null && !dungeonDoorController.northSouth))
			{
				int num4 = -1;
				this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DOOR_FEET_EW], -1, out num4);
				map.Layers[GlobalDungeonData.patternLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num4);
				map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num4);
			}
		}
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001EE488 File Offset: 0x001EC688
	private void BuildDecoIndices(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if ((current.type == CellType.FLOOR || current.IsLowerFaceWall()) && !d.data.isTopWall(ix, iy) && !current.cellVisualData.floorTileOverridden && current.cellVisualData.inheritedOverrideIndex == -1)
		{
			DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex];
			if (current.HasPitNeighbor(d.data))
			{
				return;
			}
			if (current.cellVisualData.isPattern)
			{
				List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
				bool[] array = new bool[8];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = !cellNeighbors[i].cellVisualData.isPattern && cellNeighbors[i].type != CellType.WALL;
				}
				TileIndexGrid tileIndexGrid = ((!dungeonMaterial.usesPatternLayer) ? this.t.patternIndexGrid : dungeonMaterial.patternIndexGrid);
				current.cellVisualData.preventFloorStamping = true;
				if (tileIndexGrid != null)
				{
					map.Layers[GlobalDungeonData.patternLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, tileIndexGrid.GetIndexGivenSides(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]));
				}
			}
			if (current.cellVisualData.isDecal)
			{
				List<CellData> cellNeighbors2 = d.data.GetCellNeighbors(current, true);
				bool[] array2 = new bool[8];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = !cellNeighbors2[j].cellVisualData.isDecal && cellNeighbors2[j].type != CellType.WALL;
				}
				TileIndexGrid tileIndexGrid2 = ((!dungeonMaterial.usesDecalLayer) ? this.t.decalIndexGrid : dungeonMaterial.decalIndexGrid);
				current.cellVisualData.preventFloorStamping = true;
				if (tileIndexGrid2 != null)
				{
					map.Layers[GlobalDungeonData.decalLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, tileIndexGrid2.GetIndexGivenSides(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6], array2[7]));
				}
			}
		}
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001EE6F0 File Offset: 0x001EC8F0
	private bool IsValidJungleBorderCell(CellData current, Dungeon d, int ix, int iy)
	{
		return !current.cellVisualData.ceilingHasBeenProcessed && !this.IsCardinalBorder(current, d, ix, iy) && current.type == CellType.WALL && (iy < 2 || !d.data.isFaceWallLower(ix, iy)) && !d.data.isTopDiagonalWall(ix, iy);
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x001EE758 File Offset: 0x001EC958
	private bool IsValidJungleOcclusionCell(CellData current, Dungeon d, int ix, int iy)
	{
		return this.BCheck(d, ix, iy, 1) && (!current.cellVisualData.ceilingHasBeenProcessed && !current.cellVisualData.occlusionHasBeenProcessed) && (current.type != CellType.WALL || this.IsCardinalBorder(current, d, ix, iy) || (iy > 2 && (d.data.isFaceWallLower(ix, iy) || d.data.isFaceWallHigher(ix, iy))));
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001EE7E8 File Offset: 0x001EC9E8
	private void BuildOcclusionLayerCenterJungle(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (!this.IsValidJungleOcclusionCell(current, d, ix, iy))
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		if (!this.BCheck(d, ix, iy))
		{
			flag = false;
			flag2 = false;
		}
		if (current.UniqueHash < 0.05f)
		{
			flag = false;
			flag2 = false;
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!this.IsValidJungleOcclusionCell(d.data[ix + i, iy + j], d, ix + i, iy + j))
				{
					flag2 = false;
					if (i < 2 || j < 2)
					{
						flag = false;
					}
				}
				if (!flag2 && !flag)
				{
					break;
				}
			}
			if (!flag2 && !flag)
			{
				break;
			}
		}
		if (flag2 && current.UniqueHash < 0.75f)
		{
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
			d.data[ix + 1, iy].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 2, iy].cellVisualData.occlusionHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
			d.data[ix, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 1, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 2, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
			d.data[ix, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 1, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 2, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
		}
		else if (flag)
		{
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
			d.data[ix + 1, iy].cellVisualData.occlusionHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
			d.data[ix, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
			d.data[ix + 1, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
		}
		else
		{
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
		}
		d.data[ix, iy].cellVisualData.occlusionHasBeenProcessed = true;
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001EECBC File Offset: 0x001ECEBC
	private void BuildBorderLayerCenterJungle(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (!this.IsValidJungleBorderCell(current, d, ix, iy))
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		if (!this.BCheck(d, ix, iy))
		{
			flag = false;
			flag2 = false;
		}
		if (current.UniqueHash < 0.05f)
		{
			flag = false;
			flag2 = false;
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!this.IsValidJungleBorderCell(d.data[ix + i, iy + j], d, ix + i, iy + j))
				{
					flag2 = false;
					if (i < 2 || j < 2)
					{
						flag = false;
					}
				}
				if (!flag2 && !flag)
				{
					break;
				}
			}
			if (!flag2 && !flag)
			{
				break;
			}
		}
		if (flag2 && current.UniqueHash < 0.75f)
		{
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
			d.data[ix + 1, iy].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 2, iy].cellVisualData.ceilingHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
			d.data[ix, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 1, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 2, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
			d.data[ix, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 1, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 2, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
		}
		else if (flag)
		{
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
			d.data[ix + 1, iy].cellVisualData.ceilingHasBeenProcessed = true;
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
			d.data[ix, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
			d.data[ix + 1, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
		}
		else
		{
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
			map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
		}
		d.data[ix, iy].cellVisualData.ceilingHasBeenProcessed = true;
	}

	// Token: 0x060053FB RID: 21499 RVA: 0x001EF418 File Offset: 0x001ED618
	private void BuildCollisionIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current.type == CellType.WALL && (iy < 2 || !d.data.isFaceWallLower(ix, iy)) && !d.data.isTopDiagonalWall(ix, iy))
		{
			TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].roomCeilingBorderGrid;
			if ((d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON || d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON) && current.nearestRoom != null)
			{
				tileIndexGrid = d.roomMaterialDefinitions[current.nearestRoom.RoomVisualSubtype].roomCeilingBorderGrid;
			}
			if (tileIndexGrid == null)
			{
				tileIndexGrid = d.roomMaterialDefinitions[0].roomCeilingBorderGrid;
			}
			map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, tileIndexGrid.centerIndices.indices[0]);
		}
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x001EF518 File Offset: 0x001ED718
	private void BuildPitShadowIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (!d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].doPitAO)
		{
			return;
		}
		if (current != null && current.cellVisualData.hasStampedPath)
		{
			return;
		}
		int floorLayerIndex = GlobalDungeonData.floorLayerIndex;
		if (this.BCheck(d, ix, iy, -2))
		{
			CellData cellData = d.data.cellData[ix - 1][iy];
			CellData cellData2 = d.data.cellData[ix + 1][iy];
			CellData cellData3 = d.data.cellData[ix][iy + 1];
			CellData cellData4 = d.data.cellData[ix][iy + 2];
			CellData cellData5 = d.data.cellData[ix + 1][iy + 2];
			CellData cellData6 = d.data.cellData[ix + 1][iy + 1];
			CellData cellData7 = d.data.cellData[ix - 1][iy + 2];
			CellData cellData8 = d.data.cellData[ix - 1][iy + 1];
			DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex];
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			if (dungeonMaterial.pitsAreOneDeep)
			{
				flag = cellData.type != CellType.PIT;
				flag2 = cellData2.type != CellType.PIT;
				flag3 = cellData3.type != CellType.PIT;
				flag4 = cellData6.type != CellType.PIT;
				flag5 = cellData8.type != CellType.PIT;
			}
			else
			{
				flag = cellData3.type == CellType.PIT && cellData8.type != CellType.PIT;
				flag2 = cellData3.type == CellType.PIT && cellData6.type != CellType.PIT;
				flag3 = cellData4.type != CellType.PIT && cellData3.type == CellType.PIT;
				flag4 = cellData5.type != CellType.PIT && cellData6.type == CellType.PIT;
				flag5 = cellData7.type != CellType.PIT && cellData8.type == CellType.PIT;
			}
			if (dungeonMaterial.pitfallVFXPrefab != null && dungeonMaterial.pitfallVFXPrefab.name.ToLowerInvariant().Contains("splash"))
			{
				if (flag3 && flag && !flag2)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndLeft);
				}
				else if (flag3 && flag2 && !flag)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndRight);
				}
				else if (flag3 && flag && flag2)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndBoth);
				}
				else if (flag3 && !flag && !flag2)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorTileIndex);
				}
			}
			else if (flag3 && flag && !flag2)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOBottomWallTileLeftIndex);
			}
			else if (flag3 && flag2 && !flag)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOBottomWallTileRightIndex);
			}
			else if (flag3 && flag && flag2)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOBottomWallTileBothIndex);
			}
			else if (flag3 && !flag && !flag2)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOBottomWallBaseTileIndex);
			}
			if (!flag3)
			{
				bool flag6 = flag && !d.data.isTopWall(current.positionInTilemap.x - 1, current.positionInTilemap.y + 1);
				bool flag7 = flag2 && !d.data.isTopWall(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1);
				if (flag6 && flag7)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallBoth);
				}
				else if (flag6)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallLeft);
				}
				else if (flag7)
				{
					map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallRight);
				}
			}
			if (!flag3 && flag5 && !flag && !flag2 && !flag4)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceLeft);
			}
			else if (!flag3 && !flag5 && !flag && !flag2 && flag4)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceRight);
			}
			else if (!flag3 && flag5 && !flag2 && !flag && flag4)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceBoth);
			}
			else if (!flag3 && flag5 && !flag && flag2)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceLeftWallRight);
			}
			else if (!flag3 && flag && !flag2 && flag4)
			{
				map.Layers[floorLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceRightWallLeft);
			}
		}
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x001EFC94 File Offset: 0x001EDE94
	private void BuildShadowIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (this.BCheck(d, ix, iy, -2))
		{
			CellData cellData = d.data.cellData[ix - 1][iy];
			CellData cellData2 = d.data.cellData[ix + 1][iy];
			CellData cellData3 = d.data.cellData[ix][iy + 1];
			CellData cellData4 = d.data.cellData[ix + 1][iy + 1];
			CellData cellData5 = d.data.cellData[ix - 1][iy + 1];
			bool flag = cellData.type == CellType.WALL && cellData.diagonalWallType == DiagonalWallType.NONE;
			bool flag2 = cellData2.type == CellType.WALL && cellData2.diagonalWallType == DiagonalWallType.NONE;
			bool flag3 = cellData3.type == CellType.WALL;
			bool flag4 = cellData4.type == CellType.WALL && cellData4.diagonalWallType == DiagonalWallType.NONE;
			bool flag5 = cellData5.type == CellType.WALL && cellData5.diagonalWallType == DiagonalWallType.NONE;
			if (current.parentRoom != null && current.parentRoom.area.prototypeRoom != null && current.parentRoom.area.prototypeRoom.preventFacewallAO)
			{
				flag3 = false;
				flag4 = false;
				flag5 = false;
			}
			bool flag6 = cellData3.isSecretRoomCell != current.isSecretRoomCell;
			if (cellData3.diagonalWallType == DiagonalWallType.NONE)
			{
				if (flag3 && flag && !flag2)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndLeft);
				}
				else if (flag3 && flag2 && !flag)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndRight);
				}
				else if (flag3 && flag && flag2)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallUpAndBoth);
				}
				else if (flag3 && !flag && !flag2)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorTileIndex);
				}
			}
			else if (cellData3.diagonalWallType == DiagonalWallType.NORTHEAST && cellData3.type == CellType.WALL)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, this.t.aoTileIndices.AOFloorDiagonalWallNortheast);
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, (!flag2) ? this.t.aoTileIndices.AOFloorDiagonalWallNortheastLower : this.t.aoTileIndices.AOFloorDiagonalWallNortheastLowerJoint);
			}
			else if (cellData3.diagonalWallType == DiagonalWallType.NORTHWEST && cellData3.type == CellType.WALL)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, this.t.aoTileIndices.AOFloorDiagonalWallNorthwest);
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, (!flag) ? this.t.aoTileIndices.AOFloorDiagonalWallNorthwestLower : this.t.aoTileIndices.AOFloorDiagonalWallNorthwestLowerJoint);
			}
			if (!flag3)
			{
				bool flag7 = flag && !d.data.isTopWall(ix - 1, iy + 1);
				bool flag8 = flag2 && !d.data.isTopWall(ix + 1, iy + 1);
				if (flag7 && flag8)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallBoth);
				}
				else if (flag7)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallLeft);
				}
				else if (flag8)
				{
					map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorWallRight);
				}
			}
			if (!flag3 && flag5 && !flag6 && !flag && !flag2 && !flag4)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceLeft);
			}
			else if (!flag3 && !flag5 && !flag && !flag2 && flag4 && !flag6)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceRight);
			}
			else if (!flag3 && flag5 && !flag6 && !flag2 && !flag && flag4 && !flag6)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceBoth);
			}
			else if (!flag3 && flag5 && !flag6 && !flag && flag2)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceLeftWallRight);
			}
			else if (!flag3 && flag && !flag2 && flag4 && !flag6)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOFloorPizzaSliceRightWallLeft);
			}
		}
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001F0394 File Offset: 0x001EE594
	public void ApplyTileStamp(int ix, int iy, TileStampData tsd, Dungeon d, tk2dTileMap map, int overrideTileLayerIndex = -1)
	{
		DungeonTileStampData.StampSpace occupySpace = tsd.occupySpace;
		for (int i = 0; i < tsd.width; i++)
		{
			for (int j = 0; j < tsd.height; j++)
			{
				CellVisualData cellVisualData = d.data.cellData[ix + i][iy + j].cellVisualData;
				if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
				{
					if (cellVisualData.containsObjectSpaceStamp || cellVisualData.containsWallSpaceStamp || cellVisualData.containsLight)
					{
						return;
					}
				}
				else if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
				{
					if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
					{
						if (cellVisualData.containsObjectSpaceStamp || cellVisualData.containsLight)
						{
							return;
						}
					}
					else if (cellVisualData.containsObjectSpaceStamp)
					{
						return;
					}
				}
				else if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE && (cellVisualData.containsWallSpaceStamp || cellVisualData.containsLight))
				{
					return;
				}
			}
		}
		for (int k = 0; k < tsd.width; k++)
		{
			for (int l = 0; l < tsd.height; l++)
			{
				CellData cellData = d.data.cellData[ix + k][iy + l];
				CellVisualData cellVisualData2 = cellData.cellVisualData;
				int num = ((occupySpace != DungeonTileStampData.StampSpace.OBJECT_SPACE) ? GlobalDungeonData.wallStampLayerIndex : GlobalDungeonData.objectStampLayerIndex);
				if (d.data.isFaceWallHigher(ix + k, iy + l - 1))
				{
					num = GlobalDungeonData.aboveBorderLayerIndex;
				}
				if (!d.data.isAnyFaceWall(ix + k, iy + l) && d.data.cellData[ix + k][iy + l].type == CellType.WALL)
				{
					num = GlobalDungeonData.aboveBorderLayerIndex;
				}
				if (overrideTileLayerIndex != -1)
				{
					num = overrideTileLayerIndex;
				}
				map.Layers[num].SetTile(cellData.positionInTilemap.x, cellData.positionInTilemap.y, tsd.stampTileIndices[(tsd.height - 1 - l) * tsd.width + k]);
				if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE)
				{
					cellVisualData2.containsWallSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
					cellVisualData2.containsWallSpaceStamp = true;
				}
			}
		}
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001F05E4 File Offset: 0x001EE7E4
	public void ApplyStampGeneric(int ix, int iy, StampDataBase sd, Dungeon d, tk2dTileMap map, bool flipX = false, int overrideTileLayerIndex = -1)
	{
		if (sd is TileStampData)
		{
			this.ApplyTileStamp(ix, iy, sd as TileStampData, d, map, overrideTileLayerIndex);
		}
		else if (sd is SpriteStampData)
		{
			this.ApplySpriteStamp(ix, iy, sd as SpriteStampData, d, map);
		}
		else if (sd is ObjectStampData)
		{
			TK2DDungeonAssembler.ApplyObjectStamp(ix, iy, sd as ObjectStampData, d, map, flipX, false);
		}
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001F0658 File Offset: 0x001EE858
	public static GameObject ApplyObjectStamp(int ix, int iy, ObjectStampData osd, Dungeon d, tk2dTileMap map, bool flipX = false, bool isLightStamp = false)
	{
		DungeonTileStampData.StampSpace occupySpace = osd.occupySpace;
		for (int i = 0; i < osd.width; i++)
		{
			for (int j = 0; j < osd.height; j++)
			{
				CellData cellData = d.data.cellData[ix + i][iy + j];
				CellVisualData cellVisualData = cellData.cellVisualData;
				if (cellVisualData.forcedMatchingStyle != DungeonTileStampData.IntermediaryMatchingStyle.ANY && cellVisualData.forcedMatchingStyle != osd.intermediaryMatchingStyle)
				{
					return null;
				}
				if (osd.placementRule != DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || !isLightStamp)
				{
					bool flag = cellVisualData.containsWallSpaceStamp;
					if (cellVisualData.facewallGridPreventsWallSpaceStamp && isLightStamp)
					{
						flag = false;
					}
					if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
					{
						if (cellVisualData.containsObjectSpaceStamp || flag || (!isLightStamp && cellVisualData.containsLight))
						{
							return null;
						}
						if (cellData.type == CellType.PIT)
						{
							return null;
						}
					}
					else if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
					{
						if (cellVisualData.containsObjectSpaceStamp)
						{
							return null;
						}
						if (cellData.type == CellType.PIT)
						{
							return null;
						}
					}
					else if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE && (flag || (!isLightStamp && cellVisualData.containsLight)))
					{
						return null;
					}
				}
			}
		}
		int num = ((occupySpace != DungeonTileStampData.StampSpace.OBJECT_SPACE) ? GlobalDungeonData.wallStampLayerIndex : GlobalDungeonData.objectStampLayerIndex);
		float z = map.data.Layers[num].z;
		Vector3 vector = osd.objectReference.transform.position;
		ObjectStampOptions component = osd.objectReference.GetComponent<ObjectStampOptions>();
		if (component != null)
		{
			vector = component.GetPositionOffset();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(osd.objectReference);
		gameObject.transform.position = new Vector3((float)ix, (float)iy, z) + vector;
		if (!isLightStamp && osd.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS)
		{
			gameObject.transform.position = new Vector3((float)(ix + 1), (float)iy, z) + vector.WithX(-vector.x);
		}
		tk2dSprite component2 = gameObject.GetComponent<tk2dSprite>();
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(new IntVector2(ix, iy));
		MinorBreakable componentInChildren = gameObject.GetComponentInChildren<MinorBreakable>();
		if (componentInChildren)
		{
			if (osd.placementRule == DungeonTileStampData.StampPlacementRule.ON_ANY_FLOOR)
			{
				componentInChildren.IgnoredForPotShotsModifier = true;
			}
			componentInChildren.IsDecorativeOnly = true;
		}
		IPlaceConfigurable @interface = gameObject.GetInterface<IPlaceConfigurable>();
		if (@interface != null)
		{
			@interface.ConfigureOnPlacement(absoluteRoomFromPosition);
		}
		SurfaceDecorator component3 = gameObject.GetComponent<SurfaceDecorator>();
		if (component3 != null)
		{
			component3.Decorate(absoluteRoomFromPosition);
		}
		if (flipX)
		{
			if (component2 != null)
			{
				component2.FlipX = true;
				float num2 = Mathf.Ceil(component2.GetBounds().size.x);
				gameObject.transform.position = gameObject.transform.position + new Vector3(num2, 0f, 0f);
			}
			else
			{
				gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1f, 1f, 1f));
			}
		}
		gameObject.transform.parent = ((absoluteRoomFromPosition == null) ? null : absoluteRoomFromPosition.hierarchyParent);
		DepthLookupManager.ProcessRenderer(gameObject.GetComponentInChildren<Renderer>());
		if (component2 != null)
		{
			component2.UpdateZDepth();
		}
		for (int k = 0; k < osd.width; k++)
		{
			for (int l = 0; l < osd.height; l++)
			{
				CellVisualData cellVisualData2 = d.data.cellData[ix + k][iy + l].cellVisualData;
				if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE)
				{
					cellVisualData2.containsWallSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
					cellVisualData2.containsWallSpaceStamp = true;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001F0A60 File Offset: 0x001EEC60
	public void ApplySpriteStamp(int ix, int iy, SpriteStampData ssd, Dungeon d, tk2dTileMap map)
	{
		DungeonTileStampData.StampSpace occupySpace = ssd.occupySpace;
		for (int i = 0; i < ssd.width; i++)
		{
			for (int j = 0; j < ssd.height; j++)
			{
				CellVisualData cellVisualData = d.data.cellData[ix + i][iy + j].cellVisualData;
				if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
				{
					if (cellVisualData.containsObjectSpaceStamp || cellVisualData.containsWallSpaceStamp)
					{
						return;
					}
				}
				else if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
				{
					if (cellVisualData.containsObjectSpaceStamp)
					{
						return;
					}
				}
				else if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE && cellVisualData.containsWallSpaceStamp)
				{
					return;
				}
			}
		}
		int num = ((occupySpace != DungeonTileStampData.StampSpace.OBJECT_SPACE) ? GlobalDungeonData.wallStampLayerIndex : GlobalDungeonData.objectStampLayerIndex);
		float z = map.data.Layers[num].z;
		SpriteRenderer spriteRenderer = new GameObject(ssd.spriteReference.name)
		{
			transform = 
			{
				position = new Vector3((float)ix, (float)iy, z)
			}
		}.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = ssd.spriteReference;
		DepthLookupManager.ProcessRenderer(spriteRenderer);
		for (int k = 0; k < ssd.width; k++)
		{
			for (int l = 0; l < ssd.height; l++)
			{
				CellVisualData cellVisualData2 = d.data.cellData[ix + k][iy + l].cellVisualData;
				if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE)
				{
					cellVisualData2.containsWallSpaceStamp = true;
				}
				if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES)
				{
					cellVisualData2.containsObjectSpaceStamp = true;
					cellVisualData2.containsWallSpaceStamp = true;
				}
			}
		}
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001F0C08 File Offset: 0x001EEE08
	private TileIndexGrid GetCeilingBorderIndexGrid(CellData current, Dungeon d)
	{
		TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].roomCeilingBorderGrid;
		if (tileIndexGrid == null)
		{
			tileIndexGrid = d.roomMaterialDefinitions[0].roomCeilingBorderGrid;
		}
		return tileIndexGrid;
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001F0C48 File Offset: 0x001EEE48
	private int GetCeilingCenterIndex(CellData current, TileIndexGrid gridToUse)
	{
		if (gridToUse.CeilingBorderUsesDistancedCenters)
		{
			int count = gridToUse.centerIndices.indices.Count;
			int num = Mathf.Max(0, Mathf.Min(Mathf.FloorToInt(current.distanceFromNearestRoom) - 1, count - 1));
			return gridToUse.centerIndices.indices[num];
		}
		return gridToUse.centerIndices.GetIndexByWeight();
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001F0CAC File Offset: 0x001EEEAC
	private void BuildOuterBorderIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		bool flag = (d.data.isWall(ix, iy + 1) || d.data.isTopWall(ix, iy + 1)) && !d.data.isAnyFaceWall(ix, iy + 1);
		bool flag2 = (d.data.isWall(ix + 1, iy + 1) || d.data.isTopWall(ix + 1, iy + 1)) && !d.data.isAnyFaceWall(ix + 1, iy + 1);
		bool flag3 = (d.data.isWall(ix + 1, iy) || d.data.isTopWall(ix + 1, iy)) && !d.data.isAnyFaceWall(ix + 1, iy);
		bool flag4 = (d.data.isWall(ix + 1, iy - 1) || d.data.isTopWall(ix + 1, iy - 1)) && !d.data.isAnyFaceWall(ix + 1, iy - 1);
		bool flag5 = (d.data.isWall(ix, iy - 1) || d.data.isTopWall(ix, iy - 1)) && !d.data.isAnyFaceWall(ix, iy - 1);
		bool flag6 = (d.data.isWall(ix - 1, iy - 1) || d.data.isTopWall(ix - 1, iy - 1)) && !d.data.isAnyFaceWall(ix - 1, iy - 1);
		bool flag7 = (d.data.isWall(ix - 1, iy) || d.data.isTopWall(ix - 1, iy)) && !d.data.isAnyFaceWall(ix - 1, iy);
		bool flag8 = (d.data.isWall(ix - 1, iy + 1) || d.data.isTopWall(ix - 1, iy + 1)) && !d.data.isAnyFaceWall(ix - 1, iy + 1);
		TileIndexGrid outerCeilingBorderGrid = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].outerCeilingBorderGrid;
		int indexGivenSides = outerCeilingBorderGrid.GetIndexGivenSides(flag, flag2, flag3, flag4, flag5, flag6, flag7, flag8);
		if (indexGivenSides != -1 && !current.cellVisualData.shouldIgnoreWallDrawing)
		{
			map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, indexGivenSides);
		}
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001F0F68 File Offset: 0x001EF168
	private bool IsCardinalBorder(CellData current, Dungeon d, int ix, int iy)
	{
		bool flag = d.data.isTopWall(ix, iy);
		flag = flag && !d.data[ix, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag2 = (!d.data.isWallRight(ix, iy) && !d.data.isRightTopWall(ix, iy)) || d.data.isFaceWallHigher(ix + 1, iy) || d.data.isFaceWallLower(ix + 1, iy);
		flag2 = flag2 && !d.data[ix + 1, iy].cellVisualData.shouldIgnoreBorders;
		bool flag3 = iy > 3 && d.data.isFaceWallHigher(ix, iy - 1);
		flag3 = flag3 && !d.data[ix, iy - 1].cellVisualData.shouldIgnoreBorders;
		bool flag4 = (!d.data.isWallLeft(ix, iy) && !d.data.isLeftTopWall(ix, iy)) || d.data.isFaceWallHigher(ix - 1, iy) || d.data.isFaceWallLower(ix - 1, iy);
		flag4 = flag4 && !d.data[ix - 1, iy].cellVisualData.shouldIgnoreBorders;
		return flag || flag2 || flag3 || flag4;
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001F10F4 File Offset: 0x001EF2F4
	private TileIndexGrid GetTypeBorderGridForBorderIndex(CellData current, Dungeon d, out int usedVisualType)
	{
		TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex].roomCeilingBorderGrid;
		usedVisualType = current.cellVisualData.roomVisualTypeIndex;
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
		{
			if (current.nearestRoom != null && current.distanceFromNearestRoom < 4f)
			{
				if (current.cellVisualData.IsFacewallForInteriorTransition)
				{
					tileIndexGrid = d.roomMaterialDefinitions[current.cellVisualData.InteriorTransitionIndex].roomCeilingBorderGrid;
					usedVisualType = current.cellVisualData.InteriorTransitionIndex;
				}
				else if (!current.cellVisualData.IsFeatureCell)
				{
					tileIndexGrid = d.roomMaterialDefinitions[current.nearestRoom.RoomVisualSubtype].roomCeilingBorderGrid;
					usedVisualType = current.nearestRoom.RoomVisualSubtype;
				}
			}
		}
		else if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
		{
			tileIndexGrid = d.roomMaterialDefinitions[current.nearestRoom.RoomVisualSubtype].roomCeilingBorderGrid;
			usedVisualType = current.nearestRoom.RoomVisualSubtype;
		}
		if (tileIndexGrid == null)
		{
			tileIndexGrid = d.roomMaterialDefinitions[0].roomCeilingBorderGrid;
			usedVisualType = 0;
		}
		return tileIndexGrid;
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001F1224 File Offset: 0x001EF424
	private void BuildOcclusionPartitionIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current == null)
		{
			return;
		}
		if (current.cellVisualData.ceilingHasBeenProcessed || current.cellVisualData.occlusionHasBeenProcessed)
		{
			return;
		}
		int num = -1;
		TileIndexGrid typeBorderGridForBorderIndex = this.GetTypeBorderGridForBorderIndex(current, d, out num);
		if (typeBorderGridForBorderIndex != null)
		{
			List<CellData> cellNeighbors = d.data.GetCellNeighbors(current, true);
			bool[] array = new bool[8];
			int num2 = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (cellNeighbors[i] != null)
				{
					this.GetTypeBorderGridForBorderIndex(cellNeighbors[i], d, out num2);
					if (d.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON || num2 == 0 || num == 0)
					{
						array[i] = num != num2;
					}
				}
			}
			int num3 = typeBorderGridForBorderIndex.GetIndexGivenEightSides(array);
			if (num3 == -1)
			{
				num3 = typeBorderGridForBorderIndex.centerIndices.GetIndexByWeight();
			}
			map.SetTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.occlusionPartitionIndex, num3);
		}
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x001F133C File Offset: 0x001EF53C
	private bool IsBorderCell(Dungeon d, int ix, int iy)
	{
		bool flag = d.data[ix, iy + 1].diagonalWallType != DiagonalWallType.NONE && (d.data[ix, iy + 1].IsTopWall() || d.data[ix, iy + 1].type == CellType.WALL);
		bool flag2 = d.data[ix + 1, iy].diagonalWallType != DiagonalWallType.NONE && (d.data[ix + 1, iy].IsTopWall() || d.data[ix + 1, iy].type == CellType.WALL);
		bool flag3 = d.data[ix, iy - 1].diagonalWallType != DiagonalWallType.NONE && (d.data[ix, iy - 1].IsTopWall() || d.data[ix, iy - 1].type == CellType.WALL);
		bool flag4 = d.data[ix - 1, iy].diagonalWallType != DiagonalWallType.NONE && (d.data[ix - 1, iy].IsTopWall() || d.data[ix - 1, iy].type == CellType.WALL);
		bool flag5 = d.data.isTopWall(ix, iy);
		flag5 = flag5 && !d.data[ix, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag6 = (!d.data.isWallRight(ix, iy) && !d.data.isRightTopWall(ix, iy)) || d.data.isFaceWallHigher(ix + 1, iy) || d.data.isFaceWallLower(ix + 1, iy);
		flag6 = flag6 && !d.data[ix + 1, iy].cellVisualData.shouldIgnoreBorders;
		bool flag7 = iy > 3 && d.data.isFaceWallHigher(ix, iy - 1);
		flag7 = flag7 && !d.data[ix, iy - 1].cellVisualData.shouldIgnoreBorders;
		bool flag8 = (!d.data.isWallLeft(ix, iy) && !d.data.isLeftTopWall(ix, iy)) || d.data.isFaceWallHigher(ix - 1, iy) || d.data.isFaceWallLower(ix - 1, iy);
		flag8 = flag8 && !d.data[ix - 1, iy].cellVisualData.shouldIgnoreBorders;
		bool flag9 = (!flag || !flag2) && d.data.isTopWall(ix + 1, iy) && !d.data.isTopWall(ix, iy) && (d.data.isWall(ix, iy + 1) || d.data.isTopWall(ix, iy + 1));
		flag9 = flag9 && !d.data[ix + 1, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag10 = (!flag || !flag4) && d.data.isTopWall(ix - 1, iy) && !d.data.isTopWall(ix, iy) && (d.data.isWall(ix, iy + 1) || d.data.isTopWall(ix, iy + 1));
		flag10 = flag10 && !d.data[ix - 1, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag11 = (!flag3 || !flag2) && iy > 3 && d.data.isFaceWallHigher(ix + 1, iy - 1) && !d.data.isFaceWallHigher(ix, iy - 1);
		flag11 = flag11 && !d.data[ix + 1, iy - 1].cellVisualData.shouldIgnoreBorders;
		bool flag12 = (!flag3 || !flag4) && iy > 3 && d.data.isFaceWallHigher(ix - 1, iy - 1) && !d.data.isFaceWallHigher(ix, iy - 1);
		flag12 = flag12 && !d.data[ix - 1, iy - 1].cellVisualData.shouldIgnoreBorders;
		return flag5 || flag6 || flag8 || flag7 || flag9 || flag10 || flag11 || flag12;
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x001F1814 File Offset: 0x001EFA14
	private void HandleRatChunkOverhangs(Dungeon d, int ix, int iy, tk2dTileMap map)
	{
		bool flag = this.IsBorderCell(d, ix, iy + 1);
		bool flag2 = this.IsBorderCell(d, ix + 1, iy);
		bool flag3 = this.IsBorderCell(d, ix, iy - 1);
		bool flag4 = this.IsBorderCell(d, ix - 1, iy);
		bool flag5 = (flag && flag2) || (flag2 && flag3) || (flag3 && flag4) || (flag4 && flag);
		if (flag5)
		{
			if (!flag)
			{
				map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(d.data[ix, iy + 1].positionInTilemap.x, d.data[ix, iy + 1].positionInTilemap.y, 312);
				d.data[ix, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
			}
			if (!flag2)
			{
				map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(d.data[ix + 1, iy].positionInTilemap.x, d.data[ix + 1, iy].positionInTilemap.y, 315);
				d.data[ix + 1, iy].cellVisualData.ceilingHasBeenProcessed = true;
			}
			if (!flag3)
			{
				map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(d.data[ix, iy - 1].positionInTilemap.x, d.data[ix, iy - 1].positionInTilemap.y, 313);
			}
			if (!flag4)
			{
				map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(d.data[ix - 1, iy].positionInTilemap.x, d.data[ix - 1, iy].positionInTilemap.y, 314);
			}
		}
	}

	// Token: 0x0600540A RID: 21514 RVA: 0x001F19FC File Offset: 0x001EFBFC
	private void BuildBorderIndex(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current.cellVisualData.ceilingHasBeenProcessed)
		{
			return;
		}
		bool flag = d.data[ix, iy + 1] != null && d.data[ix, iy + 1].diagonalWallType != DiagonalWallType.NONE && (d.data[ix, iy + 1].IsTopWall() || d.data[ix, iy + 1].type == CellType.WALL);
		bool flag2 = d.data[ix + 1, iy] != null && d.data[ix + 1, iy].diagonalWallType != DiagonalWallType.NONE && (d.data[ix + 1, iy].IsTopWall() || d.data[ix + 1, iy].type == CellType.WALL);
		bool flag3 = d.data[ix, iy - 1] != null && d.data[ix, iy - 1].diagonalWallType != DiagonalWallType.NONE && (d.data[ix, iy - 1].IsTopWall() || d.data[ix, iy - 1].type == CellType.WALL);
		bool flag4 = d.data[ix - 1, iy] != null && d.data[ix - 1, iy].diagonalWallType != DiagonalWallType.NONE && (d.data[ix - 1, iy].IsTopWall() || d.data[ix - 1, iy].type == CellType.WALL);
		bool flag5 = d.data.isTopWall(ix, iy);
		flag5 = flag5 && d.data[ix, iy + 1] != null && !d.data[ix, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag6 = (!d.data.isWallRight(ix, iy) && !d.data.isRightTopWall(ix, iy)) || d.data.isFaceWallHigher(ix + 1, iy) || d.data.isFaceWallLower(ix + 1, iy);
		flag6 = flag6 && d.data[ix + 1, iy] != null && !d.data[ix + 1, iy].cellVisualData.shouldIgnoreBorders;
		bool flag7 = iy > 3 && d.data.isFaceWallHigher(ix, iy - 1);
		flag7 = flag7 && d.data[ix, iy - 1] != null && !d.data[ix, iy - 1].cellVisualData.shouldIgnoreBorders;
		bool flag8 = (!d.data.isWallLeft(ix, iy) && !d.data.isLeftTopWall(ix, iy)) || d.data.isFaceWallHigher(ix - 1, iy) || d.data.isFaceWallLower(ix - 1, iy);
		flag8 = flag8 && d.data[ix - 1, iy] != null && !d.data[ix - 1, iy].cellVisualData.shouldIgnoreBorders;
		bool flag9 = (!flag || !flag2) && d.data.isTopWall(ix + 1, iy) && !d.data.isTopWall(ix, iy) && (d.data.isWall(ix, iy + 1) || d.data.isTopWall(ix, iy + 1));
		flag9 = flag9 && d.data[ix + 1, iy + 1] != null && !d.data[ix + 1, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag10 = (!flag || !flag4) && d.data.isTopWall(ix - 1, iy) && !d.data.isTopWall(ix, iy) && (d.data.isWall(ix, iy + 1) || d.data.isTopWall(ix, iy + 1));
		flag10 = flag10 && d.data[ix - 1, iy + 1] != null && !d.data[ix - 1, iy + 1].cellVisualData.shouldIgnoreBorders;
		bool flag11 = (!flag3 || !flag2) && iy > 3 && d.data.isFaceWallHigher(ix + 1, iy - 1) && !d.data.isFaceWallHigher(ix, iy - 1);
		flag11 = flag11 && d.data[ix + 1, iy - 1] != null && !d.data[ix + 1, iy - 1].cellVisualData.shouldIgnoreBorders;
		bool flag12 = (!flag3 || !flag4) && iy > 3 && d.data.isFaceWallHigher(ix - 1, iy - 1) && !d.data.isFaceWallHigher(ix, iy - 1);
		flag12 = flag12 && d.data[ix - 1, iy - 1] != null && !d.data[ix - 1, iy - 1].cellVisualData.shouldIgnoreBorders;
		int num = -1;
		int num2 = -1;
		TileIndexGrid typeBorderGridForBorderIndex = this.GetTypeBorderGridForBorderIndex(current, d, out num2);
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
		{
			int num3 = -1;
			if (!flag5)
			{
				flag5 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.North], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag9)
			{
				flag9 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.NorthEast], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag6)
			{
				flag6 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.East], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag11)
			{
				flag11 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.SouthEast], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag7)
			{
				flag7 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.South], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag12)
			{
				flag12 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.SouthWest], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag8)
			{
				flag8 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.West], d, out num3) && (num3 == 0 || num2 == 0);
			}
			if (!flag10)
			{
				flag10 = typeBorderGridForBorderIndex != this.GetTypeBorderGridForBorderIndex(d.data[current.position + IntVector2.NorthWest], d, out num3) && (num3 == 0 || num2 == 0);
			}
		}
		if (current.diagonalWallType == DiagonalWallType.NONE)
		{
			if (!flag5 && !flag9 && !flag6 && !flag11 && !flag7 && !flag12 && !flag8 && !flag10)
			{
				if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
				{
					this.BuildBorderLayerCenterJungle(current, d, map, ix, iy);
					num = -1;
				}
				else if (typeBorderGridForBorderIndex.CeilingBorderUsesDistancedCenters)
				{
					int count = typeBorderGridForBorderIndex.centerIndices.indices.Count;
					int num4 = Mathf.Max(0, Mathf.Min(Mathf.FloorToInt(current.distanceFromNearestRoom) - 1, count - 1));
					num = typeBorderGridForBorderIndex.centerIndices.indices[num4];
				}
				else
				{
					num = typeBorderGridForBorderIndex.centerIndices.GetIndexByWeight();
					if (d.tileIndices.globalSecondBorderTiles.Count > 0 && current.distanceFromNearestRoom < 3f && UnityEngine.Random.value > 0.5f)
					{
						num = d.tileIndices.globalSecondBorderTiles[UnityEngine.Random.Range(0, d.tileIndices.globalSecondBorderTiles.Count)];
					}
				}
			}
			else if (typeBorderGridForBorderIndex.UsesRatChunkBorders)
			{
				bool flag13 = iy > 3;
				if (flag13)
				{
					flag13 = !d.data[ix, iy - 1].HasFloorNeighbor(d.data, false, true);
				}
				TileIndexGrid.RatChunkResult ratChunkResult = TileIndexGrid.RatChunkResult.NONE;
				if (d.data[ix, iy].nearestRoom.area.PrototypeLostWoodsRoom)
				{
					num = typeBorderGridForBorderIndex.GetRatChunkIndexGivenSidesStatic(flag5, flag9, flag6, flag11, flag7, flag12, flag8, flag10, flag13, out ratChunkResult);
				}
				else
				{
					num = typeBorderGridForBorderIndex.GetRatChunkIndexGivenSides(flag5, flag9, flag6, flag11, flag7, flag12, flag8, flag10, flag13, out ratChunkResult);
				}
				if (ratChunkResult == TileIndexGrid.RatChunkResult.CORNER)
				{
					this.HandleRatChunkOverhangs(d, ix, iy, map);
				}
			}
			else
			{
				num = typeBorderGridForBorderIndex.GetIndexGivenSides(flag5, flag9, flag6, flag11, flag7, flag12, flag8, flag10);
			}
		}
		else
		{
			switch (current.diagonalWallType)
			{
			case DiagonalWallType.NORTHEAST:
				if (flag7 && flag8)
				{
					num = typeBorderGridForBorderIndex.diagonalBorderNE.GetIndexByWeight();
				}
				break;
			case DiagonalWallType.SOUTHEAST:
				if (flag5 && flag8)
				{
					num = typeBorderGridForBorderIndex.diagonalBorderSE.GetIndexByWeight();
				}
				else
				{
					num = typeBorderGridForBorderIndex.GetIndexGivenSides(flag5, flag9, flag6, flag11, flag7, flag12, flag8, flag10);
				}
				break;
			case DiagonalWallType.SOUTHWEST:
				if (flag5 && flag6)
				{
					num = typeBorderGridForBorderIndex.diagonalBorderSW.GetIndexByWeight();
				}
				else
				{
					num = typeBorderGridForBorderIndex.GetIndexGivenSides(flag5, flag9, flag6, flag11, flag7, flag12, flag8, flag10);
				}
				break;
			case DiagonalWallType.NORTHWEST:
				if (flag7 && flag6)
				{
					num = typeBorderGridForBorderIndex.diagonalBorderNW.GetIndexByWeight();
				}
				break;
			}
		}
		TileIndexGrid typeBorderGridForBorderIndex2 = this.GetTypeBorderGridForBorderIndex(current, d, out num2);
		if (num != -1)
		{
			if (!current.cellVisualData.shouldIgnoreWallDrawing)
			{
				map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num);
			}
			if (current.cellVisualData.shouldIgnoreWallDrawing)
			{
				BraveUtility.DrawDebugSquare(current.position.ToVector2(), Color.blue, 1000f);
			}
			if (flag5 && current.diagonalWallType != DiagonalWallType.NONE)
			{
				int num5 = -1;
				DiagonalWallType diagonalWallType = current.diagonalWallType;
				if (diagonalWallType != DiagonalWallType.SOUTHEAST)
				{
					if (diagonalWallType == DiagonalWallType.SOUTHWEST)
					{
						num5 = typeBorderGridForBorderIndex2.diagonalCeilingSW.GetIndexByWeight();
					}
				}
				else
				{
					num5 = typeBorderGridForBorderIndex2.diagonalCeilingSE.GetIndexByWeight();
				}
				if (num5 != -1)
				{
					map.Layers[GlobalDungeonData.ceilingLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num5);
					this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.ceilingLayerIndex, new Color(1f, 1f, 1f, 0f), map);
				}
				num5 = this.GetCeilingCenterIndex(current, typeBorderGridForBorderIndex2);
				map.Layers[GlobalDungeonData.ceilingLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y - 1, num5);
				this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y - 1, GlobalDungeonData.ceilingLayerIndex, new Color(1f, 1f, 1f, 0f), map);
			}
			else if (flag5)
			{
				int ceilingCenterIndex = this.GetCeilingCenterIndex(current, typeBorderGridForBorderIndex2);
				map.Layers[GlobalDungeonData.ceilingLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, ceilingCenterIndex);
				this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.ceilingLayerIndex, new Color(1f, 1f, 1f, 0f), map);
			}
			else if (flag7 && current.diagonalWallType != DiagonalWallType.NONE)
			{
				int num6 = -1;
				DiagonalWallType diagonalWallType2 = current.diagonalWallType;
				if (diagonalWallType2 != DiagonalWallType.NORTHEAST)
				{
					if (diagonalWallType2 == DiagonalWallType.NORTHWEST)
					{
						num6 = typeBorderGridForBorderIndex2.diagonalCeilingNW.GetIndexByWeight();
					}
				}
				else
				{
					num6 = typeBorderGridForBorderIndex2.diagonalCeilingNE.GetIndexByWeight();
				}
				if (num6 != -1)
				{
					map.Layers[GlobalDungeonData.ceilingLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num6);
					this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.ceilingLayerIndex, new Color(1f, 1f, 1f, 0f), map);
				}
			}
			else if (flag6 || flag8 || flag9 || flag10 || flag7 || flag11 || flag12)
			{
				int ceilingCenterIndex2 = this.GetCeilingCenterIndex(current, typeBorderGridForBorderIndex2);
				map.Layers[GlobalDungeonData.ceilingLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, ceilingCenterIndex2);
				this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.ceilingLayerIndex, new Color(1f, 1f, 1f, 0f), map);
			}
			if (flag5 || (d.data[current.position + IntVector2.Up] != null && d.data[current.position + IntVector2.Up].IsTopWall()))
			{
				this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.borderLayerIndex, new Color(1f, 1f, 1f, 0f), map);
			}
			else
			{
				this.AssignColorOverrideToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.borderLayerIndex, new Color(0f, 0f, 0f), map);
			}
		}
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x001F2A10 File Offset: 0x001F0C10
	private bool ProcessFacewallNeighborMetadata(int ix, int iy, Dungeon d, List<IndexNeighborDependency> neighborDependencies, bool preventWallStamping = false)
	{
		bool flag = d.data.isFaceWallLower(ix, iy);
		CellData cellData = d.data[ix, iy];
		cellData.cellVisualData.containsWallSpaceStamp = cellData.cellVisualData.containsWallSpaceStamp || preventWallStamping;
		bool flag2 = true;
		List<CellData> list = new List<CellData>();
		for (int i = 0; i < neighborDependencies.Count; i++)
		{
			CellData cellData2 = d.data[new IntVector2(ix, iy) + DungeonData.GetIntVector2FromDirection(neighborDependencies[i].neighborDirection)];
			if (cellData2.cellVisualData.faceWallOverrideIndex != -1 || !cellData2.IsAnyFaceWall())
			{
				flag2 = false;
				break;
			}
			if (cellData2.cellVisualData.roomVisualTypeIndex != d.data.cellData[ix][iy].cellVisualData.roomVisualTypeIndex)
			{
				flag2 = false;
				break;
			}
			if (cellData2.position.y == iy && d.data.isFaceWallLower(cellData2.position.x, cellData2.position.y) != flag)
			{
				flag2 = false;
				break;
			}
			list.Add(cellData2);
			CellData cellData3 = cellData2;
			cellData3.cellVisualData.containsWallSpaceStamp = cellData3.cellVisualData.containsWallSpaceStamp || preventWallStamping;
			cellData2.cellVisualData.faceWallOverrideIndex = neighborDependencies[i].neighborIndex;
		}
		if (!flag2)
		{
			for (int j = 0; j < list.Count; j++)
			{
				CellData cellData4 = list[j];
				cellData4.cellVisualData.faceWallOverrideIndex = -1;
			}
		}
		return flag2;
	}

	// Token: 0x0600540C RID: 21516 RVA: 0x001F2BA0 File Offset: 0x001F0DA0
	private bool FaceWallTypesMatch(CellData c1, CellData c2)
	{
		return (c1.IsLowerFaceWall() && c2.IsLowerFaceWall()) || (c1.IsUpperFacewall() && c2.IsUpperFacewall());
	}

	// Token: 0x0600540D RID: 21517 RVA: 0x001F2BD4 File Offset: 0x001F0DD4
	private bool IsNorthernmostColumnarFacewall(CellData current, Dungeon d, int ix, int iy)
	{
		for (CellData cellData = d.data[ix, iy + 1]; cellData != null; cellData = d.data[cellData.position.x, cellData.position.y + 1])
		{
			if (cellData.nearestRoom != current.nearestRoom)
			{
				return true;
			}
			if (cellData.type == CellType.FLOOR)
			{
				return false;
			}
			if (!d.data.CheckInBounds(cellData.position.x, cellData.position.y + 1))
			{
				return true;
			}
		}
		return true;
	}

	// Token: 0x0600540E RID: 21518 RVA: 0x001F2C70 File Offset: 0x001F0E70
	private void ProcessFacewallType(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy, TilesetIndexMetadata.TilesetFlagType wallType, TilesetIndexMetadata.TilesetFlagType tileOverrideType)
	{
		int num = current.cellVisualData.roomVisualTypeIndex;
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON && num == 0)
		{
			bool flag = false;
			int num2 = -1;
			for (int i = 0; i < current.nearestRoom.connectedRooms.Count; i++)
			{
				if (current.nearestRoom.GetDirectionToConnectedRoom(current.nearestRoom.connectedRooms[i]) == DungeonData.Direction.NORTH && current.nearestRoom.connectedRooms[i].RoomVisualSubtype != 0)
				{
					flag = true;
					num2 = current.nearestRoom.connectedRooms[i].RoomVisualSubtype;
					break;
				}
			}
			if (flag && current.cellVisualData.IsFacewallForInteriorTransition)
			{
				num = num2;
			}
		}
		CellData cellData = d.data.cellData[ix + 1][iy];
		CellData cellData2 = d.data.cellData[ix - 1][iy];
		if (current.cellVisualData.faceWallOverrideIndex != -1)
		{
			List<IndexNeighborDependency> dependencies = d.tileIndices.dungeonCollection.GetDependencies(current.cellVisualData.faceWallOverrideIndex);
			if (dependencies != null && dependencies.Count > 0 && current.IsUpperFacewall())
			{
				for (int j = 0; j < dependencies.Count; j++)
				{
					if (dependencies[j].neighborDirection == DungeonData.Direction.NORTH)
					{
						d.data.cellData[ix][iy + 1].cellVisualData.UsesCustomIndexOverride01 = true;
						d.data.cellData[ix][iy + 1].cellVisualData.CustomIndexOverride01 = dependencies[j].neighborIndex;
						d.data.cellData[ix][iy + 1].cellVisualData.CustomIndexOverride01Layer = GlobalDungeonData.borderLayerIndex;
					}
				}
			}
			map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, current.cellVisualData.faceWallOverrideIndex);
		}
		else
		{
			if (current.diagonalWallType != DiagonalWallType.NONE)
			{
				int num3 = -1;
				if (current.diagonalWallType == DiagonalWallType.NORTHEAST)
				{
					if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER)
					{
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_LOWER_NE], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
					}
					else if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER)
					{
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_UPPER_NE], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_TOP_NE], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, num3);
					}
				}
				else if (current.diagonalWallType == DiagonalWallType.NORTHWEST)
				{
					if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER)
					{
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_LOWER_NW], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
					}
					else if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER)
					{
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_UPPER_NW], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num3);
						this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[TilesetIndexMetadata.TilesetFlagType.DIAGONAL_FACEWALL_TOP_NW], num, out num3);
						map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, num3);
					}
				}
				else
				{
					Debug.LogError("Attempting to stamp a facewall tile on a non-facewall diagonal type.");
				}
				if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER)
				{
					if (current.diagonalWallType == DiagonalWallType.NORTHEAST)
					{
						this.AssignSpecificColorsToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0f, 1f), new Color(0f, 0f, 1f), new Color(0f, 0f, 1f), new Color(0f, 0.5f, 1f), map);
					}
					else if (current.diagonalWallType == DiagonalWallType.NORTHWEST)
					{
						this.AssignSpecificColorsToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0f, 1f), new Color(0f, 0f, 1f), new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f), map);
					}
				}
				else if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER)
				{
					if (current.diagonalWallType == DiagonalWallType.NORTHEAST)
					{
						this.AssignSpecificColorsToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0f, 1f), new Color(0f, 0.5f, 1f), new Color(0f, 0.5f, 1f), new Color(0f, 1f, 1f), map);
					}
					else if (current.diagonalWallType == DiagonalWallType.NORTHWEST)
					{
						this.AssignSpecificColorsToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0.5f, 1f), new Color(0f, 0f, 1f), new Color(0f, 1f, 1f), new Color(0f, 0.5f, 1f), map);
					}
				}
				return;
			}
			int num4 = -1;
			bool flag2 = false;
			int num5 = 0;
			while (!flag2 && num5 < 1000)
			{
				num5++;
				flag2 = true;
				TilesetIndexMetadata metadataFromTupleArray = this.GetMetadataFromTupleArray(current, this.m_metadataLookupTable[tileOverrideType], num, out num4);
				List<IndexNeighborDependency> dependencies2 = d.tileIndices.dungeonCollection.GetDependencies(num4);
				if (metadataFromTupleArray != null && dependencies2 != null && dependencies2.Count > 0)
				{
					flag2 = this.ProcessFacewallNeighborMetadata(ix, iy, d, dependencies2, metadataFromTupleArray.preventWallStamping);
				}
			}
			if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON && (tileOverrideType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTCORNER || tileOverrideType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTCORNER || tileOverrideType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTCORNER || tileOverrideType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTCORNER))
			{
				current.cellVisualData.containsWallSpaceStamp = true;
			}
			BraveUtility.Assert(num4 == -1, "FACEWALL INDEX -1, there are no facewalls defined", false);
			map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, num4);
		}
		if (current.parentRoom == null || current.parentRoom.area.prototypeRoom == null || !current.parentRoom.area.prototypeRoom.preventFacewallAO)
		{
			if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.t.aoTileIndices.AOBottomWallBaseTileIndex);
			}
			bool flag3 = cellData.type == CellType.WALL && cellData.diagonalWallType == DiagonalWallType.NONE && (!d.data.isFaceWallRight(ix, iy) || (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER && cellData.IsUpperFacewall()));
			bool flag4 = cellData2.type == CellType.WALL && cellData2.diagonalWallType == DiagonalWallType.NONE && (!d.data.isFaceWallLeft(ix, iy) || (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER && cellData2.IsUpperFacewall()));
			if (flag4 && flag3)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, (wallType != TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER) ? this.t.aoTileIndices.AOTopFacewallBothIndex : this.t.aoTileIndices.AOBottomWallTileBothIndex);
			}
			else if (flag4)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, (wallType != TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER) ? this.t.aoTileIndices.AOTopFacewallLeftIndex : this.t.aoTileIndices.AOBottomWallTileLeftIndex);
			}
			else if (flag3)
			{
				map.Layers[GlobalDungeonData.shadowLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, (wallType != TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER) ? this.t.aoTileIndices.AOTopFacewallRightIndex : this.t.aoTileIndices.AOBottomWallTileRightIndex);
			}
		}
		if (wallType == TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER)
		{
			this.AssignColorGradientToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0f, 1f), new Color(0f, 0.5f, 1f), map);
		}
		else
		{
			this.AssignColorGradientToTile(current.positionInTilemap.x, current.positionInTilemap.y, GlobalDungeonData.collisionLayerIndex, new Color(0f, 0.5f, 1f), new Color(0f, 1f, 1f), map);
		}
	}

	// Token: 0x0600540F RID: 21519 RVA: 0x001F36E4 File Offset: 0x001F18E4
	private int FindValidFacewallExpanse(int ix, int iy, Dungeon d, FacewallIndexGridDefinition gridDefinition)
	{
		int num = 0;
		int roomVisualTypeIndex = d.data[ix, iy].cellVisualData.roomVisualTypeIndex;
		while (d.data.isFaceWallLower(ix, iy) && d.data[ix, iy].cellVisualData.faceWallOverrideIndex == -1 && d.data[ix, iy].cellVisualData.roomVisualTypeIndex == roomVisualTypeIndex)
		{
			bool flag = !d.data.isFaceWallLeft(ix, iy) || !d.data.isFaceWallRight(ix, iy);
			if (!gridDefinition.canExistInCorners && flag)
			{
				break;
			}
			if (d.data[ix, iy - 2].isExitCell && !gridDefinition.canBePlacedInExits)
			{
				break;
			}
			ix++;
			num++;
			if (num >= gridDefinition.maxWidth)
			{
				break;
			}
			if (num > gridDefinition.minWidth && UnityEngine.Random.value < gridDefinition.perTileFailureRate)
			{
				break;
			}
		}
		return num;
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001F3804 File Offset: 0x001F1A04
	private bool AssignFacewallGrid(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy, FacewallIndexGridDefinition gridDefinition)
	{
		int num = this.FindValidFacewallExpanse(ix, iy, d, gridDefinition);
		if (num < gridDefinition.minWidth)
		{
			return false;
		}
		TileIndexGrid grid = gridDefinition.grid;
		int num2 = 0;
		int i = num;
		int num3 = i;
		int num4 = 0;
		if (gridDefinition.hasIntermediaries)
		{
			num3 = UnityEngine.Random.Range(gridDefinition.minIntermediaryBuffer, gridDefinition.maxIntermediaryBuffer + 1);
		}
		bool flag = true;
		int num5 = 0;
		while (i > 0)
		{
			CellData cellData = d.data[ix + num2, iy];
			CellData cellData2 = d.data[ix + num2, iy + 1];
			if (num4 > 0)
			{
				num4--;
				cellData.cellVisualData.faceWallOverrideIndex = grid.doubleNubsBottom.GetIndexByWeight();
				cellData2.cellVisualData.faceWallOverrideIndex = grid.doubleNubsTop.GetIndexByWeight();
				if (num4 == 0)
				{
					flag = true;
					num3 = UnityEngine.Random.Range(gridDefinition.minIntermediaryBuffer, gridDefinition.maxIntermediaryBuffer + 1);
				}
			}
			else
			{
				bool flag2 = false;
				BraveUtility.DrawDebugSquare(cellData.position.ToVector2(), Color.blue, 1000f);
				num3--;
				if (num3 <= 0)
				{
					if (gridDefinition.hasIntermediaries)
					{
						num4 = UnityEngine.Random.Range(gridDefinition.minIntermediaryLength, gridDefinition.maxIntermediaryLength + 1);
					}
					flag2 = true;
				}
				if (flag)
				{
					cellData.cellVisualData.faceWallOverrideIndex = grid.bottomLeftIndices.GetIndexByWeight();
					cellData2.cellVisualData.faceWallOverrideIndex = grid.topLeftIndices.GetIndexByWeight();
					cellData.cellVisualData.containsWallSpaceStamp = true;
					cellData2.cellVisualData.containsWallSpaceStamp = true;
				}
				else if (flag2 || i == 1)
				{
					cellData.cellVisualData.faceWallOverrideIndex = grid.bottomRightIndices.GetIndexByWeight();
					cellData2.cellVisualData.faceWallOverrideIndex = grid.topRightIndices.GetIndexByWeight();
					cellData.cellVisualData.containsWallSpaceStamp = true;
					cellData2.cellVisualData.containsWallSpaceStamp = true;
					if (flag2 && num4 == 0)
					{
						num3 = UnityEngine.Random.Range(gridDefinition.minIntermediaryBuffer, gridDefinition.maxIntermediaryBuffer + 1);
					}
				}
				else
				{
					cellData.cellVisualData.faceWallOverrideIndex = ((!gridDefinition.middleSectionSequential) ? grid.bottomIndices.GetIndexByWeight() : grid.bottomIndices.indices[num5]);
					if (gridDefinition.topsMatchBottoms)
					{
						cellData2.cellVisualData.faceWallOverrideIndex = grid.topIndices.indices[grid.bottomIndices.GetIndexOfIndex(cellData.cellVisualData.faceWallOverrideIndex)];
					}
					else
					{
						cellData2.cellVisualData.faceWallOverrideIndex = ((!gridDefinition.middleSectionSequential) ? grid.topIndices.GetIndexByWeight() : grid.topIndices.indices[num5]);
					}
					num5 = (num5 + 1) % grid.bottomIndices.indices.Count;
					cellData.cellVisualData.forcedMatchingStyle = gridDefinition.forcedStampMatchingStyle;
					cellData2.cellVisualData.forcedMatchingStyle = gridDefinition.forcedStampMatchingStyle;
				}
				flag = false;
				cellData.cellVisualData.containsObjectSpaceStamp = cellData.cellVisualData.containsObjectSpaceStamp || !gridDefinition.canAcceptFloorDecoration;
				cellData2.cellVisualData.containsObjectSpaceStamp = cellData2.cellVisualData.containsObjectSpaceStamp || !gridDefinition.canAcceptFloorDecoration;
				cellData.cellVisualData.facewallGridPreventsWallSpaceStamp = !gridDefinition.canAcceptWallDecoration;
				cellData2.cellVisualData.facewallGridPreventsWallSpaceStamp = !gridDefinition.canAcceptWallDecoration;
				cellData.cellVisualData.containsWallSpaceStamp = cellData.cellVisualData.containsWallSpaceStamp || !gridDefinition.canAcceptWallDecoration;
				cellData2.cellVisualData.containsWallSpaceStamp = cellData2.cellVisualData.containsWallSpaceStamp || !gridDefinition.canAcceptWallDecoration;
			}
			num2++;
			i--;
		}
		return true;
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x001F3C00 File Offset: 0x001F1E00
	private void ProcessFacewallIndices(CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
	{
		if (current.cellVisualData.shouldIgnoreWallDrawing)
		{
			return;
		}
		DungeonMaterial dungeonMaterial = d.roomMaterialDefinitions[current.cellVisualData.roomVisualTypeIndex];
		if (current.cellVisualData.IsFacewallForInteriorTransition)
		{
			dungeonMaterial = d.roomMaterialDefinitions[current.cellVisualData.InteriorTransitionIndex];
		}
		if (d.data.isSingleCellWall(ix, iy))
		{
			map.Layers[GlobalDungeonData.collisionLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, this.GetIndexFromTileArray(current, this.t.chestHighWallIndices).index);
		}
		else if (d.data.isFaceWallLower(ix, iy))
		{
			if (dungeonMaterial != null && dungeonMaterial.usesFacewallGrids)
			{
				FacewallIndexGridDefinition facewallIndexGridDefinition = dungeonMaterial.facewallGrids[UnityEngine.Random.Range(0, dungeonMaterial.facewallGrids.Length)];
				if (current.cellVisualData.faceWallOverrideIndex == -1 && UnityEngine.Random.value < facewallIndexGridDefinition.chanceToPlaceIfPossible)
				{
					this.AssignFacewallGrid(current, d, map, ix, iy, facewallIndexGridDefinition);
				}
			}
			bool flag = d.data.isWallLeft(ix, iy) && !d.data.isFaceWallLeft(ix, iy);
			bool flag2 = d.data.isWallRight(ix, iy) && !d.data.isFaceWallRight(ix, iy);
			bool flag3 = !d.data.isWallLeft(ix, iy);
			bool flag4 = !d.data.isWallRight(ix, iy);
			if (flag3 && dungeonMaterial.forceEdgesDiagonal)
			{
				current.diagonalWallType = DiagonalWallType.NORTHEAST;
			}
			if (flag4 && dungeonMaterial.forceEdgesDiagonal)
			{
				current.diagonalWallType = DiagonalWallType.NORTHWEST;
			}
			if (flag3 && !flag4 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTEDGE, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTEDGE);
			}
			else if (flag4 && !flag3 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTEDGE, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTEDGE);
			}
			else if (flag && !flag2 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTCORNER, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTCORNER);
			}
			else if (flag2 && !flag && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTCORNER, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTCORNER);
			}
			else
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER);
			}
		}
		else if (d.data.isFaceWallHigher(ix, iy))
		{
			bool flag5 = d.data.isWallLeft(ix, iy) && !d.data.isFaceWallLeft(ix, iy);
			bool flag6 = d.data.isWallRight(ix, iy) && !d.data.isFaceWallRight(ix, iy);
			bool flag7 = !d.data.isWallLeft(ix, iy) || (d.data.isFaceWallLeft(ix, iy) && !d.data[ix - 1, iy].IsUpperFacewall());
			bool flag8 = !d.data.isWallRight(ix, iy) || (d.data.isFaceWallRight(ix, iy) && !d.data[ix + 1, iy].IsUpperFacewall());
			if (flag7 && !flag8 && dungeonMaterial.forceEdgesDiagonal)
			{
				current.diagonalWallType = DiagonalWallType.NORTHEAST;
			}
			if (flag8 && !flag7 && dungeonMaterial.forceEdgesDiagonal)
			{
				current.diagonalWallType = DiagonalWallType.NORTHWEST;
			}
			if (flag7 && !flag8 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTEDGE, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTEDGE);
			}
			else if (flag8 && !flag7 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTEDGE, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTEDGE);
			}
			else if (flag5 && !flag6 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTCORNER, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTCORNER);
			}
			else if (flag6 && !flag5 && this.HasMetadataForRoomType(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTCORNER, current.cellVisualData.roomVisualTypeIndex))
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTCORNER);
			}
			else
			{
				this.ProcessFacewallType(current, d, map, ix, iy, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER);
			}
		}
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001F4130 File Offset: 0x001F2330
	public IEnumerator ConstructTK2DDungeon(Dungeon d, tk2dTileMap map)
	{
		for (int j = 0; j < d.data.Width; j++)
		{
			for (int k = 0; k < d.data.Height; k++)
			{
				this.BuildTileIndicesForCell(d, map, j, k);
			}
		}
		yield return null;
		if (d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON || d.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FINALGEON)
		{
			for (int l = 0; l < d.data.Width; l++)
			{
				for (int m = 0; m < d.data.Height; m++)
				{
					CellData cellData = d.data.cellData[l][m];
					if (cellData != null)
					{
						if (cellData.type == CellType.FLOOR)
						{
							this.BuildShadowIndex(cellData, d, map, l, m);
						}
						else if (cellData.type == CellType.PIT)
						{
							this.BuildPitShadowIndex(cellData, d, map, l, m);
						}
					}
				}
			}
		}
		TK2DInteriorDecorator decorator = new TK2DInteriorDecorator(this);
		decorator.PlaceLightDecoration(d, map);
		for (int i = 0; i < d.data.rooms.Count; i++)
		{
			if (d.data.rooms[i].area.prototypeRoom == null || d.data.rooms[i].area.prototypeRoom.usesProceduralDecoration)
			{
				decorator.HandleRoomDecoration(d.data.rooms[i], d, map);
			}
			else
			{
				decorator.HandleRoomDecorationMinimal(d.data.rooms[i], d, map);
			}
			if (i % 5 == 0)
			{
				yield return null;
			}
		}
		if ((d.data.rooms.Count - 1) % 5 != 0)
		{
			yield return null;
		}
		map.Editor__SpriteCollection = this.t.dungeonCollection;
		if (d.ActuallyGenerateTilemap)
		{
			IEnumerator BuildTracker = map.DeferredBuild(tk2dTileMap.BuildFlags.Default);
			while (BuildTracker.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x001F415C File Offset: 0x001F235C
	private void HandlePitBorderTilePlacement(CellData cell, TileIndexGrid borderGrid, Layer tileMapLayer, tk2dTileMap tileMap, Dungeon d)
	{
		if (borderGrid.PitBorderIsInternal)
		{
			if (cell.type == CellType.PIT)
			{
				List<CellData> cellNeighbors = d.data.GetCellNeighbors(cell, true);
				bool flag = cellNeighbors[0] != null && cellNeighbors[0].type == CellType.PIT;
				bool flag2 = cellNeighbors[1] != null && cellNeighbors[1].type == CellType.PIT;
				bool flag3 = cellNeighbors[2] != null && cellNeighbors[2].type == CellType.PIT;
				bool flag4 = cellNeighbors[3] != null && cellNeighbors[3].type == CellType.PIT;
				bool flag5 = cellNeighbors[4] != null && cellNeighbors[4].type == CellType.PIT;
				bool flag6 = cellNeighbors[5] != null && cellNeighbors[5].type == CellType.PIT;
				bool flag7 = cellNeighbors[6] != null && cellNeighbors[6].type == CellType.PIT;
				bool flag8 = cellNeighbors[7] != null && cellNeighbors[7].type == CellType.PIT;
				int indexGivenSides = borderGrid.GetIndexGivenSides(!flag, !flag2, !flag3, !flag4, !flag5, !flag6, !flag7, !flag8);
				tileMapLayer.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, indexGivenSides);
			}
		}
		else if (cell.type == CellType.PIT)
		{
			List<CellData> cellNeighbors2 = d.data.GetCellNeighbors(cell, false);
			bool flag9 = cellNeighbors2[0] != null && cellNeighbors2[0].type == CellType.PIT;
			bool flag10 = cellNeighbors2[1] != null && cellNeighbors2[1].type == CellType.PIT;
			bool flag11 = cellNeighbors2[2] != null && cellNeighbors2[2].type == CellType.PIT;
			bool flag12 = cellNeighbors2[3] != null && cellNeighbors2[3].type == CellType.PIT;
			int internalIndexGivenSides = borderGrid.GetInternalIndexGivenSides(flag9, flag10, flag11, flag12);
			if (internalIndexGivenSides != -1)
			{
				tileMapLayer.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, internalIndexGivenSides);
			}
		}
		else
		{
			List<CellData> cellNeighbors3 = d.data.GetCellNeighbors(cell, true);
			bool flag13 = cellNeighbors3[0] != null && (cellNeighbors3[0].type == CellType.PIT || cellNeighbors3[0].cellVisualData.RequiresPitBordering);
			bool flag14 = cellNeighbors3[1] != null && (cellNeighbors3[1].type == CellType.PIT || cellNeighbors3[1].cellVisualData.RequiresPitBordering);
			bool flag15 = cellNeighbors3[2] != null && (cellNeighbors3[2].type == CellType.PIT || cellNeighbors3[2].cellVisualData.RequiresPitBordering);
			bool flag16 = cellNeighbors3[3] != null && (cellNeighbors3[3].type == CellType.PIT || cellNeighbors3[3].cellVisualData.RequiresPitBordering);
			bool flag17 = cellNeighbors3[4] != null && (cellNeighbors3[4].type == CellType.PIT || cellNeighbors3[4].cellVisualData.RequiresPitBordering);
			bool flag18 = cellNeighbors3[5] != null && (cellNeighbors3[5].type == CellType.PIT || cellNeighbors3[5].cellVisualData.RequiresPitBordering);
			bool flag19 = cellNeighbors3[6] != null && (cellNeighbors3[6].type == CellType.PIT || cellNeighbors3[6].cellVisualData.RequiresPitBordering);
			bool flag20 = cellNeighbors3[7] != null && (cellNeighbors3[7].type == CellType.PIT || cellNeighbors3[7].cellVisualData.RequiresPitBordering);
			if (!flag13 && !flag14 && !flag15 && !flag16 && !flag17 && !flag18 && !flag19 && !flag20)
			{
				return;
			}
			int indexGivenSides2 = borderGrid.GetIndexGivenSides(flag13, flag14, flag15, flag16, flag17, flag18, flag19, flag20);
			if (borderGrid.PitBorderOverridesFloorTile)
			{
				tileMap.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, GlobalDungeonData.floorLayerIndex, indexGivenSides2);
			}
			else
			{
				tileMapLayer.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, indexGivenSides2);
			}
			if (borderGrid.PitBorderOverridesFloorTile)
			{
				TileIndexGrid tileIndexGrid = d.roomMaterialDefinitions[cell.cellVisualData.roomVisualTypeIndex].pitLayoutGrid;
				if (tileIndexGrid == null)
				{
					tileIndexGrid = d.roomMaterialDefinitions[0].pitLayoutGrid;
				}
				tileMap.Layers[GlobalDungeonData.pitLayerIndex].SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, tileIndexGrid.centerIndices.GetIndexByWeight());
			}
		}
	}

	// Token: 0x06005414 RID: 21524 RVA: 0x001F46F0 File Offset: 0x001F28F0
	private void HandlePitTilePlacement(CellData cell, TileIndexGrid pitGrid, Layer tileMapLayer, Dungeon d)
	{
		if (pitGrid == null)
		{
			return;
		}
		List<CellData> cellNeighbors = d.data.GetCellNeighbors(cell, false);
		bool flag = cellNeighbors[0] != null && cellNeighbors[0].type == CellType.PIT;
		bool flag2 = cellNeighbors[1] != null && cellNeighbors[1].type == CellType.PIT;
		bool flag3 = cellNeighbors[2] != null && cellNeighbors[2].type == CellType.PIT;
		bool flag4 = cellNeighbors[3] != null && cellNeighbors[3].type == CellType.PIT;
		bool flag5 = this.BCheck(d, cell.position.x, cell.position.y + 2) && d.data.cellData[cell.position.x][cell.position.y + 2].type == CellType.PIT;
		bool flag6 = this.BCheck(d, cell.position.x, cell.position.y + 3) && d.data.cellData[cell.position.x][cell.position.y + 3].type == CellType.PIT;
		if (cell.cellVisualData.pitOverrideIndex > -1)
		{
			tileMapLayer.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, cell.cellVisualData.pitOverrideIndex);
		}
		else
		{
			if (GameManager.Instance.Dungeon.debugSettings.WALLS_ARE_PITS)
			{
				if (cellNeighbors[2] != null && cellNeighbors[2].isExitCell)
				{
					flag3 = true;
				}
				if (cellNeighbors[0] != null && cellNeighbors[0].isExitCell)
				{
					flag = true;
				}
				if (this.BCheck(d, cell.position.x, cell.position.y + 2) && d.data.cellData[cell.position.x][cell.position.y + 2].isExitCell)
				{
					flag5 = true;
				}
				if (this.BCheck(d, cell.position.x, cell.position.y + 3) && d.data.cellData[cell.position.x][cell.position.y + 3].isExitCell)
				{
					flag6 = true;
				}
			}
			int num = pitGrid.GetIndexGivenSides(!flag, !flag5, !flag6, !flag2, !flag3, !flag4);
			if (pitGrid.PitInternalSquareGrids.Count > 0 && UnityEngine.Random.value < pitGrid.PitInternalSquareOptions.PitSquareChance && (pitGrid.PitInternalSquareOptions.CanBeFlushLeft || flag4) && (pitGrid.PitInternalSquareOptions.CanBeFlushBottom || flag3) && flag2 && flag && flag5 && flag6)
			{
				bool flag7 = this.BCheck(d, cell.position.x + 2, cell.position.y) && d.data.cellData[cell.position.x + 2][cell.position.y].type == CellType.PIT;
				bool flag8 = this.BCheck(d, cell.position.x + 1, cell.position.y + 1) && d.data.cellData[cell.position.x + 1][cell.position.y + 1].type == CellType.PIT;
				bool flag9 = this.BCheck(d, cell.position.x + 1, cell.position.y + 2) && d.data.cellData[cell.position.x + 1][cell.position.y + 2].type == CellType.PIT;
				bool flag10 = this.BCheck(d, cell.position.x + 1, cell.position.y + 3) && d.data.cellData[cell.position.x + 1][cell.position.y + 3].type == CellType.PIT;
				if ((pitGrid.PitInternalSquareOptions.CanBeFlushRight || flag7) && flag8 && flag10 && flag9)
				{
					TileIndexGrid tileIndexGrid = pitGrid.PitInternalSquareGrids[UnityEngine.Random.Range(0, pitGrid.PitInternalSquareGrids.Count)];
					num = tileIndexGrid.bottomLeftIndices.GetIndexByWeight();
					d.data.cellData[cell.position.x + 1][cell.position.y].cellVisualData.pitOverrideIndex = tileIndexGrid.bottomRightIndices.GetIndexByWeight();
					d.data.cellData[cell.position.x][cell.position.y + 1].cellVisualData.pitOverrideIndex = tileIndexGrid.topLeftIndices.GetIndexByWeight();
					d.data.cellData[cell.position.x + 1][cell.position.y + 1].cellVisualData.pitOverrideIndex = tileIndexGrid.topRightIndices.GetIndexByWeight();
				}
			}
			tileMapLayer.SetTile(cell.positionInTilemap.x, cell.positionInTilemap.y, num);
		}
		if (flag && !flag5)
		{
			this.AssignColorGradientToTile(cell.positionInTilemap.x, cell.positionInTilemap.y, GlobalDungeonData.pitLayerIndex, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 1f), GameManager.Instance.Dungeon.MainTilemap);
		}
	}

	// Token: 0x04004CCB RID: 19659
	private TileIndices t;

	// Token: 0x04004CCC RID: 19660
	private Dictionary<TilesetIndexMetadata.TilesetFlagType, List<Tuple<int, TilesetIndexMetadata>>> m_metadataLookupTable;
}
