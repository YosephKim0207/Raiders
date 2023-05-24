using System;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x02000841 RID: 2113
	[AddComponentMenu("Path-o-logical/PoolManager/Pre-Runtime Pool Item")]
	public class PreRuntimePoolItem : MonoBehaviour
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x000EF954 File Offset: 0x000EDB54
		private void Start()
		{
			SpawnPool spawnPool;
			if (!PoolManager.Pools.TryGetValue(this.poolName, out spawnPool))
			{
				string text = "PreRuntimePoolItem Error ('{0}'): No pool with the name '{1}' exists! Create one using the PoolManager Inspector interface or PoolManager.CreatePool().See the online docs for more information at http://docs.poolmanager.path-o-logical.com";
				Debug.LogError(string.Format(text, base.name, this.poolName));
				return;
			}
			spawnPool.Add(base.transform, this.prefabName, this.despawnOnStart, !this.doNotReparent);
		}

		// Token: 0x04001F2D RID: 7981
		public string poolName = string.Empty;

		// Token: 0x04001F2E RID: 7982
		public string prefabName = string.Empty;

		// Token: 0x04001F2F RID: 7983
		public bool despawnOnStart = true;

		// Token: 0x04001F30 RID: 7984
		public bool doNotReparent;
	}
}
