using System;
using UnityEngine;

// Token: 0x02001448 RID: 5192
public class OnDamagedPassiveItem : PassiveItem
{
	// Token: 0x060075E1 RID: 30177 RVA: 0x002EEC98 File Offset: 0x002ECE98
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.healthHaver.OnDamaged += this.PlayerTookDamage;
	}

	// Token: 0x060075E2 RID: 30178 RVA: 0x002EECC4 File Offset: 0x002ECEC4
	private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (resultValue < maxValue || this.DoesEffectOnArmorLoss)
		{
			if (base.Owner.CurrentGun != null && this.FlatAmmoToGive > 0)
			{
				base.Owner.CurrentGun.GainAmmo(this.FlatAmmoToGive);
			}
			if (base.Owner.CurrentGun != null && this.PercentAmmoToGive > 0f)
			{
				base.Owner.CurrentGun.GainAmmo(Mathf.CeilToInt((float)base.Owner.CurrentGun.AdjustedMaxAmmo * this.PercentAmmoToGive));
			}
			if (this.ArmorToGive > 0)
			{
				base.Owner.healthHaver.Armor += (float)this.ArmorToGive;
			}
			if (this.DoesDamageToEnemiesInRoom)
			{
				base.Owner.CurrentRoom.ApplyActionToNearbyEnemies(base.Owner.CenterPosition, 100f, delegate(AIActor enemy, float dist)
				{
					if (enemy && enemy.healthHaver)
					{
						enemy.healthHaver.ApplyDamage(this.DamageToEnemiesInRoom, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
					}
				});
			}
			if (this.HasSynergy && base.Owner.HasActiveBonusSynergy(this.RequiredSynergy, false) && this.SynergyAugmentsNextShot && base.Owner.CurrentGun.CanCriticalFire)
			{
				base.Owner.CurrentGun.ForceNextShotCritical = true;
			}
		}
	}

	// Token: 0x060075E3 RID: 30179 RVA: 0x002EEE24 File Offset: 0x002ED024
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		OnDamagedPassiveItem component = debrisObject.GetComponent<OnDamagedPassiveItem>();
		player.healthHaver.OnDamaged -= this.PlayerTookDamage;
		component.m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060075E4 RID: 30180 RVA: 0x002EEE60 File Offset: 0x002ED060
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.healthHaver.OnDamaged -= this.PlayerTookDamage;
		}
		base.OnDestroy();
	}

	// Token: 0x0400779A RID: 30618
	public int ArmorToGive;

	// Token: 0x0400779B RID: 30619
	public int FlatAmmoToGive;

	// Token: 0x0400779C RID: 30620
	public float PercentAmmoToGive;

	// Token: 0x0400779D RID: 30621
	public bool DoesEffectOnArmorLoss;

	// Token: 0x0400779E RID: 30622
	public bool DoesDamageToEnemiesInRoom;

	// Token: 0x0400779F RID: 30623
	public float DamageToEnemiesInRoom = 25f;

	// Token: 0x040077A0 RID: 30624
	public bool HasSynergy;

	// Token: 0x040077A1 RID: 30625
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x040077A2 RID: 30626
	public bool SynergyAugmentsNextShot;
}
