using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000DCD RID: 3533
[InspectorDropdownName("Bosses/MetalGearRat/BeamsBehavior")]
public class MetalGearRatBeamsBehavior : BasicAttackBehavior
{
	// Token: 0x06004AE2 RID: 19170 RVA: 0x00193B88 File Offset: 0x00191D88
	public override void Start()
	{
		base.Start();
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
		this.m_roomLowerLeft = this.m_aiActor.ParentRoom.area.UnitBottomLeft;
		this.m_roomUpperRight = this.m_aiActor.ParentRoom.area.UnitTopRight + new Vector2(0f, 3f);
	}

	// Token: 0x06004AE3 RID: 19171 RVA: 0x00193C68 File Offset: 0x00191E68
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_slitherCounter += this.m_deltaTime * this.m_aiActor.behaviorSpeculator.CooldownScale;
	}

	// Token: 0x06004AE4 RID: 19172 RVA: 0x00193C94 File Offset: 0x00191E94
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
			this.state = MetalGearRatBeamsBehavior.State.WaitingForTell;
		}
		else
		{
			this.Fire();
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AE5 RID: 19173 RVA: 0x00193D08 File Offset: 0x00191F08
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.state == MetalGearRatBeamsBehavior.State.WaitingForTell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
			{
				this.Fire();
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.state == MetalGearRatBeamsBehavior.State.Firing)
		{
			this.m_timer -= this.m_deltaTime;
			this.m_moveSpeed += this.targetMoveAcceleration * this.m_deltaTime;
			if (this.m_timer > 0f && this.specificBeamShooters[0].IsFiringLaser)
			{
				for (int i = 0; i < this.specificBeamShooters.Count; i++)
				{
					AIBeamShooter aibeamShooter = this.specificBeamShooters[i];
					Vector2? vector = null;
					if (this.m_targetData[i].hasFixedTarget)
					{
						vector = new Vector2?(this.m_targetData[i].fixedTarget);
						MetalGearRatBeamsBehavior.TargetData[] targetData = this.m_targetData;
						int num = i;
						targetData[num].fixedTargetTimer = targetData[num].fixedTargetTimer - this.m_deltaTime;
						if (this.m_targetData[i].fixedTargetTimer <= 0f)
						{
							this.m_targetData[i].fixedTarget = this.RandomTargetPosition();
							this.m_targetData[i].fixedTargetTimer = UnityEngine.Random.Range(this.randomRetargetMin, this.randomRetargetMax);
						}
					}
					else if (this.m_targetData[i].targetRigidbody)
					{
						vector = new Vector2?(this.m_targetData[i].targetRigidbody.GetUnitCenter(ColliderType.HitBox));
					}
					if (vector != null)
					{
						Vector2 pos = this.m_targetData[i].pos;
						float num2 = (vector.Value - pos).ToAngle();
						this.m_targetData[i].direction = Mathf.SmoothDampAngle(this.m_targetData[i].direction, num2, ref this.m_targetData[i].angularVelocity, this.turnTime);
					}
					this.m_targetData[i].slitherDirection = Mathf.Sin(this.m_slitherCounter * 3.1415927f / this.slitherPeriod) * this.slitherMagnitude;
					Vector2 vector2 = BraveMathCollege.DegreesToVector(this.m_targetData[i].direction + this.m_targetData[i].slitherDirection, this.m_moveSpeed);
					MetalGearRatBeamsBehavior.TargetData[] targetData2 = this.m_targetData;
					int num3 = i;
					targetData2[num3].pos = targetData2[num3].pos + vector2 * this.m_deltaTime;
					this.m_targetData[i].pos = Vector2Extensions.Clamp(this.m_targetData[i].pos, this.m_roomLowerLeft, this.m_roomUpperRight);
					Vector2 vector3 = this.m_targetData[i].pos - aibeamShooter.LaserFiringCenter;
					aibeamShooter.LaserAngle = vector3.ToAngle();
					aibeamShooter.MaxBeamLength = vector3.magnitude;
				}
				return ContinuousBehaviorResult.Continue;
			}
			this.StopLasers();
			if (!string.IsNullOrEmpty(this.PostFireAnimation))
			{
				this.state = MetalGearRatBeamsBehavior.State.WaitingForPostAnim;
				this.m_aiAnimator.PlayUntilFinished(this.PostFireAnimation, false, null, -1f, false);
				return ContinuousBehaviorResult.Continue;
			}
			return ContinuousBehaviorResult.Finished;
		}
		else
		{
			if (this.state == MetalGearRatBeamsBehavior.State.WaitingForPostAnim)
			{
				return (!this.m_aiAnimator.IsPlaying(this.PostFireAnimation)) ? ContinuousBehaviorResult.Finished : ContinuousBehaviorResult.Continue;
			}
			return ContinuousBehaviorResult.Continue;
		}
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x00194084 File Offset: 0x00192284
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
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x0019411C File Offset: 0x0019231C
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		this.StopLasers();
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x0019412C File Offset: 0x0019232C
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (this.state == MetalGearRatBeamsBehavior.State.WaitingForTell && frame.eventInfo == "fire")
		{
			this.Fire();
		}
	}

	// Token: 0x06004AE9 RID: 19177 RVA: 0x00194168 File Offset: 0x00192368
	private void Fire()
	{
		this.m_moveSpeed = this.targetMoveSpeed;
		this.m_slitherCounter = 0f;
		if (!string.IsNullOrEmpty(this.FireAnimation))
		{
			this.m_aiAnimator.EndAnimation();
			this.m_aiAnimator.PlayUntilFinished(this.FireAnimation, false, null, -1f, false);
		}
		if (this.stopWhileFiring)
		{
			this.m_aiActor.ClearPath();
		}
		this.m_targetData = new MetalGearRatBeamsBehavior.TargetData[this.specificBeamShooters.Count];
		for (int i = 0; i < this.specificBeamShooters.Count; i++)
		{
			AIBeamShooter aibeamShooter = this.specificBeamShooters[i];
			aibeamShooter.IgnoreAiActorPlayerChecks = true;
			Vector2 vector = this.RandomTargetPosition();
			this.m_targetData[i] = new MetalGearRatBeamsBehavior.TargetData
			{
				pos = vector,
				direction = BraveUtility.RandomAngle()
			};
			Vector2 vector2 = vector - aibeamShooter.LaserFiringCenter;
			aibeamShooter.MaxBeamLength = vector2.magnitude;
			if (i < this.randomTargets)
			{
				this.m_targetData[i].hasFixedTarget = true;
				this.m_targetData[i].fixedTarget = this.RandomTargetPosition();
				this.m_targetData[i].fixedTargetTimer = UnityEngine.Random.Range(this.randomRetargetMin, this.randomRetargetMax);
			}
			else
			{
				PlayerController randomActivePlayer = GameManager.Instance.GetRandomActivePlayer();
				if (randomActivePlayer && randomActivePlayer.specRigidbody)
				{
					this.m_targetData[i].targetRigidbody = randomActivePlayer.specRigidbody;
				}
			}
			aibeamShooter.StartFiringLaser(vector2.ToAngle());
		}
		this.m_timer = this.firingTime;
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
		this.state = MetalGearRatBeamsBehavior.State.Firing;
	}

	// Token: 0x06004AEA RID: 19178 RVA: 0x00194384 File Offset: 0x00192584
	private void StopLasers()
	{
		for (int i = 0; i < this.specificBeamShooters.Count; i++)
		{
			this.specificBeamShooters[i].StopFiringLaser();
		}
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x001943DC File Offset: 0x001925DC
	private Vector2 RandomTargetPosition()
	{
		Vector2 vector = this.m_roomLowerLeft + new Vector2(1f, 3f);
		Vector2 vector2 = this.m_roomUpperRight.WithY(this.m_aiActor.transform.position.y) - new Vector2(1f, 0f);
		return BraveUtility.RandomVector2(vector, vector2);
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06004AEC RID: 19180 RVA: 0x00194448 File Offset: 0x00192648
	// (set) Token: 0x06004AED RID: 19181 RVA: 0x00194450 File Offset: 0x00192650
	private MetalGearRatBeamsBehavior.State state
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

	// Token: 0x06004AEE RID: 19182 RVA: 0x00194480 File Offset: 0x00192680
	private void BeginState(MetalGearRatBeamsBehavior.State state)
	{
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x00194484 File Offset: 0x00192684
	private void EndState(MetalGearRatBeamsBehavior.State state)
	{
	}

	// Token: 0x04003FF4 RID: 16372
	public List<AIBeamShooter> specificBeamShooters;

	// Token: 0x04003FF5 RID: 16373
	public float firingTime;

	// Token: 0x04003FF6 RID: 16374
	public bool stopWhileFiring;

	// Token: 0x04003FF7 RID: 16375
	public float turnTime = 1f;

	// Token: 0x04003FF8 RID: 16376
	public float slitherPeriod;

	// Token: 0x04003FF9 RID: 16377
	public float slitherMagnitude;

	// Token: 0x04003FFA RID: 16378
	public float targetMoveSpeed = 3f;

	// Token: 0x04003FFB RID: 16379
	public float targetMoveAcceleration = 0.25f;

	// Token: 0x04003FFC RID: 16380
	public int randomTargets = 2;

	// Token: 0x04003FFD RID: 16381
	public float randomRetargetMin = 1f;

	// Token: 0x04003FFE RID: 16382
	public float randomRetargetMax = 2f;

	// Token: 0x04003FFF RID: 16383
	public BulletScriptSelector BulletScript;

	// Token: 0x04004000 RID: 16384
	public Transform ShootPoint;

	// Token: 0x04004001 RID: 16385
	[InspectorCategory("Visuals")]
	public string TellAnimation;

	// Token: 0x04004002 RID: 16386
	[InspectorCategory("Visuals")]
	public string FireAnimation;

	// Token: 0x04004003 RID: 16387
	[InspectorCategory("Visuals")]
	public string PostFireAnimation;

	// Token: 0x04004004 RID: 16388
	private MetalGearRatBeamsBehavior.TargetData[] m_targetData;

	// Token: 0x04004005 RID: 16389
	private float m_timer;

	// Token: 0x04004006 RID: 16390
	private float m_slitherCounter;

	// Token: 0x04004007 RID: 16391
	private float m_moveSpeed;

	// Token: 0x04004008 RID: 16392
	private Vector2 m_roomLowerLeft;

	// Token: 0x04004009 RID: 16393
	private Vector2 m_roomUpperRight;

	// Token: 0x0400400A RID: 16394
	private BulletScriptSource m_bulletSource;

	// Token: 0x0400400B RID: 16395
	private MetalGearRatBeamsBehavior.State m_state;

	// Token: 0x02000DCE RID: 3534
	private struct TargetData
	{
		// Token: 0x0400400C RID: 16396
		public Vector2 pos;

		// Token: 0x0400400D RID: 16397
		public float slitherCounter;

		// Token: 0x0400400E RID: 16398
		public float direction;

		// Token: 0x0400400F RID: 16399
		public float slitherDirection;

		// Token: 0x04004010 RID: 16400
		public float angularVelocity;

		// Token: 0x04004011 RID: 16401
		public bool hasFixedTarget;

		// Token: 0x04004012 RID: 16402
		public Vector2 fixedTarget;

		// Token: 0x04004013 RID: 16403
		public float fixedTargetTimer;

		// Token: 0x04004014 RID: 16404
		public SpeculativeRigidbody targetRigidbody;
	}

	// Token: 0x02000DCF RID: 3535
	private enum State
	{
		// Token: 0x04004016 RID: 16406
		Idle,
		// Token: 0x04004017 RID: 16407
		WaitingForTell,
		// Token: 0x04004018 RID: 16408
		Firing,
		// Token: 0x04004019 RID: 16409
		WaitingForPostAnim
	}
}
