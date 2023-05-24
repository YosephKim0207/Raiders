using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200169D RID: 5789
public abstract class RobotRoomFeature
{
	// Token: 0x06008710 RID: 34576 RVA: 0x003808D4 File Offset: 0x0037EAD4
	public virtual void Use()
	{
	}

	// Token: 0x06008711 RID: 34577 RVA: 0x003808D8 File Offset: 0x0037EAD8
	protected SerializedPath GenerateVerticalPath(IntVector2 BasePosition, IntVector2 Dimensions)
	{
		SerializedPath serializedPath = new SerializedPath(BasePosition + new IntVector2(0, Dimensions.y - 1));
		serializedPath.AddPosition(BasePosition);
		serializedPath.wrapMode = SerializedPath.SerializedPathWrapMode.PingPong;
		SerializedPathNode serializedPathNode = serializedPath.nodes[0];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[0] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[1];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[1] = serializedPathNode;
		return serializedPath;
	}

	// Token: 0x06008712 RID: 34578 RVA: 0x00380954 File Offset: 0x0037EB54
	protected SerializedPath GenerateHorizontalPath(IntVector2 BasePosition, IntVector2 Dimensions)
	{
		SerializedPath serializedPath = new SerializedPath(BasePosition + new IntVector2(Dimensions.x - 1, 0));
		serializedPath.AddPosition(BasePosition);
		serializedPath.wrapMode = SerializedPath.SerializedPathWrapMode.PingPong;
		SerializedPathNode serializedPathNode = serializedPath.nodes[0];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[0] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[1];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[1] = serializedPathNode;
		return serializedPath;
	}

	// Token: 0x06008713 RID: 34579 RVA: 0x003809D0 File Offset: 0x0037EBD0
	protected SerializedPath GenerateRectanglePath(IntVector2 BasePosition, IntVector2 Dimensions)
	{
		SerializedPath serializedPath = new SerializedPath(BasePosition);
		serializedPath.AddPosition(BasePosition + Dimensions.WithY(0));
		serializedPath.AddPosition(BasePosition + Dimensions);
		serializedPath.AddPosition(BasePosition + Dimensions.WithX(0));
		serializedPath.wrapMode = SerializedPath.SerializedPathWrapMode.Loop;
		SerializedPathNode serializedPathNode = serializedPath.nodes[0];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[0] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[1];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[1] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[2];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[2] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[3];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[3] = serializedPathNode;
		return serializedPath;
	}

	// Token: 0x06008714 RID: 34580 RVA: 0x00380AAC File Offset: 0x0037ECAC
	protected SerializedPath GenerateRectanglePathInset(IntVector2 BasePosition, IntVector2 Dimensions)
	{
		BasePosition += new IntVector2(-1, 0);
		Dimensions += IntVector2.One;
		SerializedPath serializedPath = new SerializedPath(BasePosition);
		serializedPath.AddPosition(BasePosition + Dimensions.WithY(0));
		serializedPath.AddPosition(BasePosition + Dimensions);
		serializedPath.AddPosition(BasePosition + Dimensions.WithX(0));
		serializedPath.wrapMode = SerializedPath.SerializedPathWrapMode.Loop;
		SerializedPathNode serializedPathNode = serializedPath.nodes[0];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.NorthEast;
		serializedPath.nodes[0] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[1];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.NorthWest;
		serializedPath.nodes[1] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[2];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		serializedPath.nodes[2] = serializedPathNode;
		serializedPathNode = serializedPath.nodes[3];
		serializedPathNode.placement = SerializedPathNode.SerializedNodePlacement.SouthEast;
		serializedPath.nodes[3] = serializedPathNode;
		return serializedPath;
	}

	// Token: 0x06008715 RID: 34581
	public abstract bool AcceptableInIdea(RobotDaveIdea idea, IntVector2 dim, bool isInternal, int numFeatures);

	// Token: 0x06008716 RID: 34582 RVA: 0x00380BA4 File Offset: 0x0037EDA4
	public virtual bool CanContainOtherFeature()
	{
		return false;
	}

