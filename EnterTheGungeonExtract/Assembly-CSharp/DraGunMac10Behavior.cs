using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DAB RID: 3499
[InspectorDropdownName("Bosses/DraGun/Mac10Behavior")]
public class DraGunMac10Behavior : BasicAttackBehavior
{
	// Token: 0x06004A13 RID: 18963 RVA: 0x0018CAD0 File Offset: 0x0018ACD0
	private bool UseUnityAnimation()
	{
		return this.unityAnimation != null;
	}

	// Token: 0x06004A14 RID: 18964 RVA: 0x0018CAE0 File Offset: 0x0018ACE0
	private bool UseAiAnimator()
	{
		return this.aiAnimator != null;
	}

	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06004A15 RID: 18965 RVA: 0x0018CAF0 File Offset: 0x0018ACF0
	// (set) Token: 0x06004A16 RID: 18966 RVA: 0x0018CAF8 File Offset: 0x0018ACF8
	private DraGunMac10Behavior.HandShootState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x0018CB28 File Offset: 0x0018AD28
	public override void Start()
	{
		base.Start();
		if (this.fireType == DraGunMac10Behavior.FireType.tk2dAnimEvent)
		{
			tk2dSpriteAnimator spriteAnimator = this.aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.tk2dAnimationEventTriggered));
		}
		if (this.fireType == DraGunMac10Behavior.FireType.UnityAnimEvent)
		{
			this.m_aiActor.behaviorSpeculator.AnimationEventTriggered += this.UnityAnimationEventTriggered;
		}
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x0018CB9C File Offset: 0x0018AD9C
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
		this.State = DraGunMac10Behavior.HandShootState.Intro;
		if (this.aiAnimator && this.useAnimationDirection)
		{
			this.aiAnimator.UseAnimatedFacingDirection = true;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x0018CBFC File Offset: 0x0018ADFC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == DraGunMac10Behavior.HandShootState.Intro)
		{
			bool flag = true;
			if (this.UseUnityAnimation())
			{
				flag &= !this.unityAnimation.IsPlaying(this.unityIntroAnim);
			}
			if (this.UseAiAnimator())
			{
				flag &= !this.aiAnimator.IsPlaying(this.aiIntroAnim);
			}
			if (flag)
			{
				this.State = DraGunMac10Behavior.HandShootState.Shooting;
			}
		}
		else if (this.State == DraGunMac10Behavior.HandShootState.Shooting)
		{
			if (this.fireType == DraGunMac10Behavior.FireType.Immediate && !this.m_isShooting)
			{
				this.ShootBulletScript();
			}
			bool flag2 = true;
			if (this.unityAnimation)
			{
				flag2 &= !this.unityAnimation.IsPlaying(this.unityShootAnim);
			}
			if (this.aiAnimator)
			{
				flag2 &= !this.aiAnimator.IsPlaying(this.aiShootAnim) || this.aiAnimator.spriteAnimator.CurrentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop;
			}
			if (flag2)
			{
				this.State = DraGunMac10Behavior.HandShootState.Outro;
			}
		}
		else if (this.State == DraGunMac10Behavior.HandShootState.Outro)
		{
			bool flag3 = true;
			if (this.unityAnimation)
			{
				flag3 &= !this.unityAnimation.IsPlaying(this.unityOutroAnim);
			}
			if (this.aiAnimator)
			{
				flag3 &= !this.aiAnimator.IsPlaying(this.aiOutroAnim) || this.aiAnimator.spriteAnimator.CurrentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop;
			}
			if (flag3)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A1A RID: 18970 RVA: 0x0018CDA0 File Offset: 0x0018AFA0
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.State = DraGunMac10Behavior.HandShootState.None;
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.aiAnimator)
		{
			this.aiAnimator.EndAnimation();
			if (this.useAnimationDirection)
			{
				this.aiAnimator.UseAnimatedFacingDirection = false;
			}
		}
		if (this.unityAnimation)
		{
			this.unityAnimation.Stop();
			this.unityAnimation.GetClip(this.unityOutroAnim).SampleAnimation(this.unityAnimation.gameObject, 1000f);
			this.unityAnimation.GetComponent<DraGunArmController>().ClipArmSprites();
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x0018CE68 File Offset: 0x0018B068
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A1C RID: 18972 RVA: 0x0018CE6C File Offset: 0x0018B06C
	private void tk2dAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		this.UnityAnimationEventTriggered(clip.GetFrame(frame).eventInfo);
	}

	// Token: 0x06004A1D RID: 18973 RVA: 0x0018CE80 File Offset: 0x0018B080
	private void UnityAnimationEventTriggered(string eventInfo)
	{
		if (eventInfo == "fire")
		{
			this.ShootBulletScript();
		}
		else if (eventInfo == "cease_fire" && this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x0018CED4 File Offset: 0x0018B0D4
	private void ShootBulletScript()
	{
		if (string.IsNullOrEmpty(this.BulletScript.scriptTypeName))
		{
			return;
		}
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
		this.m_isShooting = true;
	}

	// Token: 0x06004A1F RID: 18975 RVA: 0x0018CF54 File Offset: 0x0018B154
	private void BeginState(DraGunMac10Behavior.HandShootState state)
	{
		if (state == DraGunMac10Behavior.HandShootState.Intro)
		{
			if (this.unityAnimation)
			{
				this.unityAnimation.Play(this.unityIntroAnim);
			}
			if (this.aiAnimator)
			{
				this.aiAnimator.PlayUntilCancelled(this.aiIntroAnim, false, null, -1f, false);
			}
		}
		else if (state == DraGunMac10Behavior.HandShootState.Shooting)
		{
			if (this.unityAnimation)
			{
				this.unityAnimation.Play(this.unityShootAnim);
			}
			if (this.aiAnimator)
			{
				this.aiAnimator.PlayUntilCancelled(this.aiShootAnim, false, null, -1f, false);
			}
			this.m_isShooting = false;
		}
		else if (state == DraGunMac10Behavior.HandShootState.Outro)
		{
			if (this.unityAnimation)
			{
				this.unityAnimation.Play(this.unityOutroAnim);
			}
			if (this.aiAnimator)
			{
				this.aiAnimator.PlayUntilCancelled(this.aiOutroAnim, false, null, -1f, false);
			}
		}
	}

	// Token: 0x06004A20 RID: 18976 RVA: 0x0018D06C File Offset: 0x0018B26C
	private void EndState(DraGunMac10Behavior.HandShootState state)
	{
		if (state == DraGunMac10Behavior.HandShootState.Shooting && this.m_isShooting && this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x04003EB1 RID: 16049
	public GameObject ShootPoint;

	// Token: 0x04003EB2 RID: 16050
	public BulletScriptSelector BulletScript;

	// Token: 0x04003EB3 RID: 16051
	public DraGunMac10Behavior.FireType fireType;

	// Token: 0x04003EB4 RID: 16052
	public Animation unityAnimation;

	// Token: 0x04003EB5 RID: 16053
	[InspectorShowIf("UseUnityAnimation")]
	[InspectorIndent]
	public string unityIntroAnim;

	// Token: 0x04003EB6 RID: 16054
	[InspectorIndent]
	[InspectorShowIf("UseUnityAnimation")]
	public string unityShootAnim;

	// Token: 0x04003EB7 RID: 16055
	[InspectorIndent]
	[InspectorShowIf("UseUnityAnimation")]
	public string unityOutroAnim;

	// Token: 0x04003EB8 RID: 16056
	public AIAnimator aiAnimator;

	// Token: 0x04003EB9 RID: 16057
	[InspectorIndent]
	[InspectorShowIf("UseAiAnimator")]
	public bool useAnimationDirection;

	// Token: 0x04003EBA RID: 16058
	[InspectorIndent]
	[InspectorShowIf("UseAiAnimator")]
	public string aiIntroAnim;

	// Token: 0x04003EBB RID: 16059
	[InspectorIndent]
	[InspectorShowIf("UseAiAnimator")]
	public string aiShootAnim;

	// Token: 0x04003EBC RID: 16060
	[InspectorShowIf("UseAiAnimator")]
	[InspectorIndent]
	public string aiOutroAnim;

	// Token: 0x04003EBD RID: 16061
	private DraGunMac10Behavior.HandShootState m_state;

	// Token: 0x04003EBE RID: 16062
	private bool m_isShooting;

	// Token: 0x04003EBF RID: 16063
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000DAC RID: 3500
	private enum HandShootState
	{
		// Token: 0x04003EC1 RID: 16065
		None,
		// Token: 0x04003EC2 RID: 16066
		Intro,
		// Token: 0x04003EC3 RID: 16067
		Shooting,
		// Token: 0x04003EC4 RID: 16068
		Outro
	}

	// Token: 0x02000DAD RID: 3501
	public enum FireType
	{
		// Token: 0x04003EC6 RID: 16070
		Immediate,
		// Token: 0x04003EC7 RID: 16071
		tk2dAnimEvent,
		// Token: 0x04003EC8 RID: 16072
		UnityAnimEvent
	}
}
