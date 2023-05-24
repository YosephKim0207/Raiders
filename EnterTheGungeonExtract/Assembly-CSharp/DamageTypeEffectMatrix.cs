using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E5D RID: 3677
public class DamageTypeEffectMatrix : ScriptableObject
{
	// Token: 0x06004E50 RID: 20048 RVA: 0x001B14C8 File Offset: 0x001AF6C8
	public DamageTypeEffectDefinition GetDefinitionForType(CoreDamageTypes typeFlags)
	{
		for (int i = 0; i < this.definitions.Count; i++)
		{
			if ((typeFlags & this.definitions[i].damageType) == this.definitions[i].damageType)
			{
				return this.definitions[i];
			}
		}
		return null;
	}

	// Token: 0x040044B4 RID: 17588
	public List<DamageTypeEffectDefinition> definitions;
}
