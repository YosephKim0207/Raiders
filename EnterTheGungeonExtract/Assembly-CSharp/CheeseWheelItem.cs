using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013A8 RID: 5032
public class CheeseWheelItem : PlayerItem
{
	// Token: 0x06007206 RID: 29190 RVA: 0x002D4E00 File Offset: 0x002D3000
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		user.StartCoroutine(this.HandleDuration(user));
	}

	// Token: 0x06007207 RID: 29191 RVA: 0x002D4E18 File Offset: 0x002D3018
	private IEnumerator HandleDuration(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			yield break;
		}
		base.IsCurrentlyActive = true;
		GameObject instanceVFX = user.PlayEffectOnActor(this.TransformationVFX, Vector3.zero, true, true, false);
		instanceVFX.transform.localPosition = instanceVFX.transform.localPosition.QuantizeFloor(0.0625f);
		tk2dSprite instanceVFXSprite = instanceVFX.GetComponent<tk2dSprite>();
		tk2dSpriteAnimator instanceVFXAnimator = instanceVFX.GetComponent<tk2dSpriteAnimator>();
		user.IsVisible = false;
		user.ToggleShadowVisiblity(true);
		user.SetIsFlying(true, "pacman", false, false);
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		SpeculativeRigidbody specRigidbody = user.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePrerigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
		specRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		bool hasPlayedBody = false;
		while (this.m_activeElapsed < this.m_activeDuration && base.IsCurrentlyActive)
		{
			user.healthHaver.IsVulnerable = false;
			user.IsOnFire = false;
			user.CurrentPoisonMeterValue = 0f;
			if (user.IsVisible)
			{
				user.IsVisible = false;
				user.ToggleShadowVisiblity(true);
			}
			if (instanceVFXAnimator)
			{
				if (!hasPlayedBody)
				{
					if (!instanceVFXAnimator.IsPlaying("Resourceful_Rat_pac_intro"))
					{
						hasPlayedBody = true;
						instanceVFXAnimator.Play("Resourceful_Rat_pac_player");
					}
				}
				else if (this.m_activeElapsed > this.m_activeDuration - 0.9f && !instanceVFXAnimator.IsPlaying("Resourceful_Rat_pac_outro"))
				{
					instanceVFXAnimator.Play("Resourceful_Rat_pac_outro");
				}
			}
			if (user.specRigidbody.Velocity != Vector2.zero)
			{
				float num = user.specRigidbody.Velocity.ToAngle();
				if (instanceVFX)
				{
					instanceVFX.transform.localRotation = Quaternion.Euler(0f, 0f, num);
					instanceVFXSprite.ForceRotationRebuild();
				}
			}
			yield return null;
		}
		user.healthHaver.IsVulnerable = true;
		SpeculativeRigidbody specRigidbody3 = user.specRigidbody;
		specRigidbody3.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody3.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePrerigidbodyCollision));
		SpeculativeRigidbody specRigidbody4 = user.specRigidbody;
		specRigidbody4.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody4.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		user.SetIsFlying(false, "pacman", false, false);
		user.IsVisible = true;
		if (instanceVFX)
		{
			SpawnManager.Despawn(instanceVFX);
		}
		base.IsCurrentlyActive = false;
		yield break;
	}

	// Token: 0x06007208 RID: 29192 RVA: 0x002D4E3C File Offset: 0x002D303C
	private void HandlePrerigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody && otherRigidbody.healthHaver && otherRigidbody.healthHaver.IsDead)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06007209 RID: 29193 RVA: 0x002D4E70 File Offset: 0x002D3070
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		AIActor component = rigidbodyCollision.OtherRigidbody.GetComponent<AIActor>();
		bool flag = false;
		if (component && component.IsNormalEnemy && component.healthHaver && component.healthHaver.IsVulnerable)
		{
			if (component.FlagToSetOnDeath == GungeonFlags.BOSSKILLED_DEMONWALL)
			{
				flag = true;
				component.healthHaver.ApplyDamage(this.BossContactDamage, rigidbodyCollision.Normal * -1f, "pakku pakku", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				if (rigidbodyCollision.MyRigidbody && rigidbodyCollision.MyRigidbody.knockbackDoer)
				{
					rigidbodyCollision.MyRigidbody.knockbackDoer.ApplyKnockback(Vector2.down, 80f, false);
				}
			}
			else if (component.healthHaver.IsBoss)
			{
				flag = true;
				component.healthHaver.ApplyDamage(this.BossContactDamage, rigidbodyCollision.Normal * -1f, "pakku pakku", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				if (rigidbodyCollision.MyRigidbody && rigidbodyCollision.MyRigidbody.knockbackDoer)
				{
					rigidbodyCollision.MyRigidbody.knockbackDoer.ApplyKnockback(rigidbodyCollision.Normal, 40f, false);
				}
			}
			else
			{
				KeyBulletManController component2 = component.GetComponent<KeyBulletManController>();
				if (component2)
				{
					component2.ForceHandleRewards();
				}
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(component, rigidbodyCollision.MyRigidbody));
				component.EraseFromExistenceWithRewards(false);
			}
		}
		else
		{
			MajorBreakable component3 = rigidbodyCollision.OtherRigidbody.GetComponent<MajorBreakable>();
			BodyPartController component4 = rigidbodyCollision.OtherRigidbody.GetComponent<BodyPartController>();
			if (component4 && component3)
			{
				flag = true;
				Vector2 normalized = (rigidbodyCollision.MyRigidbody.UnitCenter - rigidbodyCollision.OtherRigidbody.UnitCenter).normalized;
				component3.ApplyDamage(this.BossContactDamage / 2f, normalized * -1f, false, false, false);
				if (component3.healthHaver)
				{
					component3.healthHaver.ApplyDamage(this.BossContactDamage / 2f, normalized * -1f, "pakku pakku", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				}
				if (rigidbodyCollision.MyRigidbody && rigidbodyCollision.MyRigidbody.knockbackDoer)
				{
					rigidbodyCollision.MyRigidbody.knockbackDoer.ApplyKnockback(normalized.normalized, 40f, false);
				}
			}
		}
		if (flag)
		{
			rigidbodyCollision.MyRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 0.5f, null);
		}
	}

	// Token: 0x0600720A RID: 29194 RVA: 0x002D5128 File Offset: 0x002D3328
	private IEnumerator HandleEnemySuck(AIActor target, SpeculativeRigidbody ownerRigidbody)
	{
		if (!target || !ownerRigidbody)
		{
			yield break;
		}
		Transform copySprite = this.CreateEmptySprite(target);
		tk2dSprite copySpriteSprite = copySprite.GetComponentInChildren<tk2dSprite>();
		if (copySpriteSprite)
		{
			copySpriteSprite.HeightOffGround = -1.25f;
		}
		Vector3 startPosition = copySprite.transform.position;
		float elapsed = 0f;
		float duration = 0.25f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (ownerRigidbody && copySprite)
			{
				Vector3 vector = ownerRigidbody.UnitCenter;
				float num = elapsed / duration * (elapsed / duration);
				copySprite.position = Vector3.Lerp(startPosition, vector, num);
				copySprite.rotation = Quaternion.Euler(0f, 0f, 720f * BraveTime.DeltaTime) * copySprite.rotation;
				copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), num);
				if (copySpriteSprite)
				{
					copySpriteSprite.UpdateZDepth();
				}
			}
			yield return null;
		}
		if (copySprite)
		{
			UnityEngine.Object.Destroy(copySprite.gameObject);
		}
		yield break;
	}

	// Token: 0x0600720B RID: 29195 RVA: 0x002D5154 File Offset: 0x002D3354
	private Transform CreateEmptySprite(AIActor target)
	{
		GameObject gameObject = new GameObject("suck image");
		gameObject.layer = target.gameObject.layer;
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
		tk2dSprite.transform.position = target.sprite.transform.position;
		GameObject gameObject2 = new GameObject("image parent");
		gameObject2.transform.position = tk2dSprite.WorldCenter;
		tk2dSprite.transform.parent = gameObject2.transform;
		if (target.optionalPalette != null)
		{
			tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
		}
		return gameObject2.transform;
	}

	// Token: 0x0600720C RID: 29196 RVA: 0x002D5234 File Offset: 0x002D3434
	protected override void OnPreDrop(PlayerController user)
	{
		base.IsCurrentlyActive = false;
		base.OnPreDrop(user);
	}

	// Token: 0x0400736C RID: 29548
	public float duration = 10f;

	// Token: 0x0400736D RID: 29549
	public float BossContactDamage = 30f;

	// Token: 0x0400736E RID: 29550
	public GameObject TransformationVFX;
}
