using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D42 RID: 3394
public class ShootBeamBehavior : BasicAttackBehavior
{
	// Token: 0x060047B3 RID: 18355 RVA: 0x00179664 File Offset: 0x00177864
	private bool ShowSpecificBeamShooter()
	{
		return this.beamSelection == ShootBeamBehavior.BeamSelection.Specify;
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x00179670 File Offset: 0x00177870
	private bool ShowFollowVars()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.Follow;
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x0017967C File Offset: 0x0017787C
	private bool ShowDegRate()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.ConstantTurn || this.trackingType == ShootBeamBehavior.TrackingType.AccelTurn;
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x00179698 File Offset: 0x00177898
	private bool ShowDegAccel()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.AccelTurn;
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x001796A4 File Offset: 0x001778A4
	private bool ShowDegCatchUpVars()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.Follow && this.useDegreeCatchUp;
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x001796BC File Offset: 0x001778BC
	private bool ShowUnitCatchUpVars()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.Follow && this.useUnitCatchUp;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x001796D4 File Offset: 0x001778D4
	private bool ShowUnitOvershootVars()
	{
		return this.trackingType == ShootBeamBehavior.TrackingType.Follow && this.useUnitOvershoot;
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x001796EC File Offset: 0x001778EC
	private bool ShowRandomInitialAimOffsetSign()
	{
		return this.initialAimOffset > 0f;
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x001796FC File Offset: 0x001778FC
	public override void Start()
	{
		base.Start();
		this.m_allBeamShooters = new List<AIBeamShooter>(this.m_aiActor.GetComponents<AIBeamShooter>());
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
			if (this.m_aiAnimator.ChildAnimator)
			{
				tk2dSpriteAnimator spriteAnimator2 = this.m_aiAnimator.ChildAnimator.spriteAnimator;
				spriteAnimator2.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator2.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
			}
		}
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x001797A8 File Offset: 0x001779A8
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_aiActor.TargetRigidbody)
		{
			this.m_targetPosition = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			this.m_backupTarget = this.m_aiActor.TargetRigidbody;
		}
		else if (this.m_backupTarget)
		{
			this.m_targetPosition = this.m_backupTarget.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x00179820 File Offset: 0x00177A20
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.TellAnimation, true, null, -1f, false);
			this.state = ShootBeamBehavior.State.WaitingForTell;
		}
		else
		{
			this.Fire();
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x00179894 File Offset: 0x00177A94
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.state == ShootBeamBehavior.State.WaitingForTell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
			{
				this.Fire();
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.state == ShootBeamBehavior.State.Firing)
		{
			this.m_firingTime += this.m_deltaTime;
			this.m_timer -= this.m_deltaTime;
			if (this.m_timer > 0f && this.m_currentBeamShooters[0].IsFiringLaser)
			{
				float num = 0f;
				if (this.trackingType == ShootBeamBehavior.TrackingType.Follow)
				{
					AIBeamShooter aibeamShooter = this.m_currentBeamShooters[0];
					Vector2 laserFiringCenter = aibeamShooter.LaserFiringCenter;
					float num2 = Vector2.Distance(this.m_targetPosition, laserFiringCenter);
					num2 = Mathf.Max(this.minUnitRadius, num2);
					float num3 = (this.m_targetPosition - laserFiringCenter).ToAngle();
					float num4 = BraveMathCollege.ClampAngle180(num3 - aibeamShooter.LaserAngle);
					float num5 = num4 * num2 * 0.017453292f;
					float num6 = this.maxUnitTurnRate;
					float num7 = Mathf.Sign(num4);
					if (this.m_unitOvershootTimer > 0f)
					{
						num7 = this.m_unitOvershootFixedDirection;
						this.m_unitOvershootTimer -= this.m_deltaTime;
						num6 = this.unitOvershootSpeed;
					}
					this.m_currentUnitTurnRate = Mathf.Clamp(this.m_currentUnitTurnRate + num7 * this.unitTurnRateAcceleration * this.m_deltaTime, -num6, num6);
					float num8 = this.m_currentUnitTurnRate / num2 * 57.29578f;
					float num9 = 0f;
					if (this.useDegreeCatchUp && Mathf.Abs(num4) > this.minDegreesForCatchUp)
					{
						float num10 = Mathf.InverseLerp(this.minDegreesForCatchUp, 180f, Mathf.Abs(num4)) * this.degreeCatchUpSpeed;
						num9 = Mathf.Max(num9, num10);
					}
					if (this.useUnitCatchUp && Mathf.Abs(num5) > this.minUnitForCatchUp)
					{
						float num11 = Mathf.InverseLerp(this.minUnitForCatchUp, this.maxUnitForCatchUp, Mathf.Abs(num5)) * this.unitCatchUpSpeed;
						float num12 = num11 / num2 * 57.29578f;
						num9 = Mathf.Max(num9, num12);
					}
					if (this.useUnitOvershoot && Mathf.Abs(num5) < this.minUnitForOvershoot)
					{
						this.m_unitOvershootFixedDirection = (float)((this.m_currentUnitTurnRate <= 0f) ? (-1) : 1);
						this.m_unitOvershootTimer = this.unitOvershootTime;
					}
					num9 *= Mathf.Sign(num4);
					num = num8 + num9;
				}
				else if (this.trackingType == ShootBeamBehavior.TrackingType.ConstantTurn)
				{
					num = this.maxDegTurnRate;
				}
				else if (this.trackingType == ShootBeamBehavior.TrackingType.AccelTurn)
				{
					this.m_currentDegTurnRate = Mathf.Clamp(this.m_currentDegTurnRate + this.degTurnRateAcceleration * this.m_deltaTime, -this.maxDegTurnRate, this.maxDegTurnRate);
					num = this.m_currentDegTurnRate;
				}
				for (int i = 0; i < this.m_currentBeamShooters.Count; i++)
				{
					AIBeamShooter aibeamShooter2 = this.m_currentBeamShooters[i];
					aibeamShooter2.LaserAngle = BraveMathCollege.ClampAngle360(aibeamShooter2.LaserAngle + num * this.m_deltaTime);
					if (this.restrictBeamLengthToAim && this.m_aiActor.TargetRigidbody)
					{
						float magnitude = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - aibeamShooter2.LaserFiringCenter).magnitude;
						aibeamShooter2.MaxBeamLength = magnitude + this.beamLengthOFfset;
						if (this.beamLengthSinMagnitude > 0f && this.beamLengthSinPeriod > 0f)
						{
							aibeamShooter2.MaxBeamLength += Mathf.Sin(this.m_firingTime / this.beamLengthSinPeriod * 3.1415927f) * this.beamLengthSinMagnitude;
							if (aibeamShooter2.MaxBeamLength < 0f)
							{
								aibeamShooter2.MaxBeamLength = 0f;
							}
						}
					}
				}
				return ContinuousBehaviorResult.Continue;
			}
			this.StopLasers();
			if (!string.IsNullOrEmpty(this.PostFireAnimation))
			{
				this.state = ShootBeamBehavior.State.WaitingForPostAnim;
				this.m_aiAnimator.PlayUntilFinished(this.PostFireAnimation, false, null, -1f, false);
				return ContinuousBehaviorResult.Continue;
			}
			return ContinuousBehaviorResult.Finished;
		}
		else
		{
			if (this.state == ShootBeamBehavior.State.WaitingForPostAnim)
			{
				return (!this.m_aiAnimator.IsPlaying(this.PostFireAnimation)) ? ContinuousBehaviorResult.Finished : ContinuousBehaviorResult.Continue;
			}
			return ContinuousBehaviorResult.Continue;
		}
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x00179CE4 File Offset: 0x00177EE4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.TellAnimation);
		}
		if (!string.IsNullOrEmpty(this.FireAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.FireAnimation);
		}
		if (!string.IsNullOrEmpty(this.PostFireAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.PostFireAnimation);
		}
		this.StopLasers();
		this.state = ShootBeamBehavior.State.Idle;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x00179D84 File Offset: 0x00177F84
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		this.StopLasers();
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x00179D94 File Offset: 0x00177F94
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (this.state == ShootBeamBehavior.State.WaitingForTell && frame.eventInfo == "fire")
		{
			this.Fire();
		}
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x00179DD0 File Offset: 0x00177FD0
	private void Fire()
	{
		if (!string.IsNullOrEmpty(this.FireAnimation))
		{
			this.m_aiAnimator.EndAnimation();
			this.m_aiAnimator.PlayUntilFinished(this.FireAnimation, false, null, -1f, false);
		}
		if (this.stopWhileFiring)
		{
			this.m_aiActor.ClearPath();
		}
		if (this.beamSelection == ShootBeamBehavior.BeamSelection.All)
		{
			this.m_currentBeamShooters.AddRange(this.m_allBeamShooters);
		}
		else if (this.beamSelection == ShootBeamBehavior.BeamSelection.Random)
		{
			this.m_currentBeamShooters.Add(BraveUtility.RandomElement<AIBeamShooter>(this.m_allBeamShooters));
		}
		else if (this.beamSelection == ShootBeamBehavior.BeamSelection.Specify)
		{
			this.m_currentBeamShooters.Add(this.specificBeamShooter);
		}
		float facingDirection = this.m_currentBeamShooters[0].CurrentAiAnimator.FacingDirection;
		float num = ((!this.randomInitialAimOffsetSign) ? 1f : BraveUtility.RandomSign());
		for (int i = 0; i < this.m_currentBeamShooters.Count; i++)
		{
			AIBeamShooter aibeamShooter = this.m_currentBeamShooters[i];
			if (this.restrictBeamLengthToAim && this.m_aiActor.TargetRigidbody)
			{
				float magnitude = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - aibeamShooter.LaserFiringCenter).magnitude;
				aibeamShooter.MaxBeamLength = magnitude;
			}
			float num2 = 0f;
			if (this.initialAimType == ShootBeamBehavior.InitialAimType.FacingDirection)
			{
				num2 = facingDirection;
			}
			else if (this.initialAimType == ShootBeamBehavior.InitialAimType.Aim)
			{
				if (this.m_aiActor.TargetRigidbody)
				{
					num2 = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - aibeamShooter.LaserFiringCenter).ToAngle();
				}
			}
			else if (this.initialAimType == ShootBeamBehavior.InitialAimType.Absolute)
			{
				num2 = 0f;
			}
			else if (this.initialAimType == ShootBeamBehavior.InitialAimType.Transform)
			{
				num2 = aibeamShooter.beamTransform.eulerAngles.z;
			}
			num2 += num * this.initialAimOffset;
			aibeamShooter.StartFiringLaser(num2);
		}
		this.m_timer = this.firingTime;
		this.m_currentUnitTurnRate = 0f;
		this.m_currentDegTurnRate = 0f;
		this.m_firingTime = 0f;
		this.state = ShootBeamBehavior.State.Firing;
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x0017A020 File Offset: 0x00178220
	private void StopLasers()
	{
		for (int i = 0; i < this.m_currentBeamShooters.Count; i++)
		{
			this.m_currentBeamShooters[i].StopFiringLaser();
		}
		this.m_currentBeamShooters.Clear();
	}

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x060047C4 RID: 18372 RVA: 0x0017A068 File Offset: 0x00178268
	// (set) Token: 0x060047C5 RID: 18373 RVA: 0x0017A070 File Offset: 0x00178270
	private ShootBeamBehavior.State state
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

	// Token: 0x060047C6 RID: 18374 RVA: 0x0017A0A0 File Offset: 0x001782A0
	private void BeginState(ShootBeamBehavior.State state)
	{
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x0017A0A4 File Offset: 0x001782A4
	private void EndState(ShootBeamBehavior.State state)
	{
	}

	// Token: 0x04003ADC RID: 15068
	public ShootBeamBehavior.BeamSelection beamSelection;

	// Token: 0x04003ADD RID: 15069
	[InspectorShowIf("ShowSpecificBeamShooter")]
	public AIBeamShooter specificBeamShooter;

	// Token: 0x04003ADE RID: 15070
	public float firingTime;

	// Token: 0x04003ADF RID: 15071
	public bool stopWhileFiring;

	// Token: 0x04003AE0 RID: 15072
	public ShootBeamBehavior.InitialAimType initialAimType;

	// Token: 0x04003AE1 RID: 15073
	public float initialAimOffset;

	// Token: 0x04003AE2 RID: 15074
	[InspectorIndent]
	[InspectorShowIf("ShowRandomInitialAimOffsetSign")]
	public bool randomInitialAimOffsetSign;

	// Token: 0x04003AE3 RID: 15075
	public bool restrictBeamLengthToAim;

	// Token: 0x04003AE4 RID: 15076
	[InspectorIndent]
	[InspectorShowIf("restrictBeamLengthToAim")]
	public float beamLengthOFfset;

	// Token: 0x04003AE5 RID: 15077
	[InspectorIndent]
	[InspectorShowIf("restrictBeamLengthToAim")]
	public float beamLengthSinMagnitude;

	// Token: 0x04003AE6 RID: 15078
	[InspectorIndent]
	[InspectorShowIf("restrictBeamLengthToAim")]
	public float beamLengthSinPeriod;

	// Token: 0x04003AE7 RID: 15079
	[InspectorHeader("Tracking")]
	public ShootBeamBehavior.TrackingType trackingType;

	// Token: 0x04003AE8 RID: 15080
	[InspectorShowIf("ShowFollowVars")]
	public float maxUnitTurnRate;

	// Token: 0x04003AE9 RID: 15081
	[InspectorShowIf("ShowFollowVars")]
	public float unitTurnRateAcceleration;

	// Token: 0x04003AEA RID: 15082
	[InspectorShowIf("ShowFollowVars")]
	public float minUnitRadius = 5f;

	// Token: 0x04003AEB RID: 15083
	[InspectorShowIf("ShowFollowVars")]
	public bool useDegreeCatchUp;

	// Token: 0x04003AEC RID: 15084
	[InspectorShowIf("ShowDegCatchUpVars")]
	[InspectorIndent]
	public float minDegreesForCatchUp;

	// Token: 0x04003AED RID: 15085
	[InspectorIndent]
	[InspectorShowIf("ShowDegCatchUpVars")]
	public float degreeCatchUpSpeed;

	// Token: 0x04003AEE RID: 15086
	[InspectorShowIf("ShowFollowVars")]
	public bool useUnitCatchUp;

	// Token: 0x04003AEF RID: 15087
	[InspectorShowIf("ShowUnitCatchUpVars")]
	[InspectorIndent]
	public float minUnitForCatchUp;

	// Token: 0x04003AF0 RID: 15088
	[InspectorShowIf("ShowUnitCatchUpVars")]
	[InspectorIndent]
	public float maxUnitForCatchUp;

	// Token: 0x04003AF1 RID: 15089
	[InspectorShowIf("ShowUnitCatchUpVars")]
	[InspectorIndent]
	public float unitCatchUpSpeed;

	// Token: 0x04003AF2 RID: 15090
	[InspectorShowIf("ShowFollowVars")]
	public bool useUnitOvershoot;

	// Token: 0x04003AF3 RID: 15091
	[InspectorIndent]
	[InspectorShowIf("ShowUnitOvershootVars")]
	public float minUnitForOvershoot;

	// Token: 0x04003AF4 RID: 15092
	[InspectorShowIf("ShowUnitOvershootVars")]
	[InspectorIndent]
	public float unitOvershootTime;

	// Token: 0x04003AF5 RID: 15093
	[InspectorShowIf("ShowUnitOvershootVars")]
	[InspectorIndent]
	public float unitOvershootSpeed;

	// Token: 0x04003AF6 RID: 15094
	[InspectorShowIf("ShowDegRate")]
	public float maxDegTurnRate;

	// Token: 0x04003AF7 RID: 15095
	[InspectorShowIf("ShowDegAccel")]
	public float degTurnRateAcceleration;

	// Token: 0x04003AF8 RID: 15096
	[InspectorCategory("Visuals")]
	public string TellAnimation;

	// Token: 0x04003AF9 RID: 15097
	[InspectorCategory("Visuals")]
	public string FireAnimation;

	// Token: 0x04003AFA RID: 15098
	[InspectorCategory("Visuals")]
	public string PostFireAnimation;

	// Token: 0x04003AFB RID: 15099
	private List<AIBeamShooter> m_allBeamShooters;

	// Token: 0x04003AFC RID: 15100
	private readonly List<AIBeamShooter> m_currentBeamShooters = new List<AIBeamShooter>();

	// Token: 0x04003AFD RID: 15101
	private float m_timer;

	// Token: 0x04003AFE RID: 15102
	private float m_firingTime;

	// Token: 0x04003AFF RID: 15103
	private Vector2 m_targetPosition;

	// Token: 0x04003B00 RID: 15104
	private float m_currentUnitTurnRate;

	// Token: 0x04003B01 RID: 15105
	private float m_currentDegTurnRate;

	// Token: 0x04003B02 RID: 15106
	private float m_unitOvershootFixedDirection;

	// Token: 0x04003B03 RID: 15107
	private float m_unitOvershootTimer;

	// Token: 0x04003B04 RID: 15108
	private SpeculativeRigidbody m_backupTarget;

	// Token: 0x04003B05 RID: 15109
	private ShootBeamBehavior.State m_state;

	// Token: 0x02000D43 RID: 3395
	public enum BeamSelection
	{
		// Token: 0x04003B07 RID: 15111
		All,
		// Token: 0x04003B08 RID: 15112
		Random,
		// Token: 0x04003B09 RID: 15113
		Specify
	}

	// Token: 0x02000D44 RID: 3396
	public enum TrackingType
	{
		// Token: 0x04003B0B RID: 15115
		Follow,
		// Token: 0x04003B0C RID: 15116
		ConstantTurn,
		// Token: 0x04003B0D RID: 15117
		AccelTurn
	}

	// Token: 0x02000D45 RID: 3397
	public enum InitialAimType
	{
		// Token: 0x04003B0F RID: 15119
		FacingDirection,
		// Token: 0x04003B10 RID: 15120
		Aim,
		// Token: 0x04003B11 RID: 15121
		Absolute,
		// Token: 0x04003B12 RID: 15122
		Transform
	}

	// Token: 0x02000D46 RID: 3398
	private enum State
	{
		// Token: 0x04003B14 RID: 15124
		Idle,
		// Token: 0x04003B15 RID: 15125
		WaitingForTell,
		// Token: 0x04003B16 RID: 15126
		Firing,
		// Token: 0x04003B17 RID: 15127
		WaitingForPostAnim
	}
}
