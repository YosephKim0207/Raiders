using System;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public static class dfNumberExtensions
{
	// Token: 0x0600165A RID: 5722 RVA: 0x0006A0A4 File Offset: 0x000682A4
	public static int Quantize(this int value, int stepSize)
	{
		if (stepSize <= 0)
		{
			return value;
		}
		return value / stepSize * stepSize;
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x0006A0B4 File Offset: 0x000682B4
	public static float Quantize(this float value, float stepSize)
	{
		if (stepSize <= 0f)
		{
			return value;
		}
		return Mathf.Floor(value / stepSize) * stepSize;
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x0006A0D0 File Offset: 0x000682D0
	public static float Quantize(this float value, float stepSize, VectorConversions conversionMethod)
	{
		if (stepSize <= 0f)
		{
			return value;
		}
		if (conversionMethod == VectorConversions.Floor)
		{
			return Mathf.Floor(value / stepSize) * stepSize;
		}
		if (conversionMethod == VectorConversions.Ceil)
		{
			return Mathf.Ceil(value / stepSize) * stepSize;
		}
		if (conversionMethod != VectorConversions.Round)
		{
			return Mathf.Round(value / stepSize) * stepSize;
		}
		return Mathf.Round(value / stepSize) * stepSize;
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x0006A130 File Offset: 0x00068330
	public static int RoundToNearest(this int value, int stepSize)
	{
		if (stepSize <= 0)
		{
			return value;
		}
		int num = value / stepSize * stepSize;
		int num2 = value % stepSize;
		if (num2 >= stepSize / 2)
		{
			return num + stepSize;
		}
		return num;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x0006A160 File Offset: 0x00068360
	public static float RoundToNearest(this float value, float stepSize)
	{
		if (stepSize <= 0f)
		{
			return value;
		}
		float num = Mathf.Floor(value / stepSize) * stepSize;
		float num2 = value - stepSize * Mathf.Floor(value / stepSize);
		if (num2 >= stepSize * 0.5f - 1E-45f)
		{
			return num + stepSize;
		}
		return num;
	}
}
