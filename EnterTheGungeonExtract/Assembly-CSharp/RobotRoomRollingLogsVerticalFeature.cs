using System;

// Token: 0x0200169E RID: 5790
public class RobotRoomRollingLogsVerticalFeature : RobotRoomFeature
{
	// Token: 0x0600871C RID: 34588 RVA: 0x00380E20 File Offset: 0x0037F020
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return idea.UseRollingLogsVertical && dim.x > 6 && dim.y > 6;
	}

	// Token: 0x0600871D RID: 34589 RVA: 0x00380E4C File Offset: 0x0037F04C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		DungeonPlaceableBehaviour rollingLogVertical = RobotDave.GetRollingLogVertical();
		SerializedPath serializedPath = base.GenerateVerticalPath(this.LocalBasePosition, new IntVector2(this.LocalDimensions.x, this.LocalDimensions.y - (rollingLogVertical.GetHeight() - 1)));
		room.paths.Add(serializedPath);
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(rollingLogVertical, room, this.LocalBasePosition, targetObjectLayer);
		prototypePlacedObjectData.assignedPathIDx = room.paths.Count - 1;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData.fieldName = "NumTiles";
		prototypePlacedObjectFieldData.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData.floatValue = (float)this.LocalDimensions.x;
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData);
	}
}
