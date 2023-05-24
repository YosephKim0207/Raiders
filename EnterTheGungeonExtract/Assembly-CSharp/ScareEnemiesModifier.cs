using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013E5 RID: 5093
public class ScareEnemiesModifier : MonoBehaviour
{
	// Token: 0x06007399 RID: 29593 RVA: 0x002DFC08 File Offset: 0x002DDE08
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x0600739A RID: 29594 RVA: 0x002DFC18 File Offset: 0x002DDE18
	private void Update()
	{
		if (this.m_gun && this.m_gun.CurrentOwner && this.m_gun.CurrentOwner.healthHaver && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.CurrentGun != this.m_gun)
			{
				return;
			}
			if (playerController.CurrentRoom == null)
			{
				return;
			}
			this.FleeData.Player = playerController;
			playerController.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.m_allEnemies);
			if (this.m_allEnemies == null || this.m_allEnemies.Count == 0)
			{
				return;
			}
			float currentAngle = this.m_gun.CurrentAngle;
			Vector2 centerPosition = this.m_gun.CurrentOwner.CenterPosition;
			for (int i = 0; i < this.m_allEnemies.Count; i++)
			{
				AIActor aiactor = this.m_allEnemies[i];
				if (aiactor && aiactor.healthHaver && aiactor.IsNormalEnemy && aiactor.IsWorthShootingAt && !aiactor.healthHaver.IsBoss && !aiactor.healthHaver.IsDead && aiactor.behaviorSpeculator)
				{
					if (BraveMathCollege.AbsAngleBetween(currentAngle, BraveMathCollege.Atan2Degrees(aiactor.CenterPosition - centerPosition)) < this.ConeAngle)
					{
						aiactor.behaviorSpeculator.FleePlayerData = ((!this.OnlyFearDuringReload || this.m_gun.IsReloading) ? this.FleeData : null);
					}
					else
					{
						aiactor.behaviorSpeculator.FleePlayerData = null;
					}
				}
			}
			this.m_allEnemies.Clear();
		}
	}

	// Token: 0x0400752F RID: 29999
	public FleePlayerData FleeData;

	// Token: 0x04007530 RID: 30000
	public float ConeAngle = 45f;

	// Token: 0x04007531 RID: 30001
	public bool OnlyFearDuringReload;

	// Token: 0x04007532 RID: 30002
	private Gun m_gun;

	// Token: 0x04007533 RID: 30003
	private List<AIActor> m_allEnemies = new List<AIActor>();
}
