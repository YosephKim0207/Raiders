using System;
using System.Collections;
using UnityEngine;

// Token: 0x020016EC RID: 5868
public class GunFormeSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600886A RID: 34922 RVA: 0x00388DC8 File Offset: 0x00386FC8
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
	}

	// Token: 0x0600886B RID: 34923 RVA: 0x00388E00 File Offset: 0x00387000
	private void Update()
	{
		if (this.m_gun && !this.m_gun.CurrentOwner && this.CurrentForme != 0)
		{
			this.ChangeForme(this.Formes[0]);
			this.CurrentForme = 0;
		}
		this.JustActiveReloaded = false;
	}

	// Token: 0x0600886C RID: 34924 RVA: 0x00388E5C File Offset: 0x0038705C
	private void HandleReloadPressed(PlayerController ownerPlayer, Gun sourceGun, bool manual)
	{
		if (this.JustActiveReloaded)
		{
			return;
		}
		if (manual && !sourceGun.IsReloading)
		{
			int nextValidForme = this.GetNextValidForme(ownerPlayer);
			if (nextValidForme != this.CurrentForme)
			{
				this.ChangeForme(this.Formes[nextValidForme]);
				this.CurrentForme = nextValidForme;
			}
		}
	}

	// Token: 0x0600886D RID: 34925 RVA: 0x00388EB0 File Offset: 0x003870B0
	private int GetNextValidForme(PlayerController ownerPlayer)
	{
		for (int i = 0; i < this.Formes.Length; i++)
		{
			int num = (i + this.CurrentForme) % this.Formes.Length;
			if (num != this.CurrentForme)
			{
				if (this.Formes[num].IsValid(ownerPlayer))
				{
					return num;
				}
			}
		}
		return this.CurrentForme;
	}

	// Token: 0x0600886E RID: 34926 RVA: 0x00388F14 File Offset: 0x00387114
	private void ChangeForme(GunFormeData targetForme)
	{
		Gun gun = PickupObjectDatabase.GetById(targetForme.FormeID) as Gun;
		this.m_gun.TransformToTargetGun(gun);
		if (this.m_gun.encounterTrackable && gun.encounterTrackable)
		{
			this.m_gun.encounterTrackable.journalData.PrimaryDisplayName = gun.encounterTrackable.journalData.PrimaryDisplayName;
			this.m_gun.encounterTrackable.journalData.ClearCache();
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController)
			{
				GameUIRoot.Instance.TemporarilyShowGunName(playerController.IsPrimaryPlayer);
			}
		}
	}

	// Token: 0x0600886F RID: 34927 RVA: 0x00388FCC File Offset: 0x003871CC
	public static void AssignTemporaryOverrideGun(PlayerController targetPlayer, int gunID, float duration)
	{
		if (targetPlayer && !targetPlayer.IsGhost)
		{
			targetPlayer.StartCoroutine(GunFormeSynergyProcessor.HandleTransformationDuration(targetPlayer, gunID, duration));
		}
	}

	// Token: 0x06008870 RID: 34928 RVA: 0x00388FF4 File Offset: 0x003871F4
	public static IEnumerator HandleTransformationDuration(PlayerController targetPlayer, int gunID, float duration)
	{
		float elapsed = 0f;
		if (targetPlayer && targetPlayer.inventory.GunLocked.Value && targetPlayer.CurrentGun)
		{
			MimicGunController component = targetPlayer.CurrentGun.GetComponent<MimicGunController>();
			if (component)
			{
				component.ForceClearMimic(false);
			}
		}
		targetPlayer.inventory.GunChangeForgiveness = true;
		Gun limitGun = PickupObjectDatabase.GetById(gunID) as Gun;
		Gun m_extantGun = targetPlayer.inventory.AddGunToInventory(limitGun, true);
		m_extantGun.CanBeDropped = false;
		m_extantGun.CanBeSold = false;
		targetPlayer.inventory.GunLocked.SetOverride("override gun", true, null);
		elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		GunFormeSynergyProcessor.ClearTemporaryOverrideGun(targetPlayer, m_extantGun);
		yield break;
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x00389020 File Offset: 0x00387220
	protected static void ClearTemporaryOverrideGun(PlayerController targetPlayer, Gun m_extantGun)
	{
		if (!targetPlayer || !m_extantGun)
		{
			return;
		}
		if (targetPlayer)
		{
			targetPlayer.inventory.GunLocked.RemoveOverride("override gun");
			targetPlayer.inventory.DestroyGun(m_extantGun);
			m_extantGun = null;
		}
		targetPlayer.inventory.GunChangeForgiveness = false;
	}

	// Token: 0x04008DD0 RID: 36304
	public GunFormeData[] Formes;

	// Token: 0x04008DD1 RID: 36305
	private Gun m_gun;

	// Token: 0x04008DD2 RID: 36306
	private int CurrentForme;

	// Token: 0x04008DD3 RID: 36307
	[NonSerialized]
	public bool JustActiveReloaded;
}
