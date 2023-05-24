using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D70 RID: 3440
public class WolfCompanionAttackBehavior : AttackBehaviorBase
{
	// Token: 0x060048AF RID: 18607 RVA: 0x00182DC8 File Offset: 0x00180FC8
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (this.leadAmount > 0f)
		{
			Vector2 vector2 = vector + this.m_aiActor.TargetRigidbody.specRigidbody.Velocity * 0.75f;
			vector = Vector2.Lerp(vector, vector2, this.leadAmount);
		}
		float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, vector);
		if (num > this.minLeapDistance && num < this.leapDistance)
		{
			this.m_state = WolfCompanionAttackBehavior.State.Charging;
			this.m_aiAnimator.PlayForDuration(this.chargeAnim, this.maximumChargeTime, true, null, -1f, false);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x00182EE4 File Offset: 0x001810E4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == WolfCompanionAttackBehavior.State.Charging)
		{
			if (!this.m_aiAnimator.IsPlaying(this.chargeAnim))
			{
				this.m_state = WolfCompanionAttackBehavior.State.Leaping;
				if (!this.m_aiActor.TargetRigidbody || !this.m_aiActor.TargetRigidbody.enabled)
				{
					this.m_state = WolfCompanionAttackBehavior.State.Idle;
					return ContinuousBehaviorResult.Finished;
				}
				Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
				Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				if (this.leadAmount > 0f)
				{
					Vector2 vector2 = vector + this.m_aiActor.TargetRigidbody.specRigidbody.Velocity * 0.75f;
					vector = Vector2.Lerp(vector, vector2, this.leadAmount);
				}
				float num = Vector2.Distance(unitCenter, vector);
				if (num > this.maxTravelDistance)
				{
					vector = unitCenter + (vector - unitCenter).normalized * this.maxTravelDistance;
					num = Vector2.Distance(unitCenter, vector);
				}
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = (vector - unitCenter).normalized * (num / this.leapTime);
				float num2 = this.m_aiActor.BehaviorVelocity.ToAngle();
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = num2;
				this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
				this.m_aiActor.DoDustUps = false;
				this.m_aiAnimator.PlayUntilFinished(this.leapAnim, true, null, -1f, false);
			}
		}
		else if (this.m_state == WolfCompanionAttackBehavior.State.Leaping)
		{
			this.m_elapsed += this.m_deltaTime;
			if (this.m_elapsed >= this.leapTime)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x001830CC File Offset: 0x001812CC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_aiActor.TargetRigidbody && this.m_aiActor.TargetRigidbody.healthHaver)
		{
			this.m_aiActor.TargetRigidbody.healthHaver.ApplyDamage(5f, this.m_aiActor.specRigidbody.Velocity, "Wolf", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			if (this.m_aiActor.CompanionOwner && this.m_aiActor.CompanionOwner.HasActiveBonusSynergy(this.DebuffSynergy, false))
			{
				this.m_aiActor.TargetRigidbody.aiActor.ApplyEffect(this.EnemyDebuff, 1f, null);
			}
		}
		this.m_state = WolfCompanionAttackBehavior.State.Idle;
		this.m_aiActor.PathableTiles = CellTypes.FLOOR;
		this.m_aiActor.DoDustUps = true;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x001831D4 File Offset: 0x001813D4
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x001831D8 File Offset: 0x001813D8
	public override float GetMinReadyRange()
	{
		return this.leapDistance;
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x001831E0 File Offset: 0x001813E0
	public override float GetMaxRange()
	{
		return this.leapDistance;
	}

	// Token: 0x04003CAD RID: 15533
	public float minLeapDistance = 1f;

	// Token: 0x04003CAE RID: 15534
	public float leapDistance = 4f;

	// Token: 0x04003CAF RID: 15535
	public float maxTravelDistance = 5f;

	// Token: 0x04003CB0 RID: 15536
	public float leadAmount;

	// Token: 0x04003CB1 RID: 15537
	public float leapTime = 0.75f;

	// Token: 0x04003CB2 RID: 15538
	public float maximumChargeTime = 0.25f;

	// Token: 0x04003CB3 RID: 15539
	public string chargeAnim;

	// Token: 0x04003CB4 RID: 15540
	public string leapAnim;

	// Token: 0x04003CB5 RID: 15541
	[LongNumericEnum]
	public CustomSynergyType DebuffSynergy;

	// Token: 0x04003CB6 RID: 15542
	public AIActorDebuffEffect EnemyDebuff;

	// Token: 0x04003CB7 RID: 15543
	private float m_elapsed;

	// Token: 0x04003CB8 RID: 15544
	private WolfCompanionAttackBehavior.State m_state;

	// Token: 0x02000D71 RID: 3441
	private enum State
	{
		// Token: 0x04003CBA RID: 15546
		Idle,
		// Token: 0x04003CBB RID: 15547
		Charging,
		// Token: 0x04003CBC RID: 15548
		Leaping
	}
}
