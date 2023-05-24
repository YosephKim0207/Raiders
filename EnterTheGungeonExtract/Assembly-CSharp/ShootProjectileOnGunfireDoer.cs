using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020014A3 RID: 5283
public class ShootProjectileOnGunfireDoer : BraveBehaviour, SingleSpawnableGunPlacedObject
{
	// Token: 0x06007822 RID: 30754 RVA: 0x0030022C File Offset: 0x002FE42C
	public void Initialize(Gun sourceGun)
	{
		if (!sourceGun || !sourceGun.CurrentOwner || !(sourceGun.CurrentOwner is PlayerController))
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.inAnimation))
		{
			base.spriteAnimator.Play(this.inAnimation);
		}
		this.m_isActive = true;
		this.m_sourceGun = sourceGun;
		Gun sourceGun2 = this.m_sourceGun;
		sourceGun2.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(sourceGun2.PostProcessProjectile, new Action<Projectile>(this.HandleProjectileFired));
		this.m_room = base.transform.position.GetAbsoluteRoom();
		this.m_ownerPlayer = sourceGun.CurrentOwner as PlayerController;
	}

	// Token: 0x06007823 RID: 30755 RVA: 0x003002E4 File Offset: 0x002FE4E4
	private void Update()
	{
		if (this.m_isActive)
		{
			if (this.m_ownerPlayer.CurrentRoom != this.m_room)
			{
				this.Deactivate();
			}
			this.m_firedThisFrame = false;
		}
	}

	// Token: 0x06007824 RID: 30756 RVA: 0x00300314 File Offset: 0x002FE514
	public void Deactivate()
	{
		this.m_isActive = false;
		if (this.m_sourceGun)
		{
			Gun sourceGun = this.m_sourceGun;
			sourceGun.PostProcessProjectile = (Action<Projectile>)Delegate.Remove(sourceGun.PostProcessProjectile, new Action<Projectile>(this.HandleProjectileFired));
		}
		if (this)
		{
			if (!string.IsNullOrEmpty(this.inAnimation))
			{
				base.spriteAnimator.PlayAndDestroyObject(this.outAnimation, null);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06007825 RID: 30757 RVA: 0x0030039C File Offset: 0x002FE59C
	private void HandleProjectileFired(Projectile obj)
	{
		if (!this || !base.sprite)
		{
			this.Deactivate();
			return;
		}
		if (this.m_isActive)
		{
			if (!this.m_firedThisFrame)
			{
				this.m_sourceGun.muzzleFlashEffects.SpawnAtPosition(base.sprite.WorldCenter.ToVector3ZUp(0f), 0f, null, null, null, null, false, null, null, false);
			}
			this.m_firedThisFrame = true;
			if (!string.IsNullOrEmpty(this.inAnimation) && !base.spriteAnimator.IsPlaying(this.fireAnimation))
			{
				base.spriteAnimator.PlayForDuration(this.fireAnimation, -1f, this.idleAnimation, false);
			}
			if (this.HasOverrideSynergy && this.m_ownerPlayer.HasActiveBonusSynergy(this.OverrideSynergy, false))
			{
				Vector2 worldCenter = base.sprite.WorldCenter;
				float num = -1f;
				AIActor nearestEnemy = base.transform.position.GetAbsoluteRoom().GetNearestEnemy(worldCenter, out num, true, false);
				if (nearestEnemy && this.m_lastFiredFrame != Time.frameCount)
				{
					this.m_lastFiredFrame = Time.frameCount;
					VolleyUtility.FireVolley(this.OverrideSynergyVolley, worldCenter, nearestEnemy.CenterPosition - worldCenter, obj.Owner, false);
				}
			}
			else if (obj)
			{
				Vector3 vector = obj.transform.position - this.m_sourceGun.barrelOffset.position;
				GameObject gameObject = SpawnManager.SpawnProjectile(obj.gameObject, base.sprite.WorldCenter + vector.XY(), obj.transform.rotation, true);
				if (gameObject)
				{
					Projectile component = gameObject.GetComponent<Projectile>();
					component.Owner = obj.Owner;
					component.Shooter = obj.Shooter;
					component.PossibleSourceGun = obj.PossibleSourceGun;
					component.collidesWithPlayer = false;
					component.collidesWithEnemies = true;
				}
			}
		}
	}

	// Token: 0x04007A41 RID: 31297
	[CheckAnimation(null)]
	public string inAnimation;

	// Token: 0x04007A42 RID: 31298
	[CheckAnimation(null)]
	public string fireAnimation;

	// Token: 0x04007A43 RID: 31299
	[CheckAnimation(null)]
	public string idleAnimation;

	// Token: 0x04007A44 RID: 31300
	[CheckAnimation(null)]
	public string outAnimation;

	// Token: 0x04007A45 RID: 31301
	public bool HasOverrideSynergy;

	// Token: 0x04007A46 RID: 31302
	[LongNumericEnum]
	public CustomSynergyType OverrideSynergy;

	// Token: 0x04007A47 RID: 31303
	public ProjectileVolleyData OverrideSynergyVolley;

	// Token: 0x04007A48 RID: 31304
	private Gun m_sourceGun;

	// Token: 0x04007A49 RID: 31305
	private PlayerController m_ownerPlayer;

	// Token: 0x04007A4A RID: 31306
	private bool m_isActive;

	// Token: 0x04007A4B RID: 31307
	private RoomHandler m_room;

	// Token: 0x04007A4C RID: 31308
	private bool m_firedThisFrame;

	// Token: 0x04007A4D RID: 31309
	private int m_lastFiredFrame = -1;
}
