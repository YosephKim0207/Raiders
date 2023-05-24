using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000F04 RID: 3844
	public class BuilderFlowNode : ArbitraryFlowBuildData
	{
		// Token: 0x060051FB RID: 20987 RVA: 0x001D3F78 File Offset: 0x001D2178
		public BuilderFlowNode(DungeonFlowNode n)
		{
			this.node = n;
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060051FC RID: 20988 RVA: 0x001D3FA4 File Offset: 0x001D21A4
		public int Connectivity
		{
			get
			{
				int num = 0;
				if (this.parentBuilderNode != null)
				{
					num++;
				}
				if (this.loopConnectedBuilderNode != null)
				{
					num++;
				}
				return num + this.childBuilderNodes.Count;
			}
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x001D3FE0 File Offset: 0x001D21E0
		public void ClearData()
		{
			this.exitToNodeMap.Clear();
			this.nodeToExitMap.Clear();
			this.IsInjectedNode = false;
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060051FE RID: 20990 RVA: 0x001D4000 File Offset: 0x001D2200
		public PrototypeDungeonRoom.RoomCategory Category
		{
			get
			{
				if (this.usesOverrideCategory)
				{
					return this.overrideCategory;
				}
				if (this.assignedPrototypeRoom != null)
				{
					return this.assignedPrototypeRoom.category;
				}
				return this.node.roomCategory;
			}
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x001D403C File Offset: 0x001D223C
		public bool IsOfDepth(int depth)
		{
			BuilderFlowNode builderFlowNode = this;
			for (int i = 0; i < depth; i++)
			{
				if (builderFlowNode.parentBuilderNode == null)
				{
					return false;
				}
				builderFlowNode = builderFlowNode.parentBuilderNode;
			}
			return true;
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x001D4074 File Offset: 0x001D2274
		public bool IsStandardCategory
		{
			get
			{
				return (this.Category == PrototypeDungeonRoom.RoomCategory.NORMAL || this.Category == PrototypeDungeonRoom.RoomCategory.CONNECTOR || this.Category == PrototypeDungeonRoom.RoomCategory.HUB) && (this.Category != PrototypeDungeonRoom.RoomCategory.NORMAL || !(this.assignedPrototypeRoom != null) || this.assignedPrototypeRoom.subCategoryNormal != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP);
			}
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x001D40D8 File Offset: 0x001D22D8
		public List<BuilderFlowNode> GetAllConnectedNodes(List<BuilderFlowNode> excluded)
		{
			List<BuilderFlowNode> list = new List<BuilderFlowNode>(this.childBuilderNodes);
			if (this.parentBuilderNode != null)
			{
				list.Add(this.parentBuilderNode);
			}
			if (this.loopConnectedBuilderNode != null)
			{
				list.Add(this.loopConnectedBuilderNode);
			}
			for (int i = 0; i < excluded.Count; i++)
			{
				list.Remove(excluded[i]);
			}
			return list;
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001D4148 File Offset: 0x001D2348
		public List<BuilderFlowNode> GetAllConnectedNodes(params BuilderFlowNode[] excluded)
		{
			List<BuilderFlowNode> list = new List<BuilderFlowNode>(this.childBuilderNodes);
			if (this.parentBuilderNode != null)
			{
				list.Add(this.parentBuilderNode);
			}
			if (this.loopConnectedBuilderNode != null)
			{
				list.Add(this.loopConnectedBuilderNode);
			}
			for (int i = 0; i < excluded.Length; i++)
			{
				list.Remove(excluded[i]);
			}
			return list;
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x001D41B0 File Offset: 0x001D23B0
		public void MakeNodeTreeRoot()
		{
			if (this.parentBuilderNode != null)
			{
				BuilderFlowNode builderFlowNode = null;
				BuilderFlowNode builderFlowNode2 = this;
				BuilderFlowNode builderFlowNode4;
				for (BuilderFlowNode builderFlowNode3 = this.parentBuilderNode; builderFlowNode3 != null; builderFlowNode3 = builderFlowNode4)
				{
					builderFlowNode4 = builderFlowNode3.parentBuilderNode;
					builderFlowNode2.parentBuilderNode = builderFlowNode;
					builderFlowNode2.childBuilderNodes.Add(builderFlowNode3);
					builderFlowNode3.parentBuilderNode = builderFlowNode2;
					builderFlowNode3.childBuilderNodes.Remove(builderFlowNode2);
					builderFlowNode = builderFlowNode2;
					builderFlowNode2 = builderFlowNode3;
				}
			}
		}

		// Token: 0x04004A27 RID: 18983
		public int identifier = -1;

		// Token: 0x04004A28 RID: 18984
		public DungeonFlowNode node;

		// Token: 0x04004A29 RID: 18985
		public bool AcquiresRoomAsNecessary;

		// Token: 0x04004A2A RID: 18986
		public PrototypeDungeonRoom assignedPrototypeRoom;

		// Token: 0x04004A2B RID: 18987
		public RoomHandler instanceRoom;

		// Token: 0x04004A2C RID: 18988
		public bool usesOverrideCategory;

		// Token: 0x04004A2D RID: 18989
		public PrototypeDungeonRoom.RoomCategory overrideCategory;

		// Token: 0x04004A2E RID: 18990
		public BuilderFlowNode parentBuilderNode;

		// Token: 0x04004A2F RID: 18991
		public List<BuilderFlowNode> childBuilderNodes;

		// Token: 0x04004A30 RID: 18992
		public BuilderFlowNode loopConnectedBuilderNode;

		// Token: 0x04004A31 RID: 18993
		public Dictionary<PrototypeRoomExit, BuilderFlowNode> exitToNodeMap = new Dictionary<PrototypeRoomExit, BuilderFlowNode>();

		// Token: 0x04004A32 RID: 18994
		public Dictionary<BuilderFlowNode, PrototypeRoomExit> nodeToExitMap = new Dictionary<BuilderFlowNode, PrototypeRoomExit>();

		// Token: 0x04004A33 RID: 18995
		public BuilderFlowNode InjectionTarget;

		// Token: 0x04004A34 RID: 18996
		public List<BuilderFlowNode> InjectionFrameSequence;

		// Token: 0x04004A35 RID: 18997
		public bool IsInjectedNode;
	}
}
