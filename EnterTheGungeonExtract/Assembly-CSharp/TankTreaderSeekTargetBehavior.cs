using System;
using System.Collections.Generic;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DE0 RID: 3552
[InspectorDropdownName("Bosses/TankTreader/SeekTargetBehavior")]
public class TankTreaderSeekTargetBehavior : RangedMovementBehavior
{
	// Token: 0x06004B3D RID: 19261 RVA: 0x001970C4 File Offset: 0x001952C4
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
	}

	// Token: 0x06004B3E RID: 19262 RVA: 0x001970D4 File Offset: 0x001952D4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_aiAnimator.LockFacingDirection = true;
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x00197114 File Offset: 0x00195314
	public override BehaviorResult Update()
	{
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		this.m_aiAnimator.FacingDirection = Mathf.MoveTowardsAngle(this.m_aiAnimator.FacingDirection, this.m_desiredFacingDirection, this.turnSpeed * this.m_deltaTime);
		if (!base.InRange() || !targetRigidbody)
		{
			if (this.m_state == TankTreaderSeekTargetBehavior.State.PathingToTarget)
			{
				this.m_aiActor.ClearPath();
				this.m_state = TankTreaderSeekTargetBehavior.State.Idle;
			}
			return BehaviorResult.Continue;
		}
		bool hasLineOfSightToTarget = this.m_aiActor.HasLineOfSightToTarget;
		float num = ((this.CustomRange < 0f) ? this.m_aiActor.DesiredCombatDistance : this.CustomRange);
		this.m_state = TankTreaderSeekTargetBehavior.State.PathingToTarget;
		if (this.StopWhenInRange && this.m_aiActor.DistanceToTarget <= num && (!this.LineOfSight || hasLineOfSightToTarget))
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			return BehaviorResult.Continue;
		}
		if (this.m_repathTimer <= 0f)
		{
			this.m_startStep = ((this.m_aiActor.specRigidbody.Velocity.magnitude <= 0.01f) ? IntVector2.Zero : BraveUtility.GetIntMajorAxis(this.m_aiActor.specRigidbody.Velocity));
			this.m_aiActor.PathfindToPosition(targetRigidbody.UnitCenter, null, false, null, new ExtraWeightingFunction(this.WeightDoer), null, false);
			this.m_repathTimer = this.PathInterval;
			this.SimplifyPath();
		}
		this.UpdateVelocity();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x001972B8 File Offset: 0x001954B8
	private void SimplifyPath()
	{
		Path path = this.m_aiActor.Path;
		if (path == null || path.Count < 2)
		{
			return;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 firstCenterVector = this.m_aiActor.Path.GetFirstCenterVector2();
		Vector2 secondCenterVector = this.m_aiActor.Path.GetSecondCenterVector2();
		float num = (firstCenterVector - unitCenter).ToAngle();
		float num2 = (secondCenterVector - unitCenter).ToAngle();
		float num3 = BraveMathCollege.ClampAngle360(num - num2);
		if (num3 > 179f && num3 < 181f)
		{
			path.Positions.RemoveFirst();
		}
		if (path.Count < 2)
		{
			return;
		}
		LinkedListNode<IntVector2> linkedListNode = path.Positions.First.Next;
		IntVector2 intVector = linkedListNode.Value - linkedListNode.Previous.Value;
		while (linkedListNode != null && linkedListNode.Next != null)
		{
			IntVector2 intVector2 = linkedListNode.Next.Value - linkedListNode.Value;
			if (intVector == intVector2)
			{
				linkedListNode = linkedListNode.Next;
				path.Positions.Remove(linkedListNode.Previous);
			}
			else
			{
				intVector = intVector2;
				linkedListNode = linkedListNode.Next;
			}
		}
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x00197408 File Offset: 0x00195608
	private int WeightDoer(IntVector2 prevStep, IntVector2 nextStep)
	{
		if (prevStep == IntVector2.Zero)
		{
			if (this.m_startStep == IntVector2.Zero)
			{
				return 0;
			}
			prevStep = this.m_startStep;
		}
		return (!(prevStep != nextStep)) ? 0 : 10;
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x00197458 File Offset: 0x00195658
	private void UpdateVelocity()
	{
		bool flag;
		Vector2 vector2;
		Vector2 vector = this.GetPathVelocityContribution(out flag, out vector2);
		Vector2 vector3 = vector;
		if (Mathf.Abs(vector2.x) < PhysicsEngine.PixelToUnit(2))
		{
			vector3.x = 0f;
		}
		if (Mathf.Abs(vector2.y) < PhysicsEngine.PixelToUnit(2))
		{
			vector3.y = 0f;
		}
		if (vector3.magnitude > 0.01f)
		{
			float num = vector.ToAngle();
			float num2 = BraveMathCollege.ClampAngle180(this.m_aiAnimator.FacingDirection - num);
			if (Mathf.Abs(num2) > 0.5f && Mathf.Abs(num2) < 179.5f)
			{
				vector = Vector2.zero;
				if (BraveMathCollege.AbsAngleBetween(this.m_aiAnimator.FacingDirection, num) <= 100f)
				{
					this.m_desiredFacingDirection = num;
				}
				else
				{
					this.m_desiredFacingDirection = BraveMathCollege.ClampAngle360(num + 180f);
				}
			}
		}
		this.m_aiActor.BehaviorVelocity = vector;
		if (flag)
		{
			this.m_aiActor.Path.RemoveFirst();
		}
	}

	// Token: 0x06004B43 RID: 19267 RVA: 0x0019756C File Offset: 0x0019576C
	private Vector2 GetPathVelocityContribution(out bool willReachGoal, out Vector2 totalDistToMove)
	{
		willReachGoal = false;
		totalDistToMove = Vector2.zero;
		if (this.m_aiActor.Path == null || this.m_aiActor.Path.Count == 0)
		{
			return Vector2.zero;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 firstCenterVector = this.m_aiActor.Path.GetFirstCenterVector2();
		totalDistToMove = firstCenterVector - unitCenter;
		float num = this.m_aiActor.MovementSpeed * this.m_aiActor.LocalDeltaTime;
		if (num > totalDistToMove.magnitude)
		{
			willReachGoal = true;
			return totalDistToMove / this.m_aiActor.LocalDeltaTime;
		}
		return this.m_aiActor.MovementSpeed * totalDistToMove.normalized;
	}

	// Token: 0x040040A0 RID: 16544
	public bool StopWhenInRange = true;

	// Token: 0x040040A1 RID: 16545
	public float CustomRange = -1f;

	// Token: 0x040040A2 RID: 16546
	[InspectorShowIf("StopWhenInRange")]
	public bool LineOfSight = true;

	// Token: 0x040040A3 RID: 16547
	public float PathInterval = 0.25f;

	// Token: 0x040040A4 RID: 16548
	public float turnSpeed = 120f;

	// Token: 0x040040A5 RID: 16549
	private float m_repathTimer;

	// Token: 0x040040A6 RID: 16550
	private IntVector2 m_startStep;

	// Token: 0x040040A7 RID: 16551
	private float m_desiredFacingDirection = -90f;

	// Token: 0x040040A8 RID: 16552
	private TankTreaderSeekTargetBehavior.State m_state;

	// Token: 0x02000DE1 RID: 3553
	private enum State
	{
		// Token: 0x040040AA RID: 16554
		Idle,
		// Token: 0x040040AB RID: 16555
		PathingToTarget
	}
}
