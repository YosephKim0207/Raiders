using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F5D RID: 3933
	public class RuntimeExitDefinition
	{
		// Token: 0x060054AD RID: 21677 RVA: 0x001FCF78 File Offset: 0x001FB178
		public RuntimeExitDefinition(RuntimeRoomExitData uExit, RuntimeRoomExitData dExit, RoomHandler upstream, RoomHandler downstream)
		{
			if (upstream.distanceFromEntrance <= downstream.distanceFromEntrance || downstream.area.IsProceduralRoom)
			{
				this.upstreamExit = uExit;
				this.downstreamExit = dExit;
				this.upstreamRoom = upstream;
				this.downstreamRoom = downstream;
			}
			else
			{
				this.upstreamExit = dExit;
				this.downstreamExit = uExit;
				this.upstreamRoom = downstream;
				this.downstreamRoom = upstream;
			}
			if (upstream.IsOnCriticalPath && downstream.IsOnCriticalPath)
			{
				this.isCriticalPath = true;
				if (uExit != null)
				{
					uExit.isCriticalPath = true;
				}
				if (dExit != null)
				{
					dExit.isCriticalPath = true;
				}
			}
			this.CalculateCellData();
			if (this.isCriticalPath)
			{
				if (this.upstreamExit != null && this.upstreamExit.referencedExit != null)
				{
					BraveUtility.DrawDebugSquare((this.upstreamExit.referencedExit.GetExitOrigin(0) - IntVector2.One + this.upstreamRoom.area.basePosition + -3 * DungeonData.GetIntVector2FromDirection(this.upstreamExit.referencedExit.exitDirection)).ToVector2(), Color.red, 1000f);
				}
				if (this.downstreamExit != null && this.downstreamExit.referencedExit != null)
				{
					BraveUtility.DrawDebugSquare((this.downstreamExit.referencedExit.GetExitOrigin(0) - IntVector2.One + this.downstreamRoom.area.basePosition + -3 * DungeonData.GetIntVector2FromDirection(this.downstreamExit.referencedExit.exitDirection)).ToVector2(), Color.blue, 1000f);
				}
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060054AE RID: 21678 RVA: 0x001FD13C File Offset: 0x001FB33C
		public RoomHandler.VisibilityStatus Visibility
		{
			get
			{
				if (this.IsVisibleFromRoom(GameManager.Instance.PrimaryPlayer.CurrentRoom) || this.ExitOccluderCells.Contains(GameManager.Instance.PrimaryPlayer.transform.position.IntXY(VectorConversions.Floor)))
				{
					return RoomHandler.VisibilityStatus.CURRENT;
				}
				return RoomHandler.VisibilityStatus.OBSCURED;
			}
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001FD190 File Offset: 0x001FB390
		public bool ContainsPosition(IntVector2 position)
		{
			return (this.m_upstreamCells != null && this.m_upstreamCells.Contains(position)) || (this.m_downstreamCells != null && this.m_downstreamCells.Contains(position));
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001FD1CC File Offset: 0x001FB3CC
		public void RemovePosition(IntVector2 position)
		{
			if (this.m_upstreamCells != null)
			{
				this.m_upstreamCells.Remove(position);
			}
			if (this.m_downstreamCells != null)
			{
				this.m_downstreamCells.Remove(position);
			}
			if (this.IntermediaryCells != null)
			{
				this.IntermediaryCells.Remove(position);
			}
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x001FD224 File Offset: 0x001FB424
		public IntVector2 GetLinearMidpoint(RoomHandler baseRoom)
		{
			if (baseRoom == this.upstreamRoom)
			{
				if (this.upstreamExit.jointedExit || this.downstreamExit == null)
				{
					return this.upstreamExit.ExitOrigin - IntVector2.One;
				}
				int num = (this.upstreamExit.TotalExitLength + this.downstreamExit.TotalExitLength) / 2;
				return this.upstreamExit.referencedExit.GetExitOrigin(num) - IntVector2.One;
			}
			else
			{
				if (baseRoom != this.downstreamRoom)
				{
					Debug.LogError("SHOULD NEVER OCCUR. LIGHTING PLACEMENT ERROR.");
					return IntVector2.Zero;
				}
				if (this.downstreamExit.jointedExit || this.upstreamExit == null)
				{
					return this.downstreamExit.ExitOrigin - IntVector2.One;
				}
				int num2 = (this.upstreamExit.TotalExitLength + this.downstreamExit.TotalExitLength) / 2;
				return this.downstreamExit.referencedExit.GetExitOrigin(num2) - IntVector2.One;
			}
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x001FD328 File Offset: 0x001FB528
		public DungeonData.Direction GetDirectionFromRoom(RoomHandler sourceRoom)
		{
			if (sourceRoom == this.upstreamRoom)
			{
				if (this.upstreamExit == null || this.upstreamExit.referencedExit == null)
				{
					return (this.downstreamExit.referencedExit.exitDirection + 4) % (DungeonData.Direction)8;
				}
				return this.upstreamExit.referencedExit.exitDirection;
			}
			else
			{
				if (sourceRoom != this.downstreamRoom)
				{
					Debug.LogError("This should never happen.");
					return (DungeonData.Direction)(-1);
				}
				if (this.downstreamExit == null || this.downstreamExit.referencedExit == null)
				{
					return (this.upstreamExit.referencedExit.exitDirection + 4) % (DungeonData.Direction)8;
				}
				return this.downstreamExit.referencedExit.exitDirection;
			}
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x001FD3DC File Offset: 0x001FB5DC
		public void GetExitLine(RoomHandler sourceRoom, out Vector2 p1, out Vector2 p2)
		{
			p1 = sourceRoom.GetCellAdjacentToExit(this).ToVector2();
			DungeonData.Direction directionFromRoom = this.GetDirectionFromRoom(sourceRoom);
			if (directionFromRoom == DungeonData.Direction.NORTH)
			{
				p1 += new Vector2(0f, 1f);
				p2 = p1 + new Vector2(2f, 0f);
			}
			else if (directionFromRoom == DungeonData.Direction.EAST)
			{
				p1 += new Vector2(1f, 0f);
				p2 = p1 + new Vector2(0f, 2f);
			}
			else if (directionFromRoom == DungeonData.Direction.SOUTH)
			{
				p2 = p1 + new Vector2(2f, 0f);
			}
			else if (directionFromRoom == DungeonData.Direction.WEST)
			{
				p2 = p1 + new Vector2(0f, 2f);
			}
			else
			{
				Debug.LogError("This should never happen.");
				p2 = Vector2.zero;
			}
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x001FD50C File Offset: 0x001FB70C
		public HashSet<IntVector2> GetCellsForRoom(RoomHandler r)
		{
			if (r == this.upstreamRoom)
			{
				return this.m_upstreamCells;
			}
			if (r == this.downstreamRoom)
			{
				return this.m_downstreamCells;
			}
			return null;
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x001FD538 File Offset: 0x001FB738
		public HashSet<IntVector2> GetCellsForOtherRoom(RoomHandler r)
		{
			if (r == this.upstreamRoom)
			{
				return this.m_downstreamCells;
			}
			if (r == this.downstreamRoom)
			{
				return this.m_upstreamCells;
			}
			return null;
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x001FD564 File Offset: 0x001FB764
		private void PlaceExitDecorables(RoomHandler targetRoom, RuntimeRoomExitData targetExit, RoomHandler otherRoom)
		{
			if (otherRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL && otherRoom.area.prototypeRoom.subCategorySpecial == PrototypeDungeonRoom.RoomSpecialSubCategory.WEIRD_SHOP)
			{
				otherRoom.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Purple_Lantern") as GameObject;
			}
			GameObject gameObject = ((!(otherRoom.area.prototypeRoom != null) || !(otherRoom.area.prototypeRoom.doorTopDecorable != null)) ? otherRoom.OptionalDoorTopDecorable : otherRoom.area.prototypeRoom.doorTopDecorable);
			if (targetExit != null && targetExit.referencedExit != null && targetRoom != null && otherRoom != null && !targetRoom.IsSecretRoom && !otherRoom.IsSecretRoom && otherRoom.area.prototypeRoom != null && gameObject != null)
			{
				IntVector2 intVector = targetExit.referencedExit.GetExitOrigin(0) - IntVector2.One + targetRoom.area.basePosition + -3 * DungeonData.GetIntVector2FromDirection(targetExit.referencedExit.exitDirection);
				Vector2 vector = intVector.ToVector2();
				Vector2 vector2 = intVector.ToVector2();
				switch (targetExit.referencedExit.exitDirection)
				{
				case DungeonData.Direction.NORTH:
					vector += new Vector2(-1.5f, 3.5f);
					vector2 += new Vector2(2.5f, 3.5f);
					break;
				case DungeonData.Direction.EAST:
					vector += new Vector2(1.5f, -0.5f);
					vector2 += new Vector2(1.5f, 4.5f);
					break;
				case DungeonData.Direction.SOUTH:
					vector += new Vector2(-1.5f, 0.5f);
					vector2 += new Vector2(2.5f, 0.5f);
					break;
				case DungeonData.Direction.WEST:
					vector += new Vector2(-1.5f, -0.5f);
					vector2 += new Vector2(-1.5f, 4.5f);
					break;
				}
				if (UnityEngine.Random.value < 0.4f)
				{
					UnityEngine.Object.Instantiate<GameObject>(gameObject, vector.ToVector3ZUp(0f) + gameObject.transform.position, Quaternion.identity);
				}
				else if (UnityEngine.Random.value < 0.8f)
				{
					UnityEngine.Object.Instantiate<GameObject>(gameObject, vector2.ToVector3ZUp(0f) + gameObject.transform.position, Quaternion.identity);
				}
				else
				{
					UnityEngine.Object.Instantiate<GameObject>(gameObject, vector.ToVector3ZUp(0f) + gameObject.transform.position, Quaternion.identity);
					UnityEngine.Object.Instantiate<GameObject>(gameObject, vector2.ToVector3ZUp(0f) + gameObject.transform.position, Quaternion.identity);
				}
			}
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x001FD860 File Offset: 0x001FBA60
		public void ProcessWestgeonData()
		{
			if (this.m_westgeonProcessed)
			{
				return;
			}
			this.m_westgeonProcessed = true;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
			{
				if (this.upstreamExit != null && this.upstreamExit.referencedExit != null && this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH && this.upstreamRoom.RoomVisualSubtype == 0 && this.downstreamRoom.RoomVisualSubtype != 0)
				{
					IntVector2 intVector = this.upstreamExit.referencedExit.GetExitOrigin(0) - IntVector2.One + this.upstreamRoom.area.basePosition + -2 * DungeonData.GetIntVector2FromDirection(this.upstreamExit.referencedExit.exitDirection);
					this.ProcessWestgeonSection(intVector, this.downstreamRoom);
				}
				if (this.downstreamExit != null && this.downstreamExit.referencedExit != null && this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH && this.downstreamRoom.RoomVisualSubtype == 0 && this.upstreamRoom.RoomVisualSubtype != 0)
				{
					IntVector2 intVector2 = this.downstreamExit.referencedExit.GetExitOrigin(0) - IntVector2.One + this.downstreamRoom.area.basePosition + -2 * DungeonData.GetIntVector2FromDirection(this.downstreamExit.referencedExit.exitDirection);
					this.ProcessWestgeonSection(intVector2, this.upstreamRoom);
				}
			}
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x001FD9FC File Offset: 0x001FBBFC
		private void ProcessWestgeonSection(IntVector2 exitConnection, RoomHandler inheritRoom)
		{
			IntVector2 intVector = exitConnection + IntVector2.Left;
			IntVector2 intVector2 = exitConnection + IntVector2.Right * 2;
			IntVector2 intVector3 = intVector;
			IntVector2 intVector4 = intVector2;
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			int num = -1;
			while (cellData != null && cellData.IsLowerFaceWall())
			{
				intVector3 = cellData.position;
				cellData.cellVisualData.IsFacewallForInteriorTransition = true;
				cellData.cellVisualData.InteriorTransitionIndex = inheritRoom.RoomVisualSubtype;
				num = inheritRoom.RoomVisualSubtype;
				CellData cellData2 = GameManager.Instance.Dungeon.data[cellData.position + IntVector2.Up];
				while (cellData2 != null && (cellData2.IsUpperFacewall() || cellData2.type == CellType.WALL) && (cellData2.nearestRoom == this.upstreamRoom || cellData2.nearestRoom == this.downstreamRoom))
				{
					cellData2.cellVisualData.IsFacewallForInteriorTransition = true;
					cellData2.cellVisualData.InteriorTransitionIndex = inheritRoom.RoomVisualSubtype;
					if (!GameManager.Instance.Dungeon.data.CheckInBounds(cellData2.position + IntVector2.Up))
					{
						break;
					}
					cellData2 = GameManager.Instance.Dungeon.data[cellData2.position + IntVector2.Up];
				}
				cellData = GameManager.Instance.Dungeon.data[cellData.position + IntVector2.Left];
			}
			cellData = GameManager.Instance.Dungeon.data[intVector2];
			while (cellData != null && cellData.IsLowerFaceWall())
			{
				intVector4 = cellData.position;
				cellData.cellVisualData.IsFacewallForInteriorTransition = true;
				cellData.cellVisualData.InteriorTransitionIndex = inheritRoom.RoomVisualSubtype;
				num = inheritRoom.RoomVisualSubtype;
				CellData cellData3 = GameManager.Instance.Dungeon.data[cellData.position + IntVector2.Up];
				while (cellData3 != null && (cellData3.IsUpperFacewall() || cellData3.type == CellType.WALL) && (cellData3.nearestRoom == this.upstreamRoom || cellData3.nearestRoom == this.downstreamRoom))
				{
					cellData3.cellVisualData.IsFacewallForInteriorTransition = true;
					cellData3.cellVisualData.InteriorTransitionIndex = inheritRoom.RoomVisualSubtype;
					if (!GameManager.Instance.Dungeon.data.CheckInBounds(cellData3.position + IntVector2.Up))
					{
						break;
					}
					cellData3 = GameManager.Instance.Dungeon.data[cellData3.position + IntVector2.Up];
				}
				cellData = GameManager.Instance.Dungeon.data[cellData.position + IntVector2.Right];
			}
			if (num != -1)
			{
				intVector3 += IntVector2.Down;
				intVector4 += IntVector2.Down;
				for (int i = intVector3.x; i <= intVector4.x; i++)
				{
					GameManager.Instance.Dungeon.data[i, intVector4.y].cellVisualData.IsFacewallForInteriorTransition = true;
					GameManager.Instance.Dungeon.data[i, intVector4.y].cellVisualData.InteriorTransitionIndex = num;
				}
			}
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001FDD98 File Offset: 0x001FBF98
		private bool CheckRowIsFloor(int minX, int maxX, int iy)
		{
			for (int i = minX; i <= maxX; i++)
			{
				if (GameManager.Instance.Dungeon.data[i, iy].type != CellType.FLOOR)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001FDDDC File Offset: 0x001FBFDC
		public IntVector2 GetDownstreamBasePosition()
		{
			if (this.upstreamRoom.area.IsProceduralRoom)
			{
				return this.GetUpstreamBasePosition();
			}
			IntVector2 intVector = IntVector2.Zero;
			switch (this.upstreamExit.referencedExit.exitDirection)
			{
			case DungeonData.Direction.NORTH:
				intVector = new IntVector2(int.MaxValue, int.MinValue);
				foreach (IntVector2 intVector2 in this.m_downstreamCells)
				{
					intVector = new IntVector2(Mathf.Min(intVector.x, intVector2.x), Mathf.Max(intVector.y, intVector2.y));
				}
				intVector += IntVector2.Up;
				break;
			case DungeonData.Direction.EAST:
				intVector = new IntVector2(int.MinValue, int.MaxValue);
				foreach (IntVector2 intVector3 in this.m_downstreamCells)
				{
					intVector = new IntVector2(Mathf.Max(intVector.x, intVector3.x), Mathf.Min(intVector.y, intVector3.y));
				}
				break;
			case DungeonData.Direction.SOUTH:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector4 in this.m_downstreamCells)
				{
					intVector = IntVector2.Min(intVector4, intVector);
				}
				break;
			case DungeonData.Direction.WEST:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector5 in this.m_downstreamCells)
				{
					intVector = IntVector2.Min(intVector5, intVector);
				}
				break;
			}
			return intVector;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001FE034 File Offset: 0x001FC234
		public IntVector2 GetUpstreamBasePosition()
		{
			if (this.downstreamExit == null || this.downstreamExit.referencedExit == null)
			{
				return this.GetDownstreamBasePosition();
			}
			IntVector2 intVector = IntVector2.Zero;
			switch (this.downstreamExit.referencedExit.exitDirection)
			{
			case DungeonData.Direction.NORTH:
				intVector = new IntVector2(int.MaxValue, int.MinValue);
				foreach (IntVector2 intVector2 in this.m_upstreamCells)
				{
					intVector = new IntVector2(Mathf.Min(intVector.x, intVector2.x), Mathf.Max(intVector.y, intVector2.y));
				}
				intVector += IntVector2.Up;
				break;
			case DungeonData.Direction.EAST:
				intVector = new IntVector2(int.MinValue, int.MaxValue);
				foreach (IntVector2 intVector3 in this.m_upstreamCells)
				{
					intVector = new IntVector2(Mathf.Max(intVector.x, intVector3.x), Mathf.Min(intVector.y, intVector3.y));
				}
				break;
			case DungeonData.Direction.SOUTH:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector4 in this.m_upstreamCells)
				{
					intVector = IntVector2.Min(intVector4, intVector);
				}
				break;
			case DungeonData.Direction.WEST:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector5 in this.m_upstreamCells)
				{
					intVector = IntVector2.Min(intVector5, intVector);
				}
				break;
			}
			return intVector;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x001FE294 File Offset: 0x001FC494
		public IntVector2 GetDownstreamNearDoorPosition()
		{
			if (this.upstreamRoom.area.IsProceduralRoom)
			{
				return this.GetUpstreamNearDoorPosition();
			}
			if (this.downstreamExit == null || this.downstreamExit.referencedExit == null)
			{
				return this.GetUpstreamNearDoorPosition();
			}
			IntVector2 intVector = IntVector2.Zero;
			switch (this.downstreamExit.referencedExit.exitDirection)
			{
			case DungeonData.Direction.NORTH:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector2 in this.m_downstreamCells)
				{
					if (intVector2.y < intVector.y || (intVector2.y == intVector.y && intVector2.x < intVector.x))
					{
						intVector = intVector2;
					}
				}
				break;
			case DungeonData.Direction.EAST:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector3 in this.m_downstreamCells)
				{
					if (intVector3.x < intVector.x || (intVector3.x == intVector.x && intVector3.y < intVector.y))
					{
						intVector = intVector3;
					}
				}
				break;
			case DungeonData.Direction.SOUTH:
				intVector = new IntVector2(int.MaxValue, int.MinValue);
				foreach (IntVector2 intVector4 in this.m_downstreamCells)
				{
					if (intVector4.y > intVector.y || (intVector4.y == intVector.y && intVector4.x < intVector.x))
					{
						intVector = intVector4;
					}
				}
				break;
			case DungeonData.Direction.WEST:
				intVector = new IntVector2(int.MinValue, int.MaxValue);
				foreach (IntVector2 intVector5 in this.m_downstreamCells)
				{
					if (intVector5.x > intVector.x || (intVector5.x == intVector.x && intVector5.y < intVector.y))
					{
						intVector = intVector5;
					}
				}
				break;
			}
			return intVector;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x001FE584 File Offset: 0x001FC784
		public IntVector2 GetUpstreamNearDoorPosition()
		{
			if (this.upstreamExit == null || this.upstreamExit.referencedExit == null)
			{
				return this.GetDownstreamNearDoorPosition();
			}
			IntVector2 intVector = IntVector2.Zero;
			switch (this.upstreamExit.referencedExit.exitDirection)
			{
			case DungeonData.Direction.NORTH:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector2 in this.m_upstreamCells)
				{
					if (intVector2.y < intVector.y || (intVector2.y == intVector.y && intVector2.x < intVector.x))
					{
						intVector = intVector2;
					}
				}
				break;
			case DungeonData.Direction.EAST:
				intVector = new IntVector2(int.MaxValue, int.MaxValue);
				foreach (IntVector2 intVector3 in this.m_upstreamCells)
				{
					if (intVector3.x < intVector.x || (intVector3.x == intVector.x && intVector3.y < intVector.y))
					{
						intVector = intVector3;
					}
				}
				break;
			case DungeonData.Direction.SOUTH:
				intVector = new IntVector2(int.MaxValue, int.MinValue);
				foreach (IntVector2 intVector4 in this.m_upstreamCells)
				{
					if (intVector4.y > intVector.y || (intVector4.y == intVector.y && intVector4.x < intVector.x))
					{
						intVector = intVector4;
					}
				}
				break;
			case DungeonData.Direction.WEST:
				intVector = new IntVector2(int.MinValue, int.MaxValue);
				foreach (IntVector2 intVector5 in this.m_upstreamCells)
				{
					if (intVector5.x > intVector.x || (intVector5.x == intVector.x && intVector5.y < intVector.y))
					{
						intVector = intVector5;
					}
				}
				break;
			}
			return intVector;
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001FE858 File Offset: 0x001FCA58
		public bool IsVisibleFromRoom(RoomHandler room)
		{
			if (Pixelator.Instance.UseTexturedOcclusion)
			{
				return true;
			}
			if (this.linkedDoor == null)
			{
				if (room == this.upstreamRoom)
				{
					if (this.downstreamRoom != null && this.downstreamRoom.secretRoomManager != null)
					{
						return this.downstreamRoom.secretRoomManager.IsOpen;
					}
				}
				else if (room == this.downstreamRoom && this.upstreamRoom != null && this.upstreamRoom.secretRoomManager != null)
				{
					return this.upstreamRoom.secretRoomManager.IsOpen;
				}
				return room == this.downstreamRoom;
			}
			if (room == this.upstreamRoom)
			{
				if (this.linkedDoor.subsidiaryBlocker == null && this.linkedDoor.subsidiaryDoor == null)
				{
					return !this.m_upstreamCells.Contains(this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false)) || this.linkedDoor.IsOpenForVisibilityTest;
				}
				if (this.linkedDoor.subsidiaryBlocker != null)
				{
					if (this.linkedDoor.IsOpenForVisibilityTest && !this.linkedDoor.subsidiaryBlocker.isSealed)
					{
						return true;
					}
				}
				else if (this.linkedDoor.subsidiaryDoor != null && this.linkedDoor.IsOpenForVisibilityTest && this.linkedDoor.subsidiaryDoor.IsOpenForVisibilityTest)
				{
					return true;
				}
				if (this.m_upstreamCells.Contains(this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false)))
				{
					if (this.linkedDoor.IsOpenForVisibilityTest)
					{
						return true;
					}
				}
				else if (this.linkedDoor.subsidiaryBlocker != null)
				{
					if (!this.linkedDoor.subsidiaryBlocker.isSealed)
					{
						return true;
					}
				}
				else if (this.linkedDoor.subsidiaryDoor != null && this.linkedDoor.subsidiaryDoor.IsOpenForVisibilityTest)
				{
					return true;
				}
				return false;
			}
			else
			{
				if (room != this.downstreamRoom)
				{
					return false;
				}
				if (this.linkedDoor.subsidiaryBlocker == null && this.linkedDoor.subsidiaryDoor == null)
				{
					return !this.m_downstreamCells.Contains(this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false)) || this.linkedDoor.IsOpenForVisibilityTest;
				}
				if (this.linkedDoor.subsidiaryBlocker != null)
				{
					if (this.linkedDoor.IsOpenForVisibilityTest && !this.linkedDoor.subsidiaryBlocker.isSealed)
					{
						return true;
					}
				}
				else if (this.linkedDoor.subsidiaryDoor != null && this.linkedDoor.IsOpenForVisibilityTest && this.linkedDoor.subsidiaryDoor.IsOpenForVisibilityTest)
				{
					return true;
				}
				if (this.m_upstreamCells.Contains(this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false)))
				{
					if (this.linkedDoor.subsidiaryBlocker != null)
					{
						if (!this.linkedDoor.subsidiaryBlocker.isSealed)
						{
							return true;
						}
					}
					else if (this.linkedDoor.subsidiaryDoor != null && this.linkedDoor.subsidiaryDoor.IsOpenForVisibilityTest)
					{
						return true;
					}
				}
				else if (this.linkedDoor.IsOpenForVisibilityTest)
				{
					return true;
				}
				return false;
			}
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001FEC1C File Offset: 0x001FCE1C
		public void ProcessExitDecorables()
		{
			if (this.upstreamRoom != null && (this.upstreamRoom.secretRoomManager != null || this.upstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET))
			{
				return;
			}
			if (this.downstreamRoom != null && (this.downstreamRoom.secretRoomManager != null || this.downstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET))
			{
				return;
			}
			this.PlaceExitDecorables(this.upstreamRoom, this.upstreamExit, this.downstreamRoom);
			this.PlaceExitDecorables(this.downstreamRoom, this.downstreamExit, this.upstreamRoom);
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x001FECCC File Offset: 0x001FCECC
		public void StampCellVisualTypes(DungeonData dungeonData)
		{
			if (GameManager.Instance.Dungeon.UsesWallWarpWingDoors && ((this.upstreamExit != null && this.upstreamExit.isWarpWingStart) || (this.downstreamExit != null && this.downstreamExit.isWarpWingStart)))
			{
				this.GenerateWarpWingPortals();
			}
			foreach (IntVector2 intVector in this.GetCellsForRoom(this.upstreamRoom))
			{
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.SOUTH)
				{
					dungeonData[intVector.x, intVector.y].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					dungeonData[intVector.x - 1, intVector.y + 2].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					dungeonData[intVector.x + 1, intVector.y + 2].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
				}
				else
				{
					dungeonData[intVector.x, intVector.y].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					if (dungeonData[intVector.x, intVector.y + 1].type == CellType.WALL)
					{
						dungeonData[intVector.x, intVector.y + 1].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					}
					if (dungeonData[intVector.x, intVector.y + 2].type == CellType.WALL)
					{
						dungeonData[intVector.x, intVector.y + 2].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					}
					if (dungeonData[intVector.x, intVector.y + 3].type == CellType.WALL)
					{
						dungeonData[intVector.x, intVector.y + 3].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					}
				}
			}
			if (this.downstreamRoom != null && this.downstreamExit != null)
			{
				foreach (IntVector2 intVector2 in this.GetCellsForRoom(this.downstreamRoom))
				{
					if (this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.SOUTH)
					{
						dungeonData[intVector2.x, intVector2.y].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						dungeonData[intVector2.x - 1, intVector2.y + 1].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						dungeonData[intVector2.x + 1, intVector2.y + 1].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
					}
					else
					{
						dungeonData[intVector2.x, intVector2.y].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						if (dungeonData[intVector2.x, intVector2.y + 1].type == CellType.WALL)
						{
							dungeonData[intVector2.x, intVector2.y + 1].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						}
						if (dungeonData[intVector2.x, intVector2.y + 2].type == CellType.WALL)
						{
							dungeonData[intVector2.x, intVector2.y + 2].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						}
						if (dungeonData[intVector2.x, intVector2.y + 3].type == CellType.WALL)
						{
							dungeonData[intVector2.x, intVector2.y + 3].cellVisualData.roomVisualTypeIndex = this.downstreamRoom.RoomVisualSubtype;
						}
					}
				}
			}
			if (this.IntermediaryCells != null && this.IntermediaryCells.Count > 0)
			{
				foreach (IntVector2 intVector3 in this.IntermediaryCells)
				{
					dungeonData[intVector3.x, intVector3.y].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
					for (int i = -1; i < 2; i++)
					{
						if (i == 0 || dungeonData[intVector3.x + i, intVector3.y].type == CellType.WALL)
						{
							int num = ((!this.upstreamExit.jointedExit || (this.upstreamExit.referencedExit.exitDirection != DungeonData.Direction.SOUTH && this.downstreamExit.referencedExit.exitDirection != DungeonData.Direction.SOUTH)) ? 2 : 0);
							for (int j = num; j < 4; j++)
							{
								if (dungeonData[intVector3.x + i, intVector3.y + j].type == CellType.WALL)
								{
									dungeonData[intVector3.x + i, intVector3.y + j].cellVisualData.roomVisualTypeIndex = this.upstreamRoom.RoomVisualSubtype;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x001FF2F8 File Offset: 0x001FD4F8
		protected void CleanupCellDataForWarpWingExits()
		{
			IntVector2 intVector = new IntVector2(int.MaxValue, int.MaxValue);
			IntVector2 intVector2 = new IntVector2(int.MinValue, int.MinValue);
			foreach (IntVector2 intVector3 in this.m_upstreamCells)
			{
				intVector = IntVector2.Min(intVector, intVector3);
				intVector2 = IntVector2.Max(intVector2, intVector3);
			}
			for (int i = intVector.x; i <= intVector2.x; i++)
			{
				for (int j = intVector.y; j <= intVector2.y; j++)
				{
					this.m_upstreamCells.Add(new IntVector2(i, j));
				}
			}
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x001FF3D8 File Offset: 0x001FD5D8
		protected void CalculateCellData()
		{
			this.m_upstreamCells = new HashSet<IntVector2>();
			this.m_downstreamCells = new HashSet<IntVector2>();
			this.ExitOccluderCells = new HashSet<IntVector2>();
			DungeonData data = GameManager.Instance.Dungeon.data;
			IntVector2 doorPositionForExit = this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false);
			bool flag = this.RequiresSubDoor();
			IntVector2 intVector = IntVector2.Zero;
			if (flag)
			{
				intVector = this.GetSubDoorPositionForExit(this.upstreamExit, this.upstreamRoom);
			}
			if (flag)
			{
				this.IntermediaryCells = new HashSet<IntVector2>();
			}
			if (this.upstreamExit != null)
			{
				IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(this.upstreamExit.referencedExit.exitDirection);
				int num = ((!this.upstreamExit.jointedExit || this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
				if (this.downstreamRoom.area.prototypeRoom == null && this.downstreamRoom.area.IsProceduralRoom && this.downstreamRoom.area.proceduralCells == null && this.upstreamExit.referencedExit.exitDirection != DungeonData.Direction.EAST)
				{
					num--;
				}
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				int num2 = 2;
				for (int i = 0; i < this.upstreamExit.TotalExitLength + num; i++)
				{
					for (int j = 0; j < this.upstreamExit.referencedExit.containedCells.Count; j++)
					{
						IntVector2 intVector2 = this.upstreamExit.referencedExit.containedCells[j].ToIntVector2(VectorConversions.Round) - IntVector2.One + this.upstreamRoom.area.basePosition + intVector2FromDirection * i;
						if (intVector2 == doorPositionForExit)
						{
							flag2 = true;
						}
						if (flag)
						{
							if (intVector2 == intVector)
							{
								flag4 = true;
							}
							if (flag4 || flag2)
							{
								this.IntermediaryCells.Add(intVector2);
							}
						}
						if (flag3)
						{
							this.m_downstreamCells.Add(intVector2);
						}
						else
						{
							this.m_upstreamCells.Add(intVector2);
						}
						if (i <= num2 && data.CheckInBoundsAndValid(intVector2))
						{
							data[intVector2].occlusionData.sharedRoomAndExitCell = true;
						}
					}
					if (flag2)
					{
						flag3 = true;
					}
				}
			}
			if (this.downstreamExit != null)
			{
				IntVector2 intVector2FromDirection2 = DungeonData.GetIntVector2FromDirection(this.downstreamExit.referencedExit.exitDirection);
				int num3 = ((!this.downstreamExit.jointedExit || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
				if (this.downstreamRoom.area.prototypeRoom == null && this.downstreamRoom.area.IsProceduralRoom && this.downstreamRoom.area.proceduralCells == null && this.upstreamExit.referencedExit.exitDirection != DungeonData.Direction.EAST)
				{
					num3--;
				}
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				int num4 = 2;
				for (int k = 0; k < this.downstreamExit.TotalExitLength + num3; k++)
				{
					for (int l = 0; l < this.downstreamExit.referencedExit.containedCells.Count; l++)
					{
						IntVector2 intVector3 = this.downstreamExit.referencedExit.containedCells[l].ToIntVector2(VectorConversions.Round) - IntVector2.One + this.downstreamRoom.area.basePosition + intVector2FromDirection2 * k;
						if (intVector3 == doorPositionForExit)
						{
							flag5 = true;
						}
						if (flag)
						{
							if (intVector3 == intVector)
							{
								flag7 = true;
							}
							if (flag7 || flag5)
							{
								this.IntermediaryCells.Add(intVector3);
							}
						}
						if (flag6)
						{
							this.m_upstreamCells.Add(intVector3);
						}
						else
						{
							this.m_downstreamCells.Add(intVector3);
						}
						if (k <= num4)
						{
							if (!data.CheckInBoundsAndValid(intVector3))
							{
								Debug.Log(string.Concat(new object[]
								{
									intVector3,
									" is out of bounds for ",
									(this.upstreamRoom == null) ? "null" : this.upstreamRoom.GetRoomName(),
									" | ",
									(this.downstreamRoom == null) ? "null" : this.downstreamRoom.GetRoomName()
								}));
								Debug.Log(this.upstreamRoom.area.basePosition + "|" + this.downstreamRoom.area.basePosition);
							}
							else
							{
								data[intVector3].occlusionData.sharedRoomAndExitCell = true;
							}
						}
					}
					if (flag5)
					{
						flag6 = true;
					}
				}
			}
			if (this.downstreamExit != null)
			{
				IntVector2 intVector4 = this.upstreamRoom.area.basePosition + this.upstreamExit.ExitOrigin - IntVector2.One;
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST)
				{
					intVector4 += IntVector2.Right;
				}
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH)
				{
					intVector4 += IntVector2.Up;
				}
				IntVector2 intVector5 = this.downstreamRoom.area.basePosition + this.downstreamExit.ExitOrigin - IntVector2.One;
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST)
				{
					intVector5 += IntVector2.Right;
				}
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH)
				{
					intVector5 += IntVector2.Up;
				}
				if (this.IntermediaryCells == null)
				{
					this.IntermediaryCells = new HashSet<IntVector2>();
				}
				if (!this.m_upstreamCells.Contains(intVector4) && !this.m_downstreamCells.Contains(intVector4) && !this.IntermediaryCells.Contains(intVector4))
				{
					this.m_upstreamCells.Add(intVector4);
					this.IntermediaryCells.Add(intVector4);
				}
				if (!this.m_upstreamCells.Contains(intVector5) && !this.m_downstreamCells.Contains(intVector5) && !this.IntermediaryCells.Contains(intVector5))
				{
					this.m_downstreamCells.Add(intVector5);
					this.IntermediaryCells.Add(intVector5);
				}
			}
			if ((this.upstreamRoom != null && !this.upstreamRoom.area.IsProceduralRoom && this.upstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET) || (this.downstreamRoom != null && !this.downstreamRoom.area.IsProceduralRoom && this.downstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET))
			{
				this.CorrectForSecretRoomDoorlessness();
			}
			if ((this.upstreamExit != null && this.upstreamExit.isWarpWingStart) || (this.downstreamExit != null && this.downstreamExit.isWarpWingStart))
			{
				this.CleanupCellDataForWarpWingExits();
			}
			if (this.m_upstreamCells != null)
			{
				foreach (IntVector2 intVector6 in this.m_upstreamCells)
				{
					this.ExitOccluderCells.Add(intVector6);
				}
			}
			if (this.m_downstreamCells != null)
			{
				foreach (IntVector2 intVector7 in this.m_downstreamCells)
				{
					this.ExitOccluderCells.Add(intVector7);
				}
			}
			if (this.IntermediaryCells != null)
			{
				foreach (IntVector2 intVector8 in this.IntermediaryCells)
				{
					this.ExitOccluderCells.Add(intVector8);
				}
			}
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			foreach (IntVector2 intVector9 in this.ExitOccluderCells)
			{
				if (this.ExitOccluderCells.Contains(intVector9 + IntVector2.Right * 2) || this.ExitOccluderCells.Contains(intVector9 + IntVector2.Left * 2) || (this.ExitOccluderCells.Contains(intVector9 + IntVector2.Left) && this.ExitOccluderCells.Contains(intVector9 + IntVector2.Right)))
				{
					hashSet.Add(intVector9 + IntVector2.Up);
					hashSet.Add(intVector9 + IntVector2.Up * 2);
				}
			}
			foreach (IntVector2 intVector10 in hashSet)
			{
				this.ExitOccluderCells.Add(intVector10);
			}
			foreach (IntVector2 intVector11 in this.ExitOccluderCells)
			{
				if (data.CheckInBoundsAndValid(intVector11))
				{
					data[intVector11].occlusionData.occlusionParentDefintion = this;
				}
			}
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x001FFE74 File Offset: 0x001FE074
		public void CorrectForSecretRoomDoorlessness()
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			IntVector2[] cardinals = IntVector2.Cardinals;
			foreach (IntVector2 intVector in this.m_upstreamCells)
			{
				if (data.CheckInBoundsAndValid(intVector))
				{
					data[intVector].occlusionData.sharedRoomAndExitCell = true;
					data[intVector].parentRoom = this.upstreamRoom;
					data[intVector].parentArea = this.upstreamRoom.area;
				}
			}
			foreach (IntVector2 intVector2 in this.m_downstreamCells)
			{
				if (data.CheckInBoundsAndValid(intVector2))
				{
					data[intVector2].occlusionData.sharedRoomAndExitCell = true;
					data[intVector2].parentRoom = this.upstreamRoom;
					data[intVector2].parentArea = this.upstreamRoom.area;
				}
			}
			foreach (IntVector2 intVector3 in this.m_downstreamCells)
			{
				for (int i = 0; i < cardinals.Length; i++)
				{
					IntVector2 intVector4 = intVector3 + cardinals[i];
					if (!this.m_downstreamCells.Contains(intVector4))
					{
						if (this.m_upstreamCells.Contains(intVector4))
						{
							if (this.IntermediaryCells == null)
							{
								this.IntermediaryCells = new HashSet<IntVector2>();
							}
							this.IntermediaryCells.Add(intVector4);
							this.IntermediaryCells.Add(intVector3);
						}
					}
				}
			}
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x00200088 File Offset: 0x001FE288
		protected DungeonData.Direction GetSubsidiaryDoorDirection()
		{
			DungeonData.Direction direction;
			if (this.upstreamExit.jointedExit)
			{
				if (!this.upstreamExit.oneWayDoor && (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST || this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.WEST))
				{
					direction = this.upstreamExit.referencedExit.exitDirection;
				}
				else
				{
					direction = this.downstreamExit.referencedExit.exitDirection;
				}
			}
			else
			{
				direction = this.downstreamExit.referencedExit.exitDirection;
			}
			return direction;
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x00200124 File Offset: 0x001FE324
		protected void GenerateSubsidiaryDoor(DungeonData dungeonData, DungeonPlaceable sourcePlaceable, DungeonDoorController mainDoor, Transform doorParentTransform)
		{
			IntVector2 subDoorPositionForExit = this.GetSubDoorPositionForExit(this.upstreamExit, this.upstreamRoom);
			if (dungeonData.HasDoorAtPosition(subDoorPositionForExit))
			{
				Debug.LogError("Attempting to generate subdoor for position twice.");
				return;
			}
			DungeonData.Direction subsidiaryDoorDirection = this.GetSubsidiaryDoorDirection();
			IntVector2 intVector = subDoorPositionForExit - this.upstreamRoom.area.basePosition;
			GameObject gameObject = sourcePlaceable.InstantiateObjectDirectional(this.upstreamRoom, intVector, subsidiaryDoorDirection);
			gameObject.transform.parent = doorParentTransform;
			DungeonDoorController component = gameObject.GetComponent<DungeonDoorController>();
			component.exitDefinition = this;
			mainDoor.subsidiaryDoor = component;
			component.parentDoor = mainDoor;
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x002001B8 File Offset: 0x001FE3B8
		public void GenerateStandaloneRoomBlocker(DungeonData dungeonData, Transform parentTransform)
		{
			if (GameManager.Instance.Dungeon.phantomBlockerDoorObjects == null)
			{
				return;
			}
			RuntimeRoomExitData runtimeRoomExitData = this.upstreamExit;
			int num = runtimeRoomExitData.TotalExitLength + runtimeRoomExitData.linkedExit.TotalExitLength - 3;
			if (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.SOUTH)
			{
				num--;
			}
			IntVector2 intVector = runtimeRoomExitData.referencedExit.GetExitOrigin(num) - IntVector2.One;
			IntVector2 intVector2 = intVector + this.upstreamRoom.area.basePosition;
			if (dungeonData.HasDoorAtPosition(intVector2))
			{
				Debug.LogError("Attempting to generate subdoor for position twice.");
				return;
			}
			DungeonData.Direction exitDirection = this.upstreamExit.referencedExit.exitDirection;
			GameObject gameObject = GameManager.Instance.Dungeon.phantomBlockerDoorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector, exitDirection);
			gameObject.transform.parent = parentTransform;
			DungeonDoorSubsidiaryBlocker component = gameObject.GetComponent<DungeonDoorSubsidiaryBlocker>();
			if (this.downstreamRoom != null)
			{
				this.downstreamRoom.standaloneBlockers.Add(component);
			}
			if (this.upstreamRoom != null)
			{
				this.upstreamRoom.standaloneBlockers.Add(component);
			}
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x002002D8 File Offset: 0x001FE4D8
		public void GenerateSecretRoomBlocker(DungeonData dungeonData, SecretRoomManager secretManager, SecretRoomDoorBeer secretDoor, Transform parentTransform)
		{
			if (GameManager.Instance.Dungeon.phantomBlockerDoorObjects == null)
			{
				return;
			}
			RuntimeRoomExitData runtimeRoomExitData = ((secretManager.room != this.upstreamRoom) ? this.downstreamExit : this.upstreamExit);
			int num = runtimeRoomExitData.TotalExitLength + runtimeRoomExitData.linkedExit.TotalExitLength - 3;
			if (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.SOUTH)
			{
				num--;
			}
			IntVector2 intVector = runtimeRoomExitData.referencedExit.GetExitOrigin(num) - IntVector2.One;
			IntVector2 intVector2 = intVector + secretManager.room.area.basePosition;
			if (dungeonData.HasDoorAtPosition(intVector2))
			{
				Debug.LogError("Attempting to generate subdoor for position twice.");
				return;
			}
			DungeonData.Direction direction = ((secretDoor.exitDef.upstreamRoom != secretManager.room) ? secretDoor.exitDef.downstreamExit.referencedExit.exitDirection : secretDoor.exitDef.upstreamExit.referencedExit.exitDirection);
			GameObject gameObject = GameManager.Instance.Dungeon.phantomBlockerDoorObjects.InstantiateObjectDirectional(secretManager.room, intVector, direction);
			gameObject.transform.parent = parentTransform;
			DungeonDoorSubsidiaryBlocker component = gameObject.GetComponent<DungeonDoorSubsidiaryBlocker>();
			component.ToggleRenderers(false);
			secretDoor.subsidiaryBlocker = component;
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x00200420 File Offset: 0x001FE620
		protected void GeneratePhantomDoorBlocker(DungeonData dungeonData, DungeonDoorController mainDoor, Transform doorParentTransform)
		{
			if (GameManager.Instance.Dungeon.phantomBlockerDoorObjects == null)
			{
				return;
			}
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON && mainDoor.OneWayDoor)
			{
				return;
			}
			IntVector2 subDoorPositionForExit = this.GetSubDoorPositionForExit(this.upstreamExit, this.upstreamRoom);
			if (dungeonData.HasDoorAtPosition(subDoorPositionForExit))
			{
				Debug.LogError("Attempting to generate subdoor for position twice.");
				return;
			}
			DungeonData.Direction direction;
			if (this.upstreamExit.jointedExit)
			{
				if (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST || this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.WEST)
				{
					direction = this.upstreamExit.referencedExit.exitDirection;
				}
				else
				{
					direction = this.downstreamExit.referencedExit.exitDirection;
				}
			}
			else
			{
				direction = this.downstreamExit.referencedExit.exitDirection;
			}
			IntVector2 intVector = subDoorPositionForExit - this.upstreamRoom.area.basePosition;
			GameObject gameObject = GameManager.Instance.Dungeon.phantomBlockerDoorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector, direction);
			gameObject.transform.parent = doorParentTransform;
			DungeonDoorSubsidiaryBlocker component = gameObject.GetComponent<DungeonDoorSubsidiaryBlocker>();
			mainDoor.subsidiaryBlocker = component;
			component.parentDoor = mainDoor;
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x00200570 File Offset: 0x001FE770
		private void GenerateWarpWingPortals()
		{
			bool flag = false;
			if (GameManager.Instance.Dungeon.UsesWallWarpWingDoors && this.upstreamRoom != null && this.downstreamRoom != null)
			{
				if (this.m_upstreamCells != null)
				{
					this.m_upstreamCells.Clear();
				}
				if (this.m_downstreamCells != null)
				{
					this.m_downstreamCells.Clear();
				}
				if (this.IntermediaryCells != null)
				{
					this.IntermediaryCells.Clear();
				}
				int wallClearanceWidth = GameManager.Instance.Dungeon.WarpWingDoorPrefab.GetComponent<PlacedWallDecorator>().wallClearanceWidth;
				List<TK2DInteriorDecorator.WallExpanse> list = this.upstreamRoom.GatherExpanses(DungeonData.Direction.NORTH, true, false, true);
				List<TK2DInteriorDecorator.WallExpanse> list2 = this.downstreamRoom.GatherExpanses(DungeonData.Direction.NORTH, true, false, true);
				Debug.Log(string.Concat(new object[] { list.Count, "|", list2.Count, "| req width: ", wallClearanceWidth }));
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].width < wallClearanceWidth)
					{
						list.RemoveAt(i);
						i--;
					}
				}
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j].width < wallClearanceWidth)
					{
						list2.RemoveAt(j);
						j--;
					}
				}
				Debug.Log(string.Concat(new object[] { list.Count, "|", list2.Count, "| post cull" }));
				TK2DInteriorDecorator.WallExpanse? wallExpanse = ((list.Count <= 0) ? null : new TK2DInteriorDecorator.WallExpanse?(list[UnityEngine.Random.Range(0, list.Count)]));
				TK2DInteriorDecorator.WallExpanse? wallExpanse2 = ((list2.Count <= 0) ? null : new TK2DInteriorDecorator.WallExpanse?(list2[UnityEngine.Random.Range(0, list2.Count)]));
				if (wallExpanse != null && wallExpanse2 != null)
				{
					GameObject warpWingDoorPrefab = GameManager.Instance.Dungeon.WarpWingDoorPrefab;
					int num = 0;
					if (wallExpanse.Value.width > wallClearanceWidth)
					{
						num = Mathf.CeilToInt((float)wallExpanse.Value.width / 2f - (float)wallClearanceWidth / 2f);
					}
					int num2 = 0;
					if (wallExpanse2.Value.width > wallClearanceWidth)
					{
						num2 = Mathf.CeilToInt((float)wallExpanse2.Value.width / 2f - (float)wallClearanceWidth / 2f);
					}
					Vector3 vector = wallExpanse.Value.basePosition.ToVector3() + Vector3.right * (float)num + Vector3.up;
					Vector3 vector2 = wallExpanse2.Value.basePosition.ToVector3() + Vector3.right * (float)num2 + Vector3.up;
					WarpPointHandler component = UnityEngine.Object.Instantiate<GameObject>(warpWingDoorPrefab, this.upstreamRoom.area.basePosition.ToVector3() + vector + warpWingDoorPrefab.transform.localPosition, Quaternion.identity).GetComponent<WarpPointHandler>();
					WarpPointHandler component2 = UnityEngine.Object.Instantiate<GameObject>(warpWingDoorPrefab, this.downstreamRoom.area.basePosition.ToVector3() + vector2 + warpWingDoorPrefab.transform.localPosition, Quaternion.identity).GetComponent<WarpPointHandler>();
					PlacedWallDecorator component3 = component.GetComponent<PlacedWallDecorator>();
					if (component3)
					{
						component3.ConfigureOnPlacement(this.upstreamRoom);
					}
					PlacedWallDecorator component4 = component2.GetComponent<PlacedWallDecorator>();
					if (component4)
					{
						component4.ConfigureOnPlacement(this.downstreamRoom);
					}
					component.GetComponent<PlacedWallDecorator>().ConfigureOnPlacement(this.upstreamRoom);
					component2.GetComponent<PlacedWallDecorator>().ConfigureOnPlacement(this.downstreamRoom);
					component.spawnOffset = new Vector2(0f, -0.25f);
					component2.spawnOffset = new Vector2(0f, -0.25f);
					component.SetTarget(component2);
					component2.SetTarget(component);
					flag = true;
				}
			}
			if (!flag)
			{
				GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/WarpWing_Portal", ".prefab");
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				WarpWingPortalController component5 = gameObject2.GetComponent<WarpWingPortalController>();
				WarpWingPortalController component6 = gameObject3.GetComponent<WarpWingPortalController>();
				component5.pairedPortal = component6;
				component5.parentRoom = this.upstreamRoom;
				component5.parentExit = this.upstreamExit;
				component6.pairedPortal = component5;
				component6.parentRoom = this.downstreamRoom;
				component6.parentExit = this.downstreamExit;
				this.upstreamExit.warpWingPortal = component5;
				this.downstreamExit.warpWingPortal = component6;
				IntVector2 intVector = this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, true);
				IntVector2 intVector2 = this.GetDoorPositionForExit(this.downstreamExit, this.downstreamRoom, true);
				intVector += DungeonData.GetIntVector2FromDirection(this.upstreamExit.referencedExit.exitDirection) * 3;
				intVector2 += DungeonData.GetIntVector2FromDirection(this.downstreamExit.referencedExit.exitDirection) * 3;
				component5.transform.position = intVector.ToVector3();
				component6.transform.position = intVector2.ToVector3();
				RoomHandler.unassignedInteractableObjects.Add(component5);
				RoomHandler.unassignedInteractableObjects.Add(component6);
			}
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x00200B18 File Offset: 0x001FED18
		public void GenerateDoorsForExit(DungeonData dungeonData, Transform doorParentTransform)
		{
			if (!GameManager.Instance.Dungeon.UsesWallWarpWingDoors && ((this.upstreamExit != null && this.upstreamExit.isWarpWingStart) || (this.downstreamExit != null && this.downstreamExit.isWarpWingStart)))
			{
				this.GenerateWarpWingPortals();
			}
			else if ((this.upstreamExit != null && this.upstreamExit.isWarpWingStart) || (this.downstreamExit != null && this.downstreamExit.isWarpWingStart))
			{
				return;
			}
			bool flag = false;
			if (this.upstreamRoom != null && this.upstreamRoom.area.prototypeRoom != null && this.upstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				flag = true;
			}
			if (this.downstreamRoom != null && this.downstreamRoom.area.prototypeRoom != null && this.downstreamRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				flag = true;
			}
			if (flag && (this.upstreamExit == null || !this.upstreamExit.oneWayDoor) && (this.downstreamExit == null || !this.downstreamExit.oneWayDoor))
			{
				return;
			}
			if (this.upstreamRoom.area.PrototypeLostWoodsRoom || this.downstreamRoom.area.PrototypeLostWoodsRoom)
			{
				this.GenerateStandaloneRoomBlocker(dungeonData, doorParentTransform);
				flag = true;
				return;
			}
			IntVector2 doorPositionForExit = this.GetDoorPositionForExit(this.upstreamExit, this.upstreamRoom, false);
			if (dungeonData.HasDoorAtPosition(doorPositionForExit))
			{
				Debug.LogError("Attempting to generate door for position twice.");
				return;
			}
			DungeonData.Direction direction;
			if (this.downstreamExit == null)
			{
				direction = this.upstreamExit.referencedExit.exitDirection;
			}
			else if (this.upstreamExit == null)
			{
				direction = this.downstreamExit.referencedExit.exitDirection;
			}
			else if (this.upstreamExit.jointedExit)
			{
				if (!this.upstreamExit.oneWayDoor && (this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST || this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.WEST))
				{
					direction = this.downstreamExit.referencedExit.exitDirection;
				}
				else
				{
					direction = this.upstreamExit.referencedExit.exitDirection;
				}
			}
			else
			{
				direction = this.upstreamExit.referencedExit.exitDirection;
			}
			DungeonPlaceable dungeonPlaceable = null;
			GameObject gameObject;
			if (this.upstreamExit != null && this.upstreamExit.oneWayDoor)
			{
				IntVector2 intVector = doorPositionForExit - this.upstreamRoom.area.basePosition;
				if (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST)
				{
					intVector += IntVector2.Down;
				}
				gameObject = GameManager.Instance.Dungeon.oneWayDoorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector, direction);
				dungeonPlaceable = GameManager.Instance.Dungeon.oneWayDoorObjects;
			}
			else if (this.upstreamExit != null && this.upstreamExit.isLockedDoor && GameManager.Instance.Dungeon.lockedDoorObjects != null)
			{
				IntVector2 intVector2 = doorPositionForExit - this.upstreamRoom.area.basePosition;
				gameObject = GameManager.Instance.Dungeon.lockedDoorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector2, direction);
				dungeonPlaceable = GameManager.Instance.Dungeon.lockedDoorObjects;
			}
			else if (this.downstreamExit != null && this.downstreamExit.referencedExit.specifiedDoor != null)
			{
				IntVector2 intVector3 = doorPositionForExit - this.downstreamRoom.area.basePosition;
				dungeonPlaceable = this.downstreamExit.referencedExit.specifiedDoor;
				if (dungeonPlaceable.variantTiers.Count > 0 && dungeonPlaceable.variantTiers[0].nonDatabasePlaceable != null)
				{
					DungeonDoorController component = dungeonPlaceable.variantTiers[0].nonDatabasePlaceable.GetComponent<DungeonDoorController>();
					if (component != null && component.Mode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
					{
						intVector3 += IntVector2.Right;
					}
				}
				gameObject = dungeonPlaceable.InstantiateObjectDirectional(this.downstreamRoom, intVector3, direction);
			}
			else if (this.upstreamExit != null && this.upstreamExit.referencedExit.specifiedDoor != null)
			{
				IntVector2 intVector4 = doorPositionForExit - this.upstreamRoom.area.basePosition;
				dungeonPlaceable = this.upstreamExit.referencedExit.specifiedDoor;
				if (dungeonPlaceable.variantTiers.Count > 0 && dungeonPlaceable.variantTiers[0].nonDatabasePlaceable != null)
				{
					DungeonDoorController component2 = dungeonPlaceable.variantTiers[0].nonDatabasePlaceable.GetComponent<DungeonDoorController>();
					if (component2 != null && component2.Mode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
					{
						intVector4 += IntVector2.Right;
					}
				}
				gameObject = dungeonPlaceable.InstantiateObjectDirectional(this.upstreamRoom, intVector4, direction);
			}
			else
			{
				if (flag)
				{
					return;
				}
				IntVector2 intVector5 = doorPositionForExit - this.upstreamRoom.area.basePosition;
				Dungeon dungeon = GameManager.Instance.Dungeon;
				if (dungeon.alternateDoorObjectsNakatomi != null)
				{
					if (this.downstreamRoom != null && (this.downstreamRoom.RoomVisualSubtype == 7 || this.downstreamRoom.RoomVisualSubtype == 8 || this.upstreamRoom.RoomVisualSubtype == 7 || this.upstreamRoom.RoomVisualSubtype == 8))
					{
						gameObject = dungeon.alternateDoorObjectsNakatomi.InstantiateObjectDirectional(this.upstreamRoom, intVector5, direction);
						dungeonPlaceable = dungeon.alternateDoorObjectsNakatomi;
					}
					else
					{
						gameObject = dungeon.doorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector5, direction);
						dungeonPlaceable = dungeon.doorObjects;
					}
				}
				else
				{
					gameObject = dungeon.doorObjects.InstantiateObjectDirectional(this.upstreamRoom, intVector5, direction);
					dungeonPlaceable = dungeon.doorObjects;
				}
			}
			if (dungeonPlaceable == null)
			{
				return;
			}
			gameObject.transform.parent = doorParentTransform;
			DungeonDoorController component3 = gameObject.GetComponent<DungeonDoorController>();
			if (dungeonPlaceable == GameManager.Instance.Dungeon.lockedDoorObjects && this.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST)
			{
				component3.FlipLockToOtherSide();
			}
			if ((this.downstreamExit != null && this.downstreamExit.oneWayDoor) || (this.upstreamExit != null && this.upstreamExit.oneWayDoor))
			{
				component3.OneWayDoor = true;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.Dungeon.oneWayDoorPressurePlate);
				Vector3 vector = Vector3.zero;
				if (direction == DungeonData.Direction.WEST || direction == DungeonData.Direction.EAST)
				{
					vector = Vector3.up;
				}
				gameObject2.transform.position = component3.transform.position + (DungeonData.GetIntVector2FromDirection(direction) * 2).ToVector3() + vector;
				PressurePlate component4 = gameObject2.GetComponent<PressurePlate>();
				component3.AssignPressurePlate(component4);
			}
			foreach (IntVector2 intVector6 in this.m_upstreamCells)
			{
				dungeonData[intVector6].exitDoor = component3;
			}
			foreach (IntVector2 intVector7 in this.m_downstreamCells)
			{
				dungeonData[intVector7].exitDoor = component3;
			}
			component3.upstreamRoom = this.upstreamRoom;
			component3.downstreamRoom = this.downstreamRoom;
			component3.exitDefinition = this;
			this.upstreamRoom.connectedDoors.Add(component3);
			this.downstreamRoom.connectedDoors.Add(component3);
			IntVector2 intVector8 = IntVector2.Zero;
			if (component3.Mode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
			{
				this.GeneratePhantomDoorBlocker(dungeonData, component3, doorParentTransform);
			}
			else if (this.RequiresSubDoor() && !flag)
			{
				DungeonPlaceable dungeonPlaceable2 = ((!(dungeonPlaceable == GameManager.Instance.Dungeon.lockedDoorObjects)) ? dungeonPlaceable : GameManager.Instance.Dungeon.doorObjects);
				intVector8 = this.GetSubDoorPositionForExit(this.upstreamExit, this.upstreamRoom);
				if (component3.SupportsSubsidiaryDoors)
				{
					this.GenerateSubsidiaryDoor(dungeonData, dungeonPlaceable2, component3, doorParentTransform);
				}
				else
				{
					DungeonData.Direction subsidiaryDoorDirection = this.GetSubsidiaryDoorDirection();
					bool flag2 = subsidiaryDoorDirection == DungeonData.Direction.NORTH || subsidiaryDoorDirection == DungeonData.Direction.SOUTH;
					dungeonData.FakeRegisterDoorFeet(intVector8, flag2);
				}
			}
			dungeonData.RegisterDoor(doorPositionForExit, component3, intVector8);
			this.linkedDoor = component3;
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x00201418 File Offset: 0x001FF618
		protected bool RequiresSubDoor()
		{
			return this.upstreamExit != null && this.downstreamExit != null && (this.upstreamExit.jointedExit || this.upstreamExit.TotalExitLength + this.downstreamExit.TotalExitLength > 7);
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x0020146C File Offset: 0x001FF66C
		protected IntVector2 GetSubDoorPositionForExit(RuntimeRoomExitData exit, RoomHandler owner)
		{
			if (exit.jointedExit)
			{
				if (!exit.oneWayDoor && (exit.linkedExit == null || exit.referencedExit.exitDirection == DungeonData.Direction.EAST || exit.referencedExit.exitDirection == DungeonData.Direction.WEST))
				{
					IntVector2 intVector = exit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.referencedExit.exitDirection);
					return intVector + owner.area.basePosition;
				}
				IntVector2 intVector2 = exit.linkedExit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.linkedExit.referencedExit.exitDirection);
				return intVector2 + owner.connectedRoomsByExit[exit.referencedExit].area.basePosition;
			}
			else
			{
				if (exit.linkedExit != null && exit.TotalExitLength + exit.linkedExit.TotalExitLength > 7)
				{
					IntVector2 intVector3 = exit.linkedExit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.linkedExit.referencedExit.exitDirection);
					return intVector3 + owner.connectedRoomsByExit[exit.referencedExit].area.basePosition;
				}
				return IntVector2.MaxValue;
			}
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x002015DC File Offset: 0x001FF7DC
		protected IntVector2 GetDoorPositionForExit(RuntimeRoomExitData exit, RoomHandler owner, bool overrideSecretRoomHandling = false)
		{
			if (exit == null)
			{
				Debug.LogError("THIS EXIT ISN'T REAL. IT ISNT REAAAALLLLLLL: " + owner.GetRoomName());
			}
			RoomHandler roomHandler = owner.connectedRoomsByExit[exit.referencedExit];
			if (!overrideSecretRoomHandling)
			{
				if (owner != null && !owner.area.IsProceduralRoom && owner.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET && exit.linkedExit != null && exit.linkedExit.referencedExit != null)
				{
					IntVector2 intVector = exit.linkedExit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.linkedExit.referencedExit.exitDirection);
					return intVector + owner.connectedRoomsByExit[exit.referencedExit].area.basePosition;
				}
				if (roomHandler != null && !roomHandler.area.IsProceduralRoom && roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET && exit.linkedExit != null && exit.linkedExit.referencedExit != null)
				{
					IntVector2 intVector2 = exit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.referencedExit.exitDirection);
					return intVector2 + owner.area.basePosition;
				}
			}
			if (!exit.oneWayDoor && exit.jointedExit && exit.linkedExit != null && (exit.referencedExit.exitDirection == DungeonData.Direction.EAST || exit.referencedExit.exitDirection == DungeonData.Direction.WEST))
			{
				IntVector2 intVector3 = exit.linkedExit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.linkedExit.referencedExit.exitDirection);
				return intVector3 + owner.connectedRoomsByExit[exit.referencedExit].area.basePosition;
			}
			IntVector2 intVector4 = exit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exit.referencedExit.exitDirection);
			return intVector4 + owner.area.basePosition;
		}

		// Token: 0x04004D9F RID: 19871
		public RuntimeRoomExitData upstreamExit;

		// Token: 0x04004DA0 RID: 19872
		public RuntimeRoomExitData downstreamExit;

		// Token: 0x04004DA1 RID: 19873
		private const int SUBDOOR_CORRIDOR_THRESHOLD = 7;

		// Token: 0x04004DA2 RID: 19874
		public RoomHandler upstreamRoom;

		// Token: 0x04004DA3 RID: 19875
		public RoomHandler downstreamRoom;

		// Token: 0x04004DA4 RID: 19876
		public DungeonDoorController linkedDoor;

		// Token: 0x04004DA5 RID: 19877
		public bool containsLight;

		// Token: 0x04004DA6 RID: 19878
		public bool isCriticalPath;

		// Token: 0x04004DA7 RID: 19879
		public HashSet<IntVector2> ExitOccluderCells;

		// Token: 0x04004DA8 RID: 19880
		public HashSet<IntVector2> IntermediaryCells;

		// Token: 0x04004DA9 RID: 19881
		protected HashSet<IntVector2> m_upstreamCells;

		// Token: 0x04004DAA RID: 19882
		protected HashSet<IntVector2> m_downstreamCells;

		// Token: 0x04004DAB RID: 19883
		private bool m_westgeonProcessed;
	}
}
