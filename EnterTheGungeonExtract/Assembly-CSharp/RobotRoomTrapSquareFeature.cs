using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016A4 RID: 5796
public class RobotRoomTrapSquareFeature : RobotRoomFeature
{
	// Token: 0x06008736 RID: 34614 RVA: 0x0038147C File Offset: 0x0037F67C
	public override bool CanContainOtherFeature()
	{
		return true;
	}

	// Token: 0x06008737 RID: 34615 RVA: 0x00381480 File Offset: 0x0037F680
	public override int RequiredInsetForOtherFeature()
	{
		return 4;
	}

	// Token: 0x06008738 RID: 34616 RVA: 0x00381484 File Offset: 0x0037F684
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return (idea.UseFloorFlameTraps || idea.UseFloorPitTraps || idea.UseFloorSpikeTraps) && dim.x > 6 && dim.y > 6;
	}

	// Token: 0x06008739 RID: 34617 RVA: 0x003814D4 File Offset: 0x0037F6D4
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
		int num2 = this.LocalBasePosition.x + 2;
		int num3 = this.LocalBasePosition.y + 2;
		int num4 = this.LocalBasePosition.x + this.LocalDimensions.x - 2;
		int num5 = this.LocalBasePosition.y + this.LocalDimensions.y - 2;
		if ((num4 - num2) % width != 0)
		{
			num4--;
		}
		if ((num5 - num3) % width != 0)
		{
			num5--;
		}
		for (int i = num2; i < num4; i += width)
		{
			for (int j = num3; j < num5; j += width)
			{
				if (i == num2 || i == num4 - width || j == num3 || j == num5 - width)
				{
					IntVector2 intVector = new IntVector2(i, j);
					base.PlaceObject(dungeonPlaceableBehaviour, room, intVector, targetObjectLayer);
				}
			}
		}
	}
}
