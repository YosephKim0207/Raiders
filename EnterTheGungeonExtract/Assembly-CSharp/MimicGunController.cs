using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013C9 RID: 5065
public class MimicGunController : MonoBehaviour
{
	// Token: 0x060072DB RID: 29403 RVA: 0x002DA640 File Offset: 0x002D8840
	public void Initialize(PlayerController p, Gun sourceGun)
	{
		this.m_initialized = true;
		this.m_gun = base.GetComponent<Gun>();
		p.inventory.GunLocked.AddOverride("mimic gun", null);
		p.OnDealtDamage += this.HandleDealtDamage;
		Gun gun = this.m_gun;
		gun.OnAmmoChanged = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnAmmoChanged, new Action<PlayerController, Gun>(this.HandleAmmoChanged));
		this.m_sourceGun = sourceGun;
		if (!string.IsNullOrEmpty(this.AcquisitionAudioEvent))
		{
			AkSoundEngine.PostEvent(this.AcquisitionAudioEvent, base.gameObject);
		}
		if (this.BecomeMimicVFX)
		{
			SpawnManager.SpawnVFX(this.BecomeMimicVFX, this.m_gun.GetSprite().WorldCenter, Quaternion.identity);
		}
		this.m_gun.OverrideAnimations = true;
		base.StartCoroutine(this.HandleDeferredAnimationOverride(1f));
		this.m_gun.spriteAnimator.PlayForDuration("mimic_gun_intro", 1f, this.m_gun.idleAnimation, false);
	}

	// Token: 0x060072DC RID: 29404 RVA: 0x002DA75C File Offset: 0x002D895C
	private void Update()
	{
		if (this.m_isClearing)
		{
			return;
		}
		if (this.m_gun && this.m_gun.ammo <= 0)
		{
			if (!string.IsNullOrEmpty(this.RefillingAmmoAudioEvent))
			{
				AkSoundEngine.PostEvent(this.RefillingAmmoAudioEvent, base.gameObject);
			}
			this.m_gun.OverrideAnimations = true;
			base.StartCoroutine(this.HandleDeferredAnimationOverride(3f));
			this.m_gun.spriteAnimator.PlayForDuration("mimic_gun_laugh", 3f, this.m_gun.idleAnimation, false);
			this.m_selfRefilling = true;
			this.m_gun.GainAmmo(this.m_gun.AdjustedMaxAmmo);
			this.m_selfRefilling = false;
		}
	}

	// Token: 0x060072DD RID: 29405 RVA: 0x002DA820 File Offset: 0x002D8A20
	public void OnDestroy()
	{
		if (!this.m_gun)
		{
			return;
		}
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			playerController.OnDealtDamage -= this.HandleDealtDamage;
			playerController.inventory.GunLocked.RemoveOverride("mimic gun");
		}
	}

	// Token: 0x060072DE RID: 29406 RVA: 0x002DA884 File Offset: 0x002D8A84
	private IEnumerator HandleDeferredAnimationOverride(float t)
	{
		yield return new WaitForSeconds(t);
		if (this.m_gun)
		{
			this.m_gun.OverrideAnimations = false;
		}
		yield break;
	}

	// Token: 0x060072DF RID: 29407 RVA: 0x002DA8A8 File Offset: 0x002D8AA8
	private void HandleDealtDamage(PlayerController source, float dmg)
	{
		if (this.m_isClearing)
		{
			return;
		}
		this.m_damageDealt += dmg;
		if (this.m_damageDealt >= this.DamageRequired)
		{
			this.ClearMimic();
		}
	}

	// Token: 0x060072E0 RID: 29408 RVA: 0x002DA8DC File Offset: 0x002D8ADC
	private void HandleAmmoChanged(PlayerController sourcePlayer, Gun sourceGun)
	{
		if (this.m_isClearing)
		{
			return;
		}
		if (sourceGun == this.m_gun && !this.m_selfRefilling && sourceGun.ammo >= sourceGun.AdjustedMaxAmmo)
		{
			this.ForceClearMimic(false);
			return;
		}
	}

	// Token: 0x060072E1 RID: 29409 RVA: 0x002DA92C File Offset: 0x002D8B2C
	public void ForceClearMimic(bool instant = false)
	{
		if (this.m_isClearing)
		{
			return;
		}
		this.m_damageDealt = 10000f;
		if (instant)
		{
			if (this.m_gun && this.m_gun.CurrentOwner)
			{
				if (this.UnbecomeMimicVfx)
				{
					SpawnManager.SpawnVFX(this.UnbecomeMimicVfx, this.m_gun.GetSprite().WorldCenter, Quaternion.identity);
				}
				PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
				playerController.OnDealtDamage -= this.HandleDealtDamage;
				playerController.inventory.GunLocked.RemoveOverride("mimic gun");
				playerController.inventory.DestroyGun(this.m_gun);
				playerController.ChangeToGunSlot(playerController.inventory.AllGuns.IndexOf(this.m_sourceGun), true);
			}
		}
		else
		{
			this.ClearMimic();
		}
	}

	// Token: 0x060072E2 RID: 29410 RVA: 0x002DAA24 File Offset: 0x002D8C24
	private IEnumerator HandleClearMimic()
	{
		if (this.m_isClearing)
		{
			yield break;
		}
		this.m_isClearing = true;
		this.m_gun.OverrideAnimations = true;
		this.m_gun.spriteAnimator.Play("mimic_gun_outro");
		while (this.m_gun.spriteAnimator.IsPlaying("mimic_gun_outro"))
		{
			yield return null;
		}
		if (this.m_gun && this.m_gun.CurrentOwner)
		{
			if (this.UnbecomeMimicVfx)
			{
				SpawnManager.SpawnVFX(this.UnbecomeMimicVfx, this.m_gun.GetSprite().WorldCenter, Quaternion.identity);
			}
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			playerController.OnDealtDamage -= this.HandleDealtDamage;
			playerController.inventory.GunLocked.RemoveOverride("mimic gun");
			playerController.inventory.DestroyGun(this.m_gun);
			playerController.ChangeToGunSlot(playerController.inventory.AllGuns.IndexOf(this.m_sourceGun), true);
		}
		yield break;
	}

	// Token: 0x060072E3 RID: 29411 RVA: 0x002DAA40 File Offset: 0x002D8C40
	private void ClearMimic()
	{
		if (this.m_isClearing)
		{
			return;
		}
		base.StartCoroutine(this.HandleClearMimic());
	}

	// Token: 0x0400742E RID: 29742
	public float DamageRequired = 300f;

	// Token: 0x0400742F RID: 29743
	public GameObject BecomeMimicVFX;

	// Token: 0x04007430 RID: 29744
	public GameObject UnbecomeMimicVfx;

	// Token: 0x04007431 RID: 29745
	[Header("Audio")]
	public string AcquisitionAudioEvent;

	// Token: 0x04007432 RID: 29746
	public string RefillingAmmoAudioEvent;

	// Token: 0x04007433 RID: 29747
	private Gun m_gun;

	// Token: 0x04007434 RID: 29748
	private bool m_initialized;

	// Token: 0x04007435 RID: 29749
	private float m_damageDealt;

	// Token: 0x04007436 RID: 29750
	private Gun m_sourceGun;

	// Token: 0x04007437 RID: 29751
	private bool m_selfRefilling;

	// Token: 0x04007438 RID: 29752
	private bool m_isClearing;
}
