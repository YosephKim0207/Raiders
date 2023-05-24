using System;
using UnityEngine;

// Token: 0x0200136A RID: 4970
public class CartographersRingItem : PassiveItem
{
	// Token: 0x0600709F RID: 28831 RVA: 0x002CAE98 File Offset: 0x002C9098
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		bool flag = false;
		if (this.executeOnPickup && !this.m_pickedUpThisRun)
		{
			flag = true;
		}
		base.Pickup(player);
		if (flag)
		{
			this.PossiblyRevealMap();
		}
		GameManager.Instance.OnNewLevelFullyLoaded += this.PossiblyRevealMap;
	}

	// Token: 0x060070A0 RID: 28832 RVA: 0x002CAEF4 File Offset: 0x002C90F4
	public void PossiblyRevealMap()
	{
		if (UnityEngine.Random.value < this.revealChanceOnLoad && Minimap.Instance != null)
		{
			Minimap.Instance.RevealAllRooms(this.revealSecretRooms);
		}
	}

	// Token: 0x060070A1 RID: 28833 RVA: 0x002CAF28 File Offset: 0x002C9128
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<CartographersRingItem>().m_pickedUpThisRun = true;
		GameManager.Instance.OnNewLevelFullyLoaded -= this.PossiblyRevealMap;
		return debrisObject;
	}

	// Token: 0x060070A2 RID: 28834 RVA: 0x002CAF60 File Offset: 0x002C9160
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && GameManager.HasInstance)
		{
			GameManager.Instance.OnNewLevelFullyLoaded -= this.PossiblyRevealMap;
		}
		base.OnDestroy();
	}

	// Token: 0x04007019 RID: 28697
	public float revealChanceOnLoad = 0.5f;

	// Token: 0x0400701A RID: 28698
	public bool revealSecretRooms;

	// Token: 0x0400701B RID: 28699
	public bool executeOnPickup;
}
