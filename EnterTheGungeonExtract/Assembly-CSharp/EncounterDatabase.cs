using System;
using UnityEngine;

// Token: 0x020014F0 RID: 5360
public class EncounterDatabase : AssetBundleDatabase<EncounterTrackable, EncounterDatabaseEntry>
{
	// Token: 0x170011FA RID: 4602
	// (get) Token: 0x06007A04 RID: 31236 RVA: 0x0030F1B8 File Offset: 0x0030D3B8
	public static EncounterDatabase Instance
	{
		get
		{
			if (EncounterDatabase.m_instance == null)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				int frameCount = Time.frameCount;
				EncounterDatabase.m_instance = EncounterDatabase.AssetBundle.LoadAsset<EncounterDatabase>("EncounterDatabase");
				DebugTime.Log(realtimeSinceStartup, frameCount, "Loading EncounterDatabase from AssetBundle", new object[0]);
			}
			return EncounterDatabase.m_instance;
		}
	}

	// Token: 0x170011FB RID: 4603
	// (get) Token: 0x06007A05 RID: 31237 RVA: 0x0030F20C File Offset: 0x0030D40C
	public static bool HasInstance
	{
		get
		{
			return EncounterDatabase.m_instance != null;
		}
	}

	// Token: 0x170011FC RID: 4604
	// (get) Token: 0x06007A06 RID: 31238 RVA: 0x0030F21C File Offset: 0x0030D41C
	public static AssetBundle AssetBundle
	{
		get
		{
			if (EncounterDatabase.m_assetBundle == null)
			{
				EncounterDatabase.m_assetBundle = ResourceManager.LoadAssetBundle("encounters_base_001");
			}
			return EncounterDatabase.m_assetBundle;
		}
	}

	// Token: 0x06007A07 RID: 31239 RVA: 0x0030F244 File Offset: 0x0030D444
	public static void Unload()
	{
		EncounterDatabase.m_instance = null;
	}

	// Token: 0x06007A08 RID: 31240 RVA: 0x0030F24C File Offset: 0x0030D44C
	public static EncounterDatabaseEntry GetEntry(string guid)
	{
		EncounterDatabaseEntry encounterDatabaseEntry = EncounterDatabase.Instance.InternalGetDataByGuid(guid);
		if (encounterDatabaseEntry != null && string.IsNullOrEmpty(encounterDatabaseEntry.ProxyEncounterGuid))
		{
			EncounterDatabase.Instance.InternalGetDataByGuid(encounterDatabaseEntry.ProxyEncounterGuid);
		}
		return encounterDatabaseEntry;
	}

	// Token: 0x06007A09 RID: 31241 RVA: 0x0030F290 File Offset: 0x0030D490
	public static bool IsProxy(string guid)
	{
		EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(guid);
		return entry != null && !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
	}

	// Token: 0x04007C7F RID: 31871
	public static EncounterDatabase m_instance;

	// Token: 0x04007C80 RID: 31872
	private static AssetBundle m_assetBundle;
}
