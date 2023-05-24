using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001502 RID: 5378
public abstract class ObjectDatabase<T> : ScriptableObject where T : UnityEngine.Object
{
	// Token: 0x06007AB7 RID: 31415 RVA: 0x003134B4 File Offset: 0x003116B4
	public int InternalGetId(T obj)
	{
		return this.Objects.IndexOf(obj);
	}

	// Token: 0x06007AB8 RID: 31416 RVA: 0x003134C4 File Offset: 0x003116C4
	public T InternalGetById(int id)
	{
		if (id < 0 || id >= this.Objects.Count)
		{
			return (T)((object)null);
		}
		return this.Objects[id];
	}

	// Token: 0x06007AB9 RID: 31417 RVA: 0x003134F4 File Offset: 0x003116F4
	public T InternalGetByName(string name)
	{
		return this.Objects.Find((T obj) => obj != null && obj.name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x04007D4D RID: 32077
	public List<T> Objects;
}
