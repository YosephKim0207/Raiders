using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DD8 RID: 3544
[InspectorDropdownName("Bosses/Bashellisk/SeekTargetBehavior")]
public class BashelliskSeekTargetBehavior : RangedMovementBehavior
{
	// Token: 0x06004B15 RID: 19221 RVA: 0x00195F08 File Offset: 0x00194108
	public override void Start()
	{
		base.Start();
		this.m_head = this.m_aiActor.GetComponent<BashelliskHeadController>();
		this.m_updateEveryFrame = true;
		if (TurboModeController.IsActive)
		{
			this.turnTime /= TurboModeController.sEnemyMovementSpeedMultiplier;
			this.snapTurnTime /= TurboModeController.sEnemyMovementSpeedMultiplier;
		}
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x00195F64 File Offset: 0x00194164
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		if (!this.m_head.IsMidPickup)
		{
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
		}
		this.m_aiAnimator.LockFacingDirection = true;
		base.DecrementTimer(ref this.m_pickupConsiderationTimer, false);
		this.m_slitherCounter += this.m_deltaTime * this.m_aiActor.behaviorSpeculator.CooldownScale;
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x00195FE0 File Offset: 0x001941E0
	public override BehaviorResult Update()
	{
		this.UpdateState();
		if (this.m_head.IsMidPickup)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_head.ReinitMovementDirection)
		{
			this.m_direction = this.m_head.aiAnimator.FacingDirection;
			this.m_head.ReinitMovementDirection = false;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		float num = (this.m_targetCenter - unitCenter).ToAngle();
		this.m_direction = Mathf.SmoothDampAngle(this.m_direction, num, ref this.m_angularVelocity, (!this.m_snapToTarget) ? this.turnTime : this.snapTurnTime);
		if ((!this.m_snapToTarget) ? this.slither : this.snapSlither)
		{
			this.m_slitherDirection = Mathf.Sin(this.m_slitherCounter * 3.1415927f / this.slitherPeriod) * this.slitherMagnitude;
		}
		this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_direction + this.m_slitherDirection, this.m_aiActor.MovementSpeed);
		return BehaviorResult.Continue;
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x06004B18 RID: 19224 RVA: 0x00196100 File Offset: 0x00194300
	// (set) Token: 0x06004B19 RID: 19225 RVA: 0x00196108 File Offset: 0x00194308
	private BashelliskSeekTargetBehavior.SeekState State
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

	// Token: 0x06004B1A RID: 19226 RVA: 0x0019612C File Offset: 0x0019432C
	private void BeginState(BashelliskSeekTargetBehavior.SeekState state)
	{
		if (state == BashelliskSeekTargetBehavior.SeekState.ConsideringPickup)
		{
			this.m_pickupConsiderationTimer = UnityEngine.Random.Range(this.minPickupDelay, this.maxPickupDelay);
		}
		else if (state == BashelliskSeekTargetBehavior.SeekState.SeekPickup)
		{
			this.m_head.CanPickup = true;
			this.m_desiredPickup = this.m_head.AvailablePickups.GetByIndexSlow(UnityEngine.Random.Range(0, this.m_head.AvailablePickups.Count)).Value;
			if (this.m_desiredPickup)
			{
				this.m_targetCenter = this.m_desiredPickup.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
		}
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001961C8 File Offset: 0x001943C8
	private void UpdateState()
	{
		this.m_snapToTarget = false;
		if (this.State == BashelliskSeekTargetBehavior.SeekState.SeekPlayer)
		{
			if (this.m_aiActor.TargetRigidbody)
			{
				this.m_targetCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			if (this.m_head.AvailablePickups.Count > 0)
			{
				this.State = BashelliskSeekTargetBehavior.SeekState.ConsideringPickup;
				this.UpdateState();
			}
		}
		else if (this.State == BashelliskSeekTargetBehavior.SeekState.ConsideringPickup)
		{
			if (this.m_head.AvailablePickups.Count == 0)
			{
				this.State = BashelliskSeekTargetBehavior.SeekState.SeekPlayer;
				this.UpdateState();
			}
			else if (this.m_pickupConsiderationTimer <= 0f)
			{
				this.State = BashelliskSeekTargetBehavior.SeekState.SeekPickup;
				this.UpdateState();
			}
		}
		else if (this.State == BashelliskSeekTargetBehavior.SeekState.SeekPickup)
		{
			if (this.m_desiredPickup && this.m_desiredPickup.aiActor.CanTargetPlayers)
			{
				if (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_targetCenter) < this.snapDist)
				{
					this.m_snapToTarget = true;
				}
			}
			else
			{
				this.State = BashelliskSeekTargetBehavior.SeekState.SeekPlayer;
				this.UpdateState();
			}
		}
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x00196300 File Offset: 0x00194500
	private void EndState(BashelliskSeekTargetBehavior.SeekState state)
	{
		if (state == BashelliskSeekTargetBehavior.SeekState.SeekPickup)
		{
			this.m_head.CanPickup = false;
		}
	}

	// Token: 0x04004062 RID: 16482
	public float turnTime = 1f;

	// Token: 0x04004063 RID: 16483
	public bool slither;

	// Token: 0x04004064 RID: 16484
	public float slitherPeriod;

	// Token: 0x04004065 RID: 16485
	public float slitherMagnitude;

	// Token: 0x04004066 RID: 16486
	public float minPickupDelay;

	// Token: 0x04004067 RID: 16487
	public float maxPickupDelay;

	// Token: 0x04004068 RID: 16488
	public float snapDist;

	// Token: 0x04004069 RID: 16489
	public float snapTurnTime;

	// Token: 0x0400406A RID: 16490
	public bool snapSlither;

	// Token: 0x0400406B RID: 16491
	private BashelliskSeekTargetBehavior.SeekState m_state;

	// Token: 0x0400406C RID: 16492
	private BashelliskHeadController m_head;

	// Token: 0x0400406D RID: 16493
	private Vector2 m_targetCenter;

	// Token: 0x0400406E RID: 16494
	private BashelliskBodyPickupController m_desiredPickup;

	// Token: 0x0400406F RID: 16495
	private bool m_snapToTarget;

	// Token: 0x04004070 RID: 16496
	private float m_slitherCounter;

	// Token: 0x04004071 RID: 16497
	private float m_direction = -90f;

	// Token: 0x04004072 RID: 16498
	private float m_slitherDirection;

	// Token: 0x04004073 RID: 16499
	private float m_angularVelocity;

	// Token: 0x04004074 RID: 16500
	private float m_pickupConsiderationTimer;

	// Token: 0x02000DD9 RID: 3545
	private enum SeekState
	{
		// Token: 0x04004076 RID: 16502
		SeekPlayer,
		// Token: 0x04004077 RID: 16503
		ConsideringPickup,
		// Token: 0x04004078 RID: 16504
		SeekPickup
	}
}
