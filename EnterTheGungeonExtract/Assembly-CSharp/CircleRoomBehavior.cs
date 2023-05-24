using System;
using UnityEngine;

// Token: 0x02000DE6 RID: 3558
public class CircleRoomBehavior : MovementBehaviorBase
{
	// Token: 0x06004B58 RID: 19288 RVA: 0x00198264 File Offset: 0x00196464
	public override void Start()
	{
		base.Start();
		this.m_center = this.m_aiActor.ParentRoom.area.UnitCenter;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x00198288 File Offset: 0x00196488
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x001982A0 File Offset: 0x001964A0
	public override BehaviorResult Update()
	{
		if (this.m_repathTimer <= 0f)
		{
			float num = (this.m_aiActor.specRigidbody.UnitCenter - this.m_center).ToAngle();
			float num2 = this.PathInterval * 2f * this.m_aiActor.MovementSpeed;
			float num3 = num + this.Direction * (num2 / this.Radius) * 57.29578f;
			Vector2 vector = this.m_center + BraveMathCollege.DegreesToVector(num3, this.Radius);
			this.m_aiActor.PathfindToPosition(vector, new Vector2?(vector), true, null, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040040D3 RID: 16595
	public float PathInterval = 0.25f;

	// Token: 0x040040D4 RID: 16596
	public float Radius = 3f;

	// Token: 0x040040D5 RID: 16597
	public float Direction = 1f;

	// Token: 0x040040D6 RID: 16598
	private float m_repathTimer;

	// Token: 0x040040D7 RID: 16599
	private Vector2 m_center;
}
