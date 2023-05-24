using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DD6 RID: 3542
[InspectorDropdownName("Bosses/TankTreader/ChargeBehavior")]
public class TankTreaderChargeBehavior : BasicAttackBehavior
{
	// Token: 0x06004B0A RID: 19210 RVA: 0x001957B0 File Offset: 0x001939B0
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
		this.m_cachedDustUpInterval = this.m_aiActor.DustUpInterval;
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x00195858 File Offset: 0x00193A58
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x00195860 File Offset: 0x00193A60
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
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
		float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, unitCenter);
		if (num > this.minRange)
		{
			PixelCollider hitboxPixelCollider = this.m_aiActor.TargetRigidbody.specRigidbody.HitboxPixelCollider;
			PixelCollider groundPixelCollider = this.m_aiActor.specRigidbody.GroundPixelCollider;
			bool flag = hitboxPixelCollider.UnitRight < groundPixelCollider.UnitLeft;
			bool flag2 = hitboxPixelCollider.UnitLeft > groundPixelCollider.UnitRight;
			bool flag3 = hitboxPixelCollider.UnitBottom > groundPixelCollider.UnitTop;
			bool flag4 = hitboxPixelCollider.UnitTop < groundPixelCollider.UnitBottom;
			Vector2 vector = Vector2.zero;
			if (flag && !flag4 && !flag3)
			{
				vector = -Vector2.right;
			}
			else if (flag2 && !flag4 && !flag3)
			{
				vector = Vector2.right;
			}
			else if (flag3 && !flag && !flag2)
			{
				vector = Vector2.up;
			}
			else if (flag4 && !flag && !flag2)
			{
				vector = -Vector2.up;
			}
			if (vector != Vector2.zero)
			{
				float num2 = BraveMathCollege.AbsAngleBetween(vector.ToAngle(), this.m_aiAnimator.FacingDirection);
				if (num2 > 90f)
				{
					num2 = Mathf.Abs(num2 - 180f);
				}
				if (num2 < 20f)
				{
					this.m_chargeDir = vector;
					this.State = TankTreaderChargeBehavior.FireState.Charging;
					this.m_updateEveryFrame = true;
					return BehaviorResult.RunContinuous;
				}
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x00195A48 File Offset: 0x00193C48
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.State == TankTreaderChargeBehavior.FireState.Charging)
		{
			this.m_aiActor.BehaviorVelocity = this.m_chargeDir.normalized * this.chargeSpeed;
			if (this.maxChargeDistance > 0f)
			{
				this.m_chargeTime += this.m_deltaTime;
				if (this.m_chargeTime * this.chargeSpeed > this.maxChargeDistance)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
		}
		else if (this.State == TankTreaderChargeBehavior.FireState.Bouncing && !this.m_aiAnimator.IsPlaying(this.hitAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x00195AEC File Offset: 0x00193CEC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.State = TankTreaderChargeBehavior.FireState.Idle;
		this.UpdateCooldowns();
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x00195B08 File Offset: 0x00193D08
	private void OnCollision(CollisionData collisionData)
	{
		if (this.State == TankTreaderChargeBehavior.FireState.Charging && !this.m_aiActor.healthHaver.IsDead)
		{
			if (collisionData.OtherRigidbody)
			{
				Projectile projectile = collisionData.OtherRigidbody.projectile;
				if (projectile && !(projectile.Owner is PlayerController))
				{
					return;
				}
			}
			this.State = TankTreaderChargeBehavior.FireState.Bouncing;
		}
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00195B78 File Offset: 0x00193D78
	// (set) Token: 0x06004B11 RID: 19217 RVA: 0x00195B80 File Offset: 0x00193D80
	private TankTreaderChargeBehavior.FireState State
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

	// Token: 0x06004B12 RID: 19218 RVA: 0x00195BB0 File Offset: 0x00193DB0
	private void BeginState(TankTreaderChargeBehavior.FireState state)
	{
		if (state != TankTreaderChargeBehavior.FireState.Idle)
		{
			if (state == TankTreaderChargeBehavior.FireState.Charging)
			{
				this.m_chargeTime = 0f;
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorVelocity = this.m_chargeDir.normalized * this.chargeSpeed;
				float num = this.m_aiActor.BehaviorVelocity.ToAngle();
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
				this.m_aiActor.DoDustUps = this.chargeDustUps;
				this.m_aiActor.DustUpInterval = this.chargeDustUpInterval;
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
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x00195DEC File Offset: 0x00193FEC
	private void EndState(TankTreaderChargeBehavior.FireState state)
	{
		if (state == TankTreaderChargeBehavior.FireState.Charging)
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
			this.m_aiActor.DustUpInterval = this.m_cachedDustUpInterval;
			this.m_aiActor.PathableTiles = this.m_cachedPathableTiles;
			this.m_aiAnimator.EndAnimationIf(this.chargeAnim);
		}
	}

	// Token: 0x04004046 RID: 16454
	public float minRange;

	// Token: 0x04004047 RID: 16455
	public string chargeAnim;

	// Token: 0x04004048 RID: 16456
	public string hitAnim;

	// Token: 0x04004049 RID: 16457
	public float chargeSpeed;

	// Token: 0x0400404A RID: 16458
	public float maxChargeDistance = -1f;

	// Token: 0x0400404B RID: 16459
	public float chargeKnockback = 50f;

	// Token: 0x0400404C RID: 16460
	public float chargeDamage = 0.5f;

	// Token: 0x0400404D RID: 16461
	public float wallRecoilForce = 10f;

	// Token: 0x0400404E RID: 16462
	public GameObject launchVfx;

	// Token: 0x0400404F RID: 16463
	public GameObject trailVfx;

	// Token: 0x04004050 RID: 16464
	public Transform trailVfxParent;

	// Token: 0x04004051 RID: 16465
	public GameObject hitVfx;

	// Token: 0x04004052 RID: 16466
	public bool chargeDustUps;

	// Token: 0x04004053 RID: 16467
	public float chargeDustUpInterval;

	// Token: 0x04004054 RID: 16468
	private TankTreaderChargeBehavior.FireState m_state;

	// Token: 0x04004055 RID: 16469
	private float m_chargeTime;

	// Token: 0x04004056 RID: 16470
	private Vector2 m_chargeDir;

	// Token: 0x04004057 RID: 16471
	private float m_cachedKnockback;

	// Token: 0x04004058 RID: 16472
	private float m_cachedDamage;

	// Token: 0x04004059 RID: 16473
	private VFXPool m_cachedVfx;

	// Token: 0x0400405A RID: 16474
	private CellTypes m_cachedPathableTiles;

	// Token: 0x0400405B RID: 16475
	private bool m_cachedDoDustUps;

	// Token: 0x0400405C RID: 16476
	private float m_cachedDustUpInterval;

	// Token: 0x0400405D RID: 16477
	private GameObject m_trailVfx;

	// Token: 0x02000DD7 RID: 3543
	private enum FireState
	{
		// Token: 0x0400405F RID: 16479
		Idle,
		// Token: 0x04004060 RID: 16480
		Charging,
		// Token: 0x04004061 RID: 16481
		Bouncing
	}
}
