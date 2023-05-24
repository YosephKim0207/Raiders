using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E76 RID: 3702
public class DungeonPlaceableContainer : MonoBehaviour
{
	// Token: 0x06004EC5 RID: 20165 RVA: 0x001B3AE8 File Offset: 0x001B1CE8
	private void Awake()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector);
		GameObject gameObject = this.placeable.InstantiateObject(absoluteRoomFromPosition, intVector - absoluteRoomFromPosition.area.basePosition, false, false);
		if (gameObject != null)
		{
			IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
			for (int i = 0; i < interfacesInChildren.Length; i++)
			{
				absoluteRoomFromPosition.RegisterInteractable(interfacesInChildren[i]);
			}
			SurfaceDecorator component = gameObject.GetComponent<SurfaceDecorator>();
			if (component != null)
			{
				component.Decorate(absoluteRoomFromPosition);
			}
		}
	}

	// Token: 0x0400458A RID: 17802
	public DungeonPlaceable placeable;
}
