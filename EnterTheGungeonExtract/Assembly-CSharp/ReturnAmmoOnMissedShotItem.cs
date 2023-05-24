using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200148A RID: 5258
public class ReturnAmmoOnMissedShotItem : PassiveItem, ILevelLoadedListener
{
	// Token: 0x06007789 RID: 30601 RVA: 0x002FA624 File Offset: 0x002F8824
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
	}

	// Token: 0x0600778A RID: 30602 RVA: 0x002FA654 File Offset: 0x002F8854
	public void BraveOnLevelWasLoaded()
	{
		this.m_slicesFired.Clear();
	}

	// Token: 0x0600778B RID: 30603 RVA: 0x002FA664 File Offset: 0x002F8864
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		if (obj.PlayerProjectileSourceGameTimeslice == -1f)
		{
			return;
		}
		if (this.m_slicesFired.ContainsKey(obj.PlayerProjectileSourceGameTimeslice))
		{
			this.m_slicesFired[obj.PlayerProjectileSourceGameTimeslice] = this.m_slicesFired[obj.PlayerProjectileSourceGameTimeslice] + 1;
		}
		else
		{
			this.m_slicesFired.Add(obj.PlayerProjectileSourceGameTimeslice, 1);
		}
		obj.OnDestruction += this.HandleProjectileDestruction;
	}

	// Token: 0x0600778C RID: 30604 RVA: 0x002FA6E8 File Offset: 0x002F88E8
	private void HandleProjectileDestruction(Projectile source)
	{
		if (source.PlayerProjectileSourceGameTimeslice == -1f)
		{
			return;
		}
		if (!this.m_slicesFired.ContainsKey(source.PlayerProjectileSourceGameTimeslice))
		{
			return;
		}
		if (this.m_player && source && source.PossibleSourceGun && !source.PossibleSourceGun.InfiniteAmmo && !source.HasImpactedEnemy)
		{
			this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] = this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] - 1;
			if (this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] == 0)
			{
				float num = this.ChanceToRegainAmmoOnMiss;
				if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.ZOMBIE_AMMO, false))
				{
					num = this.SynergyChance;
				}
				if (UnityEngine.Random.value < num)
				{
					source.PossibleSourceGun.GainAmmo(1);
				}
			}
		}
	}

	// Token: 0x0600778D RID: 30605 RVA: 0x002FA7E4 File Offset: 0x002F89E4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ReturnAmmoOnMissedShotItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		return debrisObject;
	}

	// Token: 0x0600778E RID: 30606 RVA: 0x002FA820 File Offset: 0x002F8A20
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
		}
	}

	// Token: 0x04007980 RID: 31104
	public float ChanceToRegainAmmoOnMiss = 0.25f;

	// Token: 0x04007981 RID: 31105
	public bool UsesZombieBulletsSynergy;

	// Token: 0x04007982 RID: 31106
	public float SynergyChance = 0.5f;

	// Token: 0x04007983 RID: 31107
	private PlayerController m_player;

	// Token: 0x04007984 RID: 31108
	private Dictionary<float, int> m_slicesFired = new Dictionary<float, int>();
}
