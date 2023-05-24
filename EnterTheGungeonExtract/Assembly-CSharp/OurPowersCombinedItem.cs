using System;

// Token: 0x02001457 RID: 5207
public class OurPowersCombinedItem : PassiveItem
{
	// Token: 0x06007641 RID: 30273 RVA: 0x002F0E94 File Offset: 0x002EF094
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
		player.PostProcessBeamTick += this.PostProcessBeamTick;
	}

	// Token: 0x06007642 RID: 30274 RVA: 0x002F0EF4 File Offset: 0x002EF0F4
	private float GetDamageContribution()
	{
		float num = 0f;
		if (this.m_player != null)
		{
			for (int i = 0; i < this.m_player.inventory.AllGuns.Count; i++)
			{
				Gun gun = this.m_player.inventory.AllGuns[i];
				if (!(gun == this.m_player.CurrentGun))
				{
					if (gun.DefaultModule != null)
					{
						if (gun.DefaultModule.projectiles.Count > 0 && gun.DefaultModule.projectiles[0] != null)
						{
							num += gun.DefaultModule.projectiles[0].baseData.damage * this.PercentOfOtherGunsDamage;
						}
						else if (gun.DefaultModule.chargeProjectiles != null && gun.DefaultModule.chargeProjectiles.Count > 0)
						{
							for (int j = 0; j < gun.DefaultModule.chargeProjectiles.Count; j++)
							{
								if (gun.DefaultModule.chargeProjectiles[j].Projectile != null)
								{
									num += gun.DefaultModule.chargeProjectiles[j].Projectile.baseData.damage * this.PercentOfOtherGunsDamage;
									break;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06007643 RID: 30275 RVA: 0x002F1070 File Offset: 0x002EF270
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		obj.baseData.damage += this.GetDamageContribution();
	}

	// Token: 0x06007644 RID: 30276 RVA: 0x002F108C File Offset: 0x002EF28C
	private void PostProcessBeam(BeamController beam)
	{
		beam.DamageModifier += this.GetDamageContribution();
	}

	// Token: 0x06007645 RID: 30277 RVA: 0x002F10A4 File Offset: 0x002EF2A4
	private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
	{
	}

	// Token: 0x06007646 RID: 30278 RVA: 0x002F10A8 File Offset: 0x002EF2A8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<OurPowersCombinedItem>().m_pickedUpThisRun = true;
		this.m_player = null;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.PostProcessBeamTick -= this.PostProcessBeamTick;
		return debrisObject;
	}

	// Token: 0x06007647 RID: 30279 RVA: 0x002F1108 File Offset: 0x002EF308
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
			this.m_player.PostProcessBeamTick -= this.PostProcessBeamTick;
		}
	}

	// Token: 0x04007822 RID: 30754
	public float PercentOfOtherGunsDamage = 0.02f;

	// Token: 0x04007823 RID: 30755
	protected PlayerController m_player;
}
