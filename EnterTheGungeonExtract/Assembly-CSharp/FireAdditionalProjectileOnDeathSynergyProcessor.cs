using System;
using UnityEngine;

// Token: 0x020016E3 RID: 5859
public class FireAdditionalProjectileOnDeathSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600884E RID: 34894 RVA: 0x00387E04 File Offset: 0x00386004
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_projectile.OnDestruction += this.HandleDestruction;
	}

	// Token: 0x0600884F RID: 34895 RVA: 0x00387E2C File Offset: 0x0038602C
	private void HandleDestruction(Projectile obj)
	{
		if (this.m_projectile.Owner is PlayerController && (this.m_projectile.Owner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false) && this.Source == FireAdditionalProjectileOnDeathSynergyProcessor.ProjectileSource.GUN_THROUGH_CURRENT && this.m_projectile && this.m_projectile.specRigidbody && this.m_projectile.PossibleSourceGun && this.m_projectile.PossibleSourceGun.gameObject.activeSelf)
		{
			if (this.m_projectile.specRigidbody.UnitCenter.GetAbsoluteRoom() != (this.m_projectile.Owner as PlayerController).CurrentRoom)
			{
				return;
			}
			float num = (this.m_projectile.specRigidbody.UnitCenter - this.m_projectile.PossibleSourceGun.barrelOffset.PositionVector2()).ToAngle();
			GameObject gameObject = SpawnManager.SpawnProjectile(this.ProjectileToFire.gameObject, this.m_projectile.PossibleSourceGun.barrelOffset.position, Quaternion.Euler(0f, 0f, num), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			component.Owner = this.m_projectile.Owner;
			component.Shooter = this.m_projectile.Shooter;
			component.collidesWithPlayer = false;
			if (component)
			{
				component.SpawnedFromOtherPlayerProjectile = true;
			}
		}
	}

	// Token: 0x04008D9B RID: 36251
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008D9C RID: 36252
	public Projectile ProjectileToFire;

	// Token: 0x04008D9D RID: 36253
	public FireAdditionalProjectileOnDeathSynergyProcessor.ProjectileSource Source;

	// Token: 0x04008D9E RID: 36254
	private Projectile m_projectile;

	// Token: 0x020016E4 RID: 5860
	public enum ProjectileSource
	{
		// Token: 0x04008DA0 RID: 36256
		GUN_THROUGH_CURRENT
	}
}
