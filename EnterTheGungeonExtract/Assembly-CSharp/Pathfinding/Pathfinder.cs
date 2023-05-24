using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020015B6 RID: 5558
	public class Pathfinder : MonoBehaviour
	{
		// Token: 0x06007F84 RID: 32644 RVA: 0x00337374 File Offset: 0x00335574
		public static bool CellValidator_NoTopWalls(IntVector2 cellPos)
		{
			CellData cellData = GameManager.Instance.Dungeon.data[cellPos];
			return cellData == null || !cellData.IsTopWall();
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x003373AC File Offset: 0x003355AC
		public void Awake()
		{
			Pathfinder.Instance = this;
		}

		// Token: 0x06007F86 RID: 32646 RVA: 0x003373B4 File Offset: 0x003355B4
		public void Update()
		{
			if (GameManager.Instance.PrimaryPlayer != null)
			{
				RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
				if (this.m_dirtyRooms.Contains(currentRoom))
				{
					this.RecalculateRoomClearances(currentRoom);
					this.m_dirtyRooms.Remove(currentRoom);
				}
			}
		}

		// Token: 0x06007F87 RID: 32647 RVA: 0x0033740C File Offset: 0x0033560C
		public void OnDestroy()
		{
			Pathfinder.Instance = null;
		}

		// Token: 0x06007F88 RID: 32648 RVA: 0x00337414 File Offset: 0x00335614
		public static void ClearPerLevelData()
		{
			if (Pathfinder.m_roomHandlers != null)
			{
				Pathfinder.m_roomHandlers = null;
			}
		}

		// Token: 0x06007F89 RID: 32649 RVA: 0x00337428 File Offset: 0x00335628
		public void Initialize(DungeonData dungeonData)
		{
			this.m_width = dungeonData.Width;
			this.m_height = dungeonData.Height;
			this.m_nodes = new Pathfinder.PathNode[this.m_width * this.m_height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					this.m_nodes[i + j * this.m_width] = new Pathfinder.PathNode(dungeonData.cellData[i][j], i, j);
				}
			}
			this.RecalculateClearances();
		}

		// Token: 0x06007F8A RID: 32650 RVA: 0x003374C4 File Offset: 0x003356C4
		public void InitializeRegion(DungeonData dungeonData, IntVector2 basePosition, IntVector2 dimensions)
		{
			int width = dungeonData.Width;
			int height = dungeonData.Height;
			Pathfinder.PathNode[] array = new Pathfinder.PathNode[width * height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					array[i + j * width] = this.m_nodes[i + j * this.m_width];
				}
			}
			this.m_width = width;
			this.m_height = height;
			this.m_nodes = array;
			for (int k = basePosition.x - 3; k < basePosition.x + dimensions.x + 4; k++)
			{
				for (int l = basePosition.y - 3; l < basePosition.y + dimensions.y + 4; l++)
				{
					if (k + l * this.m_width < this.m_nodes.Length && k < dungeonData.cellData.Length && l < dungeonData.cellData[k].Length)
					{
						this.m_nodes[k + l * this.m_width] = new Pathfinder.PathNode(dungeonData.cellData[k][l], k, l);
						BraveUtility.DrawDebugSquare(new Vector2((float)k, (float)l), Color.red, 1000f);
					}
				}
			}
			this.RecalculateClearances(basePosition.x, basePosition.y, basePosition.x + dimensions.x - 1, basePosition.y + dimensions.y - 1);
		}

		// Token: 0x06007F8B RID: 32651 RVA: 0x00337678 File Offset: 0x00335878
		public void RegisterObstacle(OccupiedCells cells, RoomHandler parentRoom)
		{
			if (this.m_registeredObstacles.ContainsKey(parentRoom))
			{
				this.m_registeredObstacles[parentRoom].Add(cells);
			}
			else
			{
				List<OccupiedCells> list = new List<OccupiedCells>();
				list.Add(cells);
				this.m_registeredObstacles.Add(parentRoom, list);
			}
			this.FlagRoomAsDirty(parentRoom);
		}

		// Token: 0x06007F8C RID: 32652 RVA: 0x003376D0 File Offset: 0x003358D0
		public void DeregisterObstacle(OccupiedCells cells, RoomHandler parentRoom)
		{
			if (this.m_registeredObstacles.ContainsKey(parentRoom))
			{
				this.m_registeredObstacles[parentRoom].Remove(cells);
			}
			this.FlagRoomAsDirty(parentRoom);
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x00337700 File Offset: 0x00335900
		public void FlagRoomAsDirty(RoomHandler room)
		{
			if (!this.m_dirtyRooms.Contains(room))
			{
				this.m_dirtyRooms.Add(room);
			}
		}

		// Token: 0x06007F8E RID: 32654 RVA: 0x00337720 File Offset: 0x00335920
		public void TryRecalculateRoomClearances(RoomHandler room)
		{
			if (this.m_dirtyRooms.Contains(room))
			{
				this.RecalculateRoomClearances(room);
				this.m_dirtyRooms.Remove(room);
			}
		}

		// Token: 0x06007F8F RID: 32655 RVA: 0x00337748 File Offset: 0x00335948
		public void RecalculateRoomClearances(RoomHandler room)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = 0; i < room.Cells.Count; i++)
			{
				CellData cellData = data[room.Cells[i]];
				if (cellData != null)
				{
					cellData.isOccupied = false;
				}
			}
			if (this.m_registeredObstacles.ContainsKey(room))
			{
				List<OccupiedCells> list = this.m_registeredObstacles[room];
				for (int j = 0; j < list.Count; j++)
				{
					list[j].FlagCells();
				}
			}
			if (this.m_nodes == null)
			{
				return;
			}
			int x = room.area.basePosition.x;
			int y = room.area.basePosition.y;
			int num = x + room.area.dimensions.x - 1;
			int num2 = y + room.area.dimensions.y - 1;
			this.RecalculateClearances(x, y, num, num2);
		}

		// Token: 0x06007F90 RID: 32656 RVA: 0x00337858 File Offset: 0x00335A58
		public void RecalculateClearances()
		{
			this.RecalculateClearances(0, 0, this.m_width - 1, this.m_height - 1);
		}

		// Token: 0x06007F91 RID: 32657 RVA: 0x00337874 File Offset: 0x00335A74
		private void RecalculateClearances(int minX, int minY, int maxX, int maxY)
		{
			for (int i = minX; i <= maxX; i++)
			{
				for (int j = minY; j <= maxY; j++)
				{
					int num = i + j * this.m_width;
					if (!this.m_nodes[num].IsPassable(Pathfinder.s_defaultPassableCellTypes, false, null))
					{
						this.m_nodes[num].SquareClearance = 0;
					}
					else
					{
						int num2 = Mathf.Max(maxX - i + 1, maxY - j + 1);
						int k;
						for (k = 1; k < num2; k++)
						{
							for (int l = 0; l <= k; l++)
							{
								if (!this.m_nodes[i + l + (j + k) * this.m_width].IsPassable(Pathfinder.s_defaultPassableCellTypes, false, null))
								{
									this.m_nodes[num].SquareClearance = k;
									goto IL_13D;
								}
							}
							for (int m = 0; m < k; m++)
							{
								if (!this.m_nodes[i + k + (j + m) * this.m_width].IsPassable(Pathfinder.s_defaultPassableCellTypes, false, null))
								{
									this.m_nodes[num].SquareClearance = k;
									goto IL_13D;
								}
							}
						}
						this.m_nodes[num].SquareClearance = k;
					}
					IL_13D:;
				}
			}
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x003379D8 File Offset: 0x00335BD8
		public void Smooth(Path path, Vector2 startPos, Vector2 extents, CellTypes passableCellTypes, bool canPassOccupied, IntVector2 clearance)
		{
			if (path.Positions.Count < 2)
			{
				return;
			}
			foreach (IntVector2 intVector in path.Positions)
			{
				path.PreSmoothedPositions.AddLast(intVector);
			}
			extents -= Vector2.one * (0.5f / (float)PhysicsEngine.Instance.PixelsPerUnit);
			LinkedListNode<IntVector2> linkedListNode = null;
			LinkedListNode<IntVector2> linkedListNode2 = path.Positions.First;
			int num = 2;
			while (linkedListNode2 != null)
			{
				if (!this.Walkable(startPos, Pathfinder.GetClearanceOffset(linkedListNode2.Value, clearance), extents, passableCellTypes, canPassOccupied, clearance, num > 0))
				{
					linkedListNode2 = linkedListNode;
					break;
				}
				LinkedListNode<IntVector2> linkedListNode3 = linkedListNode;
				linkedListNode = linkedListNode2;
				linkedListNode2 = linkedListNode2.Next;
				if (linkedListNode3 != null)
				{
					path.Positions.Remove(linkedListNode3);
					path.PreSmoothedPositions.AddLast(linkedListNode3.Value);
				}
				if (!canPassOccupied && this.m_nodes[this.GetNodeId(startPos.ToIntVector2(VectorConversions.Floor))].IsOccupied && !this.m_nodes[this.GetNodeId(linkedListNode.Value)].IsOccupied)
				{
					linkedListNode2 = linkedListNode;
					break;
				}
				num--;
			}
			if (linkedListNode2 == null && linkedListNode != null)
			{
				return;
			}
			if (linkedListNode == null)
			{
				linkedListNode = path.Positions.First;
			}
			linkedListNode2 = linkedListNode.Next;
			while (linkedListNode2 != null && linkedListNode2.Next != null)
			{
				if (this.Walkable(Pathfinder.GetClearanceOffset(linkedListNode.Value, clearance), Pathfinder.GetClearanceOffset(linkedListNode2.Next.Value, clearance), extents, passableCellTypes, canPassOccupied, clearance, false))
				{
					LinkedListNode<IntVector2> linkedListNode4 = linkedListNode2;
					linkedListNode2 = linkedListNode2.Next;
					path.Positions.Remove(linkedListNode4);
					path.PreSmoothedPositions.AddLast(linkedListNode4.Value);
				}
				else
				{
					linkedListNode = linkedListNode2;
					linkedListNode2 = linkedListNode2.Next;
				}
			}
		}

		// Token: 0x06007F93 RID: 32659 RVA: 0x00337BEC File Offset: 0x00335DEC
		public bool GetPath(IntVector2 start, IntVector2 end, out Path path, IntVector2? clearance = null, CellTypes passableCellTypes = CellTypes.FLOOR, CellValidator cellValidator = null, ExtraWeightingFunction extraWeightingFunction = null, bool canPassOccupied = false)
		{
			if (clearance == null)
			{
				clearance = new IntVector2?(IntVector2.Zero);
			}
			return this.GetPath(start, end, passableCellTypes, canPassOccupied, out path, clearance.Value, cellValidator, extraWeightingFunction);
		}

		// Token: 0x06007F94 RID: 32660 RVA: 0x00337C2C File Offset: 0x00335E2C
		public void UpdateActorPath(List<IntVector2> path)
		{
			for (int i = 0; i < path.Count; i++)
			{
				Pathfinder.PathNode[] nodes = this.m_nodes;
				int num = path[i].x + path[i].y * this.m_width;
				nodes[num].ActorPathCount = nodes[num].ActorPathCount + 1;
			}
		}

		// Token: 0x06007F95 RID: 32661 RVA: 0x00337C90 File Offset: 0x00335E90
		public void RemoveActorPath(List<IntVector2> path)
		{
			if (path == null || path.Count == 0)
			{
				return;
			}
			for (int i = 0; i < path.Count; i++)
			{
				Pathfinder.PathNode[] nodes = this.m_nodes;
				int num = path[i].x + path[i].y * this.m_width;
				nodes[num].ActorPathCount = nodes[num].ActorPathCount - 1;
				if (this.m_nodes[path[i].x + path[i].y * this.m_width].ActorPathCount < 0)
				{
					UnityEngine.Debug.LogWarning("Negative ActorPathCount!");
				}
			}
		}

		// Token: 0x06007F96 RID: 32662 RVA: 0x00337D4C File Offset: 0x00335F4C
		public bool IsPassable(IntVector2 pos, IntVector2? clearance = null, CellTypes? passableCellTypes = null, bool canPassOccupied = false, CellValidator cellValidator = null)
		{
			if (clearance == null)
			{
				clearance = new IntVector2?(IntVector2.One);
			}
			if (passableCellTypes == null)
			{
				passableCellTypes = new CellTypes?((CellTypes)2147483647);
			}
			return this.NodeIsValid(pos.x, pos.y) && this.m_nodes[pos.x + pos.y * this.m_width].HasClearance(clearance.Value, passableCellTypes.Value, canPassOccupied) && this.m_nodes[pos.x + pos.y * this.m_width].IsPassable(passableCellTypes.Value, canPassOccupied, cellValidator);
		}

		// Token: 0x06007F97 RID: 32663 RVA: 0x00337E1C File Offset: 0x0033601C
		public bool IsValidPathCell(IntVector2 pos)
		{
			return this.NodeIsValid(pos.x, pos.y) && this.m_nodes[pos.x + pos.y * this.m_width].CellData != null;
		}

		// Token: 0x06007F98 RID: 32664 RVA: 0x00337E70 File Offset: 0x00336070
		public static Vector2 GetClearanceOffset(IntVector2 pos, IntVector2 clearance)
		{
			return new Vector2((float)pos.x + (float)clearance.x / 2f, (float)pos.y + (float)clearance.y / 2f);
		}

		// Token: 0x06007F99 RID: 32665 RVA: 0x00337EA8 File Offset: 0x003360A8
		private bool GetPath(IntVector2 start, IntVector2 goal, CellTypes passableCellTypes, bool canPassOccupied, out Path path, IntVector2 clearance, CellValidator cellValidator = null, ExtraWeightingFunction extraWeightingFunction = null)
		{
			path = null;
			int nodeId = this.GetNodeId(goal);
			int num = start.x + start.y * this.m_width;
			if (start == goal)
			{
				path = new Path();
				return true;
			}
			this.m_pass++;
			this.m_openList.Clear();
			this.m_nodes[num].Pass = this.m_pass;
			this.m_nodes[num].ParentId = -1;
			this.m_nodes[num].Steps = 0;
			this.m_nodes[num].CombinedWeight = this.m_nodes[num].GetWeight(clearance, passableCellTypes);
			this.m_nodes[num].EstimatedRemainingDist = IntVector2.ManhattanDistance(this.m_nodes[num].Position, goal) * 2;
			this.m_nearestFailDist = this.m_nodes[num].EstimatedRemainingDist + this.m_nodes[num].ActorPathCount * 2 * 3;
			this.m_nearestFailId = num;
			this.m_openList.Add(new Pathfinder.PathNodeProxy(num, this.m_nodes[num].EstimatedCost));
			while (this.m_openList.Count > 0)
			{
				int nodeId2 = this.m_openList.Remove().NodeId;
				if (this.AtGoal(this.m_nodes[nodeId2].Position, this.m_nodes[nodeId].Position, clearance))
				{
					path = this.RecreatePath(nodeId2, clearance);
					return true;
				}
				if (this.m_nodes[nodeId2].Steps < Pathfinder.MaxSteps)
				{
					IntVector2 position = this.m_nodes[nodeId2].Position;
					if (position.y < this.m_height - 1)
					{
						this.VisitNode(nodeId2, this.GetNodeId(this.m_nodes[nodeId2].Position + IntVector2.Up), goal, passableCellTypes, canPassOccupied, clearance, cellValidator, extraWeightingFunction);
					}
					if (position.y > 0)
					{
						this.VisitNode(nodeId2, this.GetNodeId(this.m_nodes[nodeId2].Position + IntVector2.Down), goal, passableCellTypes, canPassOccupied, clearance, cellValidator, extraWeightingFunction);
					}
					if (position.x > 0)
					{
						this.VisitNode(nodeId2, this.GetNodeId(this.m_nodes[nodeId2].Position + IntVector2.Left), goal, passableCellTypes, canPassOccupied, clearance, cellValidator, extraWeightingFunction);
					}
					if (position.x < this.m_width - 1)
					{
						this.VisitNode(nodeId2, this.GetNodeId(this.m_nodes[nodeId2].Position + IntVector2.Right), goal, passableCellTypes, canPassOccupied, clearance, cellValidator, extraWeightingFunction);
					}
				}
			}
			if (this.m_nearestFailId != num)
			{
				path = this.RecreatePath(this.m_nearestFailId, clearance);
				path.WillReachFinalGoal = false;
				return true;
			}
			return false;
		}

		// Token: 0x06007F9A RID: 32666 RVA: 0x003381B0 File Offset: 0x003363B0
		private int GetNodeId(IntVector2 pos)
		{
			return pos.x + pos.y * this.m_width;
		}

		// Token: 0x06007F9B RID: 32667 RVA: 0x003381C8 File Offset: 0x003363C8
		private int GetNodeId(int x, int y)
		{
			return x + y * this.m_width;
		}

		// Token: 0x06007F9C RID: 32668 RVA: 0x003381D4 File Offset: 0x003363D4
		private bool NodeIsValid(int x, int y)
		{
			return x >= 0 && x < this.m_width && y >= 0 && y < this.m_height;
		}

		// Token: 0x06007F9D RID: 32669 RVA: 0x00338204 File Offset: 0x00336404
		private bool AtGoal(IntVector2 currentPos, IntVector2 goalPos, IntVector2 clearance)
		{
			if (clearance == IntVector2.One)
			{
				return currentPos == goalPos;
			}
			IntVector2 intVector = goalPos - currentPos;
			return intVector.x >= 0 && intVector.y >= 0 && intVector.x < clearance.x && intVector.y < clearance.y;
		}

		// Token: 0x06007F9E RID: 32670 RVA: 0x00338274 File Offset: 0x00336474
		private void VisitNode(int prevId, int nodeId, IntVector2 goal, CellTypes passableCellTypes, bool canPassOccupied, IntVector2 clearance, CellValidator cellValidator = null, ExtraWeightingFunction extraWeightingFunction = null)
		{
			if (this.m_nodes[nodeId].Pass == this.m_pass)
			{
				return;
			}
			if (this.m_nodes[nodeId].CellData == null)
			{
				return;
			}
			if (!this.m_nodes[nodeId].IsPassable(passableCellTypes, canPassOccupied, cellValidator))
			{
				return;
			}
			if (!this.m_nodes[nodeId].HasClearance(clearance, passableCellTypes, canPassOccupied))
			{
				return;
			}
			this.m_nodes[nodeId].Pass = this.m_pass;
			this.m_nodes[nodeId].ParentId = prevId;
			this.m_nodes[nodeId].Steps = this.m_nodes[prevId].Steps + 1;
			this.m_nodes[nodeId].CombinedWeight = this.m_nodes[prevId].CombinedWeight + this.m_nodes[nodeId].GetWeight(clearance, passableCellTypes);
			this.m_nodes[nodeId].EstimatedRemainingDist = IntVector2.ManhattanDistance(this.m_nodes[nodeId].Position, goal) * 2;
			int num = this.m_nodes[nodeId].EstimatedRemainingDist + this.m_nodes[nodeId].ActorPathCount * 2 * 3;
			if (extraWeightingFunction != null)
			{
				IntVector2 intVector = this.m_nodes[nodeId].Position - this.m_nodes[prevId].Position;
				IntVector2 intVector2 = IntVector2.Zero;
				int parentId = this.m_nodes[prevId].ParentId;
				if (parentId != -1)
				{
					intVector2 = this.m_nodes[prevId].Position - this.m_nodes[parentId].Position;
				}
				Pathfinder.PathNode[] nodes = this.m_nodes;
				nodes[nodeId].CombinedWeight = nodes[nodeId].CombinedWeight + extraWeightingFunction(intVector2, intVector);
			}
			if (num < this.m_nearestFailDist)
			{
				this.m_nearestFailId = nodeId;
				this.m_nearestFailDist = num;
			}
			this.m_openList.Add(new Pathfinder.PathNodeProxy(nodeId, this.m_nodes[nodeId].EstimatedCost));
		}

		// Token: 0x06007F9F RID: 32671 RVA: 0x0033849C File Offset: 0x0033669C
		private Path RecreatePath(int destId, IntVector2 clearance)
		{
			LinkedList<IntVector2> linkedList = new LinkedList<IntVector2>();
			for (int i = destId; i >= 0; i = this.m_nodes[i].ParentId)
			{
				linkedList.AddFirst(this.m_nodes[i].Position);
			}
			return new Path(linkedList, clearance);
		}

		// Token: 0x06007FA0 RID: 32672 RVA: 0x003384F0 File Offset: 0x003366F0
		private bool Walkable(Vector2 start, Vector2 end, Vector2 extents, CellTypes passableCellTypes, bool canPassOccupied, IntVector2 clearance, bool ignoreWeightChecks = false)
		{
			if ((end - start).magnitude < 0.2f)
			{
				return true;
			}
			Vector2 vector = (end - start).normalized / 5f;
			float magnitude = vector.magnitude;
			float num = Vector2.Distance(start, end);
			float num2 = float.MaxValue;
			float num3 = 0f;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 4; i++)
			{
				int num4;
				int num5;
				IntVector2 intVector;
				switch (i)
				{
				case 0:
					num4 = (int)(start.x + extents.x);
					num5 = (int)(start.y + extents.y);
					intVector = new IntVector2(1, 1);
					break;
				case 1:
					num4 = (int)(start.x + extents.x);
					num5 = (int)(start.y - extents.y);
					intVector = new IntVector2(1, clearance.y);
					break;
				case 2:
					num4 = (int)(start.x - extents.x);
					num5 = (int)(start.y + extents.y);
					intVector = new IntVector2(clearance.x, 1);
					break;
				default:
					num4 = (int)(start.x - extents.x);
					num5 = (int)(start.y - extents.y);
					intVector = clearance;
					break;
				}
				if (this.NodeIsValid(num4, num5))
				{
					int num6 = num4 + num5 * this.m_width;
					num3 = Mathf.Max(num3, (float)this.m_nodes[num6].GetWeight(intVector, passableCellTypes));
					if (this.m_nodes[num6].IsOccupied)
					{
						flag = true;
					}
					if (this.m_nodes[num6].CellType == CellType.PIT)
					{
						flag2 = true;
					}
				}
			}
			if (num3 > 0f)
			{
				num2 = num3;
			}
			Vector2 vector2 = start;
			while (num >= 0f)
			{
				num3 = 0f;
				bool flag3 = false;
				bool flag4 = false;
				for (int j = 0; j < 4; j++)
				{
					int num4;
					int num5;
					IntVector2 intVector2;
					switch (j)
					{
					case 0:
						num4 = (int)(vector2.x + extents.x);
						num5 = (int)(vector2.y + extents.y);
						intVector2 = new IntVector2(1, 1);
						break;
					case 1:
						num4 = (int)(vector2.x + extents.x);
						num5 = (int)(vector2.y - extents.y);
						intVector2 = new IntVector2(1, clearance.y);
						break;
					case 2:
						num4 = (int)(vector2.x - extents.x);
						num5 = (int)(vector2.y + extents.y);
						intVector2 = new IntVector2(clearance.x, 1);
						break;
					default:
						num4 = (int)(vector2.x - extents.x);
						num5 = (int)(vector2.y - extents.y);
						intVector2 = clearance;
						break;
					}
					int num6 = num4 + num5 * this.m_width;
					if (!this.NodeIsValid(num4, num5))
					{
						return false;
					}
					if (!this.m_nodes[num6].IsPassable((!flag2) ? passableCellTypes : (passableCellTypes | CellTypes.PIT), canPassOccupied || flag, null))
					{
						return false;
					}
					if (!ignoreWeightChecks && (float)this.m_nodes[num6].GetWeight(intVector2, passableCellTypes) > num2)
					{
						return false;
					}
					flag3 |= this.m_nodes[num6].IsOccupied;
					flag4 |= this.m_nodes[num6].CellType == CellType.PIT;
					num3 = Mathf.Max(num3, (float)this.m_nodes[num6].GetWeight(clearance, passableCellTypes));
				}
				vector2 += vector;
				num -= magnitude;
				num2 = Mathf.Min(num2, num3);
				flag = flag && flag3;
				flag2 = flag2 && flag4;
			}
			return true;
		}

		// Token: 0x06007FA1 RID: 32673 RVA: 0x003388EC File Offset: 0x00336AEC
		private bool HasRectClearance(IntVector2 position, IntVector2 clearance, CellTypes passableCellTypes, bool canPassOccupied)
		{
			for (int i = position.x; i < position.x + clearance.x; i++)
			{
				for (int j = position.y; j < position.y + clearance.y; j++)
				{
					if (i < 0 || i >= this.m_width || j < 0 || j >= this.m_height)
					{
						return false;
					}
					if (!this.m_nodes[i + j * this.m_width].IsPassable(passableCellTypes, canPassOccupied, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06007FA2 RID: 32674 RVA: 0x00338994 File Offset: 0x00336B94
		public static bool HasInstance
		{
			get
			{
				return Pathfinder.Instance != null;
			}
		}

		// Token: 0x04008221 RID: 33313
		public static int MaxSteps = 40;

		// Token: 0x04008222 RID: 33314
		public Pathfinder.DebugSettings Debug;

		// Token: 0x04008223 RID: 33315
		private static List<RoomHandler> m_roomHandlers;

		// Token: 0x04008224 RID: 33316
		public static readonly CellTypes s_defaultPassableCellTypes = CellTypes.FLOOR;

		// Token: 0x04008225 RID: 33317
		public static Pathfinder Instance;

		// Token: 0x04008226 RID: 33318
		private const int c_defaultTileWeight = 2;

		// Token: 0x04008227 RID: 33319
		private int m_pass;

		// Token: 0x04008228 RID: 33320
		private int m_width;

		// Token: 0x04008229 RID: 33321
		private int m_height;

		// Token: 0x0400822A RID: 33322
		private Pathfinder.PathNode[] m_nodes;

		// Token: 0x0400822B RID: 33323
		private BinaryHeap<Pathfinder.PathNodeProxy> m_openList = new BinaryHeap<Pathfinder.PathNodeProxy>();

		// Token: 0x0400822C RID: 33324
		private int m_nearestFailDist;

		// Token: 0x0400822D RID: 33325
		private int m_nearestFailId;

		// Token: 0x0400822E RID: 33326
		private Dictionary<RoomHandler, List<OccupiedCells>> m_registeredObstacles = new Dictionary<RoomHandler, List<OccupiedCells>>();

		// Token: 0x0400822F RID: 33327
		private List<RoomHandler> m_dirtyRooms = new List<RoomHandler>();

		// Token: 0x020015B7 RID: 5559
		private struct PathNode : IComparable<Pathfinder.PathNode>
		{
			// Token: 0x06007FA4 RID: 32676 RVA: 0x003389B4 File Offset: 0x00336BB4
			public PathNode(CellData cellData, int x, int y)
			{
				this.CellData = cellData;
				this.Position = new IntVector2(x, y);
				this.Pass = 0;
				this.ParentId = 0;
				this.Steps = 0;
				this.CombinedWeight = 0;
				this.ActorPathCount = 0;
				this.EstimatedRemainingDist = 0;
				this.FailDist = 0;
				this.SquareClearance = 0;
			}

			// Token: 0x170012EE RID: 4846
			// (get) Token: 0x06007FA5 RID: 32677 RVA: 0x00338A10 File Offset: 0x00336C10
			public int EstimatedCost
			{
				get
				{
					return this.CombinedWeight + this.EstimatedRemainingDist + 2;
				}
			}

			// Token: 0x170012EF RID: 4847
			// (get) Token: 0x06007FA6 RID: 32678 RVA: 0x00338A24 File Offset: 0x00336C24
			public bool IsOccupied
			{
				get
				{
					return this.CellData != null && this.CellData.isOccupied;
				}
			}

			// Token: 0x170012F0 RID: 4848
			// (get) Token: 0x06007FA7 RID: 32679 RVA: 0x00338A40 File Offset: 0x00336C40
			public CellType CellType
			{
				get
				{
					return (this.CellData != null) ? this.CellData.type : CellType.WALL;
				}
			}

			// Token: 0x06007FA8 RID: 32680 RVA: 0x00338A60 File Offset: 0x00336C60
			public int GetWeight(IntVector2 clearance, CellTypes passableCellTypes)
			{
				bool flag = (passableCellTypes & CellTypes.PIT) == CellTypes.PIT;
				bool flag2 = this.CellData.isOccludedByTopWall;
				bool flag3 = this.CellData.isNextToWall;
				bool flag4 = !flag && this.CellData.type == CellType.PIT && !this.CellData.fallingPrevented;
				if (clearance.x > 1 || clearance.y > 1)
				{
					int num = 0;
					while (num < clearance.x && !flag2)
					{
						int num2 = this.CellData.position.x + num;
						int num3 = 0;
						while (num3 < clearance.y && !flag2)
						{
							if (num != 0 || num3 != 0)
							{
								int num4 = this.CellData.position.y + num3;
								if (num2 >= 0 && num2 < Pathfinder.Instance.m_width && num4 >= 0 && num4 < Pathfinder.Instance.m_height)
								{
									CellData cellData = Pathfinder.Instance.m_nodes[num2 + num4 * Pathfinder.Instance.m_width].CellData;
									if (cellData.isOccludedByTopWall)
									{
										flag2 = true;
									}
									else if (cellData.isNextToWall)
									{
										flag3 = true;
									}
									if (!flag && cellData.type == CellType.PIT && !cellData.fallingPrevented)
									{
										flag4 = true;
									}
								}
							}
							num3++;
						}
						num++;
					}
				}
				int num5 = 2 + this.ActorPathCount;
				if (flag3)
				{
					num5 += 10;
				}
				if (flag2)
				{
					num5 += 2000;
				}
				if (flag4)
				{
					num5 += 10;
				}
				return num5;
			}

			// Token: 0x06007FA9 RID: 32681 RVA: 0x00338C20 File Offset: 0x00336E20
			public bool IsPassable(CellTypes passableCellTypes, bool canPassOccupied, CellValidator cellValidator = null)
			{
				return (canPassOccupied || !this.IsOccupied) && ((passableCellTypes & (CellTypes)this.CellType) == (CellTypes)this.CellType || (this.CellType == CellType.PIT && this.CellData.fallingPrevented && (passableCellTypes & CellTypes.FLOOR) == CellTypes.FLOOR)) && (cellValidator == null || cellValidator(this.Position));
			}

			// Token: 0x06007FAA RID: 32682 RVA: 0x00338C94 File Offset: 0x00336E94
			public bool HasClearance(IntVector2 clearance, CellTypes passableCellTypes, bool canPassOccupied)
			{
				if (clearance.x == clearance.y && passableCellTypes == Pathfinder.s_defaultPassableCellTypes && !canPassOccupied)
				{
					return clearance.x <= this.SquareClearance;
				}
				return Pathfinder.Instance.HasRectClearance(this.CellData.position, clearance, passableCellTypes, canPassOccupied);
			}

			// Token: 0x06007FAB RID: 32683 RVA: 0x00338CF0 File Offset: 0x00336EF0
			public int CompareTo(Pathfinder.PathNode other)
			{
				return this.EstimatedCost - other.EstimatedCost;
			}

			// Token: 0x04008230 RID: 33328
			public readonly CellData CellData;

			// Token: 0x04008231 RID: 33329
			public IntVector2 Position;

			// Token: 0x04008232 RID: 33330
			public int Pass;

			// Token: 0x04008233 RID: 33331
			public int ParentId;

			// Token: 0x04008234 RID: 33332
			public int Steps;

			// Token: 0x04008235 RID: 33333
			public int CombinedWeight;

			// Token: 0x04008236 RID: 33334
			public int ActorPathCount;

			// Token: 0x04008237 RID: 33335
			public int EstimatedRemainingDist;

			// Token: 0x04008238 RID: 33336
			public int FailDist;

			// Token: 0x04008239 RID: 33337
			public int SquareClearance;
		}

		// Token: 0x020015B8 RID: 5560
		private struct PathNodeProxy : IComparable<Pathfinder.PathNodeProxy>
		{
			// Token: 0x06007FAC RID: 32684 RVA: 0x00338D00 File Offset: 0x00336F00
			public PathNodeProxy(int nodeId, int estimatedCost)
			{
				this.NodeId = nodeId;
				this.EstimatedCost = estimatedCost;
			}

			// Token: 0x06007FAD RID: 32685 RVA: 0x00338D10 File Offset: 0x00336F10
			public int CompareTo(Pathfinder.PathNodeProxy other)
			{
				return this.EstimatedCost - other.EstimatedCost;
			}

			// Token: 0x0400823A RID: 33338
			public int NodeId;

			// Token: 0x0400823B RID: 33339
			public int EstimatedCost;
		}

		// Token: 0x020015B9 RID: 5561
		[Serializable]
		public class DebugSettings
		{
			// Token: 0x0400823C RID: 33340
			public bool DrawGrid;

			// Token: 0x0400823D RID: 33341
			public bool DrawImpassable;

			// Token: 0x0400823E RID: 33342
			public bool DrawWeights;

			// Token: 0x0400823F RID: 33343
			public bool DrawRoomNums;

			// Token: 0x04008240 RID: 33344
			public bool DrawPaths;

			// Token: 0x04008241 RID: 33345
			public bool TestPath;

			// Token: 0x04008242 RID: 33346
			public SpeculativeRigidbody TestPathOrigin;

			// Token: 0x04008243 RID: 33347
			public Vector2 TestPathClearance = new Vector2(1f, 1f);
		}
	}
}
