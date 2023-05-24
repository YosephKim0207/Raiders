using System;
using System.Collections;
using Dungeonator;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000EA7 RID: 3751
public class PlaceableAsyncSceneLoader : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06004F62 RID: 20322 RVA: 0x001B8614 File Offset: 0x001B6814
	public void ConfigureOnPlacement(RoomHandler room)
	{
		if (this.DoNoncombatSetup)
		{
			this.NoncombatSetup();
		}
		DebugTime.Log("PlaceableAsyncSceneLoader.LoadScene({0})", new object[] { this.asyncSceneName });
		if (this.asyncSceneName == "Foyer")
		{
			this.LoadBundledScene("Foyer", "foyer_002");
		}
		else if (this.asyncSceneName == "Foyer_Coop")
		{
			this.LoadBundledScene("Foyer_Coop", "foyer_003");
		}
		else
		{
			SceneManager.LoadScene(this.asyncSceneName, LoadSceneMode.Additive);
		}
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x001B86AC File Offset: 0x001B68AC
	private IEnumerator WaitForChunkLoaded(AsyncOperation loader)
	{
		while (!loader.isDone)
		{
			yield return null;
		}
		GameObject rootObject = GameObject.Find(this.asyncChunkIdentifier);
		rootObject.transform.position = base.transform.position;
		yield break;
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x001B86D0 File Offset: 0x001B68D0
	private void NoncombatSetup()
	{
		GameUIRoot.Instance.ForceHideGunPanel = true;
		GameUIRoot.Instance.ForceHideItemPanel = true;
	}

	// Token: 0x06004F65 RID: 20325 RVA: 0x001B86E8 File Offset: 0x001B68E8
	private void LoadBundledScene(string sceneName, string bundleName)
	{
		AssetBundle assetBundle = ResourceManager.LoadAssetBundle(bundleName);
		DebugTime.RecordStartTime();
		ResourceManager.LoadSceneFromBundle(assetBundle, LoadSceneMode.Additive);
		DebugTime.Log("Application.LoadLevel(foyer)", new object[0]);
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x001B8718 File Offset: 0x001B6918
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040046D2 RID: 18130
	public string asyncSceneName;

	// Token: 0x040046D3 RID: 18131
	public string asyncChunkIdentifier;

	// Token: 0x040046D4 RID: 18132
	public bool DoNoncombatSetup;
}