	// Token: 0x06008717 RID: 34583 RVA: 0x00380BA8 File Offset: 0x0037EDA8
	public virtual int RequiredInsetForOtherFeature()
	{
		return 0;
	}

	// Token: 0x06008718 RID: 34584 RVA: 0x00380BAC File Offset: 0x0037EDAC
	protected PrototypePlacedObjectData PlaceObject(DungeonPlaceable item, PrototypeDungeonRoom room, IntVector2 position, int targetObjectLayer)
	{
		if (room.CheckRegionOccupied(position.x, position.y, item.GetWidth(), item.GetHeight()))
		{
			return null;
		}
		Vector2 vector = position.ToVector2();
		PrototypePlacedObjectData prototypePlacedObjectData = new PrototypePlacedObjectData();
		prototypePlacedObjectData.fieldData = new List<PrototypePlacedObjectFieldData>();
		prototypePlacedObjectData.instancePrerequisites = new DungeonPrerequisite[0];
		prototypePlacedObjectData.placeableContents = item;
		prototypePlacedObjectData.contentsBasePosition = vector;
		int count = room.placedObjects.Count;
		room.placedObjects.Add(prototypePlacedObjectData);
		room.placedObjectPositions.Add(vector);
		for (int i = 0; i < item.GetWidth(); i++)
		{
			for (int j = 0; j < item.GetHeight(); j++)
			{
				PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(position.x + i, position.y + j);
				prototypeDungeonRoomCellData.placedObjectRUBELIndex = count;
			}
		}
		return prototypePlacedObjectData;
	}

	// Token: 0x06008719 RID: 34585 RVA: 0x00380C90 File Offset: 0x0037EE90
	protected PrototypePlacedObjectData PlaceObject(DungeonPlaceableBehaviour item, PrototypeDungeonRoom room, IntVector2 position, int targetObjectLayer)
	{
		if (room.CheckRegionOccupied(position.x, position.y, item.GetWidth(), item.GetHeight()))
		{
			return null;
		}
		Vector2 vector = position.ToVector2();
		PrototypePlacedObjectData prototypePlacedObjectData = new PrototypePlacedObjectData();
		prototypePlacedObjectData.fieldData = new List<PrototypePlacedObjectFieldData>();
		prototypePlacedObjectData.instancePrerequisites = new DungeonPrerequisite[0];
		prototypePlacedObjectData.nonenemyBehaviour = item;
		prototypePlacedObjectData.contentsBasePosition = vector;
		if (targetObjectLayer == -1)
		{
			int count = room.placedObjects.Count;
			room.placedObjects.Add(prototypePlacedObjectData);
			room.placedObjectPositions.Add(vector);
			for (int i = 0; i < item.GetWidth(); i++)
			{
				for (int j = 0; j < item.GetHeight(); j++)
				{
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = room.ForceGetCellDataAtPoint(position.x + i, position.y + j);
					prototypeDungeonRoomCellData.placedObjectRUBELIndex = count;
				}
			}
		}
		else
		{
			PrototypeRoomObjectLayer prototypeRoomObjectLayer = room.additionalObjectLayers[targetObjectLayer];
			int count2 = prototypeRoomObjectLayer.placedObjects.Count;
			prototypeRoomObjectLayer.placedObjects.Add(prototypePlacedObjectData);
			prototypeRoomObjectLayer.placedObjectBasePositions.Add(vector);
			for (int k = 0; k < item.GetWidth(); k++)
			{
				for (int l = 0; l < item.GetHeight(); l++)
				{
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData2 = room.ForceGetCellDataAtPoint(position.x + k, position.y + l);
					prototypeDungeonRoomCellData2.additionalPlacedObjectIndices[targetObjectLayer] = count2;
				}
			}
		}
		return prototypePlacedObjectData;
	}

	// Token: 0x0600871A RID: 34586
	public abstract void Develop(PrototypeDungeonRoom room, RobotDaveIdea idea, int targetObjectLayer);

	// Token: 0x04008C3F RID: 35903
	public IntVector2 LocalBasePosition;

	// Token: 0x04008C40 RID: 35904
	public IntVector2 LocalDimensions;
}
