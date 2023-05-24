using System;
using UnityEngine;

// Token: 0x02001505 RID: 5381
public class PrefabDatabase : ScriptableObject
{
	// Token: 0x17001214 RID: 4628
	// (get) Token: 0x06007AC9 RID: 31433 RVA: 0x00313C3C File Offset: 0x00311E3C
	public static PrefabDatabase Instance
	{
		get
		{
			if (PrefabDatabase.m_instance == null)
			{
				DebugTime.RecordStartTime();
				PrefabDatabase.m_instance = PrefabDatabase.AssetBundle.LoadAsset<PrefabDatabase>("PrefabDatabase");
				DebugTime.Log("Loading PrefabDatabase from AssetBundle", new object[0]);
			}
			return PrefabDatabase.m_instance;
		}
	}

	// Token: 0x17001215 RID: 4629
	// (get) Token: 0x06007ACA RID: 31434 RVA: 0x00313C7C File Offset: 0x00311E7C
	public static bool HasInstance
	{
		get
		{
			return PrefabDatabase.m_instance != null;
		}
	}

	// Token: 0x17001216 RID: 4630
	// (get) Token: 0x06007ACB RID: 31435 RVA: 0x00313C8C File Offset: 0x00311E8C
	public static AssetBundle AssetBundle
	{
		get
		{
			if (PrefabDatabase.m_assetBundle == null)
			{
				PrefabDatabase.m_assetBundle = ResourceManager.LoadAssetBundle("shared_base_001");
			}
			return PrefabDatabase.m_assetBundle;
		}
	}

	// Token: 0x04007D50 RID: 32080
	public GameObject SuperReaper;

	// Token: 0x04007D51 RID: 32081
	public GameObject ResourcefulRatThief;

	// Token: 0x04007D52 RID: 32082
	private static PrefabDatabase m_instance;

	// Token: 0x04007D53 RID: 32083
	private static AssetBundle m_assetBundle;
}
