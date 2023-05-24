using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x020014EC RID: 5356
[fiInspectorOnly]
public abstract class AssetBundleDatabase<T, U> : ScriptableObject where T : UnityEngine.Object where U : AssetBundleDatabaseEntry
{
	// Token: 0x060079F5 RID: 31221 RVA: 0x0030EFA4 File Offset: 0x0030D1A4
	public U InternalGetDataByGuid(string guid)
	{
		int i = 0;
		int count = this.Entries.Count;
		while (i < count)
		{
			U u = this.Entries[i];
			if (u != null && u.myGuid == guid)
			{
				return u;
			}
			i++;
		}
		return (U)((object)null);
	}

	// Token: 0x060079F6 RID: 31222 RVA: 0x0030F008 File Offset: 0x0030D208
	public virtual void DropReferences()
	{
		int i = 0;
		int count = this.Entries.Count;
		while (i < count)
		{
			U u = this.Entries[i];
			u.DropReference();
			i++;
		}
	}

	// Token: 0x04007C79 RID: 31865
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<U> Entries;
}
