using System;
using UnityEngine;

// Token: 0x02000D69 RID: 3433
public class TimedAttackBehavior : BasicAttackBehavior
{
	// Token: 0x0600487E RID: 18558 RVA: 0x001810AC File Offset: 0x0017F2AC
	public override void Start()
	{
		base.Start();
		this.AttackBehavior.Start();
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x001810C0 File Offset: 0x0017F2C0
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_runTimer, false);
		this.AttackBehavior.Upkeep();
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x001810E0 File Offset: 0x0017F2E0
	public override bool OverrideOtherBehaviors()
	{
		return this.AttackBehavior.OverrideOtherBehaviors();
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x001810F0 File Offset: 0x0017F2F0
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
		behaviorResult = this.AttackBehavior.Update();
		if (behaviorResult == BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (behaviorResult == BehaviorResult.RunContinuousInClass || behaviorResult == BehaviorResult.SkipRemainingClassBehaviors)
		{
			this.m_defaultBehaviorResult = BehaviorResult.RunContinuousInClass;
		}
		if (behaviorResult == BehaviorResult.RunContinuous || behaviorResult == BehaviorResult.SkipAllRemainingBehaviors)
		{
			this.m_defaultBehaviorResult = BehaviorResult.RunContinuous;
		}
		this.m_runChildContinuous = behaviorResult == BehaviorResult.RunContinuous || behaviorResult == BehaviorResult.RunContinuousInClass;
		this.m_runTimer = this.Duration;
		return this.m_defaultBehaviorResult;
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x00181180 File Offset: 0x0017F380
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_runChildContinuous)
		{
			ContinuousBehaviorResult continuousBehaviorResult = this.AttackBehavior.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.AttackBehavior.EndContinuousUpdate();
				this.m_runChildContinuous = false;
			}
			return (this.m_runTimer > 0f) ? ContinuousBehaviorResult.Continue : ContinuousBehaviorResult.Finished;
		}
		if (this.m_runTimer <= 0f)
		{
			return ContinuousBehaviorResult.Finished;
		}
		BehaviorResult behaviorResult = this.AttackBehavior.Update();
		this.m_runChildContinuous = behaviorResult == BehaviorResult.RunContinuous || behaviorResult == BehaviorResult.RunContinuousInClass;
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x00181208 File Offset: 0x0017F408
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_runChildContinuous)
		{
			this.AttackBehavior.EndContinuousUpdate();
			this.m_runChildContinuous = false;
		}
		this.UpdateCooldowns();
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00181234 File Offset: 0x0017F434
	public override void Destroy()
	{
		this.AttackBehavior.Destroy();
		base.Destroy();
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x00181248 File Offset: 0x0017F448
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		this.AttackBehavior.Init(gameObject, aiActor, aiShooter);
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x00181264 File Offset: 0x0017F464
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		this.AttackBehavior.SetDeltaTime(deltaTime);
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x0018127C File Offset: 0x0017F47C
	public override bool IsReady()
	{
		return base.IsReady() && this.AttackBehavior.IsReady();
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x00181298 File Offset: 0x0017F498
	public override bool UpdateEveryFrame()
	{
		return this.AttackBehavior.UpdateEveryFrame();
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x001812A8 File Offset: 0x0017F4A8
	public override bool IsOverridable()
	{
		return this.AttackBehavior.IsOverridable();
	}

	// Token: 0x04003C63 RID: 15459
	public float Duration;

	// Token: 0x04003C64 RID: 15460
	public BasicAttackBehavior AttackBehavior;

	// Token: 0x04003C65 RID: 15461
	private BehaviorResult m_defaultBehaviorResult;

	// Token: 0x04003C66 RID: 15462
	private bool m_runChildContinuous;

	// Token: 0x04003C67 RID: 15463
	private float m_runTimer;
}
