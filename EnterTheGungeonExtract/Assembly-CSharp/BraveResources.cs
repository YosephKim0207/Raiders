using System;
using UnityEngine;

// Token: 0x020014EE RID: 5358
public class BraveResources
{
	// Token: 0x060079FD RID: 31229 RVA: 0x0030F0A8 File Offset: 0x0030D2A8
	public static UnityEngine.Object Load(string path, string extension = ".prefab")
	{
		if (BraveResources.m_assetBundle == null)
		{
			BraveResources.EnsureLoaded();
		}
		return BraveResources.m_assetBundle.LoadAsset<UnityEngine.Object>("assets/ResourcesBundle/" + path + extension);
	}

	// Token: 0x060079FE RID: 31230 RVA: 0x0030F0D8 File Offset: 0x0030D2D8
	public static UnityEngine.Object Load(string path, Type type, string extension = ".prefab")
	{
		if (BraveResources.m_assetBundle == null)
		{
			BraveResources.EnsureLoaded();
		}
		return BraveResources.m_assetBundle.LoadAsset("assets/ResourcesBundle/" + path + extension, type);
	}

	// Token: 0x060079FF RID: 31231 RVA: 0x0030F108 File Offset: 0x0030D308
	public static T Load<T>(string path, string extension = ".prefab") where T : UnityEngine.Object
	{
		if (BraveResources.m_assetBundle == null)
		{
			BraveResources.EnsureLoaded();
		}
		return BraveResources.m_assetBundle.LoadAsset<T>("assets/ResourcesBundle/" + path + extension);
	}

	// Token: 0x06007A00 RID: 31232 RVA: 0x0030F138 File Offset: 0x0030D338
	public static void EnsureLoaded()
	{
		if (BraveResources.m_assetBundle == null)
		{
			BraveResources.m_assetBundle = ResourceManager.LoadAssetBundle("brave_resources_001");
		}
	}

	// Token: 0x04007C7E RID: 31870
	private static AssetBundle m_assetBundle;
}
