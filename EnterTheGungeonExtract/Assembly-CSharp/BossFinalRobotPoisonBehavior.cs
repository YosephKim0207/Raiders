using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D90 RID: 3472
[InspectorDropdownName("Bosses/BossFinalRobot/PoisonBehavior")]
public class BossFinalRobotPoisonBehavior : BasicAttackBehavior
{
	// Token: 0x06004986 RID: 18822 RVA: 0x00188B80 File Offset: 0x00186D80
	public override void Start()
	{
		base.Start();
		this.m_beamShooter = this.m_aiActor.GetComponent<AIBeamShooter>();
		if (!string.IsNullOrEmpty(this.tellAnimation))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
		}
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x00188BE0 File Offset: 0x00186DE0
	public override BehaviorResult Update()
	{
		base.Update();
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
		this.m_turnedDegrees = 0f;
		this.m_toggleWidthDegrees = 360f / (float)this.divisions;
		this.m_nextToggleDegrees = UnityEngine.Random.Range(0f, this.m_toggleWidthDegrees);
		this.m_beamShooter.LaserAngle = this.initialAimDirection;
		if (!string.IsNullOrEmpty(this.tellAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.tellAnimation, true, null, -1f, false);
			this.m_state = BossFinalRobotPoisonBehavior.State.WaitingForTell;
		}
		else
		{
			this.m_state = BossFinalRobotPoisonBehavior.State.Firing;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x00188CA8 File Offset: 0x00186EA8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == BossFinalRobotPoisonBehavior.State.WaitingForTell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.tellAnimation))
			{
				this.m_state = BossFinalRobotPoisonBehavior.State.Firing;
			}
		}
		else if (this.m_state == BossFinalRobotPoisonBehavior.State.Firing)
		{
			float num = Mathf.Sign(this.turnRate);
			float num2 = Mathf.Abs(this.turnRate * this.m_deltaTime);
			float num3 = this.m_beamShooter.LaserAngle;
			bool flag = this.m_beamShooter.IsFiringLaser;
			while (num2 > 0f)
			{
				if (num2 < this.m_nextToggleDegrees)
				{
					this.m_turnedDegrees += num2;
					num3 += num2 * num;
					this.m_nextToggleDegrees -= num2;
					num2 = 0f;
				}
				else
				{
					this.m_turnedDegrees += this.m_nextToggleDegrees;
					num3 += this.m_nextToggleDegrees * num;
					num2 -= this.m_nextToggleDegrees;
					this.m_nextToggleDegrees = this.m_toggleWidthDegrees;
					if (flag)
					{
						this.m_beamShooter.StopFiringLaser();
						flag = false;
					}
					else
					{
						this.m_beamShooter.StartFiringLaser(this.m_beamShooter.LaserAngle);
						if (this.m_beamShooter.LaserBeam)
						{
							this.m_beamShooter.LaserBeam.projectile.ImmuneToSustainedBlanks = true;
						}
						flag = true;
					}
				}
			}
			this.m_beamShooter.LaserAngle = BraveMathCollege.ClampAngle360(num3);
			if (this.m_turnedDegrees >= this.totalTurnDegrees)
			{
				if (!string.IsNullOrEmpty(this.tellAnimation) && this.m_aiAnimator.IsPlaying(this.tellAnimation))
				{
					if (this.m_beamShooter && this.m_beamShooter.IsFiringLaser)
					{
						this.m_beamShooter.StopFiringLaser();
					}
					this.m_state = BossFinalRobotPoisonBehavior.State.WaitingForAnim;
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == BossFinalRobotPoisonBehavior.State.WaitingForAnim && !this.m_aiAnimator.IsPlaying(this.tellAnimation))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x00188EAC File Offset: 0x001870AC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_beamShooter && this.m_beamShooter.IsFiringLaser)
		{
			this.m_beamShooter.StopFiringLaser();
		}
		if (!string.IsNullOrEmpty(this.tellAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.tellAnimation);
		}
		this.m_state = BossFinalRobotPoisonBehavior.State.None;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600498A RID: 18826 RVA: 0x00188F2C File Offset: 0x0018712C
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		this.m_beamShooter.StopFiringLaser();
	}

	// Token: 0x0600498B RID: 18827 RVA: 0x00188F40 File Offset: 0x00187140
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (this.m_state == BossFinalRobotPoisonBehavior.State.WaitingForTell && frame.eventInfo == "fire")
		{
			this.m_state = BossFinalRobotPoisonBehavior.State.Firing;
		}
	}

	// Token: 0x04003DE2 RID: 15842
	public float initialAimDirection;

	// Token: 0x04003DE3 RID: 15843
	public float turnRate = 360f;

	// Token: 0x04003DE4 RID: 15844
	public float totalTurnDegrees = 360f;

	// Token: 0x04003DE5 RID: 15845
	public int divisions = 6;

	// Token: 0x04003DE6 RID: 15846
	[InspectorCategory("Visuals")]
	public string tellAnimation;

	// Token: 0x04003DE7 RID: 15847
	private AIBeamShooter m_beamShooter;

	// Token: 0x04003DE8 RID: 15848
	private float m_turnedDegrees;

	// Token: 0x04003DE9 RID: 15849
	private float m_nextToggleDegrees;

	// Token: 0x04003DEA RID: 15850
	private float m_toggleWidthDegrees;

	// Token: 0x04003DEB RID: 15851
	private BossFinalRobotPoisonBehavior.State m_state;

	// Token: 0x02000D91 RID: 3473
	private enum State
	{
		// Token: 0x04003DED RID: 15853
		None,
		// Token: 0x04003DEE RID: 15854
		WaitingForTell,
		// Token: 0x04003DEF RID: 15855
		Firing,
		// Token: 0x04003DF0 RID: 15856
		WaitingForAnim
	}
}
