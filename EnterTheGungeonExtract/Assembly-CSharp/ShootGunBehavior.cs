using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D4E RID: 3406
public class ShootGunBehavior : BasicAttackBehavior
{
	// Token: 0x060047EF RID: 18415 RVA: 0x0017B4B0 File Offset: 0x001796B0
	private bool IsAiShooter()
	{
		return this.WeaponType == WeaponType.AIShooterProjectile;
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x0017B4BC File Offset: 0x001796BC
	private bool IsBulletScript()
	{
		return this.WeaponType == WeaponType.BulletScript;
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0017B4C8 File Offset: 0x001796C8
	private bool IsComplexBullet()
	{
		return this.WeaponType != WeaponType.AIShooterProjectile;
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x0017B4D8 File Offset: 0x001796D8
	private bool ShowLeadChance()
	{
		return this.LeadAmount != 0f;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x0017B4EC File Offset: 0x001796EC
	private bool ShowTimeBetweenShots()
	{
		return this.RespectReload && this.EmptiesClip;
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x0017B504 File Offset: 0x00179704
	public override void Start()
	{
		base.Start();
		this.m_remainingAmmo = this.MagazineCapacity;
		if (this.UseLaserSight)
		{
			if (this.UseGreenLaser)
			{
				this.m_aiActor.CurrentGun.LaserSightIsGreen = true;
			}
			this.m_aiActor.CurrentGun.ForceLaserSight = true;
		}
		Gun gun = PickupObjectDatabase.GetById(this.m_aiShooter.equippedGunId) as Gun;
		if (gun && !string.IsNullOrEmpty(gun.enemyPreFireAnimation))
		{
			tk2dSpriteAnimationClip clipByName = gun.spriteAnimator.GetClipByName(gun.enemyPreFireAnimation);
			this.m_preFireTime = clipByName.BaseClipLength;
		}
		if (this.UseLaserSight)
		{
			PhysicsEngine.Instance.OnPostRigidbodyMovement += this.OnPostRigidbodyMovement;
		}
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x0017B5CC File Offset: 0x001797CC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_nextShotTimer, false);
		base.DecrementTimer(ref this.m_reloadTimer, false);
		base.DecrementTimer(ref this.m_prefireLaserTimer, false);
		this.m_timeSinceLastShot += this.m_deltaTime;
		if (this.UseLaserSight && !this.m_laserSight && this.m_aiActor && this.m_aiActor.CurrentGun && this.m_aiActor.CurrentGun.LaserSight)
		{
			this.m_laserSight = this.m_aiActor.CurrentGun.LaserSight.GetComponent<LaserSightController>();
			if (this.PreFireLaserTime > 0f && this.m_state != ShootGunBehavior.State.PreFireLaser)
			{
				this.m_laserSight.renderer.enabled = false;
			}
		}
		if (this.AimAtFacingDirectionWhenSafe && this.m_behaviorSpeculator.TargetRigidbody == null)
		{
			this.m_aiShooter.AimInDirection(BraveMathCollege.DegreesToVector(this.m_aiAnimator.FacingDirection, 1f));
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x0017B6FC File Offset: 0x001798FC
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (this.m_behaviorSpeculator.TargetRigidbody == null)
		{
			return BehaviorResult.Continue;
		}
		bool flag = this.RespectReload && this.m_reloadTimer > 0f;
		bool flag2 = this.EmptiesClip && this.m_remainingAmmo < this.MagazineCapacity;
		bool flag3 = this.Range > 0f && this.m_aiActor.DistanceToTarget > this.Range && !flag2;
		bool flag4 = this.LineOfSight && !this.m_aiActor.HasLineOfSightToTarget && !flag2;
		if (flag || this.m_aiActor.TargetRigidbody == null || flag3 || flag4)
		{
			this.m_aiShooter.CeaseAttack();
			return BehaviorResult.Continue;
		}
		this.BeginAttack();
		if (this.PreventTargetSwitching)
		{
			this.m_aiActor.SuppressTargetSwitch = true;
		}
		this.m_updateEveryFrame = true;
		return (!this.StopDuringAttack) ? BehaviorResult.RunContinuousInClass : BehaviorResult.RunContinuous;
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x0017B834 File Offset: 0x00179A34
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == ShootGunBehavior.State.Idle)
		{
			return ContinuousBehaviorResult.Finished;
		}
		bool flag = this.LeadAmount > 0f && this.LeadChance >= 1f;
		if (this.m_state == ShootGunBehavior.State.PreFireLaser && this.UseLaserSight && this.m_prefireLaserTimer > 0f)
		{
			flag = false;
		}
		if (this.m_aiShooter.CurrentGun != null && this.m_aiActor.TargetRigidbody)
		{
			Vector2 vector = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			if (flag)
			{
				Vector2 vector2 = this.FindPredictedTargetPosition();
				vector = Vector2.Lerp(vector, vector2, this.LeadAmount);
			}
			this.m_aiShooter.OverrideAimPoint = new Vector2?(vector);
		}
		if (this.m_state == ShootGunBehavior.State.WaitingForNextShot)
		{
			if (this.m_nextShotTimer <= 0f)
			{
				this.BeginAttack();
			}
		}
		else if (this.m_state == ShootGunBehavior.State.PreFireLaser)
		{
			if (this.UseLaserSight && this.m_laserSight && this.PreFireLaserTime > 0f)
			{
				this.m_laserSight.renderer.enabled = true;
				this.m_laserSight.UpdateCountdown(this.m_prefireLaserTimer, this.PreFireLaserTime);
			}
			if (this.m_prefireLaserTimer <= 0f)
			{
				if (this.UseLaserSight && this.m_laserSight && this.PreFireLaserTime > 0f)
				{
					this.m_laserSight.ResetCountdown();
				}
				this.m_state = ShootGunBehavior.State.PreFire;
				this.m_aiShooter.StartPreFireAnim();
			}
		}
		else if (this.m_state == ShootGunBehavior.State.PreFire)
		{
			if (this.m_aiShooter.IsPreFireComplete)
			{
				this.Fire();
			}
		}
		else if (this.m_state == ShootGunBehavior.State.Firing && this.IsBulletSourceEnded())
		{
			if (this.FixTargetDuringAttack)
			{
				this.m_aiActor.bulletBank.FixedPlayerPosition = null;
			}
			if (!this.RespectReload || !this.EmptiesClip || this.m_reloadTimer > 0f)
			{
				return ContinuousBehaviorResult.Finished;
			}
			this.m_state = ShootGunBehavior.State.WaitingForNextShot;
			this.m_nextShotTimer = ((this.TimeBetweenShots <= 0f) ? this.Cooldown : this.TimeBetweenShots);
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x0017BAAC File Offset: 0x00179CAC
	public override void EndContinuousUpdate()
	{
		this.m_updateEveryFrame = false;
		this.m_state = ShootGunBehavior.State.Idle;
		if (this.HideGun)
		{
			this.m_aiShooter.ToggleGunRenderers(true, "ShootGunBehavior");
		}
		this.m_aiShooter.OverrideAimPoint = null;
		if (this.FixTargetDuringAttack)
		{
			this.m_aiActor.bulletBank.FixedPlayerPosition = null;
		}
		if (this.PreventTargetSwitching)
		{
			this.m_aiActor.SuppressTargetSwitch = false;
		}
		if (!string.IsNullOrEmpty(this.OverrideDirectionalAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.OverrideDirectionalAnimation);
		}
		else if (!string.IsNullOrEmpty(this.OverrideAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.OverrideAnimation);
		}
		if (this.UseLaserSight && this.m_laserSight)
		{
			this.m_laserSight.ResetCountdown();
		}
		this.UpdateCooldowns();
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x0017BBA8 File Offset: 0x00179DA8
	public override bool IsReady()
	{
		return base.IsReady() && (!this.RespectReload || this.m_reloadTimer <= 0f);
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x0017BBD8 File Offset: 0x00179DD8
	protected override void UpdateCooldowns()
	{
		base.UpdateCooldowns();
		if (this.GroupCooldownVariance > 0f)
		{
			List<AIActor> activeEnemies = this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (!(activeEnemies[i] == this.m_aiActor))
				{
					if ((activeEnemies[i].specRigidbody.UnitCenter - this.m_aiActor.specRigidbody.UnitCenter).sqrMagnitude < 6.25f)
					{
						this.m_cooldownTimer += UnityEngine.Random.value * this.GroupCooldownVariance;
						break;
					}
				}
			}
		}
		if (this.m_preFireTime < this.Cooldown)
		{
			this.m_cooldownTimer = Mathf.Max(0f, this.m_cooldownTimer - this.m_preFireTime);
		}
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x0017BCC4 File Offset: 0x00179EC4
	private Vector2 FindPredictedTargetPosition()
	{
		AIBulletBank.Entry bulletEntry = this.m_aiShooter.GetBulletEntry(this.OverrideBulletName);
		float num;
		if (bulletEntry.OverrideProjectile)
		{
			num = bulletEntry.ProjectileData.speed;
		}
		else
		{
			num = bulletEntry.BulletObject.GetComponent<Projectile>().baseData.speed;
		}
		if (num < 0f)
		{
			num = float.MaxValue;
		}
		Vector2 unitCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 targetVelocity = this.m_aiActor.TargetVelocity;
		Vector2 unitCenter2 = this.m_aiActor.specRigidbody.UnitCenter;
		return BraveMathCollege.GetPredictedPosition(unitCenter, targetVelocity, unitCenter2, num);
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x0017BD68 File Offset: 0x00179F68
	private bool IsBulletSourceEnded()
	{
		return this.WeaponType != WeaponType.BulletScript || this.m_aiShooter.BraveBulletSource.IsEnded;
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x0017BD88 File Offset: 0x00179F88
	private void BeginAttack()
	{
		if (this.UseLaserSight && this.PreFireLaserTime > 0f)
		{
			this.m_state = ShootGunBehavior.State.PreFireLaser;
			this.m_prefireLaserTimer = this.PreFireLaserTime;
		}
		else if (this.ShouldPreFire)
		{
			this.m_state = ShootGunBehavior.State.PreFire;
			this.m_aiShooter.StartPreFireAnim();
		}
		else
		{
			this.Fire();
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x060047FE RID: 18430 RVA: 0x0017BDF0 File Offset: 0x00179FF0
	private bool ShouldPreFire
	{
		get
		{
			return this.m_preFireTime < this.Cooldown || this.m_timeSinceLastShot > this.Cooldown * 2f;
		}
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x0017BE20 File Offset: 0x0017A020
	private void Fire()
	{
		this.m_timeSinceLastShot = 0f;
		WeaponType weaponType = this.WeaponType;
		if (weaponType != WeaponType.AIShooterProjectile)
		{
			if (weaponType == WeaponType.BulletScript)
			{
				this.m_aiShooter.ShootBulletScript(this.BulletScript);
			}
		}
		else
		{
			this.HandleAIShoot();
		}
		if (this.RespectReload)
		{
			this.m_remainingAmmo -= 1f;
			if (this.m_remainingAmmo == 0f)
			{
				this.m_remainingAmmo = this.MagazineCapacity;
				this.m_reloadTimer = this.ReloadSpeed;
				if (!this.SuppressReloadAnim)
				{
					this.m_aiShooter.Reload();
				}
			}
		}
		if (!string.IsNullOrEmpty(this.OverrideDirectionalAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.OverrideDirectionalAnimation, true, null, -1f, false);
		}
		else if (!string.IsNullOrEmpty(this.OverrideAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.OverrideAnimation, false, null, -1f, false);
		}
		if (this.IsComplexBullet())
		{
			if (this.StopDuringAttack)
			{
				this.m_aiActor.ClearPath();
			}
			if (this.FixTargetDuringAttack && this.m_aiActor.TargetRigidbody)
			{
				this.m_aiActor.bulletBank.FixedPlayerPosition = new Vector2?(this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
			}
			if (this.HideGun)
			{
				this.m_aiShooter.ToggleGunRenderers(false, "ShootGunBehavior");
			}
			this.m_state = ShootGunBehavior.State.Firing;
		}
		else if (this.RespectReload && this.EmptiesClip && this.m_reloadTimer <= 0f)
		{
			this.m_state = ShootGunBehavior.State.WaitingForNextShot;
			this.m_nextShotTimer = ((this.TimeBetweenShots <= 0f) ? this.Cooldown : this.TimeBetweenShots);
		}
		else
		{
			this.m_state = ShootGunBehavior.State.Idle;
		}
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x0017C018 File Offset: 0x0017A218
	private void HandleAIShoot()
	{
		if (this.LeadAmount <= 0f || (this.LeadChance < 1f && UnityEngine.Random.value >= this.LeadChance))
		{
			this.m_aiShooter.ShootAtTarget(this.OverrideBulletName);
		}
		else
		{
			if (!this.m_aiActor.TargetRigidbody)
			{
				return;
			}
			PixelCollider pixelCollider = this.m_aiActor.TargetRigidbody.GetPixelCollider(ColliderType.HitBox);
			Vector2 vector = ((pixelCollider == null) ? this.m_aiActor.TargetRigidbody.UnitCenter : pixelCollider.UnitCenter);
			Vector2 vector2 = this.FindPredictedTargetPosition();
			Vector2 vector3 = Vector2.Lerp(vector, vector2, this.LeadAmount);
			if (this.m_aiShooter.CurrentGun == null)
			{
				this.m_aiShooter.ShootInDirection(vector3 - this.m_aiShooter.specRigidbody.UnitCenter, null);
			}
			else
			{
				this.m_aiShooter.OverrideAimPoint = new Vector2?(vector3);
				this.m_aiShooter.AimAtPoint(vector3);
				this.m_aiShooter.Shoot(this.OverrideBulletName);
				this.m_aiShooter.OverrideAimPoint = null;
			}
		}
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x0017C158 File Offset: 0x0017A358
	private void OnPostRigidbodyMovement()
	{
		if (this.m_state == ShootGunBehavior.State.PreFireLaser && this.UseLaserSight && this.m_prefireLaserTimer > 0f && this.m_aiShooter.CurrentGun != null && this.m_aiActor.TargetRigidbody)
		{
			this.m_aiShooter.OverrideAimPoint = new Vector2?(this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
			this.m_aiShooter.AimAtOverride();
		}
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x0017C1E4 File Offset: 0x0017A3E4
	public override void OnActorPreDeath()
	{
		if (this.UseLaserSight && PhysicsEngine.HasInstance)
		{
			PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.OnPostRigidbodyMovement;
		}
		base.OnActorPreDeath();
	}

	// Token: 0x04003B58 RID: 15192
	[InspectorCategory("Conditions")]
	public float GroupCooldownVariance = 0.2f;

	// Token: 0x04003B59 RID: 15193
	[InspectorCategory("Conditions")]
	public bool LineOfSight = true;

	// Token: 0x04003B5A RID: 15194
	public WeaponType WeaponType;

	// Token: 0x04003B5B RID: 15195
	[InspectorIndent]
	[InspectorShowIf("IsAiShooter")]
	public string OverrideBulletName;

	// Token: 0x04003B5C RID: 15196
	[InspectorShowIf("IsBulletScript")]
	[InspectorIndent]
	public BulletScriptSelector BulletScript;

	// Token: 0x04003B5D RID: 15197
	[InspectorShowIf("IsComplexBullet")]
	[InspectorIndent]
	public bool FixTargetDuringAttack;

	// Token: 0x04003B5E RID: 15198
	public bool StopDuringAttack;

	// Token: 0x04003B5F RID: 15199
	public float LeadAmount;

	// Token: 0x04003B60 RID: 15200
	[InspectorShowIf("ShowLeadChance")]
	[InspectorIndent]
	public float LeadChance = 1f;

	// Token: 0x04003B61 RID: 15201
	public bool RespectReload;

	// Token: 0x04003B62 RID: 15202
	[InspectorIndent]
	[InspectorShowIf("RespectReload")]
	public float MagazineCapacity = 1f;

	// Token: 0x04003B63 RID: 15203
	[InspectorShowIf("RespectReload")]
	[InspectorIndent]
	public float ReloadSpeed = 1f;

	// Token: 0x04003B64 RID: 15204
	[InspectorIndent]
	[InspectorShowIf("RespectReload")]
	public bool EmptiesClip = true;

	// Token: 0x04003B65 RID: 15205
	[InspectorIndent]
	[InspectorShowIf("RespectReload")]
	public bool SuppressReloadAnim;

	// Token: 0x04003B66 RID: 15206
	[InspectorIndent]
	[InspectorShowIf("ShowTimeBetweenShots")]
	public float TimeBetweenShots = -1f;

	// Token: 0x04003B67 RID: 15207
	public bool PreventTargetSwitching;

	// Token: 0x04003B68 RID: 15208
	[InspectorCategory("Visuals")]
	public string OverrideAnimation;

	// Token: 0x04003B69 RID: 15209
	[InspectorCategory("Visuals")]
	public string OverrideDirectionalAnimation;

	// Token: 0x04003B6A RID: 15210
	[InspectorShowIf("IsComplexBullet")]
	[InspectorCategory("Visuals")]
	public bool HideGun;

	// Token: 0x04003B6B RID: 15211
	[InspectorCategory("Visuals")]
	public bool UseLaserSight;

	// Token: 0x04003B6C RID: 15212
	[InspectorShowIf("UseLaserSight")]
	[InspectorCategory("Visuals")]
	public bool UseGreenLaser;

	// Token: 0x04003B6D RID: 15213
	[InspectorShowIf("UseLaserSight")]
	[InspectorCategory("Visuals")]
	public float PreFireLaserTime = -1f;

	// Token: 0x04003B6E RID: 15214
	[InspectorCategory("Visuals")]
	public bool AimAtFacingDirectionWhenSafe;

	// Token: 0x04003B6F RID: 15215
	private ShootGunBehavior.State m_state;

	// Token: 0x04003B70 RID: 15216
	private LaserSightController m_laserSight;

	// Token: 0x04003B71 RID: 15217
	private float m_remainingAmmo;

	// Token: 0x04003B72 RID: 15218
	private float m_reloadTimer;

	// Token: 0x04003B73 RID: 15219
	private float m_prefireLaserTimer;

	// Token: 0x04003B74 RID: 15220
	private float m_nextShotTimer;

	// Token: 0x04003B75 RID: 15221
	private float m_preFireTime;

	// Token: 0x04003B76 RID: 15222
	private float m_timeSinceLastShot;

	// Token: 0x02000D4F RID: 3407
	private enum State
	{
		// Token: 0x04003B78 RID: 15224
		Idle,
		// Token: 0x04003B79 RID: 15225
		PreFireLaser,
		// Token: 0x04003B7A RID: 15226
		PreFire,
		// Token: 0x04003B7B RID: 15227
		Firing,
		// Token: 0x04003B7C RID: 15228
		WaitingForNextShot
	}
}
