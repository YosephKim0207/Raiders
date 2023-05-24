using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D39 RID: 3385
public class SackKnightAttackBehavior : AttackBehaviorBase
{
	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x06004774 RID: 18292 RVA: 0x00177008 File Offset: 0x00175208
	private float CurrentFormCooldown
	{
		get
		{
			switch (this.m_knight.CurrentForm)
			{
			case SackKnightController.SackKnightPhase.PEASANT:
			case SackKnightController.SackKnightPhase.SQUIRE:
				return this.SquireCooldownTime;
			case SackKnightController.SackKnightPhase.HEDGE_KNIGHT:
				return this.HedgeKnightCooldownTime;
			case SackKnightController.SackKnightPhase.KNIGHT:
				return this.KnightCooldownTime;
			case SackKnightController.SackKnightPhase.KNIGHT_LIEUTENANT:
				return this.KnightLieutenantCooldownTime;
			case SackKnightController.SackKnightPhase.KNIGHT_COMMANDER:
				return this.KnightCommanderCooldownTime;
			case SackKnightController.SackKnightPhase.HOLY_KNIGHT:
				return this.HolyKnightCooldownTime;
			case SackKnightController.SackKnightPhase.ANGELIC_KNIGHT:
				return this.AngelicKnightCooldownTime;
			case SackKnightController.SackKnightPhase.MECHAJUNKAN:
				return this.MechCooldownTime;
			default:
				return this.SquireCooldownTime;
			}
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x00177090 File Offset: 0x00175290
	public override void Start()
	{
		base.Start();
		this.m_knight = this.m_aiActor.GetComponent<SackKnightController>();
		BehaviorSpeculator behaviorSpeculator = this.m_aiActor.behaviorSpeculator;
		for (int i = 0; i < behaviorSpeculator.MovementBehaviors.Count; i++)
		{
			if (behaviorSpeculator.MovementBehaviors[i] is SeekTargetBehavior)
			{
				this.m_seekBehavior = behaviorSpeculator.MovementBehaviors[i] as SeekTargetBehavior;
			}
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x0017710C File Offset: 0x0017530C
	public override BehaviorResult Update()
	{
		base.Update();
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		if (targetRigidbody && targetRigidbody.aiActor && targetRigidbody.healthHaver && targetRigidbody.healthHaver.IsBoss)
		{
			this.m_isTargetPitBoss = GameManager.Instance.Dungeon.CellSupportsFalling(targetRigidbody.UnitCenter);
		}
		else
		{
			this.m_isTargetPitBoss = false;
		}
		if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
		{
			if (this.m_isTargetPitBoss)
			{
				this.minAttackDistance = 0.1f;
				this.maxAttackDistance = ((this.m_mechAttack != SackKnightAttackBehavior.MechaJunkanAttackType.SWORD) ? 12f : 2.5f);
			}
			else
			{
				this.minAttackDistance = 0.1f;
				this.maxAttackDistance = ((this.m_mechAttack != SackKnightAttackBehavior.MechaJunkanAttackType.SWORD) ? 12f : 1.5f);
			}
		}
		else if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
		{
			this.minAttackDistance = 0.1f;
			this.maxAttackDistance = 12f;
		}
		else if (this.m_isTargetPitBoss)
		{
			this.minAttackDistance = 0.1f;
			this.maxAttackDistance = 2f;
		}
		else
		{
			this.minAttackDistance = 0.1f;
			this.maxAttackDistance = 1f;
		}
		base.DecrementTimer(ref this.m_cooldownTimer, false);
		if (this.m_seekBehavior != null)
		{
			if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
			{
				this.m_seekBehavior.ExternalCooldownSource = false;
				this.m_seekBehavior.StopWhenInRange = true;
				this.m_seekBehavior.CustomRange = this.AngelicKnightDesiredDistance;
			}
			else
			{
				this.m_seekBehavior.ExternalCooldownSource = this.m_cooldownTimer > 0f;
				this.m_seekBehavior.StopWhenInRange = false;
				this.m_seekBehavior.CustomRange = -1f;
			}
		}
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_knight == null || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.PEASANT)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_cooldownTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 targetPoint = this.GetTargetPoint(this.m_aiActor.TargetRigidbody, unitCenter);
		float num = Vector2.Distance(unitCenter, targetPoint);
		bool flag = this.m_knight.CurrentForm != SackKnightController.SackKnightPhase.ANGELIC_KNIGHT || this.m_aiActor.HasLineOfSightToTarget;
		if (num < this.maxAttackDistance && flag)
		{
			this.m_state = SackKnightAttackBehavior.State.Charging;
			if (this.m_knight.CurrentForm != SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
			{
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = Vector2.zero;
			}
			this.m_updateEveryFrame = true;
			this.m_elapsed = 0f;
			this.m_attackCounter = 0;
			return (this.m_knight.CurrentForm != SackKnightController.SackKnightPhase.ANGELIC_KNIGHT) ? BehaviorResult.RunContinuous : BehaviorResult.RunContinuousInClass;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x00177434 File Offset: 0x00175634
	private Vector2 GetTargetPoint(SpeculativeRigidbody targetRigidbody, Vector2 myCenter)
	{
		PixelCollider hitboxPixelCollider = targetRigidbody.HitboxPixelCollider;
		return BraveMathCollege.ClosestPointOnRectangle(myCenter, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x0017745C File Offset: 0x0017565C
	private ContinuousBehaviorResult DoMechBlasters()
	{
		this.m_angelElapsed += BraveTime.DeltaTime;
		this.m_angelShootElapsed += BraveTime.DeltaTime;
		if (!this.m_aiAnimator.IsPlaying("fire"))
		{
			this.m_aiAnimator.PlayUntilCancelled("fire", true, null, -1f, false);
		}
		if (this.m_angelShootElapsed > 0.1f)
		{
			if (this.m_aiActor.TargetRigidbody)
			{
				Vector2 unitCenter = this.m_aiActor.TargetRigidbody.UnitCenter;
				float num = BraveMathCollege.Atan2Degrees(unitCenter - this.m_aiActor.CenterPosition);
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = num;
				Vector2 vector = this.m_aiActor.transform.Find("gun").position;
				GameObject gameObject = this.m_aiActor.bulletBank.CreateProjectileFromBank(vector, num, "blaster", null, false, true, false);
				Vector2 vector2 = ((BraveMathCollege.AbsAngleBetween(this.m_aiAnimator.FacingDirection, 0f) <= 90f) ? new Vector2(1f, 0f) : new Vector2(-1f, 0f));
				this.m_aiAnimator.PlayVfx("mechGunVFX", new Vector2?(vector2), null, null);
				if (gameObject && gameObject.GetComponent<Projectile>() && this.m_aiShooter && this.m_aiShooter.PostProcessProjectile != null)
				{
					this.m_aiShooter.PostProcessProjectile(gameObject.GetComponent<Projectile>());
				}
				AkSoundEngine.SetSwitch("WPN_Guns", "Sack", this.m_knight.gameObject);
				AkSoundEngine.PostEvent("Play_WPN_gun_shot_01", this.m_knight.gameObject);
			}
			else
			{
				this.m_aiAnimator.LockFacingDirection = false;
			}
			this.m_angelShootElapsed -= 0.1f;
		}
		if (this.m_angelElapsed >= 2f)
		{
			this.m_cooldownTimer = this.CurrentFormCooldown;
			this.m_state = SackKnightAttackBehavior.State.Idle;
			this.m_aiAnimator.EndAnimationIf("fire");
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x001776A8 File Offset: 0x001758A8
	private ContinuousBehaviorResult DoMechRockets()
	{
		if (this.m_state == SackKnightAttackBehavior.State.Charging)
		{
			this.m_state = SackKnightAttackBehavior.State.Leaping;
			if (!this.m_aiActor.TargetRigidbody || !this.m_aiActor.TargetRigidbody.enabled)
			{
				this.m_state = SackKnightAttackBehavior.State.Idle;
				this.m_aiAnimator.LockFacingDirection = false;
				return ContinuousBehaviorResult.Finished;
			}
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			Vector2 vector = this.GetTargetPoint(this.m_aiActor.TargetRigidbody, unitCenter);
			float num = Vector2.Distance(unitCenter, vector);
			if (num > this.maxAttackDistance)
			{
				vector = unitCenter + (vector - unitCenter).normalized * this.maxAttackDistance;
				num = Vector2.Distance(unitCenter, vector);
			}
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.PlayUntilFinished("rocket", true, null, -1f, false);
		}
		else if (this.m_state == SackKnightAttackBehavior.State.Leaping)
		{
			this.m_elapsed += this.m_deltaTime;
			float num2 = 1f;
			this.m_angelShootElapsed += BraveTime.DeltaTime;
			if (this.m_angelShootElapsed > 0.1f)
			{
				if (this.m_aiActor.TargetRigidbody)
				{
					Vector2 unitCenter2 = this.m_aiActor.TargetRigidbody.UnitCenter;
					float num3 = BraveMathCollege.Atan2Degrees(unitCenter2 - this.m_aiActor.CenterPosition);
					num3 += UnityEngine.Random.Range(-this.AngelicKnightAngleVariance, this.AngelicKnightAngleVariance);
					Vector2 vector2 = this.m_aiActor.CenterPosition + new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), 0.75f);
					GameObject gameObject = this.m_aiActor.bulletBank.CreateProjectileFromBank(vector2, num3, "mechRocket", null, false, true, false);
					if (gameObject)
					{
						RobotechProjectile component = gameObject.GetComponent<RobotechProjectile>();
						component.Owner = this.m_aiActor.CompanionOwner;
						Vector2 vector3 = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-25, 25)) * Vector2.up;
						component.ForceCurveDirection(vector3, UnityEngine.Random.Range(0.04f, 0.06f));
						component.Ramp(4f, 0.5f);
						if (this.m_aiShooter && this.m_aiShooter.PostProcessProjectile != null)
						{
							this.m_aiShooter.PostProcessProjectile(gameObject.GetComponent<Projectile>());
						}
					}
					AkSoundEngine.SetSwitch("WPN_Guns", "Sack", this.m_knight.gameObject);
					AkSoundEngine.PostEvent("Play_WPN_gun_shot_01", this.m_knight.gameObject);
				}
				this.m_angelShootElapsed -= 0.1f;
			}
			if (this.m_elapsed >= num2)
			{
				this.m_cooldownTimer = this.CurrentFormCooldown;
				this.m_aiAnimator.LockFacingDirection = false;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001779C4 File Offset: 0x00175BC4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
		{
			if (this.m_mechAttack == SackKnightAttackBehavior.MechaJunkanAttackType.GUN)
			{
				return this.DoMechBlasters();
			}
			if (this.m_mechAttack == SackKnightAttackBehavior.MechaJunkanAttackType.ROCKET)
			{
				return this.DoMechRockets();
			}
		}
		if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
		{
			this.HandleAngelAttackFrame();
			if (this.m_angelElapsed >= this.AngelicKnightCooldownTime)
			{
				this.m_cooldownTimer = this.CurrentFormCooldown;
				this.m_state = SackKnightAttackBehavior.State.Idle;
				this.m_aiAnimator.EndAnimationIf("attack");
				return ContinuousBehaviorResult.Finished;
			}
		}
		else
		{
			if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
			{
				this.m_aiAnimator.LockFacingDirection = true;
			}
			if (this.m_state == SackKnightAttackBehavior.State.Charging)
			{
				this.m_state = SackKnightAttackBehavior.State.Leaping;
				if (!this.m_aiActor.TargetRigidbody || !this.m_aiActor.TargetRigidbody.enabled)
				{
					this.m_state = SackKnightAttackBehavior.State.Idle;
					this.m_aiAnimator.LockFacingDirection = false;
					return ContinuousBehaviorResult.Finished;
				}
				Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
				Vector2 vector = this.GetTargetPoint(this.m_aiActor.TargetRigidbody, unitCenter);
				float num = Vector2.Distance(unitCenter, vector);
				if (num > this.maxAttackDistance)
				{
					vector = unitCenter + (vector - unitCenter).normalized * this.maxAttackDistance;
					num = Vector2.Distance(unitCenter, vector);
				}
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = (vector - unitCenter).normalized * (num / 0.25f);
				float num2 = this.m_aiActor.BehaviorVelocity.ToAngle();
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = num2;
				if (this.m_isTargetPitBoss)
				{
					this.m_aiActor.BehaviorVelocity = Vector2.zero;
				}
				this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
				this.m_aiActor.DoDustUps = false;
				this.m_aiAnimator.PlayUntilFinished("attack", true, null, -1f, false);
				if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
				{
					string text = ((BraveMathCollege.AbsAngleBetween(this.m_aiAnimator.FacingDirection, 0f) <= 90f) ? "mechSwordR" : "mechSwordL");
					AIAnimator aiAnimator = this.m_aiAnimator;
					string text2 = text;
					Vector2? vector2 = new Vector2?(this.m_knight.transform.position.XY());
					aiAnimator.PlayVfx(text2, null, null, vector2);
				}
			}
			else if (this.m_state == SackKnightAttackBehavior.State.Leaping)
			{
				this.m_elapsed += this.m_deltaTime;
				float num3 = 0.25f;
				if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
				{
					num3 = 0.4f;
				}
				if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.KNIGHT_COMMANDER || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
				{
					num3 = 1.2f;
					if ((double)this.m_elapsed >= 0.7 && this.m_attackCounter < 1)
					{
						this.m_attackCounter = 1;
						this.DoAttack();
					}
					if (this.m_elapsed >= 0.95f && this.m_attackCounter < 2)
					{
						this.m_attackCounter = 2;
						this.DoAttack();
					}
				}
				if (this.m_elapsed >= num3)
				{
					this.m_cooldownTimer = this.CurrentFormCooldown;
					this.m_aiAnimator.LockFacingDirection = false;
					return ContinuousBehaviorResult.Finished;
				}
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x00177D60 File Offset: 0x00175F60
	private void HandleAngelAttackFrame()
	{
		this.m_angelElapsed += BraveTime.DeltaTime;
		this.m_angelShootElapsed += BraveTime.DeltaTime;
		if (!this.m_aiAnimator.IsPlaying("attack"))
		{
			this.m_aiAnimator.PlayUntilCancelled("attack", true, null, -1f, false);
		}
		if (this.m_angelShootElapsed > 0.1f)
		{
			if (this.m_aiActor.TargetRigidbody)
			{
				Vector2 unitCenter = this.m_aiActor.TargetRigidbody.UnitCenter;
				float num = BraveMathCollege.Atan2Degrees(unitCenter - this.m_aiActor.CenterPosition);
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = num;
				num += UnityEngine.Random.Range(-this.AngelicKnightAngleVariance, this.AngelicKnightAngleVariance);
				string text = ((BraveMathCollege.AbsAngleBetween(this.m_aiAnimator.FacingDirection, 0f) >= 90f) ? "left shoot point" : "right shoot point");
				Vector2 vector = this.m_aiActor.bulletBank.GetTransform(text).position + new Vector3(0f, (float)UnityEngine.Random.Range(-3, 4) / 16f);
				GameObject gameObject = this.m_aiActor.bulletBank.CreateProjectileFromBank(vector, num, "angel", null, false, true, false);
				if (gameObject && gameObject.GetComponent<Projectile>() && this.m_aiShooter && this.m_aiShooter.PostProcessProjectile != null)
				{
					this.m_aiShooter.PostProcessProjectile(gameObject.GetComponent<Projectile>());
				}
				AkSoundEngine.SetSwitch("WPN_Guns", "Sack", this.m_knight.gameObject);
				AkSoundEngine.PostEvent("Play_WPN_gun_shot_01", this.m_knight.gameObject);
			}
			else
			{
				this.m_aiAnimator.LockFacingDirection = false;
			}
			this.m_angelShootElapsed -= 0.1f;
		}
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x00177F68 File Offset: 0x00176168
	private void DoAttack()
	{
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		if (!targetRigidbody || !targetRigidbody.enabled || !targetRigidbody.CollideWithOthers)
		{
			return;
		}
		if (!targetRigidbody.healthHaver)
		{
			return;
		}
		if (targetRigidbody.aiActor && targetRigidbody.aiActor.IsGone)
		{
			return;
		}
		float num;
		switch (this.m_knight.CurrentForm)
		{
		case SackKnightController.SackKnightPhase.PEASANT:
		case SackKnightController.SackKnightPhase.SQUIRE:
			num = this.SquireAttackDamage;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.HEDGE_KNIGHT:
			num = this.HedgeKnightAttackDamage;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.KNIGHT:
			num = this.KnightAttackDamage;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.KNIGHT_LIEUTENANT:
			num = this.KnightLieutenantAttackDamage;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.KNIGHT_COMMANDER:
			num = this.KnightCommanderAttackDamage / 3f;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.HOLY_KNIGHT:
			num = this.HolyKnightAttackDamage / 3f;
			goto IL_10D;
		case SackKnightController.SackKnightPhase.MECHAJUNKAN:
			num = this.MechAttackDamage;
			goto IL_10D;
		}
		num = this.SquireAttackDamage;
		IL_10D:
		if (this.m_aiActor.CompanionOwner && PassiveItem.IsFlagSetForCharacter(this.m_aiActor.CompanionOwner, typeof(BattleStandardItem)))
		{
			num *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
		}
		if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.KNIGHT_COMMANDER || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
		{
			VFXPool vfxpool = null;
			if (!string.IsNullOrEmpty(this.SwordHitVFX))
			{
				AIAnimator.NamedVFXPool namedVFXPool = this.m_aiAnimator.OtherVFX.Find((AIAnimator.NamedVFXPool vfx) => vfx.name == this.SwordHitVFX);
				if (namedVFXPool != null)
				{
					vfxpool = namedVFXPool.vfxPool;
				}
			}
			Exploder.DoRadialDamage(num, this.m_aiActor.specRigidbody.UnitCenter, 2.5f, false, true, this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT, vfxpool);
		}
		else
		{
			targetRigidbody.healthHaver.ApplyDamage(num, this.m_aiActor.specRigidbody.Velocity, "Ser Junkan", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			if (this.m_aiActor.CompanionOwner.HasActiveBonusSynergy(CustomSynergyType.TRASHJUNKAN, false) && targetRigidbody.aiActor)
			{
				targetRigidbody.aiActor.ApplyEffect(this.PoisonEffectForTrashSynergy, 1f, null);
			}
			if (!string.IsNullOrEmpty(this.SwordHitVFX))
			{
				PixelCollider pixelCollider = targetRigidbody.GetPixelCollider(ColliderType.HitBox);
				Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(this.m_aiActor.CenterPosition, pixelCollider.UnitBottomLeft, pixelCollider.UnitDimensions);
				Vector2 vector2 = vector - this.m_aiActor.CenterPosition;
				if (vector2 != Vector2.zero)
				{
					vector += vector2.normalized * 0.1875f;
				}
				AIAnimator aiAnimator = this.m_aiAnimator;
				string swordHitVFX = this.SwordHitVFX;
				Vector2? vector3 = new Vector2?(vector);
				aiAnimator.PlayVfx(swordHitVFX, null, null, vector3);
			}
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x00178278 File Offset: 0x00176478
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_angelShootElapsed = 0f;
		this.m_angelElapsed = 0f;
		if (this.m_knight.CurrentForm != SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
		{
			if ((this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.KNIGHT_COMMANDER || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT) && this.m_attackCounter < 1)
			{
				this.DoAttack();
			}
			if ((this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.KNIGHT_COMMANDER || this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT) && this.m_attackCounter < 2)
			{
				this.DoAttack();
			}
			this.DoAttack();
		}
		else
		{
			this.m_aiAnimator.EndAnimation();
		}
		if (this.m_knight.CurrentForm == SackKnightController.SackKnightPhase.MECHAJUNKAN)
		{
			this.m_mechAttack = this.SelectNewMechAttack();
		}
		this.m_state = SackKnightAttackBehavior.State.Idle;
		if (!this.m_aiActor.IsFlying)
		{
			this.m_aiActor.PathableTiles = CellTypes.FLOOR;
		}
		this.m_aiActor.DoDustUps = true;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x0017839C File Offset: 0x0017659C
	private SackKnightAttackBehavior.MechaJunkanAttackType SelectNewMechAttack()
	{
		float num = this.MechGunWeight + this.MechRocketWeight + this.MechSwordWeight;
		float num2 = UnityEngine.Random.value * num;
		if (num2 < this.MechGunWeight)
		{
			return SackKnightAttackBehavior.MechaJunkanAttackType.GUN;
		}
		if (num2 < this.MechGunWeight + this.MechRocketWeight)
		{
			return SackKnightAttackBehavior.MechaJunkanAttackType.ROCKET;
		}
		return SackKnightAttackBehavior.MechaJunkanAttackType.SWORD;
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x001783EC File Offset: 0x001765EC
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x001783F0 File Offset: 0x001765F0
	public override float GetMinReadyRange()
	{
		return this.maxAttackDistance;
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x001783F8 File Offset: 0x001765F8
	public override float GetMaxRange()
	{
		return this.maxAttackDistance;
	}

	// Token: 0x04003A8B RID: 14987
	public float maxAttackDistance = 1f;

	// Token: 0x04003A8C RID: 14988
	public float minAttackDistance = 0.1f;

	// Token: 0x04003A8D RID: 14989
	public float SquireAttackDamage = 3f;

	// Token: 0x04003A8E RID: 14990
	public float HedgeKnightAttackDamage = 5f;

	// Token: 0x04003A8F RID: 14991
	public float KnightAttackDamage = 7f;

	// Token: 0x04003A90 RID: 14992
	public float KnightLieutenantAttackDamage = 7f;

	// Token: 0x04003A91 RID: 14993
	public float KnightCommanderAttackDamage = 7f;

	// Token: 0x04003A92 RID: 14994
	public float HolyKnightAttackDamage = 7f;

	// Token: 0x04003A93 RID: 14995
	public float MechAttackDamage = 20f;

	// Token: 0x04003A94 RID: 14996
	public float AngelicKnightAttackDuration = 5f;

	// Token: 0x04003A95 RID: 14997
	public float AngelicKnightAngleVariance = 30f;

	// Token: 0x04003A96 RID: 14998
	public float SquireCooldownTime = 3f;

	// Token: 0x04003A97 RID: 14999
	public float HedgeKnightCooldownTime = 1.75f;

	// Token: 0x04003A98 RID: 15000
	public float KnightCooldownTime = 0.5f;

	// Token: 0x04003A99 RID: 15001
	public float KnightLieutenantCooldownTime = 0.5f;

	// Token: 0x04003A9A RID: 15002
	public float KnightCommanderCooldownTime = 2f;

	// Token: 0x04003A9B RID: 15003
	public float HolyKnightCooldownTime = 2f;

	// Token: 0x04003A9C RID: 15004
	public float AngelicKnightCooldownTime = 1f;

	// Token: 0x04003A9D RID: 15005
	public float AngelicKnightDesiredDistance = 6f;

	// Token: 0x04003A9E RID: 15006
	public float MechCooldownTime = 2f;

	// Token: 0x04003A9F RID: 15007
	public float MechGunWeight = 1f;

	// Token: 0x04003AA0 RID: 15008
	public float MechRocketWeight = 1f;

	// Token: 0x04003AA1 RID: 15009
	public float MechSwordWeight = 1f;

	// Token: 0x04003AA2 RID: 15010
	public string SwordHitVFX;

	// Token: 0x04003AA3 RID: 15011
	public GameActorHealthEffect PoisonEffectForTrashSynergy;

	// Token: 0x04003AA4 RID: 15012
	private SackKnightAttackBehavior.MechaJunkanAttackType m_mechAttack;

	// Token: 0x04003AA5 RID: 15013
	private float m_angelShootElapsed;

	// Token: 0x04003AA6 RID: 15014
	private float m_angelElapsed;

	// Token: 0x04003AA7 RID: 15015
	private SeekTargetBehavior m_seekBehavior;

	// Token: 0x04003AA8 RID: 15016
	private SackKnightController m_knight;

	// Token: 0x04003AA9 RID: 15017
	private float m_elapsed;

	// Token: 0x04003AAA RID: 15018
	private int m_attackCounter;

	// Token: 0x04003AAB RID: 15019
	private float m_cooldownTimer;

	// Token: 0x04003AAC RID: 15020
	private SackKnightAttackBehavior.State m_state;

	// Token: 0x04003AAD RID: 15021
	private bool m_isTargetPitBoss;

	// Token: 0x02000D3A RID: 3386
	private enum MechaJunkanAttackType
	{
		// Token: 0x04003AAF RID: 15023
		SWORD,
		// Token: 0x04003AB0 RID: 15024
		GUN,
		// Token: 0x04003AB1 RID: 15025
		ROCKET
	}

	// Token: 0x02000D3B RID: 3387
	private enum State
	{
		// Token: 0x04003AB3 RID: 15027
		Idle,
		// Token: 0x04003AB4 RID: 15028
		Charging,
		// Token: 0x04003AB5 RID: 15029
		Leaping
	}
}
