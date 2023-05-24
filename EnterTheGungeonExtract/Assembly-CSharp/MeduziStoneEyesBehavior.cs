using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC7 RID: 3527
[InspectorDropdownName("Bosses/Meduzi/StoneEyesBehavior")]
public class MeduziStoneEyesBehavior : BasicAttackBehavior
{
	// Token: 0x06004ACB RID: 19147 RVA: 0x00192AE4 File Offset: 0x00190CE4
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x00192B18 File Offset: 0x00190D18
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x00192B20 File Offset: 0x00190D20
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
		this.m_state = MeduziStoneEyesBehavior.State.WaitingToFire;
		this.m_aiActor.ClearPath();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x00192B80 File Offset: 0x00190D80
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == MeduziStoneEyesBehavior.State.Firing)
		{
			this.m_timer -= BraveTime.DeltaTime;
			float num = BraveMathCollege.LinearToSmoothStepInterpolate(0f, this.distortionMaxRadius, 1f - this.m_timer / this.distortionDuration);
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (!playerController.healthHaver.IsDead)
				{
					if (!playerController.spriteAnimator.QueryInvulnerabilityFrame() && playerController.healthHaver.IsVulnerable)
					{
						Vector2 unitCenter = playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox);
						float num2 = Vector2.Distance(unitCenter, this.m_distortionCenter);
						if (num2 >= this.m_prevWaveDist - 0.25f && num2 <= num + 0.25f)
						{
							float num3 = (unitCenter - this.m_distortionCenter).ToAngle();
							if (BraveMathCollege.AbsAngleBetween(playerController.FacingDirection, num3) >= 45f)
							{
								playerController.CurrentStoneGunTimer = this.stoneDuration;
							}
						}
					}
				}
			}
			this.m_prevWaveDist = num;
		}
		if (this.m_aiAnimator.IsPlaying(this.anim) || this.m_timer > 0f)
		{
			return ContinuousBehaviorResult.Continue;
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x06004ACF RID: 19151 RVA: 0x00192CE8 File Offset: 0x00190EE8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = MeduziStoneEyesBehavior.State.Idle;
		this.m_aiAnimator.EndAnimationIf(this.anim);
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x00192D18 File Offset: 0x00190F18
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x00192D1C File Offset: 0x00190F1C
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_state == MeduziStoneEyesBehavior.State.WaitingToFire && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.m_distortionCenter = this.shootPoint.transform.position.XY();
			Exploder.DoDistortionWave(this.m_distortionCenter, this.distortionIntensity, this.distortionThickness, this.distortionMaxRadius, this.distortionDuration);
			this.m_timer = this.distortionDuration - BraveTime.DeltaTime;
			this.m_state = MeduziStoneEyesBehavior.State.Firing;
			this.m_prevWaveDist = 0f;
		}
	}

	// Token: 0x04003FC1 RID: 16321
	public GameObject shootPoint;

	// Token: 0x04003FC2 RID: 16322
	public float distortionMaxRadius = 20f;

	// Token: 0x04003FC3 RID: 16323
	public float distortionDuration = 1.5f;

	// Token: 0x04003FC4 RID: 16324
	public float stoneDuration = 3f;

	// Token: 0x04003FC5 RID: 16325
	[InspectorCategory("Visuals")]
	public string anim;

	// Token: 0x04003FC6 RID: 16326
	[InspectorCategory("Visuals")]
	public float distortionIntensity = 0.5f;

	// Token: 0x04003FC7 RID: 16327
	[InspectorCategory("Visuals")]
	public float distortionThickness = 0.04f;

	// Token: 0x04003FC8 RID: 16328
	private MeduziStoneEyesBehavior.State m_state;

	// Token: 0x04003FC9 RID: 16329
	private Vector2 m_distortionCenter;

	// Token: 0x04003FCA RID: 16330
	private float m_timer;

	// Token: 0x04003FCB RID: 16331
	private float m_prevWaveDist;

	// Token: 0x02000DC8 RID: 3528
	private enum State
	{
		// Token: 0x04003FCD RID: 16333
		Idle,
		// Token: 0x04003FCE RID: 16334
		WaitingToFire,
		// Token: 0x04003FCF RID: 16335
		Firing
	}
}
