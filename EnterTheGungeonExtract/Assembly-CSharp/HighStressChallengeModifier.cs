using System;

// Token: 0x02001284 RID: 4740
public class HighStressChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A1D RID: 27165 RVA: 0x002997CC File Offset: 0x002979CC
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].OnReceivedDamage += this.OnPlayerReceivedDamage;
		}
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x00299814 File Offset: 0x00297A14
	private void OnPlayerReceivedDamage(PlayerController p)
	{
		if (p && p.healthHaver)
		{
			p.TriggerHighStress(this.StressDuration);
		}
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x00299840 File Offset: 0x00297A40
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].OnReceivedDamage -= this.OnPlayerReceivedDamage;
		}
	}

	// Token: 0x04006695 RID: 26261
	public float StressDuration = 5f;
}
