using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EF6 RID: 3830
	public class LoopBuilderComposite : ArbitraryFlowBuildData
	{
		// Token: 0x06005194 RID: 20884 RVA: 0x001CF8E0 File Offset: 0x001CDAE0
		public LoopBuilderComposite(List<BuilderFlowNode> containedNodes, DungeonFlow flow, LoopFlowBuilder owner, LoopBuilderComposite.CompositeStyle loop = LoopBuilderComposite.CompositeStyle.NON_LOOP)
		{
			this.loopStyle = loop;
			this.m_owner = owner;
			this.m_flow = flow;
			this.m_containedNodes = containedNodes;
			this.m_externalConnectedNodes = new List<BuilderFlowNode>();
			this.m_externalToInternalNodeMap = new Dictionary<BuilderFlowNode, BuilderFlowNode>();
			BuilderFlowNode[] array = this.m_containedNodes.ToArray();
			for (int i = 0; i < this.m_containedNodes.Count; i++)
			{
				BuilderFlowNode builderFlowNode = this.m_containedNodes[i];
				List<BuilderFlowNode> allConnectedNodes = builderFlowNode.GetAllConnectedNodes(array);
				for (int j = 0; j < allConnectedNodes.Count; j++)
				{
					if (!this.m_externalConnectedNodes.Contains(allConnectedNodes[j]))
					{
						this.m_externalToInternalNodeMap.Add(allConnectedNodes[j], builderFlowNode);
						this.m_externalConnectedNodes.Add(allConnectedNodes[j]);
					}
				}
			}
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x001CF9C0 File Offset: 0x001CDBC0
		protected static int GetMaxLoopDistanceThreshold()
		{
			return (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.OFFICEGEON) ? 30 : 50;
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x001CFA10 File Offset: 0x001CDC10
		public IntVector2 Dimensions
		{
			get
			{
				return this.m_dimensions;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001CFA18 File Offset: 0x001CDC18
		public List<BuilderFlowNode> Nodes
		{
			get
			{
				return this.m_containedNodes;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x001CFA20 File Offset: 0x001CDC20
		public List<BuilderFlowNode> ExternalConnectedNodes
		{
			get
			{
				return this.m_externalConnectedNodes;
			}
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001CFA28 File Offset: 0x001CDC28
		public BuilderFlowNode GetConnectedInternalNode(BuilderFlowNode external)
		{
			if (this.m_externalToInternalNodeMap.ContainsKey(external))
			{
				return this.m_externalToInternalNodeMap[external];
			}
			return null;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001CFA4C File Offset: 0x001CDC4C
		protected static RoomHandler PlacePhantomRoom(PrototypeDungeonRoom room, SemioticLayoutManager layout, IntVector2 newRoomPosition)
		{
			IntVector2 intVector = new IntVector2(room.Width, room.Height);
			RoomHandler roomHandler = new RoomHandler(new CellArea(newRoomPosition, intVector, 0)
			{
				prototypeRoom = room,
				instanceUsedExits = new List<PrototypeRoomExit>()
			});
			roomHandler.distanceFromEntrance = 0;
			roomHandler.CalculateOpulence();
			roomHandler.CanReceiveCaps = false;
			layout.StampCellAreaToLayout(roomHandler, false);
			return roomHandler;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001CFAAC File Offset: 0x001CDCAC
		public static RoomHandler PlaceRoom(BuilderFlowNode current, SemioticLayoutManager layout, IntVector2 newRoomPosition)
		{
			IntVector2 intVector = new IntVector2(current.assignedPrototypeRoom.Width, current.assignedPrototypeRoom.Height);
			CellArea cellArea = new CellArea(newRoomPosition, intVector, 0);
			cellArea.prototypeRoom = current.assignedPrototypeRoom;
			cellArea.instanceUsedExits = new List<PrototypeRoomExit>();
			if (current.usesOverrideCategory)
			{
				cellArea.PrototypeRoomCategory = current.overrideCategory;
			}
			RoomHandler roomHandler = new RoomHandler(cellArea);
			roomHandler.distanceFromEntrance = 0;
			roomHandler.CalculateOpulence();
			roomHandler.CanReceiveCaps = current.node.receivesCaps;
			current.instanceRoom = roomHandler;
			if (roomHandler.area.prototypeRoom != null && current.Category == PrototypeDungeonRoom.RoomCategory.SECRET && current.parentBuilderNode != null && current.parentBuilderNode.instanceRoom != null)
			{
				roomHandler.AssignRoomVisualType(current.parentBuilderNode.instanceRoom.RoomVisualSubtype, false);
			}
			layout.StampCellAreaToLayout(roomHandler, false);
			return roomHandler;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001CFB98 File Offset: 0x001CDD98
		public static void RemoveRoom(BuilderFlowNode current, SemioticLayoutManager layout)
		{
			if (current.instanceRoom != null)
			{
				for (int i = 0; i < layout.Rooms.Count; i++)
				{
					if (layout.Rooms[i].connectedRooms.Contains(current.instanceRoom))
					{
						layout.Rooms[i].DeregisterConnectedRoom(current.instanceRoom, layout.Rooms[i].area.exitToLocalDataMap[layout.Rooms[i].GetExitConnectedToRoom(current.instanceRoom)]);
					}
				}
				layout.StampCellAreaToLayout(current.instanceRoom, true);
				current.instanceRoom = null;
			}
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001CFC4C File Offset: 0x001CDE4C
		protected static void CleanupProceduralRoomConnectivity(RoomHandler room, SemioticLayoutManager layout)
		{
			for (int i = 0; i < room.connectedRooms.Count; i++)
			{
				RoomHandler roomHandler = room.connectedRooms[i];
				if (!layout.Rooms.Contains(roomHandler))
				{
					PrototypeRoomExit prototypeRoomExit = null;
					foreach (PrototypeRoomExit prototypeRoomExit2 in room.connectedRoomsByExit.Keys)
					{
						if (room.connectedRoomsByExit[prototypeRoomExit2] == roomHandler)
						{
							prototypeRoomExit = prototypeRoomExit2;
							break;
						}
					}
					if (prototypeRoomExit != null)
					{
						room.area.exitToLocalDataMap.Remove(prototypeRoomExit);
						room.area.instanceUsedExits.Remove(prototypeRoomExit);
						room.connectedRoomsByExit.Remove(prototypeRoomExit);
					}
					room.childRooms.Remove(roomHandler);
					room.connectedRooms.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001CFD50 File Offset: 0x001CDF50
		protected static void FinalizeProceduralRoomConnectivity(RuntimeRoomExitData exitLData, RuntimeRoomExitData exitRData, RoomHandler initialRoom, RoomHandler finalRoom, RoomHandler newProceduralRoom)
		{
			PrototypeRoomExit referencedExit = exitLData.referencedExit;
			PrototypeRoomExit referencedExit2 = exitRData.referencedExit;
			initialRoom.area.instanceUsedExits.Add(referencedExit);
			finalRoom.area.instanceUsedExits.Add(referencedExit2);
			initialRoom.area.exitToLocalDataMap.Add(referencedExit, exitLData);
			finalRoom.area.exitToLocalDataMap.Add(referencedExit2, exitRData);
			newProceduralRoom.parentRoom = initialRoom;
			newProceduralRoom.childRooms.Add(finalRoom);
			newProceduralRoom.connectedRooms.Add(initialRoom);
			newProceduralRoom.connectedRooms.Add(finalRoom);
			initialRoom.childRooms.Add(newProceduralRoom);
			initialRoom.connectedRooms.Add(newProceduralRoom);
			initialRoom.connectedRoomsByExit.Add(referencedExit, newProceduralRoom);
			finalRoom.childRooms.Add(newProceduralRoom);
			finalRoom.connectedRooms.Add(newProceduralRoom);
			finalRoom.connectedRoomsByExit.Add(referencedExit2, newProceduralRoom);
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001CFE30 File Offset: 0x001CE030
		public static RoomHandler PlaceProceduralPathRoom(IntRect rect, RuntimeRoomExitData exitL, RuntimeRoomExitData exitR, RoomHandler initialRoom, RoomHandler finalRoom, SemioticLayoutManager layout)
		{
			CellArea cellArea = new CellArea(rect.Min, rect.Dimensions, 0);
			RoomHandler roomHandler = new RoomHandler(cellArea);
			roomHandler.distanceFromEntrance = finalRoom.distanceFromEntrance + 1;
			roomHandler.CalculateOpulence();
			layout.StampCellAreaToLayout(roomHandler, false);
			layout.StampComplexExitToLayout(exitL, initialRoom.area, false);
			layout.StampComplexExitToLayout(exitR, finalRoom.area, false);
			LoopBuilderComposite.FinalizeProceduralRoomConnectivity(exitL, exitR, initialRoom, finalRoom, roomHandler);
			return roomHandler;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x001CFEA0 File Offset: 0x001CE0A0
		protected static List<IntVector2> ComposeRoomFromPath(List<IntVector2> path, PrototypeRoomExit exitL, PrototypeRoomExit exitR)
		{
			if (path.Count < 2)
			{
				return new List<IntVector2>(path);
			}
			List<List<IntVector2>> list = new List<List<IntVector2>>();
			List<IntVector2> list2 = new List<IntVector2>();
			IntVector2 intVector = path[1] - path[0];
			list2.Add(path[0]);
			for (int i = 1; i < path.Count; i++)
			{
				IntVector2 intVector2 = path[i] - path[i - 1];
				if (intVector2 != intVector)
				{
					intVector = intVector2;
					list.Add(list2);
					list2 = new List<IntVector2>();
					list2.Add(path[i - 1]);
				}
				list2.Add(path[i]);
			}
			list.Add(list2);
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int j = 0; j < list.Count; j++)
			{
				IntVector2 intVector3 = (((list[j][1] - list[j][0]).x == 0) ? IntVector2.Right : IntVector2.Up);
				for (int k = 0; k < list[j].Count; k++)
				{
					if (k == 0 || k == list[j].Count - 1)
					{
						hashSet.Add(list[j][k]);
						hashSet.Add(list[j][k] + IntVector2.Right);
						hashSet.Add(list[j][k] + IntVector2.Up);
						hashSet.Add(list[j][k] + IntVector2.One);
					}
					else
					{
						hashSet.Add(list[j][k]);
						hashSet.Add(list[j][k] + intVector3);
					}
				}
			}
			return hashSet.ToList<IntVector2>();
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x001D00B8 File Offset: 0x001CE2B8
		protected static void ConnectPathToExits(List<IntVector2> inputPath, RuntimeRoomExitData exitL, RuntimeRoomExitData exitR, RoomHandler initialRoom, RoomHandler finalRoom)
		{
			IntVector2 intVector = initialRoom.area.basePosition + exitL.ExitOrigin - IntVector2.One;
			IntVector2 intVector2 = finalRoom.area.basePosition + exitR.ExitOrigin - IntVector2.One;
			if (intVector.x == inputPath[inputPath.Count - 1].x || intVector.y == inputPath[inputPath.Count - 1].y)
			{
				IntVector2 majorAxis = (intVector - inputPath[inputPath.Count - 1]).MajorAxis;
				while (intVector != inputPath[inputPath.Count - 1])
				{
					inputPath.Add(inputPath[inputPath.Count - 1] + majorAxis);
				}
			}
			if (intVector2.x == inputPath[0].x || intVector2.y == inputPath[0].y)
			{
				IntVector2 majorAxis2 = (intVector2 - inputPath[0]).MajorAxis;
				while (intVector2 != inputPath[0])
				{
					inputPath.Insert(0, inputPath[0] + majorAxis2);
				}
			}
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001D0220 File Offset: 0x001CE420
		public static RoomHandler PlaceProceduralPathRoom(List<IntVector2> inputPath, RuntimeRoomExitData exitL, RuntimeRoomExitData exitR, RoomHandler initialRoom, RoomHandler finalRoom, SemioticLayoutManager layout)
		{
			IntVector2 intVector = new IntVector2(int.MaxValue, int.MaxValue);
			IntVector2 intVector2 = new IntVector2(int.MinValue, int.MinValue);
			LoopBuilderComposite.ConnectPathToExits(inputPath, exitL, exitR, initialRoom, finalRoom);
			List<IntVector2> list = LoopBuilderComposite.ComposeRoomFromPath(inputPath, exitL.referencedExit, exitR.referencedExit);
			for (int i = 0; i < list.Count; i++)
			{
				intVector.x = Math.Min(intVector.x, list[i].x);
				intVector.y = Math.Min(intVector.y, list[i].y);
				intVector2.x = Math.Max(intVector2.x, list[i].x);
				intVector2.y = Math.Max(intVector2.y, list[i].y);
			}
			for (int j = 0; j < list.Count; j++)
			{
				List<IntVector2> list2;
				int num;
				(list2 = list)[num = j] = list2[num] - intVector;
			}
			RoomHandler roomHandler = new RoomHandler(new CellArea(intVector, intVector2 - intVector, 0)
			{
				proceduralCells = list
			});
			roomHandler.distanceFromEntrance = finalRoom.distanceFromEntrance + 1;
			roomHandler.CalculateOpulence();
			layout.StampCellAreaToLayout(roomHandler, false);
			LoopBuilderComposite.FinalizeProceduralRoomConnectivity(exitL, exitR, initialRoom, finalRoom, roomHandler);
			return roomHandler;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x001D0398 File Offset: 0x001CE598
		protected List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> GetNumberOfIdealExitPairs(BuilderFlowNode parentNode, BuilderFlowNode currentNode, IntVector2 previousNodeBasePosition, int numExits)
		{
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsSimple = this.GetExitPairsSimple(parentNode, currentNode, previousNodeBasePosition);
			List<PrototypeRoomExit> list = new List<PrototypeRoomExit>();
			for (int i = 0; i < numExits; i++)
			{
				float num = float.MinValue;
				PrototypeRoomExit prototypeRoomExit = null;
				for (int j = 0; j < parentNode.assignedPrototypeRoom.exitData.exits.Count; j++)
				{
					PrototypeRoomExit prototypeRoomExit2 = parentNode.assignedPrototypeRoom.exitData.exits[j];
					if (!parentNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit2))
					{
						if (!list.Contains(prototypeRoomExit2))
						{
							int num2 = 0;
							for (int k = 0; k < parentNode.instanceRoom.area.instanceUsedExits.Count; k++)
							{
								num2 += IntVector2.ManhattanDistance(parentNode.instanceRoom.area.instanceUsedExits[k].GetExitOrigin(0), prototypeRoomExit2.GetExitOrigin(0));
							}
							for (int l = 0; l < list.Count; l++)
							{
								num2 += IntVector2.ManhattanDistance(list[l].GetExitOrigin(0), prototypeRoomExit2.GetExitOrigin(0));
							}
							float num3 = (float)num2 / (float)parentNode.instanceRoom.area.instanceUsedExits.Count;
							if (num3 > num)
							{
								num = num3;
								prototypeRoomExit = prototypeRoomExit2;
							}
						}
					}
				}
				if (prototypeRoomExit == null)
				{
					break;
				}
				list.Add(prototypeRoomExit);
			}
			for (int m = 0; m < exitPairsSimple.Count; m++)
			{
				if (!list.Contains(exitPairsSimple[m].First.referencedExit))
				{
					exitPairsSimple.RemoveAt(m);
					m--;
				}
			}
			return exitPairsSimple;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x001D056C File Offset: 0x001CE76C
		protected List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> GetExitPairsPreferDistance(BuilderFlowNode parentNode, BuilderFlowNode currentNode, IntVector2 previousNodeBasePosition)
		{
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsSimple = this.GetExitPairsSimple(parentNode, currentNode, previousNodeBasePosition);
			if (parentNode.instanceRoom.area.instanceUsedExits.Count < 1)
			{
				return exitPairsSimple;
			}
			Dictionary<PrototypeRoomExit, float> exitsToAverageDistanceMap = new Dictionary<PrototypeRoomExit, float>();
			for (int i = 0; i < parentNode.assignedPrototypeRoom.exitData.exits.Count; i++)
			{
				PrototypeRoomExit prototypeRoomExit = parentNode.assignedPrototypeRoom.exitData.exits[i];
				if (!parentNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit))
				{
					int num = 0;
					for (int j = 0; j < parentNode.instanceRoom.area.instanceUsedExits.Count; j++)
					{
						num += IntVector2.ManhattanDistance(parentNode.instanceRoom.area.instanceUsedExits[j].GetExitOrigin(0), prototypeRoomExit.GetExitOrigin(0));
					}
					float num2 = (float)num / (float)parentNode.instanceRoom.area.instanceUsedExits.Count;
					exitsToAverageDistanceMap.Add(prototypeRoomExit, num2);
				}
			}
			return exitPairsSimple.OrderByDescending((Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple) => exitsToAverageDistanceMap[tuple.First.referencedExit]).ToList<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001D06B0 File Offset: 0x001CE8B0
		protected List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> GetExitPairsSimple(BuilderFlowNode parentNode, BuilderFlowNode currentNode, IntVector2 previousNodeBasePosition)
		{
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> list = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> list2 = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			bool flag = true;
			if (parentNode.Category == PrototypeDungeonRoom.RoomCategory.SECRET || currentNode.Category == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				flag = false;
			}
			if (parentNode.Category == PrototypeDungeonRoom.RoomCategory.BOSS || currentNode.Category == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				flag = false;
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				flag = true;
			}
			List<PrototypeRoomExit.ExitGroup> definedExitGroups = parentNode.assignedPrototypeRoom.exitData.GetDefinedExitGroups();
			bool flag2 = definedExitGroups.Count > 1;
			for (int i = 0; i < parentNode.instanceRoom.area.instanceUsedExits.Count; i++)
			{
				definedExitGroups.Remove(parentNode.instanceRoom.area.instanceUsedExits[i].exitGroup);
			}
			if (definedExitGroups.Count == 0)
			{
				flag2 = false;
			}
			int j = 0;
			while (j < parentNode.assignedPrototypeRoom.exitData.exits.Count)
			{
				RuntimeRoomExitData runtimeRoomExitData = new RuntimeRoomExitData(parentNode.assignedPrototypeRoom.exitData.exits[j]);
				if (!flag2)
				{
					goto IL_166;
				}
				bool flag3 = false;
				for (int k = 0; k < parentNode.instanceRoom.area.instanceUsedExits.Count; k++)
				{
					if (parentNode.instanceRoom.area.instanceUsedExits[k].exitGroup == runtimeRoomExitData.referencedExit.exitGroup)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					goto IL_166;
				}
				IL_353:
				j++;
				continue;
				IL_166:
				for (int l = 0; l < currentNode.assignedPrototypeRoom.exitData.exits.Count; l++)
				{
					RuntimeRoomExitData runtimeRoomExitData2 = new RuntimeRoomExitData(currentNode.assignedPrototypeRoom.exitData.exits[l]);
					if (!parentNode.exitToNodeMap.ContainsKey(runtimeRoomExitData.referencedExit) && !currentNode.exitToNodeMap.ContainsKey(runtimeRoomExitData2.referencedExit))
					{
						if (parentNode.node.childNodeGuids.Contains(currentNode.node.guidAsString))
						{
							if (runtimeRoomExitData.referencedExit.exitType == PrototypeRoomExit.ExitType.ENTRANCE_ONLY || runtimeRoomExitData2.referencedExit.exitType == PrototypeRoomExit.ExitType.EXIT_ONLY)
							{
								goto IL_331;
							}
						}
						else if (currentNode.node.childNodeGuids.Contains(parentNode.node.guidAsString) && (runtimeRoomExitData2.referencedExit.exitType == PrototypeRoomExit.ExitType.ENTRANCE_ONLY || runtimeRoomExitData.referencedExit.exitType == PrototypeRoomExit.ExitType.EXIT_ONLY))
						{
							goto IL_331;
						}
						if ((runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.EAST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.WEST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.WEST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.EAST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.NORTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.SOUTH) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.SOUTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.NORTH))
						{
							Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = new Tuple<RuntimeRoomExitData, RuntimeRoomExitData>(runtimeRoomExitData, runtimeRoomExitData2);
							list.Add(tuple);
						}
						else if (runtimeRoomExitData.referencedExit.exitDirection != runtimeRoomExitData2.referencedExit.exitDirection)
						{
							Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple2 = new Tuple<RuntimeRoomExitData, RuntimeRoomExitData>(runtimeRoomExitData, runtimeRoomExitData2);
							list2.Add(tuple2);
						}
					}
					IL_331:;
				}
				goto IL_353;
			}
			list = list.GenerationShuffle<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			if (flag)
			{
				list.AddRange(list2.GenerationShuffle<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>());
			}
			return list;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001D0A4C File Offset: 0x001CEC4C
		protected List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> GetExitPairsForNode(BuilderFlowNode placedNode, BuilderFlowNode nextNode, IntVector2 previousRoomBasePosition, BuilderFlowNode currentLoopTargetNode, IntVector2 currentLoopTargetBasePosition, RoomHandler currentLoopTargetRoom, List<FlowActionLine> actionLines, bool minimizeLoopDistance)
		{
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsSimple = this.GetExitPairsSimple(placedNode, nextNode, previousRoomBasePosition);
			int[] array = new int[exitPairsSimple.Count];
			for (int i = 0; i < exitPairsSimple.Count; i++)
			{
				Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = exitPairsSimple[i];
				IntVector2 intVector = previousRoomBasePosition + tuple.First.ExitOrigin - IntVector2.One;
				IntVector2 intVector2 = intVector - (tuple.Second.ExitOrigin - IntVector2.One);
				int num = int.MaxValue;
				for (int j = 0; j < nextNode.assignedPrototypeRoom.exitData.exits.Count; j++)
				{
					for (int k = 0; k < currentLoopTargetNode.assignedPrototypeRoom.exitData.exits.Count; k++)
					{
						int num2 = 0;
						PrototypeRoomExit prototypeRoomExit = nextNode.assignedPrototypeRoom.exitData.exits[j];
						PrototypeRoomExit prototypeRoomExit2 = currentLoopTargetNode.assignedPrototypeRoom.exitData.exits[k];
						if (prototypeRoomExit != tuple.Second.referencedExit)
						{
							if (!nextNode.exitToNodeMap.ContainsKey(prototypeRoomExit) && !currentLoopTargetNode.exitToNodeMap.ContainsKey(prototypeRoomExit2))
							{
								if (minimizeLoopDistance)
								{
									IntVector2 intVector3 = currentLoopTargetBasePosition + prototypeRoomExit2.GetExitOrigin(prototypeRoomExit2.exitLength) - IntVector2.One;
									IntVector2 intVector4 = intVector2 + prototypeRoomExit.GetExitOrigin(prototypeRoomExit.exitLength) - IntVector2.One;
									num2 = IntVector2.ManhattanDistance(intVector3, intVector4);
								}
								num = Mathf.Min(num2, num);
							}
						}
					}
				}
				array[i] = num;
			}
			if (minimizeLoopDistance)
			{
				List<Tuple<int, Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>> list = new List<Tuple<int, Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>>();
				for (int l = 0; l < exitPairsSimple.Count; l++)
				{
					list.Add(new Tuple<int, Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>(array[l], exitPairsSimple[l]));
				}
				list.Sort((Tuple<int, Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> a, Tuple<int, Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> b) => a.First.CompareTo(b.First));
				for (int m = 0; m < exitPairsSimple.Count; m++)
				{
					exitPairsSimple[m] = list[m].Second;
				}
			}
			return exitPairsSimple;
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001D0C98 File Offset: 0x001CEE98
		protected IEnumerable BuildLoopComposite(SemioticLayoutManager layout, IntVector2 startPosition)
		{
			int numNodes = this.m_containedNodes.Count;
			BuilderFlowNode previousNodeL = this.m_containedNodes[0];
			BuilderFlowNode previousNodeR = this.m_containedNodes[0];
			this.AcquireRoomIfNecessary(this.m_containedNodes[0]);
			RoomHandler previousRoomL = LoopBuilderComposite.PlaceRoom(this.m_containedNodes[0], layout, startPosition);
			RoomHandler previousRoomR = previousRoomL;
			Guid loopGuid = Guid.NewGuid();
			previousRoomL.IsLoopMember = true;
			previousRoomL.LoopGuid = loopGuid;
			IntVector2 previousRoomLBasePosition = startPosition;
			IntVector2 previousRoomRBasePosition = startPosition;
			List<FlowActionLine> actionLines = new List<FlowActionLine>();
			int roomIndexL = 1;
			int roomIndexR = numNodes - 1;
			while (roomIndexL <= roomIndexR)
			{
				bool shouldMinimizeLoopDistance = numNodes <= 3 || (float)roomIndexL > (float)numNodes / 4f;
				BuilderFlowNode nextNodeL = this.m_containedNodes[roomIndexL];
				BuilderFlowNode nextNodeR = this.m_containedNodes[roomIndexR];
				bool acquisitionSuccess = this.AcquireRoomDirectionalIfNecessary(nextNodeL, previousNodeL);
				if (!acquisitionSuccess)
				{
					this.LoopCompositeBuildSuccess = false;
					yield break;
				}
				acquisitionSuccess = this.AcquireRoomDirectionalIfNecessary(nextNodeR, previousNodeR);
				if (!acquisitionSuccess)
				{
					this.LoopCompositeBuildSuccess = false;
					yield break;
				}
				List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsL = this.GetExitPairsForNode(previousNodeL, nextNodeL, previousRoomLBasePosition, previousNodeR, previousRoomRBasePosition, previousRoomR, actionLines, shouldMinimizeLoopDistance);
				bool success = false;
				for (int i = 0; i < exitPairsL.Count; i++)
				{
					Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = exitPairsL[i];
					if (layout.CanPlaceRoomAtAttachPointByExit2(nextNodeL.assignedPrototypeRoom, tuple.Second, previousRoomLBasePosition, tuple.First))
					{
						IntVector2 intVector = previousRoomLBasePosition + tuple.First.ExitOrigin - IntVector2.One;
						IntVector2 intVector2 = intVector - (tuple.Second.ExitOrigin - IntVector2.One);
						RoomHandler roomHandler = LoopBuilderComposite.PlaceRoom(nextNodeL, layout, intVector2);
						if (roomHandler != null)
						{
							roomHandler.IsLoopMember = true;
							roomHandler.LoopGuid = loopGuid;
						}
						if ((nextNodeL.loopConnectedBuilderNode == previousNodeL || previousNodeL.loopConnectedBuilderNode == nextNodeL) && (nextNodeL.loopConnectedBuilderNode.node.loopTargetIsOneWay || nextNodeL.node.loopTargetIsOneWay))
						{
							tuple.First.oneWayDoor = true;
							tuple.Second.oneWayDoor = true;
						}
						LoopBuilderComposite.HandleAdditionalRoomPlacementData(tuple, nextNodeL, previousNodeL, layout);
						FlowActionLine flowActionLine = new FlowActionLine(roomHandler.GetCenterCell().ToCenterVector2(), previousRoomL.GetCenterCell().ToCenterVector2());
						actionLines.Add(flowActionLine);
						previousNodeL = nextNodeL;
						previousRoomL = roomHandler;
						previousRoomLBasePosition = intVector2;
						success = true;
						break;
					}
				}
				if (!success)
				{
					BraveUtility.Log("No fitting placements L.", Color.white, BraveUtility.LogVerbosity.VERBOSE);
					this.LoopCompositeBuildSuccess = false;
					yield break;
				}
				yield return null;
				if (roomIndexL != roomIndexR)
				{
					List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsForNode = this.GetExitPairsForNode(previousNodeR, nextNodeR, previousRoomRBasePosition, previousNodeL, previousRoomLBasePosition, previousRoomL, actionLines, shouldMinimizeLoopDistance);
					bool flag = false;
					for (int j = 0; j < exitPairsForNode.Count; j++)
					{
						Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple2 = exitPairsForNode[j];
						if (layout.CanPlaceRoomAtAttachPointByExit2(nextNodeR.assignedPrototypeRoom, tuple2.Second, previousRoomRBasePosition, tuple2.First))
						{
							IntVector2 intVector3 = previousRoomRBasePosition + tuple2.First.ExitOrigin - IntVector2.One;
							IntVector2 intVector4 = intVector3 - (tuple2.Second.ExitOrigin - IntVector2.One);
							RoomHandler roomHandler2 = LoopBuilderComposite.PlaceRoom(nextNodeR, layout, intVector4);
							if (roomHandler2 != null)
							{
								roomHandler2.IsLoopMember = true;
								roomHandler2.LoopGuid = loopGuid;
							}
							if ((nextNodeR.loopConnectedBuilderNode == previousNodeR || previousNodeR.loopConnectedBuilderNode == nextNodeR) && (nextNodeR.loopConnectedBuilderNode.node.loopTargetIsOneWay || nextNodeR.node.loopTargetIsOneWay))
							{
								tuple2.First.oneWayDoor = true;
								tuple2.Second.oneWayDoor = true;
							}
							LoopBuilderComposite.HandleAdditionalRoomPlacementData(tuple2, nextNodeR, previousNodeR, layout);
							FlowActionLine flowActionLine2 = new FlowActionLine(roomHandler2.GetCenterCell().ToCenterVector2(), previousRoomR.GetCenterCell().ToCenterVector2());
							actionLines.Add(flowActionLine2);
							previousNodeR = nextNodeR;
							previousRoomR = roomHandler2;
							previousRoomRBasePosition = intVector4;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						BraveUtility.Log("No fitting placements R.", Color.white, BraveUtility.LogVerbosity.VERBOSE);
						this.LoopCompositeBuildSuccess = false;
						yield break;
					}
				}
				yield return null;
				roomIndexL++;
				roomIndexR--;
			}
			RoomHandler loopRoom = LoopBuilderComposite.AttemptLoopClosure(layout, previousRoomL, previousRoomR, previousRoomLBasePosition, previousRoomRBasePosition, 0, this.m_flow);
			if (loopRoom != null)
			{
				loopRoom.IsLoopMember = true;
				loopRoom.LoopGuid = loopGuid;
			}
			if (loopRoom != null)
			{
				this.LoopCompositeBuildSuccess = true;
				yield break;
			}
			this.LoopCompositeBuildSuccess = false;
			yield break;
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001D0CCC File Offset: 0x001CEECC
		protected static IntRect GetExitRect(PrototypeRoomExit closestExitL, PrototypeRoomExit closestExitR, IntVector2 closestExitPositionL, IntVector2 closestExitPositionR)
		{
			IntVector2 intVector = IntVector2.Min(closestExitPositionL, closestExitPositionR);
			IntVector2 intVector2 = IntVector2.Max(closestExitPositionL, closestExitPositionR);
			if (closestExitPositionL.x < closestExitPositionR.x)
			{
				if (closestExitL.exitDirection == DungeonData.Direction.EAST)
				{
					intVector += IntVector2.Right;
				}
				if (closestExitR.exitDirection == DungeonData.Direction.NORTH || closestExitR.exitDirection == DungeonData.Direction.SOUTH)
				{
					intVector2 += IntVector2.Right * 2;
				}
			}
			else
			{
				if (closestExitR.exitDirection == DungeonData.Direction.EAST)
				{
					intVector += IntVector2.Right;
				}
				if (closestExitL.exitDirection == DungeonData.Direction.NORTH || closestExitL.exitDirection == DungeonData.Direction.SOUTH)
				{
					intVector2 += IntVector2.Right * 2;
				}
			}
			if (closestExitPositionL.y < closestExitPositionR.y)
			{
				if (closestExitR.exitDirection == DungeonData.Direction.EAST || closestExitR.exitDirection == DungeonData.Direction.WEST)
				{
					intVector2 += IntVector2.Up * 2;
				}
				else if (closestExitR.exitDirection == DungeonData.Direction.SOUTH)
				{
					intVector2 += IntVector2.Up;
				}
			}
			else if (closestExitL.exitDirection == DungeonData.Direction.EAST || closestExitL.exitDirection == DungeonData.Direction.WEST)
			{
				intVector2 += IntVector2.Up * 2;
			}
			else if (closestExitL.exitDirection == DungeonData.Direction.SOUTH)
			{
				intVector2 += IntVector2.Up;
			}
			return new IntRect(intVector.x, intVector.y, intVector2.x - intVector.x, intVector2.y - intVector.y);
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x001D0E5C File Offset: 0x001CF05C
		protected static RoomHandler AttemptLoopClosure(SemioticLayoutManager layout, RoomHandler previousRoomL, RoomHandler previousRoomR, IntVector2 previousRoomLBasePosition, IntVector2 previousRoomRBasePosition, int depth, DungeonFlow flow)
		{
			List<Tuple<PrototypeRoomExit, PrototypeRoomExit>> list = new List<Tuple<PrototypeRoomExit, PrototypeRoomExit>>();
			List<int> list2 = new List<int>();
			List<PrototypeRoomExit.ExitGroup> definedExitGroups = previousRoomL.area.prototypeRoom.exitData.GetDefinedExitGroups();
			bool flag = definedExitGroups.Count > 1;
			for (int i = 0; i < previousRoomL.area.instanceUsedExits.Count; i++)
			{
				definedExitGroups.Remove(previousRoomL.area.instanceUsedExits[i].exitGroup);
			}
			if (definedExitGroups.Count == 0)
			{
				flag = false;
			}
			List<PrototypeRoomExit.ExitGroup> definedExitGroups2 = previousRoomR.area.prototypeRoom.exitData.GetDefinedExitGroups();
			bool flag2 = definedExitGroups2.Count > 1;
			for (int j = 0; j < previousRoomR.area.instanceUsedExits.Count; j++)
			{
				definedExitGroups2.Remove(previousRoomR.area.instanceUsedExits[j].exitGroup);
			}
			if (definedExitGroups2.Count == 0)
			{
				flag2 = false;
			}
			int k = 0;
			while (k < previousRoomL.area.prototypeRoom.exitData.exits.Count)
			{
				PrototypeRoomExit prototypeRoomExit = previousRoomL.area.prototypeRoom.exitData.exits[k];
				if (!flag)
				{
					goto IL_17D;
				}
				bool flag3 = false;
				for (int l = 0; l < previousRoomL.area.instanceUsedExits.Count; l++)
				{
					if (previousRoomL.area.instanceUsedExits[l].exitGroup == prototypeRoomExit.exitGroup)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					goto IL_17D;
				}
				IL_2CA:
				k++;
				continue;
				IL_17D:
				int m = 0;
				while (m < previousRoomR.area.prototypeRoom.exitData.exits.Count)
				{
					PrototypeRoomExit prototypeRoomExit2 = previousRoomR.area.prototypeRoom.exitData.exits[m];
					if (!flag2)
					{
						goto IL_209;
					}
					bool flag4 = false;
					for (int n = 0; n < previousRoomR.area.instanceUsedExits.Count; n++)
					{
						if (previousRoomR.area.instanceUsedExits[n].exitGroup == prototypeRoomExit2.exitGroup)
						{
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						goto IL_209;
					}
					IL_2A3:
					m++;
					continue;
					IL_209:
					if (previousRoomL.area.instanceUsedExits.Contains(prototypeRoomExit) || previousRoomR.area.instanceUsedExits.Contains(prototypeRoomExit2))
					{
						goto IL_2A3;
					}
					IntVector2 intVector = previousRoomLBasePosition + prototypeRoomExit.GetExitOrigin(prototypeRoomExit.exitLength + 3) - IntVector2.One;
					IntVector2 intVector2 = previousRoomRBasePosition + prototypeRoomExit2.GetExitOrigin(prototypeRoomExit2.exitLength + 3) - IntVector2.One;
					int num = IntVector2.ManhattanDistance(intVector, intVector2);
					list.Add(new Tuple<PrototypeRoomExit, PrototypeRoomExit>(prototypeRoomExit, prototypeRoomExit2));
					list2.Add(num);
					goto IL_2A3;
				}
				goto IL_2CA;
			}
			List<Tuple<PrototypeRoomExit, PrototypeRoomExit>> list3 = (from v in list2.Zip(list, (int d, Tuple<PrototypeRoomExit, PrototypeRoomExit> p) => new
				{
					Dist = d,
					Pair = p
				})
				orderby v.Dist
				select v.Pair).ToList<Tuple<PrototypeRoomExit, PrototypeRoomExit>>();
			RuntimeRoomExitData runtimeRoomExitData = null;
			RuntimeRoomExitData runtimeRoomExitData2 = null;
			List<IntVector2> list4 = null;
			int num2 = int.MaxValue;
			for (int num3 = 0; num3 < list3.Count; num3++)
			{
				PrototypeRoomExit first = list3[num3].First;
				PrototypeRoomExit second = list3[num3].Second;
				IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(first.exitDirection);
				IntVector2 intVector2FromDirection2 = DungeonData.GetIntVector2FromDirection(second.exitDirection);
				int num4 = ((first.exitDirection != DungeonData.Direction.SOUTH && first.exitDirection != DungeonData.Direction.WEST) ? 3 : 2);
				int num5 = ((second.exitDirection != DungeonData.Direction.SOUTH && second.exitDirection != DungeonData.Direction.WEST) ? 3 : 2);
				IntVector2 intVector3 = previousRoomLBasePosition + first.GetExitOrigin(first.exitLength + num4) - IntVector2.One;
				IntVector2 intVector4 = previousRoomRBasePosition + second.GetExitOrigin(second.exitLength + num5) - IntVector2.One;
				if (IntVector2.ManhattanDistance(intVector3, intVector4) < LoopBuilderComposite.GetMaxLoopDistanceThreshold())
				{
					RuntimeRoomExitData runtimeRoomExitData3 = new RuntimeRoomExitData(first);
					runtimeRoomExitData3.additionalExitLength = 3;
					RuntimeRoomExitData runtimeRoomExitData4 = new RuntimeRoomExitData(second);
					runtimeRoomExitData4.additionalExitLength = 3;
					IntRect exitRect = LoopBuilderComposite.GetExitRect(first, second, intVector3, intVector4);
					bool flag5 = exitRect.Width > 6 && exitRect.Height > 6 && (exitRect.Width > 12 || exitRect.Height > 12) && exitRect.Area < 350 && exitRect.Aspect < 5f && exitRect.Aspect > 0.2f;
					if (intVector2FromDirection == intVector2FromDirection2)
					{
						flag5 = false;
					}
					RuntimeRoomExitData runtimeRoomExitData5 = new RuntimeRoomExitData(first);
					runtimeRoomExitData5.additionalExitLength = 1;
					RuntimeRoomExitData runtimeRoomExitData6 = new RuntimeRoomExitData(second);
					runtimeRoomExitData6.additionalExitLength = 1;
					layout.StampComplexExitTemporary(runtimeRoomExitData5, previousRoomL.area);
					layout.StampComplexExitTemporary(runtimeRoomExitData6, previousRoomR.area);
					if (flag5 && layout.CanPlaceRectangle(exitRect))
					{
						IntVector2 intVector5 = intVector2FromDirection + intVector2FromDirection2;
						IntRect intRect = exitRect;
						for (int num6 = 0; num6 < 5; num6++)
						{
							int num7 = ((intVector5.x >= 0) ? 0 : num6);
							int num8 = ((intVector5.x <= 0) ? 0 : num6);
							int num9 = ((intVector5.y >= 0) ? 0 : num6);
							int num10 = ((intVector5.y <= 0) ? 0 : num6);
							if (intVector5 == IntVector2.Zero)
							{
								if (intVector2FromDirection.y == 0 && intVector2FromDirection2.y == 0)
								{
									num9 = num6;
									num10 = num6;
								}
								else
								{
									num7 = num6;
									num8 = num6;
								}
							}
							IntRect intRect2 = new IntRect(exitRect.Left - num7, exitRect.Bottom - num9, exitRect.Width + num7 + num8, exitRect.Height + num9 + num10);
							flag5 = intRect2.Area < 350 && intRect2.Aspect < 5f && intRect2.Aspect > 0.2f;
							if (flag5 && layout.CanPlaceRectangle(intRect2))
							{
								intRect = intRect2;
							}
						}
						layout.ClearTemporary();
						return LoopBuilderComposite.PlaceProceduralPathRoom(intRect, runtimeRoomExitData3, runtimeRoomExitData4, previousRoomL, previousRoomR, layout);
					}
					layout.ClearTemporary();
					List<IntVector2> list5 = layout.PathfindHallwayCompact(intVector3, DungeonData.GetIntVector2FromDirection(first.exitDirection), intVector4);
					layout.ClearTemporary();
					if (list5 != null && !layout.CanPlaceRawCellPositions(list5))
					{
						list5 = null;
					}
					if (list5 != null && list5.Count > 0 && list5.Count < num2)
					{
						runtimeRoomExitData3.additionalExitLength = 0;
						runtimeRoomExitData4.additionalExitLength = 0;
						runtimeRoomExitData = runtimeRoomExitData3;
						runtimeRoomExitData2 = runtimeRoomExitData4;
						list4 = list5;
						num2 = list4.Count;
					}
				}
			}
			if (num2 > LoopBuilderComposite.GetMaxLoopDistanceThreshold())
			{
				return null;
			}
			if (list4 != null && list4.Count > 0)
			{
				return LoopBuilderComposite.ConstructPhantomCorridor(list4, runtimeRoomExitData, runtimeRoomExitData2, previousRoomL, previousRoomR, layout, depth, flow);
			}
			return null;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001D15E4 File Offset: 0x001CF7E4
		protected static RoomHandler ConstructPhantomCorridor(List<IntVector2> path, RuntimeRoomExitData closestExitL, RuntimeRoomExitData closestExitR, RoomHandler previousRoomL, RoomHandler previousRoomR, SemioticLayoutManager layout, int depth, DungeonFlow flow)
		{
			if (path.Count < 4)
			{
				return null;
			}
			return LoopBuilderComposite.PlaceProceduralPathRoom(path, closestExitL, closestExitR, previousRoomL, previousRoomR, layout);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001D1604 File Offset: 0x001CF804
		protected IEnumerable BuildCompositeDepthFirst(SemioticLayoutManager layout, IntVector2 startPosition)
		{
			BuilderFlowNode currentNode = this.m_containedNodes[0];
			this.AcquireRoomIfNecessary(currentNode);
			RoomHandler room = LoopBuilderComposite.PlaceRoom(currentNode, layout, startPosition);
			bool success = true;
			for (int i = 0; i < currentNode.childBuilderNodes.Count; i++)
			{
				BuilderFlowNode childNode = currentNode.childBuilderNodes[i];
				if (this.m_containedNodes.Contains(childNode))
				{
					LoopBuilderComposite.CompositeNodeBuildData compositeNodeBuildData = new LoopBuilderComposite.CompositeNodeBuildData(childNode, currentNode, room, startPosition);
					IEnumerator<ProcessStatus> enumerator = this.BuildCompositeNodeDepthFirst(layout, compositeNodeBuildData).GetEnumerator();
					success = false;
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == ProcessStatus.Success)
						{
							success = true;
							break;
						}
						if (enumerator.Current == ProcessStatus.Fail)
						{
							success = false;
							break;
						}
					}
				}
				if (!success)
				{
					break;
				}
				yield return null;
			}
			if (!success)
			{
				LoopBuilderComposite.RemoveRoom(currentNode, layout);
			}
			this.LinearCompositeBuildSuccess = success;
			yield break;
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x001D1638 File Offset: 0x001CF838
		protected bool BuildComposite(SemioticLayoutManager layout, IntVector2 startPosition)
		{
			BuilderFlowNode builderFlowNode = this.m_containedNodes[0];
			this.AcquireRoomIfNecessary(builderFlowNode);
			RoomHandler roomHandler = LoopBuilderComposite.PlaceRoom(builderFlowNode, layout, startPosition);
			Queue<LoopBuilderComposite.CompositeNodeBuildData> queue = new Queue<LoopBuilderComposite.CompositeNodeBuildData>();
			for (int i = 0; i < builderFlowNode.childBuilderNodes.Count; i++)
			{
				BuilderFlowNode builderFlowNode2 = builderFlowNode.childBuilderNodes[i];
				if (this.m_containedNodes.Contains(builderFlowNode2))
				{
					queue.Enqueue(new LoopBuilderComposite.CompositeNodeBuildData(builderFlowNode2, builderFlowNode, roomHandler, startPosition));
				}
			}
			bool flag = true;
			while (queue.Count > 0)
			{
				LoopBuilderComposite.CompositeNodeBuildData compositeNodeBuildData = queue.Dequeue();
				flag = this.BuildCompositeNode(layout, compositeNodeBuildData, queue);
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x001D16EC File Offset: 0x001CF8EC
		protected bool AcquireRoomDirectionalIfNecessary(BuilderFlowNode buildNode, BuilderFlowNode parentNode)
		{
			if (!buildNode.AcquiresRoomAsNecessary)
			{
				return true;
			}
			PrototypeDungeonRoom.RoomCategory roomCategory = ((!buildNode.usesOverrideCategory) ? buildNode.node.roomCategory : buildNode.overrideCategory);
			GenericRoomTable genericRoomTable = this.m_flow.fallbackRoomTable;
			if (buildNode.node.overrideRoomTable != null)
			{
				genericRoomTable = buildNode.node.overrideRoomTable;
			}
			this.m_owner.ClearPlacedRoomData(buildNode);
			PrototypeDungeonRoom assignedPrototypeRoom = parentNode.assignedPrototypeRoom;
			List<DungeonData.Direction> list = new List<DungeonData.Direction>();
			for (int i = 0; i < assignedPrototypeRoom.exitData.exits.Count; i++)
			{
				if (!parentNode.exitToNodeMap.ContainsKey(assignedPrototypeRoom.exitData.exits[i]))
				{
					DungeonData.Direction direction = (assignedPrototypeRoom.exitData.exits[i].exitDirection + 4) % (DungeonData.Direction)8;
					list.Add(direction);
				}
			}
			PrototypeDungeonRoom availableRoomByExitDirection = this.m_owner.GetAvailableRoomByExitDirection(roomCategory, buildNode.Connectivity, list, genericRoomTable.GetCompiledList(), LoopFlowBuilder.FallbackLevel.NOT_FALLBACK);
			if (availableRoomByExitDirection != null)
			{
				buildNode.assignedPrototypeRoom = availableRoomByExitDirection;
				this.m_owner.NotifyPlacedRoomData(availableRoomByExitDirection);
				return true;
			}
			Debug.LogError("Failed to acquire a prototype room. This means the list is too sparse for the relevant category (" + roomCategory.ToString() + ") or something has gone terribly wrong. We should be falling back gracefully, though.");
			return false;
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x001D1844 File Offset: 0x001CFA44
		protected void AcquireRoomIfNecessary(BuilderFlowNode buildNode)
		{
			if (buildNode.AcquiresRoomAsNecessary)
			{
				PrototypeDungeonRoom.RoomCategory roomCategory = ((!buildNode.usesOverrideCategory) ? buildNode.node.roomCategory : buildNode.overrideCategory);
				GenericRoomTable genericRoomTable = this.m_flow.fallbackRoomTable;
				if (buildNode.node.overrideRoomTable != null)
				{
					genericRoomTable = buildNode.node.overrideRoomTable;
				}
				this.m_owner.ClearPlacedRoomData(buildNode);
				PrototypeDungeonRoom availableRoom = this.m_owner.GetAvailableRoom(roomCategory, buildNode.Connectivity, genericRoomTable.GetCompiledList(), LoopFlowBuilder.FallbackLevel.NOT_FALLBACK);
				if (availableRoom != null)
				{
					buildNode.assignedPrototypeRoom = availableRoom;
					this.m_owner.NotifyPlacedRoomData(availableRoom);
				}
				else
				{
					Debug.LogError("Failed to acquire a prototype room. This means the list is too sparse for the relevant category (" + roomCategory.ToString() + ") or something has gone terribly wrong.");
				}
			}
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001D1918 File Offset: 0x001CFB18
		protected static void HandleAdditionalRoomPlacementData(Tuple<RuntimeRoomExitData, RuntimeRoomExitData> exitPair, BuilderFlowNode nextNode, BuilderFlowNode previousNode, SemioticLayoutManager layout)
		{
			if (previousNode.nodeToExitMap.ContainsKey(nextNode))
			{
				previousNode.nodeToExitMap.Remove(nextNode);
			}
			if (nextNode.nodeToExitMap.ContainsKey(previousNode))
			{
				nextNode.nodeToExitMap.Remove(previousNode);
			}
			previousNode.exitToNodeMap.Add(exitPair.First.referencedExit, nextNode);
			previousNode.nodeToExitMap.Add(nextNode, exitPair.First.referencedExit);
			nextNode.exitToNodeMap.Add(exitPair.Second.referencedExit, previousNode);
			nextNode.nodeToExitMap.Add(previousNode, exitPair.Second.referencedExit);
			layout.StampComplexExitToLayout(exitPair.Second, nextNode.instanceRoom.area, false);
			layout.StampComplexExitToLayout(exitPair.First, previousNode.instanceRoom.area, false);
			exitPair.First.linkedExit = exitPair.Second;
			exitPair.Second.linkedExit = exitPair.First;
			if ((previousNode.parentBuilderNode == nextNode && previousNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.ONE_WAY) || (nextNode.parentBuilderNode == previousNode && nextNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.ONE_WAY))
			{
				exitPair.First.oneWayDoor = true;
				exitPair.Second.oneWayDoor = true;
			}
			if ((previousNode.parentBuilderNode == nextNode && previousNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.LOCKED) || (nextNode.parentBuilderNode == previousNode && nextNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.LOCKED))
			{
				exitPair.First.isLockedDoor = true;
				exitPair.Second.isLockedDoor = true;
			}
			previousNode.instanceRoom.RegisterConnectedRoom(nextNode.instanceRoom, exitPair.First);
			nextNode.instanceRoom.RegisterConnectedRoom(previousNode.instanceRoom, exitPair.Second);
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001D1AE4 File Offset: 0x001CFCE4
		protected static void UnhandleAdditionalRoomPlacementData(Tuple<RuntimeRoomExitData, RuntimeRoomExitData> exitPair, BuilderFlowNode nextNode, BuilderFlowNode previousNode, SemioticLayoutManager layout)
		{
			previousNode.exitToNodeMap.Remove(exitPair.First.referencedExit);
			previousNode.nodeToExitMap.Remove(nextNode);
			nextNode.exitToNodeMap.Remove(exitPair.Second.referencedExit);
			nextNode.nodeToExitMap.Remove(previousNode);
			layout.StampComplexExitToLayout(exitPair.Second, nextNode.instanceRoom.area, true);
			layout.StampComplexExitToLayout(exitPair.First, previousNode.instanceRoom.area, true);
			exitPair.First.linkedExit = null;
			exitPair.Second.linkedExit = null;
			exitPair.First.oneWayDoor = false;
			exitPair.Second.oneWayDoor = false;
			exitPair.First.isLockedDoor = false;
			exitPair.Second.isLockedDoor = false;
			previousNode.instanceRoom.DeregisterConnectedRoom(nextNode.instanceRoom, exitPair.First);
			nextNode.instanceRoom.DeregisterConnectedRoom(previousNode.instanceRoom, exitPair.Second);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001D1BE0 File Offset: 0x001CFDE0
		protected IEnumerable<ProcessStatus> BuildCompositeNodeDepthFirst(SemioticLayoutManager layout, LoopBuilderComposite.CompositeNodeBuildData currentBuildData)
		{
			if (!this.AcquireRoomDirectionalIfNecessary(currentBuildData.node, currentBuildData.parentNode))
			{
				yield return ProcessStatus.Fail;
				yield break;
			}
			if (currentBuildData.node.assignedPrototypeRoom == null && currentBuildData.node.node.priority == DungeonFlowNode.NodePriority.OPTIONAL)
			{
				yield return ProcessStatus.Success;
				yield break;
			}
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairs = this.GetExitPairsPreferDistance(currentBuildData.parentNode, currentBuildData.node, currentBuildData.parentBasePosition);
			BuilderFlowNode nextNode = currentBuildData.node;
			BuilderFlowNode previousNode = currentBuildData.parentNode;
			int numberChildFailures = 0;
			bool success = false;
			for (int i = 0; i < exitPairs.Count; i++)
			{
				Tuple<RuntimeRoomExitData, RuntimeRoomExitData> exitPair = exitPairs[i];
				IEnumerator<ProcessStatus> AttachTracker = layout.CanPlaceRoomAtAttachPointByExit(nextNode.assignedPrototypeRoom, exitPair.Second, currentBuildData.parentBasePosition, exitPair.First).GetEnumerator();
				bool attachSuccess = false;
				while (AttachTracker.MoveNext())
				{
					ProcessStatus currentStatus = AttachTracker.Current;
					if (currentStatus == ProcessStatus.Success)
					{
						attachSuccess = true;
					}
					else if (currentStatus == ProcessStatus.Fail)
					{
						attachSuccess = false;
					}
					else
					{
						yield return ProcessStatus.Incomplete;
					}
				}
				if (attachSuccess)
				{
					success = true;
					IntVector2 attachPoint = currentBuildData.parentBasePosition + exitPair.First.ExitOrigin - IntVector2.One;
					IntVector2 baseWorldPositionOfNewRoom = attachPoint - (exitPair.Second.ExitOrigin - IntVector2.One);
					RoomHandler newRoom = LoopBuilderComposite.PlaceRoom(nextNode, layout, baseWorldPositionOfNewRoom);
					if (newRoom.IsSecretRoom)
					{
						newRoom.AssignRoomVisualType(previousNode.instanceRoom.RoomVisualSubtype, false);
					}
					LoopBuilderComposite.HandleAdditionalRoomPlacementData(exitPair, nextNode, previousNode, layout);
					currentBuildData.connectionTuple = exitPair;
					List<LoopBuilderComposite.CompositeNodeBuildData> successfulChildren = new List<LoopBuilderComposite.CompositeNodeBuildData>();
					for (int j = 0; j < nextNode.childBuilderNodes.Count; j++)
					{
						BuilderFlowNode childNode = nextNode.childBuilderNodes[j];
						if (this.m_containedNodes.Contains(childNode))
						{
							LoopBuilderComposite.CompositeNodeBuildData childBuildData = new LoopBuilderComposite.CompositeNodeBuildData(childNode, nextNode, newRoom, baseWorldPositionOfNewRoom);
							foreach (ProcessStatus currentState in this.BuildCompositeNodeDepthFirst(layout, childBuildData))
							{
								if (currentState == ProcessStatus.Fail)
								{
									success = false;
									numberChildFailures++;
									break;
								}
								if (currentState == ProcessStatus.Success)
								{
									success = true;
									break;
								}
								yield return ProcessStatus.Incomplete;
							}
							yield return ProcessStatus.Incomplete;
							if (!success)
							{
								break;
							}
							successfulChildren.Add(childBuildData);
						}
					}
					if (success)
					{
						break;
					}
					numberChildFailures++;
					for (int k = 0; k < successfulChildren.Count; k++)
					{
						LoopBuilderComposite.CompositeNodeBuildData childBuildData2 = successfulChildren[k];
						if (!(childBuildData2.node.assignedPrototypeRoom == null) || childBuildData2.node.node.priority != DungeonFlowNode.NodePriority.OPTIONAL)
						{
							LoopBuilderComposite.UnhandleAdditionalRoomPlacementData(childBuildData2.connectionTuple, childBuildData2.node, childBuildData2.parentNode, layout);
							LoopBuilderComposite.RemoveRoom(childBuildData2.node, layout);
							yield return ProcessStatus.Incomplete;
						}
					}
					LoopBuilderComposite.UnhandleAdditionalRoomPlacementData(exitPair, nextNode, previousNode, layout);
					LoopBuilderComposite.RemoveRoom(currentBuildData.node, layout);
					if (numberChildFailures > 3)
					{
						yield return ProcessStatus.Fail;
						break;
					}
				}
			}
			if (success)
			{
				yield return ProcessStatus.Success;
				yield break;
			}
			yield return ProcessStatus.Fail;
			yield break;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001D1C14 File Offset: 0x001CFE14
		protected bool BuildCompositeNode(SemioticLayoutManager layout, LoopBuilderComposite.CompositeNodeBuildData currentBuildData, Queue<LoopBuilderComposite.CompositeNodeBuildData> buildQueue)
		{
			if (!this.AcquireRoomDirectionalIfNecessary(currentBuildData.node, currentBuildData.parentNode))
			{
				return false;
			}
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairsSimple = this.GetExitPairsSimple(currentBuildData.parentNode, currentBuildData.node, currentBuildData.parentBasePosition);
			BuilderFlowNode node = currentBuildData.node;
			BuilderFlowNode parentNode = currentBuildData.parentNode;
			for (int i = 0; i < exitPairsSimple.Count; i++)
			{
				Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = exitPairsSimple[i];
				IEnumerator<ProcessStatus> enumerator = layout.CanPlaceRoomAtAttachPointByExit(node.assignedPrototypeRoom, tuple.Second, currentBuildData.parentBasePosition, tuple.First).GetEnumerator();
				bool flag = false;
				while (enumerator.MoveNext())
				{
					ProcessStatus processStatus = enumerator.Current;
					if (processStatus == ProcessStatus.Success)
					{
						flag = true;
					}
					else if (processStatus == ProcessStatus.Fail)
					{
						flag = false;
					}
				}
				if (flag)
				{
					IntVector2 intVector = currentBuildData.parentBasePosition + tuple.First.ExitOrigin - IntVector2.One;
					IntVector2 intVector2 = intVector - (tuple.Second.ExitOrigin - IntVector2.One);
					RoomHandler roomHandler = LoopBuilderComposite.PlaceRoom(node, layout, intVector2);
					LoopBuilderComposite.HandleAdditionalRoomPlacementData(tuple, node, parentNode, layout);
					for (int j = 0; j < node.childBuilderNodes.Count; j++)
					{
						BuilderFlowNode builderFlowNode = node.childBuilderNodes[j];
						if (this.m_containedNodes.Contains(builderFlowNode))
						{
							LoopBuilderComposite.CompositeNodeBuildData compositeNodeBuildData = new LoopBuilderComposite.CompositeNodeBuildData(builderFlowNode, node, roomHandler, intVector2);
							buildQueue.Enqueue(compositeNodeBuildData);
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001D1DA0 File Offset: 0x001CFFA0
		protected void PostprocessLoopDirectionality()
		{
			bool flag = false;
			for (int i = 0; i < this.m_containedNodes.Count; i++)
			{
				if (this.m_containedNodes[i].loopConnectedBuilderNode != null && this.m_containedNodes.Contains(this.m_containedNodes[i].loopConnectedBuilderNode) && this.m_containedNodes[i].node.loopTargetIsOneWay)
				{
					flag = true;
				}
			}
			for (int j = 0; j < this.m_containedNodes.Count; j++)
			{
				if (this.m_containedNodes[j].instanceRoom != null)
				{
					this.m_containedNodes[j].instanceRoom.LoopIsUnidirectional = flag;
				}
			}
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001D1E68 File Offset: 0x001D0068
		public IEnumerable Build(IntVector2 startPosition)
		{
			if (this.CompletedCanvas != null)
			{
				this.CompletedCanvas.OnDestroy();
			}
			this.CompletedCanvas = null;
			SemioticLayoutManager canvas = new SemioticLayoutManager();
			if (this.loopStyle == LoopBuilderComposite.CompositeStyle.LOOP)
			{
				this.LoopCompositeBuildSuccess = false;
				IEnumerator buildTracker = this.BuildLoopComposite(canvas, startPosition).GetEnumerator();
				while (buildTracker.MoveNext())
				{
					yield return null;
				}
				if (this.LoopCompositeBuildSuccess)
				{
					this.PostprocessLoopDirectionality();
				}
				this.RequiresRegeneration = !this.LoopCompositeBuildSuccess;
			}
			else if (this.loopStyle == LoopBuilderComposite.CompositeStyle.NON_LOOP)
			{
				this.LinearCompositeBuildSuccess = false;
				IEnumerator buildTracker2 = this.BuildCompositeDepthFirst(canvas, startPosition).GetEnumerator();
				while (buildTracker2.MoveNext())
				{
					yield return null;
				}
				this.RequiresRegeneration = !this.LinearCompositeBuildSuccess;
			}
			this.CompletedCanvas = canvas;
			yield break;
		}

		// Token: 0x0400499B RID: 18843
		protected const int MIN_PATH_THRESHOLD = 4;

		// Token: 0x0400499C RID: 18844
		protected const int MIN_PATH_PHANTOM_THRESHOLD = 10;

		// Token: 0x0400499D RID: 18845
		protected const int MAX_LOOP_DISTANCE_THRESHOLD = 30;

		// Token: 0x0400499E RID: 18846
		protected const int MAX_PROC_RECTANGLE_AREA = 350;

		// Token: 0x0400499F RID: 18847
		protected const int MAX_LOOP_DISTANCE_THRESHOLD_MINES = 50;

		// Token: 0x040049A0 RID: 18848
		public bool RequiresRegeneration;

		// Token: 0x040049A1 RID: 18849
		public LoopBuilderComposite.CompositeStyle loopStyle;

		// Token: 0x040049A2 RID: 18850
		protected IntVector2 m_dimensions;

		// Token: 0x040049A3 RID: 18851
		protected List<BuilderFlowNode> m_containedNodes;

		// Token: 0x040049A4 RID: 18852
		protected Dictionary<BuilderFlowNode, BuilderFlowNode> m_externalToInternalNodeMap;

		// Token: 0x040049A5 RID: 18853
		protected List<BuilderFlowNode> m_externalConnectedNodes;

		// Token: 0x040049A6 RID: 18854
		protected LoopFlowBuilder m_owner;

		// Token: 0x040049A7 RID: 18855
		protected DungeonFlow m_flow;

		// Token: 0x040049A8 RID: 18856
		protected bool LoopCompositeBuildSuccess;

		// Token: 0x040049A9 RID: 18857
		private const bool DO_PHANTOM_CORRIDORS = false;

		// Token: 0x040049AA RID: 18858
		protected bool LinearCompositeBuildSuccess;

		// Token: 0x040049AB RID: 18859
		public SemioticLayoutManager CompletedCanvas;

		// Token: 0x02000EF7 RID: 3831
		public enum CompositeStyle
		{
			// Token: 0x040049B1 RID: 18865
			NON_LOOP,
			// Token: 0x040049B2 RID: 18866
			LOOP
		}

		// Token: 0x02000EF8 RID: 3832
		protected class CompositeNodeBuildData
		{
			// Token: 0x060051B9 RID: 20921 RVA: 0x001D1EC4 File Offset: 0x001D00C4
			public CompositeNodeBuildData(BuilderFlowNode n, BuilderFlowNode parent, RoomHandler pRoom, IntVector2 pbp)
			{
				this.node = n;
				this.parentNode = parent;
				this.parentRoom = pRoom;
				this.parentBasePosition = pbp;
			}

			// Token: 0x040049B3 RID: 18867
			public BuilderFlowNode node;

			// Token: 0x040049B4 RID: 18868
			public BuilderFlowNode parentNode;

			// Token: 0x040049B5 RID: 18869
			public RoomHandler parentRoom;

			// Token: 0x040049B6 RID: 18870
			public IntVector2 parentBasePosition;

			// Token: 0x040049B7 RID: 18871
			public Tuple<RuntimeRoomExitData, RuntimeRoomExitData> connectionTuple;
		}
	}
}
