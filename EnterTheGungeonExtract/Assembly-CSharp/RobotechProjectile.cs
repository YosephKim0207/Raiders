using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001670 RID: 5744
public class RobotechProjectile : Projectile
{
	// Token: 0x060085F5 RID: 34293 RVA: 0x003759BC File Offset: 0x00373BBC
	public override void Start()
	{
		base.Start();
		this.m_usesNormalMoveRegardless = true;
		if (this.initialOverrideTargetPoint != null)
		{
			this.m_targetPoint = this.initialOverrideTargetPoint.Value;
			this.m_mode = RobotechProjectile.Mode.InitialTarget;
		}
		else if (base.Owner is PlayerController)
		{
			if (BraveInput.GetInstanceForPlayer((base.Owner as PlayerController).PlayerIDX).IsKeyboardAndMouse(false))
			{
				Camera component = GameManager.Instance.MainCameraController.GetComponent<Camera>();
				Ray ray = component.ScreenPointToRay(Input.mousePosition);
				Plane plane = new Plane(Vector3.forward, Vector3.zero);
				float num;
				if (plane.Raycast(ray, out num))
				{
					this.m_targetPoint = ray.GetPoint(num);
					this.m_targetPoint += UnityEngine.Random.insideUnitCircle.normalized * 0.7f;
				}
			}
			else
			{
				this.m_targetPoint = (base.Owner as PlayerController).unadjustedAimPoint;
			}
			this.m_mode = RobotechProjectile.Mode.InitialTarget;
		}
		else if (base.Owner is DumbGunShooter)
		{
			this.m_targetPoint = base.Owner.transform.position + Vector3.right * 50f;
			this.m_mode = RobotechProjectile.Mode.InitialTarget;
		}
		else
		{
			this.m_currentTarget = GameManager.Instance.GetPlayerClosestToPoint(base.Owner.transform.position.XY());
			this.m_mode = RobotechProjectile.Mode.TargetLocked;
		}
		if (this.initialDumfireTime > 0f)
		{
			this.m_mode = RobotechProjectile.Mode.InitialDumbfire;
		}
		TrailController componentInChildren = base.GetComponentInChildren<TrailController>();
		if (componentInChildren)
		{
			Vector2 vector = this.m_targetPoint - base.transform.position.XY();
			float num2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (Mathf.Abs(num2) > 90f)
			{
				componentInChildren.FlipUvsY = true;
			}
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		this.m_targetSearchTimer = this.searchTime;
		if (base.Owner is DumbGunShooter)
		{
			this.m_targetSearchTimer = -1000000f;
		}
		base.UpdateCollisionMask();
	}

	// Token: 0x060085F6 RID: 34294 RVA: 0x00375C20 File Offset: 0x00373E20
	public override void Update()
	{
		if (this.m_mode == RobotechProjectile.Mode.InitialDumbfire)
		{
			this.m_initialDumbfireTimer += base.LocalDeltaTime;
			if (this.m_initialDumbfireTimer > this.initialDumfireTime)
			{
				this.m_mode = ((!(base.Owner is PlayerController) && !(base.Owner is DumbGunShooter)) ? RobotechProjectile.Mode.TargetLocked : RobotechProjectile.Mode.InitialTarget);
			}
		}
		else if (this.m_mode == RobotechProjectile.Mode.InitialTarget)
		{
			this.m_targetSearchTimer += base.LocalDeltaTime;
			if (this.m_targetSearchTimer > this.searchTime && base.Owner && GameManager.HasInstance && !GameManager.Instance.IsLoadingLevel)
			{
				RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor));
				if (this.selectRandomAutoAimTarget && absoluteRoomFromPosition != null)
				{
					List<IAutoAimTarget> autoAimTargets = absoluteRoomFromPosition.GetAutoAimTargets();
					if (autoAimTargets != null && autoAimTargets.Count > 0)
					{
						this.m_currentTarget = BraveUtility.RandomElement<IAutoAimTarget>(autoAimTargets);
						this.m_mode = RobotechProjectile.Mode.TargetLocked;
						this.m_hasGoodLock = false;
					}
				}
				if (this.m_mode == RobotechProjectile.Mode.InitialTarget)
				{
					if (RobotechProjectile.s_activeEnemies == null)
					{
						RobotechProjectile.s_activeEnemies = new List<AIActor>();
					}
					else
					{
						RobotechProjectile.s_activeEnemies.Clear();
					}
					if (absoluteRoomFromPosition != null)
					{
						absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref RobotechProjectile.s_activeEnemies);
					}
					if (RobotechProjectile.s_activeEnemies.Count > 0)
					{
						if (this.targetAcquisitionRandom)
						{
							for (int i = 0; i < RobotechProjectile.s_activeEnemies.Count; i++)
							{
								AIActor aiactor = RobotechProjectile.s_activeEnemies[i];
								if (!aiactor || !aiactor.healthHaver || aiactor.healthHaver.IsDead || aiactor.IsGone)
								{
									RobotechProjectile.s_activeEnemies.RemoveAt(i);
									i--;
								}
							}
							if (RobotechProjectile.s_activeEnemies.Count > 0)
							{
								this.m_currentTarget = RobotechProjectile.s_activeEnemies[UnityEngine.Random.Range(0, RobotechProjectile.s_activeEnemies.Count)];
								this.m_mode = RobotechProjectile.Mode.TargetLocked;
								this.m_hasGoodLock = false;
							}
						}
						else
						{
							float num = float.MaxValue;
							for (int j = 0; j < RobotechProjectile.s_activeEnemies.Count; j++)
							{
								AIActor aiactor2 = RobotechProjectile.s_activeEnemies[j];
								if (aiactor2 && aiactor2.healthHaver && aiactor2.specRigidbody)
								{
									if (!aiactor2.healthHaver.IsDead)
									{
										if (!aiactor2.IsGone)
										{
											float num2 = Vector2.Distance(aiactor2.specRigidbody.UnitCenter, this.m_targetPoint);
											if (num2 < num)
											{
												this.m_currentTarget = aiactor2;
												num = num2;
												this.m_mode = RobotechProjectile.Mode.TargetLocked;
												this.m_hasGoodLock = false;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		else if (this.m_mode == RobotechProjectile.Mode.TargetLocked && (this.m_currentTarget == null || !this.m_currentTarget.IsValid))
		{
			if (this.reacquiresTargets)
			{
				this.m_mode = RobotechProjectile.Mode.InitialTarget;
			}
			else
			{
				this.m_mode = RobotechProjectile.Mode.Dumbfire;
			}
		}
		if (this.m_mode == RobotechProjectile.Mode.TargetLocked && this.m_currentTarget != null)
		{
			this.m_targetPoint = this.m_currentTarget.AimCenter;
		}
		if (this.canLoseTarget && (this.m_mode == RobotechProjectile.Mode.InitialTarget || this.m_mode == RobotechProjectile.Mode.TargetLocked) && this)
		{
			Vector2 vector = this.m_targetPoint - base.specRigidbody.UnitCenter;
			float num3 = BraveMathCollege.ClampAngle180(Vector3.Angle(vector, this.m_currentDirection));
			if (this.m_counterCurveState != RobotechProjectile.CounterCurveState.Active && this.m_counterCurveState != RobotechProjectile.CounterCurveState.Mandated)
			{
				if (!this.m_hasGoodLock && Mathf.Abs(num3) < 10f)
				{
					this.m_hasGoodLock = true;
				}
				else if (this.m_hasGoodLock && Mathf.Abs(num3) > 90f)
				{
					this.m_hasGoodLock = false;
					this.m_mode = RobotechProjectile.Mode.Dumbfire;
				}
			}
		}
		base.Update();
	}

	// Token: 0x060085F7 RID: 34295 RVA: 0x00376074 File Offset: 0x00374274
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060085F8 RID: 34296 RVA: 0x0037607C File Offset: 0x0037427C
	public void ForceCurveDirection(Vector2 dirVec, float duration)
	{
		this.m_counterCurveState = RobotechProjectile.CounterCurveState.Mandated;
		this.m_counterCurveMandatedDirection = dirVec;
		this.counterCurveDuration = duration;
		this.m_counterCurveTimer = 0f;
		this.m_hasGoodLock = false;
	}

	// Token: 0x060085F9 RID: 34297 RVA: 0x003760A8 File Offset: 0x003742A8
	protected override void Move()
	{
		Vector2 currentDirection = this.m_currentDirection;
		if (this.baseData.UsesCustomAccelerationCurve)
		{
			float num = Mathf.Clamp01(this.m_timeElapsed / this.baseData.CustomAccelerationCurveDuration);
			this.m_currentSpeed = this.baseData.AccelerationCurve.Evaluate(num) * this.baseData.speed;
		}
		if (this.m_mode == RobotechProjectile.Mode.InitialTarget || this.m_mode == RobotechProjectile.Mode.TargetLocked)
		{
			Vector2 vector = this.m_targetPoint - base.specRigidbody.UnitCenter;
			float num2 = Mathf.Atan2(this.m_currentDirection.y, this.m_currentDirection.x) * 57.29578f;
			float num3 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			float num4 = BraveMathCollege.ClampAngle180(num3 - num2);
			if (this.m_counterCurveState == RobotechProjectile.CounterCurveState.Ready && Mathf.Abs(num4) < 1f)
			{
				if (vector.magnitude < this.counterCurveMaxDistance)
				{
					this.m_counterCurveState = RobotechProjectile.CounterCurveState.Done;
				}
				else if (UnityEngine.Random.value > this.counterCurveChance)
				{
					this.m_counterCurveState = RobotechProjectile.CounterCurveState.Done;
				}
				else
				{
					this.m_counterCurveState = RobotechProjectile.CounterCurveState.Active;
					this.m_counterCurveDeltaAngle = this.angularAcceleration * (float)((UnityEngine.Random.value >= 0.5f) ? 1 : (-1));
					this.m_counterCurveTimer = 0f;
					this.m_hasGoodLock = false;
				}
			}
			float num5 = num2;
			if (this.m_counterCurveState == RobotechProjectile.CounterCurveState.Mandated)
			{
				this.m_counterCurveTimer += base.LocalDeltaTime;
				num5 = this.m_counterCurveMandatedDirection.ToAngle();
				if (this.m_counterCurveTimer > this.counterCurveDuration)
				{
					this.m_counterCurveState = RobotechProjectile.CounterCurveState.Done;
				}
			}
			else if (this.m_counterCurveState == RobotechProjectile.CounterCurveState.Active)
			{
				this.m_counterCurveTimer += base.LocalDeltaTime;
				num5 += this.m_counterCurveDeltaAngle * base.LocalDeltaTime;
				if (this.m_counterCurveTimer > this.counterCurveDuration)
				{
					this.m_counterCurveState = RobotechProjectile.CounterCurveState.Done;
				}
			}
			else
			{
				float num6 = Mathf.Sign(num4) * Mathf.Min(Mathf.Abs(num4), Mathf.Abs(this.angularAcceleration * base.LocalDeltaTime));
				num5 += num6;
			}
			this.m_currentDirection = Quaternion.Euler(0f, 0f, num5) * Vector3.right;
			if (this.shouldRotate)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, num5);
			}
		}
		else if ((this.m_mode == RobotechProjectile.Mode.InitialDumbfire || this.m_mode == RobotechProjectile.Mode.Dumbfire) && this.shouldRotate)
		{
			float num7 = Mathf.Atan2(this.m_currentDirection.y, this.m_currentDirection.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, num7);
		}
		base.specRigidbody.Velocity = this.m_currentDirection * this.m_currentSpeed;
		base.LastVelocity = base.specRigidbody.Velocity;
		if (this.OverrideMotionModule != null)
		{
			float num8 = Mathf.DeltaAngle(BraveMathCollege.Atan2Degrees(currentDirection), BraveMathCollege.Atan2Degrees(base.LastVelocity));
			this.OverrideMotionModule.AdjustRightVector(num8);
			this.OverrideMotionModule.Move(this, base.transform, base.sprite, base.specRigidbody, ref this.m_timeElapsed, ref this.m_currentDirection, base.Inverted, this.shouldRotate);
			base.LastVelocity = base.specRigidbody.Velocity;
		}
	}

	// Token: 0x060085FA RID: 34298 RVA: 0x00376434 File Offset: 0x00374634
	public override void SetNewShooter(SpeculativeRigidbody newShooter)
	{
		this.m_mode = RobotechProjectile.Mode.Dumbfire;
		base.SetNewShooter(newShooter);
	}

	// Token: 0x060085FB RID: 34299 RVA: 0x00376444 File Offset: 0x00374644
	protected virtual void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (this.collidesWithProjectiles && otherRigidbody.projectile && !(otherRigidbody.projectile.Owner is PlayerController))
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
	}

	// Token: 0x04008A8B RID: 35467
	[Header("Robotech Params")]
	public float angularAcceleration = 220f;

	// Token: 0x04008A8C RID: 35468
	public float searchRadius = 10f;

	// Token: 0x04008A8D RID: 35469
	public float searchTime = 0.5f;

	// Token: 0x04008A8E RID: 35470
	public bool canLoseTarget = true;

	// Token: 0x04008A8F RID: 35471
	public bool reacquiresTargets;

	// Token: 0x04008A90 RID: 35472
	public bool targetAcquisitionRandom;

	// Token: 0x04008A91 RID: 35473
	public float counterCurveChance = 0.66f;

	// Token: 0x04008A92 RID: 35474
	public float counterCurveDuration = 0.2f;

	// Token: 0x04008A93 RID: 35475
	public float counterCurveMaxDistance = 7f;

	// Token: 0x04008A94 RID: 35476
	public float initialDumfireTime;

	// Token: 0x04008A95 RID: 35477
	public bool selectRandomAutoAimTarget;

	// Token: 0x04008A96 RID: 35478
	[NonSerialized]
	public Vector2? initialOverrideTargetPoint;

	// Token: 0x04008A97 RID: 35479
	private RobotechProjectile.Mode m_mode = RobotechProjectile.Mode.InitialTarget;

	// Token: 0x04008A98 RID: 35480
	private RobotechProjectile.CounterCurveState m_counterCurveState;

	// Token: 0x04008A99 RID: 35481
	private float m_counterCurveDeltaAngle;

	// Token: 0x04008A9A RID: 35482
	private float m_counterCurveTimer;

	// Token: 0x04008A9B RID: 35483
	private Vector2 m_counterCurveMandatedDirection;

	// Token: 0x04008A9C RID: 35484
	private IAutoAimTarget m_currentTarget;

	// Token: 0x04008A9D RID: 35485
	private Vector2 m_targetPoint;

	// Token: 0x04008A9E RID: 35486
	private float m_targetSearchTimer;

	// Token: 0x04008A9F RID: 35487
	private float m_initialDumbfireTimer;

	// Token: 0x04008AA0 RID: 35488
	private bool m_hasGoodLock;

	// Token: 0x04008AA1 RID: 35489
	private static List<AIActor> s_activeEnemies = new List<AIActor>();

	// Token: 0x02001671 RID: 5745
	private enum Mode
	{
		// Token: 0x04008AA3 RID: 35491
		InitialDumbfire,
		// Token: 0x04008AA4 RID: 35492
		InitialTarget,
		// Token: 0x04008AA5 RID: 35493
		TargetLocked,
		// Token: 0x04008AA6 RID: 35494
		Dumbfire
	}

	// Token: 0x02001672 RID: 5746
	private enum CounterCurveState
	{
		// Token: 0x04008AA8 RID: 35496
		Ready,
		// Token: 0x04008AA9 RID: 35497
		Active,
		// Token: 0x04008AAA RID: 35498
		Done,
		// Token: 0x04008AAB RID: 35499
		Mandated
	}
}
