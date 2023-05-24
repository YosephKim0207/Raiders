using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A0 RID: 5792
public class RobotRoomTablesFeature : RobotRoomFeature
{
	// Token: 0x06008722 RID: 34594 RVA: 0x00380FE0 File Offset: 0x0037F1E0
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x >= 7 || dim.y >= 7;
	}

	// Token: 0x06008723 RID: 34595 RVA: 0x00381000 File Offset: 0x0037F200
	public override bool CanContainOtherFeature()
	{
		return true;
	}

	// Token: 0x06008724 RID: 34596 RVA: 0x00381004 File Offset: 0x0037F204
	public override int RequiredInsetForOtherFeature()
	{
		return 5;
	}

	// Token: 0x06008725 RID: 34597 RVA: 0x00381008 File Offset: 0x0037F208
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		bool flag = this.LocalDimensions.x < this.LocalDimensions.y;
		if (Mathf.Abs(1f - (float)this.LocalDimensions.x / ((float)this.LocalDimensions.y * 1f)) < 0.25f)
		{
			flag = UnityEngine.Random.value < 0.5f;
		}
		if (flag)
		{
			DungeonPlaceable horizontalTable = RobotDave.GetHorizontalTable();
			for (int i = this.LocalBasePosition.x + 3; i < this.LocalBasePosition.x + this.LocalDimensions.x - 3; i += 4)
			{
				IntVector2 intVector = new IntVector2(i, this.LocalBasePosition.y + 3);
				IntVector2 intVector2 = new IntVector2(i, this.LocalBasePosition.y + this.LocalDimensions.y - 4);
				base.PlaceObject(horizontalTable, room, intVector, targetObjectLayer);
				base.PlaceObject(horizontalTable, room, intVector2, targetObjectLayer);
			}
		}
		else
		{
			DungeonPlaceable verticalTable = RobotDave.GetVerticalTable();
			for (int j = this.LocalBasePosition.y + 3; j < this.LocalBasePosition.y + this.LocalDimensions.y - 3; j += 4)
			{
				IntVector2 intVector3 = new IntVector2(this.LocalBasePosition.x + 3, j);
				IntVector2 intVector4 = new IntVector2(this.LocalBasePosition.x + this.LocalDimensions.x - 4, j);
				base.PlaceObject(verticalTable, room, intVector3, targetObjectLayer);
				base.PlaceObject(verticalTable, room, intVector4, targetObjectLayer);
			}
		}
	}
}
