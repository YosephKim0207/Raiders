using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200167F RID: 5759
public class SpawnItemOnGunDepletion : MonoBehaviour
{
	// Token: 0x06008657 RID: 34391 RVA: 0x003791D8 File Offset: 0x003773D8
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06008658 RID: 34392 RVA: 0x003791E8 File Offset: 0x003773E8
	private void Update()
	{
		if (base.enabled && this.m_gun && this.m_gun.ammo <= 0 && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (!this.IsSynergyContingent || playerController.HasActiveBonusSynergy(this.SynergyToCheck, false))
			{
				if (this.UsesSpecificItem)
				{
					LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(this.SpecificItemId).gameObject, playerController, false);
				}
				else if (playerController && playerController.CurrentRoom != null)
				{
					IntVector2 bestRewardLocation = playerController.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
					Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation);
					if (chest)
					{
						chest.IsLocked = false;
					}
				}
				playerController.inventory.RemoveGunFromInventory(this.m_gun);
				UnityEngine.Object.Destroy(this.m_gun.gameObject);
			}
		}
	}

	// Token: 0x04008B30 RID: 35632
	public bool IsSynergyContingent;

	// Token: 0x04008B31 RID: 35633
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008B32 RID: 35634
	public bool UsesSpecificItem;

	// Token: 0x04008B33 RID: 35635
	[PickupIdentifier]
	public int SpecificItemId;

	// Token: 0x04008B34 RID: 35636
	protected Gun m_gun;
}
