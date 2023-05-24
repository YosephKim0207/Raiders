using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020013C6 RID: 5062
public class MagazineRack : MonoBehaviour
{
	// Token: 0x060072CB RID: 29387 RVA: 0x002DA0EC File Offset: 0x002D82EC
	public IEnumerator Start()
	{
		this.HandleRadialIndicator();
		yield return new WaitForSeconds(this.Duration);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060072CC RID: 29388 RVA: 0x002DA108 File Offset: 0x002D8308
	private void Update()
	{
		if (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			float num = this.Radius;
			if (playerController.HasActiveBonusSynergy(CustomSynergyType.MAGAZINE_CLIPS, false))
			{
				num *= 2f;
			}
			if (playerController && Vector2.Distance(playerController.CenterPosition, base.transform.position.XY()) < num)
			{
				if (i == 0 && playerController.CurrentGun)
				{
					this.m_p1MaxGunAmmoThisFrame = playerController.CurrentGun.CurrentAmmo;
					this.m_p1GunIDThisFrame = playerController.CurrentGun.PickupObjectId;
				}
				if (i == 1 && playerController.CurrentGun)
				{
					this.m_p2MaxGunAmmoThisFrame = playerController.CurrentGun.CurrentAmmo;
					this.m_p2GunIDThisFrame = playerController.CurrentGun.PickupObjectId;
				}
				playerController.InfiniteAmmo.SetOverride("MagazineRack", true, null);
				playerController.OnlyFinalProjectiles.SetOverride("MagazineRack", playerController.HasActiveBonusSynergy(CustomSynergyType.JUNK_MAIL, false), null);
			}
			else if (playerController)
			{
				playerController.InfiniteAmmo.SetOverride("MagazineRack", false, null);
				playerController.OnlyFinalProjectiles.SetOverride("MagazineRack", false, null);
			}
		}
	}

	// Token: 0x060072CD RID: 29389 RVA: 0x002DA29C File Offset: 0x002D849C
	private void LateUpdate()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && playerController.CurrentGun && playerController.InfiniteAmmo.HasOverride("MagazineRack"))
			{
				int num = ((i != 0) ? this.m_p2MaxGunAmmoThisFrame : this.m_p1MaxGunAmmoThisFrame);
				int num2 = ((i != 0) ? this.m_p2GunIDThisFrame : this.m_p1GunIDThisFrame);
				if (!playerController.CurrentGun.RequiresFundsToShoot && playerController.CurrentGun.CurrentAmmo < num && playerController.CurrentGun.PickupObjectId == num2)
				{
					playerController.CurrentGun.ammo = Mathf.Min(playerController.CurrentGun.AdjustedMaxAmmo, num);
				}
			}
			if (i == 0 && playerController.CurrentGun)
			{
				this.m_p1MaxGunAmmoThisFrame = playerController.CurrentGun.CurrentAmmo;
			}
			if (i == 1 && playerController.CurrentGun)
			{
				this.m_p2MaxGunAmmoThisFrame = playerController.CurrentGun.CurrentAmmo;
			}
		}
	}

	// Token: 0x060072CE RID: 29390 RVA: 0x002DA3D0 File Offset: 0x002D85D0
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController)
			{
				playerController.InfiniteAmmo.SetOverride("MagazineRack", false, null);
				playerController.OnlyFinalProjectiles.SetOverride("MagazineRack", false, null);
			}
		}
		this.UnhandleRadialIndicator();
	}

	// Token: 0x060072CF RID: 29391 RVA: 0x002DA44C File Offset: 0x002D864C
	private void HandleRadialIndicator()
	{
		if (!this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = true;
			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<HeatIndicatorController>();
			Debug.LogError("setting color and fire");
			this.m_radialIndicator.CurrentColor = Color.white;
			this.m_radialIndicator.IsFire = false;
			float num = this.Radius;
			int num2 = -1;
			if (PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.MAGAZINE_CLIPS, out num2))
			{
				num *= 2f;
			}
			this.m_radialIndicator.CurrentRadius = num;
		}
	}

	// Token: 0x060072D0 RID: 29392 RVA: 0x002DA4F4 File Offset: 0x002D86F4
	private void UnhandleRadialIndicator()
	{
		if (this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = false;
			if (this.m_radialIndicator)
			{
				this.m_radialIndicator.EndEffect();
			}
			this.m_radialIndicator = null;
		}
	}

	// Token: 0x04007420 RID: 29728
	public float Duration = 10f;

	// Token: 0x04007421 RID: 29729
	public float Radius = 5f;

	// Token: 0x04007422 RID: 29730
	private bool m_radialIndicatorActive;

	// Token: 0x04007423 RID: 29731
	private HeatIndicatorController m_radialIndicator;

	// Token: 0x04007424 RID: 29732
	private int m_p1MaxGunAmmoThisFrame = 1000;

	// Token: 0x04007425 RID: 29733
	private int m_p1GunIDThisFrame = -1;

	// Token: 0x04007426 RID: 29734
	private int m_p2MaxGunAmmoThisFrame = 1000;

	// Token: 0x04007427 RID: 29735
	private int m_p2GunIDThisFrame = -1;
}
