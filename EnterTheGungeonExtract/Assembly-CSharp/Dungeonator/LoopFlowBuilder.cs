using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F07 RID: 3847
	public class LoopFlowBuilder
	{
		// Token: 0x06005207 RID: 20999 RVA: 0x001D42DC File Offset: 0x001D24DC
		public LoopFlowBuilder(DungeonFlow flow, LoopDungeonGenerator generator)
		{
			this.m_flow = flow;
			this.m_generator = generator;
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x001D434C File Offset: 0x001D254C
		public BuilderFlowNode ConstructNodeForInjection(PrototypeDungeonRoom exactRoom, ProceduralFlowModifierData modData, RuntimeInjectionMetadata optionalMetadata)
		{
			DungeonFlowNode dungeonFlowNode = new DungeonFlowNode(this.m_flow);
			dungeonFlowNode.overrideExactRoom = exactRoom;
			dungeonFlowNode.priority = DungeonFlowNode.NodePriority.MANDATORY;
			if (BraveRandom.GenerationRandomValue() < modData.chanceToLock)
			{
				dungeonFlowNode.forcedDoorType = DungeonFlowNode.ForcedDoorType.LOCKED;
			}
			BuilderFlowNode builderFlowNode = new BuilderFlowNode(dungeonFlowNode);
			builderFlowNode.assignedPrototypeRoom = exactRoom;
			builderFlowNode.childBuilderNodes = new List<BuilderFlowNode>();
			builderFlowNode.IsInjectedNode = true;
			if (optionalMetadata != null && optionalMetadata.forceSecret)
			{
				dungeonFlowNode.roomCategory = PrototypeDungeonRoom.RoomCategory.SECRET;
				builderFlowNode.usesOverrideCategory = true;
				builderFlowNode.overrideCategory = PrototypeDungeonRoom.RoomCategory.SECRET;
			}
			return builderFlowNode;
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x001D43D4 File Offset: 0x001D25D4
		protected void InjectValidator_RandomCombatRoom(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.parentBuilderNode != null && current.IsOfDepth(modData.RandomNodeChildMinDistanceFromEntrance) && !metastructure.ContainedInBidirectionalLoop(current) && !current.node.isWarpWingEntrance && current.IsStandardCategory && current.assignedPrototypeRoom != null && current.assignedPrototypeRoom.ContainsEnemies)
			{
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x001D4450 File Offset: 0x001D2650
		protected void InjectValidator_EndOfChain(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.parentBuilderNode != null && !current.node.isWarpWingEntrance && current.node.roomCategory != PrototypeDungeonRoom.RoomCategory.EXIT && current.childBuilderNodes.Count == 0 && current.Category != PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				if (current.parentBuilderNode != null && current.parentBuilderNode.node.isWarpWingEntrance)
				{
					return;
				}
				if (current.loopConnectedBuilderNode != null && !current.node.loopTargetIsOneWay)
				{
					return;
				}
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x001D44EC File Offset: 0x001D26EC
		protected void InjectValidator_HubAdjacentChainStart(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.parentBuilderNode != null && !current.node.isWarpWingEntrance && current.parentBuilderNode.Category == PrototypeDungeonRoom.RoomCategory.HUB)
			{
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001D4524 File Offset: 0x001D2724
		protected void InjectValidator_HubAdjacentNoLink(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.Category == PrototypeDungeonRoom.RoomCategory.HUB)
			{
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x001D453C File Offset: 0x001D273C
		protected void InjectValidator_RandomNodeChild(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.IsStandardCategory && !current.node.isWarpWingEntrance && current.node.roomCategory != PrototypeDungeonRoom.RoomCategory.EXIT && current.IsOfDepth(modData.RandomNodeChildMinDistanceFromEntrance))
			{
				if (current.parentBuilderNode != null && current.parentBuilderNode.node.isWarpWingEntrance)
				{
					return;
				}
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x001D45B0 File Offset: 0x001D27B0
		protected void InjectValidator_AfterBoss(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.parentBuilderNode != null && !current.node.isWarpWingEntrance && current.parentBuilderNode.Category == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				validNodes.Add(current);
			}
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x001D45E8 File Offset: 0x001D27E8
		protected void InjectValidator_BlackMarket(BuilderFlowNode current, List<BuilderFlowNode> validNodes, ProceduralFlowModifierData modData, FlowCompositeMetastructure metastructure)
		{
			if (current.assignedPrototypeRoom != null && current.assignedPrototypeRoom.name.Contains("Black Market"))
			{
				validNodes.Add(current);
			}
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001D461C File Offset: 0x001D281C
		protected void InjectNodeNoLinks(ProceduralFlowModifierData modData, PrototypeDungeonRoom exactRoom, BuilderFlowNode root, FlowCompositeMetastructure metastructure, RuntimeInjectionMetadata optionalMetadata)
		{
			BuilderFlowNode builderFlowNode = this.ConstructNodeForInjection(exactRoom, modData, optionalMetadata);
			builderFlowNode.node.isWarpWingEntrance = true;
			builderFlowNode.node.handlesOwnWarping = true;
			root.childBuilderNodes.Add(builderFlowNode);
			builderFlowNode.parentBuilderNode = root;
			builderFlowNode.InjectionTarget = root;
			this.allBuilderNodes.Add(builderFlowNode);
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x001D4674 File Offset: 0x001D2874
		protected bool InjectNodeBefore(ProceduralFlowModifierData modData, PrototypeDungeonRoom exactRoom, BuilderFlowNode root, Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure> validator, FlowCompositeMetastructure metastructure, RuntimeInjectionMetadata optionalMetadata)
		{
			optionalMetadata.forceSecret = false;
			BuilderFlowNode builderFlowNode = this.ConstructNodeForInjection(exactRoom, modData, optionalMetadata);
			List<BuilderFlowNode> list = new List<BuilderFlowNode>();
			Stack<BuilderFlowNode> stack = new Stack<BuilderFlowNode>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				BuilderFlowNode builderFlowNode2 = stack.Pop();
				validator(builderFlowNode2, list, modData, metastructure);
				for (int i = 0; i < builderFlowNode2.childBuilderNodes.Count; i++)
				{
					stack.Push(builderFlowNode2.childBuilderNodes[i]);
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			BuilderFlowNode builderFlowNode3 = list[BraveRandom.GenerationRandomRange(0, list.Count)];
			BuilderFlowNode parentBuilderNode = builderFlowNode3.parentBuilderNode;
			parentBuilderNode.childBuilderNodes.Remove(builderFlowNode3);
			parentBuilderNode.childBuilderNodes.Add(builderFlowNode);
			builderFlowNode.parentBuilderNode = parentBuilderNode;
			builderFlowNode3.parentBuilderNode = builderFlowNode;
			builderFlowNode.childBuilderNodes.Add(builderFlowNode3);
			builderFlowNode.InjectionTarget = builderFlowNode3;
			this.allBuilderNodes.Add(builderFlowNode);
			return true;
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x001D4778 File Offset: 0x001D2978
		protected bool InjectNodeAfter(ProceduralFlowModifierData modData, PrototypeDungeonRoom exactRoom, BuilderFlowNode root, Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure> validator, FlowCompositeMetastructure metastructure, RuntimeInjectionMetadata optionalMetadata)
		{
			BuilderFlowNode builderFlowNode = this.ConstructNodeForInjection(exactRoom, modData, optionalMetadata);
			builderFlowNode.node.isWarpWingEntrance = modData.IsWarpWing;
			List<BuilderFlowNode> list = new List<BuilderFlowNode>();
			Stack<BuilderFlowNode> stack = new Stack<BuilderFlowNode>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				BuilderFlowNode builderFlowNode2 = stack.Pop();
				validator(builderFlowNode2, list, modData, metastructure);
				for (int i = 0; i < builderFlowNode2.childBuilderNodes.Count; i++)
				{
					stack.Push(builderFlowNode2.childBuilderNodes[i]);
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			BuilderFlowNode builderFlowNode3 = list[BraveRandom.GenerationRandomRange(0, list.Count)];
			builderFlowNode3.childBuilderNodes.Add(builderFlowNode);
			builderFlowNode.parentBuilderNode = builderFlowNode3;
			builderFlowNode.childBuilderNodes = new List<BuilderFlowNode>();
			builderFlowNode.InjectionTarget = builderFlowNode3;
			this.allBuilderNodes.Add(builderFlowNode);
			return true;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x001D4864 File Offset: 0x001D2A64
		protected void RecurseCombatRooms(BuilderFlowNode currentCheckNode, List<BuilderFlowNode> currentSequence, int desiredDepth, List<List<BuilderFlowNode>> validSequences)
		{
			bool flag = currentSequence.Count == desiredDepth - 1 || currentCheckNode.childBuilderNodes.Count > 0;
			if (currentSequence.Count == desiredDepth - 1 && currentCheckNode.loopConnectedBuilderNode != null)
			{
				return;
			}
			if (currentCheckNode.IsStandardCategory && currentCheckNode.assignedPrototypeRoom != null && currentCheckNode.assignedPrototypeRoom.ContainsEnemies && flag)
			{
				List<BuilderFlowNode> list = new List<BuilderFlowNode>(currentSequence);
				list.Add(currentCheckNode);
				if (list.Count == desiredDepth)
				{
					validSequences.Add(list);
					return;
				}
				for (int i = 0; i < currentCheckNode.childBuilderNodes.Count; i++)
				{
					this.RecurseCombatRooms(currentCheckNode.childBuilderNodes[i], list, desiredDepth, validSequences);
				}
			}
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x001D4934 File Offset: 0x001D2B34
		protected void HandleInjectionFrame(ProceduralFlowModifierData modData, BuilderFlowNode root, RuntimeInjectionMetadata optionalMetadata, FlowCompositeMetastructure metastructure)
		{
			int framedCombatNodes = modData.framedCombatNodes;
			optionalMetadata.forceSecret = false;
			BuilderFlowNode builderFlowNode = this.ConstructNodeForInjection(modData.exactRoom, modData, optionalMetadata);
			BuilderFlowNode builderFlowNode2 = this.ConstructNodeForInjection(modData.exactSecondaryRoom, modData, optionalMetadata);
			List<List<BuilderFlowNode>> list = new List<List<BuilderFlowNode>>();
			Stack<BuilderFlowNode> stack = new Stack<BuilderFlowNode>();
			stack.Push(root);
			List<BuilderFlowNode> list2 = new List<BuilderFlowNode>();
			while (stack.Count > 0)
			{
				BuilderFlowNode builderFlowNode3 = stack.Pop();
				this.RecurseCombatRooms(builderFlowNode3, list2, framedCombatNodes, list);
				for (int i = 0; i < builderFlowNode3.childBuilderNodes.Count; i++)
				{
					stack.Push(builderFlowNode3.childBuilderNodes[i]);
				}
			}
			if (list.Count <= 0)
			{
				return;
			}
			List<BuilderFlowNode> list3 = list[BraveRandom.GenerationRandomRange(0, list.Count)];
			List<BuilderFlowNode> list4 = new List<BuilderFlowNode>();
			list4.Add(builderFlowNode);
			list4.AddRange(list3);
			list4.Add(builderFlowNode2);
			BuilderFlowNode builderFlowNode4 = list3[0];
			BuilderFlowNode parentBuilderNode = builderFlowNode4.parentBuilderNode;
			parentBuilderNode.childBuilderNodes.Remove(builderFlowNode4);
			parentBuilderNode.childBuilderNodes.Add(builderFlowNode);
			builderFlowNode.parentBuilderNode = parentBuilderNode;
			builderFlowNode4.parentBuilderNode = builderFlowNode;
			builderFlowNode.childBuilderNodes.Add(builderFlowNode4);
			builderFlowNode.InjectionFrameSequence = list4;
			this.allBuilderNodes.Add(builderFlowNode);
			BuilderFlowNode builderFlowNode5 = list3[list3.Count - 1];
			builderFlowNode5.childBuilderNodes.Add(builderFlowNode2);
			builderFlowNode2.parentBuilderNode = builderFlowNode5;
			builderFlowNode2.childBuilderNodes = new List<BuilderFlowNode>();
			builderFlowNode2.InjectionFrameSequence = list4;
			this.allBuilderNodes.Add(builderFlowNode2);
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x001D4AD0 File Offset: 0x001D2CD0
		protected bool ProcessSingleNodeInjection(ProceduralFlowModifierData currentInjectionData, BuilderFlowNode root, RuntimeInjectionFlags injectionFlags, FlowCompositeMetastructure metastructure, RuntimeInjectionMetadata optionalMetadata = null)
		{
			bool flag = false;
			if (currentInjectionData.RequiredValidPlaceable != null && !currentInjectionData.RequiredValidPlaceable.HasValidPlaceable())
			{
				if (flag)
				{
					Debug.LogError("Failing Injection because " + currentInjectionData.RequiredValidPlaceable.name + " has no valid placeable.");
				}
				return false;
			}
			bool flag2 = false;
			if (!flag2 && !currentInjectionData.PrerequisitesMet)
			{
				if (flag)
				{
					Debug.Log("Failing Injection because " + currentInjectionData.annotation + " has unmet prerequisites.");
				}
				return false;
			}
			if (!flag2 && currentInjectionData.exactRoom != null && !currentInjectionData.exactRoom.CheckPrerequisites())
			{
				if (flag)
				{
					Debug.Log("Failing Injection because " + currentInjectionData.exactRoom.name + " has unmet prerequisites.");
				}
				return false;
			}
			PrototypeDungeonRoom prototypeDungeonRoom = currentInjectionData.exactRoom;
			if (currentInjectionData.roomTable != null && currentInjectionData.exactRoom == null)
			{
				WeightedRoom weightedRoom = currentInjectionData.roomTable.SelectByWeight();
				if (weightedRoom != null)
				{
					prototypeDungeonRoom = weightedRoom.room;
				}
			}
			if (prototypeDungeonRoom == null)
			{
				if (currentInjectionData.roomTable != null)
				{
					if (flag)
					{
						Debug.Log("Failing Injection because " + currentInjectionData.roomTable.name + " has no valid rooms in its table.");
					}
					return false;
				}
				if (flag)
				{
					Debug.Log("Failing Injection because " + currentInjectionData.annotation + " is a NULL room injection!");
				}
				return true;
			}
			else
			{
				if (optionalMetadata != null && optionalMetadata.SucceededRandomizationCheckMap.ContainsKey(currentInjectionData))
				{
					if (!optionalMetadata.SucceededRandomizationCheckMap[currentInjectionData])
					{
						if (flag)
						{
							Debug.Log("Failing Injection on " + currentInjectionData.annotation + " by CACHED RNG.");
						}
						return false;
					}
				}
				else
				{
					if (!flag2 && BraveRandom.GenerationRandomValue() > currentInjectionData.chanceToSpawn)
					{
						if (flag)
						{
							Debug.Log("Failing Injection on " + currentInjectionData.annotation + " by RNG.");
						}
						if (optionalMetadata != null)
						{
							optionalMetadata.SucceededRandomizationCheckMap.Add(currentInjectionData, false);
						}
						return false;
					}
					if (optionalMetadata != null)
					{
						optionalMetadata.SucceededRandomizationCheckMap.Add(currentInjectionData, true);
					}
				}
				if (!flag2 && !prototypeDungeonRoom.injectionFlags.IsValid(injectionFlags))
				{
					if (flag)
					{
						Debug.Log("Failing Injection because " + prototypeDungeonRoom.name + " has invalid injection flags state.");
					}
					return false;
				}
				bool flag3 = injectionFlags.Merge(prototypeDungeonRoom.injectionFlags);
				if (flag3)
				{
					Debug.Log("Assigning FIREPLACE from room: " + prototypeDungeonRoom.name);
				}
				ProceduralFlowModifierData.FlowModifierPlacementType flowModifierPlacementType = currentInjectionData.GetPlacementRule();
				if (optionalMetadata != null && optionalMetadata.forceSecret && !currentInjectionData.DEBUG_FORCE_SPAWN)
				{
					if (!currentInjectionData.CanBeForcedSecret)
					{
						if (flag)
						{
							Debug.Log("Failing Injection because " + currentInjectionData.annotation + " cannot be forced SECRET.");
						}
						return false;
					}
					flowModifierPlacementType = ProceduralFlowModifierData.FlowModifierPlacementType.RANDOM_NODE_CHILD;
				}
				if (flag && prototypeDungeonRoom != null)
				{
					Debug.Log("Succeeding injection of room : " + prototypeDungeonRoom.name);
				}
				bool flag4 = true;
				switch (flowModifierPlacementType)
				{
				case ProceduralFlowModifierData.FlowModifierPlacementType.BEFORE_ANY_COMBAT_ROOM:
					flag4 = this.InjectNodeBefore(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_RandomCombatRoom), metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN:
					flag4 = this.InjectNodeAfter(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_EndOfChain), metastructure, optionalMetadata);
					if (!flag4)
					{
						flag4 = this.InjectNodeAfter(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_RandomNodeChild), metastructure, optionalMetadata);
					}
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.HUB_ADJACENT_CHAIN_START:
					flag4 = this.InjectNodeBefore(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_HubAdjacentChainStart), metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.HUB_ADJACENT_NO_LINK:
					flag4 = this.InjectNodeAfter(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_HubAdjacentNoLink), metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.RANDOM_NODE_CHILD:
					flag4 = this.InjectNodeAfter(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_RandomNodeChild), metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.COMBAT_FRAME:
					this.HandleInjectionFrame(currentInjectionData, root, optionalMetadata, metastructure);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.NO_LINKS:
					this.InjectNodeNoLinks(currentInjectionData, prototypeDungeonRoom, root, metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.AFTER_BOSS:
					flag4 = this.InjectNodeBefore(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_AfterBoss), metastructure, optionalMetadata);
					break;
				case ProceduralFlowModifierData.FlowModifierPlacementType.BLACK_MARKET:
					flag4 = this.InjectNodeAfter(currentInjectionData, prototypeDungeonRoom, root, new Action<BuilderFlowNode, List<BuilderFlowNode>, ProceduralFlowModifierData, FlowCompositeMetastructure>(this.InjectValidator_BlackMarket), metastructure, optionalMetadata);
					break;
				}
				if (flag4 && prototypeDungeonRoom.requiredInjectionData != null)
				{
					RuntimeInjectionMetadata runtimeInjectionMetadata = new RuntimeInjectionMetadata(prototypeDungeonRoom.requiredInjectionData);
					this.HandleNodeInjection(root, runtimeInjectionMetadata, injectionFlags, metastructure);
				}
				return flag4;
			}
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001D4F80 File Offset: 0x001D3180
		protected void HandleNodeInjection(BuilderFlowNode root, RuntimeInjectionMetadata sourceMetadata, RuntimeInjectionFlags injectionFlags, FlowCompositeMetastructure metastructure)
		{
			SharedInjectionData injectionData = sourceMetadata.injectionData;
			if (injectionData != null && injectionData.InjectionData.Count > 0)
			{
				List<int> list = Enumerable.Range(0, injectionData.InjectionData.Count).ToList<int>();
				list = list.GenerationShuffle<int>();
				if (injectionData.OnlyOne)
				{
					ProceduralFlowModifierData proceduralFlowModifierData = null;
					float num = injectionData.ChanceToSpawnOne;
					bool flag = false;
					if (injectionData.IsNPCCell)
					{
						num += (float)GameStatsManager.Instance.NumberRunsValidCellWithoutSpawn / 50f;
						if (MetaInjectionData.CellGeneratedForCurrentBlueprint || BraveRandom.IgnoreGenerationDifferentiator)
						{
							num = 0f;
						}
						if (injectionData.InjectionData.Count > 1 && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
						{
							num = 0f;
						}
						if (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON && !GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_ACTIVE_IN_FOYER))
						{
							flag = true;
							num = 1f;
						}
					}
					if (BraveRandom.GenerationRandomValue() < num)
					{
						float num2 = 0f;
						for (int i = 0; i < injectionData.InjectionData.Count; i++)
						{
							ProceduralFlowModifierData proceduralFlowModifierData2 = injectionData.InjectionData[i];
							if (!proceduralFlowModifierData2.OncePerRun || !MetaInjectionData.InjectionSetsUsedThisRun.Contains(proceduralFlowModifierData2))
							{
								if (!injectionData.IsNPCCell || proceduralFlowModifierData2.PrerequisitesMet)
								{
									if (!injectionData.IgnoreUnmetPrerequisiteEntries || proceduralFlowModifierData2.PrerequisitesMet)
									{
										num2 += proceduralFlowModifierData2.selectionWeight;
									}
								}
							}
						}
						float num3 = BraveRandom.GenerationRandomValue() * num2;
						float num4 = 0f;
						ProceduralFlowModifierData proceduralFlowModifierData3;
						if (sourceMetadata != null && sourceMetadata.HasAssignedModDataExactRoom)
						{
							proceduralFlowModifierData = sourceMetadata.AssignedModifierData;
						}
						else if (this.ShouldDoLostAdventurerHelp(injectionData, out proceduralFlowModifierData3))
						{
							proceduralFlowModifierData = proceduralFlowModifierData3;
							if (flag)
							{
								proceduralFlowModifierData = injectionData.InjectionData[0];
							}
						}
						else
						{
							for (int j = 0; j < injectionData.InjectionData.Count; j++)
							{
								ProceduralFlowModifierData proceduralFlowModifierData4 = injectionData.InjectionData[j];
								if (!proceduralFlowModifierData4.OncePerRun || !MetaInjectionData.InjectionSetsUsedThisRun.Contains(proceduralFlowModifierData4))
								{
									if (!injectionData.IsNPCCell || proceduralFlowModifierData4.PrerequisitesMet)
									{
										if (!injectionData.IgnoreUnmetPrerequisiteEntries || proceduralFlowModifierData4.PrerequisitesMet)
										{
											num4 += proceduralFlowModifierData4.selectionWeight;
											if (num4 > num3)
											{
												proceduralFlowModifierData = proceduralFlowModifierData4;
												break;
											}
										}
									}
								}
							}
							if (flag)
							{
								proceduralFlowModifierData = injectionData.InjectionData[0];
							}
						}
						if (sourceMetadata != null && !sourceMetadata.HasAssignedModDataExactRoom)
						{
							sourceMetadata.HasAssignedModDataExactRoom = true;
							if (proceduralFlowModifierData != null)
							{
								Debug.Log("Assigning METADATA: " + proceduralFlowModifierData.annotation);
							}
							sourceMetadata.AssignedModifierData = proceduralFlowModifierData;
							if (proceduralFlowModifierData != null && proceduralFlowModifierData.OncePerRun)
							{
								MetaInjectionData.InjectionSetsUsedThisRun.Add(proceduralFlowModifierData);
							}
						}
						if (proceduralFlowModifierData == null || !this.ProcessSingleNodeInjection(proceduralFlowModifierData, root, injectionFlags, metastructure, sourceMetadata))
						{
						}
					}
				}
				else
				{
					for (int k = 0; k < injectionData.InjectionData.Count; k++)
					{
						ProceduralFlowModifierData proceduralFlowModifierData5 = injectionData.InjectionData[list[k]];
						bool flag2 = this.ProcessSingleNodeInjection(proceduralFlowModifierData5, root, injectionFlags, metastructure, sourceMetadata);
					}
				}
			}
			if (injectionData != null && injectionData.AttachedInjectionData.Count > 0)
			{
				for (int l = 0; l < injectionData.AttachedInjectionData.Count; l++)
				{
					RuntimeInjectionMetadata runtimeInjectionMetadata = new RuntimeInjectionMetadata(injectionData.AttachedInjectionData[l]);
					runtimeInjectionMetadata.CopyMetadata(sourceMetadata);
					this.HandleNodeInjection(root, runtimeInjectionMetadata, injectionFlags, metastructure);
				}
			}
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x001D5368 File Offset: 0x001D3568
		private bool ShouldDoLostAdventurerHelp(SharedInjectionData injectionData, out ProceduralFlowModifierData lostAdventurerSet)
		{
			lostAdventurerSet = null;
			for (int i = 0; i < injectionData.InjectionData.Count; i++)
			{
				ProceduralFlowModifierData proceduralFlowModifierData = injectionData.InjectionData[i];
				if (!proceduralFlowModifierData.OncePerRun || !MetaInjectionData.InjectionSetsUsedThisRun.Contains(proceduralFlowModifierData))
				{
					if (!injectionData.IsNPCCell || proceduralFlowModifierData.PrerequisitesMet)
					{
						if (!injectionData.IgnoreUnmetPrerequisiteEntries || proceduralFlowModifierData.PrerequisitesMet)
						{
							if (!(proceduralFlowModifierData.annotation != "lost adventurer"))
							{
								GungeonFlags? gungeonFlags = this.LostAdventurerGetFlagFromFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
								if (gungeonFlags == null)
								{
									return false;
								}
								if (this.LostAdventurerGetFloorsHelped() == 4 && !GameStatsManager.Instance.GetFlag(gungeonFlags.Value))
								{
									lostAdventurerSet = proceduralFlowModifierData;
									return true;
								}
								return false;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x001D5468 File Offset: 0x001D3668
		private GungeonFlags? LostAdventurerGetFlagFromFloor(GlobalDungeonData.ValidTilesets floor)
		{
			if (floor == GlobalDungeonData.ValidTilesets.GUNGEON)
			{
				return new GungeonFlags?(GungeonFlags.LOST_ADVENTURER_HELPED_GUNGEON);
			}
			if (floor == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				return new GungeonFlags?(GungeonFlags.LOST_ADVENTURER_HELPED_CASTLE);
			}
			if (floor == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				return new GungeonFlags?(GungeonFlags.LOST_ADVENTURER_HELPED_MINES);
			}
			if (floor == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
			{
				return new GungeonFlags?(GungeonFlags.LOST_ADVENTURER_HELPED_CATACOMBS);
			}
			if (floor != GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				return null;
			}
			return new GungeonFlags?(GungeonFlags.LOST_ADVENTURER_HELPED_FORGE);
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001D54E0 File Offset: 0x001D36E0
		private int LostAdventurerGetFloorsHelped()
		{
			List<GungeonFlags> list = new List<GungeonFlags>(new GungeonFlags[]
			{
				GungeonFlags.LOST_ADVENTURER_HELPED_CASTLE,
				GungeonFlags.LOST_ADVENTURER_HELPED_GUNGEON,
				GungeonFlags.LOST_ADVENTURER_HELPED_MINES,
				GungeonFlags.LOST_ADVENTURER_HELPED_CATACOMBS,
				GungeonFlags.LOST_ADVENTURER_HELPED_FORGE
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (GameStatsManager.Instance.GetFlag(list[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001D5538 File Offset: 0x001D3738
		protected void HandleNodeInjection(BuilderFlowNode root, List<ProceduralFlowModifierData> flowInjectionData, RuntimeInjectionFlags injectionFlags, FlowCompositeMetastructure metastructure)
		{
			if (flowInjectionData != null && flowInjectionData.Count > 0)
			{
				for (int i = 0; i < flowInjectionData.Count; i++)
				{
					ProceduralFlowModifierData proceduralFlowModifierData = flowInjectionData[i];
					this.ProcessSingleNodeInjection(proceduralFlowModifierData, root, injectionFlags, metastructure, null);
				}
			}
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001D5584 File Offset: 0x001D3784
		protected BuilderFlowNode ComposeFlowTree()
		{
			Stack<BuilderFlowNode> stack = new Stack<BuilderFlowNode>();
			stack.Push(new BuilderFlowNode(this.m_flow.FirstNode));
			BuilderFlowNode builderFlowNode = stack.Peek();
			int num = 0;
			builderFlowNode.identifier = num;
			num++;
			while (stack.Count > 0)
			{
				BuilderFlowNode builderFlowNode2 = stack.Pop();
				if (builderFlowNode2.childBuilderNodes == null)
				{
					builderFlowNode2.childBuilderNodes = this.m_flow.NewGetNodeChildrenToBuild(builderFlowNode2, this);
				}
				this.allBuilderNodes.Add(builderFlowNode2);
				for (int i = 0; i < builderFlowNode2.childBuilderNodes.Count; i++)
				{
					if (!stack.Contains(builderFlowNode2.childBuilderNodes[i]))
					{
						if (builderFlowNode2.childBuilderNodes[i].identifier < 0)
						{
							builderFlowNode2.childBuilderNodes[i].identifier = num;
							num++;
						}
						else
						{
							Debug.Log("assigning already-assigned identifier");
						}
						stack.Push(builderFlowNode2.childBuilderNodes[i]);
					}
				}
			}
			for (int j = 0; j < this.allBuilderNodes.Count; j++)
			{
				if (!string.IsNullOrEmpty(this.allBuilderNodes[j].node.loopTargetNodeGuid))
				{
					DungeonFlowNode nodeFromGuid = this.m_flow.GetNodeFromGuid(this.allBuilderNodes[j].node.loopTargetNodeGuid);
					for (int k = 0; k < this.allBuilderNodes.Count; k++)
					{
						if (this.allBuilderNodes[k].node == nodeFromGuid)
						{
							this.allBuilderNodes[j].loopConnectedBuilderNode = this.allBuilderNodes[k];
							this.allBuilderNodes[k].loopConnectedBuilderNode = this.allBuilderNodes[j];
						}
					}
				}
			}
			return builderFlowNode;
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001D5774 File Offset: 0x001D3974
		protected BuilderFlowNode RerootTreeAtHighestConnectivity(BuilderFlowNode root)
		{
			int num = root.Connectivity;
			BuilderFlowNode builderFlowNode = root;
			Queue<BuilderFlowNode> queue = new Queue<BuilderFlowNode>();
			queue.Enqueue(root);
			while (queue.Count > 0)
			{
				BuilderFlowNode builderFlowNode2 = queue.Dequeue();
				if (builderFlowNode2.Connectivity > num)
				{
					num = builderFlowNode2.Connectivity;
					builderFlowNode = builderFlowNode2;
				}
				for (int i = 0; i < builderFlowNode2.childBuilderNodes.Count; i++)
				{
					queue.Enqueue(builderFlowNode2.childBuilderNodes[i]);
				}
			}
			builderFlowNode.MakeNodeTreeRoot();
			return builderFlowNode;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001D5800 File Offset: 0x001D3A00
		protected void PerformOperationOnTreeNodes(BuilderFlowNode root, Action<BuilderFlowNode> action)
		{
			Queue<BuilderFlowNode> queue = new Queue<BuilderFlowNode>();
			queue.Enqueue(root);
			while (queue.Count > 0)
			{
				BuilderFlowNode builderFlowNode = queue.Dequeue();
				action(builderFlowNode);
				for (int i = 0; i < builderFlowNode.childBuilderNodes.Count; i++)
				{
					queue.Enqueue(builderFlowNode.childBuilderNodes[i]);
				}
			}
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x001D5868 File Offset: 0x001D3A68
		protected DungeonFlowSubtypeRestriction GetSubtypeRestrictionFromRoom(PrototypeDungeonRoom room)
		{
			foreach (DungeonFlowSubtypeRestriction dungeonFlowSubtypeRestriction in this.roomsOfSubtypeRemaining.Keys)
			{
				if (dungeonFlowSubtypeRestriction.baseCategoryRestriction == room.category && ((room.category == PrototypeDungeonRoom.RoomCategory.BOSS && room.subCategoryBoss == dungeonFlowSubtypeRestriction.bossSubcategoryRestriction) || (room.category == PrototypeDungeonRoom.RoomCategory.NORMAL && room.subCategoryNormal == dungeonFlowSubtypeRestriction.normalSubcategoryRestriction) || (room.category == PrototypeDungeonRoom.RoomCategory.SPECIAL && room.subCategorySpecial == dungeonFlowSubtypeRestriction.specialSubcategoryRestriction) || (room.category == PrototypeDungeonRoom.RoomCategory.SECRET && room.subCategorySecret == dungeonFlowSubtypeRestriction.secretSubcategoryRestriction)))
				{
					return dungeonFlowSubtypeRestriction;
				}
			}
			return null;
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x001D5954 File Offset: 0x001D3B54
		protected bool CheckRoomAgainstRestrictedSubtypes(PrototypeDungeonRoom room)
		{
			DungeonFlowSubtypeRestriction subtypeRestrictionFromRoom = this.GetSubtypeRestrictionFromRoom(room);
			return subtypeRestrictionFromRoom != null && this.roomsOfSubtypeRemaining[subtypeRestrictionFromRoom] <= 0;
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x001D5984 File Offset: 0x001D3B84
		protected List<WeightedRoom> GetViableAvailableRooms(PrototypeDungeonRoom.RoomCategory category, int requiredExits, List<WeightedRoom> source, out float totalAvailableWeight, LoopFlowBuilder.FallbackLevel fallback = LoopFlowBuilder.FallbackLevel.NOT_FALLBACK)
		{
			List<WeightedRoom> list = new List<WeightedRoom>();
			List<int> list2 = Enumerable.Range(0, source.Count).ToList<int>().GenerationShuffle<int>();
			totalAvailableWeight = 0f;
			for (int i = 0; i < source.Count; i++)
			{
				int num = list2[i];
				WeightedRoom weightedRoom = source[num];
				PrototypeDungeonRoom room = weightedRoom.room;
				float num2 = weightedRoom.weight;
				if (!(weightedRoom.room == null))
				{
					if (!this.CheckRoomAgainstRestrictedSubtypes(room))
					{
						if (room.exitData.exits.Count >= requiredExits)
						{
							if (requiredExits != 1 || room.category != PrototypeDungeonRoom.RoomCategory.NORMAL || room.subCategoryNormal != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP)
							{
								if (!Enum.IsDefined(typeof(PrototypeDungeonRoom.RoomCategory), category) || room.category == category)
								{
									if (fallback != LoopFlowBuilder.FallbackLevel.NOT_FALLBACK || weightedRoom.room.ForceAllowDuplicates || !this.m_usedPrototypeRoomData.ContainsKey(weightedRoom.room))
									{
										int num3 = GameStatsManager.Instance.QueryRoomDifferentiator(weightedRoom.room);
										if (fallback == LoopFlowBuilder.FallbackLevel.NOT_FALLBACK && !weightedRoom.room.ForceAllowDuplicates && weightedRoom.room.category != PrototypeDungeonRoom.RoomCategory.SPECIAL && num3 > 0)
										{
											num2 *= Mathf.Clamp01(1f - 0.33f * (float)num3);
										}
										if (!this.m_excludedRoomData.Contains(weightedRoom.room))
										{
											if (weightedRoom.CheckPrerequisites())
											{
												if (room.CheckPrerequisites())
												{
													if (room.injectionFlags.IsValid(this.m_runtimeInjectionFlags))
													{
														if (fallback == LoopFlowBuilder.FallbackLevel.FALLBACK_EMERGENCY || category == PrototypeDungeonRoom.RoomCategory.NORMAL || !weightedRoom.limitedCopies || !this.m_usedPrototypeRoomData.ContainsKey(weightedRoom.room) || this.m_usedPrototypeRoomData[weightedRoom.room] < weightedRoom.maxCopies)
														{
															list.Add(weightedRoom);
															totalAvailableWeight += num2;
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
			return list;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x001D5BDC File Offset: 0x001D3DDC
		public PrototypeDungeonRoom GetAvailableRoomByExitDirection(PrototypeDungeonRoom.RoomCategory category, int requiredExits, List<DungeonData.Direction> exitDirections, List<WeightedRoom> source, LoopFlowBuilder.FallbackLevel fallback = LoopFlowBuilder.FallbackLevel.NOT_FALLBACK)
		{
			float num = 0f;
			List<WeightedRoom> viableAvailableRooms = this.GetViableAvailableRooms(category, requiredExits, source, out num, fallback);
			for (int i = 0; i < viableAvailableRooms.Count; i++)
			{
				WeightedRoom weightedRoom = viableAvailableRooms[i];
				bool flag = false;
				for (int j = 0; j < weightedRoom.room.exitData.exits.Count; j++)
				{
					PrototypeRoomExit prototypeRoomExit = weightedRoom.room.exitData.exits[j];
					if (exitDirections.Contains(prototypeRoomExit.exitDirection))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num -= weightedRoom.weight;
					viableAvailableRooms.RemoveAt(i);
					i--;
				}
			}
			if (viableAvailableRooms.Count == 0 && fallback == LoopFlowBuilder.FallbackLevel.NOT_FALLBACK)
			{
				return this.GetAvailableRoomByExitDirection(category, requiredExits, exitDirections, source, LoopFlowBuilder.FallbackLevel.FALLBACK_STANDARD);
			}
			if (viableAvailableRooms.Count == 0 && fallback == LoopFlowBuilder.FallbackLevel.FALLBACK_EMERGENCY)
			{
				return this.GetAvailableRoomByExitDirection(category, requiredExits, exitDirections, source, LoopFlowBuilder.FallbackLevel.FALLBACK_EMERGENCY);
			}
			if (viableAvailableRooms.Count == 0)
			{
				if (category == PrototypeDungeonRoom.RoomCategory.CONNECTOR)
				{
					return this.GetAvailableRoomByExitDirection(PrototypeDungeonRoom.RoomCategory.NORMAL, requiredExits, exitDirections, source, LoopFlowBuilder.FallbackLevel.NOT_FALLBACK);
				}
				Debug.LogError("Falling back due to lack of non-duplicate rooms FAILED. This should never happen.");
			}
			float num2 = BraveRandom.GenerationRandomValue() * num;
			for (int k = 0; k < viableAvailableRooms.Count; k++)
			{
				num2 -= viableAvailableRooms[k].weight;
				if (num2 <= 0f)
				{
					return viableAvailableRooms[k].room;
				}
			}
			if (viableAvailableRooms == null || viableAvailableRooms.Count == 0)
			{
				return null;
			}
			return viableAvailableRooms[0].room;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x001D5D70 File Offset: 0x001D3F70
		public PrototypeDungeonRoom GetAvailableRoom(PrototypeDungeonRoom.RoomCategory category, int requiredExits, List<WeightedRoom> source, LoopFlowBuilder.FallbackLevel fallback = LoopFlowBuilder.FallbackLevel.NOT_FALLBACK)
		{
			float num = 0f;
			List<WeightedRoom> viableAvailableRooms = this.GetViableAvailableRooms(category, requiredExits, source, out num, fallback);
			if (viableAvailableRooms.Count == 0 && fallback == LoopFlowBuilder.FallbackLevel.NOT_FALLBACK)
			{
				return this.GetAvailableRoom(category, requiredExits, source, LoopFlowBuilder.FallbackLevel.FALLBACK_STANDARD);
			}
			if (viableAvailableRooms.Count == 0 && fallback == LoopFlowBuilder.FallbackLevel.FALLBACK_STANDARD)
			{
				return this.GetAvailableRoom(category, requiredExits, source, LoopFlowBuilder.FallbackLevel.FALLBACK_EMERGENCY);
			}
			if (viableAvailableRooms.Count == 0)
			{
				if (category != PrototypeDungeonRoom.RoomCategory.SECRET)
				{
					if (category == PrototypeDungeonRoom.RoomCategory.CONNECTOR || category == PrototypeDungeonRoom.RoomCategory.HUB)
					{
						Debug.LogError("Replacing failed CONNECTOR/HUB room with room of type NORMAL.");
						return this.GetAvailableRoom(PrototypeDungeonRoom.RoomCategory.NORMAL, requiredExits, source, LoopFlowBuilder.FallbackLevel.NOT_FALLBACK);
					}
					Debug.LogError(string.Concat(new string[]
					{
						"Falling back due to lack of non-duplicate rooms (",
						requiredExits.ToString(),
						",",
						source.Count.ToString(),
						") in list of length: ",
						source.Count.ToString(),
						". FAILED: ",
						category.ToString(),
						". This should never happen."
					}));
				}
				return null;
			}
			float num2 = BraveRandom.GenerationRandomValue() * num;
			for (int i = 0; i < viableAvailableRooms.Count; i++)
			{
				num2 -= viableAvailableRooms[i].weight;
				if (num2 <= 0f)
				{
					return viableAvailableRooms[i].room;
				}
			}
			return viableAvailableRooms[0].room;
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x001D5EE8 File Offset: 0x001D40E8
		public void ClearPlacedRoomData(BuilderFlowNode buildData)
		{
			if (buildData.assignedPrototypeRoom != null)
			{
				DungeonFlowSubtypeRestriction subtypeRestrictionFromRoom = this.GetSubtypeRestrictionFromRoom(buildData.assignedPrototypeRoom);
				if (subtypeRestrictionFromRoom != null)
				{
					this.roomsOfSubtypeRemaining[subtypeRestrictionFromRoom] = this.roomsOfSubtypeRemaining[subtypeRestrictionFromRoom] + 1;
				}
				if (this.m_usedPrototypeRoomData.ContainsKey(buildData.assignedPrototypeRoom))
				{
					if (this.m_usedPrototypeRoomData[buildData.assignedPrototypeRoom] > 1)
					{
						this.m_usedPrototypeRoomData[buildData.assignedPrototypeRoom] = this.m_usedPrototypeRoomData[buildData.assignedPrototypeRoom] - 1;
					}
					else
					{
						this.m_usedPrototypeRoomData.Remove(buildData.assignedPrototypeRoom);
					}
				}
				for (int i = 0; i < buildData.assignedPrototypeRoom.excludedOtherRooms.Count; i++)
				{
					this.m_excludedRoomData.Remove(buildData.assignedPrototypeRoom.excludedOtherRooms[i]);
				}
				if (buildData.assignedPrototypeRoom.injectionFlags.CastleFireplace)
				{
					this.m_runtimeInjectionFlags.CastleFireplace = false;
				}
				buildData.assignedPrototypeRoom = null;
			}
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x001D6004 File Offset: 0x001D4204
		private bool PostprocessInjectionDataContains(SharedInjectionData test)
		{
			for (int i = 0; i < this.m_postprocessInjectionData.Count; i++)
			{
				if (this.m_postprocessInjectionData[i].injectionData == test)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x001D604C File Offset: 0x001D424C
		public void NotifyPlacedRoomData(PrototypeDungeonRoom assignedRoom)
		{
			PrototypeDungeonRoom prototypeDungeonRoom = ((!(assignedRoom.MirrorSource != null)) ? assignedRoom : assignedRoom.MirrorSource);
			DungeonFlowSubtypeRestriction subtypeRestrictionFromRoom = this.GetSubtypeRestrictionFromRoom(prototypeDungeonRoom);
			if (subtypeRestrictionFromRoom != null)
			{
				this.roomsOfSubtypeRemaining[subtypeRestrictionFromRoom] = this.roomsOfSubtypeRemaining[subtypeRestrictionFromRoom] - 1;
			}
			if (this.m_usedPrototypeRoomData.ContainsKey(prototypeDungeonRoom))
			{
				this.m_usedPrototypeRoomData[prototypeDungeonRoom] = this.m_usedPrototypeRoomData[prototypeDungeonRoom] + 1;
			}
			else
			{
				this.m_usedPrototypeRoomData.Add(prototypeDungeonRoom, 1);
			}
			for (int i = 0; i < prototypeDungeonRoom.excludedOtherRooms.Count; i++)
			{
				this.m_excludedRoomData.Add(prototypeDungeonRoom.excludedOtherRooms[i]);
			}
			if (prototypeDungeonRoom.requiredInjectionData != null && !this.PostprocessInjectionDataContains(prototypeDungeonRoom.requiredInjectionData))
			{
				this.m_postprocessInjectionData.Add(new RuntimeInjectionMetadata(prototypeDungeonRoom.requiredInjectionData));
			}
			bool flag = this.m_runtimeInjectionFlags.Merge(prototypeDungeonRoom.injectionFlags);
			if (flag)
			{
				Debug.Log("Assigning FIREPLACE from room " + prototypeDungeonRoom.name);
			}
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001D6174 File Offset: 0x001D4374
		protected void HandleBossFoyerAcquisition(BuilderFlowNode buildData)
		{
			this.HandleBossFoyerAcquisition(buildData, false);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x001D6180 File Offset: 0x001D4380
		protected void HandleBossFoyerAcquisition(BuilderFlowNode buildData, bool isFallback)
		{
			BuilderFlowNode builderFlowNode = null;
			for (int i = 0; i < buildData.childBuilderNodes.Count; i++)
			{
				if (buildData.childBuilderNodes[i].Category == PrototypeDungeonRoom.RoomCategory.BOSS)
				{
					builderFlowNode = buildData.childBuilderNodes[i];
				}
			}
			if (builderFlowNode == null)
			{
				return;
			}
			this.ClearPlacedRoomData(buildData);
			if (buildData.node.overrideExactRoom != null)
			{
				buildData.assignedPrototypeRoom = buildData.node.overrideExactRoom;
			}
			else
			{
				GenericRoomTable genericRoomTable = this.m_flow.fallbackRoomTable;
				if (buildData.node.overrideRoomTable != null)
				{
					genericRoomTable = buildData.node.overrideRoomTable;
				}
				List<WeightedRoom> list = new List<WeightedRoom>(genericRoomTable.GetCompiledList());
				for (int j = 0; j < list.Count; j++)
				{
					PrototypeDungeonRoom room = list[j].room;
					if (!isFallback && !room.CheckPrerequisites())
					{
						list.RemoveAt(j);
						j--;
					}
					else
					{
						bool flag = false;
						if (room != null)
						{
							for (int k = 0; k < builderFlowNode.assignedPrototypeRoom.exitData.exits.Count; k++)
							{
								if (builderFlowNode.assignedPrototypeRoom.exitData.exits[k].exitType != PrototypeRoomExit.ExitType.EXIT_ONLY)
								{
									List<PrototypeRoomExit> exitsMatchingDirection = room.GetExitsMatchingDirection((builderFlowNode.assignedPrototypeRoom.exitData.exits[k].exitDirection + 4) % (DungeonData.Direction)8, PrototypeRoomExit.ExitType.EXIT_ONLY);
									if (exitsMatchingDirection.Count > 0)
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (!flag)
						{
							list.RemoveAt(j);
							j--;
						}
					}
				}
				PrototypeDungeonRoom prototypeDungeonRoom = null;
				float num = 0f;
				for (int l = 0; l < list.Count; l++)
				{
					num += list[l].weight;
				}
				float num2 = BraveRandom.GenerationRandomValue() * num;
				for (int m = 0; m < list.Count; m++)
				{
					num2 -= list[m].weight;
					if (num2 <= 0f)
					{
						prototypeDungeonRoom = list[m].room;
						break;
					}
				}
				if (list.Count > 0 && prototypeDungeonRoom == null)
				{
					prototypeDungeonRoom = list[list.Count - 1].room;
				}
				if (prototypeDungeonRoom != null)
				{
					buildData.assignedPrototypeRoom = prototypeDungeonRoom;
				}
				else
				{
					if (!isFallback)
					{
						this.HandleBossFoyerAcquisition(buildData, true);
						return;
					}
					Debug.LogError("Failed to acquire a boss foyer! Something has gone wrong, or there is somehow not a boss foyer that matches the entrance direction for this boss chamber.");
				}
			}
			if (buildData.assignedPrototypeRoom != null)
			{
				this.NotifyPlacedRoomData(buildData.assignedPrototypeRoom);
			}
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x001D6458 File Offset: 0x001D4658
		protected void AcquirePrototypeRoom(BuilderFlowNode buildData)
		{
			if (this.roomsOfSubtypeRemaining == null)
			{
				this.roomsOfSubtypeRemaining = new Dictionary<DungeonFlowSubtypeRestriction, int>();
				for (int i = 0; i < this.m_flow.subtypeRestrictions.Count; i++)
				{
					this.roomsOfSubtypeRemaining.Add(this.m_flow.subtypeRestrictions[i], this.m_flow.subtypeRestrictions[i].maximumRoomsOfSubtype);
				}
			}
			this.ClearPlacedRoomData(buildData);
			if (buildData.node.UsesGlobalBossData)
			{
				buildData.assignedPrototypeRoom = GameManager.Instance.BossManager.SelectBossRoom();
			}
			else if (buildData.node.overrideExactRoom != null)
			{
				buildData.assignedPrototypeRoom = buildData.node.overrideExactRoom;
			}
			else
			{
				PrototypeDungeonRoom.RoomCategory roomCategory = ((!buildData.usesOverrideCategory) ? buildData.node.roomCategory : buildData.overrideCategory);
				if (roomCategory == PrototypeDungeonRoom.RoomCategory.CONNECTOR)
				{
					buildData.AcquiresRoomAsNecessary = true;
				}
				else
				{
					GenericRoomTable genericRoomTable = this.m_flow.fallbackRoomTable;
					if (buildData.node.overrideRoomTable != null)
					{
						genericRoomTable = buildData.node.overrideRoomTable;
					}
					List<WeightedRoom> compiledList = genericRoomTable.GetCompiledList();
					PrototypeDungeonRoom availableRoom = this.GetAvailableRoom(roomCategory, buildData.Connectivity, compiledList, LoopFlowBuilder.FallbackLevel.NOT_FALLBACK);
					if (availableRoom != null)
					{
						buildData.assignedPrototypeRoom = availableRoom;
					}
					else if (roomCategory != PrototypeDungeonRoom.RoomCategory.SECRET)
					{
						Debug.LogError("Failed to acquire a prototype room. This means the list is too sparse for the relevant category (" + roomCategory.ToString() + ") or something has gone terribly wrong.");
					}
				}
			}
			if (buildData.assignedPrototypeRoom != null)
			{
				this.NotifyPlacedRoomData(buildData.assignedPrototypeRoom);
			}
			else if (buildData.AcquiresRoomAsNecessary || buildData.node.priority == DungeonFlowNode.NodePriority.OPTIONAL)
			{
			}
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x001D662C File Offset: 0x001D482C
		protected void AssignInjectionDataToRoomHandler(BuilderFlowNode buildData)
		{
			if (buildData.instanceRoom != null)
			{
				if (buildData.InjectionTarget != null)
				{
					buildData.instanceRoom.injectionTarget = buildData.InjectionTarget.instanceRoom;
				}
				if (buildData.InjectionFrameSequence != null)
				{
					List<RoomHandler> list = new List<RoomHandler>();
					for (int i = 0; i < buildData.InjectionFrameSequence.Count; i++)
					{
						list.Add(buildData.InjectionFrameSequence[i].instanceRoom);
					}
					buildData.instanceRoom.injectionFrameData = list;
				}
			}
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001D66B8 File Offset: 0x001D48B8
		protected void DebugPrintTree(BuilderFlowNode root)
		{
			Stack<BuilderFlowNode> stack = new Stack<BuilderFlowNode>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				BuilderFlowNode builderFlowNode = stack.Pop();
				if (builderFlowNode.node != null)
				{
					Debug.Log(builderFlowNode.identifier + "|" + builderFlowNode.node.roomCategory.ToString());
				}
				for (int i = 0; i < builderFlowNode.childBuilderNodes.Count; i++)
				{
					stack.Push(builderFlowNode.childBuilderNodes[i]);
				}
			}
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x001D675C File Offset: 0x001D495C
		public List<BuilderFlowNode> FindPathBetweenNodesAdvanced(BuilderFlowNode origin, BuilderFlowNode target, List<Tuple<BuilderFlowNode, BuilderFlowNode>> excludedConnections)
		{
			Dictionary<BuilderFlowNode, int> dictionary = new Dictionary<BuilderFlowNode, int>();
			Dictionary<BuilderFlowNode, BuilderFlowNode> dictionary2 = new Dictionary<BuilderFlowNode, BuilderFlowNode>();
			for (int i = 0; i < this.allBuilderNodes.Count; i++)
			{
				int num = int.MaxValue;
				if (this.allBuilderNodes[i] == origin)
				{
					num = 0;
				}
				dictionary.Add(this.allBuilderNodes[i], num);
			}
			BuilderFlowNode builderFlowNode = origin;
			int num2 = 1;
			List<BuilderFlowNode> allConnectedNodes;
			int k;
			for (;;)
			{
				List<BuilderFlowNode> list = LoopFlowBuilder.BuilderFlowNodeListPool.Allocate();
				for (int j = 0; j < excludedConnections.Count; j++)
				{
					Tuple<BuilderFlowNode, BuilderFlowNode> tuple = excludedConnections[j];
					if (tuple.First == builderFlowNode)
					{
						list.Add(tuple.Second);
					}
				}
				allConnectedNodes = builderFlowNode.GetAllConnectedNodes(list);
				list.Clear();
				LoopFlowBuilder.BuilderFlowNodeListPool.Free(ref list);
				for (k = 0; k < allConnectedNodes.Count; k++)
				{
					if (allConnectedNodes[k] == target)
					{
						goto Block_5;
					}
					if (dictionary.ContainsKey(allConnectedNodes[k]) && dictionary[allConnectedNodes[k]] > num2)
					{
						dictionary[allConnectedNodes[k]] = num2;
						if (dictionary2.ContainsKey(allConnectedNodes[k]))
						{
							dictionary2[allConnectedNodes[k]] = builderFlowNode;
						}
						else
						{
							dictionary2.Add(allConnectedNodes[k], builderFlowNode);
						}
					}
				}
				dictionary.Remove(builderFlowNode);
				if (dictionary.Count == 0)
				{
					goto Block_10;
				}
				builderFlowNode = null;
				num2 = int.MaxValue;
				foreach (BuilderFlowNode builderFlowNode2 in dictionary.Keys)
				{
					if (dictionary[builderFlowNode2] < num2)
					{
						builderFlowNode = builderFlowNode2;
						num2 = dictionary[builderFlowNode2];
					}
				}
				if (builderFlowNode == null)
				{
					goto Block_12;
				}
			}
			Block_5:
			dictionary2.Add(allConnectedNodes[k], builderFlowNode);
			goto IL_205;
			Block_10:
			return null;
			Block_12:
			IL_205:
			if (!dictionary2.ContainsKey(target))
			{
				return null;
			}
			List<BuilderFlowNode> list2 = new List<BuilderFlowNode>();
			BuilderFlowNode builderFlowNode3 = target;
			while (builderFlowNode3 != null)
			{
				list2.Insert(0, builderFlowNode3);
				if (dictionary2.ContainsKey(builderFlowNode3))
				{
					builderFlowNode3 = dictionary2[builderFlowNode3];
				}
				else
				{
					builderFlowNode3 = null;
				}
			}
			return list2;
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x001D69D0 File Offset: 0x001D4BD0
		public List<BuilderFlowNode> FindPathBetweenNodes(BuilderFlowNode origin, BuilderFlowNode target, bool excludeDirect = false, params BuilderFlowNode[] excluded)
		{
			Dictionary<BuilderFlowNode, int> dictionary = new Dictionary<BuilderFlowNode, int>();
			Dictionary<BuilderFlowNode, BuilderFlowNode> dictionary2 = new Dictionary<BuilderFlowNode, BuilderFlowNode>();
			for (int i = 0; i < this.allBuilderNodes.Count; i++)
			{
				int num = int.MaxValue;
				if (this.allBuilderNodes[i] == origin)
				{
					num = 0;
				}
				dictionary.Add(this.allBuilderNodes[i], num);
			}
			BuilderFlowNode builderFlowNode = origin;
			int num2 = 1;
			BuilderFlowNode[] array;
			if (excluded == null)
			{
				(array = new BuilderFlowNode[1])[0] = target;
			}
			else
			{
				array = new BuilderFlowNode[excluded.Length + 1];
			}
			BuilderFlowNode[] array2 = array;
			if (excluded != null)
			{
				array2[array2.Length - 1] = target;
				for (int j = 0; j < excluded.Length; j++)
				{
					array2[j] = excluded[j];
				}
			}
			List<BuilderFlowNode> list;
			int k;
			for (;;)
			{
				list = ((!excludeDirect || builderFlowNode != origin) ? builderFlowNode.GetAllConnectedNodes(excluded) : builderFlowNode.GetAllConnectedNodes(array2));
				for (k = 0; k < list.Count; k++)
				{
					if (list[k] == target)
					{
						goto Block_7;
					}
					if (dictionary.ContainsKey(list[k]) && dictionary[list[k]] > num2)
					{
						dictionary[list[k]] = num2;
						if (dictionary2.ContainsKey(list[k]))
						{
							dictionary2[list[k]] = builderFlowNode;
						}
						else
						{
							dictionary2.Add(list[k], builderFlowNode);
						}
					}
				}
				dictionary.Remove(builderFlowNode);
				if (dictionary.Count == 0)
				{
					goto Block_12;
				}
				builderFlowNode = null;
				num2 = int.MaxValue;
				foreach (BuilderFlowNode builderFlowNode2 in dictionary.Keys)
				{
					if (dictionary[builderFlowNode2] < num2)
					{
						builderFlowNode = builderFlowNode2;
						num2 = dictionary[builderFlowNode2];
					}
				}
				if (builderFlowNode == null)
				{
					goto Block_14;
				}
			}
			Block_7:
			dictionary2.Add(list[k], builderFlowNode);
			goto IL_218;
			Block_12:
			return null;
			Block_14:
			IL_218:
			if (!dictionary2.ContainsKey(target))
			{
				return null;
			}
			List<BuilderFlowNode> list2 = new List<BuilderFlowNode>();
			BuilderFlowNode builderFlowNode3 = target;
			while (builderFlowNode3 != null)
			{
				list2.Insert(0, builderFlowNode3);
				if (dictionary2.ContainsKey(builderFlowNode3))
				{
					builderFlowNode3 = dictionary2[builderFlowNode3];
				}
				else
				{
					builderFlowNode3 = null;
				}
			}
			return list2;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x001D6C54 File Offset: 0x001D4E54
		public List<BuilderFlowNode> GetSubloopsFromLoop(LoopBuilderComposite loopComposite)
		{
			List<Tuple<BuilderFlowNode, BuilderFlowNode>> list = new List<Tuple<BuilderFlowNode, BuilderFlowNode>>();
			for (int i = 0; i < loopComposite.Nodes.Count; i++)
			{
				BuilderFlowNode builderFlowNode = loopComposite.Nodes[i];
				List<BuilderFlowNode> allConnectedNodes = builderFlowNode.GetAllConnectedNodes(new BuilderFlowNode[0]);
				for (int j = 0; j < allConnectedNodes.Count; j++)
				{
					BuilderFlowNode builderFlowNode2 = allConnectedNodes[j];
					if (loopComposite.Nodes.Contains(builderFlowNode2))
					{
						list.Add(new Tuple<BuilderFlowNode, BuilderFlowNode>(builderFlowNode, builderFlowNode2));
					}
				}
			}
			for (int k = 0; k < loopComposite.Nodes.Count; k++)
			{
				BuilderFlowNode builderFlowNode3 = loopComposite.Nodes[k];
				for (int l = k + 1; l < loopComposite.Nodes.Count; l++)
				{
					BuilderFlowNode builderFlowNode4 = loopComposite.Nodes[l];
					List<BuilderFlowNode> list2 = this.FindPathBetweenNodesAdvanced(builderFlowNode3, builderFlowNode4, list);
					if (list2 != null)
					{
						return list2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x001D6D5C File Offset: 0x001D4F5C
		public List<BuilderFlowNode> FindSimplestContainingLoop(BuilderFlowNode origin, List<BuilderFlowNode> usedNodes)
		{
			List<BuilderFlowNode> allConnectedNodes = origin.GetAllConnectedNodes(new BuilderFlowNode[0]);
			List<BuilderFlowNode> list = null;
			int num = int.MaxValue;
			for (int i = 0; i < allConnectedNodes.Count; i++)
			{
				List<BuilderFlowNode> list2 = this.FindPathBetweenNodes(origin, allConnectedNodes[i], true, usedNodes.ToArray());
				if (list2 != null && list2.Count < num)
				{
					num = list2.Count;
					list = list2;
				}
			}
			return list;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x001D6DCC File Offset: 0x001D4FCC
		public void ConvertTreeToCompositeStructure(BuilderFlowNode currentRoot, List<BuilderFlowNode> currentRunningList, FlowCompositeMetastructure currentMetastructure)
		{
			List<BuilderFlowNode> list = this.FindSimplestContainingLoop(currentRoot, currentMetastructure.usedList);
			if (list != null)
			{
				currentMetastructure.loopLists.Add(list);
				LoopBuilderComposite loopBuilderComposite = new LoopBuilderComposite(list, this.m_flow, this, LoopBuilderComposite.CompositeStyle.LOOP);
				currentMetastructure.usedList.AddRange(list);
				List<BuilderFlowNode> externalConnectedNodes = loopBuilderComposite.ExternalConnectedNodes;
				for (int i = 0; i < externalConnectedNodes.Count; i++)
				{
					BuilderFlowNode builderFlowNode = externalConnectedNodes[i];
					BuilderFlowNode connectedInternalNode = loopBuilderComposite.GetConnectedInternalNode(builderFlowNode);
					if (builderFlowNode.loopConnectedBuilderNode != connectedInternalNode && connectedInternalNode.loopConnectedBuilderNode != builderFlowNode)
					{
						if (!currentMetastructure.usedList.Contains(builderFlowNode))
						{
							this.ConvertTreeToCompositeStructure(builderFlowNode, null, currentMetastructure);
						}
					}
				}
			}
			else
			{
				if (currentRoot.node.isWarpWingEntrance)
				{
					currentRunningList = null;
				}
				else if (currentRoot.IsInjectedNode && currentRoot.node.childNodeGuids.Count == 0)
				{
					currentRunningList = null;
				}
				if (currentRunningList == null)
				{
					currentRunningList = new List<BuilderFlowNode>();
					currentMetastructure.compositeLists.Add(currentRunningList);
				}
				currentRunningList.Add(currentRoot);
				currentMetastructure.usedList.Add(currentRoot);
				for (int j = 0; j < currentRoot.childBuilderNodes.Count; j++)
				{
					BuilderFlowNode builderFlowNode2 = currentRoot.childBuilderNodes[j];
					if (!currentMetastructure.usedList.Contains(builderFlowNode2))
					{
						this.ConvertTreeToCompositeStructure(builderFlowNode2, currentRunningList, currentMetastructure);
					}
				}
			}
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x001D6F3C File Offset: 0x001D513C
		protected bool ConnectTwoPlacedLayoutNodes(BuilderFlowNode internalNode, BuilderFlowNode externalNode, SemioticLayoutManager layout)
		{
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> list = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> list2 = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			bool flag = false;
			for (int i = 0; i < externalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count; i++)
			{
				for (int j = 0; j < internalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count; j++)
				{
					PrototypeRoomExit prototypeRoomExit = externalNode.instanceRoom.area.prototypeRoom.exitData.exits[i];
					PrototypeRoomExit prototypeRoomExit2 = internalNode.instanceRoom.area.prototypeRoom.exitData.exits[j];
					if (!externalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit) && !internalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit2))
					{
						RuntimeRoomExitData runtimeRoomExitData = new RuntimeRoomExitData(prototypeRoomExit);
						RuntimeRoomExitData runtimeRoomExitData2 = new RuntimeRoomExitData(prototypeRoomExit2);
						Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = new Tuple<RuntimeRoomExitData, RuntimeRoomExitData>(runtimeRoomExitData, runtimeRoomExitData2);
						if ((runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.EAST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.WEST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.WEST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.EAST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.NORTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.SOUTH) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.SOUTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.NORTH))
						{
							list.Add(tuple);
						}
						else if (runtimeRoomExitData.referencedExit.exitDirection != runtimeRoomExitData2.referencedExit.exitDirection)
						{
							list2.Add(tuple);
						}
					}
				}
			}
			list.AddRange(list2);
			RuntimeRoomExitData runtimeRoomExitData3 = null;
			RuntimeRoomExitData runtimeRoomExitData4 = null;
			List<IntVector2> list3 = null;
			int num = int.MaxValue;
			for (int k = 0; k < list.Count; k++)
			{
				RuntimeRoomExitData first = list[k].First;
				RuntimeRoomExitData second = list[k].Second;
				PrototypeRoomExit referencedExit = first.referencedExit;
				PrototypeRoomExit referencedExit2 = second.referencedExit;
				IntVector2 intVector = externalNode.instanceRoom.area.basePosition + referencedExit.GetExitOrigin(referencedExit.exitLength + 3) - IntVector2.One;
				IntVector2 intVector2 = internalNode.instanceRoom.area.basePosition + referencedExit2.GetExitOrigin(referencedExit2.exitLength + 3) - IntVector2.One;
				SemioticLayoutManager semioticLayoutManager = new SemioticLayoutManager();
				semioticLayoutManager.MergeLayout(layout);
				RuntimeRoomExitData runtimeRoomExitData5 = new RuntimeRoomExitData(referencedExit);
				runtimeRoomExitData5.additionalExitLength = 1;
				RuntimeRoomExitData runtimeRoomExitData6 = new RuntimeRoomExitData(referencedExit2);
				runtimeRoomExitData6.additionalExitLength = 1;
				semioticLayoutManager.StampComplexExitTemporary(runtimeRoomExitData5, externalNode.instanceRoom.area);
				semioticLayoutManager.StampComplexExitTemporary(runtimeRoomExitData6, internalNode.instanceRoom.area);
				List<IntVector2> list4 = semioticLayoutManager.PathfindHallway(intVector, intVector2);
				semioticLayoutManager.ClearTemporary();
				semioticLayoutManager.OnDestroy();
				if (list4 != null && list4.Count > 0 && list4.Count < num)
				{
					runtimeRoomExitData3 = first;
					runtimeRoomExitData4 = second;
					list3 = list4;
					num = list4.Count;
					flag = true;
				}
			}
			if (flag)
			{
				runtimeRoomExitData3.additionalExitLength = 0;
				runtimeRoomExitData4.additionalExitLength = 0;
				RoomHandler roomHandler = LoopBuilderComposite.PlaceProceduralPathRoom(list3, runtimeRoomExitData3, runtimeRoomExitData4, externalNode.instanceRoom, internalNode.instanceRoom, layout);
				if (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
				{
					runtimeRoomExitData3.oneWayDoor = true;
				}
				this.m_currentMaxLengthProceduralHallway = Mathf.Max(this.m_currentMaxLengthProceduralHallway, list3.Count);
			}
			return flag;
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x001D7300 File Offset: 0x001D5500
		protected IEnumerable AttachWarpCanvasToLayout(BuilderFlowNode externalNode, BuilderFlowNode internalNode, SemioticLayoutManager canvas, SemioticLayoutManager layout)
		{
			this.AttachWarpCanvasSuccess = false;
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairs = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			for (int i = 0; i < externalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count; i++)
			{
				PrototypeRoomExit prototypeRoomExit = externalNode.instanceRoom.area.prototypeRoom.exitData.exits[i];
				for (int j = 0; j < internalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count; j++)
				{
					PrototypeRoomExit prototypeRoomExit2 = internalNode.instanceRoom.area.prototypeRoom.exitData.exits[j];
					if (!externalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit) && !internalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit2))
					{
						RuntimeRoomExitData runtimeRoomExitData = new RuntimeRoomExitData(prototypeRoomExit);
						RuntimeRoomExitData runtimeRoomExitData2 = new RuntimeRoomExitData(prototypeRoomExit2);
						Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = new Tuple<RuntimeRoomExitData, RuntimeRoomExitData>(runtimeRoomExitData, runtimeRoomExitData2);
						exitPairs.Add(tuple);
						break;
					}
				}
			}
			if (GameManager.Instance.GeneratingLevelOverrideState != GameManager.LevelOverrideState.FOYER && !externalNode.node.handlesOwnWarping && !internalNode.node.handlesOwnWarping)
			{
				if (exitPairs.Count == 0)
				{
					Debug.LogError("A warp wing has no exits and is not flagged as handling its own warping!");
					this.AttachWarpCanvasSuccess = false;
					yield break;
				}
				RuntimeRoomExitData placedExitData = exitPairs[0].First;
				RuntimeRoomExitData newExitData = exitPairs[0].Second;
				PrototypeRoomExit placedExit = placedExitData.referencedExit;
				PrototypeRoomExit newExit = newExitData.referencedExit;
				placedExitData.additionalExitLength = 4;
				newExitData.additionalExitLength = 4;
				placedExitData.isWarpWingStart = true;
				newExitData.isWarpWingStart = true;
				internalNode.exitToNodeMap.Add(newExit, externalNode);
				internalNode.nodeToExitMap.Add(externalNode, newExit);
				externalNode.exitToNodeMap.Add(placedExit, internalNode);
				externalNode.nodeToExitMap.Add(internalNode, placedExit);
				layout.StampComplexExitToLayout(placedExitData, externalNode.instanceRoom.area, false);
				layout.StampComplexExitToLayout(newExitData, internalNode.instanceRoom.area, false);
				placedExitData.linkedExit = newExitData;
				newExitData.linkedExit = placedExitData;
				externalNode.instanceRoom.RegisterConnectedRoom(internalNode.instanceRoom, placedExitData);
				internalNode.instanceRoom.RegisterConnectedRoom(externalNode.instanceRoom, newExitData);
				yield return null;
			}
			IntVector2 canvasTranslationToZero = canvas.NegativeDimensions;
			int canvasDistanceApart = 10;
			if (GameManager.Instance.GeneratingLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				canvasDistanceApart = 25;
			}
			IntVector2 additionalTranslation = new IntVector2(layout.PositiveDimensions.x + canvasDistanceApart, 0);
			canvas.HandleOffsetRooms(canvasTranslationToZero + additionalTranslation);
			layout.MergeLayout(canvas);
			this.AttachWarpCanvasSuccess = true;
			yield break;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x001D7340 File Offset: 0x001D5540
		protected bool NodeHasExitGroupsToCheck(BuilderFlowNode node)
		{
			List<PrototypeRoomExit.ExitGroup> definedExitGroups = node.assignedPrototypeRoom.exitData.GetDefinedExitGroups();
			bool flag = definedExitGroups.Count > 1;
			for (int i = 0; i < node.instanceRoom.area.instanceUsedExits.Count; i++)
			{
				definedExitGroups.Remove(node.instanceRoom.area.instanceUsedExits[i].exitGroup);
			}
			if (definedExitGroups.Count == 0)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001D73C0 File Offset: 0x001D55C0
		protected IEnumerable AttachNewCanvasToLayout(BuilderFlowNode externalNode, BuilderFlowNode internalNode, SemioticLayoutManager canvas, SemioticLayoutManager layout)
		{
			this.AttachNewCanvasSuccess = false;
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> exitPairs = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>> jointedPairs = new List<Tuple<RuntimeRoomExitData, RuntimeRoomExitData>>();
			bool success = false;
			bool supportsJointedPairs = true;
			if ((externalNode.assignedPrototypeRoom != null && externalNode.Category == PrototypeDungeonRoom.RoomCategory.SECRET) || (internalNode.assignedPrototypeRoom != null && internalNode.Category == PrototypeDungeonRoom.RoomCategory.SECRET))
			{
				supportsJointedPairs = false;
			}
			if (externalNode.instanceRoom == null)
			{
				BraveUtility.Log(externalNode.node.guidAsString, Color.magenta, BraveUtility.LogVerbosity.IMPORTANT);
			}
			bool externalNodeHasExitGroups = this.NodeHasExitGroupsToCheck(externalNode);
			bool internalNodeHasExitGroups = this.NodeHasExitGroupsToCheck(internalNode);
			int j = 0;
			while (j < externalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count)
			{
				PrototypeRoomExit prototypeRoomExit = externalNode.instanceRoom.area.prototypeRoom.exitData.exits[j];
				if (!externalNodeHasExitGroups)
				{
					goto IL_1AA;
				}
				bool flag = false;
				for (int k = 0; k < externalNode.instanceRoom.area.instanceUsedExits.Count; k++)
				{
					if (externalNode.instanceRoom.area.instanceUsedExits[k].exitGroup == prototypeRoomExit.exitGroup)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					goto IL_1AA;
				}
				IL_3BA:
				j++;
				continue;
				IL_1AA:
				int l = 0;
				while (l < internalNode.instanceRoom.area.prototypeRoom.exitData.exits.Count)
				{
					PrototypeRoomExit prototypeRoomExit2 = internalNode.instanceRoom.area.prototypeRoom.exitData.exits[l];
					if (!internalNodeHasExitGroups)
					{
						goto IL_258;
					}
					bool flag2 = false;
					for (int m = 0; m < internalNode.instanceRoom.area.instanceUsedExits.Count; m++)
					{
						if (internalNode.instanceRoom.area.instanceUsedExits[m].exitGroup == prototypeRoomExit2.exitGroup)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						goto IL_258;
					}
					IL_389:
					l++;
					continue;
					IL_258:
					if (externalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit) || internalNode.instanceRoom.area.instanceUsedExits.Contains(prototypeRoomExit2))
					{
						goto IL_389;
					}
					RuntimeRoomExitData runtimeRoomExitData = new RuntimeRoomExitData(prototypeRoomExit);
					RuntimeRoomExitData runtimeRoomExitData2 = new RuntimeRoomExitData(prototypeRoomExit2);
					Tuple<RuntimeRoomExitData, RuntimeRoomExitData> tuple = new Tuple<RuntimeRoomExitData, RuntimeRoomExitData>(runtimeRoomExitData, runtimeRoomExitData2);
					if ((runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.EAST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.WEST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.WEST && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.EAST) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.NORTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.SOUTH) || (runtimeRoomExitData.referencedExit.exitDirection == DungeonData.Direction.SOUTH && runtimeRoomExitData2.referencedExit.exitDirection == DungeonData.Direction.NORTH))
					{
						exitPairs.Add(tuple);
						goto IL_389;
					}
					if (runtimeRoomExitData.referencedExit.exitDirection == runtimeRoomExitData2.referencedExit.exitDirection)
					{
						goto IL_389;
					}
					jointedPairs.Add(tuple);
					goto IL_389;
				}
				goto IL_3BA;
			}
			if (supportsJointedPairs)
			{
				exitPairs.AddRange(jointedPairs);
			}
			for (int i = 0; i < exitPairs.Count; i++)
			{
				RuntimeRoomExitData placedExitData = exitPairs[i].First;
				RuntimeRoomExitData newExitData = exitPairs[i].Second;
				PrototypeRoomExit placedExit = placedExitData.referencedExit;
				PrototypeRoomExit newExit = newExitData.referencedExit;
				IEnumerator CanPlaceLayoutTracker = layout.CanPlaceLayoutAtPoint(canvas, placedExitData, newExitData, externalNode.instanceRoom.area.basePosition, internalNode.instanceRoom.area.basePosition).GetEnumerator();
				while (CanPlaceLayoutTracker.MoveNext())
				{
					yield return null;
				}
				if (layout.CanPlaceLayoutAtPointSuccess)
				{
					if (!internalNode.exitToNodeMap.ContainsKey(newExit) && !externalNode.exitToNodeMap.ContainsKey(placedExit))
					{
						IntVector2 intVector = externalNode.instanceRoom.area.basePosition + placedExitData.ExitOrigin - IntVector2.One;
						IntVector2 intVector2 = internalNode.instanceRoom.area.basePosition + newExitData.ExitOrigin - IntVector2.One;
						IntVector2 intVector3 = intVector - intVector2;
						canvas.HandleOffsetRooms(intVector3);
						layout.MergeLayout(canvas);
						internalNode.exitToNodeMap.Add(newExit, externalNode);
						internalNode.nodeToExitMap.Add(externalNode, newExit);
						externalNode.exitToNodeMap.Add(placedExit, internalNode);
						externalNode.nodeToExitMap.Add(internalNode, placedExit);
						layout.StampComplexExitToLayout(placedExitData, externalNode.instanceRoom.area, false);
						layout.StampComplexExitToLayout(newExitData, internalNode.instanceRoom.area, false);
						placedExitData.linkedExit = newExitData;
						newExitData.linkedExit = placedExitData;
						if ((externalNode.parentBuilderNode == internalNode && externalNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.ONE_WAY) || (internalNode.parentBuilderNode == externalNode && internalNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.ONE_WAY))
						{
							placedExitData.oneWayDoor = true;
							newExitData.oneWayDoor = true;
						}
						if ((externalNode.parentBuilderNode == internalNode && externalNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.LOCKED) || (internalNode.parentBuilderNode == externalNode && internalNode.node.forcedDoorType == DungeonFlowNode.ForcedDoorType.LOCKED))
						{
							placedExitData.isLockedDoor = true;
							newExitData.isLockedDoor = true;
						}
						externalNode.instanceRoom.RegisterConnectedRoom(internalNode.instanceRoom, placedExitData);
						internalNode.instanceRoom.RegisterConnectedRoom(externalNode.instanceRoom, newExitData);
						success = true;
						break;
					}
				}
				else
				{
					yield return null;
				}
			}
			this.AttachNewCanvasSuccess = success;
			yield break;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x001D7400 File Offset: 0x001D5600
		public SemioticLayoutManager Build(out bool generationSucceeded)
		{
			IEnumerator enumerator = this.DeferredBuild().GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			generationSucceeded = this.DeferredGenerationSuccess;
			return this.DeferredGeneratedLayout;
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x001D7438 File Offset: 0x001D5638
		private void AttachPregeneratedInjectionData()
		{
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				return;
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
			{
				return;
			}
			if (GameManager.Instance.GeneratingLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId;
				if (MetaInjectionData.CurrentRunBlueprint.ContainsKey(tilesetId))
				{
					for (int i = 0; i < MetaInjectionData.CurrentRunBlueprint[tilesetId].Count; i++)
					{
						this.m_postprocessInjectionData.Add(MetaInjectionData.CurrentRunBlueprint[tilesetId][i]);
						if (MetaInjectionData.CurrentRunBlueprint[tilesetId][i].injectionData.name.Contains("Subshop"))
						{
							this.m_runtimeInjectionFlags.ShopAnnexed = true;
						}
					}
				}
			}
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x001D7510 File Offset: 0x001D5710
		private bool IsCompositeWarpWing(LoopBuilderComposite composite)
		{
			for (int i = 0; i < composite.ExternalConnectedNodes.Count; i++)
			{
				BuilderFlowNode builderFlowNode = composite.ExternalConnectedNodes[i];
				BuilderFlowNode connectedInternalNode = composite.GetConnectedInternalNode(builderFlowNode);
				if (connectedInternalNode != null && connectedInternalNode.node.isWarpWingEntrance)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x001D7568 File Offset: 0x001D5768
		public IEnumerable DeferredBuild()
		{
			this.DeferredGenerationSuccess = true;
			this.m_postprocessInjectionData.Clear();
			this.m_runtimeInjectionFlags.Clear();
			this.AttachPregeneratedInjectionData();
			BuilderFlowNode initialRoot = this.ComposeFlowTree();
			yield return null;
			this.PerformOperationOnTreeNodes(initialRoot, new Action<BuilderFlowNode>(this.AcquirePrototypeRoom));
			this.PerformOperationOnTreeNodes(initialRoot, new Action<BuilderFlowNode>(this.HandleBossFoyerAcquisition));
			FlowCompositeMetastructure preinjectionMetastructure = new FlowCompositeMetastructure();
			this.ConvertTreeToCompositeStructure(initialRoot, null, preinjectionMetastructure);
			for (int k = 0; k < this.m_postprocessInjectionData.Count; k++)
			{
				this.HandleNodeInjection(initialRoot, this.m_postprocessInjectionData[k], this.m_runtimeInjectionFlags, preinjectionMetastructure);
			}
			if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && GameManager.Instance.CurrentGameMode != GameManager.GameMode.SUPERBOSSRUSH)
			{
				this.HandleNodeInjection(initialRoot, this.m_flow.flowInjectionData, this.m_runtimeInjectionFlags, preinjectionMetastructure);
				for (int l = 0; l < this.m_flow.sharedInjectionData.Count; l++)
				{
					if (this.m_previouslyGeneratedRuntimeMetadata.ContainsKey(this.m_flow.sharedInjectionData[l]))
					{
						Debug.Log("Previously injected: " + this.m_flow.sharedInjectionData[l].name);
						if (this.m_previouslyGeneratedRuntimeMetadata[this.m_flow.sharedInjectionData[l]].AssignedModifierData != null)
						{
							Debug.Log("Using prior: " + this.m_previouslyGeneratedRuntimeMetadata[this.m_flow.sharedInjectionData[l]].AssignedModifierData.annotation);
						}
						this.HandleNodeInjection(initialRoot, this.m_previouslyGeneratedRuntimeMetadata[this.m_flow.sharedInjectionData[l]], this.m_runtimeInjectionFlags, preinjectionMetastructure);
					}
					else
					{
						Debug.Log("No prior injection: " + this.m_flow.sharedInjectionData[l].name);
						RuntimeInjectionMetadata runtimeInjectionMetadata = new RuntimeInjectionMetadata(this.m_flow.sharedInjectionData[l]);
						this.m_previouslyGeneratedRuntimeMetadata.Add(this.m_flow.sharedInjectionData[l], runtimeInjectionMetadata);
						this.HandleNodeInjection(initialRoot, runtimeInjectionMetadata, this.m_runtimeInjectionFlags, preinjectionMetastructure);
					}
				}
			}
			BuilderFlowNode connectedRoot = this.RerootTreeAtHighestConnectivity(initialRoot);
			yield return null;
			FlowCompositeMetastructure metastructure = new FlowCompositeMetastructure();
			this.ConvertTreeToCompositeStructure(connectedRoot, null, metastructure);
			yield return null;
			List<LoopBuilderComposite> composites = new List<LoopBuilderComposite>();
			Dictionary<BuilderFlowNode, LoopBuilderComposite> nodeToCompositeMap = new Dictionary<BuilderFlowNode, LoopBuilderComposite>();
			for (int m = 0; m < metastructure.loopLists.Count; m++)
			{
				LoopBuilderComposite loopBuilderComposite = new LoopBuilderComposite(metastructure.loopLists[m], this.m_flow, this, LoopBuilderComposite.CompositeStyle.LOOP);
				composites.Add(loopBuilderComposite);
				for (int n = 0; n < metastructure.loopLists[m].Count; n++)
				{
					nodeToCompositeMap.Add(metastructure.loopLists[m][n], loopBuilderComposite);
				}
			}
			for (int num = 0; num < metastructure.compositeLists.Count; num++)
			{
				LoopBuilderComposite loopBuilderComposite2 = new LoopBuilderComposite(metastructure.compositeLists[num], this.m_flow, this, LoopBuilderComposite.CompositeStyle.NON_LOOP);
				composites.Add(loopBuilderComposite2);
				for (int num2 = 0; num2 < metastructure.compositeLists[num].Count; num2++)
				{
					nodeToCompositeMap.Add(metastructure.compositeLists[num][num2], loopBuilderComposite2);
				}
			}
			List<SemioticLayoutManager> canvases = new List<SemioticLayoutManager>();
			int regenerationAttemptCounter = 0;
			for (int i = 0; i < composites.Count; i++)
			{
				LoopBuilderComposite composite = composites[i];
				IEnumerator compositeTracker = composite.Build(IntVector2.Zero).GetEnumerator();
				while (compositeTracker.MoveNext())
				{
					yield return null;
				}
				SemioticLayoutManager compositeCanvas = composite.CompletedCanvas;
				int COMPOSITE_REGENERATION_ATTEMPTS = ((composite.loopStyle == LoopBuilderComposite.CompositeStyle.NON_LOOP) ? 5 : 100);
				if (composite.RequiresRegeneration && regenerationAttemptCounter < COMPOSITE_REGENERATION_ATTEMPTS)
				{
					regenerationAttemptCounter++;
					LoopBuilderComposite loopBuilderComposite3 = new LoopBuilderComposite(composite.Nodes, this.m_flow, this, composite.loopStyle);
					for (int num3 = 0; num3 < loopBuilderComposite3.Nodes.Count; num3++)
					{
						if (loopBuilderComposite3.Nodes[num3].assignedPrototypeRoom != null && loopBuilderComposite3.Nodes[num3].assignedPrototypeRoom.injectionFlags.CastleFireplace)
						{
							Debug.LogWarning(" ======> NOT Reacquiring for this room. <====== ");
						}
						else
						{
							this.AcquirePrototypeRoom(loopBuilderComposite3.Nodes[num3]);
						}
						loopBuilderComposite3.Nodes[num3].ClearData();
						nodeToCompositeMap[loopBuilderComposite3.Nodes[num3]] = loopBuilderComposite3;
					}
					composites[i] = loopBuilderComposite3;
					i--;
				}
				else
				{
					if (composite.RequiresRegeneration)
					{
						LoopDungeonGenerator.NUM_FAILS_COMPOSITE_REGENERATION++;
						IL_DAC:
						this.DeferredGenerationSuccess = false;
						yield break;
					}
					regenerationAttemptCounter = 0;
					canvases.Add(compositeCanvas);
				}
			}
			this.PerformOperationOnTreeNodes(connectedRoot, new Action<BuilderFlowNode>(this.AssignInjectionDataToRoomHandler));
			SemioticLayoutManager layout = new SemioticLayoutManager();
			if (!this.DEBUG_RENDER_CANVASES_SEPARATELY)
			{
				Queue<LoopBuilderComposite> compositesToProcess = new Queue<LoopBuilderComposite>();
				List<LoopBuilderComposite> mergedComposites = new List<LoopBuilderComposite>();
				List<LoopBuilderComposite> warpCompositesToQueueLater = new List<LoopBuilderComposite>();
				compositesToProcess.Enqueue(composites[0]);
				while (compositesToProcess.Count > 0)
				{
					LoopBuilderComposite composite2 = compositesToProcess.Dequeue();
					int compositeIndex = composites.IndexOf(composite2);
					SemioticLayoutManager canvas = canvases[compositeIndex];
					bool compositeHasBeenAttached = false;
					if (mergedComposites.Count == 0)
					{
						layout.MergeLayout(canvas);
						compositeHasBeenAttached = true;
					}
					if (mergedComposites.Contains(composite2))
					{
						compositeHasBeenAttached = true;
					}
					List<LoopBuilderComposite> compositesToEnqueue = new List<LoopBuilderComposite>();
					List<BuilderFlowNode> externalLinks = composite2.ExternalConnectedNodes;
					for (int j = 0; j < externalLinks.Count; j++)
					{
						BuilderFlowNode externalNode = externalLinks[j];
						LoopBuilderComposite externalLinkComposite = nodeToCompositeMap[externalNode];
						if (mergedComposites.Contains(externalLinkComposite))
						{
							BuilderFlowNode internalNode = composite2.GetConnectedInternalNode(externalNode);
							if (compositeHasBeenAttached)
							{
								bool flag = this.ConnectTwoPlacedLayoutNodes(internalNode, externalNode, layout);
								Debug.Log("Attempting to connect two extant nodes... " + flag);
							}
							else
							{
								bool success = false;
								if (internalNode.node.isWarpWingEntrance)
								{
									IEnumerator AttachTracker = this.AttachWarpCanvasToLayout(externalNode, internalNode, canvas, layout).GetEnumerator();
									while (AttachTracker.MoveNext())
									{
										yield return null;
									}
									success = this.AttachWarpCanvasSuccess;
								}
								else
								{
									IEnumerator AttachTracker2 = this.AttachNewCanvasToLayout(externalNode, internalNode, canvas, layout).GetEnumerator();
									while (AttachTracker2.MoveNext())
									{
										yield return null;
									}
									success = this.AttachNewCanvasSuccess;
								}
								compositeHasBeenAttached = true;
								if (!success)
								{
									LoopDungeonGenerator.NUM_FAILS_COMPOSITE_ATTACHMENT++;
									goto IL_DAC;
								}
							}
						}
						else if (this.IsCompositeWarpWing(externalLinkComposite))
						{
							if (!warpCompositesToQueueLater.Contains(externalLinkComposite))
							{
								warpCompositesToQueueLater.Add(externalLinkComposite);
							}
						}
						else if (!compositesToEnqueue.Contains(externalLinkComposite))
						{
							compositesToEnqueue.Add(externalLinkComposite);
						}
					}
					mergedComposites.Add(composite2);
					compositesToEnqueue.Sort((LoopBuilderComposite a, LoopBuilderComposite b) => b.Nodes.Count - a.Nodes.Count);
					for (int num4 = 0; num4 < compositesToEnqueue.Count; num4++)
					{
						compositesToProcess.Enqueue(compositesToEnqueue[num4]);
					}
					if (compositesToProcess.Count == 0 && warpCompositesToQueueLater.Count > 0)
					{
						while (warpCompositesToQueueLater.Count > 0)
						{
							compositesToProcess.Enqueue(warpCompositesToQueueLater[0]);
							warpCompositesToQueueLater.RemoveAt(0);
						}
					}
					yield return null;
				}
			}
			else
			{
				IntVector2 intVector = IntVector2.Zero;
				for (int num5 = 0; num5 < canvases.Count; num5++)
				{
					SemioticLayoutManager semioticLayoutManager = canvases[num5];
					intVector += semioticLayoutManager.NegativeDimensions;
					semioticLayoutManager.HandleOffsetRooms(new IntVector2(intVector.x, 0));
					intVector += semioticLayoutManager.PositiveDimensions + new IntVector2(10, 0);
					layout.MergeLayout(semioticLayoutManager);
				}
			}
			if (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				BuilderFlowNode builderFlowNode = null;
				BuilderFlowNode builderFlowNode2 = null;
				for (int num6 = 0; num6 < this.allBuilderNodes.Count; num6++)
				{
					BuilderFlowNode builderFlowNode3 = this.allBuilderNodes[num6];
					if (builderFlowNode3 != null && !(builderFlowNode3.assignedPrototypeRoom == null))
					{
						if (builderFlowNode3.assignedPrototypeRoom.name == "OFFICE_03_OFFICE_01")
						{
							builderFlowNode2 = builderFlowNode3;
						}
						if (builderFlowNode3.assignedPrototypeRoom.name == "OFFICE_10_RND_01")
						{
							builderFlowNode = builderFlowNode3;
						}
					}
				}
				if (builderFlowNode != null && builderFlowNode2 != null)
				{
					bool flag2 = this.ConnectTwoPlacedLayoutNodes(builderFlowNode, builderFlowNode2, layout);
				}
			}
			this.SanityCheckRooms(layout);
			this.DeferredGenerationSuccess = true;
			this.DeferredGeneratedLayout = layout;
			yield break;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x001D758C File Offset: 0x001D578C
		private void SanityCheckRooms(SemioticLayoutManager layout)
		{
			for (int i = 0; i < layout.Rooms.Count; i++)
			{
				RoomHandler roomHandler = layout.Rooms[i];
				if (!roomHandler.area.IsProceduralRoom)
				{
					bool flag = false;
					for (int j = 0; j < this.allBuilderNodes.Count; j++)
					{
						BuilderFlowNode builderFlowNode = this.allBuilderNodes[j];
						if (builderFlowNode.instanceRoom == roomHandler)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						layout.Rooms.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x04004A51 RID: 19025
		public Dictionary<DungeonFlowNode, int> usedSubchainData = new Dictionary<DungeonFlowNode, int>();

		// Token: 0x04004A52 RID: 19026
		public bool DEBUG_RENDER_CANVASES_SEPARATELY;

		// Token: 0x04004A53 RID: 19027
		protected DungeonFlow m_flow;

		// Token: 0x04004A54 RID: 19028
		protected LoopDungeonGenerator m_generator;

		// Token: 0x04004A55 RID: 19029
		protected List<BuilderFlowNode> allBuilderNodes = new List<BuilderFlowNode>();

		// Token: 0x04004A56 RID: 19030
		protected Dictionary<PrototypeDungeonRoom, int> m_usedPrototypeRoomData = new Dictionary<PrototypeDungeonRoom, int>();

		// Token: 0x04004A57 RID: 19031
		protected List<PrototypeDungeonRoom> m_excludedRoomData = new List<PrototypeDungeonRoom>();

		// Token: 0x04004A58 RID: 19032
		protected int m_currentMaxLengthProceduralHallway;

		// Token: 0x04004A59 RID: 19033
		protected Dictionary<DungeonFlowSubtypeRestriction, int> roomsOfSubtypeRemaining;

		// Token: 0x04004A5A RID: 19034
		public static ObjectPool<List<BuilderFlowNode>> BuilderFlowNodeListPool = new ObjectPool<List<BuilderFlowNode>>(() => new List<BuilderFlowNode>(), 10, null);

		// Token: 0x04004A5B RID: 19035
		protected bool AttachWarpCanvasSuccess;

		// Token: 0x04004A5C RID: 19036
		protected bool AttachNewCanvasSuccess;

		// Token: 0x04004A5D RID: 19037
		protected const int MAX_LOOP_REGENERATION_ATTEMPTS = 100;

		// Token: 0x04004A5E RID: 19038
		protected const int MAX_NONLOOP_REGENERATION_ATTEMPTS = 5;

		// Token: 0x04004A5F RID: 19039
		public SemioticLayoutManager DeferredGeneratedLayout;

		// Token: 0x04004A60 RID: 19040
		public bool DeferredGenerationSuccess;

		// Token: 0x04004A61 RID: 19041
		private List<RuntimeInjectionMetadata> m_postprocessInjectionData = new List<RuntimeInjectionMetadata>();

		// Token: 0x04004A62 RID: 19042
		private RuntimeInjectionFlags m_runtimeInjectionFlags = new RuntimeInjectionFlags();

		// Token: 0x04004A63 RID: 19043
		private Dictionary<SharedInjectionData, RuntimeInjectionMetadata> m_previouslyGeneratedRuntimeMetadata = new Dictionary<SharedInjectionData, RuntimeInjectionMetadata>();

		// Token: 0x02000F08 RID: 3848
		public enum FallbackLevel
		{
			// Token: 0x04004A65 RID: 19045
			NOT_FALLBACK,
			// Token: 0x04004A66 RID: 19046
			FALLBACK_STANDARD,
			// Token: 0x04004A67 RID: 19047
			FALLBACK_EMERGENCY
		}
	}
}
