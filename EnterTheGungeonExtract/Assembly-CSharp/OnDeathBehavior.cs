using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010B5 RID: 4277
public abstract class OnDeathBehavior : BraveBehaviour
{
	// Token: 0x06005E4A RID: 24138 RVA: 0x00242920 File Offset: 0x00240B20
	public virtual void Start()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath += this.OnPreDeath;
			if (this.deathType == OnDeathBehavior.DeathType.Death)
			{
				base.healthHaver.OnDeath += this.OnDeath;
			}
			else if (this.deathType == OnDeathBehavior.DeathType.DeathAnimTrigger)
			{
				tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
			}
		}
	}

	// Token: 0x06005E4B RID: 24139 RVA: 0x002429B0 File Offset: 0x00240BB0
	protected override void OnDestroy()
	{
		if (base.healthHaver)
		{
			if (this.deathType == OnDeathBehavior.DeathType.Death)
			{
				base.healthHaver.OnDeath -= this.OnDeath;
			}
			else if (this.deathType == OnDeathBehavior.DeathType.DeathAnimTrigger)
			{
				tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06005E4C RID: 24140
	protected abstract void OnTrigger(Vector2 dirVec);

	// Token: 0x06005E4D RID: 24141 RVA: 0x00242A30 File Offset: 0x00240C30
	private void OnPreDeath(Vector2 dirVec)
	{
		this.m_deathDir = dirVec;
		if (this.deathType == OnDeathBehavior.DeathType.PreDeath)
		{
			if (this.preDeathDelay > 0f)
			{
				base.StartCoroutine(this.DelayedOnTriggerCR(this.preDeathDelay));
			}
			else
			{
				this.OnTrigger(this.m_deathDir);
			}
		}
	}

	// Token: 0x06005E4E RID: 24142 RVA: 0x00242A84 File Offset: 0x00240C84
	private void OnDeath(Vector2 dirVec)
	{
		this.OnTrigger(this.m_deathDir);
	}

	// Token: 0x06005E4F RID: 24143 RVA: 0x00242A94 File Offset: 0x00240C94
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (base.healthHaver.IsDead && clip.GetFrame(frame).eventInfo == this.triggerName)
		{
			this.OnTrigger(this.m_deathDir);
		}
	}

	// Token: 0x06005E50 RID: 24144 RVA: 0x00242AD0 File Offset: 0x00240CD0
	private IEnumerator DelayedOnTriggerCR(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.OnTrigger(this.m_deathDir);
		yield break;
	}

	// Token: 0x0400585A RID: 22618
	public OnDeathBehavior.DeathType deathType = OnDeathBehavior.DeathType.Death;

	// Token: 0x0400585B RID: 22619
	[ShowInInspectorIf("deathType", 0, false)]
	public float preDeathDelay;

	// Token: 0x0400585C RID: 22620
	[ShowInInspectorIf("deathType", 2, false)]
	public string triggerName;

	// Token: 0x0400585D RID: 22621
	private Vector2 m_deathDir;

	// Token: 0x020010B6 RID: 4278
	public enum DeathType
	{
		// Token: 0x0400585F RID: 22623
		PreDeath,
		// Token: 0x04005860 RID: 22624
		Death,
		// Token: 0x04005861 RID: 22625
		DeathAnimTrigger
	}
}
