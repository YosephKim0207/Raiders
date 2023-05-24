using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D3C RID: 3388
public class SackKnightShieldBehavior : AttackBehaviorBase
{
	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x06004784 RID: 18308 RVA: 0x00178448 File Offset: 0x00176648
	private float CurrentFormCooldown
	{
		get
		{
			switch (this.m_knight.CurrentForm)
			{
			case SackKnightController.SackKnightPhase.PEASANT:
			case SackKnightController.SackKnightPhase.SQUIRE:
			case SackKnightController.SackKnightPhase.HEDGE_KNIGHT:
			case SackKnightController.SackKnightPhase.KNIGHT:
			case SackKnightController.SackKnightPhase.KNIGHT_LIEUTENANT:
			case SackKnightController.SackKnightPhase.KNIGHT_COMMANDER:
				return 1f;
			case SackKnightController.SackKnightPhase.HOLY_KNIGHT:
				return this.HolyKnightCooldownTime;
			case SackKnightController.SackKnightPhase.ANGELIC_KNIGHT:
				return this.AngelicKnightCooldownTime;
			default:
				return 1f;
			}
		}
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x001784A8 File Offset: 0x001766A8
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

	// Token: 0x06004786 RID: 18310 RVA: 0x00178524 File Offset: 0x00176724
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x0017852C File Offset: 0x0017672C
	public override BehaviorResult Update()
	{
		base.Update();
		base.DecrementTimer(ref this.m_cooldownTimer, false);
		if (this.m_seekBehavior != null)
		{
			this.m_seekBehavior.ExternalCooldownSource = this.m_cooldownTimer <= 0f;
		}
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_knight == null || this.m_knight.CurrentForm != SackKnightController.SackKnightPhase.HOLY_KNIGHT)
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
		bool flag = this.CheckPlayerProjectileRadius();
		if (flag)
		{
			this.m_state = SackKnightShieldBehavior.State.Charging;
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_updateEveryFrame = true;
			this.m_elapsed = 0f;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x00178620 File Offset: 0x00176820
	private Projectile GetNearestEnemyProjectile(PlayerController player)
	{
		Vector2 centerPosition = this.m_aiActor.CompanionOwner.CenterPosition;
		float num = this.BlankRadius * this.BlankRadius;
		Projectile projectile = null;
		float num2 = num;
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile2 = StaticReferenceManager.AllProjectiles[i];
			if (projectile2 && projectile2.collidesWithPlayer && projectile2.specRigidbody)
			{
				if (!(projectile2.Owner is PlayerController))
				{
					float sqrMagnitude = (centerPosition - projectile2.specRigidbody.UnitCenter).sqrMagnitude;
					if (sqrMagnitude < num && sqrMagnitude < num2)
					{
						projectile = projectile2;
						num2 = sqrMagnitude;
					}
				}
			}
		}
		return projectile;
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x001786F8 File Offset: 0x001768F8
	private bool CheckPlayerProjectileRadius()
	{
		Vector2 centerPosition = this.m_aiActor.CompanionOwner.CenterPosition;
		float num = this.BlankRadius * this.BlankRadius;
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.collidesWithPlayer && projectile.specRigidbody)
			{
				if (!(projectile.Owner is PlayerController))
				{
					if ((centerPosition - projectile.specRigidbody.UnitCenter).sqrMagnitude < num)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x001787B0 File Offset: 0x001769B0
	private Vector2 GetTargetPoint(SpeculativeRigidbody targetRigidbody, Vector2 myCenter)
	{
		PixelCollider hitboxPixelCollider = targetRigidbody.HitboxPixelCollider;
		return BraveMathCollege.ClosestPointOnRectangle(myCenter, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x001787D8 File Offset: 0x001769D8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == SackKnightShieldBehavior.State.Charging)
		{
			Projectile nearestEnemyProjectile = this.GetNearestEnemyProjectile(this.m_aiActor.CompanionOwner);
			this.m_state = SackKnightShieldBehavior.State.Leaping;
			if (!nearestEnemyProjectile)
			{
				this.m_state = SackKnightShieldBehavior.State.Idle;
				return ContinuousBehaviorResult.Finished;
			}
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			Vector2 centerPosition = this.m_aiActor.CompanionOwner.CenterPosition;
			float num = Vector2.Distance(unitCenter, centerPosition);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = (centerPosition - unitCenter).normalized * (num / 0.25f);
			float num2 = this.m_aiActor.BehaviorVelocity.ToAngle();
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = num2;
			this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
			this.m_aiActor.DoDustUps = false;
			this.m_aiAnimator.PlayForDuration("shield", 0.5f, true, null, -1f, false);
		}
		else if (this.m_state == SackKnightShieldBehavior.State.Leaping)
		{
			this.m_elapsed += this.m_deltaTime;
			float num3 = 0.25f;
			if (this.m_elapsed >= num3)
			{
				this.m_cooldownTimer = this.CurrentFormCooldown;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x00178930 File Offset: 0x00176B30
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.DoMicroBlank();
		this.m_state = SackKnightShieldBehavior.State.Idle;
		this.m_aiActor.PathableTiles = CellTypes.FLOOR;
		this.m_aiActor.DoDustUps = true;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x00178988 File Offset: 0x00176B88
	private void DoMicroBlank()
	{
		if (this.BlankVFXPrefab == null)
		{
			this.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/BlankVFX_Ghost", ".prefab");
		}
		AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", this.m_aiActor.gameObject);
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		float num = 0.25f;
		if (this.m_aiActor.sprite && this.m_aiActor.sprite.spriteAnimator && this.m_aiActor.sprite.spriteAnimator.GetClipByName("owl_hoot") != null)
		{
			this.m_aiActor.sprite.spriteAnimator.PlayForDuration("owl_hoot", -1f, "owl_idle", false);
		}
		silencerInstance.TriggerSilencer(this.m_aiActor.specRigidbody.UnitCenter, 20f, this.BlankRadius, this.BlankVFXPrefab, 0f, 3f, 3f, 3f, 30f, 3f, num, this.m_aiActor.CompanionOwner, false, false);
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x00178AB4 File Offset: 0x00176CB4
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x00178AB8 File Offset: 0x00176CB8
	public override float GetMinReadyRange()
	{
		return -1f;
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x00178AC0 File Offset: 0x00176CC0
	public override float GetMaxRange()
	{
		return float.MaxValue;
	}

	// Token: 0x04003AB6 RID: 15030
	public float KnightCommanderCooldownTime = 10f;

	// Token: 0x04003AB7 RID: 15031
	public float HolyKnightCooldownTime = 5f;

	// Token: 0x04003AB8 RID: 15032
	public float AngelicKnightCooldownTime = 5f;

	// Token: 0x04003AB9 RID: 15033
	public float BlankRadius = 5f;

	// Token: 0x04003ABA RID: 15034
	private float m_cooldownTimer;

	// Token: 0x04003ABB RID: 15035
	private GameObject BlankVFXPrefab;

	// Token: 0x04003ABC RID: 15036
	private SeekTargetBehavior m_seekBehavior;

	// Token: 0x04003ABD RID: 15037
	private SackKnightController m_knight;

	// Token: 0x04003ABE RID: 15038
	private float m_elapsed;

	// Token: 0x04003ABF RID: 15039
	private SackKnightShieldBehavior.State m_state;

	// Token: 0x02000D3D RID: 3389
	private enum State
	{
		// Token: 0x04003AC1 RID: 15041
		Idle,
		// Token: 0x04003AC2 RID: 15042
		Charging,
		// Token: 0x04003AC3 RID: 15043
		Leaping
	}
}
