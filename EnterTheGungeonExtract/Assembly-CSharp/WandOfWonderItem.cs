using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using UnityEngine;

// Token: 0x020014E0 RID: 5344
public class WandOfWonderItem : PlayerItem
{
	// Token: 0x06007989 RID: 31113 RVA: 0x00309EBC File Offset: 0x003080BC
	private AIActor GetTargetEnemy(PlayerController user)
	{
		if (user.CurrentRoom == null)
		{
			return null;
		}
		List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies == null || activeEnemies.Count <= 0)
		{
			return null;
		}
		List<AIActor> list = activeEnemies.Where((AIActor x) => !x.healthHaver.IsBoss).ToList<AIActor>();
		if (list == null || list.Count <= 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600798A RID: 31114 RVA: 0x00309F48 File Offset: 0x00308148
	public override bool CanBeUsed(PlayerController user)
	{
		return !(this.GetTargetEnemy(user) == null);
	}

	// Token: 0x0600798B RID: 31115 RVA: 0x00309F60 File Offset: 0x00308160
	protected void ProcessSingleTarget(PlayerController user, AIActor randomEnemy, ref int spawnedItems, ref int spawnedGuns)
	{
		float num = this.ItemChance;
		float num2 = this.GunChance;
		if (spawnedItems == this.MaxItemsPerRoom)
		{
			num = 0f;
		}
		if (spawnedGuns == this.MaxGunsPerRoom)
		{
			num2 = 0f;
		}
		float num3 = num + num2 + this.EnemyChance + this.VanishChance;
		float num4 = UnityEngine.Random.value * num3;
		Vector2 centerPosition = randomEnemy.CenterPosition;
		randomEnemy.EraseFromExistence(false);
		if (this.OnEffectVFX != null)
		{
			SpawnManager.SpawnVFX(this.OnEffectVFX, centerPosition, Quaternion.identity);
		}
		if (num4 < num)
		{
			GameObject gameObject = this.ItemTable.SelectByWeight(false);
			LootEngine.SpawnItem(gameObject, centerPosition, Vector2.up, 1f, true, false, false);
			spawnedItems++;
		}
		else if (num4 < num + num2)
		{
			GameObject gameObject2 = this.GunTable.SelectByWeight(false);
			LootEngine.SpawnItem(gameObject2, centerPosition, Vector2.up, 1f, true, false, false);
			spawnedGuns++;
		}
		else if (num4 < num + num2 + this.EnemyChance)
		{
			List<EnemyDatabaseEntry> list = EnemyDatabase.Instance.Entries.Where((EnemyDatabaseEntry x) => x != null && x.isNormalEnemy && !x.isInBossTab).ToList<EnemyDatabaseEntry>();
			EnemyDatabaseEntry enemyDatabaseEntry = list[UnityEngine.Random.Range(0, list.Count)];
			AIActor.Spawn(enemyDatabaseEntry.GetPrefab<AIActor>(), centerPosition.ToIntVector2(VectorConversions.Floor), user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
		}
	}

	// Token: 0x0600798C RID: 31116 RVA: 0x0030A0EC File Offset: 0x003082EC
	protected override void DoEffect(PlayerController user)
	{
		int num = 0;
		int num2 = 0;
		if (this.AffectsAllEnemiesInRoom)
		{
			List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null || activeEnemies.Count <= 0)
			{
				return;
			}
			List<AIActor> list = activeEnemies.Where((AIActor x) => !x.healthHaver.IsBoss).ToList<AIActor>();
			if (list == null || list.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.ProcessSingleTarget(user, list[i], ref num, ref num2);
			}
		}
		else
		{
			AIActor targetEnemy = this.GetTargetEnemy(user);
			if (targetEnemy == null)
			{
				return;
			}
			this.ProcessSingleTarget(user, targetEnemy, ref num, ref num2);
		}
	}

	// Token: 0x0600798D RID: 31117 RVA: 0x0030A1BC File Offset: 0x003083BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007BFF RID: 31743
	public float ItemChance = 0.25f;

	// Token: 0x04007C00 RID: 31744
	public float GunChance = 0.25f;

	// Token: 0x04007C01 RID: 31745
	public float EnemyChance = 0.5f;

	// Token: 0x04007C02 RID: 31746
	public float VanishChance = 0.25f;

	// Token: 0x04007C03 RID: 31747
	public GenericLootTable ItemTable;

	// Token: 0x04007C04 RID: 31748
	public GenericLootTable GunTable;

	// Token: 0x04007C05 RID: 31749
	public DungeonPlaceable EnemyPlaceable;

	// Token: 0x04007C06 RID: 31750
	public bool AffectsAllEnemiesInRoom;

	// Token: 0x04007C07 RID: 31751
	public int MaxItemsPerRoom = 1;

	// Token: 0x04007C08 RID: 31752
	public int MaxGunsPerRoom = 1;

	// Token: 0x04007C09 RID: 31753
	public GameObject OnEffectVFX;
}
