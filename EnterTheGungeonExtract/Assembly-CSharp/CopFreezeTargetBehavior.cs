using System;
using UnityEngine;

// Token: 0x02000D1D RID: 3357
public class CopFreezeTargetBehavior : BasicAttackBehavior
{
	// Token: 0x060046E3 RID: 18147 RVA: 0x00171F28 File Offset: 0x00170128
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x00171F30 File Offset: 0x00170130
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x00171F38 File Offset: 0x00170138
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
		if (this.m_aiActor.TargetRigidbody == null)
		{
			return BehaviorResult.Continue;
		}
		float num = Vector2.Distance(this.m_aiActor.CenterPosition, this.m_aiActor.TargetRigidbody.UnitCenter);
		if (num > this.FreezeRadius)
		{
			return BehaviorResult.Continue;
		}
		if (num < 2f)
		{
			return BehaviorResult.Continue;
		}
		this.DoFreeze();
		this.m_aiAnimator.FacingDirection = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox)).ToAngle();
		this.m_aiAnimator.LockFacingDirection = true;
		if (!this.m_aiAnimator.IsPlaying("freeze"))
		{
			this.m_aiAnimator.PlayUntilCancelled("freeze", false, null, -1f, false);
		}
		this.m_aiActor.ClearPath();
		this.m_freezeTimer = this.FreezeDelayTime;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x00172044 File Offset: 0x00170244
	private void DoFreeze()
	{
		if (this.m_aiActor.TargetRigidbody.aiActor && !this.m_aiActor.TargetRigidbody.aiActor.IsFrozen)
		{
			Debug.Log("DOING COP FREEZE");
			this.m_aiActor.TargetRigidbody.aiActor.ApplyEffect(this.FreezeEffect, 1f, null);
		}
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x001720B0 File Offset: 0x001702B0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		base.DecrementTimer(ref this.m_freezeTimer, false);
		if (this.m_freezeTimer <= 0f || this.m_aiActor.TargetRigidbody == null)
		{
			this.m_aiAnimator.EndAnimationIf("freeze");
			return ContinuousBehaviorResult.Finished;
		}
		this.DoFreeze();
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x00172114 File Offset: 0x00170314
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunRenderers(true, "ShootBulletScript");
		}
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiAnimator.EndAnimation();
		this.UpdateCooldowns();
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x00172168 File Offset: 0x00170368
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
	}

	// Token: 0x040039A9 RID: 14761
	public float FreezeRadius = 7f;

	// Token: 0x040039AA RID: 14762
	public float FreezeDelayTime = 2f;

	// Token: 0x040039AB RID: 14763
	public GameActorFreezeEffect FreezeEffect;

	// Token: 0x040039AC RID: 14764
	private float m_freezeTimer;
}
