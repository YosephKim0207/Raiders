using System;
using UnityEngine;

// Token: 0x02000D5B RID: 3419
public class SpewGoopBehavior : BasicAttackBehavior
{
	// Token: 0x06004839 RID: 18489 RVA: 0x0017E0D0 File Offset: 0x0017C2D0
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x0017E104 File Offset: 0x0017C304
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_goopTimer, false);
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x0017E11C File Offset: 0x0017C31C
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_hasGooped = false;
		this.m_aiAnimator.PlayUntilFinished(this.spewAnimation, false, null, -1f, false);
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x0017E18C File Offset: 0x0017C38C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_hasGooped || this.m_goopTimer > 0f)
		{
			return ContinuousBehaviorResult.Continue;
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x0017E1B4 File Offset: 0x0017C3B4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.EndAnimationIf(this.spewAnimation);
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x0017E1DC File Offset: 0x0017C3DC
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (!this.m_hasGooped && clip.GetFrame(frame).eventInfo == "spew")
		{
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToUse);
			goopManagerForGoopType.TimedAddGoopArc(this.goopPoint.transform.position, this.goopConeLength, this.goopConeArc, this.goopPoint.transform.right, this.goopDuration, this.goopCurve);
			this.m_goopTimer = this.goopDuration;
			this.m_hasGooped = true;
		}
	}

	// Token: 0x04003BCD RID: 15309
	public string spewAnimation;

	// Token: 0x04003BCE RID: 15310
	public Transform goopPoint;

	// Token: 0x04003BCF RID: 15311
	public GoopDefinition goopToUse;

	// Token: 0x04003BD0 RID: 15312
	public float goopConeLength = 5f;

	// Token: 0x04003BD1 RID: 15313
	public float goopConeArc = 45f;

	// Token: 0x04003BD2 RID: 15314
	public AnimationCurve goopCurve;

	// Token: 0x04003BD3 RID: 15315
	public float goopDuration = 0.5f;

	// Token: 0x04003BD4 RID: 15316
	private float m_goopTimer;

	// Token: 0x04003BD5 RID: 15317
	private bool m_hasGooped;
}
