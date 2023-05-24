using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001425 RID: 5157
public class ItemBlueprintItem : PassiveItem
{
	// Token: 0x06007509 RID: 29961 RVA: 0x002E9818 File Offset: 0x002E7A18
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		this.m_pickedUp = true;
		if (!this.m_pickedUpThisRun)
		{
			base.HandleEncounterable(player);
		}
		GameObject gameObject = (GameObject)BraveResources.Load("Global VFX/VFX_Item_Pickup", typeof(GameObject), ".prefab");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
		component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
		component.UpdateZDepth();
		this.m_pickedUpThisRun = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600750A RID: 29962 RVA: 0x002E98C4 File Offset: 0x002E7AC4
	public override DebrisObject Drop(PlayerController player)
	{
		Debug.LogError("IT SHOULD BE IMPOSSIBLE TO DROP BLUEPRINTS.");
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<ItemBlueprintItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600750B RID: 29963 RVA: 0x002E98F0 File Offset: 0x002E7AF0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040076E2 RID: 30434
	public string HologramIconSpriteName;
}
