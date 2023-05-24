using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000392 RID: 914
internal class dfClippingUtil
{
	// Token: 0x0600100D RID: 4109 RVA: 0x0004A0AC File Offset: 0x000482AC
	public static void Clip(IList<Plane> planes, dfRenderData source, dfRenderData dest)
	{
		dest.EnsureCapacity(dest.Vertices.Count + source.Vertices.Count);
		int count = source.Triangles.Count;
		Vector3[] items = source.Vertices.Items;
		int[] items2 = source.Triangles.Items;
		Vector2[] items3 = source.UV.Items;
		Color32[] items4 = source.Colors.Items;
		Matrix4x4 transform = source.Transform;
		int count2 = planes.Count;
		for (int i = 0; i < count; i += 3)
		{
			for (int j = 0; j < 3; j++)
			{
				int num = items2[i + j];
				dfClippingUtil.clipSource[0].corner[j] = transform.MultiplyPoint(items[num]);
				dfClippingUtil.clipSource[0].uv[j] = items3[num];
				dfClippingUtil.clipSource[0].color[j] = items4[num];
			}
			int num2 = 1;
			for (int k = 0; k < count2; k++)
			{
				Plane plane = planes[k];
				num2 = dfClippingUtil.clipToPlane(ref plane, dfClippingUtil.clipSource, dfClippingUtil.clipDest, num2);
				dfClippingUtil.ClipTriangle[] array = dfClippingUtil.clipSource;
				dfClippingUtil.clipSource = dfClippingUtil.clipDest;
				dfClippingUtil.clipDest = array;
			}
			for (int l = 0; l < num2; l++)
			{
				dfClippingUtil.clipSource[l].CopyTo(dest);
			}
		}
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0004A258 File Offset: 0x00048458
	private static int clipToPlane(ref Plane plane, dfClippingUtil.ClipTriangle[] source, dfClippingUtil.ClipTriangle[] dest, int count)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num += dfClippingUtil.clipToPlane(ref plane, ref source[i], dest, num);
		}
		return num;
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0004A28C File Offset: 0x0004848C
	private static int inverseClipToPlane(ref Plane plane, ref dfClippingUtil.ClipTriangle triangle, dfClippingUtil.ClipTriangle[] dest, int destIndex)
	{
		Vector3[] corner = triangle.corner;
		int num = 0;
		int num2 = 0;
		Vector3 vector = plane.normal * -1f;
		float distance = plane.distance;
		for (int i = 0; i < 3; i++)
		{
			if (Vector3.Dot(vector, corner[i]) + distance > 0f)
			{
				dfClippingUtil.inside[num++] = i;
			}
			else
			{
				num2 = i;
			}
		}
		if (num == 3)
		{
			dfClippingUtil.ClipTriangle clipTriangle = dest[destIndex];
			Array.Copy(triangle.corner, 0, clipTriangle.corner, 0, 3);
			Array.Copy(triangle.uv, 0, clipTriangle.uv, 0, 3);
			Array.Copy(triangle.color, 0, clipTriangle.color, 0, 3);
			return 1;
		}
		if (num == 0)
		{
			return 0;
		}
		if (num == 1)
		{
			int num3 = dfClippingUtil.inside[0];
			int num4 = (num3 + 1) % 3;
			int num5 = (num3 + 2) % 3;
			Vector3 vector2 = corner[num3];
			Vector3 vector3 = corner[num4];
			Vector3 vector4 = corner[num5];
			Vector2 vector5 = triangle.uv[num3];
			Vector2 vector6 = triangle.uv[num4];
			Vector2 vector7 = triangle.uv[num5];
			Color32 color = triangle.color[num3];
			Color32 color2 = triangle.color[num4];
			Color32 color3 = triangle.color[num5];
			float num6 = 0f;
			Vector3 vector8 = vector3 - vector2;
			Ray ray = new Ray(vector2, vector8.normalized);
			plane.Raycast(ray, out num6);
			float num7 = num6 / vector8.magnitude;
			Vector3 point = ray.GetPoint(num6);
			Vector2 vector9 = Vector2.Lerp(vector5, vector6, num7);
			Color32 color4 = Color32.Lerp(color, color2, num7);
			vector8 = vector4 - vector2;
			ray = new Ray(vector2, vector8.normalized);
			plane.Raycast(ray, out num6);
			num7 = num6 / vector8.magnitude;
			Vector3 point2 = ray.GetPoint(num6);
			Vector2 vector10 = Vector2.Lerp(vector5, vector7, num7);
			Color32 color5 = Color32.Lerp(color, color3, num7);
			dfClippingUtil.ClipTriangle clipTriangle2 = dest[destIndex];
			clipTriangle2.corner[0] = vector2;
			clipTriangle2.corner[1] = point;
			clipTriangle2.corner[2] = point2;
			clipTriangle2.uv[0] = vector5;
			clipTriangle2.uv[1] = vector9;
			clipTriangle2.uv[2] = vector10;
			clipTriangle2.color[0] = color;
			clipTriangle2.color[1] = color4;
			clipTriangle2.color[2] = color5;
			return 1;
		}
		int num8 = num2;
		int num9 = (num8 + 1) % 3;
		int num10 = (num8 + 2) % 3;
		Vector3 vector11 = corner[num8];
		Vector3 vector12 = corner[num9];
		Vector3 vector13 = corner[num10];
		Vector2 vector14 = triangle.uv[num8];
		Vector2 vector15 = triangle.uv[num9];
		Vector2 vector16 = triangle.uv[num10];
		Color32 color6 = triangle.color[num8];
		Color32 color7 = triangle.color[num9];
		Color32 color8 = triangle.color[num10];
		Vector3 vector17 = vector12 - vector11;
		Ray ray2 = new Ray(vector11, vector17.normalized);
		float num11 = 0f;
		plane.Raycast(ray2, out num11);
		float num12 = num11 / vector17.magnitude;
		Vector3 point3 = ray2.GetPoint(num11);
		Vector2 vector18 = Vector2.Lerp(vector14, vector15, num12);
		Color32 color9 = Color32.Lerp(color6, color7, num12);
		vector17 = vector13 - vector11;
		ray2 = new Ray(vector11, vector17.normalized);
		plane.Raycast(ray2, out num11);
		num12 = num11 / vector17.magnitude;
		Vector3 point4 = ray2.GetPoint(num11);
		Vector2 vector19 = Vector2.Lerp(vector14, vector16, num12);
		Color32 color10 = Color32.Lerp(color6, color8, num12);
		dfClippingUtil.ClipTriangle clipTriangle3 = dest[destIndex];
		clipTriangle3.corner[0] = point3;
		clipTriangle3.corner[1] = vector12;
		clipTriangle3.corner[2] = point4;
		clipTriangle3.uv[0] = vector18;
		clipTriangle3.uv[1] = vector15;
		clipTriangle3.uv[2] = vector19;
		clipTriangle3.color[0] = color9;
		clipTriangle3.color[1] = color7;
		clipTriangle3.color[2] = color10;
		clipTriangle3 = dest[++destIndex];
		clipTriangle3.corner[0] = point4;
		clipTriangle3.corner[1] = vector12;
		clipTriangle3.corner[2] = vector13;
		clipTriangle3.uv[0] = vector19;
		clipTriangle3.uv[1] = vector15;
		clipTriangle3.uv[2] = vector16;
		clipTriangle3.color[0] = color10;
		clipTriangle3.color[1] = color7;
		clipTriangle3.color[2] = color8;
		return 2;
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0004A89C File Offset: 0x00048A9C
	private static int clipToPlane(ref Plane plane, ref dfClippingUtil.ClipTriangle triangle, dfClippingUtil.ClipTriangle[] dest, int destIndex)
	{
		Vector3[] corner = triangle.corner;
		int num = 0;
		int num2 = 0;
		Vector3 normal = plane.normal;
		float distance = plane.distance;
		for (int i = 0; i < 3; i++)
		{
			if (Vector3.Dot(normal, corner[i]) + distance > 0f)
			{
				dfClippingUtil.inside[num++] = i;
			}
			else
			{
				num2 = i;
			}
		}
		if (num == 3)
		{
			dfClippingUtil.ClipTriangle clipTriangle = dest[destIndex];
			Array.Copy(triangle.corner, 0, clipTriangle.corner, 0, 3);
			Array.Copy(triangle.uv, 0, clipTriangle.uv, 0, 3);
			Array.Copy(triangle.color, 0, clipTriangle.color, 0, 3);
			return 1;
		}
		if (num == 0)
		{
			return 0;
		}
		if (num == 1)
		{
			int num3 = dfClippingUtil.inside[0];
			int num4 = (num3 + 1) % 3;
			int num5 = (num3 + 2) % 3;
			Vector3 vector = corner[num3];
			Vector3 vector2 = corner[num4];
			Vector3 vector3 = corner[num5];
			Vector2 vector4 = triangle.uv[num3];
			Vector2 vector5 = triangle.uv[num4];
			Vector2 vector6 = triangle.uv[num5];
			Color32 color = triangle.color[num3];
			Color32 color2 = triangle.color[num4];
			Color32 color3 = triangle.color[num5];
			float num6 = 0f;
			Vector3 vector7 = vector2 - vector;
			Ray ray = new Ray(vector, vector7.normalized);
			plane.Raycast(ray, out num6);
			float num7 = num6 / vector7.magnitude;
			Vector3 point = ray.GetPoint(num6);
			Vector2 vector8 = Vector2.Lerp(vector4, vector5, num7);
			Color32 color4 = Color32.Lerp(color, color2, num7);
			vector7 = vector3 - vector;
			ray = new Ray(vector, vector7.normalized);
			plane.Raycast(ray, out num6);
			num7 = num6 / vector7.magnitude;
			Vector3 point2 = ray.GetPoint(num6);
			Vector2 vector9 = Vector2.Lerp(vector4, vector6, num7);
			Color32 color5 = Color32.Lerp(color, color3, num7);
			dfClippingUtil.ClipTriangle clipTriangle2 = dest[destIndex];
			clipTriangle2.corner[0] = vector;
			clipTriangle2.corner[1] = point;
			clipTriangle2.corner[2] = point2;
			clipTriangle2.uv[0] = vector4;
			clipTriangle2.uv[1] = vector8;
			clipTriangle2.uv[2] = vector9;
			clipTriangle2.color[0] = color;
			clipTriangle2.color[1] = color4;
			clipTriangle2.color[2] = color5;
			return 1;
		}
		int num8 = num2;
		int num9 = (num8 + 1) % 3;
		int num10 = (num8 + 2) % 3;
		Vector3 vector10 = corner[num8];
		Vector3 vector11 = corner[num9];
		Vector3 vector12 = corner[num10];
		Vector2 vector13 = triangle.uv[num8];
		Vector2 vector14 = triangle.uv[num9];
		Vector2 vector15 = triangle.uv[num10];
		Color32 color6 = triangle.color[num8];
		Color32 color7 = triangle.color[num9];
		Color32 color8 = triangle.color[num10];
		Vector3 vector16 = vector11 - vector10;
		Ray ray2 = new Ray(vector10, vector16.normalized);
		float num11 = 0f;
		plane.Raycast(ray2, out num11);
		float num12 = num11 / vector16.magnitude;
		Vector3 point3 = ray2.GetPoint(num11);
		Vector2 vector17 = Vector2.Lerp(vector13, vector14, num12);
		Color32 color9 = Color32.Lerp(color6, color7, num12);
		vector16 = vector12 - vector10;
		ray2 = new Ray(vector10, vector16.normalized);
		plane.Raycast(ray2, out num11);
		num12 = num11 / vector16.magnitude;
		Vector3 point4 = ray2.GetPoint(num11);
		Vector2 vector18 = Vector2.Lerp(vector13, vector15, num12);
		Color32 color10 = Color32.Lerp(color6, color8, num12);
		dfClippingUtil.ClipTriangle clipTriangle3 = dest[destIndex];
		clipTriangle3.corner[0] = point3;
		clipTriangle3.corner[1] = vector11;
		clipTriangle3.corner[2] = point4;
		clipTriangle3.uv[0] = vector17;
		clipTriangle3.uv[1] = vector14;
		clipTriangle3.uv[2] = vector18;
		clipTriangle3.color[0] = color9;
		clipTriangle3.color[1] = color7;
		clipTriangle3.color[2] = color10;
		clipTriangle3 = dest[++destIndex];
		clipTriangle3.corner[0] = point4;
		clipTriangle3.corner[1] = vector11;
		clipTriangle3.corner[2] = vector12;
		clipTriangle3.uv[0] = vector18;
		clipTriangle3.uv[1] = vector14;
		clipTriangle3.uv[2] = vector15;
		clipTriangle3.color[0] = color10;
		clipTriangle3.color[1] = color7;
		clipTriangle3.color[2] = color8;
		return 2;
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0004AEA4 File Offset: 0x000490A4
	private static dfClippingUtil.ClipTriangle[] initClipBuffer(int size)
	{
		dfClippingUtil.ClipTriangle[] array = new dfClippingUtil.ClipTriangle[size];
		for (int i = 0; i < size; i++)
		{
			array[i].corner = new Vector3[3];
			array[i].uv = new Vector2[3];
			array[i].color = new Color32[3];
		}
		return array;
	}

	// Token: 0x04000F12 RID: 3858
	private static int[] inside = new int[3];

	// Token: 0x04000F13 RID: 3859
	private static dfClippingUtil.ClipTriangle[] clipSource = dfClippingUtil.initClipBuffer(1024);

	// Token: 0x04000F14 RID: 3860
	private static dfClippingUtil.ClipTriangle[] clipDest = dfClippingUtil.initClipBuffer(1024);

	// Token: 0x02000393 RID: 915
	protected struct ClipTriangle
	{
		// Token: 0x06001012 RID: 4114 RVA: 0x0004AF04 File Offset: 0x00049104
		public void CopyTo(ref dfClippingUtil.ClipTriangle target)
		{
			Array.Copy(this.corner, 0, target.corner, 0, 3);
			Array.Copy(this.uv, 0, target.uv, 0, 3);
			Array.Copy(this.color, 0, target.color, 0, 3);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0004AF44 File Offset: 0x00049144
		public void CopyTo(dfRenderData buffer)
		{
			int count = buffer.Vertices.Count;
			buffer.Vertices.AddRange(this.corner);
			buffer.UV.AddRange(this.uv);
			buffer.Colors.AddRange(this.color);
			buffer.Triangles.Add(count, count + 1, count + 2);
		}

		// Token: 0x04000F15 RID: 3861
		public Vector3[] corner;

		// Token: 0x04000F16 RID: 3862
		public Vector2[] uv;

		// Token: 0x04000F17 RID: 3863
		public Color32[] color;
	}
}
