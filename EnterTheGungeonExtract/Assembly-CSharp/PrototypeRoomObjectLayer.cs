using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F47 RID: 3911
[Serializable]
public class PrototypeRoomObjectLayer
{
	// Token: 0x06005438 RID: 21560 RVA: 0x001F832C File Offset: 0x001F652C
	public static PrototypeRoomObjectLayer CreateMirror(PrototypeRoomObjectLayer source, IntVector2 roomDimensions)
	{
		PrototypeRoomObjectLayer prototypeRoomObjectLayer = new PrototypeRoomObjectLayer();
		prototypeRoomObjectLayer.placedObjects = new List<PrototypePlacedObjectData>();
		for (int i = 0; i < source.placedObjects.Count; i++)
		{
			prototypeRoomObjectLayer.placedObjects.Add(source.placedObjects[i].CreateMirror(roomDimensions));
		}
		prototypeRoomObjectLayer.placedObjectBasePositions = new List<Vector2>();
		for (int j = 0; j < source.placedObjectBasePositions.Count; j++)
		{
			Vector2 vector = source.placedObjectBasePositions[j];
			vector.x = (float)roomDimensions.x - (vector.x + (float)prototypeRoomObjectLayer.placedObjects[j].GetWidth(true));
			prototypeRoomObjectLayer.placedObjectBasePositions.Add(vector);
		}
		prototypeRoomObjectLayer.layerIsReinforcementLayer = source.layerIsReinforcementLayer;
		prototypeRoomObjectLayer.shuffle = source.shuffle;
		prototypeRoomObjectLayer.randomize = source.randomize;
		prototypeRoomObjectLayer.suppressPlayerChecks = source.suppressPlayerChecks;
		prototypeRoomObjectLayer.delayTime = source.delayTime;
		prototypeRoomObjectLayer.reinforcementTriggerCondition = source.reinforcementTriggerCondition;
		prototypeRoomObjectLayer.probability = source.probability;
		prototypeRoomObjectLayer.numberTimesEncounteredRequired = source.numberTimesEncounteredRequired;
		return prototypeRoomObjectLayer;
	}

	// Token: 0x04004CEC RID: 19692
	public List<PrototypePlacedObjectData> placedObjects = new List<PrototypePlacedObjectData>();

	// Token: 0x04004CED RID: 19693
	public List<Vector2> placedObjectBasePositions = new List<Vector2>();

	// Token: 0x04004CEE RID: 19694
	public bool layerIsReinforcementLayer;

	// Token: 0x04004CEF RID: 19695
	public bool shuffle = true;

	// Token: 0x04004CF0 RID: 19696
	public int randomize = 2;

	// Token: 0x04004CF1 RID: 19697
	public bool suppressPlayerChecks;

	// Token: 0x04004CF2 RID: 19698
	public float delayTime = 15f;

	// Token: 0x04004CF3 RID: 19699
	public RoomEventTriggerCondition reinforcementTriggerCondition = RoomEventTriggerCondition.ON_ENEMIES_CLEARED;

	// Token: 0x04004CF4 RID: 19700
	public float probability = 1f;

	// Token: 0x04004CF5 RID: 19701
	public int numberTimesEncounteredRequired;
}
