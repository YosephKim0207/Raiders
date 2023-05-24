using System;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x0200145C RID: 5212
public class PassiveReflectItem : PassiveItem
{
	// Token: 0x06007669 RID: 30313 RVA: 0x002F2034 File Offset: 0x002F0234
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		this.m_player = player;
		SpeculativeRigidbody specRigidbody = player.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
	}

	// Token: 0x0600766A RID: 30314 RVA: 0x002F2084 File Offset: 0x002F0284
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<PassiveReflectItem>().m_pickedUpThisRun = true;
		SpeculativeRigidbody specRigidbody = player.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		return debrisObject;
	}

	// Token: 0x0600766B RID: 30315 RVA: 0x002F20D0 File Offset: 0x002F02D0
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (this.condition == PassiveReflectItem.Condition.WhileDodgeRolling && !this.m_player.spriteAnimator.QueryInvulnerabilityFrame())
		{
			return;
		}
		Projectile component = otherRigidbody.GetComponent<Projectile>();
		if (component != null)
		{
			PassiveReflectItem.ReflectBullet(component, this.retargetReflectedBullet, this.m_owner, this.minReflectedBulletSpeed, 1f, 1f, 0f);
			if (this.AmmoGainedOnReflection > 0)
			{
				Gun currentGun = this.m_owner.CurrentGun;
				if (currentGun && currentGun.CanGainAmmo)
				{
					currentGun.GainAmmo(this.AmmoGainedOnReflection);
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", component.gameObject);
			otherRigidbody.transform.position += component.Direction.ToVector3ZUp(0f) * 0.5f;
			otherRigidbody.Reinitialize();
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x0600766C RID: 30316 RVA: 0x002F21C0 File Offset: 0x002F03C0
	public static int ReflectBulletsInRange(Vector2 centerPoint, float radius, bool retargetReflectedBulet, GameActor newOwner, float minReflectedBulletSpeed, float scaleModifier = 1f, float damageModifier = 1f, bool applyPostprocess = false)
	{
		int num = 0;
		float num2 = radius * radius;
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = 0; i < allProjectiles.Count; i++)
		{
			Projectile projectile = allProjectiles[i];
			if (!projectile.Owner || !(projectile.Owner is PlayerController))
			{
				if (projectile.specRigidbody)
				{
					if (projectile && projectile.sprite)
					{
						float sqrMagnitude = (projectile.sprite.WorldCenter - centerPoint).sqrMagnitude;
						if (sqrMagnitude <= num2)
						{
							PassiveReflectItem.ReflectBullet(projectile, retargetReflectedBulet, newOwner, minReflectedBulletSpeed, scaleModifier, damageModifier, 0f);
							num++;
							if (applyPostprocess && newOwner is PlayerController)
							{
								SpawnManager.PoolManager.Remove(projectile.transform);
								(newOwner as PlayerController).CustomPostProcessProjectile(projectile, 1f);
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600766D RID: 30317 RVA: 0x002F22CC File Offset: 0x002F04CC
	public static void ReflectBullet(Projectile p, bool retargetReflectedBullet, GameActor newOwner, float minReflectedBulletSpeed, float scaleModifier = 1f, float damageModifier = 1f, float spread = 0f)
	{
		p.RemoveBulletScriptControl();
		AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", GameManager.Instance.gameObject);
		if (retargetReflectedBullet && p.Owner && p.Owner.specRigidbody)
		{
			p.Direction = (p.Owner.specRigidbody.GetUnitCenter(ColliderType.HitBox) - p.specRigidbody.UnitCenter).normalized;
		}
		else
		{
			Vector2 vector = p.LastVelocity;
			if (p.IsBulletScript && p.braveBulletScript && p.braveBulletScript.bullet != null)
			{
				vector = p.braveBulletScript.bullet.Velocity;
			}
			p.Direction = -vector.normalized;
			if (p.Direction == Vector2.zero)
			{
				p.Direction = UnityEngine.Random.insideUnitCircle.normalized;
			}
		}
		if (spread != 0f)
		{
			p.Direction = p.Direction.Rotate(UnityEngine.Random.Range(-spread, spread));
		}
		if (p.Owner && p.Owner.specRigidbody)
		{
			p.specRigidbody.DeregisterSpecificCollisionException(p.Owner.specRigidbody);
		}
		p.Owner = newOwner;
		p.SetNewShooter(newOwner.specRigidbody);
		p.allowSelfShooting = false;
		if (newOwner is AIActor)
		{
			p.collidesWithPlayer = true;
			p.collidesWithEnemies = false;
		}
		else
		{
			p.collidesWithPlayer = false;
			p.collidesWithEnemies = true;
		}
		if (scaleModifier != 1f)
		{
			SpawnManager.PoolManager.Remove(p.transform);
			p.RuntimeUpdateScale(scaleModifier);
		}
		if (p.Speed < minReflectedBulletSpeed)
		{
			p.Speed = minReflectedBulletSpeed;
		}
		if (p.baseData.damage < ProjectileData.FixedFallbackDamageToEnemies)
		{
			p.baseData.damage = ProjectileData.FixedFallbackDamageToEnemies;
		}
		p.baseData.damage *= damageModifier;
		if (p.baseData.damage < 10f)
		{
			p.baseData.damage = 15f;
		}
		p.UpdateCollisionMask();
		p.ResetDistance();
		p.Reflected();
	}

	// Token: 0x0600766E RID: 30318 RVA: 0x002F2520 File Offset: 0x002F0720
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007844 RID: 30788
	public PassiveReflectItem.Condition condition;

	// Token: 0x04007845 RID: 30789
	public float minReflectedBulletSpeed = 10f;

	// Token: 0x04007846 RID: 30790
	public bool retargetReflectedBullet = true;

	// Token: 0x04007847 RID: 30791
	public int AmmoGainedOnReflection;

	// Token: 0x04007848 RID: 30792
	private PlayerController m_player;

	// Token: 0x0200145D RID: 5213
	public enum Condition
	{
		// Token: 0x0400784A RID: 30794
		WhileDodgeRolling
	}
}
