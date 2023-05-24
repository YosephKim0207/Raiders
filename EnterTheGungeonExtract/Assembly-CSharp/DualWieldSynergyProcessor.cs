using System;
using UnityEngine;

// Token: 0x020016E1 RID: 5857
public class DualWieldSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600883E RID: 34878 RVA: 0x00387680 File Offset: 0x00385880
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x0600883F RID: 34879 RVA: 0x00387690 File Offset: 0x00385890
	private bool EffectValid(PlayerController p)
	{
		if (!p)
		{
			return false;
		}
		if (!p.HasActiveBonusSynergy(this.SynergyToCheck, false))
		{
			return false;
		}
		if (this.m_gun.CurrentAmmo == 0)
		{
			return false;
		}
		if (p.inventory.GunLocked.Value)
		{
			return false;
		}
		if (!this.m_isCurrentlyActive)
		{
			int indexForGun = this.GetIndexForGun(p, this.PartnerGunID);
			if (indexForGun < 0)
			{
				return false;
			}
			if (p.inventory.AllGuns[indexForGun].CurrentAmmo == 0)
			{
				return false;
			}
		}
		else if (p.CurrentSecondaryGun != null && p.CurrentSecondaryGun.PickupObjectId == this.PartnerGunID && p.CurrentSecondaryGun.CurrentAmmo == 0)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06008840 RID: 34880 RVA: 0x0038776C File Offset: 0x0038596C
	private bool PlayerUsingCorrectGuns()
	{
		return this.m_gun && this.m_gun.CurrentOwner && this.m_cachedPlayer && this.m_cachedPlayer.inventory.DualWielding && this.m_cachedPlayer.HasActiveBonusSynergy(this.SynergyToCheck, false) && (!(this.m_cachedPlayer.CurrentGun != this.m_gun) || this.m_cachedPlayer.CurrentGun.PickupObjectId == this.PartnerGunID) && (!(this.m_cachedPlayer.CurrentSecondaryGun != this.m_gun) || this.m_cachedPlayer.CurrentSecondaryGun.PickupObjectId == this.PartnerGunID);
	}

	// Token: 0x06008841 RID: 34881 RVA: 0x00387858 File Offset: 0x00385A58
	private void Update()
	{
		this.CheckStatus();
	}

	// Token: 0x06008842 RID: 34882 RVA: 0x00387860 File Offset: 0x00385A60
	private void CheckStatus()
	{
		if (this.m_isCurrentlyActive)
		{
			if (!this.PlayerUsingCorrectGuns() || !this.EffectValid(this.m_cachedPlayer))
			{
				this.DisableEffect();
			}
		}
		else if (this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.inventory.DualWielding && playerController.CurrentSecondaryGun.PickupObjectId == this.m_gun.PickupObjectId && playerController.CurrentGun.PickupObjectId == this.PartnerGunID)
			{
				this.m_isCurrentlyActive = true;
				this.m_cachedPlayer = playerController;
				return;
			}
			this.AttemptActivation(playerController);
		}
	}

	// Token: 0x06008843 RID: 34883 RVA: 0x0038792C File Offset: 0x00385B2C
	private void AttemptActivation(PlayerController ownerPlayer)
	{
		if (this.EffectValid(ownerPlayer))
		{
			this.m_isCurrentlyActive = true;
			this.m_cachedPlayer = ownerPlayer;
			ownerPlayer.inventory.SetDualWielding(true, "synergy");
			int indexForGun = this.GetIndexForGun(ownerPlayer, this.m_gun.PickupObjectId);
			int indexForGun2 = this.GetIndexForGun(ownerPlayer, this.PartnerGunID);
			ownerPlayer.inventory.SwapDualGuns();
			if (indexForGun >= 0 && indexForGun2 >= 0)
			{
				while (ownerPlayer.inventory.CurrentGun.PickupObjectId != this.PartnerGunID)
				{
					ownerPlayer.inventory.ChangeGun(1, false, false);
				}
			}
			ownerPlayer.inventory.SwapDualGuns();
			if (ownerPlayer.CurrentGun && !ownerPlayer.CurrentGun.gameObject.activeSelf)
			{
				ownerPlayer.CurrentGun.gameObject.SetActive(true);
			}
			if (ownerPlayer.CurrentSecondaryGun && !ownerPlayer.CurrentSecondaryGun.gameObject.activeSelf)
			{
				ownerPlayer.CurrentSecondaryGun.gameObject.SetActive(true);
			}
			this.m_cachedPlayer.GunChanged += this.HandleGunChanged;
		}
	}

	// Token: 0x06008844 RID: 34884 RVA: 0x00387A5C File Offset: 0x00385C5C
	private int GetIndexForGun(PlayerController p, int gunID)
	{
		for (int i = 0; i < p.inventory.AllGuns.Count; i++)
		{
			if (p.inventory.AllGuns[i].PickupObjectId == gunID)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06008845 RID: 34885 RVA: 0x00387AAC File Offset: 0x00385CAC
	private void HandleGunChanged(Gun arg1, Gun newGun, bool arg3)
	{
		this.CheckStatus();
	}

	// Token: 0x06008846 RID: 34886 RVA: 0x00387AB4 File Offset: 0x00385CB4
	private void DisableEffect()
	{
		if (this.m_isCurrentlyActive)
		{
			this.m_isCurrentlyActive = false;
			this.m_cachedPlayer.inventory.SetDualWielding(false, "synergy");
			this.m_cachedPlayer.GunChanged -= this.HandleGunChanged;
			this.m_cachedPlayer.stats.RecalculateStats(this.m_cachedPlayer, false, false);
			this.m_cachedPlayer = null;
		}
	}

	// Token: 0x04008D8D RID: 36237
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008D8E RID: 36238
	[PickupIdentifier]
	public int PartnerGunID;

	// Token: 0x04008D8F RID: 36239
	private Gun m_gun;

	// Token: 0x04008D90 RID: 36240
	private bool m_isCurrentlyActive;

	// Token: 0x04008D91 RID: 36241
	private PlayerController m_cachedPlayer;
}
