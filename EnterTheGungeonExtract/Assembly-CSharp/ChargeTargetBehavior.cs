using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DE4 RID: 3556
public class ChargeTargetBehavior : MovementBehaviorBase
{
	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x06004B4C RID: 19276 RVA: 0x00197B0C File Offset: 0x00195D0C
	// (set) Token: 0x06004B4D RID: 19277 RVA: 0x00197B14 File Offset: 0x00195D14
	protected ChargeTargetBehavior.ChargeState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			this.EndState(this.m_state);
			this.m_state = value;
			this.BeginState(this.m_state);
		}
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x00197B38 File Offset: 0x00195D38
	public override void Start()
	{
		base.Start();
		this.m_cachedKnockback = this.m_aiActor.CollisionKnockbackStrength;
		this.m_cachedDamage = this.m_aiActor.CollisionDamage;
		this.m_cachedDoDustUps = this.m_aiActor.DoDustUps;
		this.m_cachedDustUpInterval = this.m_aiActor.DustUpInterval;
		this.m_cachedVfx = this.m_aiActor.CollisionVFX;
		this.m_deceleration = this.ChargeSpeed * this.ChargeSpeed / (-2f * this.OvershootFactor);
		SpeculativeRigidbody specRigidbody = this.m_aiAnimator.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
	}

	// Token: 0x06004B4F RID: 19279 RVA: 0x00197BEC File Offset: 0x00195DEC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_phaseTimer, false);
	}

	// Token: 0x06004B50 RID: 19280 RVA: 0x00197C10 File Offset: 0x00195E10
	public override BehaviorResult Update()
	{
		if (this.m_aiActor.TargetRigidbody != null)
		{
			switch (this.State)
			{
			case ChargeTargetBehavior.ChargeState.Charging:
				return this.HandleChargeState();
			case ChargeTargetBehavior.ChargeState.Waiting:
				return this.HandleWaitState();
			case ChargeTargetBehavior.ChargeState.Bumped:
				return this.HandleBumpedState();
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B51 RID: 19281 RVA: 0x00197C6C File Offset: 0x00195E6C
	protected void BeginState(ChargeTargetBehavior.ChargeState state)
	{
		if (state == ChargeTargetBehavior.ChargeState.Charging)
		{
			this.m_playedMelee = false;
			this.m_playedBump = false;
			this.m_chargeElapsedDistance = 0f;
			this.m_aiActor.CollisionKnockbackStrength = this.ChargeKnockback;
			this.m_aiActor.CollisionDamage = this.ChargeDamage;
			this.m_aiActor.DoDustUps = this.ChargeDoDustUps;
			this.m_aiActor.DustUpInterval = this.ChargeDustUpInterval;
			if (this.ChargeHitVFX)
			{
				VFXObject vfxobject = new VFXObject();
				vfxobject.effect = this.ChargeHitVFX;
				VFXComplex vfxcomplex = new VFXComplex();
				vfxcomplex.effects = new VFXObject[] { vfxobject };
				VFXPool vfxpool = new VFXPool();
				vfxpool.type = VFXPoolType.Single;
				vfxpool.effects = new VFXComplex[] { vfxcomplex };
				this.m_aiActor.CollisionVFX = vfxpool;
			}
			this.m_aiActor.ClearPath();
			Vector2 vector = this.m_aiActor.TargetRigidbody.UnitCenter - this.m_aiActor.specRigidbody.UnitCenter;
			this.m_chargeTargetLength = vector.magnitude;
			this.m_chargeDirection = vector.normalized;
			this.m_aiActor.BehaviorOverridesVelocity = true;
		}
		else if (state == ChargeTargetBehavior.ChargeState.Waiting)
		{
			this.m_aiActor.CollisionKnockbackStrength = this.m_cachedKnockback;
			this.m_aiActor.CollisionDamage = this.m_cachedDamage;
			this.m_aiActor.DoDustUps = this.m_cachedDoDustUps;
			this.m_aiActor.DustUpInterval = this.m_cachedDustUpInterval;
			this.m_aiActor.CollisionVFX = this.m_cachedVfx;
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_currentMovementSpeed = this.m_aiActor.MovementSpeed;
			this.m_phaseTimer = this.ChargeCooldownTime;
		}
		else if (state == ChargeTargetBehavior.ChargeState.Bumped)
		{
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_currentMovementSpeed = 0f;
			this.m_phaseTimer = this.BumpTime;
		}
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x00197E70 File Offset: 0x00196070
	protected void EndState(ChargeTargetBehavior.ChargeState state)
	{
		if (state == ChargeTargetBehavior.ChargeState.Charging)
		{
			this.m_aiAnimator.EndAnimationIf("prebump");
			this.m_aiAnimator.LockFacingDirection = false;
		}
		else if (state != ChargeTargetBehavior.ChargeState.Waiting)
		{
			if (state == ChargeTargetBehavior.ChargeState.Bumped)
			{
				this.m_aiAnimator.EndAnimationIf("bump");
				this.m_aiAnimator.LockFacingDirection = false;
			}
		}
	}

	// Token: 0x06004B53 RID: 19283 RVA: 0x00197ED8 File Offset: 0x001960D8
	protected BehaviorResult HandleChargeState()
	{
		this.m_aiActor.BehaviorVelocity = this.m_chargeDirection * this.m_currentMovementSpeed;
		this.m_chargeElapsedDistance += this.m_currentMovementSpeed * this.m_deltaTime;
		this.m_aiActor.ClearPath();
		if (this.m_chargeElapsedDistance >= this.m_chargeTargetLength + this.OvershootFactor || this.m_currentMovementSpeed == 0f)
		{
			this.State = ChargeTargetBehavior.ChargeState.Waiting;
		}
		else if (this.m_chargeElapsedDistance > this.m_chargeTargetLength)
		{
			this.m_currentMovementSpeed = Mathf.Max(this.m_currentMovementSpeed + this.m_deceleration * this.m_deltaTime, 0f);
			if (this.m_playedMelee && !this.m_playedBump)
			{
				float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter);
				if (num > this.PlayMeleeAnimDistance)
				{
					this.m_aiAnimator.EndAnimationIf("prebump");
				}
			}
		}
		else
		{
			this.m_currentMovementSpeed = Mathf.Min(this.m_currentMovementSpeed + this.ChargeAcceleration * this.m_deltaTime, this.ChargeSpeed);
			if (!this.m_playedMelee)
			{
				float num2 = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter);
				if (num2 < this.PlayMeleeAnimDistance)
				{
					this.m_aiAnimator.LockFacingDirection = true;
					this.m_aiAnimator.FacingDirection = BraveMathCollege.Atan2Degrees(this.m_chargeDirection);
					this.m_aiAnimator.PlayUntilCancelled("prebump", true, null, -1f, false);
					this.m_playedMelee = true;
				}
			}
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x00198094 File Offset: 0x00196294
	protected BehaviorResult HandleWaitState()
	{
		bool hasLineOfSightToTarget = this.m_aiActor.HasLineOfSightToTarget;
		bool flag = false;
		if (hasLineOfSightToTarget)
		{
			flag = GameManager.Instance.Dungeon.data.CheckLineForCellType(this.m_aiActor.PathTile, this.m_aiActor.TargetRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), CellType.PIT);
		}
		if (hasLineOfSightToTarget && !flag && this.m_phaseTimer == 0f)
		{
			this.State = ChargeTargetBehavior.ChargeState.Charging;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x00198110 File Offset: 0x00196310
	protected BehaviorResult HandleBumpedState()
	{
		this.m_aiActor.CollisionKnockbackStrength = this.m_cachedKnockback;
		this.m_aiActor.CollisionDamage = this.m_cachedDamage;
		this.m_aiActor.DoDustUps = this.m_cachedDoDustUps;
		this.m_aiActor.DustUpInterval = this.m_cachedDustUpInterval;
		this.m_aiActor.CollisionVFX = this.m_cachedVfx;
		this.m_aiActor.ClearPath();
		if (this.m_phaseTimer == 0f)
		{
			this.State = ChargeTargetBehavior.ChargeState.Waiting;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x00198198 File Offset: 0x00196398
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.State == ChargeTargetBehavior.ChargeState.Charging && this.m_playedMelee && !this.m_playedBump && rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>())
		{
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = BraveMathCollege.Atan2Degrees(rigidbodyCollision.OtherRigidbody.UnitCenter - this.m_aiAnimator.specRigidbody.UnitCenter);
			this.m_aiAnimator.PlayUntilCancelled("bump", true, null, -1f, false);
			this.State = ChargeTargetBehavior.ChargeState.Bumped;
		}
	}

	// Token: 0x040040B5 RID: 16565
	public float ChargeCooldownTime = 1f;

	// Token: 0x040040B6 RID: 16566
	public float OvershootFactor = 3f;

	// Token: 0x040040B7 RID: 16567
	public float ChargeSpeed = 8f;

	// Token: 0x040040B8 RID: 16568
	public float ChargeAcceleration = 4f;

	// Token: 0x040040B9 RID: 16569
	public float ChargeKnockback = 50f;

	// Token: 0x040040BA RID: 16570
	public float ChargeDamage;

	// Token: 0x040040BB RID: 16571
	public bool ChargeDoDustUps;

	// Token: 0x040040BC RID: 16572
	public float ChargeDustUpInterval;

	// Token: 0x040040BD RID: 16573
	public GameObject ChargeHitVFX;

	// Token: 0x040040BE RID: 16574
	public float BumpTime = 1f;

	// Token: 0x040040BF RID: 16575
	public float PlayMeleeAnimDistance = 2f;

	// Token: 0x040040C0 RID: 16576
	protected bool m_playedMelee;

	// Token: 0x040040C1 RID: 16577
	protected bool m_playedBump;

	// Token: 0x040040C2 RID: 16578
	private ChargeTargetBehavior.ChargeState m_state = ChargeTargetBehavior.ChargeState.Waiting;

	// Token: 0x040040C3 RID: 16579
	private float m_chargeTargetLength;

	// Token: 0x040040C4 RID: 16580
	private Vector2 m_chargeDirection;

	// Token: 0x040040C5 RID: 16581
	private float m_chargeElapsedDistance;

	// Token: 0x040040C6 RID: 16582
	private float m_deceleration;

	// Token: 0x040040C7 RID: 16583
	private float m_currentMovementSpeed;

	// Token: 0x040040C8 RID: 16584
	private float m_cachedKnockback;

	// Token: 0x040040C9 RID: 16585
	private float m_cachedDamage;

	// Token: 0x040040CA RID: 16586
	private bool m_cachedDoDustUps;

	// Token: 0x040040CB RID: 16587
	private float m_cachedDustUpInterval;

	// Token: 0x040040CC RID: 16588
	private VFXPool m_cachedVfx;

	// Token: 0x040040CD RID: 16589
	private float m_repathTimer;

	// Token: 0x040040CE RID: 16590
	private float m_phaseTimer;

	// Token: 0x02000DE5 RID: 3557
	protected enum ChargeState
	{
		// Token: 0x040040D0 RID: 16592
		Charging,
		// Token: 0x040040D1 RID: 16593
		Waiting,
		// Token: 0x040040D2 RID: 16594
		Bumped
	}
}
