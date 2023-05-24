using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200036B RID: 875
public static class Vector3Extensions
{
	// Token: 0x06000E36 RID: 3638 RVA: 0x00043890 File Offset: 0x00041A90
	public static Vector3 WithX(this Vector3 vector, float x)
	{
		return new Vector3(x, vector.y, vector.z);
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x000438A8 File Offset: 0x00041AA8
	public static Vector3 WithY(this Vector3 vector, float y)
	{
		return new Vector3(vector.x, y, vector.z);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x000438C0 File Offset: 0x00041AC0
	public static Vector3 WithZ(this Vector3 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x000438D8 File Offset: 0x00041AD8
	public static Color WithAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x000438F8 File Offset: 0x00041AF8
	public static Vector3 RotateBy(this Vector3 vector, Quaternion rotation)
	{
		return rotation * vector;
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00043904 File Offset: 0x00041B04
	public static Vector4 ToVector4(this Vector3 vector)
	{
		return new Vector4(vector.x, vector.y, vector.z, 0f);
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00043928 File Offset: 0x00041B28
	public static Vector2 XY(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00043940 File Offset: 0x00041B40
	public static Vector2 YZ(this Vector3 vector)
	{
		return new Vector2(vector.y, vector.z);
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00043958 File Offset: 0x00041B58
	public static Vector2 XZ(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00043970 File Offset: 0x00041B70
	public static IntVector2 IntXY(this Vector3 vector, VectorConversions convert = VectorConversions.Round)
	{
		if (convert == VectorConversions.Floor)
		{
			return new IntVector2(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
		}
		if (convert == VectorConversions.Ceil)
		{
			return new IntVector2(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y));
		}
		if (convert == VectorConversions.Round)
		{
			return new IntVector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
		}
		BraveUtility.Log(string.Format("Called IntXY() with an unknown conversion type ({0})", convert), Color.red, BraveUtility.LogVerbosity.IMPORTANT);
		return IntVector2.Zero;
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00043A0C File Offset: 0x00041C0C
	public static bool IsWithin(this Vector3 vector, Vector3 min, Vector3 max)
	{
		return vector.x >= min.x && vector.x <= max.x && vector.y >= min.y && vector.y <= max.y;
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00043A68 File Offset: 0x00041C68
	public static CellData GetCell(this Vector2 vector)
	{
		return GameManager.Instance.Dungeon.data[vector.ToIntVector2(VectorConversions.Floor)];
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00043A88 File Offset: 0x00041C88
	public static CellData GetCell(this Vector3 vector)
	{
		if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(vector.IntXY(VectorConversions.Floor)))
		{
			return null;
		}
		return GameManager.Instance.Dungeon.data[vector.IntXY(VectorConversions.Floor)];
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00043AC8 File Offset: 0x00041CC8
	public static RoomHandler GetAbsoluteRoom(this Vector2 vector)
	{
		return GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(vector.ToIntVector2(VectorConversions.Floor));
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00043AE8 File Offset: 0x00041CE8
	public static RoomHandler GetAbsoluteRoom(this Vector3 vector)
	{
		return GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(vector.IntXY(VectorConversions.Floor));
	}
}
