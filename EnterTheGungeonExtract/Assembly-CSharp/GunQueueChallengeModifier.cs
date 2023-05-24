using System;
using Dungeonator;

// Token: 0x0200127F RID: 4735
public class GunQueueChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A05 RID: 27141 RVA: 0x00298BA8 File Offset: 0x00296DA8
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].inventory.GunLocked.SetOverride("challenge", true, null);
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnReloadPressed = (Action<PlayerController, Gun>)Delegate.Combine(playerController.OnReloadPressed, new Action<PlayerController, Gun>(this.HandleGunReloadPress));
			PlayerController playerController2 = GameManager.Instance.AllPlayers[i];
			playerController2.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(playerController2.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleGunReloaded));
		}
		this.m_gunQueueTimer = this.AutoSwitchTime;
	}

	// Token: 0x06006A06 RID: 27142 RVA: 0x00298C64 File Offset: 0x00296E64
	private void Update()
	{
		this.m_elapsed += BraveTime.DeltaTime;
		this.m_gunQueueTimer -= BraveTime.DeltaTime;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			Gun currentGun = GameManager.Instance.AllPlayers[i].CurrentGun;
			if (currentGun)
			{
				if (currentGun.ammo == 0 || (currentGun.UsesRechargeLikeActiveItem && currentGun.RemainingActiveCooldownAmount > 0f))
				{
					this.HandleGunReloaded(GameManager.Instance.AllPlayers[i], null);
				}
			}
		}
		if (this.m_gunQueueTimer <= 0f)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				this.HandleGunReloaded(GameManager.Instance.AllPlayers[j], null);
			}
		}
	}

	// Token: 0x06006A07 RID: 27143 RVA: 0x00298D54 File Offset: 0x00296F54
	public override bool IsValid(RoomHandler room)
	{
		if (room.IsGunslingKingChallengeRoom)
		{
			return false;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].inventory != null && GameManager.Instance.AllPlayers[i].inventory.AllGuns.Count > 1)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006A08 RID: 27144 RVA: 0x00298DDC File Offset: 0x00296FDC
	private void HandleGunReloadPress(PlayerController player, Gun playerGun)
	{
		if (this.m_elapsed > 1f)
		{
			this.QueueLogic(player, playerGun);
		}
	}

	// Token: 0x06006A09 RID: 27145 RVA: 0x00298DF8 File Offset: 0x00296FF8
	private void HandleGunReloaded(PlayerController player, Gun playerGun)
	{
		this.QueueLogic(player, playerGun);
	}

	// Token: 0x06006A0A RID: 27146 RVA: 0x00298E04 File Offset: 0x00297004
	private void QueueLogic(PlayerController player, Gun playerGun)
	{
		if (!this)
		{
			return;
		}
		player.inventory.GunLocked.RemoveOverride("challenge");
		Gun currentGun = player.CurrentGun;
		if (currentGun && player.inventory.GunLocked.Value)
		{
			MimicGunController component = currentGun.GetComponent<MimicGunController>();
			if (component)
			{
				component.ForceClearMimic(false);
			}
		}
		if (ChallengeManager.Instance && currentGun && currentGun.ClipShotsRemaining == 0)
		{
			for (int i = 0; i < ChallengeManager.Instance.ActiveChallenges.Count; i++)
			{
				if (ChallengeManager.Instance.ActiveChallenges[i] is GunOverheatChallengeModifier)
				{
					GunOverheatChallengeModifier gunOverheatChallengeModifier = ChallengeManager.Instance.ActiveChallenges[i] as GunOverheatChallengeModifier;
					gunOverheatChallengeModifier.ForceGoop(player);
				}
			}
		}
		if (currentGun)
		{
			currentGun.ForceImmediateReload(true);
		}
		player.inventory.GunChangeForgiveness = true;
		player.ChangeGun(1, false, false);
		player.inventory.GunChangeForgiveness = false;
		player.inventory.GunLocked.SetOverride("challenge", true, null);
		this.m_gunQueueTimer = this.AutoSwitchTime;
		this.m_elapsed = 0f;
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x00298F58 File Offset: 0x00297158
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(playerController.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleGunReloaded));
			PlayerController playerController2 = GameManager.Instance.AllPlayers[i];
			playerController2.OnReloadPressed = (Action<PlayerController, Gun>)Delegate.Remove(playerController2.OnReloadPressed, new Action<PlayerController, Gun>(this.HandleGunReloadPress));
			GameManager.Instance.AllPlayers[i].inventory.GunLocked.RemoveOverride("challenge");
		}
	}

	// Token: 0x04006682 RID: 26242
	public float AutoSwitchTime = 15f;

	// Token: 0x04006683 RID: 26243
	private float m_elapsed;

	// Token: 0x04006684 RID: 26244
	private float m_gunQueueTimer;
}
