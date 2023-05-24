using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A2 RID: 5794
public class RobotRoomSurroundingPitFeature : RobotRoomFeature
{
	// Token: 0x0600872A RID: 34602 RVA: 0x00381280 File Offset: 0x0037F480
	public override void Use()
	{
		RobotRoomSurroundingPitFeature.BeenUsed = true;
		base.Use();
	}

	// Token: 0x0600872B RID: 34603 RVA: 0x00381290 File Offset: 0x0037F490
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return !Application.isPlaying && !RobotRoomSurroundingPitFeature.BeenUsed && !isInternal && idea.CanIncludePits;
	}

	// Token: 0x0600872C RID: 34604 RVA: 0x003812B8 File Offset: 0x0037F4B8
	public override bool CanContainOtherFeature()
	{
		return true;
	}

	// Token: 0x0600872D RID: 34605 RVA: 0x003812BC File Offset: 0x0037F4BC
	public override int RequiredInsetForOtherFeature()
	{
		return 2;
	}

	// Token: 0x0600872E RID: 34606 RVA: 0x003812C0 File Offset: 0x0037F4C0
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		for (int i = this.LocalBasePosition.x; i < this.LocalBasePosition.x + this.LocalDimensions.x; i++)
		{
			for (int j = this.LocalBasePosition.y; j < this.LocalBasePosition.y + this.LocalDimensions.y; j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(i, j);
				prototypeDungeonRoomCellData.state = CellType.PIT;
			}
		}
		room.RedefineAllPitEntries();
		int num = this.RequiredInsetForOtherFeature();
		for (int k = this.LocalBasePosition.x + num; k < this.LocalBasePosition.x + this.LocalDimensions.x - num; k++)
		{
			for (int l = this.LocalBasePosition.y + num; l < this.LocalBasePosition.y + this.LocalDimensions.y - num; l++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = room.ForceGetCellDataAtPoint(k, l);
				prototypeDungeonRoomCellData2.state = CellType.FLOOR;
			}
		}
	}

	// Token: 0x04008C41 RID: 35905
	public static bool BeenUsed;
}
