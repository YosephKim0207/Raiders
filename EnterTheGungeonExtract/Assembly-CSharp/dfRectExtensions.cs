using System;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public static class dfRectExtensions
{
	// Token: 0x0600166F RID: 5743 RVA: 0x0006A63C File Offset: 0x0006883C
	public static RectOffset ConstrainPadding(this RectOffset borders)
	{
		if (borders == null)
		{
			return new RectOffset();
		}
		borders.left = Mathf.Max(0, borders.left);
		borders.right = Mathf.Max(0, borders.right);
		borders.top = Mathf.Max(0, borders.top);
		borders.bottom = Mathf.Max(0, borders.bottom);
		return borders;
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x0006A6A0 File Offset: 0x000688A0
	public static bool IsEmpty(this Rect rect)
	{
		return rect.xMin == rect.xMax || rect.yMin == rect.yMax;
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x0006A6C8 File Offset: 0x000688C8
	public static Rect Intersection(this Rect a, Rect b)
	{
		if (!a.Intersects(b))
		{
			return default(Rect);
		}
		float num = Mathf.Max(a.xMin, b.xMin);
		float num2 = Mathf.Min(a.xMax, b.xMax);
		float num3 = Mathf.Max(a.yMin, b.yMin);
		float num4 = Mathf.Min(a.yMax, b.yMax);
		return Rect.MinMaxRect(num, num3, num2, num4);
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x0006A748 File Offset: 0x00068948
	public static Rect Union(this Rect a, Rect b)
	{
		float num = Mathf.Min(a.xMin, b.xMin);
		float num2 = Mathf.Max(a.xMax, b.xMax);
		float num3 = Mathf.Min(a.yMin, b.yMin);
		float num4 = Mathf.Max(a.yMax, b.yMax);
		return Rect.MinMaxRect(num, num3, num2, num4);
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x0006A7B0 File Offset: 0x000689B0
	public static bool Contains(this Rect rect, Rect other)
	{
		bool flag = rect.x <= other.x;
		bool flag2 = rect.x + rect.width >= other.x + other.width;
		bool flag3 = rect.yMin <= other.yMin;
		bool flag4 = rect.y + rect.height >= other.y + other.height;
		return flag && flag2 && flag3 && flag4;
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0006A844 File Offset: 0x00068A44
	public static bool Intersects(this Rect rect, Rect other)
	{
		bool flag = rect.xMax < other.xMin || rect.yMax < other.yMin || rect.xMin > other.xMax || rect.yMin > other.yMax;
		return !flag;
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x0006A8A4 File Offset: 0x00068AA4
	public static Rect RoundToInt(this Rect rect)
	{
		return new Rect((float)Mathf.RoundToInt(rect.x), (float)Mathf.RoundToInt(rect.y), (float)Mathf.RoundToInt(rect.width), (float)Mathf.RoundToInt(rect.height));
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x0006A8E0 File Offset: 0x00068AE0
	public static string Debug(this Rect rect)
	{
		return string.Format("[{0},{1},{2},{3}]", new object[] { rect.xMin, rect.yMin, rect.xMax, rect.yMax });
	}
}
