using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016AB RID: 5803
public class RobotRoomMineCartSquareFeature : RobotRoomFeature
{
	// Token: 0x0600874D RID: 34637 RVA: 0x00382004 File Offset: 0x00380204
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x >= 8 && dim.y >= 8 && idea.UseMineCarts;
	}

	// Token: 0x0600874E RID: 34638 RVA: 0x00382030 File Offset: 0x00380230
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
		int num = UnityEngine.Random.Range(1, this.LocalDimensions.x / 2 - 2);
		int num2 = UnityEngine.Random.Range(1, this.LocalDimensions.y / 2 - 2);
		IntVector2 intVector = this.LocalBasePosition + new IntVector2(num, num2);
		IntVector2 intVector2 = this.LocalDimensions - new IntVector2(2 * num, 2 * num2);
		SerializedPath serializedPath = base.GenerateRectanglePath(intVector, intVector2);
		serializedPath.tilesetPathGrid = 0;
		room.paths.Add(serializedPath);
		DungeonPlaceableBehaviour mineCartPrefab = RobotDave.GetMineCartPrefab();
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(mineCartPrefab, room, intVector, targetObjectLayer);
		prototypePlacedObjectData.assignedPathIDx = room.paths.Count - 1;
	}
}
