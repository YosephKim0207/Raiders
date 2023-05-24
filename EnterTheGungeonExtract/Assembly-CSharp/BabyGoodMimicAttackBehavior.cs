using System;
using UnityEngine;

// Token: 0x02000D13 RID: 3347
public class BabyGoodMimicAttackBehavior : BasicAttackBehavior
{
	// Token: 0x06004695 RID: 18069 RVA: 0x0016EE44 File Offset: 0x0016D044
	public override void Start()
	{
		base.Start();
		HealthHaver healthHaver = this.m_aiActor.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
	}

	// Token: 0x06004696 RID: 18070 RVA: 0x0016EE78 File Offset: 0x0016D078
	private void ModifyIncomingDamage(HealthHaver health, HealthHaver.ModifyDamageEventArgs damageArgs)
	{
		this.m_wasDamaged = true;
		damageArgs.ModifiedDamage = 0f;
	}

	// Token: 0x06004697 RID: 18071 RVA: 0x0016EE8C File Offset: 0x0016D08C
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004698 RID: 18072 RVA: 0x0016EE94 File Offset: 0x0016D094
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_aiActor && this.m_aiAnimator && this.m_aiActor.CompanionOwner && this.m_aiActor.CompanionOwner.IsInCombat)
		{
			this.m_aiAnimator.OverrideIdleAnimation = "mimic";
		}
		else if (this.m_aiAnimator)
		{
			this.m_aiAnimator.OverrideIdleAnimation = string.Empty;
		}
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_wasDamaged)
		{
			return BehaviorResult.Continue;
		}
		this.m_wasDamaged = false;
		this.UpdateCooldowns();
		this.m_continuousElapsed = 0f;
		this.m_continuousShotTimer = 0f;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004699 RID: 18073 RVA: 0x0016EF7C File Offset: 0x0016D17C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_continuousElapsed > (float)this.NumberOfAttacks * this.TimeBetweenAttacks)
		{
			return ContinuousBehaviorResult.Finished;
		}
		this.m_continuousShotTimer -= BraveTime.DeltaTime;
		if (this.m_continuousShotTimer <= 0f)
		{
			Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
			this.m_aiAnimator.FacingDirection = normalized.ToAngle();
			if (this.m_aiAnimator != null)
			{
				this.m_aiAnimator.PlayUntilFinished(this.AttackAnimationName, true, null, -1f, false);
			}
			this.ShootVFX.SpawnAtPosition(this.m_aiActor.CenterPosition, normalized.ToAngle(), null, null, null, null, false, null, null, false);
			VolleyUtility.FireVolley(this.Volley, this.m_aiActor.CenterPosition, normalized, this.m_aiActor.CompanionOwner, true);
			this.m_continuousShotTimer += this.TimeBetweenAttacks;
		}
		this.m_continuousElapsed += BraveTime.DeltaTime;
		return base.ContinuousUpdate();
	}

	// Token: 0x0600469A RID: 18074 RVA: 0x0016F0A0 File Offset: 0x0016D2A0
	public override void EndContinuousUpdate()
	{
		this.m_updateEveryFrame = false;
		this.m_continuousShotTimer = 0f;
		this.m_continuousElapsed = 0f;
		if (this.m_aiAnimator)
		{
			this.m_aiAnimator.EndAnimationIf(this.AttackAnimationName);
		}
		base.EndContinuousUpdate();
	}

	// Token: 0x04003928 RID: 14632
	public string AttackAnimationName = "attack";

	// Token: 0x04003929 RID: 14633
	public ProjectileVolleyData Volley;

	// Token: 0x0400392A RID: 14634
	public float TimeBetweenAttacks = 0.25f;

	// Token: 0x0400392B RID: 14635
	public int NumberOfAttacks = 10;

	// Token: 0x0400392C RID: 14636
	public VFXPool ShootVFX;

	// Token: 0x0400392D RID: 14637
	private bool m_wasDamaged;

	// Token: 0x0400392E RID: 14638
	private float m_continuousShotTimer;

	// Token: 0x0400392F RID: 14639
	private float m_continuousElapsed;
}
