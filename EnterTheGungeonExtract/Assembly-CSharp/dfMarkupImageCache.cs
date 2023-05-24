using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class dfMarkupImageCache
{
	// Token: 0x06001B8C RID: 7052 RVA: 0x00081F58 File Offset: 0x00080158
	public static void Clear()
	{
		dfMarkupImageCache.cache.Clear();
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x00081F64 File Offset: 0x00080164
	public static void Load(string name, Texture image)
	{
		dfMarkupImageCache.cache[name.ToLowerInvariant()] = image;
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x00081F78 File Offset: 0x00080178
	public static void Unload(string name)
	{
		dfMarkupImageCache.cache.Remove(name.ToLowerInvariant());
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x00081F8C File Offset: 0x0008018C
	public static Texture Load(string path)
	{
		path = path.ToLowerInvariant();
		if (dfMarkupImageCache.cache.ContainsKey(path))
		{
			return dfMarkupImageCache.cache[path];
		}
		Texture texture = BraveResources.Load(path, ".prefab") as Texture;
		if (texture != null)
		{
			dfMarkupImageCache.cache[path] = texture;
		}
		return texture;
	}

	// Token: 0x0400158B RID: 5515
	private static Dictionary<string, Texture> cache = new Dictionary<string, Texture>();
}
