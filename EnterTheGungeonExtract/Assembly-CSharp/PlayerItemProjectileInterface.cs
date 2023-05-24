using System;

// Token: 0x02001455 RID: 5205
[Serializable]
public class PlayerItemProjectileInterface
{
	// Token: 0x0600763A RID: 30266 RVA: 0x002F0D74 File Offset: 0x002EEF74
	public Projectile GetProjectile(PlayerController targetPlayer)
	{
		if (this.UseCurrentGunProjectile && targetPlayer.CurrentGun)
		{
			Projectile currentProjectile = targetPlayer.CurrentGun.DefaultModule.GetCurrentProjectile();
			if (currentProjectile)
			{
				return currentProjectile;
			}
		}
		return this.SpecifiedProjectile;
	}

	// Token: 0x0400781D RID: 30749
	public bool UseCurrentGunProjectile = true;

	// Token: 0x0400781E RID: 30750
	public Projectile SpecifiedProjectile;
}
