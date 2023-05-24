using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000EDF RID: 3807
	public class FlowNodeBuildData
	{
		// Token: 0x06005119 RID: 20761 RVA: 0x001CB050 File Offset: 0x001C9250
		public FlowNodeBuildData(DungeonFlowNode n)
		{
			this.node = n;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001CB068 File Offset: 0x001C9268
		public void MarkExits()
		{
			this.room.area.instanceUsedExits.Add(this.roomEntrance);
			this.sourceRoom.area.instanceUsedExits.Add(this.sourceExit);
			this.room.parentRoom = this.sourceRoom;
			this.room.connectedRooms.Add(this.sourceRoom);
			this.room.connectedRoomsByExit.Add(this.roomEntrance, this.sourceRoom);
			this.sourceRoom.childRooms.Add(this.room);
			this.sourceRoom.connectedRooms.Add(this.room);
			this.sourceRoom.connectedRoomsByExit.Add(this.sourceExit, this.room);
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001CB138 File Offset: 0x001C9338
		public void UnmarkExits()
		{
			this.room.area.instanceUsedExits.Remove(this.roomEntrance);
			this.sourceRoom.area.instanceUsedExits.Remove(this.sourceExit);
			this.room.parentRoom = null;
			this.room.connectedRooms.Remove(this.sourceRoom);
			this.room.connectedRoomsByExit.Remove(this.roomEntrance);
			this.sourceRoom.childRooms.Remove(this.room);
			this.sourceRoom.connectedRooms.Remove(this.room);
			this.sourceRoom.connectedRoomsByExit.Remove(this.sourceExit);
		}

		// Token: 0x0400491F RID: 18719
		public DungeonFlowNode node;

		// Token: 0x04004920 RID: 18720
		public bool usesOverrideCategory;

		// Token: 0x04004921 RID: 18721
		public PrototypeDungeonRoom.RoomCategory overrideCategory;

		// Token: 0x04004922 RID: 18722
		public RoomHandler room;

		// Token: 0x04004923 RID: 18723
		public bool unbuilt = true;

		// Token: 0x04004924 RID: 18724
		public PrototypeRoomExit sourceExit;

		// Token: 0x04004925 RID: 18725
		public PrototypeRoomExit roomEntrance;

		// Token: 0x04004926 RID: 18726
		public RoomHandler sourceRoom;

		// Token: 0x04004927 RID: 18727
		public List<FlowNodeBuildData> childBuildData;
	}
}
