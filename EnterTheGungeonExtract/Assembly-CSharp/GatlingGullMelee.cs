using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB7 RID: 3511
[InspectorDropdownName("Bosses/GatlingGull/Melee")]
public class GatlingGullMelee : BasicAttackBehavior
{
	// Token: 0x06004A65 RID: 19045 RVA: 0x0018F4DC File Offset: 0x0018D6DC
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004A66 RID: 19046 RVA: 0x0018F4E4 File Offset: 0x0018D6E4
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004A67 RID: 19047 RVA: 0x0018F4EC File Offset: 0x0018D6EC
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
		if ((this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.UnitCenter).magnitude < this.TriggerDistance)
		{
			this.m_aiAnimator.PlayUntilFinished("melee", true, null, -1f, false);
			this.m_aiActor.ClearPath();
			tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x0018F5B8 File Offset: 0x0018D7B8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_aiActor.spriteAnimator.IsPlaying("melee"))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A69 RID: 19049 RVA: 0x0018F5E0 File Offset: 0x0018D7E0
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.UpdateCooldowns();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
	}

	// Token: 0x06004A6A RID: 19050 RVA: 0x0018F61C File Offset: 0x0018D81C
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x0018F620 File Offset: 0x0018D820
	private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventInfo == "melee_hit")
		{
			SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
			if (targetRigidbody)
			{
				Vector2 vector = targetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.CenterPoint.transform.position.XY();
				if (vector.magnitude < this.DamageDistance)
				{
					PlayerController playerController = ((!targetRigidbody.gameActor) ? null : (targetRigidbody.gameActor as PlayerController));
					if (targetRigidbody.healthHaver && targetRigidbody.healthHaver.IsVulnerable && (!playerController || !playerController.IsEthereal))
					{
						targetRigidbody.healthHaver.ApplyDamage(this.Damage, vector.normalized, this.m_aiActor.GetActorName(), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						if (targetRigidbody.knockbackDoer)
						{
							targetRigidbody.knockbackDoer.ApplyKnockback(vector.normalized, this.KnockbackForce, false);
						}
						StickyFrictionManager.Instance.RegisterCustomStickyFriction(this.StickyFriction, 0f, false, false);
					}
					if (targetRigidbody.majorBreakable)
					{
						targetRigidbody.majorBreakable.ApplyDamage(1000f, vector.normalized, true, false, false);
					}
				}
			}
		}
	}

	// Token: 0x04003F29 RID: 16169
	public float TriggerDistance = 4f;

	// Token: 0x04003F2A RID: 16170
	public float Damage = 1f;

	// Token: 0x04003F2B RID: 16171
	public float KnockbackForce = 30f;

	// Token: 0x04003F2C RID: 16172
	public GameObject CenterPoint;

	// Token: 0x04003F2D RID: 16173
	public float DamageDistance;

	// Token: 0x04003F2E RID: 16174
	public float StickyFriction = 0.1f;
}
