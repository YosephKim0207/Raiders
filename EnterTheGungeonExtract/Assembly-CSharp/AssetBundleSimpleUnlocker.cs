using System;
using UnityEngine;

// Token: 0x02001547 RID: 5447
public class AssetBundleSimpleUnlocker : MonoBehaviour
{
	// Token: 0x06007CA3 RID: 31907 RVA: 0x00323210 File Offset: 0x00321410
	public void OnGameStartup()
	{
		for (int i = 0; i < this.FlagsToSetUponLoad.Length; i++)
		{
			GameStatsManager.Instance.SetFlag(this.FlagsToSetUponLoad[i], true);
		}
	}

	// Token: 0x04007FA2 RID: 32674
	public GungeonFlags[] FlagsToSetUponLoad;
}
