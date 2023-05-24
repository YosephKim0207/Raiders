using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001273 RID: 4723
public class DemonWallChallengeModifier : ChallengeModifier
{
	// Token: 0x060069CC RID: 27084 RVA: 0x00297228 File Offset: 0x00295428
	private IEnumerator Start()
	{
		RoomHandler room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		AIActor m_boss = null;
		List<AIActor> roomEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < roomEnemies.Count; i++)
		{
			if (roomEnemies[i] && roomEnemies[i].healthHaver && roomEnemies[i].healthHaver.IsBoss)
			{
				m_boss = roomEnemies[i];
			}
		}
		yield return null;
		while (Time.timeScale == 0f)
		{
			yield return null;
		}
		yield return null;
		AIActor prefabEnemy = EnemyDatabase.GetOrLoadByGuid(this.SniperGuyGuid);
		IntVector2 spawnPosition = room.area.basePosition;
		this.m_sniper1 = AIActor.Spawn(prefabEnemy, spawnPosition + new IntVector2(4, 78), room, true, AIActor.AwakenAnimationType.Default, true);
		this.m_sniper2 = AIActor.Spawn(prefabEnemy, spawnPosition + new IntVector2(20, 78), room, true, AIActor.AwakenAnimationType.Default, true);
		this.m_sniper1.transform.position = this.m_sniper1.transform.position + new Vector3(0f, 0.25f, 0f);
		this.m_sniper2.transform.position = this.m_sniper2.transform.position + new Vector3(0f, 0.25f, 0f);
		this.m_sniper1.specRigidbody.Reinitialize();
		this.m_sniper2.specRigidbody.Reinitialize();
		this.m_sniper1.healthHaver.PreventAllDamage = true;
		this.m_sniper2.healthHaver.PreventAllDamage = true;
		this.m_sniper1.knockbackDoer.knockbackMultiplier = 0f;
		this.m_sniper2.knockbackDoer.knockbackMultiplier = 0f;
		this.m_sniper1.MovementSpeed = 0f;
		this.m_sniper2.MovementSpeed = 0f;
		ShootGunBehavior sgb = this.m_sniper1.behaviorSpeculator.AttackBehaviors[0] as ShootGunBehavior;
		sgb.Range = 400f;
		sgb.Cooldown = this.SniperCooldown;
		sgb = this.m_sniper2.behaviorSpeculator.AttackBehaviors[0] as ShootGunBehavior;
		sgb.Range = 400f;
		sgb.Cooldown = this.SniperCooldown;
		yield return null;
		this.m_sniper1.aiShooter.CurrentGun.CustomLaserSightDistance = 90f;
		this.m_sniper2.aiShooter.CurrentGun.CustomLaserSightDistance = 90f;
		this.m_sniper1.aiShooter.CurrentGun.CustomLaserSightHeight = 7f;
		this.m_sniper2.aiShooter.CurrentGun.CustomLaserSightHeight = 7f;
		this.m_sniper1.sprite.HeightOffGround = 7f;
		this.m_sniper2.sprite.HeightOffGround = 7f;
		this.m_sniper1.transform.parent = m_boss.transform.Find("sniper1");
		this.m_sniper2.transform.parent = m_boss.transform.Find("sniper2");
		yield break;
	}

	// Token: 0x060069CD RID: 27085 RVA: 0x00297244 File Offset: 0x00295444
	private void LateUpdate()
	{
		if (this.m_sniper1)
		{
			this.m_sniper1.sprite.UpdateZDepth();
		}
		if (this.m_sniper2)
		{
			this.m_sniper2.sprite.UpdateZDepth();
		}
	}

	// Token: 0x04006648 RID: 26184
	[EnemyIdentifier]
	public string SniperGuyGuid;

	// Token: 0x04006649 RID: 26185
	public float SniperCooldown = 2.4f;

	// Token: 0x0400664A RID: 26186
	private AIActor m_sniper1;

	// Token: 0x0400664B RID: 26187
	private AIActor m_sniper2;
}
