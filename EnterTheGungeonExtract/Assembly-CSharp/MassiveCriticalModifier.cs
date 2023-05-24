using System;
using UnityEngine;

// Token: 0x02001434 RID: 5172
public class MassiveCriticalModifier : MonoBehaviour
{
	// Token: 0x06007564 RID: 30052 RVA: 0x002EC3C0 File Offset: 0x002EA5C0
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Combine(gun.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HandleProjectileReplacement));
	}

	// Token: 0x06007565 RID: 30053 RVA: 0x002EC3F8 File Offset: 0x002EA5F8
	private Projectile HandleProjectileReplacement(Gun sourceGun, Projectile sourceProjectile, ProjectileModule sourceModule)
	{
		if (UnityEngine.Random.value > this.ActivationChance)
		{
			return sourceProjectile;
		}
		this.ReplacementProjectile.IsCritical = true;
		return this.ReplacementProjectile;
	}

	// Token: 0x04007749 RID: 30537
	public float ActivationChance = 0.01f;

	// Token: 0x0400774A RID: 30538
	public Projectile ReplacementProjectile;

	// Token: 0x0400774B RID: 30539
	private Gun m_gun;
}
