using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E6C RID: 3692
public class DungeonFlow : ScriptableObject
{
	// Token: 0x06004E8A RID: 20106 RVA: 0x001B21A0 File Offset: 0x001B03A0
	public int GetAverageNumberRooms()
	{
		int num = 0;
		for (int i = 0; i < this.m_nodes.Count; i++)
		{
			num += this.m_nodes[i].GetAverageNumberNodes();
		}
		return num;
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x06004E8B RID: 20107 RVA: 0x001B21E0 File Offset: 0x001B03E0
	public List<DungeonFlowNode> AllNodes
	{
		get
		{
			return this.m_nodes;
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x06004E8C RID: 20108 RVA: 0x001B21E8 File Offset: 0x001B03E8
	// (set) Token: 0x06004E8D RID: 20109 RVA: 0x001B21F8 File Offset: 0x001B03F8
	public DungeonFlowNode FirstNode
	{
		get
		{
			return this.GetNodeFromGuid(this.m_firstNodeGuid);
		}
		set
		{
			this.m_firstNodeGuid = value.guidAsString;
		}
	}

	// Token: 0x06004E8E RID: 20110 RVA: 0x001B2208 File Offset: 0x001B0408
	public void Initialize()
	{
		this.m_nodes = new List<DungeonFlowNode>();
		this.m_nodeGuids = new List<string>();
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x001B2220 File Offset: 0x001B0420
	public void GetNodesRecursive(DungeonFlowNode node, List<DungeonFlowNode> all)
	{
		if (node == null)
		{
			return;
		}
		all.Add(node);
		if (node.childNodeGuids == null)
		{
			node.childNodeGuids = new List<string>();
		}
		foreach (string text in node.childNodeGuids)
		{
			DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(text);
			this.GetNodesRecursive(nodeFromGuid, all);
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x001B22B0 File Offset: 0x001B04B0
	public void AddNodeToFlow(DungeonFlowNode newNode, DungeonFlowNode parent)
	{
		if (this.m_nodeGuids == null || this.m_nodes == null)
		{
			this.Initialize();
		}
		if (this.m_nodeGuids.Contains(newNode.guidAsString))
		{
			return;
		}
		this.m_nodes.Add(newNode);
		this.m_nodeGuids.Add(newNode.guidAsString);
		if (parent != null)
		{
			if (!parent.childNodeGuids.Contains(newNode.guidAsString))
			{
				parent.childNodeGuids.Add(newNode.guidAsString);
				newNode.parentNodeGuid = parent.guidAsString;
			}
		}
		else
		{
			newNode.parentNodeGuid = string.Empty;
		}
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x001B235C File Offset: 0x001B055C
	public void DeleteNode(DungeonFlowNode node, bool deleteChain = false)
	{
		if (deleteChain)
		{
			List<DungeonFlowNode> list = new List<DungeonFlowNode>();
			this.GetNodesRecursive(node, list);
			foreach (DungeonFlowNode dungeonFlowNode in list)
			{
				this.RemoveNodeInternal(dungeonFlowNode);
			}
		}
		else
		{
			this.RemoveNodeInternal(node);
		}
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x001B23D4 File Offset: 0x001B05D4
	private void RemoveNodeInternal(DungeonFlowNode node)
	{
		if (!string.IsNullOrEmpty(node.parentNodeGuid))
		{
			DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(node.parentNodeGuid);
			nodeFromGuid.childNodeGuids.Remove(node.guidAsString);
		}
		foreach (string text in node.childNodeGuids)
		{
			DungeonFlowNode nodeFromGuid2 = this.GetNodeFromGuid(text);
			nodeFromGuid2.parentNodeGuid = string.Empty;
		}
		if (!string.IsNullOrEmpty(node.loopTargetNodeGuid))
		{
			DungeonFlowNode nodeFromGuid3 = this.GetNodeFromGuid(node.loopTargetNodeGuid);
			nodeFromGuid3.loopTargetNodeGuid = string.Empty;
		}
		for (int i = 0; i < this.m_nodes.Count; i++)
		{
			if (this.m_nodes[i].loopTargetNodeGuid == node.guidAsString)
			{
				this.m_nodes[i].loopTargetNodeGuid = string.Empty;
			}
		}
		node.parentNodeGuid = string.Empty;
		node.childNodeGuids.Clear();
		node.loopTargetNodeGuid = string.Empty;
		this.m_nodes.Remove(node);
		this.m_nodeGuids.Remove(node.guidAsString);
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x001B2530 File Offset: 0x001B0730
	public bool IsPartOfSubchain(DungeonFlowNode node)
	{
		DungeonFlowNode dungeonFlowNode = node;
		while (!string.IsNullOrEmpty(dungeonFlowNode.parentNodeGuid))
		{
			dungeonFlowNode = this.GetNodeFromGuid(dungeonFlowNode.parentNodeGuid);
		}
		return dungeonFlowNode != this.FirstNode;
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001B2578 File Offset: 0x001B0778
	private PrototypeDungeonRoom.RoomCategory GetCategoryFromChar(char c)
	{
		PrototypeDungeonRoom.RoomCategory roomCategory;
		switch (c)
		{
		case 'b':
			roomCategory = PrototypeDungeonRoom.RoomCategory.BOSS;
			break;
		case 'c':
			roomCategory = PrototypeDungeonRoom.RoomCategory.CONNECTOR;
			break;
		default:
			switch (c)
			{
			case 'r':
				roomCategory = PrototypeDungeonRoom.RoomCategory.REWARD;
				break;
			case 's':
				roomCategory = PrototypeDungeonRoom.RoomCategory.SPECIAL;
				break;
			case 't':
				roomCategory = PrototypeDungeonRoom.RoomCategory.SECRET;
				break;
			default:
				if (c != 'n')
				{
					roomCategory = PrototypeDungeonRoom.RoomCategory.NORMAL;
				}
				else
				{
					roomCategory = PrototypeDungeonRoom.RoomCategory.NORMAL;
				}
				break;
			case 'x':
				roomCategory = PrototypeDungeonRoom.RoomCategory.EXIT;
				break;
			}
			break;
		case 'e':
			roomCategory = PrototypeDungeonRoom.RoomCategory.ENTRANCE;
			break;
		case 'h':
			roomCategory = PrototypeDungeonRoom.RoomCategory.HUB;
			break;
		}
		return roomCategory;
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001B2624 File Offset: 0x001B0824
	public DungeonFlowNode GetSubchainRootFromNode(DungeonFlowNode source, LoopFlowBuilder builder)
	{
		List<DungeonFlowNode> list = new List<DungeonFlowNode>();
		for (int i = 0; i < this.m_nodes.Count; i++)
		{
			if (!string.IsNullOrEmpty(this.m_nodes[i].subchainIdentifier))
			{
				if (source.subchainIdentifiers.Contains(this.m_nodes[i].subchainIdentifier))
				{
					if (!this.m_nodes[i].limitedCopiesOfSubchain || !builder.usedSubchainData.ContainsKey(this.m_nodes[i]) || builder.usedSubchainData[this.m_nodes[i]] < this.m_nodes[i].maxCopiesOfSubchain)
					{
						list.Add(this.m_nodes[i]);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[BraveRandom.GenerationRandomRange(0, list.Count)];
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001B272C File Offset: 0x001B092C
	public DungeonFlowNode GetSubchainRootFromNode(DungeonFlowNode source, DungeonFlowBuilder builder)
	{
		List<DungeonFlowNode> list = new List<DungeonFlowNode>();
		for (int i = 0; i < this.m_nodes.Count; i++)
		{
			if (!string.IsNullOrEmpty(this.m_nodes[i].subchainIdentifier))
			{
				if (source.subchainIdentifiers.Contains(this.m_nodes[i].subchainIdentifier))
				{
					if (!this.m_nodes[i].limitedCopiesOfSubchain || !builder.usedSubchainData.ContainsKey(this.m_nodes[i]) || builder.usedSubchainData[this.m_nodes[i]] < this.m_nodes[i].maxCopiesOfSubchain)
					{
						list.Add(this.m_nodes[i]);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[BraveRandom.GenerationRandomRange(0, list.Count)];
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x001B2834 File Offset: 0x001B0A34
	public List<BuilderFlowNode> NewGetNodeChildrenToBuild(BuilderFlowNode parentBuilderNode, LoopFlowBuilder builder)
	{
		DungeonFlowNode node = parentBuilderNode.node;
		List<BuilderFlowNode> list = new List<BuilderFlowNode>();
		for (int i = 0; i < node.childNodeGuids.Count; i++)
		{
			DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(node.childNodeGuids[i]);
			if (nodeFromGuid.nodeType == DungeonFlowNode.ControlNodeType.SELECTOR)
			{
				List<DungeonFlowNode> selectorNodeChildren = this.GetSelectorNodeChildren(nodeFromGuid);
				for (int j = 0; j < selectorNodeChildren.Count; j++)
				{
					list.Add(new BuilderFlowNode(selectorNodeChildren[j]));
				}
			}
			else if (nodeFromGuid.nodeType == DungeonFlowNode.ControlNodeType.SUBCHAIN)
			{
				DungeonFlowNode subchainRootFromNode = this.GetSubchainRootFromNode(nodeFromGuid, builder);
				if (!(subchainRootFromNode == null))
				{
					if (builder.usedSubchainData.ContainsKey(subchainRootFromNode))
					{
						builder.usedSubchainData[subchainRootFromNode] = builder.usedSubchainData[subchainRootFromNode] + 1;
					}
					else
					{
						builder.usedSubchainData.Add(subchainRootFromNode, 1);
					}
					list.Add(new BuilderFlowNode(subchainRootFromNode));
				}
			}
			else if (nodeFromGuid.nodeExpands)
			{
				string text = nodeFromGuid.EvolveChainToCompletion();
				BuilderFlowNode builderFlowNode = null;
				foreach (char c in text)
				{
					PrototypeDungeonRoom.RoomCategory categoryFromChar = this.GetCategoryFromChar(c);
					BuilderFlowNode builderFlowNode2 = new BuilderFlowNode(nodeFromGuid);
					builderFlowNode2.usesOverrideCategory = true;
					builderFlowNode2.overrideCategory = categoryFromChar;
					if (builderFlowNode == null)
					{
						builderFlowNode2.parentBuilderNode = parentBuilderNode;
						list.Add(builderFlowNode2);
					}
					else
					{
						builderFlowNode2.parentBuilderNode = builderFlowNode;
						builderFlowNode.childBuilderNodes = new List<BuilderFlowNode>();
						builderFlowNode.childBuilderNodes.Add(builderFlowNode2);
					}
					builderFlowNode = builderFlowNode2;
				}
			}
			else if (BraveRandom.GenerationRandomValue() <= nodeFromGuid.percentChance)
			{
				list.Add(new BuilderFlowNode(nodeFromGuid));
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			if (list[l].parentBuilderNode == null)
			{
				list[l].parentBuilderNode = parentBuilderNode;
			}
		}
		return list;
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001B2A44 File Offset: 0x001B0C44
	public List<FlowNodeBuildData> GetNodeChildrenToBuild(DungeonFlowNode source, DungeonFlowBuilder builder)
	{
		List<FlowNodeBuildData> list = new List<FlowNodeBuildData>();
		for (int i = 0; i < source.childNodeGuids.Count; i++)
		{
			DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(source.childNodeGuids[i]);
			if (nodeFromGuid.nodeType == DungeonFlowNode.ControlNodeType.SELECTOR)
			{
				List<DungeonFlowNode> selectorNodeChildren = this.GetSelectorNodeChildren(nodeFromGuid);
				for (int j = 0; j < selectorNodeChildren.Count; j++)
				{
					list.Add(new FlowNodeBuildData(selectorNodeChildren[j]));
				}
			}
			else if (nodeFromGuid.nodeType == DungeonFlowNode.ControlNodeType.SUBCHAIN)
			{
				DungeonFlowNode subchainRootFromNode = this.GetSubchainRootFromNode(nodeFromGuid, builder);
				if (!(subchainRootFromNode == null))
				{
					if (builder.usedSubchainData.ContainsKey(subchainRootFromNode))
					{
						builder.usedSubchainData[subchainRootFromNode] = builder.usedSubchainData[subchainRootFromNode] + 1;
					}
					else
					{
						builder.usedSubchainData.Add(subchainRootFromNode, 1);
					}
					list.Add(new FlowNodeBuildData(subchainRootFromNode));
				}
			}
			else if (nodeFromGuid.nodeExpands)
			{
				string text = nodeFromGuid.EvolveChainToCompletion();
				FlowNodeBuildData flowNodeBuildData = null;
				foreach (char c in text)
				{
					PrototypeDungeonRoom.RoomCategory categoryFromChar = this.GetCategoryFromChar(c);
					FlowNodeBuildData flowNodeBuildData2 = new FlowNodeBuildData(nodeFromGuid);
					flowNodeBuildData2.usesOverrideCategory = true;
					flowNodeBuildData2.overrideCategory = categoryFromChar;
					if (flowNodeBuildData == null)
					{
						list.Add(flowNodeBuildData2);
					}
					else
					{
						flowNodeBuildData.childBuildData = new List<FlowNodeBuildData>();
						flowNodeBuildData.childBuildData.Add(flowNodeBuildData2);
					}
					flowNodeBuildData = flowNodeBuildData2;
				}
			}
			else if (BraveRandom.GenerationRandomValue() <= nodeFromGuid.percentChance)
			{
				list.Add(new FlowNodeBuildData(nodeFromGuid));
			}
		}
		int num = -1;
		for (int l = 0; l < list.Count; l++)
		{
			if (this.SubchainContainsRoomOfType(list[l].node, PrototypeDungeonRoom.RoomCategory.EXIT))
			{
				num = l;
				break;
			}
		}
		if (num != -1 && num != 0)
		{
			FlowNodeBuildData flowNodeBuildData3 = list[num];
			list.RemoveAt(num);
			list.Insert(0, flowNodeBuildData3);
		}
		return list;
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x001B2C68 File Offset: 0x001B0E68
	public List<DungeonFlowNode> GetCapChainRootNodes(DungeonFlowBuilder builder)
	{
		List<DungeonFlowNode> list = new List<DungeonFlowNode>();
		for (int i = 0; i < this.m_nodes.Count; i++)
		{
			if (this.m_nodes[i].capSubchain)
			{
				if (!this.m_nodes[i].limitedCopiesOfSubchain || !builder.usedSubchainData.ContainsKey(this.m_nodes[i]) || builder.usedSubchainData[this.m_nodes[i]] < this.m_nodes[i].maxCopiesOfSubchain)
				{
					list.Add(this.m_nodes[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x001B2D24 File Offset: 0x001B0F24
	public bool SubchainContainsRoomOfType(DungeonFlowNode baseNode, PrototypeDungeonRoom.RoomCategory type)
	{
		List<DungeonFlowNode> list = new List<DungeonFlowNode>();
		this.GetNodesRecursive(baseNode, list);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].roomCategory == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001B2D6C File Offset: 0x001B0F6C
	public List<DungeonFlowNode> GetSelectorNodeChildren(DungeonFlowNode source)
	{
		BraveUtility.Assert(source.nodeType != DungeonFlowNode.ControlNodeType.SELECTOR, "Processing selector node on non-selector node.", false);
		int num = BraveRandom.GenerationRandomRange(source.minChildrenToBuild, source.maxChildrenToBuild + 1);
		List<DungeonFlowNode> list = new List<DungeonFlowNode>();
		float num2 = 0f;
		for (int i = 0; i < source.childNodeGuids.Count; i++)
		{
			DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(source.childNodeGuids[i]);
			num2 += nodeFromGuid.percentChance;
		}
		List<string> list2 = new List<string>(source.childNodeGuids);
		for (int j = 0; j < num; j++)
		{
			float num3 = BraveRandom.GenerationRandomValue() * num2;
			float num4 = 0f;
			for (int k = 0; k < list2.Count; k++)
			{
				DungeonFlowNode nodeFromGuid2 = this.GetNodeFromGuid(list2[k]);
				num4 += nodeFromGuid2.percentChance;
				if (num4 >= num3)
				{
					list.Add(nodeFromGuid2);
					if (!source.canBuildDuplicateChildren)
					{
						num2 -= nodeFromGuid2.percentChance;
						list2.RemoveAt(k);
					}
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001B2E8C File Offset: 0x001B108C
	public DungeonFlowNode GetNodeFromGuid(string guid)
	{
		int num = this.m_nodeGuids.IndexOf(guid);
		if (num >= 0)
		{
			return this.m_nodes[num];
		}
		return null;
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001B2EBC File Offset: 0x001B10BC
	public void ConnectNodes(DungeonFlowNode parent, DungeonFlowNode child)
	{
		if (!string.IsNullOrEmpty(parent.parentNodeGuid))
		{
			string text = parent.parentNodeGuid;
			while (!string.IsNullOrEmpty(text))
			{
				if (text == child.guidAsString)
				{
					return;
				}
				DungeonFlowNode nodeFromGuid = this.GetNodeFromGuid(text);
				text = nodeFromGuid.parentNodeGuid;
			}
		}
		if (!string.IsNullOrEmpty(child.parentNodeGuid))
		{
			DungeonFlowNode nodeFromGuid2 = this.GetNodeFromGuid(child.parentNodeGuid);
			nodeFromGuid2.childNodeGuids.Remove(child.guidAsString);
		}
		if (parent.loopTargetNodeGuid == child.guidAsString)
		{
			parent.loopTargetNodeGuid = string.Empty;
		}
		if (child.loopTargetNodeGuid == parent.guidAsString)
		{
			child.loopTargetNodeGuid = string.Empty;
		}
		child.parentNodeGuid = parent.guidAsString;
		parent.childNodeGuids.Add(child.guidAsString);
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001B2FA0 File Offset: 0x001B11A0
	public void LoopConnectNodes(DungeonFlowNode chainEnd, DungeonFlowNode loopTarget)
	{
		if (chainEnd.childNodeGuids.Contains(loopTarget.guidAsString) || loopTarget.childNodeGuids.Contains(chainEnd.guidAsString))
		{
			this.DisconnectNodes(chainEnd, loopTarget);
		}
		if (chainEnd.loopTargetNodeGuid == loopTarget.guidAsString)
		{
			if (chainEnd.loopTargetIsOneWay)
			{
				chainEnd.loopTargetIsOneWay = false;
				chainEnd.loopTargetNodeGuid = string.Empty;
			}
			else
			{
				chainEnd.loopTargetIsOneWay = true;
			}
		}
		else
		{
			chainEnd.loopTargetNodeGuid = loopTarget.guidAsString;
		}
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001B3034 File Offset: 0x001B1234
	public void DisconnectNodes(DungeonFlowNode node1, DungeonFlowNode node2)
	{
		if (node1.childNodeGuids.Contains(node2.guidAsString))
		{
			node1.childNodeGuids.Remove(node2.guidAsString);
			node2.parentNodeGuid = string.Empty;
		}
		else if (node2.childNodeGuids.Contains(node1.guidAsString))
		{
			node2.childNodeGuids.Remove(node1.guidAsString);
			node1.parentNodeGuid = string.Empty;
		}
		if (node1.loopTargetNodeGuid == node2.guidAsString)
		{
			node1.loopTargetNodeGuid = string.Empty;
		}
		else if (node2.loopTargetNodeGuid == node1.guidAsString)
		{
			node2.loopTargetNodeGuid = string.Empty;
		}
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001B30F4 File Offset: 0x001B12F4
	private DungeonFlowNode GetRootOfNode(DungeonFlowNode node)
	{
		DungeonFlowNode dungeonFlowNode = node;
		while (!string.IsNullOrEmpty(dungeonFlowNode.parentNodeGuid))
		{
			dungeonFlowNode = this.GetNodeFromGuid(dungeonFlowNode.parentNodeGuid);
		}
		return dungeonFlowNode;
	}

	// Token: 0x040044F4 RID: 17652
	[SerializeField]
	public GenericRoomTable fallbackRoomTable;

	// Token: 0x040044F5 RID: 17653
	[SerializeField]
	public GenericRoomTable phantomRoomTable;

	// Token: 0x040044F6 RID: 17654
	[SerializeField]
	public List<DungeonFlowSubtypeRestriction> subtypeRestrictions;

	// Token: 0x040044F7 RID: 17655
	[NonSerialized]
	public GenericRoomTable evolvedRoomTable;

	// Token: 0x040044F8 RID: 17656
	[SerializeField]
	private List<DungeonFlowNode> m_nodes;

	// Token: 0x040044F9 RID: 17657
	[SerializeField]
	private List<string> m_nodeGuids;

	// Token: 0x040044FA RID: 17658
	[SerializeField]
	private string m_firstNodeGuid;

	// Token: 0x040044FB RID: 17659
	[SerializeField]
	public List<ProceduralFlowModifierData> flowInjectionData;

	// Token: 0x040044FC RID: 17660
	[SerializeField]
	public List<SharedInjectionData> sharedInjectionData;
}
