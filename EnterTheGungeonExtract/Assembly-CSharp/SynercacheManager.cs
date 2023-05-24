using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x0200121E RID: 4638
public class SynercacheManager : BraveBehaviour
{
	// Token: 0x060067BD RID: 26557 RVA: 0x0028A054 File Offset: 0x00288254
	public static void ClearPerLevelData()
	{
		SynercacheManager.UseCachedSynergyIDs = false;
		SynercacheManager.LastCachedSynergyIDs.Clear();
	}

	// Token: 0x060067BE RID: 26558 RVA: 0x0028A068 File Offset: 0x00288268
	private void Start()
	{
		this.m_room = base.transform.position.GetAbsoluteRoom();
		if (this.TriggersOnMinimapVisibility)
		{
			RoomHandler room = this.m_room;
			room.OnRevealedOnMap = (Action)Delegate.Combine(room.OnRevealedOnMap, new Action(this.Cache));
		}
		this.m_room.BecameVisible += this.HandleBecameVisible;
	}

	// Token: 0x060067BF RID: 26559 RVA: 0x0028A0D4 File Offset: 0x002882D4
	private void HandleBecameVisible(float delay)
	{
		this.Cache();
	}

	// Token: 0x060067C0 RID: 26560 RVA: 0x0028A0DC File Offset: 0x002882DC
	private void Cache()
	{
		if (this.m_synercached)
		{
			return;
		}
		this.m_synercached = true;
		SynercacheManager.LastCachedSynergyIDs.Clear();
		this.m_room.BecameVisible -= this.HandleBecameVisible;
		RoomHandler room = this.m_room;
		room.OnRevealedOnMap = (Action)Delegate.Remove(room.OnRevealedOnMap, new Action(this.Cache));
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			for (int j = 0; j < playerController.passiveItems.Count; j++)
			{
				PickupObject pickupObject = playerController.passiveItems[j];
				if (pickupObject)
				{
					SynercacheManager.LastCachedSynergyIDs.Add(pickupObject.PickupObjectId);
				}
			}
			for (int k = 0; k < playerController.activeItems.Count; k++)
			{
				PickupObject pickupObject2 = playerController.activeItems[k];
				if (pickupObject2)
				{
					SynercacheManager.LastCachedSynergyIDs.Add(pickupObject2.PickupObjectId);
				}
			}
			for (int l = 0; l < playerController.inventory.AllGuns.Count; l++)
			{
				PickupObject pickupObject3 = playerController.inventory.AllGuns[l];
				if (pickupObject3)
				{
					SynercacheManager.LastCachedSynergyIDs.Add(pickupObject3.PickupObjectId);
				}
			}
		}
	}

	// Token: 0x040063BA RID: 25530
	public static bool UseCachedSynergyIDs = false;

	// Token: 0x040063BB RID: 25531
	public static List<int> LastCachedSynergyIDs = new List<int>();

	// Token: 0x040063BC RID: 25532
	public bool TriggersOnMinimapVisibility;

	// Token: 0x040063BD RID: 25533
	private bool m_synercached;

	// Token: 0x040063BE RID: 25534
	private RoomHandler m_room;
}
