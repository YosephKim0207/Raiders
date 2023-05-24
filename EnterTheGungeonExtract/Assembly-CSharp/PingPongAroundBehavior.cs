using System;
using UnityEngine;

// Token: 0x02000DEE RID: 3566
public class PingPongAroundBehavior : MovementBehaviorBase
{
	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x06004B90 RID: 19344 RVA: 0x0019B578 File Offset: 0x00199778
	private bool ReflectX
	{
		get
		{
			return this.motionType == PingPongAroundBehavior.MotionType.Diagonals || this.motionType == PingPongAroundBehavior.MotionType.Horizontal;
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x06004B91 RID: 19345 RVA: 0x0019B594 File Offset: 0x00199794
	private bool ReflectY
	{
		get
		{
			return this.motionType == PingPongAroundBehavior.MotionType.Diagonals || this.motionType == PingPongAroundBehavior.MotionType.Vertical;
		}
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x0019B5B0 File Offset: 0x001997B0
	public override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x0019B5E8 File Offset: 0x001997E8
	public override BehaviorResult Update()
	{
		this.m_startingAngle = BraveMathCollege.ClampAngle360(BraveUtility.RandomElement<float>(this.startingAngles));
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startingAngle, this.m_aiActor.MovementSpeed);
		this.m_isBouncing = true;
		return BehaviorResult.RunContinuousInClass;
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x0019B640 File Offset: 0x00199840
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		return this.m_aiActor.BehaviorOverridesVelocity ? ContinuousBehaviorResult.Continue : ContinuousBehaviorResult.Finished;
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x0019B660 File Offset: 0x00199860
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_isBouncing = false;
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x0019B670 File Offset: 0x00199870
	protected virtual void OnCollision(CollisionData collision)
	{
		if (!this.m_isBouncing)
		{
			return;
		}
		if (collision.OtherRigidbody && collision.OtherRigidbody.projectile)
		{
			return;
		}
		if (collision.CollidedX || collision.CollidedY)
		{
			Vector2 vector = collision.MyRigidbody.Velocity;
			if (collision.CollidedX && this.ReflectX)
			{
				vector.x *= -1f;
			}
			if (collision.CollidedY && this.ReflectY)
			{
				vector.y *= -1f;
			}
			if (this.motionType == PingPongAroundBehavior.MotionType.Horizontal)
			{
				vector.y = 0f;
			}
			if (this.motionType == PingPongAroundBehavior.MotionType.Vertical)
			{
				vector.x = 0f;
			}
			vector = vector.normalized * this.m_aiActor.MovementSpeed;
			PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
			this.m_aiActor.BehaviorVelocity = vector;
		}
	}

	// Token: 0x04004146 RID: 16710
	public float[] startingAngles = new float[] { 45f, 135f, 225f, 315f };

	// Token: 0x04004147 RID: 16711
	public PingPongAroundBehavior.MotionType motionType = PingPongAroundBehavior.MotionType.Diagonals;

	// Token: 0x04004148 RID: 16712
	private bool m_isBouncing;

	// Token: 0x04004149 RID: 16713
	private float m_startingAngle;

	// Token: 0x02000DEF RID: 3567
	public enum MotionType
	{
		// Token: 0x0400414B RID: 16715
		Diagonals = 10,
		// Token: 0x0400414C RID: 16716
		Horizontal = 20,
		// Token: 0x0400414D RID: 16717
		Vertical = 30
	}
}
