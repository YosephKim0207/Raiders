using System;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public static class dfPivotExtensions
{
	// Token: 0x060011C7 RID: 4551 RVA: 0x00052BE0 File Offset: 0x00050DE0
	public static Vector2 AsOffset(this dfPivotPoint pivot)
	{
		switch (pivot)
		{
		case dfPivotPoint.TopLeft:
			return Vector2.zero;
		case dfPivotPoint.TopCenter:
			return new Vector2(0.5f, 0f);
		case dfPivotPoint.TopRight:
			return new Vector2(1f, 0f);
		case dfPivotPoint.MiddleLeft:
			return new Vector2(0f, 0.5f);
		case dfPivotPoint.MiddleCenter:
			return new Vector2(0.5f, 0.5f);
		case dfPivotPoint.MiddleRight:
			return new Vector2(1f, 0.5f);
		case dfPivotPoint.BottomLeft:
			return new Vector2(0f, 1f);
		case dfPivotPoint.BottomCenter:
			return new Vector2(0.5f, 1f);
		case dfPivotPoint.BottomRight:
			return new Vector2(1f, 1f);
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00052CA8 File Offset: 0x00050EA8
	public static Vector3 TransformToCenter(this dfPivotPoint pivot, Vector2 size)
	{
		switch (pivot)
		{
		case dfPivotPoint.TopLeft:
			return new Vector2(0.5f * size.x, 0.5f * -size.y);
		case dfPivotPoint.TopCenter:
			return new Vector2(0f, 0.5f * -size.y);
		case dfPivotPoint.TopRight:
			return new Vector2(0.5f * -size.x, 0.5f * -size.y);
		case dfPivotPoint.MiddleLeft:
			return new Vector2(0.5f * size.x, 0f);
		case dfPivotPoint.MiddleCenter:
			return new Vector2(0f, 0f);
		case dfPivotPoint.MiddleRight:
			return new Vector2(0.5f * -size.x, 0f);
		case dfPivotPoint.BottomLeft:
			return new Vector2(0.5f * size.x, 0.5f * size.y);
		case dfPivotPoint.BottomCenter:
			return new Vector2(0f, 0.5f * size.y);
		case dfPivotPoint.BottomRight:
			return new Vector2(0.5f * -size.x, 0.5f * size.y);
		default:
			throw new Exception(string.Concat(new object[]
			{
				"Unhandled ",
				pivot.GetType().Name,
				" value: ",
				pivot
			}));
		}
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00052E48 File Offset: 0x00051048
	public static Vector3 UpperLeftToTransform(this dfPivotPoint pivot, Vector2 size)
	{
		return pivot.TransformToUpperLeft(size).Scale(-1f, -1f, 1f);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00052E68 File Offset: 0x00051068
	public static Vector3 TransformToUpperLeft(this dfPivotPoint pivot, Vector2 size)
	{
		switch (pivot)
		{
		case dfPivotPoint.TopLeft:
			return new Vector2(0f, 0f);
		case dfPivotPoint.TopCenter:
			return new Vector2(0.5f * -size.x, 0f);
		case dfPivotPoint.TopRight:
			return new Vector2(-size.x, 0f);
		case dfPivotPoint.MiddleLeft:
			return new Vector2(0f, 0.5f * size.y);
		case dfPivotPoint.MiddleCenter:
			return new Vector2(0.5f * -size.x, 0.5f * size.y);
		case dfPivotPoint.MiddleRight:
			return new Vector2(-size.x, 0.5f * size.y);
		case dfPivotPoint.BottomLeft:
			return new Vector2(0f, size.y);
		case dfPivotPoint.BottomCenter:
			return new Vector2(0.5f * -size.x, size.y);
		case dfPivotPoint.BottomRight:
			return new Vector2(-size.x, size.y);
		default:
			throw new Exception(string.Concat(new object[]
			{
				"Unhandled ",
				pivot.GetType().Name,
				" value: ",
				pivot
			}));
		}
	}
}
