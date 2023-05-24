using System;
using UnityEngine;

// Token: 0x020010AD RID: 4269
public class ActiveKnockbackData
{
	// Token: 0x06005E28 RID: 24104 RVA: 0x00241D84 File Offset: 0x0023FF84
	public ActiveKnockbackData(Vector2 k, float t, bool i)
	{
		this.knockback = k;
		this.initialKnockback = k;
		this.curveTime = t;
		this.immutable = i;
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x00241DA8 File Offset: 0x0023FFA8
	public ActiveKnockbackData(Vector2 k, AnimationCurve curve, float t, bool i)
	{
		this.knockback = k;
		this.initialKnockback = k;
		this.curveFalloff = curve;
		this.curveTime = t;
		this.immutable = i;
	}

	// Token: 0x04005835 RID: 22581
	public Vector2 initialKnockback;

	// Token: 0x04005836 RID: 22582
	public Vector2 knockback;

	// Token: 0x04005837 RID: 22583
	public float elapsedTime;

	// Token: 0x04005838 RID: 22584
	public float curveTime;

	// Token: 0x04005839 RID: 22585
	public AnimationCurve curveFalloff;

	// Token: 0x0400583A RID: 22586
	public GameObject sourceObject;

	// Token: 0x0400583B RID: 22587
	public bool immutable;
}
