using System;

// Token: 0x02001334 RID: 4916
public class ActiveItemCooldownItem : PassiveItem
{
	// Token: 0x06006F70 RID: 28528 RVA: 0x002C2B18 File Offset: 0x002C0D18
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		PlayerItem.AllowDamageCooldownOnActive = true;
		base.Pickup(player);
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x002C2B34 File Offset: 0x002C0D34
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		PlayerItem.AllowDamageCooldownOnActive = false;
		debrisObject.GetComponent<ActiveItemCooldownItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x002C2B5C File Offset: 0x002C0D5C
	protected override void OnDestroy()
	{
		PlayerItem.AllowDamageCooldownOnActive = false;
		base.OnDestroy();
	}
}
