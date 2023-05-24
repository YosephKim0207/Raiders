using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02000E03 RID: 3587
public class DazedBehavior : OverrideBehaviorBase
{
	// Token: 0x06004BF3 RID: 19443 RVA: 0x0019E424 File Offset: 0x0019C624
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004BF4 RID: 19444 RVA: 0x0019E42C File Offset: 0x0019C62C
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004BF5 RID: 19445 RVA: 0x0019E434 File Offset: 0x0019C634
	public override bool OverrideOtherBehaviors()
	{
		return true;
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x0019E438 File Offset: 0x0019C638
	public override BehaviorResult Update()
	{
		this.m_repathTimer -= this.m_aiActor.LocalDeltaTime;
		this.m_pauseTimer -= this.m_aiActor.LocalDeltaTime;
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		IntVector2? targetPos = this.m_targetPos;
		if (targetPos == null && this.m_repathTimer > 0f)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (this.m_pauseTimer > 0f)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		IntVector2? targetPos2 = this.m_targetPos;
		if (targetPos2 != null && this.m_aiActor.PathComplete)
		{
			this.m_targetPos = null;
			if (this.PointReachedPauseTime > 0f)
			{
				this.m_pauseTimer = this.PointReachedPauseTime;
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
		}
		if (this.m_repathTimer <= 0f)
		{
			this.m_repathTimer = this.PathInterval;
			IntVector2? targetPos3 = this.m_targetPos;
			if (targetPos3 != null && !this.SimpleCellValidator(this.m_targetPos.Value))
			{
				this.m_targetPos = null;
			}
			IntVector2? targetPos4 = this.m_targetPos;
			if (targetPos4 == null)
			{
				this.m_targetPos = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, new CellValidator(this.SimpleCellValidator));
			}
			IntVector2? targetPos5 = this.m_targetPos;
			if (targetPos5 == null)
			{
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
			this.m_aiActor.PathfindToPosition(this.m_targetPos.Value.ToCenterVector2(), null, true, null, null, null, false);
		}
		return BehaviorResult.SkipAllRemainingBehaviors;
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x0019E610 File Offset: 0x0019C810
	private bool SimpleCellValidator(IntVector2 c)
	{
		if (Vector2.Distance(c.ToVector2(), this.m_aiActor.CenterPosition) > 4f)
		{
			return false;
		}
		for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
		{
			for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
			{
				if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040041CD RID: 16845
	public float PointReachedPauseTime = 0.5f;

	// Token: 0x040041CE RID: 16846
	public float PathInterval = 0.5f;

	// Token: 0x040041CF RID: 16847
	private float m_repathTimer;

	// Token: 0x040041D0 RID: 16848
	private float m_pauseTimer;

	// Token: 0x040041D1 RID: 16849
	private IntVector2? m_targetPos;
}
