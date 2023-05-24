using System;
using UnityEngine;

// Token: 0x020014F5 RID: 5365
public class FlowDatabase
{
	// Token: 0x06007A41 RID: 31297 RVA: 0x0030FFB0 File Offset: 0x0030E1B0
	public static DungeonFlow GetOrLoadByName(string name)
	{
		if (!FlowDatabase.m_assetBundle)
		{
			FlowDatabase.m_assetBundle = ResourceManager.LoadAssetBundle("flows_base_001");
		}
		string text = name;
		if (text.Contains("/"))
		{
			text = name.Substring(name.LastIndexOf("/") + 1);
		}
		DebugTime.RecordStartTime();
		DungeonFlow dungeonFlow = FlowDatabase.m_assetBundle.LoadAsset<DungeonFlow>(text);
		DebugTime.Log("AssetBundle.LoadAsset<DungeonFlow>({0})", new object[] { text });
		return dungeonFlow;
	}

	// Token: 0x04007CA4 RID: 31908
	private static AssetBundle m_assetBundle;
}
