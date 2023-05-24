using System;
using UnityEngine;

// Token: 0x020016AD RID: 5805
public class RobotRoomConveyorVerticalFeature : RobotRoomFeature
{
	// Token: 0x06008753 RID: 34643 RVA: 0x003822A0 File Offset: 0x003804A0
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x >= 3 && dim.y >= 3 && idea.UseFloorConveyorBelts;
	}

	// Token: 0x06008754 RID: 34644 RVA: 0x003822CC File Offset: 0x003804CC
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		DungeonPlaceableBehaviour verticalConveyorPrefab = RobotDave.GetVerticalConveyorPrefab();
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(verticalConveyorPrefab, room, this.LocalBasePosition, targetObjectLayer);
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData.fieldName = "ConveyorWidth";
		prototypePlacedObjectFieldData.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData.floatValue = (float)this.LocalDimensions.x;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData2 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData2.fieldName = "ConveyorHeight";
		prototypePlacedObjectFieldData2.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData2.floatValue = (float)this.LocalDimensions.y;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData3 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData3.fieldName = "VelocityY";
		prototypePlacedObjectFieldData3.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData3.floatValue = (float)((UnityEngine.Random.value <= 0.5f) ? (-4) : 4);
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData4 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData4.fieldName = "VelocityX";
		prototypePlacedObjectFieldData4.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData4.floatValue = 0f;
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData2);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData3);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData4);
	}
}
