using System;
using System.Collections;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;

// Token: 0x02001651 RID: 5713
public class InstantlyDamageAllProjectile : Projectile
{
	// Token: 0x06008560 RID: 34144 RVA: 0x00370964 File Offset: 0x0036EB64
	protected override void Move()
	{
		if (this.DoesWhiteFlash)
		{
			Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
		}
		if (this.DoesCameraFlash)
		{
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.125f, 0f, false, false);
			Pixelator.Instance.TimedFreezeFrame(0.25f, 0.125f);
		}
		if (this.DoesAmbientVFX && this.AmbientVFXTime > 0f && this.AmbientVFX != null)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleAmbientSpawnTime(base.transform.position, this.AmbientVFXTime));
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		absoluteRoom.ApplyActionToNearbyEnemies(base.transform.position.XY(), 100f, new Action<AIActor, float>(this.ProcessEnemy));
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = allProjectiles.Count - 1; i >= 0; i--)
		{
			Projectile projectile = allProjectiles[i];
			if (projectile && projectile.collidesWithProjectiles)
			{
				if (!projectile.collidesOnlyWithPlayerProjectiles || base.Owner is PlayerController)
				{
					BounceProjModifier component = projectile.GetComponent<BounceProjModifier>();
					if (component)
					{
						if (component.numberOfBounces <= 0)
						{
							projectile.DieInAir(false, true, true, false);
						}
						else
						{
							projectile.Direction *= -1f;
							float num = projectile.Direction.ToAngle();
							if (this.shouldRotate)
							{
								base.transform.rotation = Quaternion.Euler(0f, 0f, num);
							}
							projectile.Speed *= 1f - component.percentVelocityToLoseOnBounce;
							if (base.braveBulletScript && base.braveBulletScript.bullet != null)
							{
								base.braveBulletScript.bullet.Direction = num;
								base.braveBulletScript.bullet.Speed *= 1f - component.percentVelocityToLoseOnBounce;
							}
							component.Bounce(this, projectile.specRigidbody.UnitCenter, null);
						}
					}
				}
			}
		}
		base.DieInAir(false, true, true, false);
	}

	// Token: 0x06008561 RID: 34145 RVA: 0x00370BC0 File Offset: 0x0036EDC0
	protected void HandleAmbientVFXSpawn(Vector2 centerPoint, float radius)
	{
		if (this.AmbientVFX == null)
		{
			return;
		}
		bool flag = false;
		this.m_ambientTimer -= BraveTime.DeltaTime;
		if (this.m_ambientTimer <= 0f)
		{
			flag = true;
			this.m_ambientTimer = this.minTimeBetweenAmbientVFX;
		}
		if (flag)
		{
			Vector2 vector = centerPoint + UnityEngine.Random.insideUnitCircle * radius;
			SpawnManager.SpawnVFX(this.AmbientVFX, vector, Quaternion.identity);
		}
	}

	// Token: 0x06008562 RID: 34146 RVA: 0x00370C40 File Offset: 0x0036EE40
	protected IEnumerator HandleAmbientSpawnTime(Vector2 centerPoint, float remainingTime)
	{
		float elapsed = 0f;
		while (elapsed < remainingTime)
		{
			elapsed += BraveTime.DeltaTime;
			this.HandleAmbientVFXSpawn(centerPoint, 10f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008563 RID: 34147 RVA: 0x00370C6C File Offset: 0x0036EE6C
	public void ProcessEnemy(AIActor a, float b)
	{
		if (a && a.IsNormalEnemy && a.healthHaver && !a.IsGone)
		{
			if (base.Owner)
			{
				a.healthHaver.ApplyDamage(base.ModifiedDamage, Vector2.zero, base.OwnerName, this.damageTypes, DamageCategory.Normal, false, null, false);
			}
			else
			{
				a.healthHaver.ApplyDamage(base.ModifiedDamage, Vector2.zero, "projectile", this.damageTypes, DamageCategory.Normal, false, null, false);
			}
			if (this.DoesRadialSlow)
			{
				this.ApplySlowToEnemy(a);
			}
			if (this.AppliesStun && a.healthHaver.IsAlive && a.behaviorSpeculator && UnityEngine.Random.value < this.StunApplyChance)
			{
				a.behaviorSpeculator.Stun(this.AppliedStunDuration, true);
			}
			if (this.DamagedEnemyVFX != null)
			{
				a.PlayEffectOnActor(this.DamagedEnemyVFX, Vector3.zero, false, true, false);
			}
		}
	}

	// Token: 0x06008564 RID: 34148 RVA: 0x00370D90 File Offset: 0x0036EF90
	protected void ApplySlowToEnemy(AIActor target)
	{
		target.StartCoroutine(this.ProcessSlow(target));
	}

	// Token: 0x06008565 RID: 34149 RVA: 0x00370DA0 File Offset: 0x0036EFA0
	private IEnumerator ProcessSlow(AIActor target)
	{
		float elapsed = 0f;
		if (this.RadialSlowInTime > 0f)
		{
			while (elapsed < this.RadialSlowInTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / this.RadialSlowInTime;
				target.LocalTimeScale = Mathf.Lerp(1f, this.RadialSlowTimeModifier, t);
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.RadialSlowHoldTime > 0f)
		{
			while (elapsed < this.RadialSlowHoldTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = this.RadialSlowTimeModifier;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.RadialSlowOutTime > 0f)
		{
			while (elapsed < this.RadialSlowOutTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t2 = elapsed / this.RadialSlowOutTime;
				target.LocalTimeScale = Mathf.Lerp(this.RadialSlowTimeModifier, 1f, t2);
				yield return null;
			}
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x04008993 RID: 35219
	public bool DoesWhiteFlash;

	// Token: 0x04008994 RID: 35220
	public bool DoesCameraFlash;

	// Token: 0x04008995 RID: 35221
	public bool DoesAmbientVFX;

	// Token: 0x04008996 RID: 35222
	public float AmbientVFXTime;

	// Token: 0x04008997 RID: 35223
	public GameObject AmbientVFX;

	// Token: 0x04008998 RID: 35224
	public float minTimeBetweenAmbientVFX = 0.1f;

	// Token: 0x04008999 RID: 35225
	public GameObject DamagedEnemyVFX;

	// Token: 0x0400899A RID: 35226
	[Header("Radial Slow Options")]
	public bool DoesRadialSlow;

	// Token: 0x0400899B RID: 35227
	[ShowInInspectorIf("DoesRadialSlow", false)]
	public float RadialSlowInTime;

	// Token: 0x0400899C RID: 35228
	[ShowInInspectorIf("DoesRadialSlow", false)]
	public float RadialSlowHoldTime = 1f;

	// Token: 0x0400899D RID: 35229
	[ShowInInspectorIf("DoesRadialSlow", false)]
	public float RadialSlowOutTime = 0.5f;

	// Token: 0x0400899E RID: 35230
	[ShowInInspectorIf("DoesRadialSlow", false)]
	public float RadialSlowTimeModifier = 0.25f;

	// Token: 0x0400899F RID: 35231
	private float m_ambientTimer;
}
