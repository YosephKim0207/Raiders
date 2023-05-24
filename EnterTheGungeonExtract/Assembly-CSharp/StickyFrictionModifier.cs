using System;
using UnityEngine;

// Token: 0x020016D6 RID: 5846
public class StickyFrictionModifier
{
	// Token: 0x06008803 RID: 34819 RVA: 0x0038633C File Offset: 0x0038453C
	public StickyFrictionModifier(float l, float m, bool falloff = true)
	{
		this.length = l * GameManager.Options.StickyFrictionMultiplier;
		this.magnitude = Mathf.Clamp01(m);
		this.usesFalloff = falloff;
	}

	// Token: 0x06008804 RID: 34820 RVA: 0x00386370 File Offset: 0x00384570
	public float GetCurrentMagnitude()
	{
		if (this.usesFalloff)
		{
			float num = this.elapsed / this.length;
			float num2 = Mathf.Clamp01(num * num * num);
			return Mathf.Lerp(this.magnitude, 1f, num2);
		}
		return this.magnitude;
	}

	// Token: 0x04008D3A RID: 36154
	public float length;

	// Token: 0x04008D3B RID: 36155
	public float magnitude;

	// Token: 0x04008D3C RID: 36156
	public float elapsed;

	// Token: 0x04008D3D RID: 36157
	public bool usesFalloff = true;
}
