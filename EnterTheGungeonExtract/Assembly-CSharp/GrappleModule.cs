using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200140E RID: 5134
[Serializable]
public class GrappleModule
{
	// Token: 0x0600747F RID: 29823 RVA: 0x002E59B4 File Offset: 0x002E3BB4
	public void Trigger(PlayerController user)
	{
		this.m_lastUser = user;
		this.m_extantGrapple = UnityEngine.Object.Instantiate<GameObject>(this.GrapplePrefab);
		this.m_hasImpactedEnemy = false;
		this.m_hasImpactedTile = false;
		this.m_hasImpactedItem = false;
		this.m_hasImpactedShopItem = false;
		this.m_tileImpactFake = false;
		this.m_isDone = false;
		tk2dTiledSprite componentInChildren = this.m_extantGrapple.GetComponentInChildren<tk2dTiledSprite>();
		componentInChildren.dimensions = new Vector2(3f, componentInChildren.dimensions.y);
		this.m_extantGrapple.transform.position = user.CenterPosition.ToVector3ZUp(0f);
		this.m_lastCoroutine = user.StartCoroutine(this.HandleGrappleEffect(user));
	}

	// Token: 0x06007480 RID: 29824 RVA: 0x002E5A60 File Offset: 0x002E3C60
	public void MarkDone()
	{
		this.m_isDone = true;
	}

	// Token: 0x06007481 RID: 29825 RVA: 0x002E5A6C File Offset: 0x002E3C6C
	public void ForceEndGrapple()
	{
		if (!this.m_isDone)
		{
			if (this.m_lastUser != null)
			{
				this.m_lastUser.healthHaver.IsVulnerable = true;
				this.m_lastUser.SetIsFlying(false, "grapple", false, false);
				this.m_lastUser.CurrentInputState = PlayerInputState.AllInput;
			}
			this.m_isDone = true;
			this.m_lastUser = null;
			PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.PostMovementUpdate;
		}
	}

	// Token: 0x06007482 RID: 29826 RVA: 0x002E5AEC File Offset: 0x002E3CEC
	public void ForceEndGrappleImmediate()
	{
		if (!this.m_isDone)
		{
			if (this.m_lastUser != null)
			{
				if (this.m_lastCoroutine != null)
				{
					this.m_lastUser.StopCoroutine(this.m_lastCoroutine);
				}
				this.m_lastUser.healthHaver.IsVulnerable = true;
				this.m_lastUser.SetIsFlying(false, "grapple", false, false);
				this.m_lastUser.CurrentInputState = PlayerInputState.AllInput;
			}
			this.m_isDone = true;
			this.m_lastUser = null;
			PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.PostMovementUpdate;
			UnityEngine.Object.Destroy(this.m_extantGrapple);
			this.m_extantGrapple = null;
			if (this.FinishedCallback != null)
			{
				this.FinishedCallback();
			}
		}
	}

	// Token: 0x06007483 RID: 29827 RVA: 0x002E5BB0 File Offset: 0x002E3DB0
	public void ClearExtantGrapple()
	{
		if (this.m_extantGrapple != null)
		{
			UnityEngine.Object.Destroy(this.m_extantGrapple);
			this.m_extantGrapple = null;
		}
	}

