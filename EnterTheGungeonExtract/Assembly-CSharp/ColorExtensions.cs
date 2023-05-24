using System;
using UnityEngine;

// Token: 0x02000367 RID: 871
public static class ColorExtensions
{
	// Token: 0x06000E07 RID: 3591 RVA: 0x00043068 File Offset: 0x00041268
	public static bool EqualsNonAlloc(this Color32 color, Color32 other)
	{
		return color.r == other.r && color.g == other.g && color.b == other.b && color.a == other.a;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x000430C4 File Offset: 0x000412C4
	public static Color SmoothStep(Color start, Color end, float t)
	{
		return new Color(Mathf.SmoothStep(start.r, end.r, t), Mathf.SmoothStep(start.g, end.g, t), Mathf.SmoothStep(start.b, end.b, t), Mathf.SmoothStep(start.a, end.a, t));
	}
}
