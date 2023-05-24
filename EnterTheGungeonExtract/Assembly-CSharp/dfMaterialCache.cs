using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000413 RID: 1043
internal class dfMaterialCache
{
	// Token: 0x060017AF RID: 6063 RVA: 0x00071530 File Offset: 0x0006F730
	public static void ForceUpdate(Material BaseMaterial)
	{
		dfMaterialCache.Cache cache = null;
		if (dfMaterialCache.caches.TryGetValue(BaseMaterial, out cache))
		{
			cache.Clear();
			cache.Reset();
		}
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x00071560 File Offset: 0x0006F760
	public static Material Lookup(Material BaseMaterial)
	{
		if (BaseMaterial == null)
		{
			Debug.LogError("Cache lookup on null material");
			return null;
		}
		dfMaterialCache.Cache cache = null;
		if (!dfMaterialCache.caches.TryGetValue(BaseMaterial, out cache))
		{
			dfMaterialCache.Cache cache2 = new dfMaterialCache.Cache(BaseMaterial);
			dfMaterialCache.caches[BaseMaterial] = cache2;
			cache = cache2;
		}
		return cache.Obtain();
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000715B4 File Offset: 0x0006F7B4
	public static void Reset()
	{
		dfMaterialCache.Cache.ResetAll();
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000715BC File Offset: 0x0006F7BC
	public static void Clear()
	{
		dfMaterialCache.Cache.ClearAll();
		dfMaterialCache.caches.Clear();
	}

	// Token: 0x04001310 RID: 4880
	private static Dictionary<Material, dfMaterialCache.Cache> caches = new Dictionary<Material, dfMaterialCache.Cache>();

	// Token: 0x02000414 RID: 1044
	private class Cache
	{
		// Token: 0x060017B4 RID: 6068 RVA: 0x000715DC File Offset: 0x0006F7DC
		private Cache()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000715F8 File Offset: 0x0006F7F8
		public Cache(Material BaseMaterial)
		{
			this.baseMaterial = BaseMaterial;
			this.instances.Add(BaseMaterial);
			dfMaterialCache.Cache.cacheInstances.Add(this);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0007162C File Offset: 0x0006F82C
		public static void ClearAll()
		{
			for (int i = 0; i < dfMaterialCache.Cache.cacheInstances.Count; i++)
			{
				dfMaterialCache.Cache.cacheInstances[i].Clear();
			}
			dfMaterialCache.Cache.cacheInstances.Clear();
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00071670 File Offset: 0x0006F870
		public static void ResetAll()
		{
			for (int i = 0; i < dfMaterialCache.Cache.cacheInstances.Count; i++)
			{
				dfMaterialCache.Cache.cacheInstances[i].Reset();
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000716A8 File Offset: 0x0006F8A8
		public Material Obtain()
		{
			if (this.currentIndex < this.instances.Count)
			{
				return this.instances[this.currentIndex++];
			}
			this.currentIndex++;
			Material material = new Material(this.baseMaterial)
			{
				hideFlags = (HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset),
				name = string.Format("{0} (Copy {1})", this.baseMaterial.name, this.currentIndex)
			};
			this.instances.Add(material);
			return material;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00071740 File Offset: 0x0006F940
		public void Reset()
		{
			this.currentIndex = 0;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0007174C File Offset: 0x0006F94C
		public void Clear()
		{
			this.currentIndex = 0;
			for (int i = 1; i < this.instances.Count; i++)
			{
				Material material = this.instances[i];
				if (material != null)
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(material);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(material);
					}
				}
			}
			this.instances.Clear();
		}

		// Token: 0x04001311 RID: 4881
		private static List<dfMaterialCache.Cache> cacheInstances = new List<dfMaterialCache.Cache>();

		// Token: 0x04001312 RID: 4882
		private Material baseMaterial;

		// Token: 0x04001313 RID: 4883
		private List<Material> instances = new List<Material>(10);

		// Token: 0x04001314 RID: 4884
		private int currentIndex;
	}
}
