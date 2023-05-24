using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC5 RID: 3525
[InspectorDropdownName("Bosses/Infinilich/SpinBeamsBehavior")]
public class InfinilichSpinBeamsBehavior : BasicAttackBehavior
{
	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06004AC2 RID: 19138 RVA: 0x00192564 File Offset: 0x00190764
	// (set) Token: 0x06004AC3 RID: 19139 RVA: 0x0019256C File Offset: 0x0019076C
	private InfinilichSpinBeamsBehavior.SpinState State
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

	// Token: 0x06004AC4 RID: 19140 RVA: 0x0019259C File Offset: 0x0019079C
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
		this.State = InfinilichSpinBeamsBehavior.SpinState.ArmsIn;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AC5 RID: 19141 RVA: 0x001925EC File Offset: 0x001907EC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == InfinilichSpinBeamsBehavior.SpinState.ArmsIn)
		{
			if (!this.m_aiAnimator.IsPlaying("arms_in"))
			{
				this.m_startPoint = this.m_aiActor.specRigidbody.UnitCenter;
				this.m_targetPoint = this.m_aiActor.ParentRoom.area.Center + new Vector2(0f, 11f);
				Vector2 vector = this.m_targetPoint - this.m_startPoint;
				float magnitude = vector.magnitude;
				this.m_setupTime = Mathf.Min(this.SetupTime, 1.5f * magnitude / this.FlightSpeed);
				this.m_aiAnimator.FacingDirection = vector.ToAngle();
				this.State = InfinilichSpinBeamsBehavior.SpinState.GoToStartPoint;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.State == InfinilichSpinBeamsBehavior.SpinState.GoToStartPoint)
		{
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			Vector2 vector2 = Vector2Extensions.SmoothStep(this.m_startPoint, this.m_targetPoint, this.m_setupTimer / this.m_setupTime);
			if (this.m_setupTimer > this.m_setupTime)
			{
				this.m_aiActor.BehaviorVelocity = Vector2.zero;
				this.m_targetPoint = this.m_aiActor.ParentRoom.area.Center - new Vector2(0f, 11f);
				this.m_futureTargets.Clear();
				this.m_futureTargets.Add(this.m_aiActor.ParentRoom.area.Center + new Vector2(0f, 11f));
				this.m_futureTargets.Add(this.m_aiActor.ParentRoom.area.Center);
				this.State = InfinilichSpinBeamsBehavior.SpinState.BeamMode;
				return ContinuousBehaviorResult.Continue;
			}
			this.m_aiActor.BehaviorVelocity = (vector2 - unitCenter) / BraveTime.DeltaTime;
			this.m_aiAnimator.FacingDirection = this.m_aiActor.BehaviorVelocity.ToAngle();
			this.m_setupTimer += this.m_deltaTime;
		}
		else if (this.State == InfinilichSpinBeamsBehavior.SpinState.BeamMode)
		{
			Vector2 vector3 = this.m_targetPoint - this.m_aiActor.specRigidbody.UnitCenter;
			float magnitude2 = vector3.magnitude;
			if (magnitude2 < 0.1f)
			{
				if (this.m_futureTargets.Count > 0)
				{
					this.m_targetPoint = this.m_futureTargets[0];
					this.m_futureTargets.RemoveAt(0);
					this.m_aiActor.BehaviorVelocity = Vector2.zero;
					return ContinuousBehaviorResult.Continue;
				}
				this.m_aiActor.BehaviorVelocity = Vector2.zero;
				this.State = InfinilichSpinBeamsBehavior.SpinState.ArmsOut;
				return ContinuousBehaviorResult.Continue;
			}
			else
			{
				float num = this.FlightSpeed;
				if (magnitude2 < this.FlightSpeed * this.m_deltaTime)
				{
					num = magnitude2 / this.m_deltaTime;
				}
				this.m_aiActor.BehaviorVelocity = vector3.WithX(0f).normalized * num;
			}
		}
		else if (this.State == InfinilichSpinBeamsBehavior.SpinState.ArmsOut && !this.m_aiAnimator.IsPlaying("arms_out"))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AC6 RID: 19142 RVA: 0x00192910 File Offset: 0x00190B10
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.State = InfinilichSpinBeamsBehavior.SpinState.None;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x00192938 File Offset: 0x00190B38
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

	// Token: 0x06004AC8 RID: 19144 RVA: 0x00192998 File Offset: 0x00190B98
	private void BeginState(InfinilichSpinBeamsBehavior.SpinState state)
	{
		if (state == InfinilichSpinBeamsBehavior.SpinState.ArmsIn)
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilCancelled("arms_in", false, null, -1f, false);
		}
		else if (state == InfinilichSpinBeamsBehavior.SpinState.GoToStartPoint)
		{
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.PlayUntilCancelled("spin", false, null, -1f, false);
			this.m_setupTimer = 0f;
		}
		else if (state == InfinilichSpinBeamsBehavior.SpinState.BeamMode)
		{
			this.m_aiAnimator.FacingDirection = -90f;
			this.Fire();
		}
		else if (state == InfinilichSpinBeamsBehavior.SpinState.ArmsOut)
		{
			this.m_aiAnimator.PlayUntilFinished("arms_out", false, null, -1f, false);
		}
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x00192A70 File Offset: 0x00190C70
	private void EndState(InfinilichSpinBeamsBehavior.SpinState state)
	{
		if (state == InfinilichSpinBeamsBehavior.SpinState.BeamMode && this.m_bulletSource && !this.m_bulletSource.IsEnded)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x04003FB0 RID: 16304
	public float SetupTime = 1f;

	// Token: 0x04003FB1 RID: 16305
	public float FlightSpeed = 6f;

	// Token: 0x04003FB2 RID: 16306
	public GameObject ShootPoint;

	// Token: 0x04003FB3 RID: 16307
	public BulletScriptSelector BulletScript;

	// Token: 0x04003FB4 RID: 16308
	private InfinilichSpinBeamsBehavior.SpinState m_state;

	// Token: 0x04003FB5 RID: 16309
	private Vector2 m_startPoint;

	// Token: 0x04003FB6 RID: 16310
	private Vector2 m_targetPoint;

	// Token: 0x04003FB7 RID: 16311
	private List<Vector2> m_futureTargets = new List<Vector2>();

	// Token: 0x04003FB8 RID: 16312
	private float m_setupTime;

	// Token: 0x04003FB9 RID: 16313
	private float m_setupTimer;

	// Token: 0x04003FBA RID: 16314
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000DC6 RID: 3526
	private enum SpinState
	{
		// Token: 0x04003FBC RID: 16316
		None,
		// Token: 0x04003FBD RID: 16317
		ArmsIn,
		// Token: 0x04003FBE RID: 16318
		GoToStartPoint,
		// Token: 0x04003FBF RID: 16319
		BeamMode,
		// Token: 0x04003FC0 RID: 16320
		ArmsOut
	}
}
