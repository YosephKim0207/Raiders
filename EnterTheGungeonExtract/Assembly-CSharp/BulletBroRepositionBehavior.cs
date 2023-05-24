using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D9F RID: 3487
[InspectorDropdownName("Bosses/BulletBro/RepositionBehavior")]
public class BulletBroRepositionBehavior : BasicAttackBehavior
{
	// Token: 0x060049D7 RID: 18903 RVA: 0x0018AD28 File Offset: 0x00188F28
	public override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		this.m_cachedKnockback = this.m_aiActor.CollisionKnockbackStrength;
		this.m_cachedDamage = this.m_aiActor.CollisionDamage;
		this.m_cachedVfx = this.m_aiActor.CollisionVFX;
		this.m_cachedPathableTiles = this.m_aiActor.PathableTiles;
		this.m_cachedDoDustUps = this.m_aiActor.DoDustUps;
		this.m_otherBro = BroController.GetOtherBro(this.m_aiActor.gameObject).aiActor;
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x0018ADD8 File Offset: 0x00188FD8
	public override void Upkeep()
	{
		base.Upkeep();
		if (BulletBroRepositionBehavior.s_staticCooldown > 0f && BulletBroRepositionBehavior.s_lastStaticUpdateFrameNum != Time.frameCount)
		{
			BulletBroRepositionBehavior.s_staticCooldown = Mathf.Max(0f, BulletBroRepositionBehavior.s_staticCooldown - this.m_deltaTime);
			BulletBroRepositionBehavior.s_lastStaticUpdateFrameNum = Time.frameCount;
		}
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x0018AE30 File Offset: 0x00189030
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.m_otherBro)
		{
			return BehaviorResult.Continue;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		Vector2 unitCenter = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 unitCenter2 = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 unitCenter3 = this.m_otherBro.specRigidbody.UnitCenter;
		float num = (unitCenter2 - unitCenter).ToAngle();
		float num2 = (unitCenter3 - unitCenter).ToAngle();
		if (BraveMathCollege.AbsAngleBetween(num, num2) < this.triggerAngle)
		{
			Vector2 vector = unitCenter - unitCenter3;
			this.m_targetCenter = unitCenter3 + vector + vector.normalized * 7f;
			this.m_lastAngleToTarget = (this.m_targetCenter - unitCenter2).ToAngle();
			this.State = BulletBroRepositionBehavior.FireState.Priming;
			BulletBroRepositionBehavior.s_staticCooldown += this.StaticCooldown;
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		this.m_cachedTargetCenter = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		return BehaviorResult.Continue;
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x0018AF74 File Offset: 0x00189174
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.State == BulletBroRepositionBehavior.FireState.Priming)
		{
			if (!this.m_aiAnimator.IsPlaying(this.primeAnim))
			{
				if (!this.m_aiActor.TargetRigidbody)
				{
					return ContinuousBehaviorResult.Finished;
				}
				this.State = BulletBroRepositionBehavior.FireState.Charging;
			}
		}
		else if (this.State == BulletBroRepositionBehavior.FireState.Charging)
		{
			Vector2 cachedTargetCenter = this.m_cachedTargetCenter;
			if (this.m_aiActor.TargetRigidbody)
			{
				this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			if (this.m_otherBro)
			{
				Vector2 unitCenter2 = this.m_otherBro.specRigidbody.UnitCenter;
				Vector2 vector = cachedTargetCenter - unitCenter2;
				this.m_targetCenter = unitCenter2 + vector + vector.normalized * 7f;
			}
			float num = (this.m_targetCenter - unitCenter).ToAngle();
			if (BraveMathCollege.AbsAngleBetween(num, this.m_lastAngleToTarget) > 135f)
			{
				return ContinuousBehaviorResult.Finished;
			}
			this.m_aiActor.BehaviorVelocity = (this.m_targetCenter - unitCenter).normalized * this.chargeSpeed;
			this.m_aiAnimator.FacingDirection = this.m_aiActor.BehaviorVelocity.ToAngle();
			this.m_lastAngleToTarget = num;
		}
		else if (this.State == BulletBroRepositionBehavior.FireState.Bouncing && !this.m_aiAnimator.IsPlaying(this.hitAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x0018B108 File Offset: 0x00189308
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.State = BulletBroRepositionBehavior.FireState.Idle;
		this.UpdateCooldowns();
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x0018B124 File Offset: 0x00189324
	public override bool IsReady()
	{
		return base.IsReady() && BulletBroRepositionBehavior.s_staticCooldown <= 0f;
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x0018B144 File Offset: 0x00189344
	private void OnCollision(CollisionData collisionData)
	{
		if (this.State == BulletBroRepositionBehavior.FireState.Charging && !this.m_aiActor.healthHaver.IsDead)
		{
			this.State = BulletBroRepositionBehavior.FireState.Bouncing;
		}
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x060049DE RID: 18910 RVA: 0x0018B170 File Offset: 0x00189370
	// (set) Token: 0x060049DF RID: 18911 RVA: 0x0018B178 File Offset: 0x00189378
	private BulletBroRepositionBehavior.FireState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x0018B1A8 File Offset: 0x001893A8
	private void BeginState(BulletBroRepositionBehavior.FireState state)
	{
		if (state == BulletBroRepositionBehavior.FireState.Idle)
		{
			if (this.HideGun)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "BulletBroRepositionBehavior");
			}
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_aiAnimator.LockFacingDirection = false;
		}
		else if (state == BulletBroRepositionBehavior.FireState.Priming)
		{
			if (this.HideGun)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "BulletBroRepositionBehavior");
			}
			this.m_aiAnimator.PlayUntilFinished(this.primeAnim, true, null, -1f, false);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
		}
		else if (state == BulletBroRepositionBehavior.FireState.Charging)
		{
			if (this.HideGun)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "BulletBroRepositionBehavior");
			}
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = (this.m_targetCenter - this.m_aiActor.specRigidbody.UnitCenter).normalized * this.chargeSpeed;
			float num = this.m_aiActor.BehaviorVelocity.ToAngle();
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = num;
			this.m_aiActor.CollisionKnockbackStrength = this.chargeKnockback;
			this.m_aiActor.CollisionDamage = this.chargeDamage;
			if (this.hitVfx)
			{
				VFXObject vfxobject = new VFXObject();
				vfxobject.effect = this.hitVfx;
				VFXComplex vfxcomplex = new VFXComplex();
				vfxcomplex.effects = new VFXObject[] { vfxobject };
				VFXPool vfxpool = new VFXPool();
				vfxpool.type = VFXPoolType.Single;
				vfxpool.effects = new VFXComplex[] { vfxcomplex };
				this.m_aiActor.CollisionVFX = vfxpool;
			}
			this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
			this.m_aiActor.DoDustUps = false;
			this.m_aiAnimator.PlayUntilFinished(this.chargeAnim, true, null, -1f, false);
			if (this.launchVfx)
			{
				SpawnManager.SpawnVFX(this.launchVfx, this.m_aiActor.specRigidbody.UnitCenter, Quaternion.identity);
			}
			if (this.trailVfx)
			{
				this.m_trailVfx = SpawnManager.SpawnParticleSystem(this.trailVfx, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, num));
				if (this.trailVfxParent)
				{
					this.m_trailVfx.transform.parent = this.trailVfxParent;
				}
				else
				{
					this.m_trailVfx.transform.parent = this.m_aiActor.transform;
				}
				ParticleKiller component = this.m_trailVfx.GetComponent<ParticleKiller>();
				if (component != null)
				{
					component.Awake();
				}
			}
			this.m_aiActor.specRigidbody.ForceRegenerate(null, null);
		}
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x0018B4BC File Offset: 0x001896BC
	private void EndState(BulletBroRepositionBehavior.FireState state)
	{
		if (state == BulletBroRepositionBehavior.FireState.Charging)
		{
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilFinished(this.hitAnim, true, null, -1f, false);
			this.m_aiActor.CollisionKnockbackStrength = this.m_cachedKnockback;
			this.m_aiActor.CollisionDamage = this.m_cachedDamage;
			this.m_aiActor.CollisionVFX = this.m_cachedVfx;
			if (this.m_trailVfx)
			{
				ParticleKiller component = this.m_trailVfx.GetComponent<ParticleKiller>();
				if (component)
				{
					component.StopEmitting();
				}
				else
				{
					SpawnManager.Despawn(this.m_trailVfx);
				}
				this.m_trailVfx = null;
			}
			this.m_aiActor.DoDustUps = this.m_cachedDoDustUps;
			this.m_aiActor.PathableTiles = this.m_cachedPathableTiles;
			this.m_aiAnimator.EndAnimationIf(this.chargeAnim);
		}
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x0018B5A8 File Offset: 0x001897A8
	private void TestTargetPosition(IntVector2 testPos, float broAngleToTarget, Vector2 targetCenter, ref IntVector2? targetPos, ref float targetAngleFromBro)
	{
		float num = BraveMathCollege.AbsAngleBetween(broAngleToTarget, (testPos.ToCenterVector2() - targetCenter).ToAngle());
		if (num > targetAngleFromBro)
		{
			targetPos = new IntVector2?(testPos);
			targetAngleFromBro = num;
		}
	}

	// Token: 0x04003E39 RID: 15929
	public float triggerAngle = 30f;

	// Token: 0x04003E3A RID: 15930
	public string primeAnim;

	// Token: 0x04003E3B RID: 15931
	public string chargeAnim;

	// Token: 0x04003E3C RID: 15932
	public string hitAnim;

	// Token: 0x04003E3D RID: 15933
	public float chargeSpeed;

	// Token: 0x04003E3E RID: 15934
	public float chargeKnockback = 50f;

	// Token: 0x04003E3F RID: 15935
	public float chargeDamage = 0.5f;

	// Token: 0x04003E40 RID: 15936
	public bool HideGun;

	// Token: 0x04003E41 RID: 15937
	public GameObject launchVfx;

	// Token: 0x04003E42 RID: 15938
	public GameObject trailVfx;

	// Token: 0x04003E43 RID: 15939
	public Transform trailVfxParent;

	// Token: 0x04003E44 RID: 15940
	public GameObject hitVfx;

	// Token: 0x04003E45 RID: 15941
	[InspectorCategory("Conditions")]
	public float StaticCooldown;

	// Token: 0x04003E46 RID: 15942
	private BulletBroRepositionBehavior.FireState m_state;

	// Token: 0x04003E47 RID: 15943
	private AIActor m_otherBro;

	// Token: 0x04003E48 RID: 15944
	private Vector2 m_targetCenter;

	// Token: 0x04003E49 RID: 15945
	private float m_lastAngleToTarget;

	// Token: 0x04003E4A RID: 15946
	private float m_cachedKnockback;

	// Token: 0x04003E4B RID: 15947
	private float m_cachedDamage;

	// Token: 0x04003E4C RID: 15948
	private VFXPool m_cachedVfx;

	// Token: 0x04003E4D RID: 15949
	private CellTypes m_cachedPathableTiles;

	// Token: 0x04003E4E RID: 15950
	private bool m_cachedDoDustUps;

	// Token: 0x04003E4F RID: 15951
	private GameObject m_trailVfx;

	// Token: 0x04003E50 RID: 15952
	private Vector2 m_cachedTargetCenter;

	// Token: 0x04003E51 RID: 15953
	private static float s_staticCooldown;

	// Token: 0x04003E52 RID: 15954
	private static int s_lastStaticUpdateFrameNum = -1;

	// Token: 0x02000DA0 RID: 3488
	private enum FireState
	{
		// Token: 0x04003E54 RID: 15956
		Idle,
		// Token: 0x04003E55 RID: 15957
		Priming,
		// Token: 0x04003E56 RID: 15958
		Charging,
		// Token: 0x04003E57 RID: 15959
		Bouncing
	}
}
