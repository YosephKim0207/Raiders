using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016BF RID: 5823
public static class ResourceCache
{
	// Token: 0x06008768 RID: 34664 RVA: 0x00382878 File Offset: 0x00380A78
	public static UnityEngine.Object Acquire(string resourceName)
	{
		if (!ResourceCache.m_resourceCache.ContainsKey(resourceName))
		{
			ResourceCache.m_resourceCache.Add(resourceName, BraveResources.Load(resourceName, ".prefab"));
		}
		return ResourceCache.m_resourceCache[resourceName];
	}

	// Token: 0x06008769 RID: 34665 RVA: 0x003828AC File Offset: 0x00380AAC
	public static void ClearCache()
	{
		ResourceCache.m_resourceCache.Clear();
	}

	// Token: 0x04008C92 RID: 35986
	private static Dictionary<string, UnityEngine.Object> m_resourceCache = new Dictionary<string, UnityEngine.Object>();
}
