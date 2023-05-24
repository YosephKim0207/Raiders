using System;
using UnityEngine;

// Token: 0x02000E05 RID: 3589
public class ModifySpeedBehavior : OverrideBehaviorBase
{
	// Token: 0x06004C03 RID: 19459 RVA: 0x0019EC08 File Offset: 0x0019CE08
	public override BehaviorResult Update()
	{
		if (this.m_aiActor.TargetRigidbody)
		{
			float distanceToTarget = this.m_aiActor.DistanceToTarget;
			float num = Mathf.InverseLerp(this.minSpeedDistance, this.maxSpeedDistance, distanceToTarget);
			this.m_aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(Mathf.Lerp(this.minSpeed, this.maxSpeed, num));
			if (this.m_aiActor.IsBlackPhantom)
			{
				this.m_aiActor.MovementSpeed *= this.m_aiActor.BlackPhantomProperties.MovementSpeedMultiplier;
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x040041DB RID: 16859
	public float minSpeed;

	// Token: 0x040041DC RID: 16860
	public float minSpeedDistance;

	// Token: 0x040041DD RID: 16861
	public float maxSpeed;

	// Token: 0x040041DE RID: 16862
	public float maxSpeedDistance;
}
