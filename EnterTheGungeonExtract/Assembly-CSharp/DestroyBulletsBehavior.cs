using System;
using System.Collections.ObjectModel;
using FullInspector;
using UnityEngine;

// Token: 0x02000D23 RID: 3363
public class DestroyBulletsBehavior : BasicAttackBehavior
{
	// Token: 0x06004703 RID: 18179 RVA: 0x00173600 File Offset: 0x00171800
	public override void Start()
	{
		base.Start();
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
		}
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x00173650 File Offset: 0x00171850
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
		if (this.m_behaviorSpeculator.AttackCooldown <= 0f && this.m_behaviorSpeculator.GlobalCooldown <= 0f && this.m_cooldownTimer < this.SkippableCooldown)
		{
			bool flag = false;
			Vector2 unitCenter = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile.Owner is PlayerController)
				{
					if (projectile.specRigidbody)
					{
						if (Vector2.Distance(unitCenter, projectile.specRigidbody.UnitCenter) <= this.SkippableRadius)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.m_cooldownTimer = 0f;
			}
		}
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x00173750 File Offset: 0x00171950
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.TellAnimation, false, null, -1f, false);
			}
			this.m_state = DestroyBulletsBehavior.State.WaitingForTell;
		}
		else
		{
			this.StartBlanking();
		}
		this.m_aiActor.ClearPath();
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "DestroyBulletsBehavior");
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x0017382C File Offset: 0x00171A2C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == DestroyBulletsBehavior.State.WaitingForTell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
			{
				this.StartBlanking();
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == DestroyBulletsBehavior.State.Blanking)
		{
			Vector2 unitCenter = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;
			float num = this.RadiusCurve.Evaluate(Mathf.InverseLerp(this.BlankTime, 0f, this.m_timer)) * this.Radius;
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile.Owner is PlayerController)
				{
					if (projectile.specRigidbody)
					{
						if (Vector2.Distance(unitCenter, projectile.specRigidbody.UnitCenter) <= num)
						{
							if (this.OverrideHitVfx)
							{
								projectile.hitEffects.overrideMidairDeathVFX = this.OverrideHitVfx;
							}
							projectile.DieInAir(false, true, true, false);
						}
					}
				}
			}
			if (this.m_timer <= 0f)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x00173964 File Offset: 0x00171B64
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.TellAnimation);
		}
		if (!string.IsNullOrEmpty(this.BlankAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.BlankAnimation);
		}
		if (!string.IsNullOrEmpty(this.BlankVfx))
		{
			this.m_aiAnimator.StopVfx(this.BlankVfx);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "DestroyBulletsBehavior");
		}
		this.m_state = DestroyBulletsBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x00173A2C File Offset: 0x00171C2C
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (this.m_state == DestroyBulletsBehavior.State.WaitingForTell && frame.eventInfo == "blank")
		{
			this.StartBlanking();
		}
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x00173A68 File Offset: 0x00171C68
	private void StartBlanking()
	{
		if (!string.IsNullOrEmpty(this.BlankAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.BlankAnimation, false, null, -1f, false);
		}
		if (!string.IsNullOrEmpty(this.BlankVfx))
		{
			this.m_aiAnimator.PlayVfx(this.BlankVfx, null, null, null);
		}
		this.m_timer = this.BlankTime;
		this.m_state = DestroyBulletsBehavior.State.Blanking;
	}

	// Token: 0x040039E1 RID: 14817
	public float SkippableCooldown;

	// Token: 0x040039E2 RID: 14818
	public float SkippableRadius;

	// Token: 0x040039E3 RID: 14819
	public float Radius;

	// Token: 0x040039E4 RID: 14820
	public float BlankTime;

	// Token: 0x040039E5 RID: 14821
	public AnimationCurve RadiusCurve;

	// Token: 0x040039E6 RID: 14822
	[InspectorCategory("Visuals")]
	public string TellAnimation;

	// Token: 0x040039E7 RID: 14823
	[InspectorCategory("Visuals")]
	public string BlankAnimation;

	// Token: 0x040039E8 RID: 14824
	[InspectorCategory("Visuals")]
	public string BlankVfx;

	// Token: 0x040039E9 RID: 14825
	[InspectorCategory("Visuals")]
	public GameObject OverrideHitVfx;

	// Token: 0x040039EA RID: 14826
	private DestroyBulletsBehavior.State m_state;

	// Token: 0x040039EB RID: 14827
	private float m_timer;

	// Token: 0x02000D24 RID: 3364
	private enum State
	{
		// Token: 0x040039ED RID: 14829
		Idle,
		// Token: 0x040039EE RID: 14830
		WaitingForTell,
		// Token: 0x040039EF RID: 14831
		Blanking
	}
}
