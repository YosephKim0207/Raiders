using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010D8 RID: 4312
public class EnemyFactory : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06005EF8 RID: 24312 RVA: 0x00247EC4 File Offset: 0x002460C4
	public void ConfigureOnPlacement(RoomHandler room)
	{
		room.OnEnemiesCleared = (Action)Delegate.Combine(room.OnEnemiesCleared, new Action(this.OnWaveCleared));
		this.m_room = room;
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x00247EF0 File Offset: 0x002460F0
	private void Start()
	{
		this.SpawnWave();
	}

	// Token: 0x06005EFA RID: 24314 RVA: 0x00247EF8 File Offset: 0x002460F8
	protected List<EnemyFactorySpawnPoint> AcquireSpawnPoints()
	{
		return this.m_room.GetComponentsInRoom<EnemyFactorySpawnPoint>();
	}

	// Token: 0x06005EFB RID: 24315 RVA: 0x00247F08 File Offset: 0x00246108
	private IEnumerator SpawnWaveCR()
	{
		yield return new WaitForSeconds(this.delayBetweenWaves);
		EnemyFactoryWaveDefinition waveToSpawn = this.waves[this.m_currentWave];
		List<EnemyFactorySpawnPoint> spawnPoints = this.AcquireSpawnPoints();
		if (waveToSpawn.exactDefinition)
		{
			for (int i = 0; i < waveToSpawn.enemyList.Count; i++)
			{
				IntVector2 intVector = spawnPoints[this.m_spawnPointIterator].transform.position.IntXY(VectorConversions.Floor);
				spawnPoints[this.m_spawnPointIterator].OnSpawn(waveToSpawn.enemyList[i], intVector, this.m_room);
				this.m_spawnPointIterator = (this.m_spawnPointIterator + 1) % spawnPoints.Count;
			}
		}
		else
		{
			int num = UnityEngine.Random.Range(waveToSpawn.inexactMinCount, waveToSpawn.inexactMaxCount + 1);
			for (int j = 0; j < num; j++)
			{
				IntVector2 intVector2 = spawnPoints[this.m_spawnPointIterator].transform.position.IntXY(VectorConversions.Floor);
				spawnPoints[this.m_spawnPointIterator].OnSpawn(waveToSpawn.enemyList[UnityEngine.Random.Range(0, waveToSpawn.enemyList.Count)], intVector2, this.m_room);
				this.m_spawnPointIterator = (this.m_spawnPointIterator + 1) % spawnPoints.Count;
			}
		}
		yield break;
	}

	// Token: 0x06005EFC RID: 24316 RVA: 0x00247F24 File Offset: 0x00246124
	public void SpawnWave()
	{
		base.StartCoroutine(this.SpawnWaveCR());
	}

	// Token: 0x06005EFD RID: 24317 RVA: 0x00247F34 File Offset: 0x00246134
	protected void ProvideReward()
	{
		if (this.rewardChestPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rewardChestPrefab, base.transform.position, Quaternion.identity);
			Chest component = gameObject.GetComponent<Chest>();
			component.ConfigureOnPlacement(this.m_room);
			this.m_room.RegisterInteractable(component);
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(component.specRigidbody, null, false);
		}
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x00247FA8 File Offset: 0x002461A8
	public void OnWaveCleared()
	{
		if (this.m_currentWave < this.waves.Count - 1)
		{
			this.m_currentWave++;
			this.SpawnWave();
		}
		else if (!this.m_finished)
		{
			this.m_finished = true;
			this.m_room.HandleRoomAction(RoomEventTriggerAction.UNSEAL_ROOM);
			this.ProvideReward();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06005EFF RID: 24319 RVA: 0x00248018 File Offset: 0x00246218
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400592F RID: 22831
	[BetterList]
	public List<EnemyFactoryWaveDefinition> waves;

	// Token: 0x04005930 RID: 22832
	public float delayBetweenWaves = 1f;

	// Token: 0x04005931 RID: 22833
	public GameObject rewardChestPrefab;

	// Token: 0x04005932 RID: 22834
	protected int m_currentWave;

	// Token: 0x04005933 RID: 22835
	protected RoomHandler m_room;

	// Token: 0x04005934 RID: 22836
	protected int m_spawnPointIterator;

	// Token: 0x04005935 RID: 22837
	protected bool m_finished;
}
