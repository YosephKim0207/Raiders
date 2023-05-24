using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013D9 RID: 5081
public class PunchoutGunController : MonoBehaviour
{
	// Token: 0x0600734B RID: 29515 RVA: 0x002DDEA0 File Offset: 0x002DC0A0
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Combine(gun.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HandlePrefireModifier));
		Gun gun2 = this.m_gun;
		gun2.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun2.PostProcessProjectile, new Action<Projectile>(this.HandleProjectileFired));
		Gun gun3 = this.m_gun;
		gun3.OnDropped = (Action)Delegate.Combine(gun3.OnDropped, new Action(this.HandleDropped));
		this.CachedFireAnimation = this.m_gun.shootAnimation;
		this.CachedChargeAnimation = this.m_gun.chargeAnimation;
	}

	// Token: 0x0600734C RID: 29516 RVA: 0x002DDF50 File Offset: 0x002DC150
	private void Update()
	{
		if (!this.m_cachedPlayer && this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			this.m_cachedPlayer = this.m_gun.CurrentOwner as PlayerController;
			this.m_cachedPlayer.OnReceivedDamage += this.HandleWasDamaged;
		}
		else if (this.m_cachedPlayer && this.m_gun && !this.m_gun.CurrentOwner)
		{
			this.m_cachedPlayer.OnReceivedDamage -= this.HandleWasDamaged;
			this.m_cachedPlayer = null;
		}
	}

	// Token: 0x0600734D RID: 29517 RVA: 0x002DE018 File Offset: 0x002DC218
	private void HandleWasDamaged(PlayerController obj)
	{
		if (!base.enabled)
		{
			return;
		}
		this.RemoveAllStars();
	}

	// Token: 0x0600734E RID: 29518 RVA: 0x002DE02C File Offset: 0x002DC22C
	private void HandleDropped()
	{
		this.RemoveAllStars();
	}

	// Token: 0x0600734F RID: 29519 RVA: 0x002DE034 File Offset: 0x002DC234
	public void OnDisable()
	{
		this.RemoveAllStars();
	}

	// Token: 0x06007350 RID: 29520 RVA: 0x002DE03C File Offset: 0x002DC23C
	public void OnDestroy()
	{
		this.RemoveAllStars();
		if (this.m_cachedPlayer)
		{
			this.m_cachedPlayer.OnReceivedDamage -= this.HandleWasDamaged;
			this.m_cachedPlayer = null;
		}
	}

	// Token: 0x06007351 RID: 29521 RVA: 0x002DE074 File Offset: 0x002DC274
	private Projectile HandlePrefireModifier(Gun sourceGun, Projectile sourceProjectile, ProjectileModule sourceModule)
	{
		bool flag = sourceProjectile == this.OverrideProjectile;
		if (this.m_extantStars.Count >= 3 && flag && this.m_cachedPlayer)
		{
			this.RemoveAllStars();
			if (this.m_gun.spriteAnimator)
			{
				this.m_gun.OverrideAnimations = true;
				this.m_gun.spriteAnimator.Play(this.OverrideFireAnimation);
			}
			return sourceProjectile;
		}
		return this.BaseProjectile;
	}

	// Token: 0x06007352 RID: 29522 RVA: 0x002DE0FC File Offset: 0x002DC2FC
	private void HandleProjectileFired(Projectile spawnedProjectile)
	{
		spawnedProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(spawnedProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleEnemyHit));
		this.m_gun.OverrideAnimations = false;
		if (this.m_extantStars.Count < 3 && this.m_gun.chargeAnimation != this.CachedChargeAnimation)
		{
			this.m_gun.shootAnimation = this.CachedFireAnimation;
			this.m_gun.chargeAnimation = this.CachedChargeAnimation;
		}
	}

	// Token: 0x06007353 RID: 29523 RVA: 0x002DE188 File Offset: 0x002DC388
	private void RemoveAllStars()
	{
		if (this.m_cachedPlayer && GameUIRoot.HasInstance)
		{
			GameUIAmmoController ammoControllerForPlayerID = GameUIRoot.Instance.GetAmmoControllerForPlayerID(this.m_cachedPlayer.PlayerIDX);
			for (int i = this.m_extantStars.Count - 1; i >= 0; i--)
			{
				ammoControllerForPlayerID.DeregisterAdditionalSprite(this.m_extantStars[i]);
			}
		}
		this.m_extantStars.Clear();
	}

	// Token: 0x06007354 RID: 29524 RVA: 0x002DE200 File Offset: 0x002DC400
	private void HandleEnemyHit(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
	{
		if (this.m_gun.CurrentOwner is PlayerController && fatal && this.m_extantStars.Count < 3)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			GameUIAmmoController ammoControllerForPlayerID = GameUIRoot.Instance.GetAmmoControllerForPlayerID(playerController.PlayerIDX);
			dfSprite dfSprite = ammoControllerForPlayerID.RegisterNewAdditionalSprite(this.UIStarSpriteName);
			this.m_extantStars.Add(dfSprite);
			if (this.m_extantStars.Count >= 3)
			{
				this.m_gun.chargeAnimation = this.OverrideChargeAnimation;
			}
		}
	}

	// Token: 0x040074D5 RID: 29909
	public string UIStarSpriteName;

	// Token: 0x040074D6 RID: 29910
	public Projectile BaseProjectile;

	// Token: 0x040074D7 RID: 29911
	public Projectile OverrideProjectile;

	// Token: 0x040074D8 RID: 29912
	public float ChargeTimeNormal;

	// Token: 0x040074D9 RID: 29913
	public float ChargeTimeStar = 0.5f;

	// Token: 0x040074DA RID: 29914
	[CheckAnimation(null)]
	public string OverrideFireAnimation;

	// Token: 0x040074DB RID: 29915
	[CheckAnimation(null)]
	public string OverrideChargeAnimation;

	// Token: 0x040074DC RID: 29916
	private string CachedFireAnimation;

	// Token: 0x040074DD RID: 29917
	private string CachedChargeAnimation;

	// Token: 0x040074DE RID: 29918
	private Gun m_gun;

	// Token: 0x040074DF RID: 29919
	private List<dfSprite> m_extantStars = new List<dfSprite>();

	// Token: 0x040074E0 RID: 29920
	private PlayerController m_cachedPlayer;
}
