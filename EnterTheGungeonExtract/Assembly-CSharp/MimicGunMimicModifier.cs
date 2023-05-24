using System;
using UnityEngine;

// Token: 0x020013CC RID: 5068
public class MimicGunMimicModifier : MonoBehaviour
{
	// Token: 0x060072F1 RID: 29425 RVA: 0x002DACFC File Offset: 0x002D8EFC
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x060072F2 RID: 29426 RVA: 0x002DAD0C File Offset: 0x002D8F0C
	private void Update()
	{
		if (!this.m_initialized && this.m_gun.CurrentOwner != null)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.IsGunLocked)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			Gun gun = playerController.inventory.AddGunToInventory(PickupObjectDatabase.GetById(GlobalItemIds.GunMimicID) as Gun, true);
			gun.GetComponent<MimicGunController>().Initialize(playerController, this.m_gun);
			this.m_initialized = true;
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x04007442 RID: 29762
	private Gun m_gun;

	// Token: 0x04007443 RID: 29763
	private bool m_initialized;
}
