using System;
using UnityEngine;

// Token: 0x02001456 RID: 5206
public class OnPurchasePassiveItem : PassiveItem
{
	// Token: 0x0600763C RID: 30268 RVA: 0x002F0DE8 File Offset: 0x002EEFE8
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnItemPurchased += this.OnItemPurchased;
	}

	// Token: 0x0600763D RID: 30269 RVA: 0x002F0E10 File Offset: 0x002EF010
	private void OnItemPurchased(PlayerController player, ShopItemController obj)
	{
		if (UnityEngine.Random.value < this.ActivationChance && this.DoesHeal)
		{
			player.healthHaver.ApplyHealing(this.HealingAmount);
		}
	}

	// Token: 0x0600763E RID: 30270 RVA: 0x002F0E40 File Offset: 0x002EF040
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		OnPurchasePassiveItem component = debrisObject.GetComponent<OnPurchasePassiveItem>();
		player.OnItemPurchased -= this.OnItemPurchased;
		component.m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600763F RID: 30271 RVA: 0x002F0E78 File Offset: 0x002EF078
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400781F RID: 30751
	public float ActivationChance = 0.5f;

	// Token: 0x04007820 RID: 30752
	public bool DoesHeal = true;

	// Token: 0x04007821 RID: 30753
	public float HealingAmount = 0.5f;
}
