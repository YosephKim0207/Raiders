using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001467 RID: 5223
public class PortableTurretController : MonoBehaviour
{
	// Token: 0x060076C3 RID: 30403 RVA: 0x002F5968 File Offset: 0x002F3B68
	private void Awake()
	{
		this.actor = base.GetComponent<AIActor>();
		this.actor.PreventFallingInPitsEver = true;
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x002F5984 File Offset: 0x002F3B84
	private void Start()
	{
		this.actor.CanTargetEnemies = true;
		this.actor.CanTargetPlayers = false;
		this.actor.ParentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		this.actor.HasBeenEngaged = true;
		RoomHandler parentRoom = this.actor.ParentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.HandleRoomCleared));
		AIShooter aiShooter = this.actor.aiShooter;
		aiShooter.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(aiShooter.PostProcessProjectile, new Action<Projectile>(this.PostProcessProjectile));
		base.GetComponent<tk2dSpriteAnimator>().QueueAnimation("portable_turret_fire");
		this.actor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
		this.actor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
		base.StartCoroutine(this.HandleTimedDestroy());
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x002F5A88 File Offset: 0x002F3C88
	private void Update()
	{
		if (this.actor && this.actor.IsFalling)
		{
			base.GetComponent<tk2dSpriteAnimator>().Play("portable_turret_undeploy");
			tk2dSpriteAnimator component = base.GetComponent<tk2dSpriteAnimator>();
			component.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(component.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.Disappear));
		}
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x002F5AEC File Offset: 0x002F3CEC
	private void PostProcessProjectile(Projectile obj)
	{
		if (this.sourcePlayer)
		{
			this.sourcePlayer.DoPostProcessProjectile(obj);
			if (this.sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.TURRET_RANDOMIZER, false))
			{
				if (this.m_fallbackProjectile == null)
				{
					this.m_fallbackProjectile = this.actor.bulletBank.Bullets[0].BulletObject;
				}
				this.actor.bulletBank.Bullets[0].BulletObject = ProjectileRandomizerItem.GetRandomizerProjectileFromPlayer(this.sourcePlayer, this.m_fallbackProjectile.GetComponent<Projectile>(), 800).gameObject;
			}
			if (this.sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.CAPTAINPLANTIT, false))
			{
			}
		}
	}

	// Token: 0x060076C7 RID: 30407 RVA: 0x002F5BAC File Offset: 0x002F3DAC
	public void NotifyDropped()
	{
		this.HandleRoomCleared();
	}

	// Token: 0x060076C8 RID: 30408 RVA: 0x002F5BB4 File Offset: 0x002F3DB4
	private IEnumerator HandleTimedDestroy()
	{
		yield return new WaitForSeconds(this.maxDuration);
		base.GetComponent<tk2dSpriteAnimator>().Play("portable_turret_undeploy");
		tk2dSpriteAnimator component = base.GetComponent<tk2dSpriteAnimator>();
		component.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(component.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.Disappear));
		yield break;
	}

	// Token: 0x060076C9 RID: 30409 RVA: 0x002F5BD0 File Offset: 0x002F3DD0
	private void HandleRoomCleared()
	{
		if (this.actor)
		{
			base.GetComponent<tk2dSpriteAnimator>().Play("portable_turret_undeploy");
			tk2dSpriteAnimator component = base.GetComponent<tk2dSpriteAnimator>();
			component.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(component.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.Disappear));
		}
	}

	// Token: 0x060076CA RID: 30410 RVA: 0x002F5C24 File Offset: 0x002F3E24
	private void Disappear(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040078C0 RID: 30912
	[NonSerialized]
	public PlayerController sourcePlayer;

	// Token: 0x040078C1 RID: 30913
	public float maxDuration = 20f;

	// Token: 0x040078C2 RID: 30914
	private AIActor actor;

	// Token: 0x040078C3 RID: 30915
	private GameObject m_fallbackProjectile;
}
