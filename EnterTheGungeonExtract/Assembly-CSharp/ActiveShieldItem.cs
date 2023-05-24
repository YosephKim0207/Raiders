using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001335 RID: 4917
public class ActiveShieldItem : PlayerItem
{
	// Token: 0x06006F74 RID: 28532 RVA: 0x002C2B8C File Offset: 0x002C0D8C
	protected override void DoEffect(PlayerController user)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", base.gameObject);
		base.IsCurrentlyActive = true;
		this.instanceShield = user.RegisterAttachedObject(this.prefabToAttachToPlayer, "jetpack", 0f);
		this.instanceShieldSprite = this.instanceShield.GetComponentInChildren<tk2dSprite>();
		if (user.HasActiveBonusSynergy(CustomSynergyType.MIRROR_SHIELD, false))
		{
			this.instanceShieldSprite.spriteAnimator.Play("shield2_on");
		}
		user.ChangeAttachedSpriteDepth(this.instanceShieldSprite, -1f);
		SpeculativeRigidbody specRigidbody = user.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreventBulletCollisions));
		user.specRigidbody.BlockBeams = true;
		user.MovementModifiers += this.NoMotionModifier;
		user.IsStationary = true;
		user.IsGunLocked = true;
		user.OnPreDodgeRoll += this.HandleDodgeRollStarted;
		user.OnTriedToInitiateAttack += this.HandleTriedAttack;
		user.StartCoroutine(this.HandleDuration(user));
	}

	// Token: 0x06006F75 RID: 28533 RVA: 0x002C2CA8 File Offset: 0x002C0EA8
	private IEnumerator HandleDuration(PlayerController user)
	{
		if (user && user.HasActiveBonusSynergy(CustomSynergyType.MIRROR_SHIELD, false))
		{
			yield return null;
			if (this.instanceShieldSprite)
			{
				this.instanceShieldSprite.spriteAnimator.Play("shield2_on");
			}
		}
		float ela = 0f;
		while (ela < this.MaxShieldTime && base.IsCurrentlyActive)
		{
			ela += BraveTime.DeltaTime;
			this.m_activeElapsed = ela;
			this.m_activeDuration = this.MaxShieldTime;
			if (ela > this.MaxShieldTime - this.DurationPortionToFlicker)
			{
				bool flag = ela * 6f % 1f > 0.5f;
				if (this.instanceShieldSprite)
				{
					this.instanceShieldSprite.renderer.enabled = flag;
				}
			}
			yield return null;
		}
		if (base.IsCurrentlyActive)
		{
			if (this.instanceShieldSprite)
			{
				this.instanceShieldSprite.renderer.enabled = true;
				this.instanceShieldSprite.HeightOffGround = 0.5f;
				this.instanceShieldSprite.UpdateZDepth();
			}
			this.DoActiveEffect(user);
		}
		yield break;
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x002C2CCC File Offset: 0x002C0ECC
	private void HandleTriedAttack(PlayerController obj)
	{
		this.DoActiveEffect(obj);
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x002C2CD8 File Offset: 0x002C0ED8
	private void HandleDodgeRollStarted(PlayerController obj)
	{
		this.DoActiveEffect(obj);
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x002C2CE4 File Offset: 0x002C0EE4
	private void PreventBulletCollisions(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.projectile)
		{
			if (this.LastOwner && this.LastOwner.HasActiveBonusSynergy(CustomSynergyType.MIRROR_SHIELD, false))
			{
				PassiveReflectItem.ReflectBullet(otherRigidbody.projectile, true, this.LastOwner, 10f, 1f, 1f, 0f);
			}
			else
			{
				otherRigidbody.projectile.DieInAir(false, true, true, false);
			}
			PhysicsEngine.SkipCollision = true;
		}
		if (otherRigidbody.aiActor)
		{
			if (otherRigidbody.knockbackDoer)
			{
				otherRigidbody.knockbackDoer.ApplyKnockback(otherRigidbody.UnitCenter - myRigidbody.UnitCenter, 25f, false);
			}
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x002C2DB0 File Offset: 0x002C0FB0
	private void LateUpdate()
	{
		if (base.IsCurrentlyActive)
		{
			this.instanceShieldSprite.HeightOffGround = 0.5f;
			this.instanceShieldSprite.UpdateZDepth();
		}
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x002C2DD8 File Offset: 0x002C0FD8
	private void NoMotionModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
	{
		voluntaryVel = Vector2.zero;
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x002C2DE8 File Offset: 0x002C0FE8
	protected override void DoActiveEffect(PlayerController user)
	{
		base.IsCurrentlyActive = false;
		user.MovementModifiers -= this.NoMotionModifier;
		SpeculativeRigidbody specRigidbody = user.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreventBulletCollisions));
		user.specRigidbody.BlockBeams = false;
		Transform parent = this.instanceShield.transform.parent;
		user.DeregisterAttachedObject(this.instanceShield, false);
		user.IsStationary = false;
		user.IsGunLocked = false;
		user.OnPreDodgeRoll -= this.HandleDodgeRollStarted;
		user.OnTriedToInitiateAttack -= this.HandleTriedAttack;
		this.instanceShield.transform.parent = parent;
		this.instanceShieldSprite.spriteAnimator.Play((!user || !user.HasActiveBonusSynergy(CustomSynergyType.MIRROR_SHIELD, false)) ? "shield_off" : "shield2_off");
		tk2dSpriteAnimator spriteAnimator = this.instanceShieldSprite.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DestroyParentObject));
		this.instanceShieldSprite = null;
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x002C2F10 File Offset: 0x002C1110
	private void DestroyParentObject(tk2dSpriteAnimator source, tk2dSpriteAnimationClip clip)
	{
		UnityEngine.Object.Destroy(source.transform.parent.gameObject);
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x002C2F28 File Offset: 0x002C1128
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x002C2F3C File Offset: 0x002C113C
	public override void OnItemSwitched(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x002C2F50 File Offset: 0x002C1150
	protected override void OnDestroy()
	{
		if (this.LastOwner != null)
		{
			this.LastOwner.OnPreDodgeRoll -= this.HandleDodgeRollStarted;
			this.LastOwner.OnTriedToInitiateAttack -= this.HandleTriedAttack;
		}
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(this.LastOwner);
		}
		base.OnDestroy();
	}

	// Token: 0x04006EB2 RID: 28338
	public GameObject prefabToAttachToPlayer;

	// Token: 0x04006EB3 RID: 28339
	public float MaxShieldTime = 7f;

	// Token: 0x04006EB4 RID: 28340
	public float DurationPortionToFlicker = 2f;

	// Token: 0x04006EB5 RID: 28341
	private GameObject instanceShield;

	// Token: 0x04006EB6 RID: 28342
	private tk2dSprite instanceShieldSprite;
}
