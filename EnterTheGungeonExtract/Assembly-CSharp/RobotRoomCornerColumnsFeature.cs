using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A5 RID: 5797
public class RobotRoomCornerColumnsFeature : RobotRoomFeature
{
	// Token: 0x0600873B RID: 34619 RVA: 0x00381664 File Offset: 0x0037F864
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return !Application.isPlaying && numFeatures < 4 && dim.x >= 8 && dim.y >= 8;
	}

	// Token: 0x0600873C RID: 34620 RVA: 0x0038169C File Offset: 0x0037F89C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		int num = 2;
		IntVector2 intVector = this.LocalBasePosition;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				room.ForceGetCellDataAtPoint(intVector.x + i, intVector.y + j).state = CellType.WALL;
			}
		}
		intVector = this.LocalBasePosition + new IntVector2(this.LocalDimensions.x - num, 0);
		for (int k = 0; k < num; k++)
		{
			for (int l = 0; l < num; l++)
			{
				room.ForceGetCellDataAtPoint(intVector.x + k, intVector.y + l).state = CellType.WALL;
			}
		}
		intVector = this.LocalBasePosition + new IntVector2(this.LocalDimensions.x - num, this.LocalDimensions.y - num);
		for (int m = 0; m < num; m++)
		{
			for (int n = 0; n < num; n++)
			{
				room.ForceGetCellDataAtPoint(intVector.x + m, intVector.y + n).state = CellType.WALL;
			}
		}
		intVector = this.LocalBasePosition + new IntVector2(0, this.LocalDimensions.y - num);
		for (int num2 = 0; num2 < num; num2++)
		{
			for (int num3 = 0; num3 < num; num3++)
			{
				room.ForceGetCellDataAtPoint(intVector.x + num2, intVector.y + num3).state = CellType.WALL;
			}
		}
	}
}
