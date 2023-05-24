using System;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DF4 RID: 3572
public class SmoothSeekTargetBehavior : RangedMovementBehavior
{
	// Token: 0x06004BA8 RID: 19368 RVA: 0x0019C0E8 File Offset: 0x0019A2E8
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
		this.m_bobPeriod = this.bobPeriod + UnityEngine.Random.Range(-this.bobPeriodVariance, this.bobPeriodVariance);
		this.m_direction = -90f;
		if (this.m_aiAnimator)
		{
			this.m_aiAnimator.FacingDirection = -90f;
		}
		this.m_targetCenter = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x0019C164 File Offset: 0x0019A364
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_timer += this.m_deltaTime;
		this.m_timeSinceLastUpdate += this.m_deltaTime;
		base.DecrementTimer(ref this.m_pathTimer, false);
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x0019C1A0 File Offset: 0x0019A3A0
	public override BehaviorResult Update()
	{
		if (this.m_timeSinceLastUpdate > 0.4f)
		{
			this.m_direction = this.m_aiAnimator.FacingDirection;
		}
		this.m_timeSinceLastUpdate = 0f;
		if (this.pathfind && !this.m_aiActor.HasLineOfSightToTarget)
		{
			if (this.m_pathTimer <= 0f)
			{
				this.UpdateTargetCenter();
				Path path = null;
				if (Pathfinder.Instance.GetPath(this.m_aiActor.PathTile, this.m_targetCenter.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(this.m_aiActor.Clearance), this.m_aiActor.PathableTiles, null, null, false) && path.Count > 0)
				{
					path.Smooth(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.specRigidbody.UnitDimensions / 2f, this.m_aiActor.PathableTiles, false, this.m_aiActor.Clearance);
					this.m_targetCenter = path.GetFirstCenterVector2();
				}
				this.m_pathTimer += this.pathInterval;
			}
		}
		else
		{
			this.UpdateTargetCenter();
		}
		float num = this.turnTime;
		if (this.stoppedTurnMultiplier != 0f && this.m_aiActor.specRigidbody.Velocity.magnitude < this.m_aiActor.MovementSpeed / 2f)
		{
			num *= this.stoppedTurnMultiplier;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		float num2 = (this.m_targetCenter - unitCenter).ToAngle();
		if (this.targetTolerance > 0f)
		{
			float num3 = Mathf.DeltaAngle(num2, this.m_direction);
			if (Mathf.Abs(num3) < this.targetTolerance)
			{
				num2 = this.m_direction;
			}
			else
			{
				num2 += Mathf.Sign(num3) * this.targetTolerance;
			}
		}
		this.m_direction = Mathf.SmoothDampAngle(this.m_direction, num2, ref this.m_angularVelocity, num);
		if (this.slither)
		{
			this.m_slitherDirection = Mathf.Sin(this.m_timer * 3.1415927f / this.slitherPeriod) * this.slitherMagnitude;
		}
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_direction + this.m_slitherDirection, this.m_aiActor.MovementSpeed);
		if (this.bob)
		{
			float num4 = Mathf.Sin(this.m_timer * 3.1415927f / this.m_bobPeriod) * this.bobMagnitude;
			if (this.m_deltaTime > 0f)
			{
				this.m_aiActor.BehaviorVelocity += new Vector2(0f, num4 - this.m_lastBobOffset) / this.m_deltaTime;
			}
			this.m_lastBobOffset = num4;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004BAB RID: 19371 RVA: 0x0019C480 File Offset: 0x0019A680
	private void UpdateTargetCenter()
	{
		if (this.m_aiActor.TargetRigidbody)
		{
			this.m_targetCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x04004164 RID: 16740
	public float turnTime = 1f;

	// Token: 0x04004165 RID: 16741
	public float stoppedTurnMultiplier = 1f;

	// Token: 0x04004166 RID: 16742
	public float targetTolerance;

	// Token: 0x04004167 RID: 16743
	public bool slither;

	// Token: 0x04004168 RID: 16744
	[InspectorShowIf("slither")]
	[InspectorIndent]
	public float slitherPeriod;

	// Token: 0x04004169 RID: 16745
	[InspectorShowIf("slither")]
	[InspectorIndent]
	public float slitherMagnitude;

	// Token: 0x0400416A RID: 16746
	public bool bob;

	// Token: 0x0400416B RID: 16747
	[InspectorIndent]
	[InspectorShowIf("bob")]
	public float bobPeriod;

	// Token: 0x0400416C RID: 16748
	[InspectorIndent]
	[InspectorShowIf("bob")]
	public float bobPeriodVariance;

	// Token: 0x0400416D RID: 16749
	[InspectorIndent]
	[InspectorShowIf("bob")]
	public float bobMagnitude;

	// Token: 0x0400416E RID: 16750
	public bool pathfind;

	// Token: 0x0400416F RID: 16751
	[InspectorIndent]
	[InspectorShowIf("pathfind")]
	public float pathInterval = 0.25f;

	// Token: 0x04004170 RID: 16752
	private Vector2 m_targetCenter;

	// Token: 0x04004171 RID: 16753
	private float m_timer;

	// Token: 0x04004172 RID: 16754
	private float m_pathTimer;

	// Token: 0x04004173 RID: 16755
	private float m_direction = -90f;

	// Token: 0x04004174 RID: 16756
	private float m_angularVelocity;

	// Token: 0x04004175 RID: 16757
	private float m_slitherDirection;

	// Token: 0x04004176 RID: 16758
	private float m_bobPeriod;

	// Token: 0x04004177 RID: 16759
	private float m_lastBobOffset;

	// Token: 0x04004178 RID: 16760
	private float m_timeSinceLastUpdate;
}
