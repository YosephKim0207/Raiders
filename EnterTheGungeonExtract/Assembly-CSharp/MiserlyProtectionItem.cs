using System;
using UnityEngine;

// Token: 0x0200143D RID: 5181
public class MiserlyProtectionItem : BasicStatPickup
{
	// Token: 0x06007595 RID: 30101 RVA: 0x002ED58C File Offset: 0x002EB78C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnItemPurchased += this.OnItemPurchased;
	}

	// Token: 0x06007596 RID: 30102 RVA: 0x002ED5B4 File Offset: 0x002EB7B4
	public void Break()
	{
		this.m_pickedUp = true;
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	// Token: 0x06007597 RID: 30103 RVA: 0x002ED5D0 File Offset: 0x002EB7D0
	private void OnItemPurchased(PlayerController player, ShopItemController obj)
	{
		if (obj != null && obj.item is MiserlyProtectionItem)
		{
			return;
		}
		if (player.HasActiveBonusSynergy(CustomSynergyType.MISERLY_PIGTECTION, false))
		{
			return;
		}
		player.DropPassiveItem(this);
	}

	// Token: 0x06007598 RID: 30104 RVA: 0x002ED60C File Offset: 0x002EB80C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		MiserlyProtectionItem component = debrisObject.GetComponent<MiserlyProtectionItem>();
		player.OnItemPurchased -= this.OnItemPurchased;
		component.m_pickedUpThisRun = true;
		if (!player.HasActiveBonusSynergy(CustomSynergyType.MISERLY_PIGTECTION, false))
		{
			component.Break();
		}
		return debrisObject;
	}

	// Token: 0x06007599 RID: 30105 RVA: 0x002ED65C File Offset: 0x002EB85C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
