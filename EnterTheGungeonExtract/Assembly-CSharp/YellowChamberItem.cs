using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020014E2 RID: 5346
public class YellowChamberItem : PassiveItem
{
	// Token: 0x0600799E RID: 31134 RVA: 0x0030AA70 File Offset: 0x00308C70
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.OnEnteredCombat));
	}

	// Token: 0x0600799F RID: 31135 RVA: 0x0030AAB0 File Offset: 0x00308CB0
	private void OnEnteredCombat()
	{
		if (this.m_currentlyCharmedEnemy)
		{
			return;
		}
		if (UnityEngine.Random.value < this.ChanceToHappen)
		{
			this.m_player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.m_enemyList);
			for (int i = 0; i < this.m_enemyList.Count; i++)
			{
				AIActor aiactor = this.m_enemyList[i];
				if (!aiactor || !aiactor.IsNormalEnemy || !aiactor.healthHaver || aiactor.healthHaver.IsBoss || aiactor.IsHarmlessEnemy)
				{
					this.m_enemyList.RemoveAt(i);
					i--;
				}
			}
			if (this.m_enemyList.Count > 1)
			{
				AIActor aiactor2 = this.m_enemyList[UnityEngine.Random.Range(0, this.m_enemyList.Count)];
				aiactor2.IgnoreForRoomClear = true;
				aiactor2.ParentRoom.ResetEnemyHPPercentage();
				aiactor2.ApplyEffect(this.CharmEffect, 1f, null);
				this.m_currentlyCharmedEnemy = aiactor2;
			}
		}
	}

	// Token: 0x060079A0 RID: 31136 RVA: 0x0030ABC8 File Offset: 0x00308DC8
	protected override void Update()
	{
		if (this.m_pickedUp && this.m_player && this.m_currentlyCharmedEnemy && (this.m_player.CurrentRoom == null || this.m_player.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) <= 0))
		{
			this.EatCharmedEnemy();
		}
		base.Update();
	}

	// Token: 0x060079A1 RID: 31137 RVA: 0x0030AC34 File Offset: 0x00308E34
	private void EatCharmedEnemy()
	{
		if (!this.m_currentlyCharmedEnemy)
		{
			return;
		}
		if (this.m_currentlyCharmedEnemy.behaviorSpeculator)
		{
			this.m_currentlyCharmedEnemy.behaviorSpeculator.Stun(1f, true);
		}
		if (this.m_currentlyCharmedEnemy.knockbackDoer)
		{
			this.m_currentlyCharmedEnemy.knockbackDoer.SetImmobile(true, "YellowChamberItem");
		}
		GameObject gameObject = this.m_currentlyCharmedEnemy.PlayEffectOnActor(this.EraseVFX, new Vector3(0f, -1f, 0f), false, false, false);
		this.m_currentlyCharmedEnemy.StartCoroutine(this.DelayedDestroyEnemy(this.m_currentlyCharmedEnemy, gameObject.GetComponent<tk2dSpriteAnimator>()));
		this.m_currentlyCharmedEnemy = null;
	}

	// Token: 0x060079A2 RID: 31138 RVA: 0x0030ACF8 File Offset: 0x00308EF8
	private IEnumerator DelayedDestroyEnemy(AIActor enemy, tk2dSpriteAnimator vfxAnimator)
	{
		if (vfxAnimator)
		{
			vfxAnimator.sprite.IsPerpendicular = false;
			vfxAnimator.sprite.HeightOffGround = -1f;
		}
		while (enemy && vfxAnimator && vfxAnimator.sprite.GetCurrentSpriteDef().name != "kthuliber_tentacles_010")
		{
			vfxAnimator.sprite.UpdateZDepth();
			yield return null;
		}
		if (vfxAnimator)
		{
			vfxAnimator.sprite.IsPerpendicular = true;
			vfxAnimator.sprite.HeightOffGround = 1.5f;
			vfxAnimator.sprite.UpdateZDepth();
		}
		if (enemy)
		{
			enemy.EraseFromExistence(false);
		}
		yield break;
	}

	// Token: 0x060079A3 RID: 31139 RVA: 0x0030AD1C File Offset: 0x00308F1C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<YellowChamberItem>().m_pickedUpThisRun = true;
		player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.OnEnteredCombat));
		return debrisObject;
	}

	// Token: 0x060079A4 RID: 31140 RVA: 0x0030AD68 File Offset: 0x00308F68
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			PlayerController player = this.m_player;
			player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.OnEnteredCombat));
		}
	}

	// Token: 0x04007C1F RID: 31775
	public float ChanceToHappen = 0.25f;

	// Token: 0x04007C20 RID: 31776
	public GameActorCharmEffect CharmEffect;

	// Token: 0x04007C21 RID: 31777
	public GameObject EraseVFX;

	// Token: 0x04007C22 RID: 31778
	private PlayerController m_player;

	// Token: 0x04007C23 RID: 31779
	private AIActor m_currentlyCharmedEnemy;

	// Token: 0x04007C24 RID: 31780
	private List<AIActor> m_enemyList = new List<AIActor>();
}
