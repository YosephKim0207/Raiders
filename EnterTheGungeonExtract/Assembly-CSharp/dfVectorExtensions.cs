using System;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public static class dfVectorExtensions
{
	// Token: 0x0600165F RID: 5727 RVA: 0x0006A1AC File Offset: 0x000683AC
	public static bool IsNaN(this Vector3 vector)
	{
		return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x0006A1E0 File Offset: 0x000683E0
	public static Vector3 ClampRotation(this Vector3 euler)
	{
		if (euler.x < 0f)
		{
			euler.x += 360f;
		}
		if (euler.x >= 360f)
		{
			euler.x -= 360f;
		}
		if (euler.y < 0f)
		{
			euler.y += 360f;
		}
		if (euler.y >= 360f)
		{
			euler.y -= 360f;
		}
		if (euler.z < 0f)
		{
			euler.z += 360f;
		}
		if (euler.z >= 360f)
		{
			euler.z -= 360f;
		}
		return euler;
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x0006A2C8 File Offset: 0x000684C8
	public static Vector2 Scale(this Vector2 vector, float x, float y)
	{
		return new Vector2(vector.x * x, vector.y * y);
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x0006A2E4 File Offset: 0x000684E4
	public static Vector3 Scale(this Vector3 vector, float x, float y, float z)
	{
		return new Vector3(vector.x * x, vector.y * y, vector.z * z);
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x0006A308 File Offset: 0x00068508
	public static Vector3 FloorToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.FloorToInt(vector.x), (float)Mathf.FloorToInt(vector.y), (float)Mathf.FloorToInt(vector.z));
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x0006A338 File Offset: 0x00068538
	public static Vector3 CeilToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.CeilToInt(vector.x), (float)Mathf.CeilToInt(vector.y), (float)Mathf.CeilToInt(vector.z));
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x0006A368 File Offset: 0x00068568
	public static Vector2 FloorToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.FloorToInt(vector.x), (float)Mathf.FloorToInt(vector.y));
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x0006A38C File Offset: 0x0006858C
	public static Vector2 CeilToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.CeilToInt(vector.x), (float)Mathf.CeilToInt(vector.y));
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0006A3B0 File Offset: 0x000685B0
	public static Vector3 RoundToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y), (float)Mathf.RoundToInt(vector.z));
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0006A3E0 File Offset: 0x000685E0
	public static Vector2 RoundToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y));
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0006A404 File Offset: 0x00068604
	public static Vector2 Quantize(this Vector2 vector, float discreteValue)
	{
		vector.x = (float)Mathf.RoundToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.RoundToInt(vector.y / discreteValue) * discreteValue;
		return vector;
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x0006A438 File Offset: 0x00068638
	public static Vector2 Quantize(this Vector2 vector, float discreteValue, VectorConversions conversionMethod)
	{
		if (conversionMethod == VectorConversions.Ceil)
		{
			vector.x = (float)Mathf.CeilToInt(vector.x / discreteValue) * discreteValue;
			vector.y = (float)Mathf.CeilToInt(vector.y / discreteValue) * discreteValue;
			return vector;
		}
		if (conversionMethod != VectorConversions.Floor)
		{
			return vector.Quantize(discreteValue);
		}
		vector.x = (float)Mathf.FloorToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.FloorToInt(vector.y / discreteValue) * discreteValue;
		return vector;
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x0006A4C4 File Offset: 0x000686C4
	public static Vector3 Quantize(this Vector3 vector, float discreteValue, VectorConversions conversionMethod)
	{
		if (conversionMethod == VectorConversions.Ceil)
		{
			vector.x = (float)Mathf.CeilToInt(vector.x / discreteValue) * discreteValue;
			vector.y = (float)Mathf.CeilToInt(vector.y / discreteValue) * discreteValue;
			vector.z = (float)Mathf.CeilToInt(vector.z / discreteValue) * discreteValue;
			return vector;
		}
		if (conversionMethod != VectorConversions.Floor)
		{
			return vector.Quantize(discreteValue);
		}
		vector.x = (float)Mathf.FloorToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.FloorToInt(vector.y / discreteValue) * discreteValue;
		vector.z = (float)Mathf.FloorToInt(vector.z / discreteValue) * discreteValue;
		return vector;
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x0006A580 File Offset: 0x00068780
	public static Vector3 Quantize(this Vector3 vector, float discreteValue)
	{
		vector.x = (float)Mathf.RoundToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.RoundToInt(vector.y / discreteValue) * discreteValue;
		vector.z = (float)Mathf.RoundToInt(vector.z / discreteValue) * discreteValue;
		return vector;
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x0006A5D8 File Offset: 0x000687D8
	public static Vector3 QuantizeFloor(this Vector3 vector, float discreteValue)
	{
		vector.x = (float)Mathf.FloorToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.FloorToInt(vector.y / discreteValue) * discreteValue;
		vector.z = (float)Mathf.FloorToInt(vector.z / discreteValue) * discreteValue;
		return vector;
	}
}
