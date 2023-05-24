using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D95 RID: 3477
[InspectorDropdownName("Bosses/BossStatues/LineBehavior")]
public class BossStatuesLineBehavior : BossStatuesPatternBehavior
{
	// Token: 0x060049A9 RID: 18857 RVA: 0x00189D9C File Offset: 0x00187F9C
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x00189DA4 File Offset: 0x00187FA4
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_state == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			base.DecrementTimer(ref this.m_durationTimer, false);
		}
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x00189DC8 File Offset: 0x00187FC8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x00189DD0 File Offset: 0x00187FD0
	protected override void InitPositions()
	{
		this.m_statuePositions = new Vector2[this.m_activeStatueCount];
		RoomHandler parentRoom = this.m_activeStatues[0].aiActor.ParentRoom;
		Vector2 vector = parentRoom.area.basePosition.ToVector2() + new Vector2(1f, 1f);
		Vector2 vector2 = (parentRoom.area.basePosition + parentRoom.area.dimensions).ToVector2() + new Vector2(-1f, -2f);
		float num = 0f;
		switch (this.direction)
		{
		case BossStatuesLineBehavior.Direction.LeftToRight:
			this.m_minPos = new Vector2(vector.x, vector2.y);
			this.m_maxPos = new Vector2(vector.x, vector.y);
			this.m_velocity = Vector2.right;
			num = vector2.x - vector.x;
			break;
		case BossStatuesLineBehavior.Direction.RightToLeft:
			this.m_minPos = new Vector2(vector2.x, vector2.y);
			this.m_maxPos = new Vector2(vector2.x, vector.y);
			this.m_velocity = -Vector2.right;
			num = vector2.x - vector.x;
			break;
		case BossStatuesLineBehavior.Direction.TopToBottom:
			this.m_minPos = new Vector2(vector.x, vector2.y);
			this.m_maxPos = new Vector2(vector2.x, vector2.y);
			this.m_velocity = -Vector2.up;
			num = vector2.y - vector.y;
			break;
		case BossStatuesLineBehavior.Direction.BottomToTop:
			this.m_minPos = new Vector2(vector.x, vector.y);
			this.m_maxPos = new Vector2(vector2.x, vector.y);
			this.m_velocity = Vector2.up;
			num = vector2.y - vector.y;
			break;
		}
		this.m_deltaPos = (this.m_maxPos - this.m_minPos) / (float)this.m_activeStatueCount;
		float effectiveMoveSpeed = this.m_statuesController.GetEffectiveMoveSpeed((this.OverrideMoveSpeed <= 0f) ? this.m_statuesController.moveSpeed : this.OverrideMoveSpeed);
		this.m_velocity *= effectiveMoveSpeed;
		if (this.Duration > 0f)
		{
			this.m_durationTimer = this.Duration;
		}
		else
		{
			this.m_durationTimer = num / effectiveMoveSpeed;
		}
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_statuePositions[i] = this.m_minPos + ((float)i + 0.5f) * this.m_deltaPos;
		}
		base.ReorderStatues(this.m_statuePositions);
		for (int j = 0; j < this.m_activeStatueCount; j++)
		{
			this.m_activeStatues[j].Target = new Vector2?(this.m_statuePositions[j]);
		}
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x0018A114 File Offset: 0x00188314
	protected override void UpdatePositions()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_statuePositions[i] += this.m_velocity * this.m_deltaTime;
			this.m_activeStatues[i].Target = new Vector2?(this.m_statuePositions[i]);
		}
	}

	// Token: 0x060049AE RID: 18862 RVA: 0x0018A18C File Offset: 0x0018838C
	protected override bool IsFinished()
	{
		return this.m_durationTimer <= 0f;
	}

	// Token: 0x04003E12 RID: 15890
	public float Duration;

	// Token: 0x04003E13 RID: 15891
	public BossStatuesLineBehavior.Direction direction;

	// Token: 0x04003E14 RID: 15892
	private Vector2[] m_statuePositions;

	// Token: 0x04003E15 RID: 15893
	protected float m_durationTimer;

	// Token: 0x04003E16 RID: 15894
	private Vector2 m_minPos;

	// Token: 0x04003E17 RID: 15895
	private Vector2 m_maxPos;

	// Token: 0x04003E18 RID: 15896
	private Vector2 m_deltaPos;

	// Token: 0x04003E19 RID: 15897
	private Vector2 m_velocity;

	// Token: 0x02000D96 RID: 3478
	public enum Direction
	{
		// Token: 0x04003E1B RID: 15899
		LeftToRight,
		// Token: 0x04003E1C RID: 15900
		RightToLeft,
		// Token: 0x04003E1D RID: 15901
		TopToBottom,
		// Token: 0x04003E1E RID: 15902
		BottomToTop
	}
}
