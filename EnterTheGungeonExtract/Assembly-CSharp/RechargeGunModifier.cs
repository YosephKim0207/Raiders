using System;
using UnityEngine;

// Token: 0x0200166B RID: 5739
public class RechargeGunModifier : MonoBehaviour
{
	// Token: 0x060085D9 RID: 34265 RVA: 0x00373E14 File Offset: 0x00372014
	public void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Combine(gun.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HandleReplaceProjectile));
		Gun gun2 = this.m_gun;
		gun2.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun2.PostProcessProjectile, new Action<Projectile>(this.PostProcessProjectile));
		Gun gun3 = this.m_gun;
		gun3.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun3.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
	}

	// Token: 0x060085DA RID: 34266 RVA: 0x00373EA4 File Offset: 0x003720A4
	private Projectile HandleReplaceProjectile(Gun arg1, Projectile arg2, ProjectileModule arg3)
	{
		Projectile projectile = arg2;
		int num = 0;
		for (int i = 0; i < this.Projectiles.Length; i++)
		{
			if (this.m_counter > this.Projectiles[i].RequiredStacks && num <= this.Projectiles[i].RequiredStacks)
			{
				num = this.Projectiles[i].RequiredStacks;
				projectile = this.Projectiles[i].ReplacementProjectile;
			}
		}
		return projectile;
	}

	// Token: 0x060085DB RID: 34267 RVA: 0x00373F18 File Offset: 0x00372118
	private void PostProcessProjectile(Projectile obj)
	{
		int num = Mathf.Min(this.StackFalloffPoint, this.m_counter);
		int num2 = Mathf.Max(0, this.m_counter - this.StackFalloffPoint);
		float num3 = 1f + this.MaxDamageMultiplierPerStack * (float)num + this.MinDamageMultiplierPerStack * (float)num2;
		num3 = Mathf.Min(this.MultiplierCap, num3);
		obj.baseData.damage *= num3;
	}

	// Token: 0x060085DC RID: 34268 RVA: 0x00373F88 File Offset: 0x00372188
	private void Update()
	{
		if (!this.m_callbackInitialized && this.m_gun.CurrentOwner is PlayerController)
		{
			this.m_callbackInitialized = true;
			this.m_cachedPlayer = this.m_gun.CurrentOwner as PlayerController;
			this.m_cachedPlayer.OnTriedToInitiateAttack += this.HandleTriedToInitiateAttack;
		}
		else if (this.m_callbackInitialized && !(this.m_gun.CurrentOwner is PlayerController))
		{
			this.m_callbackInitialized = false;
			if (this.m_cachedPlayer)
			{
				this.m_cachedPlayer.OnTriedToInitiateAttack -= this.HandleTriedToInitiateAttack;
				this.m_cachedPlayer = null;
			}
		}
		if (this.m_wasReloading && this.m_gun && !this.m_gun.IsReloading)
		{
			this.m_wasReloading = false;
		}
	}

	// Token: 0x060085DD RID: 34269 RVA: 0x00374078 File Offset: 0x00372278
	private void HandleTriedToInitiateAttack(PlayerController sourcePlayer)
	{
	}

	// Token: 0x060085DE RID: 34270 RVA: 0x0037407C File Offset: 0x0037227C
	private void HandleReloadPressed(PlayerController ownerPlayer, Gun sourceGun, bool something)
	{
		if (sourceGun.IsReloading)
		{
			if (!this.m_wasReloading)
			{
				this.m_counter = 0;
				this.m_wasReloading = true;
			}
			this.m_counter++;
			AkSoundEngine.PostEvent("Play_WPN_RechargeGun_Recharge_01", base.gameObject);
		}
		else
		{
			this.m_wasReloading = false;
		}
	}

	// Token: 0x04008A52 RID: 35410
	public float MaxDamageMultiplierPerStack = 0.1f;

	// Token: 0x04008A53 RID: 35411
	public float MinDamageMultiplierPerStack = 0.05f;

	// Token: 0x04008A54 RID: 35412
	public float MultiplierCap = 4f;

	// Token: 0x04008A55 RID: 35413
	public int StackFalloffPoint = 5;

	// Token: 0x04008A56 RID: 35414
	public RechargeGunProjectileTier[] Projectiles;

	// Token: 0x04008A57 RID: 35415
	private Gun m_gun;

	// Token: 0x04008A58 RID: 35416
	private bool m_callbackInitialized;

	// Token: 0x04008A59 RID: 35417
	private PlayerController m_cachedPlayer;

	// Token: 0x04008A5A RID: 35418
	private int m_counter = 1;

	// Token: 0x04008A5B RID: 35419
	private bool m_wasReloading;
}
