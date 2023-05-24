using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DA7 RID: 3495
[InspectorDropdownName("Bosses/DraGun/GrenadeBehavior")]
public class DraGunGrenadeBehavior : BasicAttackBehavior
{
	// Token: 0x060049FE RID: 18942 RVA: 0x0018C0D4 File Offset: 0x0018A2D4
	public override void Start()
	{
		base.Start();
		this.m_dragun = this.m_aiActor.GetComponent<DraGunController>();
		if (this.aiAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = this.aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x0018C134 File Offset: 0x0018A334
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x0018C14C File Offset: 0x0018A34C
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
		if (this.delay <= 0f)
		{
			this.StartAttack();
		}
		else
		{
			this.m_timer = this.delay;
			this.m_isAttacking = false;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x0018C1AC File Offset: 0x0018A3AC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_isAttacking)
		{
			if (this.m_timer <= 0f)
			{
				this.StartAttack();
			}
		}
		else if (!this.m_isAttacking2)
		{
			if (this.m_timer <= 0f)
			{
				this.StartAttack2();
			}
		}
		else
		{
			bool flag = true;
			if (this.unityAnimation)
			{
				flag &= !this.unityAnimation.IsPlaying(this.unityShootAnim);
			}
			if (this.aiAnimator)
			{
				flag &= !this.aiAnimator.IsPlaying(this.aiShootAnim);
			}
			if (this.unityAnimation2)
			{
				flag &= !this.unityAnimation2.IsPlaying(this.unityShootAnim2);
			}
			if (this.aiAnimator2)
			{
				flag &= !this.aiAnimator2.IsPlaying(this.aiShootAnim2);
			}
			if (flag)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x0018C2B4 File Offset: 0x0018A4B4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.aiAnimator)
		{
			this.aiAnimator.EndAnimation();
		}
		if (this.unityAnimation)
		{
			this.unityAnimation.Stop();
			this.unityAnimation.GetClip(this.unityShootAnim).SampleAnimation(this.unityAnimation.gameObject, 1000f);
			this.unityAnimation.GetComponent<DraGunArmController>().UnclipHandSprite();
		}
		if (this.aiAnimator2)
		{
			this.aiAnimator2.EndAnimation();
		}
		if (this.unityAnimation2)
		{
			this.unityAnimation2.Stop();
			this.unityAnimation2.GetClip(this.unityShootAnim2).SampleAnimation(this.unityAnimation2.gameObject, 1000f);
			this.unityAnimation2.GetComponent<DraGunArmController>().UnclipHandSprite();
		}
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.overrideHeadPosition)
		{
			this.m_dragun.OverrideTargetX = null;
		}
		this.m_isAttacking = false;
		this.m_isAttacking2 = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x0018C3F8 File Offset: 0x0018A5F8
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_isAttacking && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.Fire();
		}
	}

	// Token: 0x06004A04 RID: 18948 RVA: 0x0018C428 File Offset: 0x0018A628
	private void StartAttack()
	{
		if (this.unityAnimation)
		{
			this.unityAnimation.Play(this.unityShootAnim);
		}
		if (this.aiAnimator)
		{
			this.aiAnimator.PlayUntilCancelled(this.aiShootAnim, false, null, -1f, false);
		}
		if (this.overrideHeadPosition)
		{
			this.m_dragun.OverrideTargetX = new float?(this.headPosition);
		}
		this.m_isAttacking = true;
		if (this.delay2 <= 0f)
		{
			this.StartAttack2();
		}
		else
		{
			this.m_timer = this.delay2;
			this.m_isAttacking2 = false;
		}
	}

	// Token: 0x06004A05 RID: 18949 RVA: 0x0018C4D8 File Offset: 0x0018A6D8
	private void StartAttack2()
	{
		if (this.unityAnimation2)
		{
			this.unityAnimation2.Play(this.unityShootAnim2);
		}
		if (this.aiAnimator2)
		{
			this.aiAnimator2.PlayUntilCancelled(this.aiShootAnim2, false, null, -1f, false);
		}
		this.m_isAttacking2 = true;
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x0018C538 File Offset: 0x0018A738
	private void Fire()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x04003E87 RID: 16007
	public float delay;

	// Token: 0x04003E88 RID: 16008
	public float delay2;

	// Token: 0x04003E89 RID: 16009
	public GameObject ShootPoint;

	// Token: 0x04003E8A RID: 16010
	public BulletScriptSelector BulletScript;

	// Token: 0x04003E8B RID: 16011
	public Animation unityAnimation;

	// Token: 0x04003E8C RID: 16012
	public string unityShootAnim;

	// Token: 0x04003E8D RID: 16013
	public AIAnimator aiAnimator;

	// Token: 0x04003E8E RID: 16014
	public string aiShootAnim;

	// Token: 0x04003E8F RID: 16015
	public Animation unityAnimation2;

	// Token: 0x04003E90 RID: 16016
	public string unityShootAnim2;

	// Token: 0x04003E91 RID: 16017
	public AIAnimator aiAnimator2;

	// Token: 0x04003E92 RID: 16018
	public string aiShootAnim2;

	// Token: 0x04003E93 RID: 16019
	public bool overrideHeadPosition;

	// Token: 0x04003E94 RID: 16020
	[InspectorShowIf("overrideHeadPosition")]
	public float headPosition;

	// Token: 0x04003E95 RID: 16021
	private DraGunController m_dragun;

	// Token: 0x04003E96 RID: 16022
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003E97 RID: 16023
	private float m_timer;

	// Token: 0x04003E98 RID: 16024
	private bool m_isAttacking;

	// Token: 0x04003E99 RID: 16025
	private bool m_isAttacking2;
}
