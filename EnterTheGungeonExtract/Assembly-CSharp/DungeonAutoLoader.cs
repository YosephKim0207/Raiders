using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F6E RID: 3950
public class DungeonAutoLoader : MonoBehaviour
{
	// Token: 0x06005526 RID: 21798 RVA: 0x0020558C File Offset: 0x0020378C
	public void Awake()
	{
		if (GameManager.Instance.DungeonToAutoLoad)
		{
			UnityEngine.Object.Instantiate<Dungeon>(GameManager.Instance.DungeonToAutoLoad);
			GameManager.Instance.DungeonToAutoLoad = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
