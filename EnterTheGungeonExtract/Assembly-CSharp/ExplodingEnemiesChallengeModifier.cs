using System;
using UnityEngine;

// Token: 0x02001278 RID: 4728
public class ExplodingEnemiesChallengeModifier : ChallengeModifier
{
	// Token: 0x060069E3 RID: 27107 RVA: 0x00297E4C File Offset: 0x0029604C
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x060069E4 RID: 27108 RVA: 0x00297EA4 File Offset: 0x002960A4
	private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
	{
		if (enemyHealth && !enemyHealth.IsBoss && fatal && enemyHealth.aiActor && enemyHealth.aiActor.IsNormalEnemy)
		{
			string name = enemyHealth.name;
			if (name.StartsWith("Bashellisk"))
			{
				return;
			}
			if (name.StartsWith("Blobulin") || name.StartsWith("Poisbulin"))
			{
				return;
			}
			Exploder.Explode(enemyHealth.aiActor.CenterPosition, this.explosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
		}
	}

	// Token: 0x060069E5 RID: 27109 RVA: 0x00297F4C File Offset: 0x0029614C
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x04006660 RID: 26208
	public ExplosionData explosion;
}
