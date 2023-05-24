using System;
using Dungeonator;

// Token: 0x02001277 RID: 4727
public class EnemyDeathBurstChallengeModifier : ChallengeModifier
{
	// Token: 0x060069DD RID: 27101 RVA: 0x00297B50 File Offset: 0x00295D50
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x060069DE RID: 27102 RVA: 0x00297BA8 File Offset: 0x00295DA8
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
			this.SetDeathBurst(enemyHealth);
		}
	}

	// Token: 0x060069DF RID: 27103 RVA: 0x00297C34 File Offset: 0x00295E34
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x060069E0 RID: 27104 RVA: 0x00297C8C File Offset: 0x00295E8C
	public override bool IsValid(RoomHandler room)
	{
		return room.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS && base.IsValid(room);
	}

	// Token: 0x060069E1 RID: 27105 RVA: 0x00297CA8 File Offset: 0x00295EA8
	private void SetDeathBurst(HealthHaver healthHaver)
	{
		AIActor aiActor = healthHaver.aiActor;
		if (!aiActor || !aiActor.IsNormalEnemy || !aiActor.healthHaver || aiActor.healthHaver.IsBoss)
		{
			return;
		}
		if (!healthHaver.spawnBulletScript)
		{
			if (healthHaver.bulletBank)
			{
				AIBulletBank.Entry bullet = healthHaver.bulletBank.GetBullet("default");
				if (bullet == null)
				{
					AIBulletBank.Entry entry = new AIBulletBank.Entry();
					entry.Name = "default";
					entry.BulletObject = this.DefaultFallbackProjectile.gameObject;
					entry.ProjectileData = new ProjectileData();
					entry.ProjectileData.onDestroyBulletScript = new BulletScriptSelector();
					healthHaver.bulletBank.Bullets.Add(entry);
				}
				else if (bullet.BulletObject == null)
				{
					bullet.BulletObject = this.DefaultFallbackProjectile.gameObject;
				}
				healthHaver.spawnBulletScript = true;
				healthHaver.chanceToSpawnBulletScript = 1f;
				healthHaver.bulletScriptType = HealthHaver.BulletScriptType.OnPreDeath;
				healthHaver.bulletScript = this.DeathBulletScript;
				if (!string.IsNullOrEmpty(healthHaver.overrideDeathAnimBulletScript))
				{
					string overrideDeathAnimBulletScript = healthHaver.overrideDeathAnimBulletScript;
					bool flag = false;
					if (healthHaver.aiAnimator && healthHaver.aiAnimator.HasDirectionalAnimation(overrideDeathAnimBulletScript))
					{
						flag = true;
					}
					if (healthHaver.spriteAnimator && healthHaver.spriteAnimator.GetClipByName(overrideDeathAnimBulletScript) != null)
					{
						flag = true;
					}
					if (!flag)
					{
						healthHaver.overrideDeathAnimBulletScript = string.Empty;
					}
				}
			}
		}
		else
		{
			healthHaver.chanceToSpawnBulletScript = 1f;
		}
	}

	// Token: 0x0400665E RID: 26206
	public BulletScriptSelector DeathBulletScript;

	// Token: 0x0400665F RID: 26207
	public Projectile DefaultFallbackProjectile;
}
