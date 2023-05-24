using System;
using UnityEngine;

// Token: 0x02000D28 RID: 3368
public class ExplodeInRadius : AttackBehaviorBase
{
	// Token: 0x06004719 RID: 18201 RVA: 0x00173FE4 File Offset: 0x001721E4
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimationClip clipByName = this.m_gameObject.GetComponent<tk2dSpriteAnimator>().GetClipByName("explode");
		if (clipByName != null)
		{
			this.m_explodeTime = (float)clipByName.frames.Length / clipByName.fps;
		}
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x0017402C File Offset: 0x0017222C
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.minLifetime > 0f)
		{
			this.m_lifetime += this.m_deltaTime;
		}
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x00174058 File Offset: 0x00172258
	public override BehaviorResult Update()
	{
		if (this.m_aiActor.healthHaver.IsDead)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (this.minLifetime > 0f && this.m_lifetime < this.minLifetime)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_aiActor.TargetRigidbody != null && this.m_aiActor.DistanceToTarget < this.explodeDistance)
		{
			this.m_closeEnoughToExplodeTimer += this.m_deltaTime;
			if (this.m_closeEnoughToExplodeTimer > this.explodeCountDown)
			{
				this.m_aiAnimator.PlayForDuration("explode", this.m_explodeTime, false, null, -1f, false);
				if (this.stopMovement)
				{
					this.m_aiActor.ClearPath();
				}
				this.m_updateEveryFrame = true;
				return BehaviorResult.RunContinuous;
			}
		}
		else
		{
			this.m_closeEnoughToExplodeTimer = 0f;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x00174140 File Offset: 0x00172340
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_elapsed < this.m_explodeTime)
		{
			this.m_elapsed += this.m_deltaTime;
			return ContinuousBehaviorResult.Continue;
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x0017416C File Offset: 0x0017236C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_aiActor.healthHaver.PreventAllDamage)
		{
			this.m_aiActor.healthHaver.PreventAllDamage = false;
		}
		ExplodeOnDeath component = this.m_aiActor.GetComponent<ExplodeOnDeath>();
		if (component && component.LinearChainExplosion)
		{
			component.ChainIsReversed = false;
			component.explosionData.damage = 5f;
		}
		if (this.m_aiActor.healthHaver.IsAlive)
		{
			this.m_aiActor.healthHaver.ApplyDamage(float.MaxValue, Vector2.zero, "self-immolation", CoreDamageTypes.Fire, DamageCategory.Unstoppable, true, null, false);
		}
		this.m_updateEveryFrame = false;
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x00174220 File Offset: 0x00172420
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x00174224 File Offset: 0x00172424
	public override float GetMinReadyRange()
	{
		return -1f;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x0017422C File Offset: 0x0017242C
	public override float GetMaxRange()
	{
		return -1f;
	}

	// Token: 0x040039FF RID: 14847
	public float explodeDistance = 1f;

	// Token: 0x04003A00 RID: 14848
	public float explodeCountDown;

	// Token: 0x04003A01 RID: 14849
	public bool stopMovement;

	// Token: 0x04003A02 RID: 14850
	public float minLifetime;

	// Token: 0x04003A03 RID: 14851
	protected float m_closeEnoughToExplodeTimer;

	// Token: 0x04003A04 RID: 14852
	protected float m_explodeTime;

	// Token: 0x04003A05 RID: 14853
	protected float m_lifetime;

	// Token: 0x04003A06 RID: 14854
	protected float m_elapsed;
}
