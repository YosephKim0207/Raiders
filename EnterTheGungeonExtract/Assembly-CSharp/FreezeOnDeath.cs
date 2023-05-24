using System;
using UnityEngine;

// Token: 0x02001093 RID: 4243
public class FreezeOnDeath : BraveBehaviour
{
	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x06005D5A RID: 23898 RVA: 0x0023CB34 File Offset: 0x0023AD34
	// (set) Token: 0x06005D5B RID: 23899 RVA: 0x0023CB3C File Offset: 0x0023AD3C
	public bool IsDisintegrating { get; set; }

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x06005D5C RID: 23900 RVA: 0x0023CB48 File Offset: 0x0023AD48
	// (set) Token: 0x06005D5D RID: 23901 RVA: 0x0023CB50 File Offset: 0x0023AD50
	public bool IsDeathFrozen { get; set; }

	// Token: 0x06005D5E RID: 23902 RVA: 0x0023CB5C File Offset: 0x0023AD5C
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005D5F RID: 23903 RVA: 0x0023CB84 File Offset: 0x0023AD84
	protected override void OnDestroy()
	{
		if (base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeathCompleted));
		}
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
		StaticReferenceManager.AllCorpses.Add(base.gameObject);
		base.OnDestroy();
	}

	// Token: 0x06005D60 RID: 23904 RVA: 0x0023CC18 File Offset: 0x0023AE18
	private void OnPreDeath(Vector2 dir)
	{
		if (base.aiActor && base.healthHaver && base.aiActor.IsFalling)
		{
			base.healthHaver.ManualDeathHandling = false;
			return;
		}
		base.aiAnimator.PlayUntilCancelled(this.deathFreezeAnim, true, null, -1f, false);
		this.IsDeathFrozen = true;
		base.aiActor.IsFrozen = true;
		base.aiActor.ForceDeath(Vector2.zero, false);
		base.aiActor.ImmuneToAllEffects = true;
		base.aiActor.RemoveAllEffects(true);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		StaticReferenceManager.AllCorpses.Add(base.gameObject);
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x0023CCF0 File Offset: 0x0023AEF0
	private void OnCollision(CollisionData collisionData)
	{
		if (collisionData.OtherRigidbody)
		{
			if (collisionData.OtherRigidbody.projectile)
			{
				this.DoFullDeath(this.deathShatterAnim);
				return;
			}
			PlayerController component = collisionData.OtherRigidbody.GetComponent<PlayerController>();
			if (component && component.IsDodgeRolling)
			{
				this.DoFullDeath(this.deathInstantShatterAnim);
				return;
			}
		}
	}

	// Token: 0x06005D62 RID: 23906 RVA: 0x0023CD60 File Offset: 0x0023AF60
	private void DeathVfxTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (frame.eventInfo == "vfx")
		{
			SpawnManager.SpawnVFX(this.shatterVfx, base.specRigidbody.HitboxPixelCollider.UnitCenter, Quaternion.identity);
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.DeathVfxTriggered));
		}
	}

	// Token: 0x06005D63 RID: 23907 RVA: 0x0023CDD8 File Offset: 0x0023AFD8
	private void DeathCompleted(tk2dSpriteAnimator tk2DSpriteAnimator, tk2dSpriteAnimationClip tk2DSpriteAnimationClip)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005D64 RID: 23908 RVA: 0x0023CDE8 File Offset: 0x0023AFE8
	public void HandleDisintegration()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		base.specRigidbody.enabled = false;
	}

	// Token: 0x06005D65 RID: 23909 RVA: 0x0023CE20 File Offset: 0x0023B020
	private void DoFullDeath(string deathAnim)
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		base.specRigidbody.enabled = false;
		base.aiAnimator.PlayUntilCancelled(deathAnim, true, null, -1f, false);
		if (this.shatterVfx)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.DeathVfxTriggered));
		}
		tk2dSpriteAnimator spriteAnimator2 = base.spriteAnimator;
		spriteAnimator2.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator2.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeathCompleted));
		StaticReferenceManager.AllCorpses.Remove(base.gameObject);
	}

	// Token: 0x04005742 RID: 22338
	[CheckDirectionalAnimation(null)]
	public string deathFreezeAnim;

	// Token: 0x04005743 RID: 22339
	[CheckDirectionalAnimation(null)]
	public string deathShatterAnim;

	// Token: 0x04005744 RID: 22340
	[CheckDirectionalAnimation(null)]
	public string deathInstantShatterAnim;

	// Token: 0x04005745 RID: 22341
	public GameObject shatterVfx;
}
