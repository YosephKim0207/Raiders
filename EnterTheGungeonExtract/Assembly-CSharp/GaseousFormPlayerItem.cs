using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001408 RID: 5128
public class GaseousFormPlayerItem : PlayerItem
{
	// Token: 0x06007467 RID: 29799 RVA: 0x002E518C File Offset: 0x002E338C
	protected override void DoEffect(PlayerController user)
	{
		if (!user)
		{
			return;
		}
		user.StartCoroutine(this.HandleDuration(user));
		AkSoundEngine.PostEvent("Play_OBJ_metalskin_activate_01", base.gameObject);
	}

	// Token: 0x06007468 RID: 29800 RVA: 0x002E51BC File Offset: 0x002E33BC
	private void ChangeRendering(PlayerController user, bool val)
	{
		if (!user)
		{
			return;
		}
		if (val)
		{
			user.ChangeSpecialShaderFlag(0, 1f);
			user.FlatColorOverridden = true;
			user.ChangeFlatColorOverride(new Color(0.4f, 0.31f, 0.49f, 1f));
			user.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
			user.ToggleShadowVisiblity(false);
			SpriteOutlineManager.RemoveOutlineFromSprite(user.sprite, true);
		}
		else
		{
			user.ChangeSpecialShaderFlag(0, 0f);
			user.FlatColorOverridden = false;
			user.ChangeFlatColorOverride(Color.clear);
			user.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
			user.ToggleShadowVisiblity(true);
			if (!SpriteOutlineManager.HasOutline(user.sprite))
			{
				SpriteOutlineManager.AddOutlineToSprite(user.sprite, user.outlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
		}
	}

	// Token: 0x06007469 RID: 29801 RVA: 0x002E529C File Offset: 0x002E349C
	private IEnumerator HandleDuration(PlayerController user)
	{
		base.IsCurrentlyActive = true;
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.Duration;
		if (user && user.specRigidbody)
		{
			user.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.Projectile));
		}
		if (user)
		{
			user.IsEthereal = true;
			if (user.healthHaver)
			{
				user.healthHaver.IsVulnerable = false;
			}
			user.SetIsFlying(true, "gaseousform", true, false);
			user.SetCapableOfStealing(true, "GaseousFormPlayerItem", null);
			this.ChangeRendering(user, true);
		}
		float elapsed = 0f;
		while (elapsed < this.Duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (user && user.healthHaver)
			{
				user.healthHaver.IsVulnerable = false;
			}
			yield return null;
		}
		if (user)
		{
			this.ChangeRendering(user, false);
			user.SetIsFlying(false, "gaseousform", true, false);
			user.IsEthereal = false;
			if (user.healthHaver)
			{
				user.healthHaver.IsVulnerable = true;
			}
			user.SetCapableOfStealing(false, "GaseousFormPlayerItem", null);
			if (user.specRigidbody)
			{
				user.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.Projectile));
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(user.specRigidbody, null, false);
			}
		}
		base.IsCurrentlyActive = false;
		if (this)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
		}
		yield break;
	}

	// Token: 0x0600746A RID: 29802 RVA: 0x002E52C0 File Offset: 0x002E34C0
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			base.StopAllCoroutines();
			if (user)
			{
				this.ChangeRendering(user, false);
				user.SetIsFlying(false, "gaseousform", true, false);
				user.IsEthereal = false;
				if (user.healthHaver)
				{
					user.healthHaver.IsVulnerable = true;
				}
				user.SetCapableOfStealing(false, "GaseousFormPlayerItem", null);
				if (user.specRigidbody)
				{
					user.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.Projectile));
					PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(user.specRigidbody, null, false);
				}
				base.IsCurrentlyActive = false;
			}
			if (this)
			{
				AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
			}
		}
	}

	// Token: 0x0600746B RID: 29803 RVA: 0x002E5398 File Offset: 0x002E3598
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007612 RID: 30226
	public float Duration = 5f;
}
