using System;
using UnityEngine;

// Token: 0x02001405 RID: 5125
public class FireVolleyOnRollItem : PassiveItem
{
	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06007453 RID: 29779 RVA: 0x002E466C File Offset: 0x002E286C
	// (set) Token: 0x06007454 RID: 29780 RVA: 0x002E468C File Offset: 0x002E288C
	public ProjectileVolleyData ModVolley
	{
		get
		{
			if (this.m_modVolley == null)
			{
				return this.Volley;
			}
			return this.m_modVolley;
		}
		set
		{
			this.m_modVolley = value;
		}
	}

	// Token: 0x06007455 RID: 29781 RVA: 0x002E4698 File Offset: 0x002E2898
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnRollStarted += this.OnRollStarted;
	}

	// Token: 0x06007456 RID: 29782 RVA: 0x002E46C0 File Offset: 0x002E28C0
	private void OnRollStarted(PlayerController obj, Vector2 dirVec)
	{
		if (this.m_cooldown <= 0f)
		{
			if (GameManager.HasInstance && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				return;
			}
			this.FireVolley(obj, dirVec);
			this.m_cooldown = this.FireCooldown;
			if (!string.IsNullOrEmpty(this.AudioEvent))
			{
				AkSoundEngine.PostEvent(this.AudioEvent, base.gameObject);
			}
		}
	}

	// Token: 0x06007457 RID: 29783 RVA: 0x002E4730 File Offset: 0x002E2930
	protected override void Update()
	{
		base.Update();
		this.m_cooldown -= BraveTime.DeltaTime;
	}

	// Token: 0x06007458 RID: 29784 RVA: 0x002E474C File Offset: 0x002E294C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<FireVolleyOnRollItem>().m_pickedUpThisRun = true;
		player.OnRollStarted -= this.OnRollStarted;
		return debrisObject;
	}

	// Token: 0x06007459 RID: 29785 RVA: 0x002E4780 File Offset: 0x002E2980
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.OnRollStarted;
		}
		base.OnDestroy();
	}

	// Token: 0x0600745A RID: 29786 RVA: 0x002E47B0 File Offset: 0x002E29B0
	private void FireVolley(PlayerController p, Vector2 dirVec)
	{
		for (int i = 0; i < this.ModVolley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = this.ModVolley.projectiles[i];
			this.ShootSingleProjectile(p.CenterPosition, BraveMathCollege.Atan2Degrees(dirVec * -1f), projectileModule, 100f);
		}
	}

	// Token: 0x0600745B RID: 29787 RVA: 0x002E4818 File Offset: 0x002E2A18
	private void ShootSingleProjectile(Vector3 offset, float fireAngle, ProjectileModule mod, float chargeTime)
	{
		PlayerController owner = this.m_owner;
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
		if (GameManager.Instance.InTutorial && owner != null)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerFiredGun");
		}
		offset = new Vector3(offset.x, offset.y, -1f);
		float angleForShot = mod.GetAngleForShot(1f, 1f, null);
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, offset + Quaternion.Euler(0f, 0f, fireAngle) * mod.positionOffset, Quaternion.Euler(0f, 0f, fireAngle + angleForShot), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = this.m_owner;
		component.Shooter = this.m_owner.specRigidbody;
		component.Inverted = mod.inverted;
		if (this.m_owner.aiShooter)
		{
			component.collidesWithEnemies = this.m_owner.aiShooter.CanShootOtherEnemies;
		}
		if (this.m_owner != null)
		{
			PlayerStats stats = owner.stats;
			component.baseData.damage *= stats.GetStatValue(PlayerStats.StatType.Damage);
			component.baseData.speed *= stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
			component.baseData.force *= stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
			component.baseData.range *= stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
		}
		if (this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.METROID_CAN_CRAWL, false) && this.itemName == "Roll Bombs")
		{
			ExplosiveModifier component2 = component.GetComponent<ExplosiveModifier>();
			if (component2)
			{
				if (this.MetroidCrawlUsesCustomExplosion)
				{
					component2.explosionData = this.MetroidCrawlSynergyExplosion;
				}
				else
				{
					component2.explosionData.damage = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData.damage;
					component2.explosionData.damageRadius = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData.damageRadius;
					component2.explosionData.damageToPlayer = 0f;
					component2.explosionData.effect = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData.effect;
				}
			}
		}
		if (this.Volley != null && this.Volley.UsesShotgunStyleVelocityRandomizer)
		{
			component.baseData.speed *= this.Volley.GetVolleySpeedMod();
		}
		component.PlayerProjectileSourceGameTimeslice = Time.time;
		if (this.m_owner != null)
		{
			owner.DoPostProcessProjectile(component);
		}
		if (mod.mirror)
		{
			gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, offset + Quaternion.Euler(0f, 0f, fireAngle) * mod.InversePositionOffset, Quaternion.Euler(0f, 0f, fireAngle - angleForShot), true);
			Projectile component3 = gameObject.GetComponent<Projectile>();
			component3.Inverted = true;
			component3.Owner = this.m_owner;
			component3.Shooter = this.m_owner.specRigidbody;
			if (this.m_owner.aiShooter)
			{
				component3.collidesWithEnemies = this.m_owner.aiShooter.CanShootOtherEnemies;
			}
			component3.PlayerProjectileSourceGameTimeslice = Time.time;
			if (this.m_owner != null)
			{
				owner.DoPostProcessProjectile(component3);
			}
			component3.baseData.SetAll(component.baseData);
			component3.IsCritical = component.IsCritical;
		}
	}

	// Token: 0x040075F6 RID: 30198
	public float FireCooldown = 2f;

	// Token: 0x040075F7 RID: 30199
	public ProjectileVolleyData Volley;

	// Token: 0x040075F8 RID: 30200
	public string AudioEvent;

	// Token: 0x040075F9 RID: 30201
	public bool MetroidCrawlUsesCustomExplosion;

	// Token: 0x040075FA RID: 30202
	public ExplosionData MetroidCrawlSynergyExplosion;

	// Token: 0x040075FB RID: 30203
	private ProjectileVolleyData m_modVolley;

	// Token: 0x040075FC RID: 30204
	private float m_cooldown;

	// Token: 0x040075FD RID: 30205
	private PlayerController m_player;
}
