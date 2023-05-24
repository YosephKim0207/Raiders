using System;
using UnityEngine;

// Token: 0x020016AC RID: 5804
public class RobotRoomConveyorHorizontalFeature : RobotRoomFeature
{
	// Token: 0x06008750 RID: 34640 RVA: 0x00382160 File Offset: 0x00380360
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return dim.x >= 3 && dim.y >= 3 && idea.UseFloorConveyorBelts;
	}

	// Token: 0x06008751 RID: 34641 RVA: 0x0038218C File Offset: 0x0038038C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		DungeonPlaceableBehaviour horizontalConveyorPrefab = RobotDave.GetHorizontalConveyorPrefab();
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(horizontalConveyorPrefab, room, this.LocalBasePosition, targetObjectLayer);
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData.fieldName = "ConveyorWidth";
		prototypePlacedObjectFieldData.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData.floatValue = (float)this.LocalDimensions.x;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData2 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData2.fieldName = "ConveyorHeight";
		prototypePlacedObjectFieldData2.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData2.floatValue = (float)this.LocalDimensions.y;
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData3 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData3.fieldName = "VelocityX";
		prototypePlacedObjectFieldData3.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData3.floatValue = (float)((UnityEngine.Random.value <= 0.5f) ? (-4) : 4);
		PrototypePlacedObjectFieldData prototypePlacedObjectFieldData4 = new PrototypePlacedObjectFieldData();
		prototypePlacedObjectFieldData4.fieldName = "VelocityY";
		prototypePlacedObjectFieldData4.fieldType = PrototypePlacedObjectFieldData.FieldType.FLOAT;
		prototypePlacedObjectFieldData4.floatValue = 0f;
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData2);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData3);
		prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData4);
	}
}
