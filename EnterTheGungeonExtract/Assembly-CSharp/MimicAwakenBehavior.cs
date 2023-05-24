using System;
using UnityEngine;

// Token: 0x02000D31 RID: 3377
public class MimicAwakenBehavior : BasicAttackBehavior
{
	// Token: 0x06004751 RID: 18257 RVA: 0x001754FC File Offset: 0x001736FC
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x00175530 File Offset: 0x00173730
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiActor.HasBeenEngaged = true;
		this.m_aiActor.CollisionDamage = 0f;
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x00175554 File Offset: 0x00173754
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_hasFired)
		{
			return BehaviorResult.Continue;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.GetComponent<WallMimicController>())
		{
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = -90f;
		}
		this.m_aiActor.ClearPath();
		this.m_aiAnimator.PlayUntilFinished(this.awakenAnim, true, null, -1f, false);
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x001755E4 File Offset: 0x001737E4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_aiAnimator.IsPlaying(this.awakenAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x00175608 File Offset: 0x00173808
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiActor.CollisionDamage = 0.5f;
		this.m_aiActor.knockbackDoer.weight = 35f;
		this.m_hasFired = true;
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x00175680 File Offset: 0x00173880
	private void ShootBulletScript()
	{
		if (!this.m_bulletScriptSource)
		{
			this.m_bulletScriptSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletScriptSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletScriptSource.BulletScript = this.BulletScript;
		this.m_bulletScriptSource.Initialize();
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x001756E0 File Offset: 0x001738E0
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (frame.eventInfo == "fire")
		{
			this.ShootBulletScript();
		}
	}

	// Token: 0x04003A3E RID: 14910
	public string awakenAnim;

	// Token: 0x04003A3F RID: 14911
	public GameObject ShootPoint;

	// Token: 0x04003A40 RID: 14912
	public BulletScriptSelector BulletScript;

	// Token: 0x04003A41 RID: 14913
	private bool m_hasFired;

	// Token: 0x04003A42 RID: 14914
	private BulletScriptSource m_bulletScriptSource;
}
