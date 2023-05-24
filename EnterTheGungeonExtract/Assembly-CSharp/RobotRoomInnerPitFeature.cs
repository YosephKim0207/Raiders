using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A1 RID: 5793
public class RobotRoomInnerPitFeature : RobotRoomFeature
{
	// Token: 0x06008727 RID: 34599 RVA: 0x003811A0 File Offset: 0x0037F3A0
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x > 7 && dim.y > 7 && idea.CanIncludePits;
	}

	// Token: 0x06008728 RID: 34600 RVA: 0x003811C4 File Offset: 0x0037F3C4
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		int num = UnityEngine.Random.Range(4, this.LocalDimensions.x / 2);
		int num2 = UnityEngine.Random.Range(4, this.LocalDimensions.y / 2);
		int num3 = (this.LocalDimensions.x - num) / 2 + this.LocalBasePosition.x;
		int num4 = (this.LocalDimensions.y - num2) / 2 + this.LocalBasePosition.y;
		for (int i = num3; i < num3 + num; i++)
		{
			for (int j = num4; j < num4 + num2; j++)
			{
				room.ForceGetCellDataAtPoint(i, j).state = CellType.PIT;
			}
		}
		room.RedefineAllPitEntries();
	}
}
