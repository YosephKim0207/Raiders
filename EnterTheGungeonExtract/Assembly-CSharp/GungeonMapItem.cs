using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001414 RID: 5140
public class GungeonMapItem : PassiveItem
{
	// Token: 0x060074A2 RID: 29858 RVA: 0x002E6E44 File Offset: 0x002E5044
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
		player.EverHadMap = true;
		player.stats.RecalculateStats(player, false, false);
		if (Minimap.Instance != null)
		{
			Minimap.Instance.RevealAllRooms(true);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060074A3 RID: 29859 RVA: 0x002E6F20 File Offset: 0x002E5120
	public override DebrisObject Drop(PlayerController player)
	{
		Debug.LogError("IT SHOULD BE IMPOSSIBLE TO DROP MAPS.");
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<GungeonMapItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060074A4 RID: 29860 RVA: 0x002E6F4C File Offset: 0x002E514C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
