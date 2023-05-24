using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB2 RID: 3506
[InspectorDropdownName("Bosses/DraGun/ThrowKnifeBehavior")]
public class DraGunThrowKnifeBehavior : BasicAttackBehavior
{
	// Token: 0x06004A3D RID: 19005 RVA: 0x0018DB84 File Offset: 0x0018BD84
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

	// Token: 0x06004A3E RID: 19006 RVA: 0x0018DBE4 File Offset: 0x0018BDE4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x0018DBFC File Offset: 0x0018BDFC
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

	// Token: 0x06004A40 RID: 19008 RVA: 0x0018DC5C File Offset: 0x0018BE5C
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

	// Token: 0x06004A41 RID: 19009 RVA: 0x0018DCF0 File Offset: 0x0018BEF0
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
		this.m_isAttacking = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x0018DD84 File Offset: 0x0018BF84
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x0018DD88 File Offset: 0x0018BF88
	public override bool IsReady()
	{
		if (!base.IsReady())
		{
			return false;
		}
		List<AIActor> activeEnemies = this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i].name.Contains("knife", true))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x0018DDEC File Offset: 0x0018BFEC
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_isAttacking && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.m_dragun.bulletBank.CreateProjectileFromBank(this.ShootPoint.transform.position, this.angle, "knife", null, false, true, false);
		}
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x0018DE54 File Offset: 0x0018C054
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
		this.m_isAttacking = true;
	}

	// Token: 0x04003EF0 RID: 16112
	public float delay;

	// Token: 0x04003EF1 RID: 16113
	public GameObject ShootPoint;

	// Token: 0x04003EF2 RID: 16114
	public string BulletName;

	// Token: 0x04003EF3 RID: 16115
	public float angle;

	// Token: 0x04003EF4 RID: 16116
	public Animation unityAnimation;

	// Token: 0x04003EF5 RID: 16117
	public string unityShootAnim;

	// Token: 0x04003EF6 RID: 16118
	public AIAnimator aiAnimator;

	// Token: 0x04003EF7 RID: 16119
	public string aiShootAnim;

	// Token: 0x04003EF8 RID: 16120
	private DraGunController m_dragun;

	// Token: 0x04003EF9 RID: 16121
	private float m_timer;

	// Token: 0x04003EFA RID: 16122
	private bool m_isAttacking;
}
