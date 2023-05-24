using System;
using UnityEngine;

// Token: 0x0200155B RID: 5467
public static class MagnificenceConstants
{
	// Token: 0x06007D40 RID: 32064 RVA: 0x0032A03C File Offset: 0x0032823C
	public static PickupObject.ItemQuality ModifyQualityByMagnificence(PickupObject.ItemQuality targetQuality, float CurrentMagnificence, float dChance, float cChance, float bChance)
	{
		if (targetQuality != PickupObject.ItemQuality.S && targetQuality != PickupObject.ItemQuality.A)
		{
			return targetQuality;
		}
		float num = 0.006260342f + 0.9935921f * Mathf.Exp(-1.626339f * CurrentMagnificence);
		num = Mathf.Clamp01(num);
		float num2 = UnityEngine.Random.value;
		if (UnityEngine.Random.value <= num)
		{
			return targetQuality;
		}
		float num3 = dChance + cChance + bChance;
		num2 = UnityEngine.Random.value * num3;
		if (num2 < dChance)
		{
			return PickupObject.ItemQuality.D;
		}
		if (num2 < dChance + cChance)
		{
			return PickupObject.ItemQuality.C;
		}
		if (num2 < dChance + cChance + bChance)
		{
			return PickupObject.ItemQuality.B;
		}
		return (UnityEngine.Random.value >= 0.5f) ? PickupObject.ItemQuality.B : PickupObject.ItemQuality.C;
	}

	// Token: 0x0400804E RID: 32846
	public const float COMMON_MAGNFIICENCE_ADJUSTMENT = 0f;

	// Token: 0x0400804F RID: 32847
	public const float A_MAGNIFICENCE_ADJUSTMENT = 1f;

	// Token: 0x04008050 RID: 32848
	public const float S_MAGNIFICENCE_ADJUSTMENT = 1f;
}
