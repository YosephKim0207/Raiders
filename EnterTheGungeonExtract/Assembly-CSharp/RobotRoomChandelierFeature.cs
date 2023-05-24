using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020016A7 RID: 5799
public class RobotRoomChandelierFeature : RobotRoomFeature
{
	// Token: 0x06008741 RID: 34625 RVA: 0x00381A18 File Offset: 0x0037FC18
	public override bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures)
	{
		return idea.UseChandeliers && dim.x >= 5 && dim.y >= 5 && !isInternal;
	}

	// Token: 0x06008742 RID: 34626 RVA: 0x00381A4C File Offset: 0x0037FC4C
	public override void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer)
	{
		int num = this.LocalDimensions.x / 2 - 1;
		int num2 = this.LocalDimensions.y / 2 - 1;
		IntVector2 intVector = this.LocalBasePosition + new IntVector2(num, num2);
		DungeonPlaceableBehaviour chandelierPrefab = RobotDave.GetChandelierPrefab();
		PrototypePlacedObjectData prototypePlacedObjectData = base.PlaceObject(chandelierPrefab, room, intVector, targetObjectLayer);
		IntVector2 intVector2 = new IntVector2(UnityEngine.Random.Range(0, this.LocalDimensions.x), this.LocalDimensions.y - 1);
		IntVector2 intVector3 = this.LocalBasePosition + intVector2;
		PrototypeEventTriggerArea prototypeEventTriggerArea = room.AddEventTriggerArea(new List<IntVector2> { intVector3 });
		int num3 = room.eventTriggerAreas.IndexOf(prototypeEventTriggerArea);
		prototypePlacedObjectData.linkedTriggerAreaIDs = new List<int> { num3 };
	}
}
