using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A8 RID: 5800
public class RobotRoomCaveInFeature : RobotRoomFeature
{
	// Token: 0x06008744 RID: 34628 RVA: 0x00381B1C File Offset: 0x0037FD1C
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return idea.UseCaveIns && dim.x >= 5 && dim.y >= 5;
	}

	// Token: 0x06008745 RID: 34629 RVA: 0x00381B48 File Offset: 0x0037FD48
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		int num = this.LocalDimensions.x / 2 - 1;
		int num2 = this.LocalDimensions.y / 2 - 1;
		IntVector2 intVector = this.LocalBasePosition + new IntVector2(num, num2);
		DungeonPlaceableBehaviour caveInPrefab = RobotDave.GetCaveInPrefab();
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(caveInPrefab, room, intVector, targetObjectLayer);
		IntVector2[] array = new IntVector2[]
		{
			new IntVector2(1, 1),
			new IntVector2(num, 1),
			new IntVector2(this.LocalDimensions.x - 2, 1),
			new IntVector2(1, num2),
			new IntVector2(this.LocalDimensions.x - 2, num2),
			new IntVector2(1, this.LocalDimensions.y - 2),
			new IntVector2(num, this.LocalDimensions.y - 2),
			new IntVector2(this.LocalDimensions.x - 2, this.LocalDimensions.y - 2)
		};
		IntVector2 intVector2 = this.LocalBasePosition + array[UnityEngine.Random.Range(0, 8)];
		PrototypeEventTriggerArea prototypeEventTriggerArea = room.AddEventTriggerArea(new List<IntVector2> { intVector2 });
		int num3 = room.eventTriggerAreas.IndexOf(prototypeEventTriggerArea);
		prototypePlacedObjectData.linkedTriggerAreaIDs = new List<int> { num3 };
	}
}
