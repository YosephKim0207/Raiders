using System;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;

// Token: 0x02001276 RID: 4726
public class DragunRageChallengeModifier : ChallengeModifier
{
	// Token: 0x060069D9 RID: 27097 RVA: 0x00297918 File Offset: 0x00295B18
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
			{
				this.m_boss = activeEnemies[i];
			}
		}
		if (this.m_boss)
		{
			this.m_boss.bulletBank.OnBulletSpawned += this.HandleProjectiles;
		}
	}

	// Token: 0x060069DA RID: 27098 RVA: 0x002979CC File Offset: 0x00295BCC
	private void HandleProjectiles(Bullet source, Projectile p)
	{
		string bankName = source.BankName;
		if (bankName != null)
		{
			if (bankName == "Breath")
			{
				BounceProjModifier orAddComponent = p.gameObject.GetOrAddComponent<BounceProjModifier>();
				orAddComponent.numberOfBounces = 1;
				orAddComponent.onlyBounceOffTiles = true;
				orAddComponent.removeBulletScriptControl = true;
			}
		}
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x00297A28 File Offset: 0x00295C28
	private void Update()
	{
		if (this.m_boss)
		{
			this.m_boss.LocalTimeScale = 1.25f;
			if (this.m_boss.behaviorSpeculator.ActiveContinuousAttackBehavior is AttackBehaviorGroup)
			{
				BehaviorBase behaviorBase = this.m_boss.behaviorSpeculator.ActiveContinuousAttackBehavior;
				while (behaviorBase is AttackBehaviorGroup)
				{
					behaviorBase = (behaviorBase as AttackBehaviorGroup).CurrentBehavior;
				}
				if (behaviorBase is SimultaneousAttackBehaviorGroup && (behaviorBase as SimultaneousAttackBehaviorGroup).AttackBehaviors.Count > 0 && (behaviorBase as SimultaneousAttackBehaviorGroup).AttackBehaviors[0] is DraGunGlockBehavior)
				{
					DraGunGlockBehavior draGunGlockBehavior = (DraGunGlockBehavior)(behaviorBase as SimultaneousAttackBehaviorGroup).AttackBehaviors[0];
					if (draGunGlockBehavior.attacks.Length >= 1 && draGunGlockBehavior.attacks[0].bulletScript.scriptTypeName.Contains("GlockRicochet"))
					{
						this.m_boss.LocalTimeScale = this.GlockRicochetTimescaleIncrease;
					}
					else
					{
						this.m_boss.LocalTimeScale = this.NormalGlockTimescaleIncrease;
					}
				}
			}
		}
	}

	// Token: 0x0400665B RID: 26203
	private AIActor m_boss;

	// Token: 0x0400665C RID: 26204
	public float GlockRicochetTimescaleIncrease = 1.7f;

	// Token: 0x0400665D RID: 26205
	public float NormalGlockTimescaleIncrease = 1.5f;
}
