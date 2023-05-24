using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020014EF RID: 5359
public class DungeonDatabase
{
	// Token: 0x06007A02 RID: 31234 RVA: 0x0030F164 File Offset: 0x0030D364
	public static Dungeon GetOrLoadByName(string name)
	{
		AssetBundle assetBundle = ResourceManager.LoadAssetBundle("dungeons/" + name.ToLower());
		DebugTime.RecordStartTime();
		Dungeon component = assetBundle.LoadAsset<GameObject>(name).GetComponent<Dungeon>();
		DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
		return component;
	}
}
