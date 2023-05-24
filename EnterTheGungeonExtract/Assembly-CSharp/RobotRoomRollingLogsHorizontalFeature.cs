using System;

// Token: 0x0200169F RID: 5791
public class RobotRoomRollingLogsHorizontalFeature : RobotRoomFeature
{
	// Token: 0x0600871F RID: 34591 RVA: 0x00380F00 File Offset: 0x0037F100
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return idea.UseRollingLogsHorizontal && dim.x > 6 && dim.y > 6;
	}

	// Token: 0x06008720 RID: 34592 RVA: 0x00380F2C File Offset: 0x0037F12C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		DungeonPlaceableBehaviour rollingLogHorizontal = RobotDave.GetRollingLogHorizontal();
		SerializedPath serializedPath = base.GenerateHorizontalPath(this.LocalBasePosition, new IntVector2(this.LocalDimensions.x - (rollingLogHorizontal.GetWidth() - 1), this.LocalDimensions.y));
		room.paths.Add(serializedPath);
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(rollingLogHorizontal, room, this.LocalBasePosition, targetObjectLayer);
		prototypePlacedObjectData.assignedPathIDx = room.paths.Count - 1;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData.fieldName = "NumTiles";
		prototypePlacedObjectFieldData.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData.floatValue = (float)this.LocalDimensions.y;
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData);
	}
}
