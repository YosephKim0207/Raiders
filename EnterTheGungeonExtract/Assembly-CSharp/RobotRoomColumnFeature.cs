using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A9 RID: 5801
public class RobotRoomColumnFeature : RobotRoomFeature
{
	// Token: 0x06008747 RID: 34631 RVA: 0x00381CEC File Offset: 0x0037FEEC
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x >= 8 && dim.y >= 8;
	}

	// Token: 0x06008748 RID: 34632 RVA: 0x00381D0C File Offset: 0x0037FF0C
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
		int num = UnityEngine.Random.Range(3, this.LocalDimensions.x / 2 - 1);
		int num2 = UnityEngine.Random.Range(3, this.LocalDimensions.y / 2 - 1);
		IntVector2 intVector = this.LocalBasePosition + new IntVector2(num, num2);
		IntVector2 intVector2 = this.LocalDimensions - new IntVector2(2 * num, 2 * num2);
		for (int k = intVector.x; k < intVector.x + intVector2.x; k++)
		{
			for (int l = intVector.y; l < intVector.y + intVector2.y; l++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = room.ForceGetCellDataAtPoint(k, l);
				prototypeDungeonRoomCellData2.state = CellType.WALL;
			}
		}
		if (idea.UseWallSawblades)
		{
			SerializedPath serializedPath = base.GenerateRectanglePathInset(intVector, intVector2);
			room.paths.Add(serializedPath);
			DungeonPlaceableBehaviour sawbladePrefab = RobotDave.GetSawbladePrefab();
			PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(sawbladePrefab, room, intVector, targetObjectLayer);
			prototypePlacedObjectData.assignedPathIDx = room.paths.Count - 1;
		}
	}
}
