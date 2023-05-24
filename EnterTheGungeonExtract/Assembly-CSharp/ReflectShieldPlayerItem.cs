using System;
using System.Collections;

// Token: 0x02001481 RID: 5249
public class ReflectShieldPlayerItem : PlayerItem
{
	// Token: 0x06007759 RID: 30553 RVA: 0x002F941C File Offset: 0x002F761C
	protected override void DoEffect(PlayerController user)
	{
		this.userSRB = user.specRigidbody;
		user.StartCoroutine(this.HandleShield(user));
		AkSoundEngine.PostEvent("Play_OBJ_metalskin_activate_01", base.gameObject);
	}

	// Token: 0x0600775A RID: 30554 RVA: 0x002F944C File Offset: 0x002F764C
	private IEnumerator HandleShield(PlayerController user)
	{
		base.IsCurrentlyActive = true;
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		this.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
		user.sprite.usesOverrideMaterial = true;
		user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
		SpeculativeRigidbody specRigidbody = user.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		user.healthHaver.IsVulnerable = false;
		float elapsed = 0f;
		while (elapsed < this.duration)
		{
			elapsed += BraveTime.DeltaTime;
			user.healthHaver.IsVulnerable = false;
			yield return null;
		}
		if (user)
		{
			user.healthHaver.IsVulnerable = true;
			user.ClearOverrideShader();
			user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
			SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			base.IsCurrentlyActive = false;
		}
		if (this)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
		}
		yield break;
	}

	// Token: 0x0600775B RID: 30555 RVA: 0x002F9470 File Offset: 0x002F7670
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		Projectile component = otherRigidbody.GetComponent<Projectile>();
		if (component != null && !(component.Owner is PlayerController))
		{
			PassiveReflectItem.ReflectBullet(component, true, this.userSRB.gameActor, 10f, 1f, 1f, 0f);
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x0600775C RID: 30556 RVA: 0x002F94CC File Offset: 0x002F76CC
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			base.StopAllCoroutines();
			if (user)
			{
				user.healthHaver.IsVulnerable = true;
				user.ClearOverrideShader();
				user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
				SpeculativeRigidbody specRigidbody = user.specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
				base.IsCurrentlyActive = false;
			}
			if (this)
			{
				AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
			}
		}
	}

	// Token: 0x0600775D RID: 30557 RVA: 0x002F9564 File Offset: 0x002F7764
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007955 RID: 31061
	public float duration = 5f;

	// Token: 0x04007956 RID: 31062
	protected SpeculativeRigidbody userSRB;

	// Token: 0x04007957 RID: 31063
	private bool m_usedOverrideMaterial;
}
