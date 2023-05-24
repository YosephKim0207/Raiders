using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001631 RID: 5681
public static class GlobalDispersalParticleManager
{
	// Token: 0x060084A2 RID: 33954 RVA: 0x0036A228 File Offset: 0x00368428
	public static ParticleSystem GetSystemForPrefab(GameObject prefab)
	{
		if (GlobalDispersalParticleManager.PrefabToSystemMap == null)
		{
			GlobalDispersalParticleManager.PrefabToSystemMap = new Dictionary<GameObject, ParticleSystem>();
		}
		if (GlobalDispersalParticleManager.PrefabToSystemMap.ContainsKey(prefab))
		{
			return GlobalDispersalParticleManager.PrefabToSystemMap[prefab];
		}
		ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>();
		GlobalDispersalParticleManager.PrefabToSystemMap.Add(prefab, component);
		return component;
	}

	// Token: 0x060084A3 RID: 33955 RVA: 0x0036A288 File Offset: 0x00368488
	public static void Clear()
	{
		if (GlobalDispersalParticleManager.PrefabToSystemMap != null)
		{
			GlobalDispersalParticleManager.PrefabToSystemMap.Clear();
		}
	}

	// Token: 0x04008854 RID: 34900
	public static Dictionary<GameObject, ParticleSystem> PrefabToSystemMap;
}
