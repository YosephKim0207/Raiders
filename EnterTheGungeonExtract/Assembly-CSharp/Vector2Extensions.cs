using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
public static class Vector2Extensions
{
	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00043504 File Offset: 0x00041704
	public static Vector2 min
	{
		get
		{
			return new Vector2(float.MinValue, float.MinValue);
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00043518 File Offset: 0x00041718
	public static Vector2 max
	{
		get
		{
			return new Vector2(float.MaxValue, float.MaxValue);
		}
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0004352C File Offset: 0x0004172C
	public static bool Approximately(this Vector2 vector, Vector2 other)
	{
		return Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y);
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0004355C File Offset: 0x0004175C
	public static float ComponentProduct(this Vector2 vector)
	{
		return vector.x * vector.y;
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00043570 File Offset: 0x00041770
	public static Vector2 WithX(this Vector2 vector, float x)
	{
		return new Vector2(x, vector.y);
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00043580 File Offset: 0x00041780
	public static Vector2 WithY(this Vector2 vector, float y)
	{
		return new Vector2(vector.x, y);
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00043590 File Offset: 0x00041790
	public static Vector2 Rotate(this Vector2 v, float degrees)
	{
		float num = Mathf.Sin(degrees * 0.017453292f);
		float num2 = Mathf.Cos(degrees * 0.017453292f);
		float x = v.x;
		float y = v.y;
		v.x = num2 * x - num * y;
		v.y = num * x + num2 * y;
		return v;
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x000435E4 File Offset: 0x000417E4
	public static Vector4 ToVector4(this Vector2 vector)
	{
		return new Vector4(vector.x, vector.y, 0f, 0f);
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x00043604 File Offset: 0x00041804
	public static IntVector2 ToIntVector2(this Vector2 vector, VectorConversions convertMethod = VectorConversions.Round)
	{
		if (convertMethod == VectorConversions.Ceil)
		{
			return new IntVector2(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y));
		}
		if (convertMethod == VectorConversions.Floor)
		{
			return new IntVector2(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
		}
		return new IntVector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x00043678 File Offset: 0x00041878
	public static Vector3 ToVector3XUp(this Vector2 vector, float x = 0f)
	{
		return new Vector3(x, vector.x, vector.y);
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x00043690 File Offset: 0x00041890
	public static Vector3 ToVector3YUp(this Vector2 vector, float y = 0f)
	{
		return new Vector3(vector.x, y, vector.y);
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x000436A8 File Offset: 0x000418A8
	public static Vector3 ToVector3ZUp(this Vector2 vector, float z = 0f)
	{
		return new Vector3(vector.x, vector.y, z);
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x000436C0 File Offset: 0x000418C0
	public static Vector3 ToVector3ZisY(this Vector2 vector, float offset = 0f)
	{
		return new Vector3(vector.x, vector.y, vector.y + offset);
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x000436E0 File Offset: 0x000418E0
	public static bool IsWithin(this Vector2 vector, Vector2 min, Vector2 max)
	{
		return vector.x >= min.x && vector.x <= max.x && vector.y >= min.y && vector.y <= max.y;
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0004373C File Offset: 0x0004193C
	public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
	{
		return new Vector2(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y));
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00043778 File Offset: 0x00041978
	public static float ToAngle(this Vector2 vector)
	{
		return BraveMathCollege.Atan2Degrees(vector);
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00043780 File Offset: 0x00041980
	public static Vector2 Abs(this Vector2 vector)
	{
		return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x000437A0 File Offset: 0x000419A0
	public static float Cross(Vector2 a, Vector2 b)
	{
		return a.x * b.y - a.y * b.x;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x000437C4 File Offset: 0x000419C4
	public static Vector2 Cross(Vector2 a, float s)
	{
		return new Vector2(s * a.y, -s * a.x);
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x000437E0 File Offset: 0x000419E0
	public static Vector2 Cross(float s, Vector2 a)
	{
		return new Vector2(-s * a.y, s * a.x);
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x000437FC File Offset: 0x000419FC
	public static bool IsHorizontal(this Vector2 vector)
	{
		return Mathf.Abs(vector.x) > 0f && vector.y == 0f;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00043828 File Offset: 0x00041A28
	public static Vector2 SmoothStep(Vector2 from, Vector2 to, float t)
	{
		return new Vector2(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t));
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00043858 File Offset: 0x00041A58
	public static float SqrDistance(Vector2 a, Vector2 b)
	{
		double num = (double)(a.x - b.x);
		double num2 = (double)(a.y - b.y);
		return (float)(num * num + num2 * num2);
	}
}
