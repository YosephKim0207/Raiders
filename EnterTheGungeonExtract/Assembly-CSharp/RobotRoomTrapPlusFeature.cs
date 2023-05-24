using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016A6 RID: 5798
public class RobotRoomTrapPlusFeature : RobotRoomFeature
{
	// Token: 0x0600873E RID: 34622 RVA: 0x0038184C File Offset: 0x0037FA4C
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return (idea.UseFloorFlameTraps || idea.UseFloorPitTraps || idea.UseFloorSpikeTraps) && numFeatures == 1;
	}

	// Token: 0x0600873F RID: 34623 RVA: 0x0038187C File Offset: 0x0037FA7C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		List<int> list = new List<int>();
		if (idea.UseFloorPitTraps)
		{
			list.Add(0);
		}
		if (idea.UseFloorSpikeTraps)
		{
			list.Add(1);
		}
		if (idea.UseFloorFlameTraps)
		{
			list.Add(2);
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		DungeonPlaceableBehaviour dungeonPlaceableBehaviour = null;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					dungeonPlaceableBehaviour = RobotDave.GetFloorFlameTrap();
				}
			}
			else
			{
				dungeonPlaceableBehaviour = RobotDave.GetSpikesTrap();
			}
		}
		else
		{
			dungeonPlaceableBehaviour = RobotDave.GetPitTrap();
		}
		int width = dungeonPlaceableBehaviour.GetWidth();
		if (this.LocalDimensions.x % width == 0)
		{
			int num2 = this.LocalBasePosition.y + Mathf.FloorToInt((float)this.LocalDimensions.y / 2f) - (width - 1);
			for (int i = 0; i < this.LocalDimensions.x; i += width)
			{
				base.PlaceObject(dungeonPlaceableBehaviour, room, new IntVector2(this.LocalBasePosition.x + i, num2), targetObjectLayer);
			}
		}
		if (this.LocalDimensions.y % width == 0)
		{
			int num3 = this.LocalBasePosition.x + Mathf.FloorToInt((float)this.LocalDimensions.x / 2f) - (width - 1);
			for (int j = 0; j < this.LocalDimensions.y; j += width)
			{
				base.PlaceObject(dungeonPlaceableBehaviour, room, new IntVector2(num3, this.LocalBasePosition.y + j), targetObjectLayer);
			}
		}
	}
}
