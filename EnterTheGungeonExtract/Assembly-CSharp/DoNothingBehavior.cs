using System;

// Token: 0x02000D27 RID: 3367
public class DoNothingBehavior : BasicAttackBehavior
{
	// Token: 0x06004714 RID: 18196 RVA: 0x00173F4C File Offset: 0x0017214C
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x00173F54 File Offset: 0x00172154
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_doNothingTimer, false);
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x00173F6C File Offset: 0x0017216C
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_hasDoneNothing)
		{
			return BehaviorResult.Continue;
		}
		this.m_doNothingTimer = this.DoNothingTimer;
		if (this.m_aiActor)
		{
			this.m_aiActor.ClearPath();
		}
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x00173FAC File Offset: 0x001721AC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_doNothingTimer > 0f)
		{
			return ContinuousBehaviorResult.Continue;
		}
		this.m_hasDoneNothing = true;
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x040039FC RID: 14844
	public float DoNothingTimer = 2f;

	// Token: 0x040039FD RID: 14845
	private float m_doNothingTimer;

	// Token: 0x040039FE RID: 14846
	private bool m_hasDoneNothing;
}
