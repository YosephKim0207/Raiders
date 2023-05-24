using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D7F RID: 3455
[InspectorDropdownName("Bosses/Beholster/LaserBehavior")]
public class BeholsterLaserBehavior : BasicAttackBehavior
{
	// Token: 0x06004932 RID: 18738 RVA: 0x00186734 File Offset: 0x00184934
	public override void Start()
	{
		base.Start();
		this.m_beholsterController = this.m_aiActor.GetComponent<BeholsterController>();
	}

	// Token: 0x06004933 RID: 18739 RVA: 0x00186750 File Offset: 0x00184950
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_aiActor.TargetRigidbody)
		{
			this.m_targetPosition = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			this.m_backupTarget = this.m_aiActor.TargetRigidbody;
		}
		else if (this.m_backupTarget)
		{
			this.m_targetPosition = this.m_backupTarget.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x06004934 RID: 18740 RVA: 0x001867C8 File Offset: 0x001849C8
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
		this.m_aiActor.ClearPath();
		this.m_beholsterController.StopFiringTentacles(null);
		this.m_beholsterController.PrechargeFiringLaser();
		this.m_state = BeholsterLaserBehavior.State.PreCharging;
		this.m_aiActor.SuppressTargetSwitch = true;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x00186838 File Offset: 0x00184A38
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		Vector2 vector = this.m_aiActor.transform.position.XY() + this.m_beholsterController.firingEllipseCenter;
		if (this.m_state == BeholsterLaserBehavior.State.PreCharging)
		{
			if (!this.m_aiActor.spriteAnimator.Playing)
			{
				this.m_beholsterController.ChargeFiringLaser(this.chargeTime);
				this.m_timer = this.chargeTime;
				this.m_state = BeholsterLaserBehavior.State.Charging;
			}
		}
		else
		{
			if (this.m_state == BeholsterLaserBehavior.State.Charging)
			{
				this.m_timer -= this.m_deltaTime;
				if (this.m_timer <= 0f)
				{
					float facingDirection = this.m_aiActor.aiAnimator.FacingDirection;
					this.m_beholsterController.StartFiringLaser(facingDirection);
					this.m_timer = this.firingTime;
					this.m_state = BeholsterLaserBehavior.State.Firing;
				}
				return ContinuousBehaviorResult.Continue;
			}
			if (this.m_state == BeholsterLaserBehavior.State.Firing)
			{
				this.m_timer -= this.m_deltaTime;
				if (this.m_timer <= 0f || !this.m_beholsterController.FiringLaser)
				{
					return ContinuousBehaviorResult.Finished;
				}
				float num12;
				if (this.trackingType == BeholsterLaserBehavior.TrackingType.Follow)
				{
					float num = Vector2.Distance(this.m_targetPosition, vector);
					float num2 = (this.m_targetPosition - vector).ToAngle();
					float num3 = BraveMathCollege.ClampAngle180(num2 - this.m_beholsterController.LaserAngle);
					float num4 = num3 * num * 0.017453292f;
					float num5 = this.maxTurnRate;
					float num6 = Mathf.Sign(num3);
					if (this.m_unitOvershootTimer > 0f)
					{
						num6 = this.m_unitOvershootFixedDirection;
						this.m_unitOvershootTimer -= this.m_deltaTime;
						num5 = this.unitOvershootSpeed;
					}
					this.m_currentUnitTurnRate = Mathf.Clamp(this.m_currentUnitTurnRate + num6 * this.turnRateAcceleration * this.m_deltaTime, -num5, num5);
					float num7 = this.m_currentUnitTurnRate / num * 57.29578f;
					float num8 = 0f;
					if (this.useDegreeCatchUp && Mathf.Abs(num3) > this.minDegreesForCatchUp)
					{
						float num9 = Mathf.InverseLerp(this.minDegreesForCatchUp, 180f, Mathf.Abs(num3)) * this.degreeCatchUpSpeed;
						num8 = Mathf.Max(num8, num9);
					}
					if (this.useUnitCatchUp && Mathf.Abs(num4) > this.minUnitForCatchUp)
					{
						float num10 = Mathf.InverseLerp(this.minUnitForCatchUp, this.maxUnitForCatchUp, Mathf.Abs(num4)) * this.unitCatchUpSpeed;
						float num11 = num10 / num * 57.29578f;
						num8 = Mathf.Max(num8, num11);
					}
					if (this.useUnitOvershoot && Mathf.Abs(num4) < this.minUnitForOvershoot)
					{
						this.m_unitOvershootFixedDirection = (float)((this.m_currentUnitTurnRate <= 0f) ? (-1) : 1);
						this.m_unitOvershootTimer = this.unitOvershootTime;
					}
					num8 *= Mathf.Sign(num3);
					num12 = BraveMathCollege.ClampAngle360(this.m_beholsterController.LaserAngle + (num7 + num8) * this.m_deltaTime);
				}
				else
				{
					num12 = BraveMathCollege.ClampAngle360(this.m_beholsterController.LaserAngle + this.maxTurnRate * this.m_deltaTime);
				}
				if (this.m_beholsterController.LaserBeam && this.m_beholsterController.LaserBeam.State != BasicBeamController.BeamState.Charging)
				{
					this.m_beholsterController.LaserAngle = num12;
				}
				return ContinuousBehaviorResult.Continue;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004936 RID: 18742 RVA: 0x00186B98 File Offset: 0x00184D98
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_beholsterController.StopFiringLaser();
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiActor.SuppressTargetSwitch = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004937 RID: 18743 RVA: 0x00186BD0 File Offset: 0x00184DD0
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x04003D4E RID: 15694
	public BeholsterLaserBehavior.TrackingType trackingType;

	// Token: 0x04003D4F RID: 15695
	public float initialAimOffset;

	// Token: 0x04003D50 RID: 15696
	public float chargeTime;

	// Token: 0x04003D51 RID: 15697
	public float firingTime;

	// Token: 0x04003D52 RID: 15698
	public float maxTurnRate;

	// Token: 0x04003D53 RID: 15699
	public float turnRateAcceleration;

	// Token: 0x04003D54 RID: 15700
	public bool useDegreeCatchUp;

	// Token: 0x04003D55 RID: 15701
	[InspectorShowIf("useDegreeCatchUp")]
	[InspectorIndent]
	public float minDegreesForCatchUp;

	// Token: 0x04003D56 RID: 15702
	[InspectorIndent]
	[InspectorShowIf("useDegreeCatchUp")]
	public float degreeCatchUpSpeed;

	// Token: 0x04003D57 RID: 15703
	public bool useUnitCatchUp;

	// Token: 0x04003D58 RID: 15704
	[InspectorIndent]
	[InspectorShowIf("useUnitCatchUp")]
	public float minUnitForCatchUp;

	// Token: 0x04003D59 RID: 15705
	[InspectorIndent]
	[InspectorShowIf("useUnitCatchUp")]
	public float maxUnitForCatchUp;

	// Token: 0x04003D5A RID: 15706
	[InspectorShowIf("useUnitCatchUp")]
	[InspectorIndent]
	public float unitCatchUpSpeed;

	// Token: 0x04003D5B RID: 15707
	public bool useUnitOvershoot;

	// Token: 0x04003D5C RID: 15708
	[InspectorIndent]
	[InspectorShowIf("useUnitOvershoot")]
	public float minUnitForOvershoot;

	// Token: 0x04003D5D RID: 15709
	[InspectorShowIf("useUnitOvershoot")]
	[InspectorIndent]
	public float unitOvershootTime;

	// Token: 0x04003D5E RID: 15710
	[InspectorShowIf("useUnitOvershoot")]
	[InspectorIndent]
	public float unitOvershootSpeed;

	// Token: 0x04003D5F RID: 15711
	private BeholsterController m_beholsterController;

	// Token: 0x04003D60 RID: 15712
	private BeholsterLaserBehavior.State m_state;

	// Token: 0x04003D61 RID: 15713
	private float m_timer;

	// Token: 0x04003D62 RID: 15714
	private Vector2 m_targetPosition;

	// Token: 0x04003D63 RID: 15715
	private float m_currentUnitTurnRate;

	// Token: 0x04003D64 RID: 15716
	private float m_unitOvershootFixedDirection;

	// Token: 0x04003D65 RID: 15717
	private float m_unitOvershootTimer;

	// Token: 0x04003D66 RID: 15718
	private SpeculativeRigidbody m_backupTarget;

	// Token: 0x02000D80 RID: 3456
	public enum State
	{
		// Token: 0x04003D68 RID: 15720
		PreCharging,
		// Token: 0x04003D69 RID: 15721
		Charging,
		// Token: 0x04003D6A RID: 15722
		Firing
	}

	// Token: 0x02000D81 RID: 3457
	public enum TrackingType
	{
		// Token: 0x04003D6C RID: 15724
		Follow,
		// Token: 0x04003D6D RID: 15725
		ConstantTurn
	}
}
