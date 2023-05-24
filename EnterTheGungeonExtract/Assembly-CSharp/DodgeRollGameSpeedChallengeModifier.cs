using System;
using UnityEngine;

// Token: 0x02001275 RID: 4725
public class DodgeRollGameSpeedChallengeModifier : ChallengeModifier
{
	// Token: 0x060069D5 RID: 27093 RVA: 0x002977D8 File Offset: 0x002959D8
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].OnPreDodgeRoll += this.OnDodgeRoll;
		}
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x00297820 File Offset: 0x00295A20
	private void OnDodgeRoll(PlayerController obj)
	{
		float num = this.SpeedGain;
		float num2 = this.SpeedMax;
		if (GameManager.Instance.PrimaryPlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
		{
			num = this.BossSpeedGain;
			num2 = this.BossSpeedMax;
		}
		this.CurrentSpeedModifier = Mathf.Clamp(this.CurrentSpeedModifier + num * 0.01f, 1f, num2);
		BraveTime.ClearMultiplier(base.gameObject);
		BraveTime.RegisterTimeScaleMultiplier(this.CurrentSpeedModifier, base.gameObject);
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x002978A4 File Offset: 0x00295AA4
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].OnPreDodgeRoll -= this.OnDodgeRoll;
		}
		BraveTime.ClearMultiplier(base.gameObject);
	}

	// Token: 0x04006656 RID: 26198
	public float SpeedGain = 2.5f;

	// Token: 0x04006657 RID: 26199
	public float SpeedMax = 1.5f;

	// Token: 0x04006658 RID: 26200
	[Header("Boss Parameters")]
	public float BossSpeedGain = 1f;

	// Token: 0x04006659 RID: 26201
	public float BossSpeedMax = 1.3f;

	// Token: 0x0400665A RID: 26202
	private float CurrentSpeedModifier = 1f;
}
