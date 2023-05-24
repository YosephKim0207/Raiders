using System;
using UnityEngine;

// Token: 0x0200146C RID: 5228
public class ProjectileRandomizerItem : PassiveItem
{
	// Token: 0x060076DF RID: 30431 RVA: 0x002F61C4 File Offset: 0x002F43C4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
	}

	// Token: 0x060076E0 RID: 30432 RVA: 0x002F6204 File Offset: 0x002F4404
	private static float GetQualityModifiedAmmo(Gun cg)
	{
		PickupObject.ItemQuality quality = cg.quality;
		if (quality == PickupObject.ItemQuality.A)
		{
			return Mathf.Min((float)cg.AdjustedMaxAmmo * 0.8f, 250f);
		}
		if (quality != PickupObject.ItemQuality.S)
		{
			return (float)cg.AdjustedMaxAmmo;
		}
		return Mathf.Min((float)cg.AdjustedMaxAmmo * 0.7f, 100f);
	}

	// Token: 0x060076E1 RID: 30433 RVA: 0x002F6264 File Offset: 0x002F4464
	public static Projectile GetRandomizerProjectileFromPlayer(PlayerController sourcePlayer, Projectile fallbackProjectile, int fallbackAmmo)
	{
		int num = fallbackAmmo;
		for (int i = 0; i < sourcePlayer.inventory.AllGuns.Count; i++)
		{
			if (sourcePlayer.inventory.AllGuns[i] && !sourcePlayer.inventory.AllGuns[i].InfiniteAmmo)
			{
				Gun gun = sourcePlayer.inventory.AllGuns[i];
				num += Mathf.CeilToInt(ProjectileRandomizerItem.GetQualityModifiedAmmo(gun));
			}
		}
		int num2 = fallbackAmmo;
		float num3 = (float)num * UnityEngine.Random.value;
		if ((float)num2 > num3)
		{
			return fallbackProjectile;
		}
		for (int j = 0; j < sourcePlayer.inventory.AllGuns.Count; j++)
		{
			if (sourcePlayer.inventory.AllGuns[j] && !sourcePlayer.inventory.AllGuns[j].InfiniteAmmo)
			{
				Gun gun2 = sourcePlayer.inventory.AllGuns[j];
				num2 += Mathf.CeilToInt(ProjectileRandomizerItem.GetQualityModifiedAmmo(gun2));
				if ((float)num2 > num3)
				{
					ProjectileModule defaultModule = sourcePlayer.inventory.AllGuns[j].DefaultModule;
					if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
					{
						return fallbackProjectile;
					}
					if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
					{
						Projectile projectile = null;
						for (int k = 0; k < 15; k++)
						{
							ProjectileModule.ChargeProjectile chargeProjectile = defaultModule.chargeProjectiles[UnityEngine.Random.Range(0, defaultModule.chargeProjectiles.Count)];
							if (chargeProjectile != null)
							{
								projectile = chargeProjectile.Projectile;
							}
							if (projectile)
							{
								break;
							}
						}
						return projectile ?? fallbackProjectile;
					}
					Projectile currentProjectile = defaultModule.GetCurrentProjectile();
					return currentProjectile ?? fallbackProjectile;
				}
			}
		}
		return fallbackProjectile;
	}

	// Token: 0x060076E2 RID: 30434 RVA: 0x002F643C File Offset: 0x002F463C
	private Projectile HandlePreFireProjectileModification(Gun sourceGun, Projectile sourceProjectile)
	{
		float num = this.OverallChanceToTakeEffect;
		if (sourceGun && sourceGun.Volley != null)
		{
			num /= (float)sourceGun.Volley.projectiles.Count;
		}
		if (UnityEngine.Random.value > num)
		{
			return sourceProjectile;
		}
		if (sourceGun && sourceGun.InfiniteAmmo)
		{
			return sourceProjectile;
		}
		int num2 = 0;
		if (this.m_player && this.m_player.inventory != null)
		{
			for (int i = 0; i < this.m_player.inventory.AllGuns.Count; i++)
			{
				if (this.m_player.inventory.AllGuns[i] && !this.m_player.inventory.AllGuns[i].InfiniteAmmo)
				{
					Gun gun = this.m_player.inventory.AllGuns[i];
					num2 += Mathf.CeilToInt(ProjectileRandomizerItem.GetQualityModifiedAmmo(gun));
				}
			}
			int num3 = 0;
			float num4 = (float)num2 * UnityEngine.Random.value;
			for (int j = 0; j < this.m_player.inventory.AllGuns.Count; j++)
			{
				if (this.m_player.inventory.AllGuns[j] && !this.m_player.inventory.AllGuns[j].InfiniteAmmo)
				{
					Gun gun2 = this.m_player.inventory.AllGuns[j];
					num3 += Mathf.CeilToInt(ProjectileRandomizerItem.GetQualityModifiedAmmo(gun2));
					if ((float)num3 > num4)
					{
						ProjectileModule defaultModule = this.m_player.inventory.AllGuns[j].DefaultModule;
						if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
						{
							BeamController.FreeFireBeam(defaultModule.GetCurrentProjectile(), this.m_player, this.m_player.CurrentGun.CurrentAngle, this.BeamShootDuration, true);
							return sourceProjectile;
						}
						if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
						{
							Projectile projectile = null;
							for (int k = 0; k < 15; k++)
							{
								ProjectileModule.ChargeProjectile chargeProjectile = defaultModule.chargeProjectiles[UnityEngine.Random.Range(0, defaultModule.chargeProjectiles.Count)];
								if (chargeProjectile != null)
								{
									projectile = chargeProjectile.Projectile;
								}
								if (projectile)
								{
									break;
								}
							}
							return projectile ?? sourceProjectile;
						}
						Projectile currentProjectile = defaultModule.GetCurrentProjectile();
						return currentProjectile ?? sourceProjectile;
					}
				}
			}
		}
		return sourceProjectile;
	}

	// Token: 0x060076E3 RID: 30435 RVA: 0x002F66E0 File Offset: 0x002F48E0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ProjectileRandomizerItem>().m_pickedUpThisRun = true;
		player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
		return debrisObject;
	}

	// Token: 0x060076E4 RID: 30436 RVA: 0x002F672C File Offset: 0x002F492C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			PlayerController player = this.m_player;
			player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
		}
	}

	// Token: 0x040078D0 RID: 30928
	public float OverallChanceToTakeEffect = 0.5f;

	// Token: 0x040078D1 RID: 30929
	public float BeamShootDuration = 3f;

	// Token: 0x040078D2 RID: 30930
	private PlayerController m_player;
}
