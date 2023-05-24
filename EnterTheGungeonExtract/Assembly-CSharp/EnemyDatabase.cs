using System;
using UnityEngine;

// Token: 0x020014F3 RID: 5363
public class EnemyDatabase : AssetBundleDatabase<AIActor, EnemyDatabaseEntry>
{
	// Token: 0x17001207 RID: 4615
	// (get) Token: 0x06007A2F RID: 31279 RVA: 0x0030FC58 File Offset: 0x0030DE58
	public static EnemyDatabase Instance
	{
		get
		{
			if (EnemyDatabase.m_instance == null)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				int frameCount = Time.frameCount;
				EnemyDatabase.m_instance = EnemyDatabase.AssetBundle.LoadAsset<EnemyDatabase>("EnemyDatabase");
				DebugTime.Log(realtimeSinceStartup, frameCount, "Loading EnemyDatabase from AssetBundle", new object[0]);
			}
			return EnemyDatabase.m_instance;
		}
	}

	// Token: 0x17001208 RID: 4616
	// (get) Token: 0x06007A30 RID: 31280 RVA: 0x0030FCAC File Offset: 0x0030DEAC
	public static bool HasInstance
	{
		get
		{
			return EnemyDatabase.m_instance != null;
		}
	}

	// Token: 0x17001209 RID: 4617
	// (get) Token: 0x06007A31 RID: 31281 RVA: 0x0030FCBC File Offset: 0x0030DEBC
	public static AssetBundle AssetBundle
	{
		get
		{
			if (EnemyDatabase.m_assetBundle == null)
			{
				EnemyDatabase.m_assetBundle = ResourceManager.LoadAssetBundle("enemies_base_001");
			}
			return EnemyDatabase.m_assetBundle;
		}
	}

	// Token: 0x06007A32 RID: 31282 RVA: 0x0030FCE4 File Offset: 0x0030DEE4
	public override void DropReferences()
	{
		base.DropReferences();
	}

	// Token: 0x06007A33 RID: 31283 RVA: 0x0030FCEC File Offset: 0x0030DEEC
	public AIActor InternalGetByName(string name)
	{
		int i = 0;
		int count = this.Entries.Count;
		while (i < count)
		{
			EnemyDatabaseEntry enemyDatabaseEntry = this.Entries[i];
			if (enemyDatabaseEntry != null && enemyDatabaseEntry.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return enemyDatabaseEntry.GetPrefab<AIActor>();
			}
			i++;
		}
		return null;
	}

	// Token: 0x06007A34 RID: 31284 RVA: 0x0030FD44 File Offset: 0x0030DF44
	public AIActor InternalGetByGuid(string guid)
	{
		int i = 0;
		int count = this.Entries.Count;
		while (i < count)
		{
			EnemyDatabaseEntry enemyDatabaseEntry = this.Entries[i];
			if (enemyDatabaseEntry != null && enemyDatabaseEntry.myGuid == guid)
			{
				return enemyDatabaseEntry.GetPrefab<AIActor>();
			}
			i++;
		}
		return null;
	}

	// Token: 0x06007A35 RID: 31285 RVA: 0x0030FD9C File Offset: 0x0030DF9C
	public static void Unload()
	{
		EnemyDatabase.m_instance = null;
	}

	// Token: 0x06007A36 RID: 31286 RVA: 0x0030FDA4 File Offset: 0x0030DFA4
	public static AIActor GetOrLoadByName(string name)
	{
		return EnemyDatabase.Instance.InternalGetByName(name);
	}

	// Token: 0x06007A37 RID: 31287 RVA: 0x0030FDB4 File Offset: 0x0030DFB4
	public static AIActor GetOrLoadByGuid(string guid)
	{
		return EnemyDatabase.Instance.InternalGetByGuid(guid);
	}

	// Token: 0x06007A38 RID: 31288 RVA: 0x0030FDC4 File Offset: 0x0030DFC4
	public static EnemyDatabaseEntry GetEntry(string guid)
	{
		return EnemyDatabase.Instance.InternalGetDataByGuid(guid);
	}

	// Token: 0x04007C9B RID: 31899
	private static EnemyDatabase m_instance;

	// Token: 0x04007C9C RID: 31900
	private static AssetBundle m_assetBundle;
}
