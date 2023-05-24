using System;
using UnityEngine;

// Token: 0x02000DFA RID: 3578
public class WaitThenChargeBehavior : MovementBehaviorBase
{
	// Token: 0x06004BC3 RID: 19395 RVA: 0x0019D708 File Offset: 0x0019B908
	public override void Start()
	{
		base.Start();
		this.m_delayTimer = this.Delay;
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x0019D71C File Offset: 0x0019B91C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_delayTimer, false);
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x0019D734 File Offset: 0x0019B934
	public override BehaviorResult Update()
	{
		if (this.m_isCharging)
		{
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_chargeDirection, this.m_aiActor.MovementSpeed);
		}
		else if (this.m_delayTimer <= 0f)
		{
			this.m_isCharging = true;
			if (this.m_aiActor.TargetRigidbody)
			{
				this.m_chargeDirection = (this.m_aiActor.behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox)).ToAngle();
			}
			else
			{
				this.m_chargeDirection = UnityEngine.Random.Range(0f, 360f);
			}
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_chargeDirection, this.m_aiActor.MovementSpeed);
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040041A7 RID: 16807
	public float Delay;

	// Token: 0x040041A8 RID: 16808
	private float m_delayTimer;

	// Token: 0x040041A9 RID: 16809
	private bool m_isCharging;

	// Token: 0x040041AA RID: 16810
	private float m_chargeDirection;

	// Token: 0x040041AB RID: 16811
	private Vector2 m_center;
}
