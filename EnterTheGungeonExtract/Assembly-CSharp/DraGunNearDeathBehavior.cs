using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DAE RID: 3502
[InspectorDropdownName("Bosses/DraGun/NearDeathBehavior")]
public class DraGunNearDeathBehavior : BasicAttackBehavior
{
	// Token: 0x06004A22 RID: 18978 RVA: 0x0018D0B0 File Offset: 0x0018B2B0
	public override void Start()
	{
		base.Start();
		this.m_dragun = this.m_aiActor.GetComponent<DraGunController>();
		this.m_head = this.m_dragun.head;
		this.m_headAnimator = this.m_head.aiAnimator;
		this.m_heartAutoAimTarget = this.m_dragun.GetComponentsInChildren<AutoAimTarget>(true)[0];
		this.m_heartAutoAimTarget.enabled = false;
		this.m_healthHaver = this.m_aiActor.healthHaver;
		this.m_hitEffectHandler = this.m_aiActor.hitEffectHandler;
		this.Attacks.Start();
	}

	// Token: 0x06004A23 RID: 18979 RVA: 0x0018D144 File Offset: 0x0018B344
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_state != DraGunNearDeathBehavior.State.Inactive)
		{
			base.DecrementTimer(ref this.m_timer, false);
		}
		this.Attacks.Upkeep();
	}

	// Token: 0x06004A24 RID: 18980 RVA: 0x0018D170 File Offset: 0x0018B370
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
		this.m_state = DraGunNearDeathBehavior.State.Attacking;
		this.EyesAnimator.PlayUntilFinished("eyes_idle", false, null, -1f, false);
		this.Attacks.Update();
		SilencerInstance.s_MaxRadiusLimiter = new float?(8f);
		this.m_healthHaver.IsVulnerable = false;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A25 RID: 18981 RVA: 0x0018D1E8 File Offset: 0x0018B3E8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == DraGunNearDeathBehavior.State.Attacking)
		{
			ContinuousBehaviorResult continuousBehaviorResult = this.Attacks.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.Attacks.EndContinuousUpdate();
				SilencerInstance.s_MaxRadiusLimiter = null;
				this.m_state = DraGunNearDeathBehavior.State.WeakIntro;
				this.EyesAnimator.PlayUntilFinished("eyes_out", false, null, -1f, false);
				this.m_aiAnimator.PlayUntilCancelled("weak_intro", false, null, -1f, false);
				this.LeftHandAnimator.PlayUntilCancelled("weak_idle", false, null, -1f, false);
				this.RightHandAnimator.PlayUntilCancelled("weak_idle", false, null, -1f, false);
				this.WingsAnimator.PlayUntilCancelled("weak_idle", false, null, -1f, false);
				this.m_head.OverrideDesiredPosition = new Vector2?(this.m_aiActor.specRigidbody.UnitCenter + new Vector2(-3f, 7f));
				this.m_heartAutoAimTarget.enabled = true;
			}
		}
		else if (this.m_state == DraGunNearDeathBehavior.State.WeakIntro)
		{
			if (!this.m_aiAnimator.IsPlaying("weak_intro"))
			{
				this.m_state = DraGunNearDeathBehavior.State.Weak;
				this.m_aiAnimator.PlayUntilCancelled("weak_idle", false, null, -1f, false);
				this.m_headAnimator.PlayUntilCancelled("weak_idle", false, null, -1f, false);
				this.m_healthHaver.IsVulnerable = true;
				this.m_hitEffectHandler.additionalHitEffects[0].chance = 1f;
				this.m_timer = this.DamageTime;
				if (this.m_dragun.MaybeConvertToGold())
				{
					this.m_timer = 100000f;
					this.m_aiActor.healthHaver.minimumHealth = 1f;
				}
			}
		}
		else if (this.m_state == DraGunNearDeathBehavior.State.Weak)
		{
			if (this.m_timer <= 0f)
			{
				this.m_state = DraGunNearDeathBehavior.State.WeakOutro;
				this.m_aiAnimator.PlayUntilCancelled("weak_outro", false, null, -1f, false);
				this.m_headAnimator.EndAnimation();
				this.EyesAnimator.PlayUntilFinished("eyes_idle", false, null, -1f, false);
				this.m_healthHaver.IsVulnerable = false;
				this.m_hitEffectHandler.additionalHitEffects[0].chance = 0f;
				this.m_head.OverrideDesiredPosition = null;
			}
		}
		else if (this.m_state == DraGunNearDeathBehavior.State.WeakOutro && !this.m_aiAnimator.IsPlaying("weak_outro"))
		{
			this.m_state = DraGunNearDeathBehavior.State.Attacking;
			this.m_aiAnimator.EndAnimation();
			this.LeftHandAnimator.EndAnimation();
			this.RightHandAnimator.EndAnimation();
			this.WingsAnimator.EndAnimation();
			this.Attacks.Update();
			this.m_heartAutoAimTarget.enabled = false;
			SilencerInstance.s_MaxRadiusLimiter = new float?(8f);
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A26 RID: 18982 RVA: 0x0018D4D8 File Offset: 0x0018B6D8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = DraGunNearDeathBehavior.State.Inactive;
		this.m_aiAnimator.EndAnimation();
		this.m_headAnimator.EndAnimation();
		this.LeftHandAnimator.EndAnimation();
		this.RightHandAnimator.EndAnimation();
		this.WingsAnimator.EndAnimation();
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
		this.m_heartAutoAimTarget.enabled = false;
		SilencerInstance.s_MaxRadiusLimiter = null;
	}

	// Token: 0x06004A27 RID: 18983 RVA: 0x0018D558 File Offset: 0x0018B758
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		this.Attacks.Init(gameObject, aiActor, aiShooter);
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x0018D574 File Offset: 0x0018B774
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		this.Attacks.SetDeltaTime(deltaTime);
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x0018D58C File Offset: 0x0018B78C
	public override bool IsReady()
	{
		return this.m_dragun.IsNearDeath && base.IsReady();
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x0018D5A8 File Offset: 0x0018B7A8
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x0018D5AC File Offset: 0x0018B7AC
	public override void OnActorPreDeath()
	{
		SilencerInstance.s_MaxRadiusLimiter = null;
		base.OnActorPreDeath();
		this.Attacks.OnActorPreDeath();
	}

	// Token: 0x04003EC9 RID: 16073
	public float DamageTime = 5f;

	// Token: 0x04003ECA RID: 16074
	public AIAnimator LeftHandAnimator;

	// Token: 0x04003ECB RID: 16075
	public AIAnimator RightHandAnimator;

	// Token: 0x04003ECC RID: 16076
	public AIAnimator WingsAnimator;

	// Token: 0x04003ECD RID: 16077
	public AIAnimator EyesAnimator;

	// Token: 0x04003ECE RID: 16078
	public AttackBehaviorGroup Attacks;

	// Token: 0x04003ECF RID: 16079
	private DraGunController m_dragun;

	// Token: 0x04003ED0 RID: 16080
	private DraGunHeadController m_head;

	// Token: 0x04003ED1 RID: 16081
	private AIAnimator m_headAnimator;

	// Token: 0x04003ED2 RID: 16082
	private AutoAimTarget m_heartAutoAimTarget;

	// Token: 0x04003ED3 RID: 16083
	private HealthHaver m_healthHaver;

	// Token: 0x04003ED4 RID: 16084
	private HitEffectHandler m_hitEffectHandler;

	// Token: 0x04003ED5 RID: 16085
	private DraGunNearDeathBehavior.State m_state;

	// Token: 0x04003ED6 RID: 16086
	private float m_timer;

	// Token: 0x02000DAF RID: 3503
	private enum State
	{
		// Token: 0x04003ED8 RID: 16088
		Inactive,
		// Token: 0x04003ED9 RID: 16089
		Attacking,
		// Token: 0x04003EDA RID: 16090
		WeakIntro,
		// Token: 0x04003EDB RID: 16091
		Weak,
		// Token: 0x04003EDC RID: 16092
		WeakOutro
	}
}
