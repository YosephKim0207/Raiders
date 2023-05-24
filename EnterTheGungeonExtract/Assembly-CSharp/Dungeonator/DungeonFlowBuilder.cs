using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EE1 RID: 3809
	public class DungeonFlowBuilder
	{
		// Token: 0x06005120 RID: 20768 RVA: 0x001CB424 File Offset: 0x001C9624
		public DungeonFlowBuilder(DungeonFlow flow, SemioticLayoutManager layout)
		{
			this.coreAreas = new List<RoomHandler>();
			this.additionalAreas = new List<RoomHandler>();
			this.m_flow = flow;
			this.m_layoutRef = layout;
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x001CB484 File Offset: 0x001C9684
		public SemioticLayoutManager Layout
		{
			get
			{
				return this.m_layoutRef;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x001CB48C File Offset: 0x001C968C
		public RoomHandler StartRoom
		{
			get
			{
				return this.coreAreas[0];
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x001CB49C File Offset: 0x001C969C
		public RoomHandler EndRoom
		{
			get
			{
				return this.coreAreas[this.coreAreas.Count - 1];
			}
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x001CB4B8 File Offset: 0x001C96B8
		private int ContainsPrototypeRoom(PrototypeDungeonRoom r)
		{
			int num = 0;
			for (int i = 0; i < this.coreAreas.Count; i++)
			{
				if (this.coreAreas[i].area.prototypeRoom == r)
				{
					num++;
				}
			}
			for (int j = 0; j < this.additionalAreas.Count; j++)
			{
				if (this.additionalAreas[j].area.prototypeRoom == r)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x001CB54C File Offset: 0x001C974C
		private PrototypeRoomExit RoomIsViableAtPosition(PrototypeDungeonRoom room, IntVector2 attachPoint, DungeonData.Direction newRoomExitDirection)
		{
			if (!room.CheckPrerequisites())
			{
				return null;
			}
			List<PrototypeRoomExit> unusedExitsOnSide = room.exitData.GetUnusedExitsOnSide(newRoomExitDirection);
			PrototypeRoomExit prototypeRoomExit = null;
			for (int i = 0; i < unusedExitsOnSide.Count; i++)
			{
				if (unusedExitsOnSide[i].exitType == PrototypeRoomExit.ExitType.EXIT_ONLY)
				{
					return null;
				}
				if (this.m_layoutRef.CanPlaceRoomAtAttachPointByExit(room, unusedExitsOnSide[i], attachPoint))
				{
					prototypeRoomExit = unusedExitsOnSide[i];
					break;
				}
			}
			if (prototypeRoomExit == null)
			{
				return null;
			}
			return prototypeRoomExit;
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x001CB5D0 File Offset: 0x001C97D0
		private Dictionary<WeightedRoom, PrototypeRoomExit> GetViableRoomsFromList(List<WeightedRoom> source, PrototypeDungeonRoom.RoomCategory category, IntVector2 attachPoint, DungeonData.Direction newRoomExitDirection)
		{
			Dictionary<WeightedRoom, PrototypeRoomExit> dictionary = new Dictionary<WeightedRoom, PrototypeRoomExit>();
			List<int> list = Enumerable.Range(0, source.Count).ToList<int>().GenerationShuffle<int>();
			for (int i = 0; i < source.Count; i++)
			{
				int num = list[i];
				WeightedRoom weightedRoom = source[num];
				PrototypeDungeonRoom room = weightedRoom.room;
				if (!Enum.IsDefined(typeof(PrototypeDungeonRoom.RoomCategory), category) || room.category == category)
				{
					if (weightedRoom.CheckPrerequisites())
					{
						if (room.CheckPrerequisites())
						{
							if (!weightedRoom.limitedCopies || this.ContainsPrototypeRoom(room) < weightedRoom.maxCopies)
							{
								List<PrototypeRoomExit> unusedExitsOnSide = room.exitData.GetUnusedExitsOnSide(newRoomExitDirection);
								PrototypeRoomExit prototypeRoomExit = null;
								for (int j = 0; j < unusedExitsOnSide.Count; j++)
								{
									if (unusedExitsOnSide[j].exitType != PrototypeRoomExit.ExitType.EXIT_ONLY)
									{
										bool flag = this.m_layoutRef.CanPlaceRoomAtAttachPointByExit(room, unusedExitsOnSide[j], attachPoint);
										if (flag)
										{
											prototypeRoomExit = unusedExitsOnSide[j];
											break;
										}
									}
								}
								if (prototypeRoomExit != null)
								{
									dictionary.Add(weightedRoom, prototypeRoomExit);
								}
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001CB730 File Offset: 0x001C9930
		private void AddActionLine(FlowActionLine line)
		{
			if (this.m_actionLines == null)
			{
				this.m_actionLines = new List<FlowActionLine>();
			}
			this.m_actionLines.Add(line);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001CB754 File Offset: 0x001C9954
		private bool CheckActionLineCrossings(Vector2 p1, Vector2 p2)
		{
			FlowActionLine flowActionLine = new FlowActionLine(p1, p2);
			for (int i = 0; i < this.m_actionLines.Count; i++)
			{
				if (this.m_actionLines[i].Crosses(flowActionLine))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001CB7A0 File Offset: 0x001C99A0
		private DungeonFlowNode SelectNodeByWeightingWithoutDuplicates(List<DungeonFlowNode> nodes, HashSet<DungeonFlowNode> duplicates)
		{
			float num = 0f;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!duplicates.Contains(nodes[i]))
				{
					num += nodes[i].percentChance;
				}
			}
			float num2 = BraveRandom.GenerationRandomValue() * num;
			float num3 = 0f;
			for (int j = 0; j < nodes.Count; j++)
			{
				if (!duplicates.Contains(nodes[j]))
				{
					num3 += nodes[j].percentChance;
					if (num3 > num2)
					{
						return nodes[j];
					}
				}
			}
			return nodes[nodes.Count - 1];
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001CB860 File Offset: 0x001C9A60
		private int SelectIndexByWeightingWithoutDuplicates(List<DungeonFlowBuilder.FlowRoomAttachData> chainRooms, HashSet<int> duplicates)
		{
			float num = 0f;
			for (int i = 0; i < chainRooms.Count; i++)
			{
				if (!duplicates.Contains(i))
				{
					num += chainRooms[i].weightedRoom.weight;
				}
			}
			float num2 = BraveRandom.GenerationRandomValue() * num;
			float num3 = 0f;
			for (int j = 0; j < chainRooms.Count; j++)
			{
				if (!duplicates.Contains(j))
				{
					num3 += chainRooms[j].weightedRoom.weight;
					if (num3 > num2)
					{
						return j;
					}
				}
			}
			return chainRooms.Count - 1;
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001CB91C File Offset: 0x001C9B1C
		private int SelectIndexByWeighting(List<WeightedRoom> chainRooms)
		{
			float num = 0f;
			for (int i = 0; i < chainRooms.Count; i++)
			{
				num += chainRooms[i].weight;
			}
			float num2 = BraveRandom.GenerationRandomValue() * num;
			float num3 = 0f;
			for (int j = 0; j < chainRooms.Count; j++)
			{
				num3 += chainRooms[j].weight;
				if (num3 > num2)
				{
					return j;
				}
			}
			return chainRooms.Count - 1;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001CB9A0 File Offset: 0x001C9BA0
		private WeightedRoom GetViableRoomPrototype(PrototypeDungeonRoom.RoomCategory category, IntVector2 attachPoint, DungeonData.Direction extendDirection, ref PrototypeRoomExit exitRef, List<WeightedRoom> roomTable)
		{
			DungeonData.Direction direction = (extendDirection + 4) % (DungeonData.Direction)8;
			Dictionary<WeightedRoom, PrototypeRoomExit> viableRoomsFromList = this.GetViableRoomsFromList(roomTable, category, attachPoint, direction);
			if (viableRoomsFromList.Keys.Count > 0)
			{
				WeightedRoom weightedRoom = viableRoomsFromList.Keys.First<WeightedRoom>();
				exitRef = viableRoomsFromList[weightedRoom];
				return weightedRoom;
			}
			return null;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001CB9EC File Offset: 0x001C9BEC
		public void DebugActionLines()
		{
			for (int i = 0; i < this.m_actionLines.Count; i++)
			{
				Debug.DrawLine(this.m_actionLines[i].point1, this.m_actionLines[i].point2, Color.yellow, 1000f);
			}
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x001CBA50 File Offset: 0x001C9C50
		private void RecomposeNodeStructure(FlowNodeBuildData currentNodeBuildData, DungeonChainStructure extantStructure, List<DungeonChainStructure> runningList)
		{
			DungeonFlowNode node = currentNodeBuildData.node;
			extantStructure.containedNodes.Add(currentNodeBuildData);
			if (!string.IsNullOrEmpty(node.loopTargetNodeGuid) || node.childNodeGuids.Count == 0)
			{
				runningList.Add(extantStructure);
				extantStructure = null;
			}
			if (currentNodeBuildData.childBuildData == null)
			{
				currentNodeBuildData.childBuildData = this.m_flow.GetNodeChildrenToBuild(node, this);
			}
			for (int i = 0; i < currentNodeBuildData.childBuildData.Count; i++)
			{
				FlowNodeBuildData flowNodeBuildData = currentNodeBuildData.childBuildData[i];
				DungeonChainStructure dungeonChainStructure = ((i != 0) ? null : extantStructure);
				if (dungeonChainStructure == null)
				{
					dungeonChainStructure = new DungeonChainStructure();
					dungeonChainStructure.parentNode = currentNodeBuildData;
				}
				this.RecomposeNodeStructure(flowNodeBuildData, dungeonChainStructure, runningList);
			}
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001CBB0C File Offset: 0x001C9D0C
		private void DecomposeLoopSubchains(List<DungeonChainStructure> subchains)
		{
			for (int i = 0; i < subchains.Count; i++)
			{
				DungeonChainStructure dungeonChainStructure = subchains[i];
				if (dungeonChainStructure.optionalRequiredNode != null && dungeonChainStructure.containedNodes.Count > 1)
				{
					int count = dungeonChainStructure.containedNodes.Count;
					int num = BraveRandom.GenerationRandomRange(1, count - 1);
					List<FlowNodeBuildData> list = new List<FlowNodeBuildData>();
					for (int j = count - 1; j >= num; j--)
					{
						list.Add(dungeonChainStructure.containedNodes[j]);
						dungeonChainStructure.containedNodes.RemoveAt(j);
					}
					DungeonChainStructure dungeonChainStructure2 = new DungeonChainStructure();
					dungeonChainStructure.optionalRequiredNode.childBuildData.Add(list[0]);
					dungeonChainStructure2.parentNode = dungeonChainStructure.optionalRequiredNode;
					dungeonChainStructure2.containedNodes = list;
					dungeonChainStructure2.optionalRequiredNode = dungeonChainStructure.containedNodes[dungeonChainStructure.containedNodes.Count - 1];
					dungeonChainStructure.optionalRequiredNode = dungeonChainStructure2.containedNodes[dungeonChainStructure2.containedNodes.Count - 1];
					if (dungeonChainStructure2.containedNodes.Count >= dungeonChainStructure.containedNodes.Count)
					{
						subchains.Insert(i + 1, dungeonChainStructure2);
					}
					else
					{
						subchains.Insert(i, dungeonChainStructure2);
					}
					i++;
				}
			}
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001CBC58 File Offset: 0x001C9E58
		protected List<DungeonChainStructure> ComposeBuildOrderSimple()
		{
			List<DungeonChainStructure> list = new List<DungeonChainStructure>();
			List<FlowNodeBuildData> list2 = new List<FlowNodeBuildData>();
			Stack<FlowNodeBuildData> stack = new Stack<FlowNodeBuildData>();
			stack.Push(new FlowNodeBuildData(this.m_flow.FirstNode));
			while (stack.Count > 0)
			{
				FlowNodeBuildData flowNodeBuildData = stack.Pop();
				list2.Add(flowNodeBuildData);
				if (flowNodeBuildData.childBuildData == null)
				{
					flowNodeBuildData.childBuildData = this.m_flow.GetNodeChildrenToBuild(flowNodeBuildData.node, this);
				}
				for (int i = 0; i < flowNodeBuildData.childBuildData.Count; i++)
				{
					if (!stack.Contains(flowNodeBuildData.childBuildData[i]))
					{
						stack.Push(flowNodeBuildData.childBuildData[i]);
					}
				}
			}
			list.Add(new DungeonChainStructure
			{
				containedNodes = list2
			});
			return list;
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001CBD38 File Offset: 0x001C9F38
		public List<DungeonChainStructure> ComposeBuildOrder()
		{
			if (this.m_cachedComposedChains != null)
			{
				return this.m_cachedComposedChains;
			}
			this.m_cachedComposedChains = null;
			return this.ComposeBuildOrderSimple();
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001CBD5C File Offset: 0x001C9F5C
		public bool Build(RoomHandler startRoom)
		{
			this.m_debugger = new FlowBuilderDebugger();
			this.coreAreas.Add(startRoom);
			List<DungeonChainStructure> list = this.ComposeBuildOrder();
			bool flag = true;
			for (int i = 0; i < list.Count; i++)
			{
				DungeonChainStructure dungeonChainStructure = list[i];
				RoomHandler roomHandler = startRoom;
				if (i > 0)
				{
					roomHandler = dungeonChainStructure.parentNode.room;
				}
				flag = this.BuildNode(dungeonChainStructure.containedNodes[0], roomHandler, null, true);
				if (!flag)
				{
					break;
				}
			}
			if (!flag)
			{
				this.coreAreas.RemoveAt(0);
			}
			this.m_debugger.FinalizeLog();
			return flag;
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001CBE00 File Offset: 0x001CA000
		protected void ShuffleExitsByMetric(ref List<PrototypeRoomExit> unusedExits, PrototypeRoomExit previouslyUsedExit)
		{
			switch (this.exitMetric)
			{
			case ChainSetupData.ExitPreferenceMetric.RANDOM:
				unusedExits = unusedExits.GenerationShuffle<PrototypeRoomExit>();
				break;
			case ChainSetupData.ExitPreferenceMetric.HORIZONTAL:
				break;
			case ChainSetupData.ExitPreferenceMetric.VERTICAL:
				Debug.LogError("Vertical not yet implemented");
				break;
			case ChainSetupData.ExitPreferenceMetric.FARTHEST:
				if (previouslyUsedExit == null)
				{
					unusedExits = unusedExits.GenerationShuffle<PrototypeRoomExit>();
				}
				else
				{
					unusedExits = unusedExits.OrderBy((PrototypeRoomExit a) => IntVector2.ManhattanDistance(a.GetExitOrigin(a.exitLength), previouslyUsedExit.GetExitOrigin(previouslyUsedExit.exitLength))).ToList<PrototypeRoomExit>();
				}
				break;
			case ChainSetupData.ExitPreferenceMetric.NEAREST:
				if (previouslyUsedExit == null)
				{
					unusedExits = unusedExits.GenerationShuffle<PrototypeRoomExit>();
				}
				else
				{
					unusedExits = unusedExits.OrderByDescending((PrototypeRoomExit a) => Vector2.Distance(a.GetExitOrigin(a.exitLength).ToVector2(), previouslyUsedExit.GetExitOrigin(previouslyUsedExit.exitLength).ToVector2())).ToList<PrototypeRoomExit>();
				}
				break;
			default:
				unusedExits = unusedExits.GenerationShuffle<PrototypeRoomExit>();
				break;
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001CBEE0 File Offset: 0x001CA0E0
		private List<DungeonFlowBuilder.FlowRoomAttachData> GetViableRoomsForExits(CellArea areaToExtendFrom, PrototypeDungeonRoom.RoomCategory nextRoomCategory, List<PrototypeRoomExit> unusedExits, List<WeightedRoom> roomTable)
		{
			List<DungeonFlowBuilder.FlowRoomAttachData> list = new List<DungeonFlowBuilder.FlowRoomAttachData>();
			for (int i = 0; i < unusedExits.Count; i++)
			{
				PrototypeRoomExit prototypeRoomExit = unusedExits[i];
				IntVector2 intVector = areaToExtendFrom.basePosition + prototypeRoomExit.GetExitOrigin(prototypeRoomExit.exitLength) - IntVector2.One;
				DungeonData.Direction exitDirection = prototypeRoomExit.exitDirection;
				DungeonData.Direction direction = (exitDirection + 4) % (DungeonData.Direction)8;
				Dictionary<WeightedRoom, PrototypeRoomExit> viableRoomsFromList = this.GetViableRoomsFromList(roomTable, nextRoomCategory, intVector, direction);
				foreach (WeightedRoom weightedRoom in viableRoomsFromList.Keys)
				{
					list.Add(new DungeonFlowBuilder.FlowRoomAttachData(weightedRoom, viableRoomsFromList[weightedRoom], prototypeRoomExit));
				}
			}
			return list;
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001CBFB8 File Offset: 0x001CA1B8
		private void RecursivelyUnstampChildren(FlowNodeBuildData buildData)
		{
			for (int i = 0; i < buildData.childBuildData.Count; i++)
			{
				this.RecursivelyUnstampChildren(buildData.childBuildData[i]);
			}
			if (buildData.room != null)
			{
				this.m_layoutRef.StampCellAreaToLayout(buildData.room, true);
				if (buildData.room.flowActionLine != null)
				{
					this.m_actionLines.Remove(buildData.room.flowActionLine);
					buildData.room.flowActionLine = null;
				}
				if (this.coreAreas.Contains(buildData.room))
				{
					this.coreAreas.Remove(buildData.room);
				}
				this.roomToUndoDataMap.Remove(buildData.room);
				buildData.UnmarkExits();
			}
			buildData.unbuilt = true;
			this.dataToRoomMap.Remove(buildData);
			buildData.room = null;
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x001CC0A0 File Offset: 0x001CA2A0
		private bool HandleNodeChildren(FlowNodeBuildData originalNodeBuildData, DungeonChainStructure chain)
		{
			originalNodeBuildData.MarkExits();
			originalNodeBuildData.unbuilt = false;
			FlowActionLine flowActionLine = new FlowActionLine(originalNodeBuildData.room.GetCenterCell().ToCenterVector2(), originalNodeBuildData.sourceRoom.GetCenterCell().ToCenterVector2());
			this.AddActionLine(flowActionLine);
			originalNodeBuildData.room.flowActionLine = flowActionLine;
			bool flag = true;
			if (chain != null)
			{
				int num = chain.containedNodes.IndexOf(originalNodeBuildData) + 1;
				if (num >= chain.containedNodes.Count)
				{
					if (chain.optionalRequiredNode != null && chain.optionalRequiredNode.room != null && !GameManager.Instance.Dungeon.debugSettings.DISABLE_LOOPS)
					{
						flag = this.BuildLoopNode(originalNodeBuildData, chain.optionalRequiredNode, chain);
					}
				}
				else
				{
					flag = this.BuildNode(chain.containedNodes[num], originalNodeBuildData.room, chain, false);
					if (chain.containedNodes[num].node.priority == DungeonFlowNode.NodePriority.OPTIONAL)
					{
						flag = true;
					}
				}
			}
			else
			{
				for (int i = 0; i < originalNodeBuildData.childBuildData.Count; i++)
				{
					flag = this.BuildNode(originalNodeBuildData.childBuildData[i], originalNodeBuildData.room, null, false);
					if (originalNodeBuildData.childBuildData[i].node.priority == DungeonFlowNode.NodePriority.OPTIONAL)
					{
						flag = true;
					}
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x001CC21C File Offset: 0x001CA41C
		private IntRect GetSpanningRectangle(IntVector2 p1, IntVector2 p2, out bool valid)
		{
			int num = Math.Min(p1.x, p2.x);
			int num2 = Math.Min(p1.y, p2.y);
			int num3 = Math.Max(p1.x, p2.x);
			int num4 = Math.Max(p1.y, p2.y);
			IntRect intRect = new IntRect(num, num2, num3 - num, num4 - num2);
			valid = this.m_layoutRef.CanPlaceRectangle(intRect);
			return intRect;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x001CC29C File Offset: 0x001CA49C
		private bool BuildLoopNode(FlowNodeBuildData chainEndData, FlowNodeBuildData loopTargetData, DungeonChainStructure chain)
		{
			RoomHandler room = chainEndData.room;
			RoomHandler room2 = loopTargetData.room;
			CellArea area = room.area;
			CellArea area2 = room2.area;
			if (area2.prototypeRoom != null && area.prototypeRoom != null)
			{
				List<PrototypeRoomExit> unusedExitsFromInstance = area.prototypeRoom.exitData.GetUnusedExitsFromInstance(area);
				List<PrototypeRoomExit> unusedExitsFromInstance2 = area2.prototypeRoom.exitData.GetUnusedExitsFromInstance(area2);
				List<DungeonFlowBuilder.LoopPathData> list = new List<DungeonFlowBuilder.LoopPathData>();
				for (int i = 0; i < unusedExitsFromInstance.Count; i++)
				{
					for (int j = 0; j < unusedExitsFromInstance2.Count; j++)
					{
						PrototypeRoomExit prototypeRoomExit = unusedExitsFromInstance[i];
						PrototypeRoomExit prototypeRoomExit2 = unusedExitsFromInstance2[j];
						IntVector2 intVector = area.basePosition + prototypeRoomExit.GetExitOrigin(prototypeRoomExit.exitLength) - IntVector2.One + DungeonData.GetIntVector2FromDirection(prototypeRoomExit.exitDirection);
						IntVector2 intVector2 = area2.basePosition + prototypeRoomExit2.GetExitOrigin(prototypeRoomExit2.exitLength) - IntVector2.One + DungeonData.GetIntVector2FromDirection(prototypeRoomExit2.exitDirection);
						List<IntVector2> list2 = this.m_layoutRef.TraceHallway(intVector, intVector2, prototypeRoomExit.exitDirection, prototypeRoomExit2.exitDirection);
						if (list2 != null)
						{
							list.Add(new DungeonFlowBuilder.LoopPathData(list2, prototypeRoomExit, prototypeRoomExit2));
						}
					}
				}
				if (list.Count > 0)
				{
					DungeonFlowBuilder.LoopPathData loopPathData = list[0];
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].path.Count < loopPathData.path.Count)
						{
							loopPathData = list[k];
						}
					}
					IntVector2 intVector3 = new IntVector2(int.MaxValue, int.MaxValue);
					IntVector2 intVector4 = new IntVector2(int.MinValue, int.MinValue);
					for (int l = 0; l < loopPathData.path.Count; l++)
					{
						intVector3.x = Math.Min(intVector3.x, loopPathData.path[l].x);
						intVector3.y = Math.Min(intVector3.y, loopPathData.path[l].y);
						intVector4.x = Math.Max(intVector4.x, loopPathData.path[l].x);
						intVector4.y = Math.Max(intVector4.y, loopPathData.path[l].y);
					}
					for (int m = 0; m < loopPathData.path.Count; m++)
					{
						List<IntVector2> path;
						int num;
						(path = loopPathData.path)[num = m] = path[num] - intVector3;
					}
					RoomHandler roomHandler = new RoomHandler(new CellArea(intVector3, intVector4, 0)
					{
						proceduralCells = loopPathData.path
					});
					roomHandler.distanceFromEntrance = room2.distanceFromEntrance + 1;
					roomHandler.CalculateOpulence();
					this.coreAreas.Add(roomHandler);
					this.m_layoutRef.StampCellAreaToLayout(roomHandler, false);
					room.area.instanceUsedExits.Add(loopPathData.initialExit);
					room2.area.instanceUsedExits.Add(loopPathData.finalExit);
					roomHandler.parentRoom = room;
					roomHandler.childRooms.Add(room2);
					room.childRooms.Add(roomHandler);
					room.connectedRooms.Add(roomHandler);
					room.connectedRoomsByExit.Add(loopPathData.initialExit, roomHandler);
					room2.childRooms.Add(roomHandler);
					room2.connectedRooms.Add(roomHandler);
					room2.connectedRoomsByExit.Add(loopPathData.finalExit, roomHandler);
					return true;
				}
				if (unusedExitsFromInstance2.Count == 0 || unusedExitsFromInstance.Count == 0)
				{
					BraveUtility.Log("No free exits to generate loop. No loop generated.", Color.cyan, BraveUtility.LogVerbosity.CHATTY);
				}
				else
				{
					BraveUtility.Log("All loops failed. No loop generated.", Color.cyan, BraveUtility.LogVerbosity.CHATTY);
				}
			}
			else
			{
				Debug.LogError("Procedural rooms not implemented in loop generation yet!");
			}
			return false;
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x001CC6E0 File Offset: 0x001CA8E0
		private bool BuildNode(FlowNodeBuildData nodeBuildData, RoomHandler roomToExtendFrom, DungeonChainStructure chain, bool initial = false)
		{
			DungeonFlowNode node = nodeBuildData.node;
			if (node == null)
			{
				return true;
			}
			if (this.dataToRoomMap.ContainsKey(nodeBuildData))
			{
				Debug.LogError("FAILURE");
				this.RecursivelyUnstampChildren(nodeBuildData);
			}
			if (node.nodeType != DungeonFlowNode.ControlNodeType.ROOM)
			{
				DungeonFlowNode.ControlNodeType nodeType = node.nodeType;
				if (nodeType != DungeonFlowNode.ControlNodeType.SUBCHAIN)
				{
					if (nodeType == DungeonFlowNode.ControlNodeType.SELECTOR)
					{
						Debug.Break();
					}
				}
				else
				{
					Debug.Break();
				}
			}
			else
			{
				PrototypeDungeonRoom.RoomCategory roomCategory = ((!nodeBuildData.usesOverrideCategory) ? node.roomCategory : nodeBuildData.overrideCategory);
				CellArea area = roomToExtendFrom.area;
				if (!(area.prototypeRoom != null))
				{
					Debug.LogError("Procedural room handling not yet implemented!");
					return false;
				}
				List<PrototypeRoomExit> unusedExitsFromInstance = area.prototypeRoom.exitData.GetUnusedExitsFromInstance(area);
				PrototypeRoomExit prototypeRoomExit = null;
				if (area.instanceUsedExits.Count != 0)
				{
					prototypeRoomExit = area.instanceUsedExits[BraveRandom.GenerationRandomRange(0, area.instanceUsedExits.Count)];
				}
				if (chain != null && chain.optionalRequiredNode != null && chain.optionalRequiredNode.room != null)
				{
					for (int i = 0; i < unusedExitsFromInstance.Count; i++)
					{
						Vector2 vector = (area.basePosition + unusedExitsFromInstance[i].GetExitOrigin(unusedExitsFromInstance[i].exitLength) - IntVector2.One).ToCenterVector2();
						Vector2 vector2 = chain.optionalRequiredNode.room.GetCenterCell().ToCenterVector2();
						vector2 += (vector - vector2).normalized;
						if (this.CheckActionLineCrossings(vector, vector2))
						{
							unusedExitsFromInstance.RemoveAt(i);
							i--;
						}
					}
				}
				this.ShuffleExitsByMetric(ref unusedExitsFromInstance, prototypeRoomExit);
				List<WeightedRoom> list;
				if (node.UsesGlobalBossData)
				{
					list = GameManager.Instance.BossManager.SelectBossTable().GetCompiledList();
				}
				else if (node.overrideExactRoom != null)
				{
					WeightedRoom weightedRoom = new WeightedRoom();
					weightedRoom.room = node.overrideExactRoom;
					weightedRoom.weight = 1f;
					list = new List<WeightedRoom>();
					list.Add(weightedRoom);
					roomCategory = node.overrideExactRoom.category;
				}
				else
				{
					GenericRoomTable genericRoomTable = this.m_flow.fallbackRoomTable;
					if (node.overrideRoomTable != null)
					{
						genericRoomTable = node.overrideRoomTable;
					}
					list = genericRoomTable.GetCompiledList();
				}
				List<DungeonFlowBuilder.FlowRoomAttachData> viableRoomsForExits = this.GetViableRoomsForExits(area, roomCategory, unusedExitsFromInstance, list);
				int num = viableRoomsForExits.Count;
				for (int j = 0; j < num; j++)
				{
					DungeonFlowBuilder.FlowRoomAttachData flowRoomAttachData = viableRoomsForExits[j];
					if (this.ContainsPrototypeRoom(flowRoomAttachData.weightedRoom.room) > 0)
					{
						viableRoomsForExits.RemoveAt(j);
						viableRoomsForExits.Add(flowRoomAttachData);
						j--;
						num--;
					}
				}
				if (viableRoomsForExits == null || viableRoomsForExits.Count == 0)
				{
					return false;
				}
				if (nodeBuildData.childBuildData == null && this.m_flow.IsPartOfSubchain(nodeBuildData.node))
				{
					nodeBuildData.childBuildData = this.m_flow.GetNodeChildrenToBuild(nodeBuildData.node, this);
				}
				List<FlowNodeBuildData> childBuildData = nodeBuildData.childBuildData;
				int num2 = 0;
				for (int k = 0; k < childBuildData.Count; k++)
				{
					num2 += ((childBuildData[k].node.priority != DungeonFlowNode.NodePriority.MANDATORY) ? 0 : 1);
				}
				HashSet<int> hashSet = new HashSet<int>();
				for (int l = 0; l < viableRoomsForExits.Count; l++)
				{
					int num3 = this.SelectIndexByWeightingWithoutDuplicates(viableRoomsForExits, hashSet);
					hashSet.Add(num3);
					PrototypeDungeonRoom room = viableRoomsForExits[num3].weightedRoom.room;
					if (room.exitData.exits.Count >= num2 + 1)
					{
						PrototypeRoomExit exitOfNewRoom = viableRoomsForExits[num3].exitOfNewRoom;
						PrototypeRoomExit exitToUse = viableRoomsForExits[num3].exitToUse;
						IntVector2 intVector = area.basePosition + exitToUse.GetExitOrigin(exitToUse.exitLength) - IntVector2.One;
						IntVector2 intVector2 = intVector - (exitOfNewRoom.GetExitOrigin(exitOfNewRoom.exitLength) - IntVector2.One);
						if (chain != null && chain.optionalRequiredNode != null && chain.optionalRequiredNode.room != null)
						{
							if (chain.previousLoopDistanceMetric == 2147483647)
							{
								chain.previousLoopDistanceMetric = IntVector2.ManhattanDistance(chain.optionalRequiredNode.room.GetCenterCell(), intVector);
							}
							int num4 = int.MaxValue;
							for (int m = 0; m < room.exitData.exits.Count; m++)
							{
								if (room.exitData.exits[m] != exitOfNewRoom)
								{
									num4 = Math.Min(num4, IntVector2.ManhattanDistance(chain.optionalRequiredNode.room.GetCenterCell(), intVector2 + room.exitData.exits[m].GetExitOrigin(room.exitData.exits[m].exitLength) - IntVector2.One));
								}
							}
							if (num4 > chain.previousLoopDistanceMetric)
							{
								goto IL_697;
							}
							chain.previousLoopDistanceMetric = num4;
						}
						CellArea cellArea = new CellArea(intVector2, new IntVector2(room.Width, room.Height), 0);
						cellArea.prototypeRoom = room;
						cellArea.instanceUsedExits = new List<PrototypeRoomExit>();
						if (nodeBuildData.usesOverrideCategory)
						{
							cellArea.PrototypeRoomCategory = nodeBuildData.overrideCategory;
						}
						RoomHandler roomHandler = new RoomHandler(cellArea);
						roomHandler.distanceFromEntrance = roomToExtendFrom.distanceFromEntrance + 1;
						roomHandler.CalculateOpulence();
						roomHandler.CanReceiveCaps = node.receivesCaps;
						this.coreAreas.Add(roomHandler);
						this.m_layoutRef.StampCellAreaToLayout(roomHandler, false);
						nodeBuildData.room = roomHandler;
						nodeBuildData.roomEntrance = exitOfNewRoom;
						nodeBuildData.sourceExit = exitToUse;
						nodeBuildData.sourceRoom = roomToExtendFrom;
						this.roomToUndoDataMap.Add(roomHandler, nodeBuildData);
						this.dataToRoomMap.Add(nodeBuildData, roomHandler);
						this.m_debugger.Log(roomToExtendFrom, roomHandler);
						this.m_debugger.LogMonoHeapStatus();
						bool flag = this.HandleNodeChildren(nodeBuildData, chain);
						if (flag)
						{
							return true;
						}
						this.m_debugger.Log(roomToExtendFrom.area.prototypeRoom.name + " is falling back...");
						this.RecursivelyUnstampChildren(nodeBuildData);
					}
					IL_697:;
				}
			}
			this.m_debugger.Log(roomToExtendFrom.area.prototypeRoom.name + " completely failed.");
			return false;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001CCDC0 File Offset: 0x001CAFC0
		public void AppendCapChains()
		{
			List<RoomHandler> roomsWithViableExits = new List<RoomHandler>();
			List<PrototypeRoomExit> viableExitsToCap = new List<PrototypeRoomExit>();
			for (int i = 0; i < this.coreAreas.Count; i++)
			{
				PrototypeDungeonRoom prototypeRoom = this.coreAreas[i].area.prototypeRoom;
				if (!(prototypeRoom == null))
				{
					if (this.coreAreas[i].CanReceiveCaps)
					{
						for (int j = 0; j < prototypeRoom.exitData.exits.Count; j++)
						{
							if (!this.coreAreas[i].area.instanceUsedExits.Contains(prototypeRoom.exitData.exits[j]))
							{
								if (prototypeRoom.exitData.exits[j].exitType != PrototypeRoomExit.ExitType.ENTRANCE_ONLY)
								{
									roomsWithViableExits.Add(this.coreAreas[i]);
									viableExitsToCap.Add(prototypeRoom.exitData.exits[j]);
								}
							}
						}
					}
				}
			}
			List<int> list = Enumerable.Range(0, roomsWithViableExits.Count).ToList<int>();
			list = list.GenerationShuffle<int>();
			roomsWithViableExits = list.Select((int index) => roomsWithViableExits[index]).ToList<RoomHandler>();
			viableExitsToCap = list.Select((int index) => viableExitsToCap[index]).ToList<PrototypeRoomExit>();
			for (int k = 0; k < viableExitsToCap.Count; k++)
			{
				List<DungeonFlowNode> capChainRootNodes = this.m_flow.GetCapChainRootNodes(this);
				if (capChainRootNodes == null || capChainRootNodes.Count == 0)
				{
					break;
				}
				HashSet<DungeonFlowNode> hashSet = new HashSet<DungeonFlowNode>();
				DungeonFlowNode dungeonFlowNode = this.SelectNodeByWeightingWithoutDuplicates(capChainRootNodes, hashSet);
				hashSet.Add(dungeonFlowNode);
				bool flag = this.BuildNode(new FlowNodeBuildData(dungeonFlowNode)
				{
					childBuildData = this.m_flow.GetNodeChildrenToBuild(dungeonFlowNode, this)
				}, roomsWithViableExits[k], null, false);
				if (flag)
				{
					if (this.usedSubchainData.ContainsKey(dungeonFlowNode))
					{
						this.usedSubchainData[dungeonFlowNode] = this.usedSubchainData[dungeonFlowNode] + 1;
					}
					else
					{
						this.usedSubchainData.Add(dungeonFlowNode, 1);
					}
				}
			}
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x001CD038 File Offset: 0x001CB238
		public bool AttemptAppendExtraRoom(ExtraIncludedRoomData extraRoomData)
		{
			List<RoomHandler> roomsWithViableExits = new List<RoomHandler>();
			List<PrototypeRoomExit> viableExitsToCap = new List<PrototypeRoomExit>();
			for (int i = 0; i < this.coreAreas.Count; i++)
			{
				PrototypeDungeonRoom prototypeRoom = this.coreAreas[i].area.prototypeRoom;
				if (!(prototypeRoom == null))
				{
					for (int j = 0; j < prototypeRoom.exitData.exits.Count; j++)
					{
						if (!this.coreAreas[i].area.instanceUsedExits.Contains(prototypeRoom.exitData.exits[j]))
						{
							if (prototypeRoom.exitData.exits[j].exitType != PrototypeRoomExit.ExitType.ENTRANCE_ONLY)
							{
								roomsWithViableExits.Add(this.coreAreas[i]);
								viableExitsToCap.Add(prototypeRoom.exitData.exits[j]);
							}
						}
					}
				}
			}
			List<int> list = Enumerable.Range(0, roomsWithViableExits.Count).ToList<int>();
			list = list.GenerationShuffle<int>();
			roomsWithViableExits = list.Select((int index) => roomsWithViableExits[index]).ToList<RoomHandler>();
			viableExitsToCap = list.Select((int index) => viableExitsToCap[index]).ToList<PrototypeRoomExit>();
			for (int k = 0; k < viableExitsToCap.Count; k++)
			{
				PrototypeRoomExit prototypeRoomExit = viableExitsToCap[k];
				IntVector2 intVector = roomsWithViableExits[k].area.basePosition + prototypeRoomExit.GetExitOrigin(prototypeRoomExit.exitLength) - IntVector2.One;
				DungeonData.Direction direction = (prototypeRoomExit.exitDirection + 4) % (DungeonData.Direction)8;
				PrototypeRoomExit prototypeRoomExit2 = this.RoomIsViableAtPosition(extraRoomData.room, intVector, direction);
				if (prototypeRoomExit2 != null)
				{
					IntVector2 intVector2 = intVector - (prototypeRoomExit2.GetExitOrigin(prototypeRoomExit2.exitLength) - IntVector2.One);
					CellArea cellArea = new CellArea(intVector2, new IntVector2(extraRoomData.room.Width, extraRoomData.room.Height), 0);
					cellArea.prototypeRoom = extraRoomData.room;
					cellArea.instanceUsedExits = new List<PrototypeRoomExit>();
					RoomHandler roomHandler = new RoomHandler(cellArea);
					roomHandler.distanceFromEntrance = roomsWithViableExits[k].distanceFromEntrance + 1;
					roomHandler.CalculateOpulence();
					this.additionalAreas.Add(roomHandler);
					this.m_layoutRef.StampCellAreaToLayout(roomHandler, false);
					cellArea.instanceUsedExits.Add(prototypeRoomExit2);
					roomsWithViableExits[k].area.instanceUsedExits.Add(prototypeRoomExit);
					roomHandler.parentRoom = roomsWithViableExits[k];
					roomHandler.connectedRooms.Add(roomsWithViableExits[k]);
					roomHandler.connectedRoomsByExit.Add(prototypeRoomExit2, roomsWithViableExits[k]);
					roomsWithViableExits[k].childRooms.Add(roomHandler);
					roomsWithViableExits[k].connectedRooms.Add(roomHandler);
					roomsWithViableExits[k].connectedRoomsByExit.Add(prototypeRoomExit, roomHandler);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400492A RID: 18730
		public List<RoomHandler> coreAreas;

		// Token: 0x0400492B RID: 18731
		public List<RoomHandler> additionalAreas;

		// Token: 0x0400492C RID: 18732
		public Dictionary<DungeonFlowNode, int> usedSubchainData = new Dictionary<DungeonFlowNode, int>();

		// Token: 0x0400492D RID: 18733
		private Dictionary<RoomHandler, FlowNodeBuildData> roomToUndoDataMap = new Dictionary<RoomHandler, FlowNodeBuildData>();

		// Token: 0x0400492E RID: 18734
		private Dictionary<FlowNodeBuildData, RoomHandler> dataToRoomMap = new Dictionary<FlowNodeBuildData, RoomHandler>();

		// Token: 0x0400492F RID: 18735
		private List<DungeonChainStructure> m_cachedComposedChains;

		// Token: 0x04004930 RID: 18736
		private SemioticLayoutManager m_layoutRef;

		// Token: 0x04004931 RID: 18737
		private DungeonFlow m_flow;

		// Token: 0x04004932 RID: 18738
		private List<FlowActionLine> m_actionLines;

		// Token: 0x04004933 RID: 18739
		private ChainSetupData.ExitPreferenceMetric exitMetric = ChainSetupData.ExitPreferenceMetric.FARTHEST;

		// Token: 0x04004934 RID: 18740
		private FlowBuilderDebugger m_debugger;

		// Token: 0x02000EE2 RID: 3810
		private struct FlowRoomAttachData
		{
			// Token: 0x0600513C RID: 20796 RVA: 0x001CD3A8 File Offset: 0x001CB5A8
			public FlowRoomAttachData(WeightedRoom w, PrototypeRoomExit exitOfNew, PrototypeRoomExit exitOfOld)
			{
				this.weightedRoom = w;
				this.exitOfNewRoom = exitOfNew;
				this.exitToUse = exitOfOld;
			}

			// Token: 0x04004935 RID: 18741
			public WeightedRoom weightedRoom;

			// Token: 0x04004936 RID: 18742
			public PrototypeRoomExit exitOfNewRoom;

			// Token: 0x04004937 RID: 18743
			public PrototypeRoomExit exitToUse;
		}

		// Token: 0x02000EE3 RID: 3811
		internal class LoopPathData
		{
			// Token: 0x0600513D RID: 20797 RVA: 0x001CD3C0 File Offset: 0x001CB5C0
			public LoopPathData(List<IntVector2> path, PrototypeRoomExit initialExit, PrototypeRoomExit finalExit)
			{
				this.path = path;
				this.initialExit = initialExit;
				this.finalExit = finalExit;
			}

			// Token: 0x04004938 RID: 18744
			public List<IntVector2> path;

			// Token: 0x04004939 RID: 18745
			public PrototypeRoomExit initialExit;

			// Token: 0x0400493A RID: 18746
			public PrototypeRoomExit finalExit;
		}
	}
}
