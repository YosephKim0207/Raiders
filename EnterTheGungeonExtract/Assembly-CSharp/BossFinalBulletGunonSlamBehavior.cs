using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D89 RID: 3465
[InspectorDropdownName("Bosses/BossFinalBullet/GunonSlamBehavior")]
public class BossFinalBulletGunonSlamBehavior : BasicAttackBehavior
{
	// Token: 0x06004957 RID: 18775 RVA: 0x00187CE4 File Offset: 0x00185EE4
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		this.m_traps = new List<PitTrapController>(UnityEngine.Object.FindObjectsOfType<PitTrapController>());
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x00187D34 File Offset: 0x00185F34
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
		this.m_aiAnimator.PlayUntilFinished(this.anim, true, null, -1f, false);
		this.m_slammed = false;
		this.m_aiActor.ClearPath();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x00187D94 File Offset: 0x00185F94
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_aiAnimator.IsPlaying(this.anim))
		{
			if (!this.m_slammed)
			{
				this.Slam();
			}
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x00187DC8 File Offset: 0x00185FC8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.EndAnimationIf(this.anim);
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x00187DF0 File Offset: 0x00185FF0
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x00187DF4 File Offset: 0x00185FF4
	public override bool IsReady()
	{
		return base.IsReady() && this.m_traps.Count > this.MinTrapsLeft();
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x00187E18 File Offset: 0x00186018
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (!this.m_slammed && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.Slam();
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x00187E48 File Offset: 0x00186048
	private void Slam()
	{
		int num = Mathf.Min(this.m_traps.Count - this.MinTrapsLeft(), 2);
		for (int i = 0; i < num; i++)
		{
			int num2 = UnityEngine.Random.Range(0, this.m_traps.Count);
			this.m_traps[num2].Trigger();
			this.m_traps.RemoveAt(num2);
		}
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x00187EB0 File Offset: 0x001860B0
	private int MinTrapsLeft()
	{
		return Mathf.RoundToInt(Mathf.Lerp((float)this.numTraps, 0f, Mathf.InverseLerp(1f, 0.33f, this.m_aiActor.healthHaver.GetCurrentHealthPercentage())));
	}

	// Token: 0x04003DB1 RID: 15793
	public int numTraps = 6;

	// Token: 0x04003DB2 RID: 15794
	[InspectorCategory("Visuals")]
	public string anim;

	// Token: 0x04003DB3 RID: 15795
	private List<PitTrapController> m_traps;

	// Token: 0x04003DB4 RID: 15796
	private bool m_slammed;
}
