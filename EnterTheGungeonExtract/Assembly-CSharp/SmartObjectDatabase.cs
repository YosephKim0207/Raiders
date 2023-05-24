using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001508 RID: 5384
[fiInspectorOnly]
public abstract class SmartObjectDatabase<T, U> : ScriptableObject where T : UnityEngine.Object where U : DatabaseEntry
{
	// Token: 0x06007ADC RID: 31452 RVA: 0x00313EEC File Offset: 0x003120EC
	public T InternalGetByName(string name)
	{
		U u = this.Entries.Find((U obj) => obj != null && obj.name.Equals(name, StringComparison.OrdinalIgnoreCase));
		return (u == null) ? ((T)((object)null)) : u.GetPrefab<T>();
	}

	// Token: 0x06007ADD RID: 31453 RVA: 0x00313F44 File Offset: 0x00312144
	public T InternalGetByGuid(string guid)
	{
		U u = this.Entries.Find((U ds) => ds != null && ds.myGuid == guid);
		return (u == null) ? ((T)((object)null)) : u.GetPrefab<T>();
	}

	// Token: 0x06007ADE RID: 31454 RVA: 0x00313F9C File Offset: 0x0031219C
	public U InternalGetDataByGuid(string guid)
	{
		return this.Entries.Find((U ds) => ds != null && ds.myGuid == guid);
	}

	// Token: 0x06007ADF RID: 31455 RVA: 0x00313FD0 File Offset: 0x003121D0
	public void DropReferences()
	{
		for (int i = 0; i < this.Entries.Count; i++)
		{
			U u = this.Entries[i];
			u.DropReference();
		}
	}

	// Token: 0x04007D5A RID: 32090
	[InspectorCollectionRotorzFlags(DisableReordering = true, ShowIndices = true)]
	public List<T> Objects;

	// Token: 0x04007D5B RID: 32091
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	[FormerlySerializedAs("GoodObjects")]
	public List<U> Entries;
}
