using System;

// Token: 0x0200127E RID: 4734
public class GunOverheatChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A00 RID: 27136 RVA: 0x00298A90 File Offset: 0x00296C90
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(playerController.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleGunReloaded));
		}
	}

	// Token: 0x06006A01 RID: 27137 RVA: 0x00298AE8 File Offset: 0x00296CE8
	private void HandleGunReloaded(PlayerController player, Gun playerGun)
	{
		if (playerGun.ClipShotsRemaining == 0)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.Goop).TimedAddGoopCircle(player.CenterPosition, this.Radius, 0.5f, false);
		}
	}

	// Token: 0x06006A02 RID: 27138 RVA: 0x00298B18 File Offset: 0x00296D18
	public void ForceGoop(PlayerController player)
	{
		DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.Goop).TimedAddGoopCircle(player.CenterPosition, this.Radius, 0.5f, false);
	}

	// Token: 0x06006A03 RID: 27139 RVA: 0x00298B3C File Offset: 0x00296D3C
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(playerController.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleGunReloaded));
		}
	}

	// Token: 0x04006680 RID: 26240
	public GoopDefinition Goop;

	// Token: 0x04006681 RID: 26241
	public float Radius = 3f;
}
