using System;
using UnityEngine;

// Token: 0x02001659 RID: 5721
public class MomentumProjectile : Projectile
{
	// Token: 0x06008584 RID: 34180 RVA: 0x00371528 File Offset: 0x0036F728
	public override void Start()
	{
		base.Start();
		if (base.Owner && base.Owner.specRigidbody)
		{
			this.m_currentDirection = this.m_currentDirection.normalized * (1f - this.momentumFraction) + base.Owner.specRigidbody.Velocity.normalized * this.momentumFraction;
			this.m_currentDirection = this.m_currentDirection.normalized;
		}
	}

	// Token: 0x06008585 RID: 34181 RVA: 0x003715B8 File Offset: 0x0036F7B8
	protected override void Move()
	{
		this.m_timeElapsed += base.LocalDeltaTime;
		if (this.angularVelocity != 0f)
		{
			base.transform.RotateAround(base.transform.position.XY(), Vector3.forward, this.angularVelocity * base.LocalDeltaTime);
		}
		if (this.baseData.UsesCustomAccelerationCurve)
		{
			float num = Mathf.Clamp01((this.m_timeElapsed - this.baseData.IgnoreAccelCurveTime) / this.baseData.CustomAccelerationCurveDuration);
			this.m_currentSpeed = this.baseData.AccelerationCurve.Evaluate(num) * this.baseData.speed;
		}
		base.specRigidbody.Velocity = this.m_currentDirection * this.m_currentSpeed;
		this.m_currentSpeed *= 1f - this.baseData.damping * base.LocalDeltaTime;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008586 RID: 34182 RVA: 0x003716C4 File Offset: 0x0036F8C4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040089BD RID: 35261
	public float momentumFraction = 0.35f;
}
