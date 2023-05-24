using System;
using UnityEngine;

// Token: 0x0200135A RID: 4954
public class BonusDamageToSpecificEnemies : MonoBehaviour
{
	// Token: 0x0600705A RID: 28762 RVA: 0x002C8F80 File Offset: 0x002C7180
	public void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		Projectile projectile = this.m_projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x0600705B RID: 28763 RVA: 0x002C8FB8 File Offset: 0x002C71B8
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitEnemyRigidbody, bool killedEnemy)
	{
		if (!killedEnemy && hitEnemyRigidbody.aiActor && Array.IndexOf<string>(this.enemyGuids, hitEnemyRigidbody.aiActor.EnemyGuid) != -1)
		{
			hitEnemyRigidbody.aiActor.healthHaver.ApplyDamage(sourceProjectile.ModifiedDamage * this.damageFraction, Vector2.zero, "bonus damage", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
		}
	}

	// Token: 0x04006FD0 RID: 28624
	[EnemyIdentifier]
	public string[] enemyGuids;

	// Token: 0x04006FD1 RID: 28625
	public float damageFraction = 0.5f;

	// Token: 0x04006FD2 RID: 28626
	private Projectile m_projectile;
}
