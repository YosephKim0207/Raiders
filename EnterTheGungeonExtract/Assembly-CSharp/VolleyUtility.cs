using System;
using UnityEngine;

// Token: 0x0200184E RID: 6222
public static class VolleyUtility
{
	// Token: 0x0600932B RID: 37675 RVA: 0x003E1CAC File Offset: 0x003DFEAC
	public static void FireVolley(ProjectileVolleyData sourceVolley, Vector2 shootPoint, Vector2 aimDirection, GameActor possibleOwner = null, bool treatedAsNonProjectileForChallenge = false)
	{
		for (int i = 0; i < sourceVolley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = sourceVolley.projectiles[i];
			VolleyUtility.ShootSingleProjectile(projectileModule, sourceVolley, shootPoint, BraveMathCollege.Atan2Degrees(aimDirection), 0f, possibleOwner, treatedAsNonProjectileForChallenge);
		}
	}

	// Token: 0x0600932C RID: 37676 RVA: 0x003E1CF8 File Offset: 0x003DFEF8
	public static void ShootSingleProjectile(ProjectileModule mod, ProjectileVolleyData volley, Vector2 shootPoint, float fireAngle, float chargeTime, GameActor possibleOwner = null, bool treatedAsNonProjectileForChallenge = false)
	{
		Projectile projectile = null;
		if (mod.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			ProjectileModule.ChargeProjectile chargeProjectile = mod.GetChargeProjectile(chargeTime);
			if (chargeProjectile != null)
			{
				projectile = chargeProjectile.Projectile;
				projectile.pierceMinorBreakables = true;
			}
		}
		else
		{
			projectile = mod.GetCurrentProjectile();
		}
		if (!projectile)
		{
			if (mod.shootStyle != ProjectileModule.ShootStyle.Charged)
			{
				mod.IncrementShootCount();
			}
			return;
		}
		float angleForShot = mod.GetAngleForShot(1f, 1f, null);
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, shootPoint.ToVector3ZisY(0f) + Quaternion.Euler(0f, 0f, fireAngle) * mod.positionOffset, Quaternion.Euler(0f, 0f, fireAngle + angleForShot), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (possibleOwner)
		{
			component.Owner = possibleOwner;
			component.Shooter = possibleOwner.specRigidbody;
		}
		if (treatedAsNonProjectileForChallenge)
		{
			component.TreatedAsNonProjectileForChallenge = true;
		}
		component.Inverted = mod.inverted;
		if (volley != null && volley.UsesShotgunStyleVelocityRandomizer)
		{
			component.baseData.speed *= volley.GetVolleySpeedMod();
		}
		component.PlayerProjectileSourceGameTimeslice = Time.time;
		if (possibleOwner is PlayerController)
		{
			(possibleOwner as PlayerController).DoPostProcessProjectile(component);
		}
		if (mod.mirror)
		{
			gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, shootPoint.ToVector3ZisY(0f) + Quaternion.Euler(0f, 0f, fireAngle) * mod.InversePositionOffset, Quaternion.Euler(0f, 0f, fireAngle - angleForShot), true);
			Projectile component2 = gameObject.GetComponent<Projectile>();
			component2.Inverted = true;
			if (possibleOwner)
			{
				component2.Owner = possibleOwner;
				component2.Shooter = possibleOwner.specRigidbody;
				if (possibleOwner.aiShooter)
				{
					component2.collidesWithEnemies = possibleOwner.aiShooter.CanShootOtherEnemies;
				}
			}
			if (treatedAsNonProjectileForChallenge)
			{
				component2.TreatedAsNonProjectileForChallenge = true;
			}
			component2.PlayerProjectileSourceGameTimeslice = Time.time;
			if (possibleOwner is PlayerController)
			{
				(possibleOwner as PlayerController).DoPostProcessProjectile(component2);
			}
			component2.baseData.SetAll(component.baseData);
			component2.IsCritical = component.IsCritical;
		}
	}

	// Token: 0x0600932D RID: 37677 RVA: 0x003E1F64 File Offset: 0x003E0164
	public static Projectile ShootSingleProjectile(Projectile currentProjectile, Vector2 shootPoint, float fireAngle, bool inverted, GameActor possibleOwner = null)
	{
		float num = 0f;
		GameObject gameObject = SpawnManager.SpawnProjectile(currentProjectile.gameObject, shootPoint.ToVector3ZisY(0f), Quaternion.Euler(0f, 0f, fireAngle + num), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (possibleOwner)
		{
			component.Owner = possibleOwner;
			component.Shooter = possibleOwner.specRigidbody;
		}
		component.Inverted = inverted;
		component.PlayerProjectileSourceGameTimeslice = Time.time;
		if (possibleOwner is PlayerController)
		{
			(possibleOwner as PlayerController).DoPostProcessProjectile(component);
		}
		return component;
	}
}
