using System;
using UnityEngine;

// Token: 0x02000E18 RID: 3608
public abstract class AppliedEffectBase : MonoBehaviour
{
	// Token: 0x06004C6F RID: 19567
	public abstract void Initialize(AppliedEffectBase source);

	// Token: 0x06004C70 RID: 19568
	public abstract void AddSelfToTarget(GameObject target);
}
