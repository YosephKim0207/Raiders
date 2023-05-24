using System;
using Dungeonator;
using FullInspector;

// Token: 0x02000D8F RID: 3471
[InspectorDropdownName("Bosses/BossFinalMarine/WaitBehavior")]
public class BossFinalMarineWaitBehavior : AttackBehaviorBase
{
	// Token: 0x0600497E RID: 18814 RVA: 0x00188AB0 File Offset: 0x00186CB0
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_waitTimer, false);
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x00188AC8 File Offset: 0x00186CC8
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		this.m_waitTimer = this.time;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004980 RID: 18816 RVA: 0x00188B00 File Offset: 0x00186D00
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_waitTimer <= 0f)
		{
			return ContinuousBehaviorResult.Finished;
		}
		if (this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count <= 1)
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x00188B34 File Offset: 0x00186D34
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x00188B44 File Offset: 0x00186D44
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06004983 RID: 18819 RVA: 0x00188B48 File Offset: 0x00186D48
	public override float GetMinReadyRange()
	{
		return 0f;
	}

	// Token: 0x06004984 RID: 18820 RVA: 0x00188B50 File Offset: 0x00186D50
	public override float GetMaxRange()
	{
		return float.MaxValue;
	}

	// Token: 0x04003DE0 RID: 15840
	public float time;

	// Token: 0x04003DE1 RID: 15841
	private float m_waitTimer;
}
