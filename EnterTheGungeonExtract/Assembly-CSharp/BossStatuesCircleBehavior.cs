using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D93 RID: 3475
[InspectorDropdownName("Bosses/BossStatues/CircleBehavior")]
public class BossStatuesCircleBehavior : BossStatuesPatternBehavior
{
	// Token: 0x06004994 RID: 18836 RVA: 0x00189270 File Offset: 0x00187470
	public override void Start()
	{
		base.Start();
		this.m_cachedStatueAngle = 0.5f * (360f / (float)this.m_statuesController.allStatues.Count);
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x0018929C File Offset: 0x0018749C
	public override void Upkeep()
	{
		base.DecrementTimer(ref this.m_durationTimer, false);
		base.Upkeep();
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x001892B4 File Offset: 0x001874B4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_cachedStatueAngle = BraveMathCollege.ClampAngle360(this.m_statueAngles[0]);
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x001892D0 File Offset: 0x001874D0
	protected override void InitPositions()
	{
		RoomHandler parentRoom = this.m_activeStatues[0].aiActor.ParentRoom;
		this.m_roomLowerLeft = parentRoom.area.basePosition.ToVector2() + new Vector2(1f, 1f);
		this.m_roomUpperRight = (parentRoom.area.basePosition + parentRoom.area.dimensions).ToVector2() + new Vector2(-1f, -5f);
		float num = 6.2831855f * this.CircleRadius;
		this.m_circularSpeed = this.m_statuesController.GetEffectiveMoveSpeed((this.OverrideMoveSpeed <= 0f) ? this.m_statuesController.moveSpeed : this.OverrideMoveSpeed);
		this.m_rotationSpeed = 360f / (num / this.m_circularSpeed);
		this.m_circleCenter = Vector2.zero;
		this.m_statueAngles = new float[this.m_activeStatueCount];
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_statueAngles[i] = this.m_cachedStatueAngle + (float)i * (360f / (float)this.m_activeStatueCount);
			this.m_circleCenter += this.m_activeStatues[i].GroundPosition;
		}
		this.m_circleCenter /= (float)this.m_activeStatueCount;
		this.m_circleCenter = BraveMathCollege.ClampToBounds(this.m_circleCenter, this.m_roomLowerLeft + new Vector2(this.CircleRadius, this.CircleRadius), this.m_roomUpperRight - new Vector2(this.CircleRadius, this.CircleRadius));
		if (this.UseFixedCircleCenter)
		{
			this.m_circleCenter = this.m_statuesController.PatternCenter;
		}
		Vector2[] array = new Vector2[this.m_activeStatueCount];
		for (int j = 0; j < this.m_activeStatueCount; j++)
		{
			array[j] = this.GetTargetPoint(this.m_statueAngles[j]);
		}
		base.ReorderStatues(array);
		for (int k = 0; k < array.Length; k++)
		{
			this.m_activeStatues[k].Target = new Vector2?(this.GetTargetPoint(this.m_statueAngles[k]));
		}
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x00189530 File Offset: 0x00187730
	protected override void UpdatePositions()
	{
		PlayerController playerClosestToPoint = GameManager.Instance.GetPlayerClosestToPoint(this.m_circleCenter);
		if (playerClosestToPoint)
		{
			this.m_circleCenter = Vector2.MoveTowards(this.m_circleCenter, playerClosestToPoint.specRigidbody.UnitCenter, this.CircleCenterVelocity * this.m_deltaTime);
			this.m_circleCenter = BraveMathCollege.ClampToBounds(this.m_circleCenter, this.m_roomLowerLeft + new Vector2(this.CircleRadius, this.CircleRadius), this.m_roomUpperRight - new Vector2(this.CircleRadius, this.CircleRadius));
		}
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_statueAngles[i] += this.m_deltaTime * this.m_rotationSpeed;
			this.m_activeStatues[i].Target = new Vector2?(this.GetTargetPoint(this.m_statueAngles[i]));
		}
		this.m_statuesController.OverrideMoveSpeed = new float?(this.m_circularSpeed + this.CircleCenterVelocity * 2f);
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x00189648 File Offset: 0x00187848
	protected override bool IsFinished()
	{
		return this.m_durationTimer <= 0f;
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x0018965C File Offset: 0x0018785C
	protected override void BeginState(BossStatuesPatternBehavior.PatternState state)
	{
		if (state == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			this.m_durationTimer = this.Duration;
		}
		base.BeginState(state);
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x00189678 File Offset: 0x00187878
	private Vector2 GetTargetPoint(float angle)
	{
		return this.m_circleCenter + BraveMathCollege.DegreesToVector(angle, this.CircleRadius);
	}

	// Token: 0x04003DF9 RID: 15865
	public float Duration;

	// Token: 0x04003DFA RID: 15866
	public float CircleRadius;

	// Token: 0x04003DFB RID: 15867
	public bool UseFixedCircleCenter;

	// Token: 0x04003DFC RID: 15868
	public float CircleCenterVelocity;

	// Token: 0x04003DFD RID: 15869
	private float[] m_statueAngles;

	// Token: 0x04003DFE RID: 15870
	private float m_cachedStatueAngle;

	// Token: 0x04003DFF RID: 15871
	private float m_rotationSpeed;

	// Token: 0x04003E00 RID: 15872
	private float m_circularSpeed;

	// Token: 0x04003E01 RID: 15873
	private Vector2 m_roomLowerLeft;

	// Token: 0x04003E02 RID: 15874
	private Vector2 m_roomUpperRight;

	// Token: 0x04003E03 RID: 15875
	private Vector2 m_circleCenter;

	// Token: 0x04003E04 RID: 15876
	protected float m_durationTimer;
}
