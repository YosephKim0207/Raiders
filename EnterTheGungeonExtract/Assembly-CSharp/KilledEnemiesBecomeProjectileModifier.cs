using System;
using UnityEngine;

// Token: 0x020012A3 RID: 4771
public class KilledEnemiesBecomeProjectileModifier : BraveBehaviour
{
	// Token: 0x06006ABF RID: 27327 RVA: 0x0029DBD8 File Offset: 0x0029BDD8
	public void Start()
	{
		this.m_projectile = base.projectile;
		if (this.m_projectile)
		{
			Projectile projectile = this.m_projectile;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		}
	}

	// Token: 0x06006AC0 RID: 27328 RVA: 0x0029DC28 File Offset: 0x0029BE28
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool killedEnemy)
	{
		if (killedEnemy && hitRigidbody)
		{
			AIActor aiActor = hitRigidbody.aiActor;
			if (aiActor && aiActor.IsNormalEnemy && aiActor.healthHaver && !aiActor.healthHaver.IsBoss)
			{
				if (aiActor.GetComponent<ExplodeOnDeath>())
				{
					UnityEngine.Object.Destroy(aiActor.GetComponent<ExplodeOnDeath>());
				}
				if (this.CompletelyBecomeProjectile && hitRigidbody.sprite)
				{
					aiActor.specRigidbody.enabled = false;
					aiActor.EraseFromExistence(false);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BaseProjectile.gameObject, aiActor.transform.position, Quaternion.Euler(0f, 0f, sourceProjectile.LastVelocity.ToAngle()));
					Projectile component = gameObject.GetComponent<Projectile>();
					tk2dBaseSprite sprite = component.sprite;
					sprite.SetSprite(hitRigidbody.sprite.Collection, hitRigidbody.sprite.spriteId);
					component.shouldRotate = true;
				}
				else
				{
					hitRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox));
					hitRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(hitRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandleHitEnemyHitEnemy));
				}
			}
		}
	}

	// Token: 0x06006AC1 RID: 27329 RVA: 0x0029DD68 File Offset: 0x0029BF68
	private void HandleHitEnemyHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody && otherRigidbody.aiActor && myRigidbody && myRigidbody.healthHaver)
		{
			AIActor aiActor = otherRigidbody.aiActor;
			myRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(myRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandleHitEnemyHitEnemy));
			if (aiActor.IsNormalEnemy && aiActor.healthHaver)
			{
				aiActor.healthHaver.ApplyDamage(myRigidbody.healthHaver.GetMaxHealth() * 2f, myRigidbody.Velocity, "Pinball", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
		}
	}

	// Token: 0x0400675F RID: 26463
	public bool CompletelyBecomeProjectile;

	// Token: 0x04006760 RID: 26464
	public Projectile BaseProjectile;

	// Token: 0x04006761 RID: 26465
	private Projectile m_projectile;
}
