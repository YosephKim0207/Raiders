using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010B4 RID: 4276
public class MirrorImageController : BraveBehaviour
{
	// Token: 0x06005E40 RID: 24128 RVA: 0x0024254C File Offset: 0x0024074C
	public void Awake()
	{
		base.aiActor.CanDropCurrency = false;
		base.aiActor.CanDropItems = false;
		base.aiActor.CollisionDamage = 0f;
		if (base.aiActor.encounterTrackable)
		{
			UnityEngine.Object.Destroy(base.aiActor.encounterTrackable);
		}
		base.behaviorSpeculator.AttackCooldown = 10f;
		base.RegenerateCache();
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x002425BC File Offset: 0x002407BC
	public void Update()
	{
		base.behaviorSpeculator.AttackCooldown = 10f;
		if (this.m_host)
		{
			if (this.m_host.behaviorSpeculator.ActiveContinuousAttackBehavior != null)
			{
				base.aiActor.ClearPath();
				base.behaviorSpeculator.GlobalCooldown = 1f;
			}
			else
			{
				base.behaviorSpeculator.GlobalCooldown = 0f;
			}
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x00242630 File Offset: 0x00240830
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_host)
		{
			this.m_host.healthHaver.OnPreDeath -= this.OnHostPreDeath;
			AIAnimator aiAnimator = this.m_host.aiAnimator;
			aiAnimator.OnPlayUntilFinished = (AIAnimator.PlayUntilFinishedDelegate)Delegate.Remove(aiAnimator.OnPlayUntilFinished, new AIAnimator.PlayUntilFinishedDelegate(this.PlayUntilFinished));
			AIAnimator aiAnimator2 = this.m_host.aiAnimator;
			aiAnimator2.OnEndAnimationIf = (AIAnimator.EndAnimationIfDelegate)Delegate.Remove(aiAnimator2.OnEndAnimationIf, new AIAnimator.EndAnimationIfDelegate(this.EndAnimationIf));
			AIAnimator aiAnimator3 = this.m_host.aiAnimator;
			aiAnimator3.OnPlayVfx = (AIAnimator.PlayVfxDelegate)Delegate.Remove(aiAnimator3.OnPlayVfx, new AIAnimator.PlayVfxDelegate(this.PlayVfx));
			AIAnimator aiAnimator4 = this.m_host.aiAnimator;
			aiAnimator4.OnStopVfx = (AIAnimator.StopVfxDelegate)Delegate.Remove(aiAnimator4.OnStopVfx, new AIAnimator.StopVfxDelegate(this.StopVfx));
		}
	}

	// Token: 0x06005E43 RID: 24131 RVA: 0x00242720 File Offset: 0x00240920
	public void SetHost(AIActor host)
	{
		this.m_host = host;
		if (!this.m_host)
		{
			return;
		}
		base.aiAnimator.CopyStateFrom(this.m_host.aiAnimator);
		AIAnimator aiAnimator = this.m_host.aiAnimator;
		aiAnimator.OnPlayUntilFinished = (AIAnimator.PlayUntilFinishedDelegate)Delegate.Combine(aiAnimator.OnPlayUntilFinished, new AIAnimator.PlayUntilFinishedDelegate(this.PlayUntilFinished));
		AIAnimator aiAnimator2 = this.m_host.aiAnimator;
		aiAnimator2.OnEndAnimationIf = (AIAnimator.EndAnimationIfDelegate)Delegate.Combine(aiAnimator2.OnEndAnimationIf, new AIAnimator.EndAnimationIfDelegate(this.EndAnimationIf));
		AIAnimator aiAnimator3 = this.m_host.aiAnimator;
		aiAnimator3.OnPlayVfx = (AIAnimator.PlayVfxDelegate)Delegate.Combine(aiAnimator3.OnPlayVfx, new AIAnimator.PlayVfxDelegate(this.PlayVfx));
		AIAnimator aiAnimator4 = this.m_host.aiAnimator;
		aiAnimator4.OnStopVfx = (AIAnimator.StopVfxDelegate)Delegate.Combine(aiAnimator4.OnStopVfx, new AIAnimator.StopVfxDelegate(this.StopVfx));
		this.m_host.healthHaver.OnPreDeath += this.OnHostPreDeath;
	}

	// Token: 0x06005E44 RID: 24132 RVA: 0x00242828 File Offset: 0x00240A28
	private void OnHostPreDeath(Vector2 deathDir)
	{
		base.healthHaver.ApplyDamage(100000f, Vector2.zero, "Mirror Host Death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
	}

	// Token: 0x06005E45 RID: 24133 RVA: 0x00242854 File Offset: 0x00240A54
	private void PlayUntilFinished(string name, bool suppressHitStates, string overrideHitState, float warpClipDuration, bool skipChildAnimators)
	{
		if (base.healthHaver.IsAlive && this.MirrorAnimations.Contains(name))
		{
			base.aiAnimator.PlayUntilFinished(name, suppressHitStates, overrideHitState, warpClipDuration, skipChildAnimators);
		}
	}

	// Token: 0x06005E46 RID: 24134 RVA: 0x0024288C File Offset: 0x00240A8C
	private void EndAnimationIf(string name)
	{
		if (base.healthHaver.IsAlive)
		{
			base.aiAnimator.EndAnimationIf(name);
		}
	}

	// Token: 0x06005E47 RID: 24135 RVA: 0x002428AC File Offset: 0x00240AAC
	private void PlayVfx(string name, Vector2? sourceNormal, Vector2? sourceVelocity, Vector2? position)
	{
		if (base.healthHaver.IsAlive && this.MirrorAnimations.Contains(name))
		{
			base.aiAnimator.PlayVfx(name, sourceNormal, sourceVelocity, position);
		}
	}

	// Token: 0x06005E48 RID: 24136 RVA: 0x002428E0 File Offset: 0x00240AE0
	private void StopVfx(string name)
	{
		if (base.healthHaver.IsAlive && this.MirrorAnimations.Contains(name))
		{
			base.aiAnimator.StopVfx(name);
		}
	}

	// Token: 0x04005858 RID: 22616
	public List<string> MirrorAnimations = new List<string>();

	// Token: 0x04005859 RID: 22617
	private AIActor m_host;
}
