using System;
using Dungeonator;

// Token: 0x020016A3 RID: 5795
public class FlatExpanseFeature : RobotRoomFeature
{
	// Token: 0x06008731 RID: 34609 RVA: 0x003813E4 File Offset: 0x0037F5E4
	public override bool CanContainOtherFeature()
	{
		return true;
	}

	// Token: 0x06008732 RID: 34610 RVA: 0x003813E8 File Offset: 0x0037F5E8
	public override int RequiredInsetForOtherFeature()
	{
		return 2;
	}

	// Token: 0x06008733 RID: 34611 RVA: 0x003813EC File Offset: 0x0037F5EC
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return true;
	}

	// Token: 0x06008734 RID: 34612 RVA: 0x003813F0 File Offset: 0x0037F5F0
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		for (int i = this.LocalBasePosition.x; i < this.LocalBasePosition.x + this.LocalDimensions.x; i++)
		{
			for (int j = this.LocalBasePosition.y; j < this.LocalBasePosition.y + this.LocalDimensions.y; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(i, j);
				prototypeDungeonRoomCellData.state = CellType.FLOOR;
			}
		}
	}
}
