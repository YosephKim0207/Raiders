using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DDD RID: 3549
[InspectorDropdownName("Minibosses/Fusebomb/SeekTargetBehavior")]
public class FusebombFloatBehavior : MovementBehaviorBase
{
	// Token: 0x06004B2B RID: 19243 RVA: 0x00196A34 File Offset: 0x00194C34
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
	}

	// Token: 0x06004B2C RID: 19244 RVA: 0x00196A44 File Offset: 0x00194C44
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiActor.OverridePathVelocity = null;
	}

	// Token: 0x06004B2D RID: 19245 RVA: 0x00196A6C File Offset: 0x00194C6C
	public override BehaviorResult Update()
	{
		this.m_timer += this.m_deltaTime;
		Vector2 vector = this.m_aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(Mathf.SmoothStep(this.minPoint.x, this.maxPoint.x, Mathf.PingPong(this.m_timer, this.period.x) / this.period.x), Mathf.SmoothStep(this.minPoint.y, this.maxPoint.y, Mathf.PingPong(this.m_timer, this.period.y) / this.period.y));
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 vector2 = vector - unitCenter;
		Vector2 vector3;
		if (this.m_deltaTime > 0f && vector2.magnitude > 0f)
		{
			vector3 = vector2 / BraveTime.DeltaTime;
			if (this.MaxSpeed >= 0f && vector3.magnitude > this.MaxSpeed)
			{
				vector3 = this.MaxSpeed * vector3.normalized;
			}
		}
		else
		{
			vector3 = Vector2.zero;
		}
		this.m_isMoving = true;
		this.m_aiActor.OverridePathVelocity = new Vector2?(vector3);
		return BehaviorResult.Continue;
	}

	// Token: 0x04004090 RID: 16528
	public Vector2 minPoint;

	// Token: 0x04004091 RID: 16529
	public Vector2 maxPoint;

	// Token: 0x04004092 RID: 16530
	public Vector2 period;

	// Token: 0x04004093 RID: 16531
	public float MaxSpeed = 6f;

	// Token: 0x04004094 RID: 16532
	private float m_timer;

	// Token: 0x04004095 RID: 16533
	private bool m_isMoving;
}
