using System;
using FullInspector;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020014F4 RID: 5364
[Serializable]
public class EnemyDatabaseEntry : AssetBundleDatabaseEntry
{
	// Token: 0x06007A39 RID: 31289 RVA: 0x0030FDD4 File Offset: 0x0030DFD4
	public EnemyDatabaseEntry()
	{
	}

	// Token: 0x06007A3A RID: 31290 RVA: 0x0030FDE4 File Offset: 0x0030DFE4
	public EnemyDatabaseEntry(AIActor enemy)
	{
		this.myGuid = enemy.EnemyGuid;
		this.SetAll(enemy);
	}

	// Token: 0x1700120A RID: 4618
	// (get) Token: 0x06007A3B RID: 31291 RVA: 0x0030FE08 File Offset: 0x0030E008
	public override AssetBundle assetBundle
	{
		get
		{
			return EnemyDatabase.AssetBundle;
		}
	}

	// Token: 0x06007A3C RID: 31292 RVA: 0x0030FE10 File Offset: 0x0030E010
	public override void DropReference()
	{
		base.DropReference();
	}

	// Token: 0x06007A3D RID: 31293 RVA: 0x0030FE18 File Offset: 0x0030E018
	public T GetPrefab<T>() where T : UnityEngine.Object
	{
		if (!this.loadedPrefab)
		{
			this.loadedPrefab = this.assetBundle.LoadAsset<GameObject>(base.name + ".prefab").GetComponent<T>();
		}
		return this.loadedPrefab as T;
	}

	// Token: 0x06007A3E RID: 31294 RVA: 0x0030FE70 File Offset: 0x0030E070
	public void SetAll(AIActor enemy)
	{
		this.difficulty = enemy.difficulty;
		this.placeableWidth = enemy.placeableWidth;
		this.placeableHeight = enemy.placeableHeight;
		this.isNormalEnemy = enemy.IsNormalEnemy;
		this.isInBossTab = enemy.InBossAmmonomiconTab;
		this.encounterGuid = ((!enemy.encounterTrackable) ? string.Empty : enemy.encounterTrackable.TrueEncounterGuid);
		this.ForcedPositionInAmmonomicon = enemy.ForcedPositionInAmmonomicon;
	}

	// Token: 0x06007A3F RID: 31295 RVA: 0x0030FEF0 File Offset: 0x0030E0F0
	public bool Equals(AIActor other)
	{
		return !(other == null) && (this.difficulty == other.difficulty && this.placeableWidth == other.placeableWidth && this.placeableHeight == other.placeableHeight && this.isNormalEnemy == other.IsNormalEnemy && this.isInBossTab == other.InBossAmmonomiconTab && this.encounterGuid == ((!other.encounterTrackable) ? string.Empty : other.encounterTrackable.TrueEncounterGuid)) && this.ForcedPositionInAmmonomicon == other.ForcedPositionInAmmonomicon;
	}

	// Token: 0x04007C9D RID: 31901
	[InspectorDisabled]
	public DungeonPlaceableBehaviour.PlaceableDifficulty difficulty;

	// Token: 0x04007C9E RID: 31902
	[InspectorDisabled]
	public int placeableWidth;

	// Token: 0x04007C9F RID: 31903
	[InspectorDisabled]
	public int placeableHeight;

	// Token: 0x04007CA0 RID: 31904
	[InspectorDisabled]
	public bool isNormalEnemy;

	// Token: 0x04007CA1 RID: 31905
	[FormerlySerializedAs("isBoss")]
	[InspectorDisabled]
	public bool isInBossTab;

	// Token: 0x04007CA2 RID: 31906
	[InspectorDisabled]
	public string encounterGuid;

	// Token: 0x04007CA3 RID: 31907
	[InspectorDisabled]
	public int ForcedPositionInAmmonomicon = -1;
}
