using System;
using UnityEngine;

// Token: 0x02000CF2 RID: 3314
public class PickupIdentifierAttribute : PropertyAttribute, DatabaseIdentifierAttribute
{
	// Token: 0x0600463F RID: 17983 RVA: 0x0016D33C File Offset: 0x0016B53C
	public PickupIdentifierAttribute()
	{
		this.PickupType = typeof(PickupObject);
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x0016D354 File Offset: 0x0016B554
	public PickupIdentifierAttribute(Type type)
	{
		this.PickupType = type;
	}

	// Token: 0x040038C3 RID: 14531
	public Type PickupType;
}
