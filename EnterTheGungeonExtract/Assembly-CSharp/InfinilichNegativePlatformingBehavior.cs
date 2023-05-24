using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC3 RID: 3523
[InspectorDropdownName("Bosses/Infinilich/NegativePlatformingBehavior")]
public class InfinilichNegativePlatformingBehavior : BasicAttackBehavior
{
	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x0019218C File Offset: 0x0019038C
	// (set) Token: 0x06004ABA RID: 19130 RVA: 0x00192194 File Offset: 0x00190394
	private InfinilichNegativePlatformingBehavior.SpinState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x001921C4 File Offset: 0x001903C4
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.State = InfinilichNegativePlatformingBehavior.SpinState.ArmsIn;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x00192214 File Offset: 0x00190414
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == InfinilichNegativePlatformingBehavior.SpinState.ArmsIn)
		{
			if (!this.m_aiAnimator.IsPlaying("arms_in"))
			{
				this.m_startPoint = this.m_aiActor.specRigidbody.UnitCenter;
				this.m_targetPoint = this.m_aiActor.ParentRoom.area.Center + new Vector2(0f, -1.5f);
				float magnitude = (this.m_targetPoint - this.m_startPoint).magnitude;
				this.m_setupTime = Mathf.Min(this.SetupTime, 1.5f * magnitude / this.FlightSpeed);
				this.State = InfinilichNegativePlatformingBehavior.SpinState.GoToStartPoint;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.State == InfinilichNegativePlatformingBehavior.SpinState.GoToStartPoint)
		{
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			Vector2 vector = Vector2Extensions.SmoothStep(this.m_startPoint, this.m_targetPoint, this.m_setupTimer / this.m_setupTime);
			if (this.m_setupTimer > this.m_setupTime)
			{
				this.m_aiActor.BehaviorVelocity = Vector2.zero;
				this.State = InfinilichNegativePlatformingBehavior.SpinState.BulletScript;
				return ContinuousBehaviorResult.Continue;
			}
			this.m_aiActor.BehaviorVelocity = (vector - unitCenter) / BraveTime.DeltaTime;
			this.m_setupTimer += this.m_deltaTime;
		}
		else if (this.State == InfinilichNegativePlatformingBehavior.SpinState.BulletScript)
		{
			if (this.m_bulletSource.IsEnded)
			{
				this.State = InfinilichNegativePlatformingBehavior.SpinState.ArmsOut;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.State == InfinilichNegativePlatformingBehavior.SpinState.ArmsOut && !this.m_aiAnimator.IsPlaying("arms_out"))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x001923C0 File Offset: 0x001905C0
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.State = InfinilichNegativePlatformingBehavior.SpinState.None;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x001923E8 File Offset: 0x001905E8
	private void Fire()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x00192448 File Offset: 0x00190648
	private void BeginState(InfinilichNegativePlatformingBehavior.SpinState state)
	{
		if (state == InfinilichNegativePlatformingBehavior.SpinState.ArmsIn)
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilCancelled("arms_in", false, null, -1f, false);
		}
		else if (state == InfinilichNegativePlatformingBehavior.SpinState.GoToStartPoint)
		{
			this.m_aiAnimator.PlayUntilCancelled("spin", false, null, -1f, false);
			this.m_setupTimer = 0f;
		}
		else if (state == InfinilichNegativePlatformingBehavior.SpinState.BulletScript)
		{
			this.Fire();
		}
		else if (state == InfinilichNegativePlatformingBehavior.SpinState.ArmsOut)
		{
			this.m_aiAnimator.PlayUntilFinished("arms_out", false, null, -1f, false);
		}
	}

	// Token: 0x06004AC0 RID: 19136 RVA: 0x00192504 File Offset: 0x00190704
	private void EndState(InfinilichNegativePlatformingBehavior.SpinState state)
	{
		if (state == InfinilichNegativePlatformingBehavior.SpinState.BulletScript && this.m_bulletSource && !this.m_bulletSource.IsEnded)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x04003FA0 RID: 16288
	public float SetupTime = 1f;

	// Token: 0x04003FA1 RID: 16289
	public float FlightSpeed = 6f;

	// Token: 0x04003FA2 RID: 16290
	public GameObject ShootPoint;

	// Token: 0x04003FA3 RID: 16291
	public BulletScriptSelector BulletScript;

	// Token: 0x04003FA4 RID: 16292
	private InfinilichNegativePlatformingBehavior.SpinState m_state;

	// Token: 0x04003FA5 RID: 16293
	private Vector2 m_startPoint;

	// Token: 0x04003FA6 RID: 16294
	private Vector2 m_targetPoint;

	// Token: 0x04003FA7 RID: 16295
	private float m_setupTime;

	// Token: 0x04003FA8 RID: 16296
	private float m_setupTimer;

	// Token: 0x04003FA9 RID: 16297
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000DC4 RID: 3524
	private enum SpinState
	{
		// Token: 0x04003FAB RID: 16299
		None,
		// Token: 0x04003FAC RID: 16300
		ArmsIn,
		// Token: 0x04003FAD RID: 16301
		GoToStartPoint,
		// Token: 0x04003FAE RID: 16302
		BulletScript,
		// Token: 0x04003FAF RID: 16303
		ArmsOut
	}
}
