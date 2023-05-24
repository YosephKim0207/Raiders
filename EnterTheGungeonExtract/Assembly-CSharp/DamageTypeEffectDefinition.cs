using System;
using UnityEngine;

// Token: 0x02000E5E RID: 3678
[Serializable]
public class DamageTypeEffectDefinition
{
	// Token: 0x040044B5 RID: 17589
	[HideInInspector]
	[SerializeField]
	public string name = "dongs";

	// Token: 0x040044B6 RID: 17590
	public CoreDamageTypes damageType;

	// Token: 0x040044B7 RID: 17591
	public VFXPool wallDecals;
}
