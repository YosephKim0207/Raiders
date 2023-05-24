using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB1 RID: 3505
[InspectorDropdownName("Bosses/DraGun/RPGBehavior")]
public class DraGunRPGBehavior : BasicAttackBehavior
{
	// Token: 0x06004A34 RID: 18996 RVA: 0x0018D82C File Offset: 0x0018BA2C
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

	// Token: 0x06004A35 RID: 18997 RVA: 0x0018D88C File Offset: 0x0018BA8C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A36 RID: 18998 RVA: 0x0018D8A4 File Offset: 0x0018BAA4
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
			this.StartThrow();
		}
		else
		{
			this.m_timer = this.delay;
			this.m_isAttacking = false;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x0018D904 File Offset: 0x0018BB04
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_isAttacking)
		{
			if (this.m_timer <= 0f)
			{
				this.StartThrow();
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
			if (flag)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x0018D998 File Offset: 0x0018BB98
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
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.overrideHeadPosition)
		{
			this.m_dragun.OverrideTargetX = null;
		}
		this.m_isAttacking = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A39 RID: 19001 RVA: 0x0018DA68 File Offset: 0x0018BC68
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_isAttacking && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.Fire();
		}
	}

	// Token: 0x06004A3A RID: 19002 RVA: 0x0018DA98 File Offset: 0x0018BC98
	private void StartThrow()
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
	}

	// Token: 0x06004A3B RID: 19003 RVA: 0x0018DB1C File Offset: 0x0018BD1C
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

	// Token: 0x04003EE3 RID: 16099
	public float delay;

	// Token: 0x04003EE4 RID: 16100
	public GameObject ShootPoint;

	// Token: 0x04003EE5 RID: 16101
	public BulletScriptSelector BulletScript;

	// Token: 0x04003EE6 RID: 16102
	public Animation unityAnimation;

	// Token: 0x04003EE7 RID: 16103
	public string unityShootAnim;

	// Token: 0x04003EE8 RID: 16104
	public AIAnimator aiAnimator;

	// Token: 0x04003EE9 RID: 16105
	public string aiShootAnim;

	// Token: 0x04003EEA RID: 16106
	public bool overrideHeadPosition;

	// Token: 0x04003EEB RID: 16107
	[InspectorShowIf("overrideHeadPosition")]
	public float headPosition;

	// Token: 0x04003EEC RID: 16108
	private DraGunController m_dragun;

	// Token: 0x04003EED RID: 16109
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003EEE RID: 16110
	private float m_timer;

	// Token: 0x04003EEF RID: 16111
	private bool m_isAttacking;
}
