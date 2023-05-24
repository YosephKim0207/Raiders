using System;
using UnityEngine;

// Token: 0x020016EF RID: 5871
public class GunMergeSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600887B RID: 34939 RVA: 0x00389268 File Offset: 0x00387468
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x0600887C RID: 34940 RVA: 0x00389278 File Offset: 0x00387478
	public void Update()
	{
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			for (int i = 0; i < playerController.inventory.AllGuns.Count; i++)
			{
				if (playerController.inventory.AllGuns[i].PickupObjectId == this.OtherGunID)
				{
					playerController.inventory.RemoveGunFromInventory(playerController.inventory.AllGuns[i]);
					playerController.inventory.RemoveGunFromInventory(this.m_gun);
					LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.MergeGunID).gameObject, playerController, true);
				}
			}
		}
	}

	// Token: 0x04008DE0 RID: 36320
	[PickupIdentifier]
	public int OtherGunID;

	// Token: 0x04008DE1 RID: 36321
	[PickupIdentifier]
	public int MergeGunID;

	// Token: 0x04008DE2 RID: 36322
	private Gun m_gun;
}
