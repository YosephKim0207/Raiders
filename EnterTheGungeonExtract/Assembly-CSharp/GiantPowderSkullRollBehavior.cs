using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DBE RID: 3518
[InspectorDropdownName("Bosses/GiantPowderSkull/RollBehavior")]
public class GiantPowderSkullRollBehavior : BasicAttackBehavior
{
	// Token: 0x06004AA1 RID: 19105 RVA: 0x00191054 File Offset: 0x0018F254
	public override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
	}

	// Token: 0x06004AA2 RID: 19106 RVA: 0x0019108C File Offset: 0x0018F28C
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_timeSinceLastBounce += this.m_deltaTime;
	}

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001910A8 File Offset: 0x0018F2A8
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_startingAngle = BraveMathCollege.ClampAngle360(BraveUtility.RandomElement<float>(this.startingAngles));
		this.m_bounces = 0;
		if (this.trailParticleSystem)
		{
			this.m_cachedVelocityFraction = this.trailParticleSystem.VelocityFraction;
			this.trailParticleSystem.VelocityFraction = 0f;
		}
		this.m_state = GiantPowderSkullRollBehavior.RollState.Charge;
		this.m_aiAnimator.LockFacingDirection = true;
		this.m_aiAnimator.FacingDirection = this.m_startingAngle;
		this.m_aiAnimator.PlayUntilFinished("roll_charge", false, null, -1f, false);
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_aiActor.specRigidbody.PixelColliders[0].Enabled = false;
		this.m_aiActor.specRigidbody.PixelColliders[1].Enabled = false;
		this.m_aiActor.specRigidbody.PixelColliders[3].Enabled = true;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001911E0 File Offset: 0x0018F3E0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == GiantPowderSkullRollBehavior.RollState.Charge)
		{
			if (!this.m_aiAnimator.IsPlaying("roll_charge"))
			{
				this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startingAngle, this.rollSpeed);
				this.m_aiAnimator.FacingDirection = this.m_aiActor.BehaviorVelocity.ToAngle();
				this.m_state = GiantPowderSkullRollBehavior.RollState.Rolling;
				this.m_aiAnimator.PlayUntilCancelled("roll", false, null, -1f, false);
			}
		}
		else if (this.m_state != GiantPowderSkullRollBehavior.RollState.Rolling)
		{
			if (this.m_state == GiantPowderSkullRollBehavior.RollState.Stopping && !this.m_aiAnimator.IsPlaying("roll_out"))
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x001912A8 File Offset: 0x0018F4A8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.LockFacingDirection = false;
		if (this.trailParticleSystem)
		{
			this.trailParticleSystem.VelocityFraction = this.m_cachedVelocityFraction;
		}
		this.m_aiActor.specRigidbody.PixelColliders[0].Enabled = true;
		this.m_aiActor.specRigidbody.PixelColliders[1].Enabled = true;
		this.m_aiActor.specRigidbody.PixelColliders[3].Enabled = false;
		this.m_state = GiantPowderSkullRollBehavior.RollState.None;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x00191350 File Offset: 0x0018F550
	protected virtual void OnCollision(CollisionData collision)
	{
		if (this.m_state != GiantPowderSkullRollBehavior.RollState.Rolling)
		{
			return;
		}
		if (collision.OtherRigidbody)
		{
			return;
		}
		if (this.m_timeSinceLastBounce > 1f)
		{
			this.m_bounces++;
			SpawnManager.SpawnBulletScript(this.m_aiActor, this.collisionBulletScript, null, null, false, null);
		}
		AkSoundEngine.PostEvent("Play_ENM_statue_stomp_01", this.m_aiActor.gameObject);
		if (this.m_bounces > this.numBounces)
		{
			PhysicsEngine.PostSliceVelocity = new Vector2?(Vector2.zero);
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_state = GiantPowderSkullRollBehavior.RollState.Stopping;
			this.m_aiAnimator.PlayUntilFinished("roll_stop", false, null, -1f, false);
			return;
		}
		Vector2 vector = collision.MyRigidbody.Velocity;
		if (collision.CollidedX)
		{
			vector.x *= -1f;
		}
		if (collision.CollidedY)
		{
			vector.y *= -1f;
		}
		vector = vector.normalized * this.rollSpeed;
		this.m_aiAnimator.FacingDirection = vector.ToAngle();
		PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
		this.m_aiActor.BehaviorVelocity = vector;
		this.m_timeSinceLastBounce = 0f;
	}

	// Token: 0x04003F73 RID: 16243
	public float[] startingAngles = new float[] { 45f, 135f, 225f, 315f };

	// Token: 0x04003F74 RID: 16244
	public float rollSpeed = 9f;

	// Token: 0x04003F75 RID: 16245
	public int numBounces = 3;

	// Token: 0x04003F76 RID: 16246
	public BulletScriptSelector collisionBulletScript;

	// Token: 0x04003F77 RID: 16247
	[InspectorCategory("Visuals")]
	public PowderSkullParticleController trailParticleSystem;

	// Token: 0x04003F78 RID: 16248
	private GiantPowderSkullRollBehavior.RollState m_state;

	// Token: 0x04003F79 RID: 16249
	private float m_cachedVelocityFraction;

	// Token: 0x04003F7A RID: 16250
	private float m_timeSinceLastBounce;

	// Token: 0x04003F7B RID: 16251
	private int m_bounces;

	// Token: 0x04003F7C RID: 16252
	private float m_startingAngle;

	// Token: 0x02000DBF RID: 3519
	private enum RollState
	{
		// Token: 0x04003F7E RID: 16254
		None,
		// Token: 0x04003F7F RID: 16255
		Charge,
		// Token: 0x04003F80 RID: 16256
		Rolling,
		// Token: 0x04003F81 RID: 16257
		Stopping
	}
}
