using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001506 RID: 5382
public class ResourceManager
{
	// Token: 0x06007ACD RID: 31437 RVA: 0x00313CBC File Offset: 0x00311EBC
	public static void Init()
	{
		if (ResourceManager.LoadedBundles != null)
		{
			return;
		}
		ResourceManager.LoadedBundles = new Dictionary<string, AssetBundle>();
		for (int i = 0; i < ResourceManager.BundlePrereqs.Length; i++)
		{
			ResourceManager.LoadAssetBundle(ResourceManager.BundlePrereqs[i]);
		}
	}

	// Token: 0x06007ACE RID: 31438 RVA: 0x00313D04 File Offset: 0x00311F04
	public static IEnumerator InitAsync()
	{
		if (ResourceManager.LoadedBundles != null)
		{
			yield break;
		}
		ResourceManager.LoadedBundles = new Dictionary<string, AssetBundle>();
		for (int i = 0; i < ResourceManager.BundlePrereqs.Length; i++)
		{
			ResourceManager.LoadAssetBundle(ResourceManager.BundlePrereqs[i]);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007ACF RID: 31439 RVA: 0x00313D18 File Offset: 0x00311F18
	public static AssetBundle LoadAssetBundle(string path)
	{
		if (ResourceManager.LoadedBundles == null)
		{
			ResourceManager.Init();
		}
		AssetBundle assetBundle;
		if (ResourceManager.LoadedBundles.TryGetValue(path, out assetBundle))
		{
			return assetBundle;
		}
		string text = Path.Combine(Application.streamingAssetsPath, Path.Combine("Assets/", path));
		DebugTime.RecordStartTime();
		assetBundle = AssetBundle.LoadFromFile(text);
		DebugTime.Log("AssetBundle.LoadFromFile({0})", new object[] { path });
		ResourceManager.LoadedBundles.Add(path, assetBundle);
		return assetBundle;
	}

	// Token: 0x06007AD0 RID: 31440 RVA: 0x00313D8C File Offset: 0x00311F8C
	public static void LoadSceneFromBundle(AssetBundle assetBundle, LoadSceneMode mode)
	{
		SceneManager.LoadScene(ResourceManager.GetSceneName(assetBundle), mode);
	}

	// Token: 0x06007AD1 RID: 31441 RVA: 0x00313D9C File Offset: 0x00311F9C
	public static AsyncOperation LoadSceneAsyncFromBundle(AssetBundle assetBundle, LoadSceneMode mode)
	{
		return SceneManager.LoadSceneAsync(ResourceManager.GetSceneName(assetBundle), mode);
	}

	// Token: 0x06007AD2 RID: 31442 RVA: 0x00313DAC File Offset: 0x00311FAC
	public static void LoadLevelFromBundle(AssetBundle assetBundle)
	{
		Application.LoadLevel(ResourceManager.GetSceneName(assetBundle));
	}

	// Token: 0x06007AD3 RID: 31443 RVA: 0x00313DBC File Offset: 0x00311FBC
	private static string GetSceneName(AssetBundle assetBundle)
	{
		return assetBundle.GetAllScenePaths()[0];
	}

	// Token: 0x04007D54 RID: 32084
	private static Dictionary<string, AssetBundle> LoadedBundles;

	// Token: 0x04007D55 RID: 32085
	private static string[] BundlePrereqs = new string[] { "shared_base_001", "shared_auto_001", "shared_auto_002", "brave_resources_001", "enemies_base_001", "dungeons/base_foyer" };
}
