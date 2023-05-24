using System;
using UnityEngine;

// Token: 0x02000BDF RID: 3039
public static class tk2dSpriteGeomGen
{
	// Token: 0x0600404D RID: 16461 RVA: 0x001468A4 File Offset: 0x00144AA4
	public static void SetSpriteColors(Color32[] dest, int offset, int numVertices, Color c, bool premulAlpha)
	{
		if (premulAlpha)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		Color32 color = c;
		for (int i = 0; i < numVertices; i++)
		{
			dest[offset + i] = color;
		}
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x00146920 File Offset: 0x00144B20
	public static Vector2 GetAnchorOffset(tk2dBaseSprite.Anchor anchor, float width, float height)
	{
		Vector2 zero = Vector2.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero.x = (float)((int)(width / 2f));
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.x = (float)((int)width);
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.LowerRight:
			zero.y = (float)((int)height);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero.y = (float)((int)(height / 2f));
			break;
		}
		return zero;
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x001469E4 File Offset: 0x00144BE4
	public static void GetSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		numVertices = 4;
		numIndices = spriteDef.indices.Length;
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x001469F4 File Offset: 0x00144BF4
	public static void SetSpriteGeom(Vector3[] pos, Vector2[] uv, Vector3[] norm, Vector4[] tang, int offset, tk2dSpriteDefinition spriteDef, Vector3 scale)
	{
		pos[offset] = Vector3.Scale(spriteDef.position0, scale);
		pos[offset + 1] = Vector3.Scale(spriteDef.position1, scale);
		pos[offset + 2] = Vector3.Scale(spriteDef.position2, scale);
		pos[offset + 3] = Vector3.Scale(spriteDef.position3, scale);
		for (int i = 0; i < spriteDef.uvs.Length; i++)
		{
			uv[offset + i] = spriteDef.uvs[i];
		}
		if (norm != null && spriteDef.normals != null)
		{
			for (int j = 0; j < spriteDef.normals.Length; j++)
			{
				norm[offset + j] = spriteDef.normals[j];
			}
		}
		if (tang != null && spriteDef.tangents != null)
		{
			for (int k = 0; k < spriteDef.tangents.Length; k++)
			{
				tang[offset + k] = spriteDef.tangents[k];
			}
		}
	}

	// Token: 0x06004051 RID: 16465 RVA: 0x00146B48 File Offset: 0x00144D48
	public static void SetSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		for (int i = 0; i < spriteDef.indices.Length; i++)
		{
			indices[offset + i] = vStart + spriteDef.indices[i];
		}
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x00146B80 File Offset: 0x00144D80
	public static void GetClippedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		numVertices = 4;
		numIndices = 6;
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x00146B88 File Offset: 0x00144D88
	public static void SetClippedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 clipBottomLeft, Vector2 clipTopRight, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		Vector3 vector = spriteDef.untrimmedBoundsDataCenter - spriteDef.untrimmedBoundsDataExtents * 0.5f;
		Vector3 vector2 = spriteDef.untrimmedBoundsDataCenter + spriteDef.untrimmedBoundsDataExtents * 0.5f;
		float num = Mathf.Lerp(vector.x, vector2.x, clipBottomLeft.x);
		float num2 = Mathf.Lerp(vector.x, vector2.x, clipTopRight.x);
		float num3 = Mathf.Lerp(vector.y, vector2.y, clipBottomLeft.y);
		float num4 = Mathf.Lerp(vector.y, vector2.y, clipTopRight.y);
		Vector3 boundsDataExtents = spriteDef.boundsDataExtents;
		Vector3 vector3 = spriteDef.boundsDataCenter - boundsDataExtents * 0.5f;
		float num5 = (num - vector3.x) / boundsDataExtents.x;
		float num6 = (num2 - vector3.x) / boundsDataExtents.x;
		float num7 = (num3 - vector3.y) / boundsDataExtents.y;
		float num8 = (num4 - vector3.y) / boundsDataExtents.y;
		Vector2 vector4 = new Vector2(Mathf.Clamp01(num5), Mathf.Clamp01(num7));
		Vector2 vector5 = new Vector2(Mathf.Clamp01(num6), Mathf.Clamp01(num8));
		Vector3 position = spriteDef.position0;
		Vector3 position2 = spriteDef.position3;
		Vector3 vector6 = new Vector3(Mathf.Lerp(position.x, position2.x, vector4.x) * scale.x, Mathf.Lerp(position.y, position2.y, vector4.y) * scale.y, position.z * scale.z);
		Vector3 vector7 = new Vector3(Mathf.Lerp(position.x, position2.x, vector5.x) * scale.x, Mathf.Lerp(position.y, position2.y, vector5.y) * scale.y, position.z * scale.z);
		boundsCenter.Set(vector6.x + (vector7.x - vector6.x) * 0.5f, vector6.y + (vector7.y - vector6.y) * 0.5f, colliderOffsetZ);
		boundsExtents.Set((vector7.x - vector6.x) * 0.5f, (vector7.y - vector6.y) * 0.5f, colliderExtentZ);
		pos[offset] = new Vector3(vector6.x, vector6.y, vector6.z);
		pos[offset + 1] = new Vector3(vector7.x, vector6.y, vector6.z);
		pos[offset + 2] = new Vector3(vector6.x, vector7.y, vector6.z);
		pos[offset + 3] = new Vector3(vector7.x, vector7.y, vector6.z);
		if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
		{
			Vector2 vector8 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.x));
			Vector2 vector9 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
			uv[offset] = new Vector2(vector8.x, vector8.y);
			uv[offset + 1] = new Vector2(vector8.x, vector9.y);
			uv[offset + 2] = new Vector2(vector9.x, vector8.y);
			uv[offset + 3] = new Vector2(vector9.x, vector9.y);
		}
		else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
		{
			Vector2 vector10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.x));
			Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
			uv[offset] = new Vector2(vector10.x, vector10.y);
			uv[offset + 2] = new Vector2(vector11.x, vector10.y);
			uv[offset + 1] = new Vector2(vector10.x, vector11.y);
			uv[offset + 3] = new Vector2(vector11.x, vector11.y);
		}
		else
		{
			Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.y));
			Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.y));
			uv[offset] = new Vector2(vector12.x, vector12.y);
			uv[offset + 1] = new Vector2(vector13.x, vector12.y);
			uv[offset + 2] = new Vector2(vector12.x, vector13.y);
			uv[offset + 3] = new Vector2(vector13.x, vector13.y);
		}
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x001472E0 File Offset: 0x001454E0
	public static void SetClippedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		indices[offset] = vStart;
		indices[offset + 1] = vStart + 3;
		indices[offset + 2] = vStart + 1;
		indices[offset + 3] = vStart + 2;
		indices[offset + 4] = vStart + 3;
		indices[offset + 5] = vStart;
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x0014730C File Offset: 0x0014550C
	public static void GetSlicedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, bool borderOnly, bool tileStretchedSprite, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom)
	{
		if (tileStretchedSprite)
		{
			tk2dSpriteGeomGen.GetSlicedTiledSpriteGeomDesc(out numVertices, out numIndices, spriteDef, borderOnly, dimensions, borderBottomLeft, borderTopRight, borderCornerBottom);
			return;
		}
		numVertices = 16;
		numIndices = ((!borderOnly) ? 54 : 48);
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x0014733C File Offset: 0x0014553C
	public static void SetSlicedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, bool borderOnly, Vector3 scale, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ, Vector2 anchorOffset, bool tileStretchedSprite)
	{
		if (tileStretchedSprite)
		{
			tk2dSpriteGeomGen.SetSlicedTiledSpriteGeom(pos, uv, offset, out boundsCenter, out boundsExtents, spriteDef, borderOnly, scale, dimensions, borderBottomLeft, borderTopRight, borderCornerBottom, anchor, colliderOffsetZ, colliderExtentZ, anchorOffset);
			return;
		}
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		float x = spriteDef.texelSize.x;
		float y = spriteDef.texelSize.y;
		float num = spriteDef.position1.x - spriteDef.position0.x;
		float num2 = spriteDef.position2.y - spriteDef.position0.y;
		float num3 = borderTopRight.y * num2;
		float num4 = borderBottomLeft.y * num2;
		float num5 = borderTopRight.x * num;
		float num6 = borderBottomLeft.x * num;
		float num7 = dimensions.x * x;
		float num8 = dimensions.y * y;
		float num9 = 0f;
		float num10 = 0f;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			num9 = (float)(-(float)((int)(dimensions.x / 2f)));
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			num9 = (float)(-(float)((int)dimensions.x));
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			num10 = (float)(-(float)((int)(dimensions.y / 2f)));
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			num10 = (float)(-(float)((int)dimensions.y));
			break;
		}
		num9 -= anchorOffset.x;
		num10 -= anchorOffset.y;
		num9 *= x;
		num10 *= y;
		boundsCenter.Set(scale.x * (num7 * 0.5f + num9), scale.y * (num8 * 0.5f + num10), colliderOffsetZ);
		boundsExtents.Set(scale.x * (num7 * 0.5f), scale.y * (num8 * 0.5f), colliderExtentZ);
		Vector2[] uvs = spriteDef.uvs;
		Vector2 vector = uvs[1] - uvs[0];
		Vector2 vector2 = uvs[2] - uvs[0];
		Vector3 vector3 = new Vector3(num9, num10, 0f);
		Vector3[] array = new Vector3[]
		{
			vector3,
			vector3 + new Vector3(0f, num4, 0f),
			vector3 + new Vector3(0f, num8 - num3, 0f),
			vector3 + new Vector3(0f, num8, 0f)
		};
		Vector2[] array2 = new Vector2[]
		{
			uvs[0],
			uvs[0] + vector2 * borderBottomLeft.y,
			uvs[0] + vector2 * (1f - borderTopRight.y),
			uvs[0] + vector2
		};
		for (int i = 0; i < 4; i++)
		{
			pos[offset + i * 4] = array[i];
			pos[offset + i * 4 + 1] = array[i] + new Vector3(num6, 0f, 0f);
			pos[offset + i * 4 + 2] = array[i] + new Vector3(num7 - num5, 0f, 0f);
			pos[offset + i * 4 + 3] = array[i] + new Vector3(num7, 0f, 0f);
			for (int j = 0; j < 4; j++)
			{
				pos[offset + i * 4 + j] = Vector3.Scale(pos[offset + i * 4 + j], scale);
			}
			uv[offset + i * 4] = array2[i];
			uv[offset + i * 4 + 1] = array2[i] + vector * borderBottomLeft.x;
			uv[offset + i * 4 + 2] = array2[i] + vector * (1f - borderTopRight.x);
			uv[offset + i * 4 + 3] = array2[i] + vector;
		}
	}

	// Token: 0x06004057 RID: 16471 RVA: 0x001478B4 File Offset: 0x00145AB4
	public static void SetSlicedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, bool borderOnly, bool tileStretchedSprite, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom)
	{
		if (tileStretchedSprite)
		{
			tk2dSpriteGeomGen.SetSlicedTiledSpriteIndices(indices, offset, vStart, spriteDef, borderOnly, dimensions, borderBottomLeft, borderTopRight, borderCornerBottom);
			return;
		}
		int[] array = new int[]
		{
			0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
			5, 6, 2, 6, 3, 3, 6, 7, 4, 8,
			5, 5, 8, 9, 6, 10, 7, 7, 10, 11,
			8, 12, 9, 9, 12, 13, 9, 13, 10, 10,
			13, 14, 10, 14, 11, 11, 14, 15, 5, 9,
			6, 6, 9, 10
		};
		int num = array.Length;
		if (borderOnly)
		{
			num -= 6;
		}
		for (int i = 0; i < num; i++)
		{
			indices[offset + i] = vStart + array[i];
		}
	}

	// Token: 0x06004058 RID: 16472 RVA: 0x0014791C File Offset: 0x00145B1C
	public static void GetSlicedTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, bool borderOnly, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom)
	{
		float x = spriteDef.texelSize.x;
		float y = spriteDef.texelSize.y;
		float num = spriteDef.position1.x - spriteDef.position0.x;
		float num2 = spriteDef.position2.y - spriteDef.position0.y;
		float num3 = borderTopRight.y * num2;
		float num4 = borderBottomLeft.y * num2;
		float num5 = borderTopRight.x * num;
		float num6 = borderBottomLeft.x * num;
		float num7 = borderCornerBottom * num2;
		float num8 = dimensions.x * x;
		float num9 = dimensions.y * y;
		float num10 = num - num5 - num6;
		float num11 = num2 - num3 - num4 - num7;
		float num12 = (num8 - num5 - num6) / num10;
		float num13 = (num9 - num3 - num4) / num11;
		int num14 = Mathf.CeilToInt(num12);
		if (num6 > 0f)
		{
			num14++;
		}
		if (num5 > 0f)
		{
			num14++;
		}
		int num15 = Mathf.CeilToInt(num13);
		if (num3 > 0f)
		{
			num15++;
		}
		if (num4 > 0f)
		{
			num15++;
		}
		int num16 = num14 * num15;
		if (borderOnly)
		{
			num16 -= Mathf.CeilToInt(num12) * Mathf.CeilToInt(num13);
		}
		if (borderCornerBottom > 0f)
		{
			num16 += num14;
		}
		numVertices = num16 * 4;
		numIndices = num16 * 6;
	}

	// Token: 0x06004059 RID: 16473 RVA: 0x00147A84 File Offset: 0x00145C84
	public static void SetSlicedTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, bool borderOnly, Vector3 scale, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ, Vector2 anchorOffset)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		float x = spriteDef.texelSize.x;
		float y = spriteDef.texelSize.y;
		float num = spriteDef.position1.x - spriteDef.position0.x;
		float num2 = spriteDef.position2.y - spriteDef.position0.y;
		float num3 = borderTopRight.y * num2;
		float num4 = borderBottomLeft.y * num2;
		float num5 = borderTopRight.x * num;
		float num6 = borderBottomLeft.x * num;
		float num7 = borderCornerBottom * num2;
		float num8 = dimensions.x * x;
		float num9 = dimensions.y * y;
		float num10 = num - num5 - num6;
		float num11 = num2 - num3 - num4 - num7;
		int num12 = Mathf.CeilToInt((num8 - num5 - num6) / num10);
		int num13 = Mathf.CeilToInt((num9 - num3 - num4) / num11);
		float num14 = 0f;
		float num15 = 0f;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			num14 = (float)(-(float)((int)(dimensions.x / 2f)));
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			num14 = (float)(-(float)((int)dimensions.x));
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			num15 = (float)(-(float)((int)(dimensions.y / 2f)));
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			num15 = (float)(-(float)((int)dimensions.y));
			break;
		}
		num14 -= anchorOffset.x;
		num15 -= anchorOffset.y;
		num14 *= x;
		num15 *= y;
		boundsCenter.Set(scale.x * (num8 * 0.5f + num14), scale.y * (num9 * 0.5f + num15), colliderOffsetZ);
		boundsExtents.Set(scale.x * (num8 * 0.5f), scale.y * (num9 * 0.5f), colliderExtentZ);
		Vector2[] uvs = spriteDef.uvs;
		Vector2 vector = uvs[1] - uvs[0];
		Vector2 vector2 = uvs[2] - uvs[0];
		Vector3 vector3 = new Vector3(num14, num15, 0f);
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector2 zero3 = Vector2.zero;
		Vector2 zero4 = Vector2.zero;
		int i = 0;
		while (i < num12 + 2)
		{
			if (i == 0)
			{
				if (num6 != 0f)
				{
					zero.x = 0f;
					zero2.x = num6;
					zero3.x = uvs[0].x;
					zero4.x = uvs[0].x + vector.x * borderBottomLeft.x;
					goto IL_43D;
				}
			}
			else if (i == num12 + 1)
			{
				if (num5 != 0f)
				{
					zero.x = num8 - num5;
					zero2.x = num8;
					zero3.x = uvs[0].x + vector.x * (1f - borderTopRight.x);
					zero4.x = uvs[0].x + vector.x;
					goto IL_43D;
				}
			}
			else
			{
				zero.x = num6 + (float)(i - 1) * num10;
				zero2.x = Mathf.Min(num6 + (float)i * num10, num8 - num5);
				zero3.x = uvs[0].x + vector.x * borderBottomLeft.x;
				zero4.x = uvs[0].x + vector.x * (1f - borderTopRight.x);
				if (i == num12)
				{
					zero4.x = Mathf.Lerp(zero3.x, zero4.x, (zero2.x - zero.x) / num10);
					goto IL_43D;
				}
				goto IL_43D;
			}
			IL_922:
			i++;
			continue;
			IL_43D:
			if (borderCornerBottom > 0f)
			{
				zero.y = 0f;
				zero2.y = num7;
				zero3.y = uvs[0].y;
				zero4.y = (uvs[0] + vector2 * borderCornerBottom).y;
				pos[offset] = Vector3.Scale(vector3 + new Vector3(zero.x, -num7, 2f * num7), scale);
				pos[offset + 1] = Vector3.Scale(vector3 + new Vector3(zero2.x, -num7, 2f * num7), scale);
				pos[offset + 2] = Vector3.Scale(vector3 + new Vector3(zero.x, 0f, 0f), scale);
				pos[offset + 3] = Vector3.Scale(vector3 + new Vector3(zero2.x, 0f, 0f), scale);
				uv[offset] = new Vector2(zero3.x, zero3.y);
				uv[offset + 1] = new Vector2(zero4.x, zero3.y);
				uv[offset + 2] = new Vector2(zero3.x, zero4.y);
				uv[offset + 3] = new Vector2(zero4.x, zero4.y);
				offset += 4;
			}
			int j = 0;
			while (j < num13 + 2)
			{
				if (j == 0)
				{
					if (num4 != 0f)
					{
						zero.y = 0f;
						zero2.y = num4;
						zero3.y = (uvs[0] + vector2 * borderCornerBottom).y;
						zero4.y = (uvs[0] + vector2 * (borderBottomLeft.y + borderCornerBottom)).y;
						goto IL_7D0;
					}
				}
				else if (j == num13 + 1)
				{
					if (num3 != 0f)
					{
						zero.y = num9 - num3;
						zero2.y = num9;
						zero3.y = uvs[0].y + vector2.y * (1f - borderTopRight.y);
						zero4.y = uvs[0].y + vector2.y;
						goto IL_7D0;
					}
				}
				else if (!borderOnly || i == 0 || i == num12 + 1)
				{
					zero.y = num4 + (float)(j - 1) * num11;
					zero2.y = Mathf.Min(num4 + (float)j * num11, num9 - num3);
					zero3.y = uvs[0].y + vector2.y * (borderBottomLeft.y + borderCornerBottom);
					zero4.y = uvs[0].y + vector2.y * (1f - borderTopRight.y);
					if (j == num13)
					{
						zero4.y = Mathf.Lerp(zero3.y, zero4.y, (zero2.y - zero.y) / num11);
						goto IL_7D0;
					}
					goto IL_7D0;
				}
				IL_911:
				j++;
				continue;
				IL_7D0:
				pos[offset] = Vector3.Scale(vector3 + new Vector3(zero.x, zero.y), scale);
				pos[offset + 1] = Vector3.Scale(vector3 + new Vector3(zero2.x, zero.y), scale);
				pos[offset + 2] = Vector3.Scale(vector3 + new Vector3(zero.x, zero2.y), scale);
				pos[offset + 3] = Vector3.Scale(vector3 + new Vector3(zero2.x, zero2.y), scale);
				uv[offset] = new Vector2(zero3.x, zero3.y);
				uv[offset + 1] = new Vector2(zero4.x, zero3.y);
				uv[offset + 2] = new Vector2(zero3.x, zero4.y);
				uv[offset + 3] = new Vector2(zero4.x, zero4.y);
				offset += 4;
				goto IL_911;
			}
			goto IL_922;
		}
	}

	// Token: 0x0600405A RID: 16474 RVA: 0x001483C4 File Offset: 0x001465C4
	public static void SetSlicedTiledSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, bool borderOnly, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, float borderCornerBottom)
	{
		int num;
		int num2;
		tk2dSpriteGeomGen.GetSlicedTiledSpriteGeomDesc(out num, out num2, spriteDef, borderOnly, dimensions, borderBottomLeft, borderTopRight, borderCornerBottom);
		int num3 = 0;
		for (int i = 0; i < num2; i += 6)
		{
			indices[offset + i] = vStart + spriteDef.indices[0] + num3;
			indices[offset + i + 1] = vStart + spriteDef.indices[1] + num3;
			indices[offset + i + 2] = vStart + spriteDef.indices[2] + num3;
			indices[offset + i + 3] = vStart + spriteDef.indices[3] + num3;
			indices[offset + i + 4] = vStart + spriteDef.indices[4] + num3;
			indices[offset + i + 5] = vStart + spriteDef.indices[5] + num3;
			num3 += 4;
		}
	}

	// Token: 0x0600405B RID: 16475 RVA: 0x00148470 File Offset: 0x00146670
	public static void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsDataExtents.x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsDataExtents.y);
		numVertices = num * num2 * 4;
		numIndices = num * num2 * 6;
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x001484D8 File Offset: 0x001466D8
	public static void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsDataExtents.x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsDataExtents.y);
		Vector2 vector = new Vector2(dimensions.x * spriteDef.texelSize.x * scale.x, dimensions.y * spriteDef.texelSize.y * scale.y);
		Vector2 vector2 = Vector2.Scale(spriteDef.texelSize, scale) * 0.1f;
		Vector3 vector3 = Vector3.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector3.x = -(vector.x / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector3.x = -vector.x;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector3.y = -(vector.y / 2f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector3.y = -vector.y;
			break;
		}
		Vector3 vector4 = vector3;
		vector3 -= Vector3.Scale(spriteDef.position0, scale);
		boundsCenter.Set(vector.x * 0.5f + vector4.x, vector.y * 0.5f + vector4.y, colliderOffsetZ);
		boundsExtents.Set(vector.x * 0.5f, vector.y * 0.5f, colliderExtentZ);
		int num3 = 0;
		Vector3 vector5 = Vector3.Scale(spriteDef.untrimmedBoundsDataExtents, scale);
		Vector3 zero = Vector3.zero;
		Vector3 vector6 = zero;
		for (int i = 0; i < num2; i++)
		{
			vector6.x = zero.x;
			for (int j = 0; j < num; j++)
			{
				float num4 = 1f;
				float num5 = 1f;
				if (Mathf.Abs(vector6.x + vector5.x) > Mathf.Abs(vector.x) + vector2.x)
				{
					num4 = vector.x % vector5.x / vector5.x;
				}
				if (Mathf.Abs(vector6.y + vector5.y) > Mathf.Abs(vector.y) + vector2.y)
				{
					num5 = vector.y % vector5.y / vector5.y;
				}
				Vector3 vector7 = vector6 + vector3;
				if (num4 != 1f || num5 != 1f)
				{
					Vector2 zero2 = Vector2.zero;
					Vector2 vector8 = new Vector2(num4, num5);
					Vector3 vector9 = new Vector3(Mathf.Lerp(spriteDef.position0.x, spriteDef.position3.x, zero2.x) * scale.x, Mathf.Lerp(spriteDef.position0.y, spriteDef.position3.y, zero2.y) * scale.y, spriteDef.position0.z * scale.z);
					Vector3 vector10 = new Vector3(Mathf.Lerp(spriteDef.position0.x, spriteDef.position3.x, vector8.x) * scale.x, Mathf.Lerp(spriteDef.position0.y, spriteDef.position3.y, vector8.y) * scale.y, spriteDef.position0.z * scale.z);
					pos[offset + num3] = vector7 + new Vector3(vector9.x, vector9.y, vector9.z);
					pos[offset + num3 + 1] = vector7 + new Vector3(vector10.x, vector9.y, vector9.z);
					pos[offset + num3 + 2] = vector7 + new Vector3(vector9.x, vector10.y, vector9.z);
					pos[offset + num3 + 3] = vector7 + new Vector3(vector10.x, vector10.y, vector9.z);
					if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector8.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector8.x));
						uv[offset + num3] = new Vector2(vector11.x, vector11.y);
						uv[offset + num3 + 1] = new Vector2(vector11.x, vector12.y);
						uv[offset + num3 + 2] = new Vector2(vector12.x, vector11.y);
						uv[offset + num3 + 3] = new Vector2(vector12.x, vector12.y);
					}
					else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
					{
						Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector8.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector8.x));
						uv[offset + num3] = new Vector2(vector13.x, vector13.y);
						uv[offset + num3 + 2] = new Vector2(vector14.x, vector13.y);
						uv[offset + num3 + 1] = new Vector2(vector13.x, vector14.y);
						uv[offset + num3 + 3] = new Vector2(vector14.x, vector14.y);
					}
					else
					{
						Vector2 vector15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.y));
						Vector2 vector16 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector8.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector8.y));
						uv[offset + num3] = new Vector2(vector15.x, vector15.y);
						uv[offset + num3 + 1] = new Vector2(vector16.x, vector15.y);
						uv[offset + num3 + 2] = new Vector2(vector15.x, vector16.y);
						uv[offset + num3 + 3] = new Vector2(vector16.x, vector16.y);
					}
				}
				else
				{
					pos[offset + num3] = vector7 + Vector3.Scale(spriteDef.position0, scale);
					pos[offset + num3 + 1] = vector7 + Vector3.Scale(spriteDef.position1, scale);
					pos[offset + num3 + 2] = vector7 + Vector3.Scale(spriteDef.position2, scale);
					pos[offset + num3 + 3] = vector7 + Vector3.Scale(spriteDef.position3, scale);
					uv[offset + num3] = spriteDef.uvs[0];
					uv[offset + num3 + 1] = spriteDef.uvs[1];
					uv[offset + num3 + 2] = spriteDef.uvs[2];
					uv[offset + num3 + 3] = spriteDef.uvs[3];
				}
				num3 += 4;
				vector6.x += vector5.x;
			}
			vector6.y += vector5.y;
		}
	}

	// Token: 0x0600405D RID: 16477 RVA: 0x00148F48 File Offset: 0x00147148
	public static void SetTiledSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, Vector2 dimensions, tk2dTiledSprite.OverrideGetTiledSpriteGeomDescDelegate overrideGetTiledSpriteGeomDesc = null)
	{
		int num2;
		if (overrideGetTiledSpriteGeomDesc != null)
		{
			int num;
			overrideGetTiledSpriteGeomDesc(out num, out num2, spriteDef, dimensions);
		}
		else
		{
			int num;
			tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, spriteDef, dimensions);
		}
		int num3 = 0;
		for (int i = 0; i < num2; i += 6)
		{
			indices[offset + i] = vStart + spriteDef.indices[0] + num3;
			indices[offset + i + 1] = vStart + spriteDef.indices[1] + num3;
			indices[offset + i + 2] = vStart + spriteDef.indices[2] + num3;
			indices[offset + i + 3] = vStart + spriteDef.indices[3] + num3;
			indices[offset + i + 4] = vStart + spriteDef.indices[4] + num3;
			indices[offset + i + 5] = vStart + spriteDef.indices[5] + num3;
			num3 += 4;
		}
		for (int j = offset + num2; j < indices.Length; j++)
		{
			indices[j] = 0;
		}
	}

	// Token: 0x0600405E RID: 16478 RVA: 0x00149024 File Offset: 0x00147224
	public static void SetBoxMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, Vector3 origin, Vector3 extents, Matrix4x4 mat, Vector3 baseScale)
	{
		tk2dSpriteGeomGen.boxScaleMatrix.m03 = origin.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m13 = origin.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m23 = origin.z * baseScale.z;
		tk2dSpriteGeomGen.boxScaleMatrix.m00 = extents.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m11 = extents.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m22 = extents.z * baseScale.z;
		Matrix4x4 matrix4x = mat * tk2dSpriteGeomGen.boxScaleMatrix;
		for (int i = 0; i < 8; i++)
		{
			pos[posOffset + i] = matrix4x.MultiplyPoint(tk2dSpriteGeomGen.boxUnitVertices[i]);
		}
		float num = mat.m00 * mat.m11 * mat.m22 * baseScale.x * baseScale.y * baseScale.z;
		int[] array = ((num < 0f) ? tk2dSpriteGeomGen.boxIndicesBack : tk2dSpriteGeomGen.boxIndicesFwd);
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	// Token: 0x0600405F RID: 16479 RVA: 0x0014917C File Offset: 0x0014737C
	public static void SetSpriteDefinitionMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, tk2dSpriteDefinition spriteDef, Matrix4x4 mat, Vector3 baseScale)
	{
		for (int i = 0; i < spriteDef.colliderVertices.Length; i++)
		{
			Vector3 vector = Vector3.Scale(spriteDef.colliderVertices[i], baseScale);
			vector = mat.MultiplyPoint(vector);
			pos[posOffset + i] = vector;
		}
		int[] indices2 = spriteDef.indices;
		for (int j = 0; j < indices2.Length; j++)
		{
			indices[indicesOffset + j] = vStart + indices2[j];
		}
	}

	// Token: 0x06004060 RID: 16480 RVA: 0x001491FC File Offset: 0x001473FC
	public static void SetSpriteVertexNormals(Vector3[] pos, Vector3 pMin, Vector3 pMax, Vector3[] spriteDefNormals, Vector4[] spriteDefTangents, Vector3[] normals, Vector4[] tangents)
	{
		Vector3 vector = pMax - pMin;
		int num = pos.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = pos[i];
			float num2 = (vector2.x - pMin.x) / vector.x;
			float num3 = (vector2.y - pMin.y) / vector.y;
			float num4 = (1f - num2) * (1f - num3);
			float num5 = num2 * (1f - num3);
			float num6 = (1f - num2) * num3;
			float num7 = num2 * num3;
			if (spriteDefNormals != null && spriteDefNormals.Length == 4 && i < normals.Length)
			{
				normals[i] = spriteDefNormals[0] * num4 + spriteDefNormals[1] * num5 + spriteDefNormals[2] * num6 + spriteDefNormals[3] * num7;
			}
			if (spriteDefTangents != null && spriteDefTangents.Length == 4 && i < tangents.Length)
			{
				tangents[i] = spriteDefTangents[0] * num4 + spriteDefTangents[1] * num5 + spriteDefTangents[2] * num6 + spriteDefTangents[3] * num7;
			}
		}
	}

	// Token: 0x06004061 RID: 16481 RVA: 0x001493A4 File Offset: 0x001475A4
	public static void SetSpriteVertexNormalsFast(Vector3[] pos, Vector3[] normals, Vector4[] tangents)
	{
		int num = pos.Length;
		Vector3 back = Vector3.back;
		Vector4 vector = new Vector4(1f, 0f, 0f, 1f);
		for (int i = 0; i < num; i++)
		{
			normals[i] = back;
			tangents[i] = vector;
		}
	}

	// Token: 0x04003348 RID: 13128
	private static readonly int[] boxIndicesBack = new int[]
	{
		0, 1, 2, 2, 1, 3, 6, 5, 4, 7,
		5, 6, 3, 7, 6, 2, 3, 6, 4, 5,
		1, 4, 1, 0, 6, 4, 0, 6, 0, 2,
		1, 7, 3, 5, 7, 1
	};

	// Token: 0x04003349 RID: 13129
	private static readonly int[] boxIndicesFwd = new int[]
	{
		2, 1, 0, 3, 1, 2, 4, 5, 6, 6,
		5, 7, 6, 7, 3, 6, 3, 2, 1, 5,
		4, 0, 1, 4, 0, 4, 6, 2, 0, 6,
		3, 7, 1, 1, 7, 5
	};

	// Token: 0x0400334A RID: 13130
	private static readonly Vector3[] boxUnitVertices = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x0400334B RID: 13131
	private static Matrix4x4 boxScaleMatrix = Matrix4x4.identity;
}
