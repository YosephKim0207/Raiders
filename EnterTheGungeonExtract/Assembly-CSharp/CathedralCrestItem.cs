using System;

// Token: 0x0200136B RID: 4971
public class CathedralCrestItem : PassiveItem
{
	// Token: 0x060070A4 RID: 28836 RVA: 0x002CAF9C File Offset: 0x002C919C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.healthHaver.HasCrest = true;
		player.OnReceivedDamage += this.PlayerDamaged;
		player.healthHaver.Armor += 1f;
	}

	// Token: 0x060070A5 RID: 28837 RVA: 0x002CAFF4 File Offset: 0x002C91F4
	private void PlayerDamaged(PlayerController obj)
	{
		obj.healthHaver.HasCrest = false;
		obj.RemovePassiveItem(this.PickupObjectId);
	}

	// Token: 0x060070A6 RID: 28838 RVA: 0x002CB010 File Offset: 0x002C9210
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.healthHaver.HasCrest = false;
		if (debrisObject)
		{
			CathedralCrestItem component = debrisObject.GetComponent<CathedralCrestItem>();
			if (component)
			{
				component.m_pickedUpThisRun = true;
			}
		}
		player.OnReceivedDamage -= this.PlayerDamaged;
		return debrisObject;
	}

	// Token: 0x060070A7 RID: 28839 RVA: 0x002CB068 File Offset: 0x002C9268
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && GameManager.HasInstance && base.Owner)
		{
			base.Owner.healthHaver.HasCrest = false;
			base.Owner.OnReceivedDamage -= this.PlayerDamaged;
		}
		base.OnDestroy();
	}
}