	// Token: 0x06007484 RID: 29828 RVA: 0x002E5BD8 File Offset: 0x002E3DD8
	protected IEnumerator HandleGrappleEffect(PlayerController user)
	{
		SpeculativeRigidbody grappleRigidbody = this.m_extantGrapple.GetComponent<SpeculativeRigidbody>();
		grappleRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker));
		PhysicsEngine.Instance.OnPostRigidbodyMovement += this.PostMovementUpdate;
		Vector2 startPoint = user.CenterPosition;
		Vector2 aimDirection = user.unadjustedAimPoint.XY() - startPoint;
		grappleRigidbody.RegisterSpecificCollisionException(user.specRigidbody);
		grappleRigidbody.transform.position = startPoint.ToVector3ZUp(0f);
		grappleRigidbody.Velocity = aimDirection.normalized * this.GrappleSpeed;
		grappleRigidbody.Reinitialize();
		SpeculativeRigidbody speculativeRigidbody = grappleRigidbody;
		speculativeRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.ImpactedTile));
		SpeculativeRigidbody speculativeRigidbody2 = grappleRigidbody;
		speculativeRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.ImpactedRigidbody));
		SpeculativeRigidbody speculativeRigidbody3 = grappleRigidbody;
		speculativeRigidbody3.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		tk2dTiledSprite chainSprite = grappleRigidbody.GetComponentInChildren<tk2dTiledSprite>();
		chainSprite.dimensions = new Vector2(3f, chainSprite.dimensions.y);
		this.m_isDone = false;
		float totalDistanceToGrapple = -1f;
		float grappledDistance = 0f;
		while (!this.m_isDone)
		{
			if (this.m_extantGrapple == null)
			{
				yield break;
			}
			if (user && user.healthHaver && user.healthHaver.IsDead)
			{
				break;
			}
			Vector2 currentDirVec = grappleRigidbody.UnitCenter - user.CenterPosition;
			int pixelsWide = Mathf.RoundToInt(currentDirVec.magnitude / 0.0625f);
			chainSprite.dimensions = new Vector2((float)pixelsWide, chainSprite.dimensions.y);
			float currentChainSpriteAngle = BraveMathCollege.Atan2Degrees(currentDirVec);
			grappleRigidbody.transform.rotation = Quaternion.Euler(0f, 0f, currentChainSpriteAngle);
			IPlayerInteractable nearestIxable = user.CurrentRoom.GetNearestInteractable(grappleRigidbody.UnitCenter, 1f, user);
			if (nearestIxable is PickupObject && !(nearestIxable as PickupObject).IsBeingEyedByRat)
			{
				AkSoundEngine.PostEvent("Play_WPN_metalbullet_impact_01", this.sourceGameObject);
				grappleRigidbody.CollideWithOthers = false;
				this.m_hasImpactedItem = true;
				this.m_impactedItem = nearestIxable as PickupObject;
			}
			if (this.m_hasImpactedEnemy)
			{
				this.m_impactedEnemy.healthHaver.ApplyDamage(this.DamageToEnemies, currentDirVec.normalized, "Grapple", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				if (this.m_impactedEnemy.knockbackDoer)
				{
					this.m_impactedEnemy.knockbackDoer.ApplyKnockback(currentDirVec.normalized, this.EnemyKnockbackForce, false);
				}
				if (this.m_impactedEnemy.behaviorSpeculator)
				{
					this.m_impactedEnemy.behaviorSpeculator.Stun(3f, true);
				}
				this.m_isDone = true;
			}
			else if (this.m_hasImpactedItem)
			{
				AkSoundEngine.PostEvent("Play_WPN_metalbullet_impact_01", this.sourceGameObject);
				if (totalDistanceToGrapple == -1f)
				{
					totalDistanceToGrapple = currentDirVec.magnitude;
				}
				grappledDistance += this.GrappleSpeed * BraveTime.DeltaTime;
				grappleRigidbody.Velocity = (user.CenterPosition - grappleRigidbody.UnitCenter).normalized * this.GrappleSpeed;
				if (this.m_impactedItem.specRigidbody != null)
				{
					this.m_impactedItem.specRigidbody.Velocity = grappleRigidbody.Velocity;
				}
				else
				{
					this.m_impactedItem.sprite.PlaceAtPositionByAnchor(grappleRigidbody.UnitCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
				if (grappledDistance >= totalDistanceToGrapple || Vector2.Distance(user.specRigidbody.UnitCenter, grappleRigidbody.UnitCenter) < 0.5f)
				{
					if (this.m_impactedItem && this.m_impactedItem.specRigidbody)
					{
						this.m_impactedItem.specRigidbody.Velocity = Vector2.zero;
					}
					this.m_isDone = true;
				}
			}
			else if (this.m_hasImpactedShopItem)
			{
				if (totalDistanceToGrapple == -1f)
				{
					totalDistanceToGrapple = currentDirVec.magnitude;
				}
				grappledDistance += this.GrappleSpeed * BraveTime.DeltaTime;
				grappleRigidbody.Velocity = (user.CenterPosition - grappleRigidbody.UnitCenter).normalized * this.GrappleSpeed;
				if (this.m_impactedShopItem)
				{
					this.m_impactedShopItem.sprite.PlaceAtPositionByAnchor(grappleRigidbody.UnitCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
				if (grappledDistance >= totalDistanceToGrapple || Vector2.Distance(user.specRigidbody.UnitCenter, grappleRigidbody.UnitCenter) < 0.5f)
				{
					if (this.m_impactedItem && this.m_impactedItem.specRigidbody)
					{
						this.m_impactedItem.specRigidbody.Velocity = Vector2.zero;
					}
					this.m_isDone = true;
					if (this.m_impactedShopItem)
					{
						this.m_impactedShopItem.ForceSteal(this.m_lastUser);
					}
				}
			}
			else if (this.m_hasImpactedTile)
			{
				AkSoundEngine.PostEvent("Play_OBJ_hook_pull_01", this.sourceGameObject);
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON && user && user.CurrentRoom != null && user.CurrentRoom.AdditionalRoomState == RoomHandler.CustomRoomState.LICH_PHASE_THREE)
				{
					this.m_tileImpactFake = true;
				}
				if (this.m_tileImpactFake)
				{
					this.m_isDone = true;
				}
				else
				{
					if (totalDistanceToGrapple == -1f)
					{
						if (user.HasActiveBonusSynergy(CustomSynergyType.NINJA_TOOLS, false))
						{
							user.PostProcessProjectile += this.HandleNinjaToolsSynergy;
						}
						totalDistanceToGrapple = currentDirVec.magnitude;
					}
					user.healthHaver.IsVulnerable = false;
					user.CurrentInputState = PlayerInputState.NoMovement;
					user.SetIsFlying(true, "grapple", false, false);
					user.specRigidbody.Velocity = currentDirVec.normalized * this.GrappleRetractSpeed;
					grappledDistance += this.GrappleRetractSpeed * BraveTime.DeltaTime;
					if (grappledDistance >= totalDistanceToGrapple || Vector2.Distance(user.specRigidbody.UnitCenter, grappleRigidbody.UnitCenter) < 1.5f)
					{
						this.m_isDone = true;
						user.PostProcessProjectile -= this.HandleNinjaToolsSynergy;
					}
				}
			}
			yield return null;
		}
		if (user)
		{
			user.PostProcessProjectile -= this.HandleNinjaToolsSynergy;
		}
		this.m_lastUser = null;
		PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.PostMovementUpdate;
		UnityEngine.Object.Destroy(this.m_extantGrapple);
		this.m_extantGrapple = null;
		if (this.FinishedCallback != null)
		{
			this.FinishedCallback();
		}
		user.healthHaver.IsVulnerable = true;
		user.SetIsFlying(false, "grapple", false, false);
		user.CurrentInputState = PlayerInputState.AllInput;
		yield break;
	}

	// Token: 0x06007485 RID: 29829 RVA: 0x002E5BFC File Offset: 0x002E3DFC
	private void HandleNinjaToolsSynergy(Projectile sourceProjectile, float beamEffectPercentage)
	{
		HomingModifier homingModifier = sourceProjectile.GetComponent<HomingModifier>();
		if (homingModifier == null)
		{
			homingModifier = sourceProjectile.gameObject.AddComponent<HomingModifier>();
			homingModifier.HomingRadius = 0f;
			homingModifier.AngularVelocity = 0f;
		}
		homingModifier.HomingRadius = Mathf.Max(12f, homingModifier.HomingRadius);
		homingModifier.AngularVelocity = Mathf.Max(720f, homingModifier.HomingRadius);
	}

	// Token: 0x06007486 RID: 29830 RVA: 0x002E5C6C File Offset: 0x002E3E6C
	protected virtual void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (this.m_hasImpactedItem || this.m_hasImpactedEnemy)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.GetComponent<PlayerController>() != null)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.GetComponent<MinorBreakable>() != null)
		{
			if (!otherRigidbody.GetComponent<MinorBreakable>().IsBroken)
			{
				otherRigidbody.GetComponent<MinorBreakable>().Break();
			}
			PhysicsEngine.SkipCollision = true;
			return;
		}
	}

	// Token: 0x06007487 RID: 29831 RVA: 0x002E5CE4 File Offset: 0x002E3EE4
	private void ImpactedRigidbody(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.aiActor)
		{
			this.m_impactedEnemy = rigidbodyCollision.OtherRigidbody.aiActor;
			this.m_hasImpactedEnemy = true;
			rigidbodyCollision.MyRigidbody.Velocity = Vector2.zero;
			return;
		}
		ShopItemController component = rigidbodyCollision.OtherRigidbody.GetComponent<ShopItemController>();
		if (component)
		{
			AkSoundEngine.PostEvent("Play_WPN_metalbullet_impact_01", this.sourceGameObject);
			this.m_impactedShopItem = (component.Locked ? null : component);
			this.m_hasImpactedShopItem = true;
			component.specRigidbody.enabled = false;
			rigidbodyCollision.MyRigidbody.Velocity = Vector2.zero;
			return;
		}
		this.m_hasImpactedTile = true;
		rigidbodyCollision.MyRigidbody.Velocity = Vector2.zero;
	}

	// Token: 0x06007488 RID: 29832 RVA: 0x002E5DAC File Offset: 0x002E3FAC
	private void ImpactedTile(CollisionData tileCollision)
	{
		this.m_hasImpactedTile = true;
		tileCollision.MyRigidbody.Velocity = Vector2.zero;
	}

	// Token: 0x06007489 RID: 29833 RVA: 0x002E5DC8 File Offset: 0x002E3FC8
	private void PostMovementUpdate()
	{
		if (this.m_lastUser && this.m_extantGrapple)
		{
			SpeculativeRigidbody component = this.m_extantGrapple.GetComponent<SpeculativeRigidbody>();
			tk2dTiledSprite componentInChildren = component.GetComponentInChildren<tk2dTiledSprite>();
			Vector2 vector = component.UnitCenter - this.m_lastUser.CenterPosition;
			int num = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			componentInChildren.dimensions = new Vector2((float)num, componentInChildren.dimensions.y);
			float num2 = BraveMathCollege.Atan2Degrees(vector);
			component.transform.rotation = Quaternion.Euler(0f, 0f, num2);
		}
	}

	// Token: 0x04007647 RID: 30279
	public GameObject GrapplePrefab;

	// Token: 0x04007648 RID: 30280
	public float GrappleSpeed = 10f;

	// Token: 0x04007649 RID: 30281
	public float GrappleRetractSpeed = 10f;

	// Token: 0x0400764A RID: 30282
	public float DamageToEnemies = 10f;

	// Token: 0x0400764B RID: 30283
	public float EnemyKnockbackForce = 10f;

	// Token: 0x0400764C RID: 30284
	public GameObject sourceGameObject;

	// Token: 0x0400764D RID: 30285
	public Action FinishedCallback;

	// Token: 0x0400764E RID: 30286
	private GameObject m_extantGrapple;

	// Token: 0x0400764F RID: 30287
	private bool m_hasImpactedTile;

	// Token: 0x04007650 RID: 30288
	private bool m_hasImpactedEnemy;

	// Token: 0x04007651 RID: 30289
	private bool m_hasImpactedShopItem;

	// Token: 0x04007652 RID: 30290
	private bool m_hasImpactedItem;

	// Token: 0x04007653 RID: 30291
	private bool m_tileImpactFake;

	// Token: 0x04007654 RID: 30292
	private AIActor m_impactedEnemy;

	// Token: 0x04007655 RID: 30293
	private PickupObject m_impactedItem;

	// Token: 0x04007656 RID: 30294
	private ShopItemController m_impactedShopItem;

	// Token: 0x04007657 RID: 30295
	private bool m_isDone;

	// Token: 0x04007658 RID: 30296
	private PlayerController m_lastUser;

	// Token: 0x04007659 RID: 30297
	private Coroutine m_lastCoroutine;
}
