using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D40 RID: 3392
public class ShieldPlayerBehavior : AttackBehaviorBase
{
	// Token: 0x060047A6 RID: 18342 RVA: 0x00179098 File Offset: 0x00177298
	public override void Start()
	{
		base.Start();
		BehaviorSpeculator behaviorSpeculator = this.m_aiActor.behaviorSpeculator;
		for (int i = 0; i < behaviorSpeculator.MovementBehaviors.Count; i++)
		{
			if (behaviorSpeculator.MovementBehaviors[i] is SeekTargetBehavior)
			{
				this.m_seekBehavior = behaviorSpeculator.MovementBehaviors[i] as SeekTargetBehavior;
			}
		}
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x00179100 File Offset: 0x00177300
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x00179108 File Offset: 0x00177308
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
			this.m_state = ShieldPlayerBehavior.State.Charging;
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_updateEveryFrame = true;
			this.m_elapsed = 0f;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x001791D8 File Offset: 0x001773D8
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

	// Token: 0x060047AA RID: 18346 RVA: 0x001792B0 File Offset: 0x001774B0
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

	// Token: 0x060047AB RID: 18347 RVA: 0x00179368 File Offset: 0x00177568
	private Vector2 GetTargetPoint(SpeculativeRigidbody targetRigidbody, Vector2 myCenter)
	{
		PixelCollider hitboxPixelCollider = targetRigidbody.HitboxPixelCollider;
		return BraveMathCollege.ClosestPointOnRectangle(myCenter, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x00179390 File Offset: 0x00177590
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == ShieldPlayerBehavior.State.Charging)
		{
			Projectile nearestEnemyProjectile = this.GetNearestEnemyProjectile(this.m_aiActor.CompanionOwner);
			this.m_state = ShieldPlayerBehavior.State.Leaping;
			if (!nearestEnemyProjectile)
			{
				this.m_state = ShieldPlayerBehavior.State.Idle;
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
			if (this.AnimationTime <= 0f)
			{
				this.m_aiAnimator.PlayUntilFinished(this.AnimationName, true, null, -1f, false);
			}
			else
			{
				this.m_aiAnimator.PlayForDuration(this.AnimationName, this.AnimationTime, true, null, -1f, false);
			}
		}
		else if (this.m_state == ShieldPlayerBehavior.State.Leaping)
		{
			this.m_elapsed += this.m_deltaTime;
			float num3 = 0.25f;
			if (this.m_elapsed >= num3)
			{
				this.m_cooldownTimer = this.Cooldown;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x0017951C File Offset: 0x0017771C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.DoMicroBlank();
		this.m_state = ShieldPlayerBehavior.State.Idle;
		this.m_aiActor.PathableTiles = CellTypes.FLOOR;
		this.m_aiActor.DoDustUps = true;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x00179574 File Offset: 0x00177774
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
		silencerInstance.TriggerSilencer(this.m_aiActor.specRigidbody.UnitCenter, 20f, this.BlankRadius, this.BlankVFXPrefab, 0f, 3f, 3f, 3f, 30f, 3f, num, this.m_aiActor.CompanionOwner, false, false);
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x00179630 File Offset: 0x00177830
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x00179634 File Offset: 0x00177834
	public override float GetMinReadyRange()
	{
		return -1f;
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x0017963C File Offset: 0x0017783C
	public override float GetMaxRange()
	{
		return float.MaxValue;
	}

	// Token: 0x04003ACF RID: 15055
	public float BlankRadius = 5f;

	// Token: 0x04003AD0 RID: 15056
	public float Cooldown = 10f;

	// Token: 0x04003AD1 RID: 15057
	public string AnimationName = "block";

	// Token: 0x04003AD2 RID: 15058
	public float AnimationTime = 0.5f;

	// Token: 0x04003AD3 RID: 15059
	private float m_cooldownTimer;

	// Token: 0x04003AD4 RID: 15060
	private GameObject BlankVFXPrefab;

	// Token: 0x04003AD5 RID: 15061
	private SeekTargetBehavior m_seekBehavior;

	// Token: 0x04003AD6 RID: 15062
	private float m_elapsed;

	// Token: 0x04003AD7 RID: 15063
	private ShieldPlayerBehavior.State m_state;

	// Token: 0x02000D41 RID: 3393
	private enum State
	{
		// Token: 0x04003AD9 RID: 15065
		Idle,
		// Token: 0x04003ADA RID: 15066
		Charging,
		// Token: 0x04003ADB RID: 15067
		Leaping
	}
}
