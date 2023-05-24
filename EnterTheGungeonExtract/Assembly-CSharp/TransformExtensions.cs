using System;
using UnityEngine;

// Token: 0x02000369 RID: 873
public static class TransformExtensions
{
	// Token: 0x06000E17 RID: 3607 RVA: 0x000433B8 File Offset: 0x000415B8
	public static Vector2 PositionVector2(this Transform t)
	{
		return new Vector2(t.position.x, t.position.y);
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000433E8 File Offset: 0x000415E8
	public static void MovePixelsWorld(this Transform t, IntVector2 offset)
	{
		t.MovePixelsWorld(offset.x, offset.y);
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x00043400 File Offset: 0x00041600
	public static void MovePixelsWorld(this Transform t, int x, int y)
	{
		t.position += new Vector3((float)x * 0.0625f, (float)y * 0.0625f, 0f);
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00043430 File Offset: 0x00041630
	public static void MovePixelsWorld(this Transform t, int x, int y, int z)
	{
		t.position += new Vector3((float)x * 0.0625f, (float)y * 0.0625f, (float)z * 0.0625f);
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x00043460 File Offset: 0x00041660
	public static void MovePixelsLocal(this Transform t, IntVector2 offset)
	{
		t.MovePixelsLocal(offset.x, offset.y);
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00043478 File Offset: 0x00041678
	public static void MovePixelsLocal(this Transform t, int x, int y)
	{
		t.localPosition += new Vector3((float)x * 0.0625f, (float)y * 0.0625f, 0f);
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x000434A8 File Offset: 0x000416A8
	public static void MovePixelsLocal(this Transform t, int x, int y, int z)
	{
		t.localPosition += new Vector3((float)x * 0.0625f, (float)y * 0.0625f, (float)z * 0.0625f);
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000434D8 File Offset: 0x000416D8
	public static Transform GetFirstLeafChild(this Transform t)
	{
		Transform transform = t;
		while (transform.childCount > 0)
		{
			transform = transform.GetChild(0);
		}
		return transform;
	}
}
