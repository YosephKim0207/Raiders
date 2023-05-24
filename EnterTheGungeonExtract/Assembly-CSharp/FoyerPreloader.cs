using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012DF RID: 4831
public class FoyerPreloader : MonoBehaviour
{
	// Token: 0x06006C42 RID: 27714 RVA: 0x002A9E14 File Offset: 0x002A8014
	public void Awake()
	{
		if (FoyerPreloader.IsFirstLoadScreen)
		{
			this.LoadingLabel.gameObject.SetActive(false);
			this.LanguageManager.enabled = false;
			this.m_wasFirstLoadScreen = true;
			FoyerPreloader.IsFirstLoadScreen = false;
		}
		else if (FoyerPreloader.IsRatLoad)
		{
			this.Throbber.IsVisible = false;
			this.RatThrobber.IsVisible = true;
			FoyerPreloader.IsRatLoad = false;
		}
	}

	// Token: 0x06006C43 RID: 27715 RVA: 0x002A9E84 File Offset: 0x002A8084
	public void Update()
	{
		if (this.m_wasFirstLoadScreen && Time.frameCount >= 5 && !this.m_isLoading)
		{
			base.StartCoroutine(this.AsyncLoadFoyer());
			this.m_isLoading = true;
		}
	}

	// Token: 0x06006C44 RID: 27716 RVA: 0x002A9EBC File Offset: 0x002A80BC
	private IEnumerator AsyncLoadFoyer()
	{
		DebugTime.Log("FoyerLoader.AsyncLoadFoyer()", new object[0]);
		GameManager.AttemptSoundEngineInitializationAsync();
		yield return base.StartCoroutine(ResourceManager.InitAsync());
		DebugTime.RecordStartTime();
		GameManager targetManager = (BraveResources.Load("_GameManager", ".prefab") as GameObject).GetComponent<GameManager>();
		DebugTime.Log("Preloaded GameManager", new object[0]);
		yield return null;
		EnemyDatabase enemyDatabasePreload = EnemyDatabase.Instance;
		yield return null;
		EncounterDatabase encounterDatabasePreload = EncounterDatabase.Instance;
		yield return null;
		while (!GameManager.AUDIO_ENABLED)
		{
			yield return null;
		}
		if (this.m_wasFirstLoadScreen)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		AssetBundle assetBundle = ResourceManager.LoadAssetBundle("foyer_001");
		DebugTime.RecordStartTime();
		ResourceManager.LoadLevelFromBundle(assetBundle);
		DebugTime.Log("Application.LoadLevel(foyer)", new object[0]);
		if (this.m_wasFirstLoadScreen)
		{
			DebugTime.Log("Starting to destroy the load screen", new object[0]);
			int skipFrames = 3;
			for (int i = 0; i < skipFrames; i++)
			{
				yield return null;
			}
			DebugTime.Log("Finished destroying the load screen", new object[0]);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x04006949 RID: 26953
	public static bool IsFirstLoadScreen = true;

	// Token: 0x0400694A RID: 26954
	public static bool IsRatLoad;

	// Token: 0x0400694B RID: 26955
	public dfLabel LoadingLabel;

	// Token: 0x0400694C RID: 26956
	public dfLanguageManager LanguageManager;

	// Token: 0x0400694D RID: 26957
	public dfSprite Throbber;

	// Token: 0x0400694E RID: 26958
	public dfSprite RatThrobber;

	// Token: 0x0400694F RID: 26959
	private bool m_wasFirstLoadScreen;

	// Token: 0x04006950 RID: 26960
	private bool m_isLoading;
}
