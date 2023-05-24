using System;
using UnityEngine;

// Token: 0x020014ED RID: 5357
[Serializable]
public abstract class AssetBundleDatabaseEntry
{
	// Token: 0x170011F7 RID: 4599
	// (get) Token: 0x060079F8 RID: 31224
	public abstract AssetBundle assetBundle { get; }

	// Token: 0x170011F8 RID: 4600
	// (get) Token: 0x060079F9 RID: 31225 RVA: 0x0030F058 File Offset: 0x0030D258
	public string name
	{
		get
		{
			return this.path.Substring(this.path.LastIndexOf('/') + 1).Replace(".prefab", string.Empty);
		}
	}

	// Token: 0x170011F9 RID: 4601
	// (get) Token: 0x060079FA RID: 31226 RVA: 0x0030F084 File Offset: 0x0030D284
	public bool HasLoadedPrefab
	{
		get
		{
			return this.loadedPrefab;
		}
	}

	// Token: 0x060079FB RID: 31227 RVA: 0x0030F094 File Offset: 0x0030D294
	public virtual void DropReference()
	{
		this.loadedPrefab = null;
	}

	// Token: 0x04007C7A RID: 31866
	public string myGuid;

	// Token: 0x04007C7B RID: 31867
	public string unityGuid;

	// Token: 0x04007C7C RID: 31868
	public string path;

	// Token: 0x04007C7D RID: 31869
	[NonSerialized]
	protected UnityEngine.Object loadedPrefab;
}
