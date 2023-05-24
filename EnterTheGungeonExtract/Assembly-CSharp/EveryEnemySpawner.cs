using System;
using System.Collections;
using Dungeonator;

// Token: 0x02001252 RID: 4690
public class EveryEnemySpawner : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x0600692C RID: 26924 RVA: 0x00292918 File Offset: 0x00290B18
	public void Start()
	{
		this.m_room.Entered += this.PlayerEntered;
		this.m_blobulinPrefab = EnemyDatabase.Instance.Entries.Find((EnemyDatabaseEntry e) => e.path.Contains("/Blobulin.prefab")).GetPrefab<AIActor>();
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x00292974 File Offset: 0x00290B74
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x0600692E RID: 26926 RVA: 0x00292980 File Offset: 0x00290B80
	public void PlayerEntered(PlayerController playerController)
	{
		base.StartCoroutine(this.SpawnAllEnemies());
	}

	// Token: 0x0600692F RID: 26927 RVA: 0x00292990 File Offset: 0x00290B90
	private IEnumerator SpawnAllEnemies()
	{
		foreach (EnemyDatabaseEntry entry in EnemyDatabase.Instance.Entries)
		{
			if (entry.isNormalEnemy && !entry.isInBossTab)
			{
				bool shouldBreak = false;
				for (int i = 0; i < this.ignoreList.Length; i++)
				{
					if (entry.path.Contains("/" + this.ignoreList[i] + ".prefab"))
					{
						shouldBreak = true;
						break;
					}
				}
				if (!shouldBreak)
				{
					IntVector2 pos = base.transform.position.XY().ToIntVector2(VectorConversions.Floor);
					for (int j = -5; j <= 5; j++)
					{
						for (int k = -5; k <= 5; k++)
						{
							DeadlyDeadlyGoopManager.ForceClearGoopsInCell(new IntVector2(pos.x + j, pos.y + k));
						}
					}
					AIActor prefab = entry.GetPrefab<AIActor>();
					IntVector2 intVector = base.transform.position.XY().ToIntVector2(VectorConversions.Floor);
					RoomHandler room = this.m_room;
					bool flag = !this.reinforce;
					AIActor enemy = AIActor.Spawn(prefab, intVector, room, false, AIActor.AwakenAnimationType.Default, flag);
					if (this.reinforce)
					{
						enemy.HandleReinforcementFallIntoRoom(0f);
					}
					if (enemy.name.Contains("MetalCubeGuy"))
					{
						AIActor.Spawn(this.m_blobulinPrefab, base.transform.position.XY().ToIntVector2(VectorConversions.Floor) + new IntVector2(-2, 0), this.m_room, false, AIActor.AwakenAnimationType.Default, true);
					}
					this.m_room.SealRoom();
					float unsealedTime = 0f;
					float requiredUnsealedTime = ((!enemy.GetComponent<SpawnEnemyOnDeath>()) ? 0.5f : 1.5f);
					while ((enemy && enemy.healthHaver.IsAlive) || unsealedTime < requiredUnsealedTime)
					{
						unsealedTime = ((!this.m_room.IsSealed) ? (unsealedTime + BraveTime.DeltaTime) : 0f);
						yield return null;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x04006588 RID: 25992
	public string[] ignoreList;

	// Token: 0x04006589 RID: 25993
	public bool reinforce;

	// Token: 0x0400658A RID: 25994
	private RoomHandler m_room;

	// Token: 0x0400658B RID: 25995
	private AIActor m_blobulinPrefab;
}
