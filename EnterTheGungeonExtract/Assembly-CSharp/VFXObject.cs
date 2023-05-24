using System;
using UnityEngine;

// Token: 0x0200184D RID: 6221
[Serializable]
public class VFXObject
{
	// Token: 0x04009AB0 RID: 39600
	public GameObject effect;

	// Token: 0x04009AB1 RID: 39601
	public bool orphaned;

	// Token: 0x04009AB2 RID: 39602
	public bool attached = true;

	// Token: 0x04009AB3 RID: 39603
	public bool persistsOnDeath;

	// Token: 0x04009AB4 RID: 39604
	public bool usesZHeight;

	// Token: 0x04009AB5 RID: 39605
	public float zHeight;

	// Token: 0x04009AB6 RID: 39606
	public VFXAlignment alignment;

	// Token: 0x04009AB7 RID: 39607
	[HideInInspector]
	public bool destructible;
}
