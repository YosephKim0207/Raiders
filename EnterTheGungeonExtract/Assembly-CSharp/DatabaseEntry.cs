using System;
using UnityEngine;

// Token: 0x0200150C RID: 5388
[Serializable]
public class DatabaseEntry
{
	// Token: 0x17001219 RID: 4633
	// (get) Token: 0x06007AE7 RID: 31463 RVA: 0x003140B0 File Offset: 0x003122B0
	public string name
	{
		get
		{
			return this.path.Substring(this.path.LastIndexOf('/') + 1).Replace(".prefab", string.Empty);
		}
	}

	// Token: 0x1700121A RID: 4634
	// (get) Token: 0x06007AE8 RID: 31464 RVA: 0x003140DC File Offset: 0x003122DC
	public bool HasLoadedPrefab
	{
		get
		{
			return this.loadedPrefab;
		}
	}

	// Token: 0x06007AE9 RID: 31465 RVA: 0x003140EC File Offset: 0x003122EC
	public T GetPrefab<T>() where T : UnityEngine.Object
	{
		if (!this.loadedPrefab)
		{
			if (!this.path.StartsWith("Assets/Resources/"))
			{
				Debug.LogErrorFormat("Trying to instantate an object that doesn't live in Resources! {0} {1} {2}", new object[] { this.myGuid, this.unityGuid, this.path });
				return (T)((object)null);
			}
			this.loadedPrefab = BraveResources.Load<T>(this.path.Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty), ".prefab");
		}
		return this.loadedPrefab as T;
	}

	// Token: 0x06007AEA RID: 31466 RVA: 0x0031419C File Offset: 0x0031239C
	public void DropReference()
	{
		this.loadedPrefab = null;
	}

	// Token: 0x04007D5F RID: 32095
	public string myGuid;

	// Token: 0x04007D60 RID: 32096
	public string unityGuid;

	// Token: 0x04007D61 RID: 32097
	public string path;

	// Token: 0x04007D62 RID: 32098
	[NonSerialized]
	private UnityEngine.Object loadedPrefab;
}
