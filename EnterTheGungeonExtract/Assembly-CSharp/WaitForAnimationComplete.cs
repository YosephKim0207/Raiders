using System;

// Token: 0x02000E08 RID: 3592
public class WaitForAnimationComplete : OverrideBehaviorBase
{
	// Token: 0x06004C12 RID: 19474 RVA: 0x0019F0C4 File Offset: 0x0019D2C4
	public override void Start()
	{
		base.Start();
		this.remainingDelay = this.ExtraDelay;
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x0019F0D8 File Offset: 0x0019D2D8
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x0019F0E0 File Offset: 0x0019D2E0
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		for (int i = 0; i < this.TargetAnimations.Length; i++)
		{
			if (this.m_aiAnimator != null)
			{
				if (this.m_aiAnimator.IsPlaying(this.TargetAnimations[i]))
				{
					this.remainingDelay = this.ExtraDelay;
					return BehaviorResult.SkipAllRemainingBehaviors;
				}
			}
			else if (this.m_aiActor.spriteAnimator.IsPlaying(this.TargetAnimations[i]))
			{
				this.remainingDelay = this.ExtraDelay;
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
		}
		if (this.remainingDelay > 0f)
		{
			this.remainingDelay -= this.m_deltaTime;
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		return behaviorResult;
	}

	// Token: 0x040041EA RID: 16874
	public string[] TargetAnimations;

	// Token: 0x040041EB RID: 16875
	public float ExtraDelay;

	// Token: 0x040041EC RID: 16876
	protected float remainingDelay;
}
