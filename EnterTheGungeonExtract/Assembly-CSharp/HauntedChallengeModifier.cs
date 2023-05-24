using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001281 RID: 4737
public class HauntedChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A10 RID: 27152 RVA: 0x002990D0 File Offset: 0x002972D0
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x06006A11 RID: 27153 RVA: 0x00299128 File Offset: 0x00297328
	private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
	{
		if (enemyHealth && !enemyHealth.IsBoss && fatal && UnityEngine.Random.value < this.Chance && enemyHealth.aiActor && enemyHealth.aiActor.IsNormalEnemy && enemyHealth.aiActor.ActorName != "Hollow Point")
		{
			if (enemyHealth.GetComponent<SpawnEnemyOnDeath>())
			{
				return;
			}
			string actorName = enemyHealth.aiActor.ActorName;
			if (actorName == "Blobulin" || actorName == "Bombshee")
			{
				return;
			}
			if (actorName.Contains("Bullat"))
			{
				return;
			}
			if (actorName.StartsWith("Mine Flayer ") && UnityEngine.Random.value > 0.25f)
			{
				return;
			}
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (enemyHealth.aiActor.ParentRoom != bestActivePlayer.CurrentRoom)
			{
				return;
			}
			if (enemyHealth.aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) <= 0)
			{
				return;
			}
			Vector2 centerPosition = enemyHealth.aiActor.CenterPosition;
			IntVector2 intVector = centerPosition.ToIntVector2(VectorConversions.Floor);
			if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
			{
				CellData cellData = GameManager.Instance.Dungeon.data[intVector];
				if (cellData.isExitCell)
				{
					return;
				}
				if (cellData.parentRoom != bestActivePlayer.CurrentRoom)
				{
					return;
				}
				if (centerPosition.GetAbsoluteRoom() != bestActivePlayer.CurrentRoom)
				{
					return;
				}
				AIActor aiactor = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(this.GhostGuid), centerPosition, bestActivePlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
				if (aiactor && !string.IsNullOrEmpty(this.GhostOverrideSpawnAnimation))
				{
					AIAnimator.NamedDirectionalAnimation namedDirectionalAnimation = aiactor.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation vfx) => vfx.name == "awaken");
					if (namedDirectionalAnimation != null)
					{
						namedDirectionalAnimation.anim.Prefix = this.GhostOverrideSpawnAnimation;
					}
				}
				aiactor.HasBeenEngaged = true;
			}
		}
	}

	// Token: 0x06006A12 RID: 27154 RVA: 0x00299344 File Offset: 0x00297544
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(playerController.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
	}

	// Token: 0x04006688 RID: 26248
	[EnemyIdentifier]
	public string GhostGuid;

	// Token: 0x04006689 RID: 26249
	public float Chance = 0.5f;

	// Token: 0x0400668A RID: 26250
	public string GhostOverrideSpawnAnimation;
}
