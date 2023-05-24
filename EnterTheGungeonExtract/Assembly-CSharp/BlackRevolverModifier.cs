using System;
using UnityEngine;

// Token: 0x020013A4 RID: 5028
public class BlackRevolverModifier : MonoBehaviour
{
	// Token: 0x060071EC RID: 29164 RVA: 0x002D43E0 File Offset: 0x002D25E0
	public void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_gun = this.m_projectile.PossibleSourceGun;
	}

	// Token: 0x060071ED RID: 29165 RVA: 0x002D4400 File Offset: 0x002D2600
	public void Update()
	{
		if (this.m_gun && this.m_gun.CurrentOwner && this.m_projectile)
		{
			Vector2 unitCenter = this.m_projectile.specRigidbody.UnitCenter;
			Vector2 direction = this.m_projectile.Direction;
			float num = this.WakeRadius * this.WakeRadius;
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile = StaticReferenceManager.AllProjectiles[i];
				if (projectile && projectile.Owner != this.m_gun.CurrentOwner)
				{
					Vector2 vector = ((!projectile.specRigidbody) ? projectile.transform.position.XY() : projectile.specRigidbody.UnitCenter);
					float sqrMagnitude = (vector - unitCenter).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						Vector2 vector2 = direction;
						this.RedirectBullet(projectile, this.m_gun.CurrentOwner, vector2, 10f, 0f, 1f, 1f);
					}
				}
			}
		}
	}

	// Token: 0x060071EE RID: 29166 RVA: 0x002D453C File Offset: 0x002D273C
	public void RedirectBullet(Projectile p, GameActor newOwner, Vector2 newDirection, float minReflectedBulletSpeed, float angleVariance = 0f, float scaleModifier = 1f, float damageModifier = 1f)
	{
		p.RemoveBulletScriptControl();
		p.Direction = newDirection.normalized;
		if (p.Direction == Vector2.zero)
		{
			p.Direction = UnityEngine.Random.insideUnitCircle.normalized;
		}
		if (angleVariance != 0f)
		{
			p.Direction = p.Direction.Rotate(UnityEngine.Random.Range(-angleVariance, angleVariance));
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
		p.UpdateCollisionMask();
		p.ResetDistance();
		p.Reflected();
	}

	// Token: 0x04007356 RID: 29526
	public float WakeRadius = 3f;

	// Token: 0x04007357 RID: 29527
	private Projectile m_projectile;

	// Token: 0x04007358 RID: 29528
	private Gun m_gun;
}
