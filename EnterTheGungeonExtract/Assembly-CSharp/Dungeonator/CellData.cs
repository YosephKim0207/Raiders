using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000EB2 RID: 3762
	public class CellData
	{
		// Token: 0x06004F89 RID: 20361 RVA: 0x001B9D0C File Offset: 0x001B7F0C
		public CellData(int x, int y, CellType t = CellType.WALL)
		{
			this.position = new IntVector2(x, y);
			this.positionInTilemap = new IntVector2(x, y);
			this.type = t;
			this.cellVisualData = default(CellVisualData);
			this.cellVisualData.distanceToNearestLight = 100;
			this.cellVisualData.faceWallOverrideIndex = -1;
			this.cellVisualData.pitOverrideIndex = -1;
			this.cellVisualData.inheritedOverrideIndex = -1;
			this.cellVisualData.pathTilesetGridIndex = -1;
			this.cellVisualData.forcedMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY;
			this.cellVisualData.RatChunkBorderIndex = -1;
			this.occlusionData = new CellOcclusionData(this);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x001B9DD0 File Offset: 0x001B7FD0
		public CellData(IntVector2 p, CellType t = CellType.WALL)
		{
			this.position = p;
			this.positionInTilemap = p;
			this.type = t;
			this.cellVisualData = default(CellVisualData);
			this.cellVisualData.distanceToNearestLight = 100;
			this.cellVisualData.faceWallOverrideIndex = -1;
			this.cellVisualData.pitOverrideIndex = -1;
			this.cellVisualData.inheritedOverrideIndex = -1;
			this.cellVisualData.pathTilesetGridIndex = -1;
			this.cellVisualData.forcedMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY;
			this.cellVisualData.RatChunkBorderIndex = -1;
			this.occlusionData = new CellOcclusionData(this);
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001B9E88 File Offset: 0x001B8088
		public bool isNextToWall
		{
			get
			{
				if (this.m_isNextToWall == null)
				{
					this.m_isNextToWall = new bool?(this.HasWallNeighbor(true, false));
				}
				return this.m_isNextToWall.Value;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06004F8C RID: 20364 RVA: 0x001B9EB8 File Offset: 0x001B80B8
		// (set) Token: 0x06004F8D RID: 20365 RVA: 0x001B9EC0 File Offset: 0x001B80C0
		public bool cachedCanContainTeleporter { get; set; }

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x001B9ECC File Offset: 0x001B80CC
		public bool IsPassable
		{
			get
			{
				return !this.isOccupied && this.type == CellType.FLOOR;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x001B9EE8 File Offset: 0x001B80E8
		public float UniqueHash
		{
			get
			{
				int num = 0;
				num += this.position.x;
				num += num << 10;
				num ^= num >> 6;
				num += this.position.y;
				num += num << 10;
				num ^= num >> 6;
				num += num << 3;
				num ^= num >> 11;
				num += num << 15;
				uint num2 = (uint)num;
				return num2 * 1f / 4.2949673E+09f;
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x001B9F54 File Offset: 0x001B8154
		public bool HasPhantomCarpetNeighbor(bool includeDiagonals = true)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<CellData> cellNeighbors = data.GetCellNeighbors(this, includeDiagonals);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (cellNeighbors[i] != null)
				{
					if (cellNeighbors[i].cellVisualData.IsPhantomCarpet)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001B9FBC File Offset: 0x001B81BC
		public bool SurroundedByPits(bool includeDiagonals = true)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<CellData> cellNeighbors = data.GetCellNeighbors(this, includeDiagonals);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (cellNeighbors[i] != null)
				{
					if (cellNeighbors[i].type != CellType.PIT)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x001BA020 File Offset: 0x001B8220
		public bool HasFloorNeighbor(DungeonData d, bool includeTopwalls = false, bool includeDiagonals = false)
		{
			List<CellData> cellNeighbors = d.GetCellNeighbors(this, includeDiagonals);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (cellNeighbors[i] != null)
				{
					if (cellNeighbors[i].type == CellType.FLOOR && (includeTopwalls || !cellNeighbors[i].IsTopWall()))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001BA08C File Offset: 0x001B828C
		public bool HasWallNeighbor(bool includeDiagonals = true, bool includeTopwalls = true)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<CellData> cellNeighbors = data.GetCellNeighbors(this, includeDiagonals);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (cellNeighbors[i] != null)
				{
					if (!includeTopwalls)
					{
						if (includeDiagonals)
						{
							if (i >= 3 && i <= 5)
							{
								goto IL_75;
							}
						}
						else if (i == 2)
						{
							goto IL_75;
						}
					}
					if (cellNeighbors[i].type == CellType.WALL)
					{
						return true;
					}
				}
				IL_75:;
			}
			return false;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001BA120 File Offset: 0x001B8320
		public bool HasPitNeighbor(DungeonData d)
		{
			List<CellData> cellNeighbors = d.GetCellNeighbors(this, true);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (cellNeighbors[i] != null)
				{
					if (cellNeighbors[i].type == CellType.PIT)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001BA174 File Offset: 0x001B8374
		public PrototypeRoomPitEntry.PitBorderType GetPitBorderType(DungeonData d)
		{
			if (this.parentArea != null && this.parentArea.IsProceduralRoom)
			{
				return PrototypeRoomPitEntry.PitBorderType.FLAT;
			}
			if (this.type == CellType.PIT)
			{
				PrototypeRoomPitEntry prototypeRoomPitEntry = null;
				if (this.parentArea != null && !this.parentArea.IsProceduralRoom)
				{
					prototypeRoomPitEntry = this.parentArea.prototypeRoom.GetPitEntryFromPosition(this.position - this.parentArea.basePosition + IntVector2.One);
				}
				if (prototypeRoomPitEntry != null)
				{
					return prototypeRoomPitEntry.borderType;
				}
			}
			else if (this.type != CellType.WALL || this.breakable)
			{
				foreach (CellData cellData in d.GetCellNeighbors(this, true))
				{
					if (this.parentArea != null && cellData != null && cellData.parentArea != null && !(cellData.parentArea.prototypeRoom == null))
					{
						if (cellData.type == CellType.PIT)
						{
							PrototypeRoomPitEntry pitEntryFromPosition = cellData.parentArea.prototypeRoom.GetPitEntryFromPosition(cellData.position - this.parentArea.basePosition + IntVector2.One);
							if (pitEntryFromPosition != null)
							{
								return pitEntryFromPosition.borderType;
							}
						}
					}
				}
				return PrototypeRoomPitEntry.PitBorderType.NONE;
			}
			return PrototypeRoomPitEntry.PitBorderType.NONE;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001BA2F8 File Offset: 0x001B84F8
		public bool IsSideWallAdjacent()
		{
			return this.type != CellType.WALL && (GameManager.Instance.Dungeon.data[this.position + IntVector2.Right].type == CellType.WALL || GameManager.Instance.Dungeon.data[this.position + IntVector2.Left].type == CellType.WALL);
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001BA374 File Offset: 0x001B8574
		public bool IsLowerFaceWall()
		{
			if (!Dungeon.IsGenerating)
			{
				if (!this.m_hasCachedLowerFacewallness)
				{
					this.m_lowerFacewallness = GameManager.Instance.Dungeon.data.isFaceWallLower(this.position.x, this.position.y);
					this.m_hasCachedLowerFacewallness = true;
				}
				return this.m_lowerFacewallness;
			}
			return GameManager.Instance.Dungeon.data.isFaceWallLower(this.position.x, this.position.y);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x001BA400 File Offset: 0x001B8600
		public bool IsUpperFacewall()
		{
			if (!Dungeon.IsGenerating)
			{
				if (!this.m_hasCachedUpperFacewallness)
				{
					this.m_upperFacewallness = GameManager.Instance.Dungeon.data.isFaceWallHigher(this.position.x, this.position.y);
					this.m_hasCachedUpperFacewallness = true;
				}
				return this.m_upperFacewallness;
			}
			return GameManager.Instance.Dungeon.data.isFaceWallHigher(this.position.x, this.position.y);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x001BA48C File Offset: 0x001B868C
		public bool IsAnyFaceWall()
		{
			if (this.m_cachedFacewallness != null)
			{
				return this.m_cachedFacewallness.Value;
			}
			bool flag = GameManager.Instance.Dungeon.data.isAnyFaceWall(this.position.x, this.position.y);
			if (!Dungeon.IsGenerating)
			{
				this.m_cachedFacewallness = new bool?(flag);
			}
			return flag;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x001BA4F8 File Offset: 0x001B86F8
		public bool IsTopWall()
		{
			return GameManager.Instance.Dungeon.data.isTopWall(this.position.x, this.position.y);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x001BA524 File Offset: 0x001B8724
		public bool HasPassableNeighbor(DungeonData d)
		{
			foreach (CellData cellData in d.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (cellData.IsPassable)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x001BA59C File Offset: 0x001B879C
		public CellData GetExitNeighbor()
		{
			foreach (CellData cellData in GameManager.Instance.Dungeon.data.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (cellData.isExitCell)
					{
						return cellData;
					}
				}
			}
			return null;
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x001BA624 File Offset: 0x001B8824
		public bool HasNonTopWallWallNeighbor()
		{
			List<CellData> cellNeighbors = GameManager.Instance.Dungeon.data.GetCellNeighbors(this, true);
			for (int i = 0; i < cellNeighbors.Count; i++)
			{
				if (i < 3 || i > 5)
				{
					if (cellNeighbors[i].type == CellType.WALL)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x001BA688 File Offset: 0x001B8888
		public bool HasTypeNeighbor(DungeonData d, CellType t)
		{
			foreach (CellData cellData in d.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (cellData.type == t)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x001BA700 File Offset: 0x001B8900
		public bool HasFaceWallNeighbor(DungeonData d)
		{
			foreach (CellData cellData in d.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (d.isAnyFaceWall(cellData.position.x, cellData.position.y))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x001BA790 File Offset: 0x001B8990
		public bool HasMossyNeighbor(DungeonData d)
		{
			if (this.type == CellType.WALL)
			{
				return false;
			}
			foreach (CellData cellData in d.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (cellData.type == CellType.WALL || cellData.cellVisualData.isDecal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x001BA828 File Offset: 0x001B8A28
		public bool HasPatternNeighbor(DungeonData d)
		{
			if (this.type == CellType.WALL)
			{
				return false;
			}
			foreach (CellData cellData in d.GetCellNeighbors(this, false))
			{
				if (cellData != null)
				{
					if (cellData.type == CellType.WALL || cellData.cellVisualData.isPattern)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004742 RID: 18242
		public IntVector2 position;

		// Token: 0x04004743 RID: 18243
		public CellType type;

		// Token: 0x04004744 RID: 18244
		public DiagonalWallType diagonalWallType;

		// Token: 0x04004745 RID: 18245
		public IntVector2 positionInTilemap;

		// Token: 0x04004746 RID: 18246
		public bool breakable;

		// Token: 0x04004747 RID: 18247
		public bool fallingPrevented;

		// Token: 0x04004748 RID: 18248
		public List<SpeculativeRigidbody> platforms;

		// Token: 0x04004749 RID: 18249
		public bool containsTrap;

		// Token: 0x0400474A RID: 18250
		public bool forceAllowGoop;

		// Token: 0x0400474B RID: 18251
		public bool forceDisallowGoop;

		// Token: 0x0400474C RID: 18252
		public bool isWallMimicHideout;

		// Token: 0x0400474D RID: 18253
		public bool doesDamage;

		// Token: 0x0400474E RID: 18254
		public CellDamageDefinition damageDefinition;

		// Token: 0x0400474F RID: 18255
		public bool isExitCell;

		// Token: 0x04004750 RID: 18256
		public DungeonData.Direction exitDirection;

		// Token: 0x04004751 RID: 18257
		public bool isDoorFrameCell;

		// Token: 0x04004752 RID: 18258
		public bool isExitNonOccluder;

		// Token: 0x04004753 RID: 18259
		public DungeonDoorController exitDoor;

		// Token: 0x04004754 RID: 18260
		public RoomHandler connectedRoom1;

		// Token: 0x04004755 RID: 18261
		public RoomHandler connectedRoom2;

		// Token: 0x04004756 RID: 18262
		public bool isSecretRoomCell;

		// Token: 0x04004757 RID: 18263
		public RoomHandler nearestRoom;

		// Token: 0x04004758 RID: 18264
		public float distanceFromNearestRoom = float.MaxValue;

		// Token: 0x04004759 RID: 18265
		public CellVisualData cellVisualData;

		// Token: 0x0400475A RID: 18266
		public bool isOccupied;

		// Token: 0x0400475B RID: 18267
		public bool isOccludedByTopWall;

		// Token: 0x0400475C RID: 18268
		private bool? m_isNextToWall;

		// Token: 0x0400475D RID: 18269
		public CellOcclusionData occlusionData;

		// Token: 0x0400475E RID: 18270
		public CellArea parentArea;

		// Token: 0x0400475F RID: 18271
		public RoomHandler parentRoom;

		// Token: 0x04004760 RID: 18272
		[NonSerialized]
		public RoomHandler targetPitfallRoom;

		// Token: 0x04004761 RID: 18273
		public bool hasBeenGenerated;

		// Token: 0x04004762 RID: 18274
		public bool isRoomInternal = true;

		// Token: 0x04004763 RID: 18275
		public bool isGridConnected;

		// Token: 0x04004764 RID: 18276
		public float lastSplashTime = -1f;

		// Token: 0x04004765 RID: 18277
		public bool IsFireplaceCell;

		// Token: 0x04004766 RID: 18278
		public bool PreventRewardSpawn;

		// Token: 0x04004767 RID: 18279
		public bool IsTrapZone;

		// Token: 0x04004768 RID: 18280
		public bool IsPlayerInaccessible;

		// Token: 0x04004769 RID: 18281
		public Action<CellData> OnCellGooped;

		// Token: 0x0400476B RID: 18283
		public bool HasCachedPhysicsTile;

		// Token: 0x0400476C RID: 18284
		public PhysicsEngine.Tile CachedPhysicsTile;

		// Token: 0x0400476D RID: 18285
		[NonSerialized]
		private bool m_hasCachedUpperFacewallness;

		// Token: 0x0400476E RID: 18286
		[NonSerialized]
		private bool m_upperFacewallness;

		// Token: 0x0400476F RID: 18287
		[NonSerialized]
		private bool m_hasCachedLowerFacewallness;

		// Token: 0x04004770 RID: 18288
		[NonSerialized]
		private bool m_lowerFacewallness;

		// Token: 0x04004771 RID: 18289
		private bool? m_cachedFacewallness;
	}
}
