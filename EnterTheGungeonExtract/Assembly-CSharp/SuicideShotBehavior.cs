using System;
using UnityEngine;

// Token: 0x02000D60 RID: 3424
public class SuicideShotBehavior : BasicAttackBehavior
{
	// Token: 0x06004859 RID: 18521 RVA: 0x0017ECDC File Offset: 0x0017CEDC
	public override void Start()
	{
		base.Start();
		AIBulletBank.Entry bullet = this.m_aiActor.bulletBank.GetBullet(this.bulletBankName);
		this.m_cachedProjectileSpeed = ((!bullet.OverrideProjectile) ? bullet.BulletObject.GetComponent<Projectile>().baseData.speed : bullet.ProjectileData.speed);
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x0017ED68 File Offset: 0x0017CF68
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (this.leadAmount > 0f)
		{
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.m_aiActor.TargetVelocity, this.m_aiActor.specRigidbody.UnitCenter, this.m_cachedProjectileSpeed);
			vector = Vector2.Lerp(vector, predictedPosition, this.leadAmount);
		}
		this.m_cachedTargetCenter = vector;
		float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, vector);
		if (num > this.minRange)
		{
			this.m_aiAnimator.PlayUntilFinished(this.chargeAnim, true, null, -1f, false);
			this.m_aiActor.ClearPath();
			if (this.invulnerableDuringAnimatoin)
			{
				this.m_aiActor.healthHaver.minimumHealth = 1f;
			}
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x0017EE88 File Offset: 0x0017D088
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (!this.m_aiAnimator.IsPlaying(this.chargeAnim))
		{
			Vector2 vector = this.m_cachedTargetCenter;
			if (this.m_aiActor.TargetRigidbody)
			{
				vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				if (this.leadAmount > 0f)
				{
					Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.m_aiActor.TargetVelocity, this.m_aiActor.specRigidbody.UnitCenter, this.m_cachedProjectileSpeed);
					vector = Vector2.Lerp(vector, predictedPosition, this.leadAmount);
				}
			}
			float num = (float)((this.numBullets - 1) * -(float)this.degreesBetween) * 0.5f;
			for (int i = 0; i < this.numBullets; i++)
			{
				Vector2 unitCenter = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;
				this.m_aiActor.bulletBank.CreateProjectileFromBank(unitCenter, (vector - unitCenter).ToAngle() + num, this.bulletBankName, null, false, true, false);
				num += (float)this.degreesBetween;
			}
			if (!string.IsNullOrEmpty(this.fireVfx))
			{
				this.m_aiActor.aiAnimator.PlayVfx(this.fireVfx, null, null, null);
			}
			if (this.suppressNormalDeath)
			{
				this.m_aiActor.healthHaver.ManualDeathHandling = true;
				this.m_aiActor.healthHaver.DisableStickyFriction = true;
			}
			this.m_aiActor.AdditionalSingleCoinDropChance = 0f;
			this.m_aiActor.healthHaver.SuppressDeathSounds = true;
			if (this.invulnerableDuringAnimatoin)
			{
				this.m_aiActor.healthHaver.minimumHealth = 0f;
			}
			this.m_aiActor.healthHaver.ApplyDamage(10000f, Vector2.zero, "Suicide Attack", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			if (this.suppressNormalDeath)
			{
				this.m_aiActor.ParentRoom.DeregisterEnemy(this.m_aiActor, false);
				UnityEngine.Object.Destroy(this.m_aiActor.gameObject);
			}
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x0017F0AC File Offset: 0x0017D2AC
	public override void EndContinuousUpdate()
	{
		if (this.invulnerableDuringAnimatoin)
		{
			this.m_aiActor.healthHaver.minimumHealth = 0f;
		}
		base.EndContinuousUpdate();
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x0017F0D4 File Offset: 0x0017D2D4
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (frame.eventInfo == "disable_shadow" && this.m_aiActor.ShadowObject)
		{
			this.m_aiActor.ShadowObject.SetActive(false);
		}
	}

	// Token: 0x04003BF5 RID: 15349
	public float minRange;

	// Token: 0x04003BF6 RID: 15350
	public float leadAmount;

	// Token: 0x04003BF7 RID: 15351
	public string chargeAnim;

	// Token: 0x04003BF8 RID: 15352
	public int numBullets = 1;

	// Token: 0x04003BF9 RID: 15353
	public int degreesBetween;

	// Token: 0x04003BFA RID: 15354
	public string bulletBankName;

	// Token: 0x04003BFB RID: 15355
	public bool suppressNormalDeath;

	// Token: 0x04003BFC RID: 15356
	public bool invulnerableDuringAnimatoin;

	// Token: 0x04003BFD RID: 15357
	public string fireVfx;

	// Token: 0x04003BFE RID: 15358
	private float m_cachedProjectileSpeed;

	// Token: 0x04003BFF RID: 15359
	private Vector2 m_cachedTargetCenter;
}
